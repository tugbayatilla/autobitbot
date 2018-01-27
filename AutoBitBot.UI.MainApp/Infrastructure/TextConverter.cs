using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.UI.MainApp.Infrastructure
{
    public class TextConverter
    {
        private readonly Func<string, string> _convertion;

        public TextConverter(Func<string, string> convertion)
        {
            _convertion = convertion;
        }

        public string ConvertText(string inputText)
        {
            return _convertion(inputText);
        }
    }
}
