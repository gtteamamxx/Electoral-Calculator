using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace ElectoralCalculator.Behaviors
{
    public class DisableBackSpaceKeyBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyDown += AssociatedObject_KeyDown;
        }

        private void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Back || e.Key == Key.BrowserBack)
            {
                e.Handled = true;
                return;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
        }
    }
}
