using ElectoralCalculator.Models.Save;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElectoralCalculator.ViewModels
{
    public interface IStatsViewModel
    {
        RelayCommand SavePDFCommand { get; }
        RelayCommand SaveCSVCommand { get; }
        RelayCommand RefreshCommand { get; }

        bool IsGeneratingStatsProgressRingActive { get; set; }
        UIElement StatsContent { get; set; }

        void Refresh_ClickAsync();
        void SaveCSV_Click();
        void SavePDF_Click();
        void SaveStats(ISaveMethod method);
    }
}
