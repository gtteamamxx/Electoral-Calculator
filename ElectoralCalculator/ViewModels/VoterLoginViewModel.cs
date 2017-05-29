using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using ElectoralCalculator.Models.DTO;
using ElectoralCalculator.Models.Vote;
using System.Runtime.CompilerServices;
using ElectoralCalculator.Views;
using ElectoralCalculator.MVVMMessages;
using ElectoralCalculator.Models.Database;
using ElectoralCalculator.Models.Application;
using ElectoralCalculator.MVVMMessageHandlers;

namespace ElectoralCalculator.ViewModels
{
    public class VoterLoginViewModel : BaseViewModel, IVoterLoginViewModel
    {
        public VoterLoginViewModel()
        {
            LoginToVoteAppHandler.SetViewModel(this);
        }

        #region Commands

        private RelayCommand _loginCommand;
        public RelayCommand LoginCommand =>
            _loginCommand ?? (_loginCommand = new RelayCommand(Login_ClickAsync));

        #endregion

        #region Properties
        private string _name;
        public string Name
        {
            get => _name;
            set => OnPropertyChanged(ref _name, value);
        }

        private string _surname;
        public string Surname
        {
            get => _surname;
            set => OnPropertyChanged(ref _surname, value);
        }

        private string _pesel;
        public string Pesel
        {
            get => _pesel;
            set => OnPropertyChanged(ref _pesel, value);
        }

        private bool _isLoggingProgressRingActive;
        public bool IsLoggingProgressRingActive
        {
            get => _isLoggingProgressRingActive;
            set => OnPropertyChanged(ref _isLoggingProgressRingActive, value);
        }
        #endregion

        public async void Login_ClickAsync()
            => await ValidateUserAsync();

        public async Task ValidateUserAsync()
        {
            if (IsLoggingProgressRingActive)
                return;

            IsLoggingProgressRingActive = true;

            try
            {
                if (await UserCanLoginAsync())
                {
                    AddUserToSerivceAsync();
                    ShowVotePage();
                    UserService.Instance.StartSendingStatus();
                }
            }
            catch
            {
                MessageBox.Show("Error occured. Try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            IsLoggingProgressRingActive = false;
        }

        public void AddUserToSerivceAsync()
        {
            var loggedPerson = new PersonDto()
            {
                Name = Name,
                Surname = Surname,
                Pesel = SHA512Encrypter.SHA512(Pesel),
            };
            UserService.Instance.LoggedPerson = loggedPerson;
        }

        private void ShowVotePage()
            => MVVMMessagerService.SendMessage(new ChangeFrameSourceMessage(new VotePage()));

        public async Task<bool> UserCanLoginAsync()
        {
            var userToValidate = new PersonDto
            {
                Name = Name,
                Surname = Surname,
                Pesel = Pesel ?? string.Empty
            };

            if (!await UserCanVote(userToValidate))
                return false;

            if (!await ValidatePersonAsync(userToValidate))
                return false;

            if (!ValidatePeselData(userToValidate))
                return false;

            return true;
        }

        public bool ValidatePeselData(PersonDto userToValidate)
        {
            if (!new PeselValidator().ValidatePesel(userToValidate.Pesel, out var dateTime))
            {
                MessageBox.Show("Entered PESEL is not valid!", "Info", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            {
                var year = DateTime.UtcNow.Subtract(dateTime).TotalDays / 365;
                if (year < 18)
                {
                    MessageBox.Show("You are under 18! You can't vote!", "Info", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> ValidatePersonAsync(PersonDto userToValidate)
        {
            if (!new UserFullnameValidator().ValidateFullName(Name, Surname))
            {
                MessageBox.Show("You typed bad first name/surname!", "Info", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!await new UserFullNameDatabaseValidator().ValidateUserFromDatabase(userToValidate))
            {
                MessageBox.Show("Somebody was used this PESEL with different Name/Surname!", "Info", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if(await new UserAlreadyLoggedValidator().UserAlreadyLogged(
                SHA512Encrypter.SHA512(userToValidate.Pesel), UserService.Instance.SessionKey) == true)
            {
                MessageBox.Show("This person is already logged! Try again later.", "Info", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public async Task<bool> UserCanVote(PersonDto userToValidate)
        {
            if (!await new BlackListUserValidator().ValidateUserAsync(userToValidate))
            {
                MessageBox.Show("You aren't able to vote!", "Info", MessageBoxButton.OK, MessageBoxImage.Error);
                await DatabaseManager.Instance.IncrementPersonWithLawTriedToVote();
                return false;
            }
            return true;
        }

        protected override void OnPropertyChanged<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            if (propertyName != nameof(IsLoggingProgressRingActive) && IsLoggingProgressRingActive)
                return;

            base.OnPropertyChanged(ref property, value, propertyName);
        }
    }
}
