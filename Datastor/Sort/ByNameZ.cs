using LibraryItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datastor.Sort
{
    public class ByNameZ : IComparer<Items>
    {
        public int Compare(Items x, Items y) => y.Title.CompareTo(x.Title);
        public override string ToString() => "Z - A";
    
    }
}
