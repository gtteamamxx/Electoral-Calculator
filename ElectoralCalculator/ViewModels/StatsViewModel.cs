using ElectoralCalculator.Models.Application;
using ElectoralCalculator.Models.Database;
using ElectoralCalculator.Models.Save;
using ElectoralCalculator.Models.Stats;
using ElectoralCalculator.MVVMMessageHandlers;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Controls;

namespace ElectoralCalculator.ViewModels
{
    public class StatsViewModel : BaseViewModel, IStatsViewModel
    {
        #region Commands
        private RelayCommand _savePDFCommand;
        public RelayCommand SavePDFCommand =>
            _savePDFCommand ?? (_savePDFCommand = new RelayCommand(SavePDF_Click));

        private RelayCommand _saveCSVCommand;
        public RelayCommand SaveCSVCommand =>
            _saveCSVCommand ?? (_saveCSVCommand = new RelayCommand(SaveCSV_Click));

        private RelayCommand _refreshCommand;
        public RelayCommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new RelayCommand(Refresh_ClickAsync));

        #endregion

        #region Properties
        private bool _isGeneratingStatsProgressRingActive;
        public bool IsGeneratingStatsProgressRingActive
        {
            get => _isGeneratingStatsProgressRingActive;
            set => OnPropertyChanged(ref _isGeneratingStatsProgressRingActive, value);
        }

        private UIElement _statsContent;
        public UIElement StatsContent
        {
            get => _statsContent;
            set => OnPropertyChanged(ref _statsContent, value);
        }
        #endregion

        public StatsViewModel()
        {
            Refresh_ClickAsync();
            RefreshStatsRequestHandler.SetStatsViewModel(this);
        }

        public async void Refresh_ClickAsync()
        {
            if (IsGeneratingStatsProgressRingActive)
                return;

            IsGeneratingStatsProgressRingActive = true;
            StatsContent = null;

            if (UserService.Instance.LoggedPerson.Voted)
                StatsContent = await new StatsViewGenerator().GenerateStatsViewAsync();
            else
                StatsContent = new UserControl()
                {
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Content = new TextBlock()
                    {
                        Text = "You have to vote before viewing stats!",
                        FontSize = 20
                    }
                };

            if (StatsContent == null)
                MessageBoxHelper.Show("Error durning refreshing page", MessageBoxHelper.MessageType.Error);

            IsGeneratingStatsProgressRingActive = false;
        }

        public void SavePDF_Click()
        {
            SaveStats(new PDFSaverMethod());
        }

        public void SaveCSV_Click()
        {
            SaveStats(new CSVSaverMethod());
        }

        public void SaveStats(ISaveMethod method)
        {
            if (StatsContent == null)
            {
                MessageBox.Show("Before exporting to file, you have to refresh stats.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var stats = (StatsNonDb)((UserControl)StatsContent).DataContext;
            new ExportManager().SaveFile(method, stats);
        }
    }
}
