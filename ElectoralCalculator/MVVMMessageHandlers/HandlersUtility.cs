using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.MVVMMessageHandlers
{
    public class HandlersUtility
    {
        public static void RegisterHandlers(ISet<object> list)
        {
            var typeInfo = typeof(IMVVMMessageHandler<>).GetTypeInfo();
            var assembly = typeInfo.Assembly;

            var types = assembly.GetTypes().Where(p => p.Namespace == typeInfo.Namespace);

            foreach (var type in types)
            {
                if (type.Name == typeInfo.Name || type == typeof(HandlersUtility) 
                    || !type.Name.ToLower().Contains("handler")
                    || type.Name.Contains("Base"))
                    continue;
                list.Add(Activator.CreateInstance(type));
            }
        }
    }
}
