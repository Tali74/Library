
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryItems
{
    public class ISBN
    {
        private const int _prefix = 1000;
        public static Dictionary<string, int> Countries { get; set; }//המספר תלוי בשם המדינה ולכל מדינה יש ערך מםפרי
        public static Dictionary<string, int> Publishers { get; set; }//לכל סופר יש ערך מספרי

        private string _country;
        private string _publisher;

        static ISBN()
        {
            Countries = new Dictionary<string, int>();
            Publishers = new Dictionary<string, int>();
        }
        public static int Prefix { get => _prefix; }
        public string Country
        {
            get => _country;
            set
            {
                if (!Countries.ContainsKey(value))
                    throw new IsbnException($"Country '{value}' is not recognized by ISBN");
                _country = value;
            }
        }

        public string Publisher
        {
            get => _publisher;
            set
            {
                if (!Publishers.ContainsKey(value))
                    throw new IsbnException($"Publisher '{value}' is not recognized by ISBN");
                _publisher = value;
            }
        }
        public int SerialNumber { get; set; }
        public int Control { get => (Countries[Country] + Publishers[Publisher] + SerialNumber) % 10; }
        public static bool operator /(ISBN iSBN1, ISBN iSBN2)
        {
            if (iSBN1.ToString().Equals(iSBN2.ToString()))
                return true;
            throw new IsbnException("ISBN is incorrect");
        }
        public override string ToString() => $"{_prefix}-{Countries[Country]:D3}-{Publishers[Publisher]:D3}-{SerialNumber:D3}-{Control}";
    }
}
