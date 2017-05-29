using ElectoralCalculator.Models.Application;
using ElectoralCalculator.Models.Database;
using ElectoralCalculator.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.Models.Vote
{
    public class UserFullNameDatabaseValidator
    {
        public async Task<bool> ValidateUserFromDatabase(PersonDto person)
        {
            var personInDb = await Task.Run(
                () => DatabaseManager.Instance.GetPerson(SHA512Encrypter.SHA512(person.Pesel)));
            if (personInDb == null)
                return true;

            person.Voted = personInDb.Voted;

            return person.Name.ToLower().Trim() == personInDb.Name.ToLower().Trim()
                         && person.Surname.ToLower().Trim() == personInDb.Surname.ToLower().Trim();

        }
    }
}
