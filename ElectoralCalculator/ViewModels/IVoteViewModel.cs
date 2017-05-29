using System.Windows.Controls;

namespace ElectoralCalculator.ViewModels
{
    public interface IVoteViewModel
    {
        string Header { get; set; }
        TabItem SelectedTab { get; set; }

        void Logout();
    }
}