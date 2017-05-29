using ElectoralCalculator.Models.Application;
using ElectoralCalculator.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.Models.Vote
{
    public class BlackListUserValidator
    {
        public async Task<bool> ValidateUserAsync(PersonDto person)
        {
            var downloader = new HTTP.BlackListVotersDownloader();
            var persons = await downloader.GetDisallowedPersonsAsync();

            if (persons == null)
                return false;

            var personPesel = SHA512Encrypter.SHA512(person.Pesel);
            return persons.Any(p => SHA512Encrypter.SHA512(p.Pesel) == personPesel) ? false : true;
        }
    }
}
