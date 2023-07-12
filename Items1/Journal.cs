using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryItems
{
    public class Journal : Items//קלאס ג'רונל שיורש מקלאס עצם
    {
        public static List<string> JournalGenres = new List<string>();
        public List<string> Contributors { get; private set; }
        public List<string> Editors { get; private set; }
        public List<string> Genres { get; private set; }
        public Journal(string title, double price, DateTime publishDate, string author, int amount, int sale = 0)
             : base(title, price, publishDate, author, amount)
        {
            Contributors = new List<string>();
            Editors = new List<string>();
            Genres = new List<string>();
            Auther = author;
            Amount = amount;
        }

        public override string ToString() => $"Journal: {Title}, {PublicationDate:d}";
        public override string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format)
            {
                case "$": return $"J: {Title} - {Price:c} - Sale {Sale}%";
                case "$$": return $"{Price:c}";
                case "S": return $"Journal: {Title}, Published: {PublicationDate}, Category: {string.Join(", ", Genres)}";
                case "NoSale": return $"{_price:c}";
                default:
                    return this.ToString();
            }
        }
        public override void OnSetSale() => Sale = 20;
        public override void OnEndSale() => Sale = 0;
    }
}

