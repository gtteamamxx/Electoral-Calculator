using ElectoralCalculator.Models.Database;
using ElectoralCalculator.Models.DTO;
using ElectoralCalculator.MVVMMessages;
using ElectoralCalculator.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElectoralCalculator.Models.Application
{
    public class UserService : IUserService
    {
        public static readonly int IDLE_TIME = 15;

        private UserService() { }

        private static UserService _instance;
        public static UserService Instance =>
            _instance ?? (_instance = new UserService());

        public PersonDto LoggedPerson { get; set; }
        public string SessionKey { get; private set; }
        public int SessionTimestamp { get; set; }

        private bool _stopSending = false;

        public void InitSession()
        {
            SessionKey = Guid.NewGuid().ToString();
        }

        public void Logout()
        {
            StopSendingStatus();
            LoggedPerson = null;
            MVVMMessagerService.SendMessage(new ChangeFrameSourceMessage(new VoterLoginPage()));
            GC.Collect();
        }

        public async void StartSendingStatus()
        {
            await Task.Run(async () =>
            {
                while(true)
                {
                    if (_stopSending)
                    {
                        _stopSending = false;
                        return;
                    }

                    var currentTimestamp = await DatabaseManager.Instance.GetCurrentTimestamp();
                    if (currentTimestamp == null
                    || !await DatabaseManager.Instance.UpdateSessionAsync
                            (this.LoggedPerson.Pesel, this.SessionKey, currentTimestamp ?? default(int)))
                    {
                        MessageBoxHelper.Show("Error occurred durning sending update status.", MessageBoxHelper.MessageType.Error);
                        UserService.Instance.Logout();
                    }

                    await Task.Delay(TimeSpan.FromSeconds(IDLE_TIME - 1));
                }
            });
        }

        public void StopSendingStatus()
            => _stopSending = true;
    }
}
