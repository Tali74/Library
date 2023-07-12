using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryItems
{
    internal class Book : Items
    {
        public static List<string> BookGenres = new List<string>();
        public static List<string> KnownAuthors = new List<string>();
        public ISBN ISBN { get; private set; }
        public List<string> Authors { get; private set; }
        public string Publisher
        {
            get { return this.ISBN.Publisher; }
            set { this.ISBN.Publisher = value; }
        }
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
        public Book(string title, double price, int serialNumber = 0, int sale = 0)
            : base(title, price, sale)
        {
            this.ISBN = new ISBN() { SerialNumber = serialNumber };
            Authors = new List<string>();
            Genres = new List<string>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Book information");
            sb.AppendLine($"Book title: {Title}");
            sb.AppendLine($"Publisher: {this.ISBN.Publisher}");
            sb.AppendLine($"Book Gener: {Genres}");
            sb.AppendLine($"Author: {AuthorsPrint()}");
            sb.AppendLine($"ISBN: {ISBN.ToString()}");
            if (InStock == true)
            {
                sb.AppendLine($"Book is in stock");
            }
            else
            {
                sb.AppendLine($"Book is out of stock.\nDate:{outOfStock:d}");
                sb.AppendLine($"Return until: {returnUntil:d}");
                if (returnUntil < DateTime.Now)
                {
                    sb.AppendLine("OVERDUE!!!");
                }
            }
            return sb.ToString();
        }
    }
}

