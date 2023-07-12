using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPeople
{
    public class CustomerP : People//נפתח קלאס לקוח היורש מאנשים המכיל מידע כמו שם וסיסמה וכמות ספרים מקסימלית שלקוח יכול ללוות 
    {
        public const int MaxItems = 3;
        public CustomerP(string name, string password) : base(name, password)
        {

        }
    }
}
