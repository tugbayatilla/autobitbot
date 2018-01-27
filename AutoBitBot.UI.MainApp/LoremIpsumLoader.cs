using AutoBitBot.UI.App.Content;
using AutoBitBot.UI.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.UI.App
{
    /// <summary>
    /// Loads lorem ipsum content regardless the given uri.
    /// </summary>
    public class LoremIpsumLoader
        : DefaultContentLoader
    {
        /// <summary>
        /// Loads the content from specified uri.
        /// </summary>
        /// <param name="uri">The content uri</param>
        /// <returns>The loaded content.</returns>
        protected override object LoadContent(Uri uri)
        {
            // return a new LoremIpsum user control instance no matter the uri
            return new LoremIpsum();
        }
    }
}
