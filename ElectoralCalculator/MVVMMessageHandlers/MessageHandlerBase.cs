using ElectoralCalculator.MVVMMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.MVVMMessageHandlers
{
    public abstract class MessageHandlerBase<T> : IMVVMMessageHandler<T> where T : MessageBase
    {
        public abstract void Handle(T obj);

        public MessageHandlerBase()
        {
            if (MVVMMessagerService.ReceiverExist<T>())
                MVVMMessagerService.UnregisterReceiver<T>();
        }
    }
}
