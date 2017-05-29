using ElectoralCalculator.MVVMMessages;
using ElectoralCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElectoralCalculator.MVVMMessageHandlers
{
    public class RefreshStatsRequestHandler : MessageHandlerBase<RefreshStatsMessage>
    {
        private static StatsViewModel _statsPageViewModel;

        public RefreshStatsRequestHandler() : base()
        {
            MVVMMessagerService.RegisterReceiver<RefreshStatsMessage>(Handle);
        }

        public static void SetStatsViewModel(StatsViewModel viewModel)
            => _statsPageViewModel = viewModel;

        public override void Handle(RefreshStatsMessage obj)
        {
            _statsPageViewModel.Refresh_ClickAsync();
        }
    }
}
