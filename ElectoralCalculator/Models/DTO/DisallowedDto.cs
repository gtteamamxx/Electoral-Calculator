using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.Models.DTO
{
    [JsonObject("disallowed")]
    public class DisallowedDto
    {
        [JsonProperty("person")]
        public List<PersonDto> Persons { get; set; }
    }
}
