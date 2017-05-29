using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.Models.Database
{
    public class CandidateStatsNonDb
    {
        public string Name { get; set; }
        public int PartyId { get; set; }
        public int VotesNum { get; set; }
    }
}
