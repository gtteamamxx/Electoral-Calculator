using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.Models.DTO
{
    [JsonObject("Candidates")]
    public class CandidatesDto
    {
        [JsonProperty("candidate")]
        public List<CandidateDto> Candidates { get; set; }
    }
}
