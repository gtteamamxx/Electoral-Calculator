using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElectoralCalculator.Models.Application
{
    public static class MessageBoxHelper
    {
        public enum MessageType
        {
            Error,
            Question,
            Information
        }

        public static void Show(string info, MessageType type)
        {
            string title = type == MessageType.Error ?
                "Error" : type == MessageType.Question
                ? "Question" : "Information";

            var icon = type == MessageType.Error ?
                (dynamic)MessageBoxImage.Error : type == MessageType.Question
                ? MessageBoxImage.Question : MessageBoxImage.Information;

            MessageBox.Show(info, title, MessageBoxButton.OK, icon);
        }
    }
}
