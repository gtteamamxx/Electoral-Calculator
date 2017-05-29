using ElectoralCalculator.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.Models.HTTP
{
    public interface IBlackListDownloader
    {
        Task<List<PersonDto>> GetDisallowedPersonsAsync();
    }
}
