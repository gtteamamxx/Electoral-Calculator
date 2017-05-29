using ElectoralCalculator.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ElectoralCalculator.ViewModels
{
    public interface IStartWindowViewModel
    {
        RelayCommand FrameLoadedCommand { get; }

        void Frame_Loaded();
    }
}
