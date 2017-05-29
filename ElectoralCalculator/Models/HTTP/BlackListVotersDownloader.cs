using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectoralCalculator.Models.DTO;
using System.Net;
using Newtonsoft.Json;

namespace ElectoralCalculator.Models.HTTP
{
    public class BlackListVotersDownloader : IBlackListDownloader
    {
        private static readonly string BLOCKED_PERSONS_URL = "url removed";

        public async Task<List<PersonDto>> GetDisallowedPersonsAsync()
        {
            try
            {
                using (var webClient = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    var jsonString = await webClient.DownloadStringTaskAsync(BLOCKED_PERSONS_URL);
                    var blackListRoot = (await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<BlackListRoot>(jsonString)));
                    return blackListRoot.Disallowed.Persons;
                }
            }
            catch
            {
                return null; //
            }
        }
    }
}
