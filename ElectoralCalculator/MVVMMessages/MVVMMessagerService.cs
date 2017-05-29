using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.MVVMMessages
{
    public class MVVMMessagerService
    {
        private static Dictionary<Type, object> _registeredReceivers;

        private MVVMMessagerService() { }

        public static void Init()
        {
            _registeredReceivers = new Dictionary<Type, object>();
        }

        public static void RegisterReceiver<T>(Action<T> action) 
            where T : MessageBase
        {
            if (_registeredReceivers == null)
                _registeredReceivers = new Dictionary<Type, object>();
            if (_registeredReceivers.Any(p => p.Key == typeof(T)))
                throw new Exception("Receiver of this type already exist!");
            _registeredReceivers.Add(typeof(T), action);
        }

        public static void SendMessage<T>(T message) 
            where T : MessageBase
        {
            if (!ReceiverExist<T>())
                throw new Exception("You can't send message to unregistered type!");

            foreach (KeyValuePair<Type, object> receiver in _registeredReceivers.Where(p => p.Key == typeof(T)))
            {
                var action = receiver.Value;
                var methodInfo = action.GetType().GetMethod("Invoke");

                if (methodInfo.GetParameters().Count() == 1)
                    methodInfo.Invoke(action, new[] { message });
                else
                    throw new Exception("Bad MVVM message");
            }
        }

        public static bool ReceiverExist<T>() where T : MessageBase
            => _registeredReceivers.Any(p => p.Key == typeof(T));

        public static void UnregisterReceiver<T>() where T : MessageBase
        {
            if (!_registeredReceivers.Any(p => p.Key == typeof(T)))
                throw new Exception("Can't unregister `" + typeof(T)+ "` because it doesnt exist.");
            _registeredReceivers.Remove(typeof(T));
        }
    }
}