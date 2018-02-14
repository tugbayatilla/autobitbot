using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace AutoBitBot.Infrastructure
{
    public static class SingleFieldDataGridFilterMediator
    {
        public static void Filter<T>(String filter, DataGrid dataGrid, Func<T, String> searchPattern) where T : class
        {
            if (filter.Length < 3 && filter.Length != 0)
            {
                return;
            }
            if (searchPattern == null)
            {
                return;
            }

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);
            if (collectionView == null)
            {
                return;
            }

            if (filter == "")
            {
                collectionView.Filter = null;
            }
            else
            {
                collectionView.Filter = o =>
                {
                    var p = o as T;
                    if (p == null)
                    {
                        return true;
                    }

                    return searchPattern.Invoke(p).ToUpperInvariant().Contains(filter.ToUpperInvariant());
                };
            }
        }

    }
}
