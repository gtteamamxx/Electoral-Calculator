using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.Models.DTO
{
    [JsonObject("Candidate")]
    public class CandidateDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("party")]
        public string Party { get; set; }

        public override string ToString()
        {
            return this.Name;
        }

        [JsonIgnore]
        public bool IsChecked { get; set; }
    }
}
