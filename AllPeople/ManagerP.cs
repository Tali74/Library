using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPeople
{
    public class ManagerP : People////נפתח קלאס מנהל היורש מאנשים המכיל שם וסיסמה, למנהל גם יש את היכולת להתחיל הנחות ולסיים
    {
        //נכניס איוונטים של תחילת הנחה וסיום
        public static event Action SetSaleEvenHandler;
        public static event Action EndSaleEvenHandler;

        public ManagerP(string name, string password) : base(name, password)
        {

        }
        public static void OnSetSale() => SetSaleEvenHandler.Invoke();
        public static void OnEndSale() => EndSaleEvenHandler.Invoke();
    }
}
