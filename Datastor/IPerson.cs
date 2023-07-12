using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datastor
{
   public interface IPersone<T> where T : class
    {
        IQueryable<T> Get();
        T Add(T person);
        T Delete(T person);
        void EndSale();
        void SetSale();
    }
}
