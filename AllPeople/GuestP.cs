using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPeople
{
    public class GuestP : People//נפתח קלאס אורח היורש מאנשים המכיל שם אורח קבוע וכלום כי לאורח אין סיסמה 
    {
        public GuestP() : base("Guest", "")
        {

        }
    }
}
