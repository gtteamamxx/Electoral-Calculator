using ElectoralCalculator.MVVMMessageHandlers;
using ElectoralCalculator.MVVMMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Navigation;

namespace ElectoralCalculator.Behaviors
{
    public class ChangeFrameSourceBehavior : Behavior<Frame>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            ChangeFrameSourceHandler.SetFrameInstance(AssociatedObject);
            AssociatedObject.Navigated += AssociatedObject_Navigated;
        }

        private void AssociatedObject_Navigated(object sender, NavigationEventArgs e)
        {
            AssociatedObject.NavigationService.RemoveBackEntry();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Navigated -= AssociatedObject_Navigated;
        }
    }
}
