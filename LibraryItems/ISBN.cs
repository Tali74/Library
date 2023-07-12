using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryItems
{
    internal class ISBN
    {
        private const int _prefix = 978;
        public static Dictionary<string, int> Publishers { get; set; }
        private string _publisher;
        public ISBN()
        {
            Publishers = new Dictionary<string, int>();
        }
        public static int Prefix { get => _prefix; }
        public string Publisher
        {
            get => _publisher;
            set
            {
                if (!Publishers.ContainsKey(value))
                    throw new Exception("Publisher" + value + "is not recognized by ISBN");
                _publisher = value;
            }
        }
        public int SerialNumber { get; set; }
        public int Control { get => (Publishers[Publisher] + SerialNumber) % 10; }
        public static bool operator /(ISBN isbn1, ISBN isbn2)
        {
            if (isbn1.ToString().Equals(isbn2.ToString()))
                return true;
            throw new Exception("ISBN is incorrect");
        }
    }
}
