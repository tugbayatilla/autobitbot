using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoBitBot.UI.Presentation
{
    /// <summary>
    /// Represents a displayable link.
    /// </summary>
    public class Link
        : DisplayableFrameworkElement
    {
        public static readonly DependencyProperty CountProperty = DependencyProperty.Register("Count", typeof(Int32), typeof(Link), new PropertyMetadata(0));
        private Uri source;

        public Link()
        {
            //SetCurrentValue(CountProperty, 0);
        }

        /// <summary>
        /// Gets or sets the source uri.
        /// </summary>
        /// <value>The source.</value>
        public Uri Source
        {
            get { return this.source; }
            set
            {
                if (this.source != value)
                {
                    this.source = value;
                    FirePropertyChanged(nameof(Source));
                }
            }
        }


        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public Int32 Count
        {
            get => (Int32)GetValue(CountProperty);
            set => SetValue(CountProperty, value);
        }

    }
}
