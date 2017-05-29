using GalaSoft.MvvmLight.Command;
using ElectoralCalculator.MVVMMessages;
using ElectoralCalculator.Views;

namespace ElectoralCalculator.ViewModels
{
    public class StartWindowViewModel : BaseViewModel, IStartWindowViewModel
    {
        #region Commands
        private RelayCommand _frameLoadedCommand;
        public RelayCommand FrameLoadedCommand =>
            _frameLoadedCommand ?? (_frameLoadedCommand = new RelayCommand(Frame_Loaded));

        #endregion

        public void Frame_Loaded()
        {
            ValidateDatabase();
        }

        private void ValidateDatabase()
        {
            MVVMMessagerService.SendMessage(new ChangeFrameSourceMessage(new ValidateDatabasePage()));
        }
    }
}
