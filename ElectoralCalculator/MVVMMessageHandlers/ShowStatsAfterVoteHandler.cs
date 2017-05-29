using ElectoralCalculator.MVVMMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ElectoralCalculator.MVVMMessageHandlers
{
    public class ShowStatsAfterVoteHandler : MessageHandlerBase<ShowStatsPageMessage>
    {
        private static TabControl _tabControl;

        public ShowStatsAfterVoteHandler() : base()
        {
            MVVMMessagerService.RegisterReceiver<ShowStatsPageMessage>(Handle);
        }

        public static void SetTabControl(TabControl tabControl)
            => _tabControl = tabControl;

        public async override void Handle(ShowStatsPageMessage obj)
        {
            await Task.Delay(500);
            _tabControl.SelectedItem = _tabControl.Items.Cast<TabItem>().First(p => p.Name == "StatsTab");
        }
    }
}
