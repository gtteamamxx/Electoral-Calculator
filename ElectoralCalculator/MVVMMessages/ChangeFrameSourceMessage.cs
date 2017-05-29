using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ElectoralCalculator.MVVMMessages
{
    public class ChangeFrameSourceMessage : MessageBase
    {
        public Page PageToChange;

        public ChangeFrameSourceMessage(Page pageToChange)
        {
            base.FirstObject = this.PageToChange = pageToChange;
        }
    }
}
