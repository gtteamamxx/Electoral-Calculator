using ElectoralCalculator.MVVMMessages;
using ElectoralCalculator.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElectoralCalculator.ViewModels
{
    public class ValidateDatabaseViewModel
    {
        public ValidateDatabaseViewModel()
        {
            Task.Run(() => ValidateDatabaseAsync());
        }

        private async Task ValidateDatabaseAsync()
        {
            await new TaskFactory().StartNew(() =>
            {
                using (var context = new ElectoralDatabase())
                {
                    if (!context.Database.Exists())
                    {
                        MessageBox.Show("Database dont exist! Build project with new App.config", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());
                    }
                }
            });

            ShowLoginPage();
        }

        private void ShowLoginPage()
        {
            Application.Current.Dispatcher.Invoke(() => 
                MVVMMessagerService.SendMessage(new ChangeFrameSourceMessage(new VoterLoginPage())));
        }
    }
}
