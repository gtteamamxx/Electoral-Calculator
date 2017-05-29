using ElectoralCalculator.Models.Application;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ElectoralCalculator.ViewModels
{
    public class VoteViewModel : BaseViewModel, IVoteViewModel
    {
        #region Properties
        private string _header;
        public string Header
        {
            get => _header;
            set => OnPropertyChanged(ref _header, value);
        }

        private TabItem _selectedTab;
        public TabItem SelectedTab
        {
            get => _selectedTab;
            set
            {
                if ((string)value.Header == "Logout")
                {
                    Logout();
                    return;
                }
                OnPropertyChanged(ref _selectedTab, value);
            }
        }
        #endregion

        public VoteViewModel()
        {
            Header = $"User: {UserService.Instance.LoggedPerson.Name} {UserService.Instance.LoggedPerson.Surname}";
        }

        public void Logout()
        {
            UserService.Instance.Logout();
        }
    }
}
