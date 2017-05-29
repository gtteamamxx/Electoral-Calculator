using ElectoralCalculator.Models.Application;
using ElectoralCalculator.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.Models.Vote
{
    public class UserAlreadyLoggedValidator
    {
        public async Task<bool?> UserAlreadyLogged(string pesel, string guid)
        {
            var session = await DatabaseManager.Instance.GetSession(pesel);
            if (session == null)
                return false;

            var currentTimestamp = await DatabaseManager.Instance.GetCurrentTimestamp();
            if (currentTimestamp == null)
                return null;

            if (session.guid == guid)
                return false;
            if (session.timestamp + UserService.IDLE_TIME < currentTimestamp)
                return false;

            return true;
        }
    }
}
