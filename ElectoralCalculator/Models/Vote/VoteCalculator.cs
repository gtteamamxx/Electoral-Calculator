using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectoralCalculator.Models.DTO;

namespace ElectoralCalculator.Models.Vote
{
    public class VoteCalculator : IVoteCalculator
    {
        private bool ValidateVote(IEnumerable<CandidateDto> selectedCandidates)
            => selectedCandidates.Count() == 1;

        public CandidateDto GetValidCandidate(ObservableCollection<ObservableCollection<CandidateDto>> selectedCandidates)
            => selectedCandidates.SelectMany(p => p).FirstOrDefault(p => p.IsChecked);

        public bool IsVoteGood(ObservableCollection<ObservableCollection<CandidateDto>> selectedCandidates)
        {
            var listWithCandidates = selectedCandidates.SelectMany(p => p)
                                                       .Where(p => p.IsChecked);
            return ValidateVote(listWithCandidates);
        }
    }
}

