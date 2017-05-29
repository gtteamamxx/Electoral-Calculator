using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.MVVMMessageHandlers
{
    public interface IMVVMMessageHandler<T>
    {
        void Handle(T obj);
    }
}
