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
    public class CandidatesDownloader : ICandidatesDownloader
    {
        private static readonly string CANDIDATES_URL = "url.. removed";

        public async Task<List<CandidateDto>> GetCandidatesAsync()
        {
            try
            {
                using (var webClient = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    var jsonString = await webClient.DownloadStringTaskAsync(CANDIDATES_URL);
                    var blackListRoot = (await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<CandidatesRoot>(jsonString)));
                    return blackListRoot.Candidates.Candidates;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
