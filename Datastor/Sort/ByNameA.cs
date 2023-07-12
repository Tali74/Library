using LibraryItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datastor.Sort
{
    public class ByNameA : IComparer<Items>
    {
        public int Compare(Items x, Items y) => x.Title.CompareTo(y.Title);
        public override string ToString() => "A - Z";
    }
}
