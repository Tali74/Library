using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryItems
{
    public class Journal : Items
    {
        public static List<string> JournalGenres = new List<string>();
        public static List<string> Contributors { get; private set; }
        public List<string> Editors { get; private set; }
        public List<string> Genres { get; private set; }
        public Journal(string title, double price, int sale = 0)
            : base(title, price, sale)
        {
            Contributors = new List<string>();
            Editors = new List<string>();
            Genres = new List<string>();
        }
      
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Jornal information:");
            sb.AppendLine($"Jornal title: {Title}");
            sb.AppendLine($"Publish date: {PublishDate}");
            sb.AppendLine($"Jornal Gener: {Genres}");
            sb.AppendLine($"Editor: {Editors}");
            return sb.ToString();
        }

    }
}

