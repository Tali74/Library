using LibraryItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datastor.Sort
{
    internal class ByPriceLow : IComparer<Items>
    {
        public int Compare(Items x, Items y) => x.Price.CompareTo(y.Price);
        public override string ToString() => "Price low - high";
    }
}
