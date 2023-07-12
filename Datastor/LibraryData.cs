using LibraryItems;
using System;
using System.Collections.Generic;
using System.Linq;
using AllPeople;
using System.Collections;

namespace Datastor
{
    public class LibraryData : IData<Items>, ILonable<Items>, IEnumerable//התנהגות ומעשים עם המידע
    {
        readonly DbService _dbService = DbService.Instance;//יצירת שמירת מידע

        public List<Items> this[List<Items> currentList, string title]//חיפור רשימה של עצמים לפי שם
        {
            get
            {
                List<Items> tempList = currentList.FindAll(i => i.Title.ToLower().Contains(title.ToLower()));
                if (tempList.Count == 0)
                    throw new SearchExp($"Library doesn't contain:\n\'{title}\'");
                return tempList;
            }
        }
        public Items this[Guid id]//חיפוש לפי גייד
        {
            get
            {
                Items li = _dbService.LibraryItems.Find(item => item.Id == id);
                if (li is null)
                    throw new SearchExp("No item found by GUID");
                return li;
            }
        }
        public Book this[ISBN isbn]//חיפוש לפי ISBN
        {
            get
            {
                List<Items> tempBooks = _dbService.LibraryItems.FindAll((i) => i is Book);
                foreach (Book book in tempBooks)
                    if (book.ISBN.ToString() == isbn.ToString())
                        return book;
                throw new SearchExp($"No book found by this isbn - {isbn}");
            }
        }
        public static void Swap(ref Items old, Items @new)//החלפת עצמים חדשים ישנים
        {
            if (@new != null)
            {
                old = @new;
                ManagerP.EndSaleEvenHandler += old.OnEndSale;
                ManagerP.SetSaleEvenHandler += old.OnSetSale;
            }
        }
        public List<Items> FindType(Type type)//יש 2 סגים- ספרים או ג'ורנלים
        {
            if (type == null || type.Equals(typeof(Items)))
                return _dbService.LibraryItems;
            else if (type.Equals(typeof(Book)))
                return _dbService.LibraryItems.FindAll((i) => i is Book);
            else if (type.Equals(typeof(Journal)))
                return _dbService.LibraryItems.FindAll((i) => i is Journal);
            return _dbService.LibraryItems;
        }
        public Items Get(Guid id) => _dbService.LibraryItems.FirstOrDefault(i => i.Id == id);
        public Items Add(Items item)//יצירת עצם חדש
        {
            if (item == null)
                throw new NullReferenceException("Null input isn't valid");

            ManagerP.SetSaleEvenHandler += item.OnSetSale;
            ManagerP.EndSaleEvenHandler += item.OnEndSale;
            _dbService.LibraryItems.Add(item);
            return item;
        }
        public Items Delete(Items libraryItem = null, Guid id = default(Guid))//מחיקת עצם
        {
            Items item = null;
            if (id != default(Guid))
            {
                item = _dbService.LibraryItems.FirstOrDefault(i => i.Id == id);
                if (item != null)
                    _dbService.LibraryItems.Remove(item);
            }
            else if (libraryItem != null)
            {
                item = _dbService.LibraryItems.FirstOrDefault(i => i == libraryItem);
                if (item != null) _dbService.LibraryItems.Remove(libraryItem);
            }
            return item;
        }
        public Items Update(Items old, Items @new)//עדכון עצם
        {
            var temp = old;
            if (@new != null && old != null)
            {
                _dbService.LibraryItems.Remove(old);
                ManagerP.EndSaleEvenHandler += @new.OnEndSale;
                ManagerP.SetSaleEvenHandler += @new.OnSetSale;
                _dbService.LibraryItems.Add(@new);
            }
            return @new;
        }
        public IQueryable<Items> GetAvailable() => _dbService.LibraryItems.AsQueryable();


        public Dictionary<Items, People> GetLoaned() => _dbService.LoanedLibraryItems;
        public Items RetrieveItem(int index)
        {
            if (index > _dbService.LoanedLibraryItems.Count || index < 0)
                throw new IndexOutOfRangeException();

            Items temp = Add(_dbService.LoanedLibraryItems.ElementAt(index).Key);
            People p = _dbService.LoanedLibraryItems.ElementAt(index).Value;
            p.RemoveItem(temp);
            _dbService.LoanedLibraryItems.Remove(temp);
            return temp;
        } 
        public Items RetrieveItem(Items item)//הלוואת עצם
        {
            if (item == null || !_dbService.LoanedLibraryItems.ContainsKey(item))
                throw new SearchExp($"No loadned item named: {item}");
            Items temp = this.Add(_dbService.LoanedLibraryItems.FirstOrDefault(x => x.Key == item).Key);
            _dbService.LoanedLibraryItems.Remove(temp);
            return temp;
        } 
        public Items RetrieveItem(People person)
        {
            var element = _dbService.LoanedLibraryItems.FirstOrDefault(x => x.Value == person).Key;
            if (element != null)
            {
                _dbService.LoanedLibraryItems.Remove(element);
                return element;
            }
            return null;
        } 
        public Items AddLoan(Items item, People owner, DateTime itemReturnDate)
        {
            if (itemReturnDate < DateTime.Now) itemReturnDate = DateTime.Now.AddDays(1);
            _dbService.LoanedLibraryItems.Add(item, owner);
            owner.AddNewItem(item, itemReturnDate);
            return Delete(item);
        }

        public IEnumerator GetEnumerator() 
        {
            for (int i = 0; i < _dbService.LibraryItems.Count; i++)
                if (_dbService.LibraryItems[i] != null)
                    yield return _dbService.LibraryItems[i];
        }
    }
}
