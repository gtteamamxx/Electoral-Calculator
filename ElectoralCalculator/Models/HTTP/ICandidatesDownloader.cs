using ElectoralCalculator.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectoralCalculator.Models.HTTP
{
    public interface ICandidatesDownloader
    {
        Task<List<CandidateDto>> GetCandidatesAsync();
    }
}