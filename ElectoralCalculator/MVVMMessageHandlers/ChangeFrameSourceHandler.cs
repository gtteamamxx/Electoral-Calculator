using ElectoralCalculator.MVVMMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ElectoralCalculator.MVVMMessageHandlers
{
    public class ChangeFrameSourceHandler : MessageHandlerBase<ChangeFrameSourceMessage>
    {
        private static Frame _frameToChange;

        public ChangeFrameSourceHandler() : base()
        {
            MVVMMessagerService.RegisterReceiver<ChangeFrameSourceMessage>(Handle);
        }

        public static void SetFrameInstance(Frame frame)
            => _frameToChange = frame;

        public override void Handle(ChangeFrameSourceMessage obj)
            => _frameToChange.NavigationService.Navigate(obj.PageToChange);
    }
}
