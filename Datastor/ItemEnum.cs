using LibraryItems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datastor
{
     class ItemEnum : IEnumerator
    {
        public List<Items> _items;
        int position = -1;
        public ItemEnum(List<Items>list)
        {
            _items = list;
        }
        public bool MoveNext()
        {
            position++;
            return (position < _items.Count);
        }
        public void Reset()
        {
            position = -1;
        }
 
        object IEnumerator.Current { get => Current; }
        private Items Current
        {
            get
            {
                try { return _items[position]; }
                catch (IndexOutOfRangeException) { throw new InvalidOperationException(); }
            }
        }
    }
}
