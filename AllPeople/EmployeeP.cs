using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPeople
{
    public class EmployeeP : People//נפתח קלאס עובד היורש מאנשים המכיל שם וסיסמה
    {
        public EmployeeP(string name, string password) : base(name, password)
        {

        }
    }
}
