using ElectoralCalculator.Models.DTO;
using System.Threading.Tasks;

namespace ElectoralCalculator.Models.Database
{
    public interface IDatabaseManager
    {
        Person GetPerson(string pesel);
        Task<bool> CreatePersonAsync(Person person);

        Candidate GetCandidate(string name);
        Candidate GetCandidate(int id);

        Task<bool> IncrementVotesNumAsync(bool isVoteGood);
        Task<bool> IncrementPersonWithLawTriedToVote();

        Task<StatsNonDb> GetStatsAsync();

        Task<int?> GetCurrentTimestamp();
        Task<bool> UpdateSessionAsync(string pesel, string key, int timestamp);
        Task<LoginService> GetSession(string pesel);
    }
}