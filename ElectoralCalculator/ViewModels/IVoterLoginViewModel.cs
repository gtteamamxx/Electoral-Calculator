using ElectoralCalculator.Models.DTO;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.ViewModels
{
    public interface IVoterLoginViewModel
    {
        RelayCommand LoginCommand { get; }

        string Name { get; set; }
        string Surname { get; set; }
        string Pesel { get; set; }
        bool IsLoggingProgressRingActive { get; set; }

        void Login_ClickAsync();

        Task ValidateUserAsync();
        bool ValidatePeselData(PersonDto userToValidate);
        Task<bool> ValidatePersonAsync(PersonDto userToValidate);
        Task<bool> UserCanVote(PersonDto userToValidate);

        void AddUserToSerivceAsync();
        Task<bool> UserCanLoginAsync();
    }
}
