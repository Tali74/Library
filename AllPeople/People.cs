using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LibraryItems;

namespace AllPeople// קלאס אנשים אשרי סוגי אנשים יורשים ממנו
{
    public abstract class People : IFormattable
    {
        string _name;
        string _password;
        Dictionary<Items, DateTime> _loanedItems;

        public string Name => _name;
        public string Password => _password;
        public People(string name, string password)
        {
            _name = name;
            _password = password;
            _loanedItems = new Dictionary<Items, DateTime>();
        }
        //ניצור דישינרי לעצמים מלווים על ידי שמירת העצם ותאריך
        public Dictionary<Items, DateTime> LoanedItems => _loanedItems;

        //בהוספת עצם חדש אסור שהוא יהיה ריק או עם תאריך עתידי
        public void AddNewItem(Items items, DateTime exp)
        {
            if (items == null || exp < DateTime.Now)
                throw new ArgumentNullException("Item is null");
            LoanedItems.Add(items, exp);
        }
        
        //מחיקת עצם
        public void RemoveItem(Items item) => LoanedItems?.Remove(item);
        
        //סיסמה חדשה
        public void SetNewPassword(string newPassword) => _password = newPassword;

        //יצירת סטרינג שם
        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format)
            {
                case "n":
                    return _name;
                default:
                    return ToString();
            }
        }
        public override string ToString() => _name;
    }
}
