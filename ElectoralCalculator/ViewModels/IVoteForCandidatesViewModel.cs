using ElectoralCalculator.Models.DTO;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.ViewModels
{
    public interface IVoteForCandidatesViewModel
    {
        RelayCommand VoteCommand { get; }

        ObservableCollection<ObservableCollection<CandidateDto>> CandidatesGroupList { get; set; }
        bool IsVoteButtonEnabled { get; set; }
        bool IsProgressRingActive { get; set; }
        string Header { get; set; }

        void Vote_ClickAsync();

        void InitializeView();

        void LoadCandidates();
        Candidate GetValidCandidate();
        void MarkSelectedCandidate(Candidate candidate);
        void AddCandidatesToView(List<CandidateDto> candidates);

        void SetCheckBoxEnabledIfPersonVoted();
        Task SetVoteButtonEnableIfPersonCanVoteAsync();

        Task<bool> TryToUpdatePersonAsync(CandidateDto candidateDto);
        Task<bool> TryToUpdateVotesAsync(bool isVoteGood);
    }
}
