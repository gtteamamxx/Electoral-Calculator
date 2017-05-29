using ElectoralCalculator.Models.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectoralCalculator.Models.DTO;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using ElectoralCalculator.Models.Vote;
using ElectoralCalculator.Models.Application;
using ElectoralCalculator.Models.Database;
using ElectoralCalculator.MVVMMessages;

namespace ElectoralCalculator.ViewModels
{
    public class VoteForCandidatesViewModel : BaseViewModel, IVoteForCandidatesViewModel
    {
        #region Commands
        private RelayCommand _voteCommand;
        public RelayCommand VoteCommand =>
            _voteCommand ?? (_voteCommand = new RelayCommand(Vote_ClickAsync));
        #endregion

        #region Properties
        private ObservableCollection<ObservableCollection<CandidateDto>> _candidatesGroupList;
        public ObservableCollection<ObservableCollection<CandidateDto>> CandidatesGroupList
        {
            get => _candidatesGroupList;
            set => OnPropertyChanged(ref _candidatesGroupList, value);
        }

        private bool _isVoteButtonEnabled;
        public bool IsVoteButtonEnabled
        {
            get => _isVoteButtonEnabled;
            set => OnPropertyChanged(ref _isVoteButtonEnabled, value);
        }

        private bool _isProgressRingActive;
        public bool IsProgressRingActive
        {
            get => _isProgressRingActive;
            set => OnPropertyChanged(ref _isProgressRingActive, value);
        }

        private string _header;
        public string Header
        {
            get => _header;
            set => OnPropertyChanged(ref _header, value);
        }

        #endregion

        public VoteForCandidatesViewModel()
        {
            CandidatesGroupList = new ObservableCollection<ObservableCollection<CandidateDto>>();
            InitializeView();
        }

        private void UpdateHeaderText()
            => Header = !IsVoteButtonEnabled ? "You already voted." : "Vote for candidate";

        public void InitializeView()
        {
            Task.Run(() => SetVoteButtonEnableIfPersonCanVoteAsync()).Wait();
            UpdateHeaderText();
            LoadCandidates();
            SetCheckBoxEnabledIfPersonVoted();
        }

        public void SetCheckBoxEnabledIfPersonVoted()
        {
            if (IsVoteButtonEnabled)
                return;
            var candidate = GetValidCandidate();
            if (candidate == null)
                return;
            MarkSelectedCandidate(candidate);
        }

        public Candidate GetValidCandidate()
        {
            var person = DatabaseManager.Instance.GetPerson(UserService.Instance.LoggedPerson.Pesel);
            return person.CandidateId == null ? null : DatabaseManager.Instance.GetCandidate(person.CandidateId ?? default(int));
        }

        public void MarkSelectedCandidate(Candidate cand)
        {
            var candidateToMarkAsSelected = CandidatesGroupList
                .SelectMany(p => p)
                .FirstOrDefault(p => p.Name == cand.Name);

            if (candidateToMarkAsSelected != null)
                candidateToMarkAsSelected.IsChecked = true;
        }

        public async Task SetVoteButtonEnableIfPersonCanVoteAsync()
        {
            var loggedPerson = UserService.Instance.LoggedPerson;
            bool isUserAlreadyVoted = await new UserAlreadyVotedValidator().UserAlreadyVotedAsync(loggedPerson);
            IsVoteButtonEnabled = !isUserAlreadyVoted;
        }

        public void LoadCandidates()
        {
            var candidatesDto = Task.Run(() => new CandidatesDownloader().GetCandidatesAsync()).Result;
            CheckCandidates(candidatesDto);
            AddCandidatesToView(candidatesDto);
        }

        private void CheckCandidates(List<CandidateDto> candidates)
        {
            if (candidates == null)
            {
                MessageBox.Show("Unable to download candidates list! Aborting", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());
            }
        }

        public void AddCandidatesToView(List<CandidateDto> candidatesDto)
        {
            var candidatesGrouped = candidatesDto.ToLookup(p => p.Party).ToList();
            for (int i = 0; i < 4; i++)
            {
                var observableCandidates = new ObservableCollection<CandidateDto>();
                CandidatesGroupList.Add(observableCandidates);

                candidatesGrouped[i].ToList().ForEach(p => observableCandidates.Add(p));
            }
        }

        public async void Vote_ClickAsync()
        {
            if (IsProgressRingActive)
                return;
            IsProgressRingActive = true;

            var result = AskPersonToVote();
            if(result == MessageBoxResult.No)
            {
                IsProgressRingActive = false;
                return;
            }

            if (!await CheckUserAlreadyLoggedAsync())
                return;

            if (await UserAlreadyVotedAsync())
                MessageBox.Show("You already voted!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (!await new BlackListUserValidator().ValidateUserAsync(UserService.Instance.LoggedPerson))
            {
                MessageBox.Show("You are not able to vote! Logging off..", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                UserService.Instance.Logout();
                return;
            }
            else
            {
                if (await DoVoteAsync())
                {
                    UpdateUIAfterVote();
                    ShowStatsPage();
                }
                else
                    MessageBoxHelper.Show("Error durning voting. Try again", MessageBoxHelper.MessageType.Error);
            }

            IsProgressRingActive = false;
        }

        private async Task<bool> CheckUserAlreadyLoggedAsync()
        {
            var userAlreadyLogged = await UserAlreadyLoggedAsync();

            if (userAlreadyLogged == null)
            {
                MessageBox.Show("Can't renew credentialas! Logging off...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                UserService.Instance.Logout();
                return false;
            }
            else if (userAlreadyLogged == true)
            {
                MessageBox.Show("You are already logged in!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                UserService.Instance.Logout();
                return false;
            }

            return true;
        }

        private async Task<bool?> UserAlreadyLoggedAsync()
        {
            return await new UserAlreadyLoggedValidator().UserAlreadyLogged(UserService.Instance.LoggedPerson.Pesel,
                UserService.Instance.SessionKey);
        }

        private async Task<bool> UserAlreadyVotedAsync()
            => await new UserAlreadyVotedValidator().UserAlreadyVotedAsync(UserService.Instance.LoggedPerson);

        private void ShowStatsPage()
        {
            MVVMMessagerService.SendMessage(new RefreshStatsMessage());
            MVVMMessagerService.SendMessage(new ShowStatsPageMessage());
        }

        private async Task<bool> DoVoteAsync()
        {
            try
            {
                var voteCalculator = new VoteCalculator();
                CandidateDto candidateDto = null;

                bool isVoteGood = voteCalculator.IsVoteGood(CandidatesGroupList);
                if (isVoteGood)
                    candidateDto = voteCalculator.GetValidCandidate(CandidatesGroupList);

                return await TryToUpdatePersonAsync(candidateDto) &&
                    await TryToUpdateVotesAsync(isVoteGood);
            }
            catch
            {
                return false;
            }
        }

        private void UpdateUIAfterVote()
        {
            IsVoteButtonEnabled = false;
            UpdateHeaderText();
        }

        public async Task<bool> TryToUpdateVotesAsync(bool isVoteGood)
        {
            try
            {
                return await DatabaseManager.Instance.IncrementVotesNumAsync(isVoteGood);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> TryToUpdatePersonAsync(CandidateDto candidateDto)
        {
            var person = DatabaseManager.Instance.GetPerson(UserService.Instance.LoggedPerson.Pesel);
            var candidate = candidateDto == null ? null : DatabaseManager.Instance.GetCandidate(candidateDto.Name);

            var personAdded = await DatabaseManager.Instance.CreatePersonAsync(new Person()
            {
                Name = UserService.Instance.LoggedPerson.Name,
                Surname = UserService.Instance.LoggedPerson.Surname,
                Pesel = UserService.Instance.LoggedPerson.Pesel,
                Voted = true,
                CandidateId = candidate?.Id
            });

            if (personAdded)
                UserService.Instance.LoggedPerson.Voted = true;
            return personAdded;
        }

        private MessageBoxResult AskPersonToVote()
            => MessageBox.Show("Are you sure you want to vote?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
    }
}
