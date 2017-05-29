using ElectoralCalculator.Models.DTO;

namespace ElectoralCalculator.Models.Application
{
    public interface IUserService
    {
        PersonDto LoggedPerson { get; set; }
        string SessionKey { get; }
        int SessionTimestamp { get; set; }
        void Logout();
        void InitSession();

        void StartSendingStatus();
        void StopSendingStatus();
    }
}