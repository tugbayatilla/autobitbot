using AutoBitBot.Infrastructure.Dialog;
using AutoBitBot.UI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoBitBot.UI.MainApp.Infrastructure
{
    public class ModernDialogService
    {

        public static Boolean ShowDialog(Object content, String title, MessageBoxButton buttons, MessageBoxIcon icon)
        {
            var dlg = new ModernDialog
            {
                Title = title,
                Content = content
            };

            var buttonList = new List<Button>();

            if (buttons == (MessageBoxButton.OK))
            {
                buttonList.Add(dlg.OkButton);
            }
            else if (buttons == (MessageBoxButton.OKCancel))
            {
                buttonList.Add(dlg.OkButton);
                buttonList.Add(dlg.CancelButton);
            }
            else if (buttons == (MessageBoxButton.YesNo))
            {
                buttonList.Add(dlg.YesButton);
                buttonList.Add(dlg.NoButton);
            }
            else if (buttons == (MessageBoxButton.YesNoCancel))
            {
                buttonList.Add(dlg.YesButton);
                buttonList.Add(dlg.NoButton);
                buttonList.Add(dlg.CancelButton);
            }

            dlg.Buttons = buttonList.ToArray();

            return dlg.ShowDialog() ?? false;
        }

        public static Boolean ConfirmDialog(Object content, String title)
        {
            return ShowDialog(content, title, MessageBoxButton.YesNo, MessageBoxIcon.Question);
        }
        public static Boolean InfoDialog(Object content, String title)
        {
            return ShowDialog(content, title, MessageBoxButton.OK, MessageBoxIcon.Question);
        }
        public static Boolean ErrorDialog(Object content, String title)
        {
            return ShowDialog(content, title, MessageBoxButton.OK, MessageBoxIcon.Error);
        }
        public static Boolean WarningDialog(Object content, String title)
        {
            return ShowDialog(content, title, MessageBoxButton.OK, MessageBoxIcon.Warning);
        }
    }
}
