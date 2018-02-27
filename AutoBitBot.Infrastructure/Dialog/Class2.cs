using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Dialog
{
    public class DialogService : IDialogService
    {
        public bool OpenFileDialog(bool checkFileExists, string Filter, out string FileName)
        {
            FileName = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            //openFileDialog.Filter = "All Image Files | *.jpg;*.png | All files | *.*";
            openFileDialog.Filter = Filter;
            openFileDialog.CheckFileExists = checkFileExists;
            bool result = ((bool)openFileDialog.ShowDialog());
            if (result)
            {
                FileName = openFileDialog.FileName;
            }
            return result;
        }



        public MessageBoxResult ShowMessageBox(string message, string caption, MessageBoxButton buttons, MessageBoxIcon icon)
        {
            return (MessageBoxResult)System.Windows.MessageBox.Show(message, caption,
                (System.Windows.MessageBoxButton)buttons,
                (System.Windows.MessageBoxImage)icon);
        }
    }
}
