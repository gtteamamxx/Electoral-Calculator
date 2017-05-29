using ElectoralCalculator.Models.Application;
using ElectoralCalculator.Models.DTO;
using ElectoralCalculator.MVVMMessageHandlers;
using ElectoralCalculator.MVVMMessages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace ElectoralCalculator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static ISet<object> _handlers;
        public App()
        {
            MVVMMessagerService.Init();
            _handlers = new HashSet<object>();
            HandlersUtility.RegisterHandlers(_handlers);
            InitSession();
        }

        private void InitSession()
        {
            UserService.Instance.InitSession();
        }
    }
}
