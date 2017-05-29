using ElectoralCalculator.MVVMMessageHandlers;
using ElectoralCalculator.MVVMMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace ElectoralCalculator.Behaviors
{
    public class ShowStatsWhenVotedBehavior : Behavior<TabControl>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            ShowStatsAfterVoteHandler.SetTabControl(AssociatedObject);
        }
    }
}
