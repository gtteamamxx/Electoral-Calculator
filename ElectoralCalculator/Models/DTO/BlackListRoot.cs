using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.Models.DTO
{
    public class BlackListRoot
    {
        [JsonProperty("disallowed")]
        public DisallowedDto Disallowed { get; set; }
    }
}
