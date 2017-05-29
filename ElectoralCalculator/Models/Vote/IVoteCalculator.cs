using ElectoralCalculator.Models.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.Models.Vote
{
    public interface IVoteCalculator
    {
        CandidateDto GetValidCandidate(ObservableCollection<ObservableCollection<CandidateDto>> selectedCandidates);
        bool IsVoteGood(ObservableCollection<ObservableCollection<CandidateDto>> selectedCandidates);
    }
}
