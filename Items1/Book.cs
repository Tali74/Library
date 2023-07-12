using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryItems
{
    public class Book : Items//קלאס של ספר שיורש מקלאס עצם
    {
        public static List<string> BookGenres = new List<string>();//יצירת רשימה של זאנרים
        public static List<string> KnownAuthors = new List<string>();//ררשימה של סופרים
        public ISBN ISBN { get; private set; }// המספר הזה תלוי בסופר ובמדינה
        private const string _defaultCountry = "Israel";

        public List<string> Authors { get; private set; }
        public string Publisher
        {
            get { return this.ISBN.Publisher; }
            set { this.ISBN.Publisher = value; }
        }
        public int Revision { get; set; }

        public StringBuilder AuthorsPrint()
        {
            StringBuilder authors = new StringBuilder();
            for (int i = 0; i < Authors.Count; i++)
            {
                authors.Append($"{Authors[i]} ");
            }
            return authors;
        }
        public List<string> Genres { get; private set; }
        public Book(string title, double price, DateTime publishDate, string author, int amount, int serialNumber = 0, string country = _defaultCountry, int sale = 0)
            : base(title, price, publishDate, author, amount)
        {
            this.ISBN = new ISBN();
            Authors = new List<string>();
            Genres = new List<string>();
        }

        public override string ToString() => $"Book: {Title}, {PublicationDate:d}";
        public override string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format)
            {
                case "s": return $"B: {Title} {Revision}";
                case "$": return $"B: {Title} - {Revision} - {Price:c} - Sale {Sale}%";
                case "$$": return $"{Price:c}";
                case "S": return $"Book: {Title}, Published: {PublicationDate}, Revision: {Revision}, Category: {string.Join(", ", Genres)}";
                case "NoSale": return $"{_price:c}";
                default:
                    return this.ToString();
            }
        }
        public override void OnSetSale() => Sale = 15;//אם יש מבצע אז הוא 15%
        public override void OnEndSale() => Sale = 0;//אין מבצע
    }
}

