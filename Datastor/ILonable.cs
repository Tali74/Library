using AllPeople;
using System;
using System.Collections.Generic;


namespace Datastor
{
    public interface ILonable<T> where T : class
    {
        Dictionary<T, People> GetLoaned();
        T AddLoan(T item, People owner, DateTime itemReturnDate);
        T RetrieveItem(T item);
        T RetrieveItem(int index);
    }
}
