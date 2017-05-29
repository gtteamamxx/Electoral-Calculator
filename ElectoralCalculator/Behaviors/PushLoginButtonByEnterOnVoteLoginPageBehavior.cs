using ElectoralCalculator.MVVMMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace ElectoralCalculator.Behaviors
{
    public class PushLoginButtonByEnterOnVoteLoginPageBehavior : Behavior<Page>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyDown += AssociatedObject_KeyDown;
        }

        private void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                MVVMMessagerService.SendMessage(new LoginToVoteAppMessage());
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
        }
    }
}
