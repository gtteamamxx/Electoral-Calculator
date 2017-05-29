using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.Models.DTO
{
    [JsonObject("person")]
    public class PersonDto
    {
        [JsonProperty("pesel")]
        public string Pesel { get; set; }

        [JsonIgnore]
        public string Name { get; set; }

        [JsonIgnore]
        public string Surname { get; set; }

        [JsonIgnore]
        public bool Voted { get; set; }
    }
}
