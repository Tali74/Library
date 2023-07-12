using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datastor
{
    public interface  IData<T> where T : class//קלאס של מידע
    {
        T Add(T item);
        IQueryable<T> GetAvailable();
        T Get(Guid id);
        T Update(T old, T @new);
        T Delete(T item, Guid id = default(Guid));
    }
}
