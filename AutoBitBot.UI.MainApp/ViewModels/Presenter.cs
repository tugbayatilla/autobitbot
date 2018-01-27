using AutoBitBot.UI.MainApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoBitBot.UI.MainApp.ViewModels
{
    public class Presenter
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate{ };
        private readonly TextConverter _textConverter
            = new TextConverter(s => s.ToUpper());
        private string _someText;
        private readonly ObservableCollection<string> _history
            = new ObservableCollection<string>();

        public string SomeText
        {
            get { return _someText; }
            set
            {
                _someText = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SomeText)));
            }
        }

        public IEnumerable<string> History
        {
            get { return _history; }
        }

        public ICommand ConvertTextCommand
        {
            get { return new DelegateCommand(ConvertText); }
        }

        private void ConvertText()
        {
            if (string.IsNullOrWhiteSpace(SomeText)) return;
            AddToHistory(_textConverter.ConvertText(SomeText));
            SomeText = string.Empty;
        }

        private void AddToHistory(string item)
        {
            if (!_history.Contains(item))
                _history.Add(item);
        }
    }
}
