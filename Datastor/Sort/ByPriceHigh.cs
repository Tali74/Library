using LibraryItems;
using System.Collections.Generic;



namespace Datastor.Sort
{
    public class ByPriceHigh :IComparer<Items>
    {
        public int Compare(Items x, Items y) => y.Price.CompareTo(x.Price);
        public override string ToString() => "Price high - low";
    }
}
