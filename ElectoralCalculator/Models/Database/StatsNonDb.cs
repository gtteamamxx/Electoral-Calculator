using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.Models.Database
{
    public class StatsNonDb
    {
        public List<CandidateStatsNonDb> Candidates { get; set; }
        public List<PartyStatsNonDb> Parties { get; set; }
        public int ValidVotesNum { get; set; }
        public int UnvalidVotesNun { get; set; }
        public int WithoutLawTries { get; set; }

        public StatsNonDb()
        {
            Candidates = new List<CandidateStatsNonDb>();
            Parties = new List<PartyStatsNonDb>();
        }
    }
}
