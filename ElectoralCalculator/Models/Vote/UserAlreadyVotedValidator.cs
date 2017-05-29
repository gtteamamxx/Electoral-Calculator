using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectoralCalculator.Models.DTO;
using ElectoralCalculator.Models.Database;

namespace ElectoralCalculator.Models.Vote
{
    public class UserAlreadyVotedValidator
    {
        public async Task<bool> UserAlreadyVotedAsync(PersonDto person)
        {
            var personInDb = await Task.Run(() => DatabaseManager.Instance.GetPerson(person.Pesel));
            return personInDb?.Voted ?? false;
        }
    }
}
