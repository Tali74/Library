using System;
using System.Collections.Generic;
using LibraryItems;
using System.Text.Json.Serialization;
using AllPeople;
using Datastor.Sort;

namespace Datastor
{
    public class DbService//עמוד של מאגר המידע
    {
        public static List<IComparer<Items>> SortLibraryItem { get; private set; }
        [JsonPropertyName("Items")]
        public List<Items> LibraryItems { get; private set; }
        [JsonPropertyName("People")]
        public List<People> peoples { get; private set; }

        [JsonPropertyName("LoanedLibraryItems")]
        public Dictionary<Items, People> LoanedLibraryItems { get; private set; } = new Dictionary<Items, People>();

        static DbService dbService;

        public static DbService Instance
        {
            get
            {
                if (dbService == null)
                    dbService = new DbService();
                return dbService;
            }
        }

        private DbService()//יצירת רשימות של עצמים ושמות
        {
            LibraryItems = new List<Items>();
            peoples = new List<People>();
            Init();
        }
        static DbService()
        { 
            SortLibraryItem = new List<IComparer<Items>>()//דרכים למיוון
            {
                new ByNameA(),
                new ByNameZ(),
                new ByPriceHigh(),
                new ByPriceLow(),
            };
        }

        void Init()
        {
            Init_Items();
            Init_People();
        }
        void Init_People()//יצירת לקוחות, עובדים ומנהלים
        {
            var customer1 = new CustomerP("tali", "12345");
            var customer2 = new CustomerP("gaya", "12345");


            var employee1 = new EmployeeP("omer", "employee");

            var manager = new ManagerP("avital", "manager");

            peoples.AddRange(new People[] { customer1, customer2, employee1, manager });

        }
        void Init_Items()// יצירת ספרים וג'ורנילם וכל מה שזה דורש
        {
            ISBN.Publishers.Add("Doubleday", 5);
            ISBN.Publishers.Add("Cemetery Dance Publications", 7);
            ISBN.Publishers.Add("Viking Press", 13);
            ISBN.Publishers.Add("Milkweed Editions", 20);
            ISBN.Publishers.Add("Harper", 11);
            ISBN.Publishers.Add("Knopf", 20);
            ISBN.Publishers.Add("Orbit", 7);

            

            ISBN.Countries.Add("Israel", 100);
            ISBN.Countries.Add("United States", 230);
            ISBN.Countries.Add("United Kingdom", 340);
            ISBN.Countries.Add("China", 70);
            ISBN.Countries.Add("India", 135);
            ISBN.Countries.Add("Indonesia", 275);
            ISBN.Countries.Add("Russia", 145);
            ISBN.Countries.Add("Germany", 340);
            ISBN.Countries.Add("France", 67);
            ISBN.Countries.Add("Thailand", 66);
            ISBN.Countries.Add("Italy", 189);
            ISBN.Countries.Add("Canada", 154);
            ISBN.Countries.Add("South Korea", 58);
            ISBN.Countries.Add("Vietnam", 127);
            ISBN.Countries.Add("Japan", 126);
            ISBN.Countries.Add("Pakistanm", 229);


            Book.KnownAuthors.Add("Dan Brown");
            Book.KnownAuthors.Add("stephen king");
            Book.KnownAuthors.Add("stephen king");
            Book.KnownAuthors.Add("Barbara Kingsolver");
            Book.KnownAuthors.Add("Robin Wall Kimmerer");
            Book.KnownAuthors.Add("Cormac McCarthy");
            Book.KnownAuthors.Add("N.K.Jemisin");
            Book.KnownAuthors.Add("Ed Yong");

            
            Book.BookGenres.Add("Mystery");
            Book.BookGenres.Add("detective");
            Book.BookGenres.Add("fiction");
            Book.BookGenres.Add("conspiracy");
            Book.BookGenres.Add("thriller");
            Book.BookGenres.Add("Action");
            Book.BookGenres.Add("adventure");
            Book.BookGenres.Add("Historical");
            Book.BookGenres.Add("Classic");
            Book.BookGenres.Add("Drama");
            Book.BookGenres.Add("Horror");
            Book.BookGenres.Add("Nature");

            

            Journal.JournalGenres.Add("Entertainment");
            Journal.JournalGenres.Add("Nature");
            Journal.JournalGenres.Add(" News magazine");
            Journal.JournalGenres.Add("Sports");
            Journal.JournalGenres.Add("Economic");
            Journal.JournalGenres.Add("General interest");



            var book1 = new Book("The Da Vinci Code",8, new DateTime(2003,4, 8), "Dan Brown",10);
            book1.Authors.Add("Dan Brown");
            book1.Publisher = "Doubleday";
            book1.Genres.Add("Mystery");
            book1.Genres.Add("detective");
            book1.Genres.Add("fiction");
            book1.Genres.Add("thriller");
            ManagerP.SetSaleEvenHandler += book1.OnSetSale;
            ManagerP.EndSaleEvenHandler += book1.OnEndSale;

            var book2 = new Book("INFERNO", 16.95, new DateTime(2014, 5, 6), "Dan Brown", 30);
            book2.Authors.Add("Dan Brown");
            book2.Publisher = "Doubleday";
            book2.Genres.Add("Mystery");
            book2.Genres.Add("detective");
            book2.Genres.Add("fiction");
            ManagerP.SetSaleEvenHandler += book2.OnSetSale;
            ManagerP.EndSaleEvenHandler += book2.OnEndSale;

            var book3 = new Book("Origin", 20, new DateTime(2017, 10, 3), "Dan Brown", 30);
            book3.Authors.Add("Dan Brown");
            book3.Publisher = "Doubleday";
            book3.Genres.Add("Mystery");
            book3.Genres.Add("detective");
            book3.Genres.Add("fiction");
            ManagerP.SetSaleEvenHandler += book3.OnSetSale;
            ManagerP.EndSaleEvenHandler += book3.OnEndSale;

            var book4 = new Book("1408", 18.99, new DateTime(2002, 3, 3), "stephen king", 30);
            book4.Authors.Add("stephen king");
            book4.Publisher = "Cemetery Dance Publications";
            book4.Genres.Add("Mystery");
            book4.Genres.Add("Horror"); book4.Genres.Add("fiction");
            ManagerP.SetSaleEvenHandler += book4.OnSetSale;
            ManagerP.EndSaleEvenHandler += book4.OnEndSale;

            var book5 = new Book("The Green Mile", 14.00, new DateTime(2007, 10, 3), "stephen king", 3);
            book5.Authors.Add("stephen king");
            book5.Publisher = "Cemetery Dance Publications";
            book5.Genres.Add("Mystery");
            book5.Genres.Add("Horror");
            book5.Genres.Add("fiction");
            ManagerP.SetSaleEvenHandler += book5.OnSetSale;
            ManagerP.EndSaleEvenHandler += book5.OnEndSale;

            var book6 = new Book("IT", 17.00, new DateTime(1986, 10, 3), "stephen king", 8);
            book6.Authors.Add("stephen king");
            book6.Publisher = "Viking Press";
            book6.Genres.Add("Mystery");
            book6.Genres.Add("Horror");
            book6.Genres.Add("fiction");
            ManagerP.SetSaleEvenHandler += book6.OnSetSale;
            ManagerP.EndSaleEvenHandler += book6.OnEndSale;

            var book7 = new Book("The Shining", 17.00, new DateTime(1977, 6, 3), "stephen king", 4);
            book7.Authors.Add("stephen king");
            book7.Publisher = "Doubleday";
            book7.Genres.Add("Mystery");
            book7.Genres.Add("Horror");
            book7.Genres.Add("fiction");
            ManagerP.SetSaleEvenHandler += book7.OnSetSale;
            ManagerP.EndSaleEvenHandler += book7.OnEndSale;

            var book8 = new Book("Demon Copperhead", 30.00, new DateTime(2022, 10, 18), "Barbara Kingsolver", 4);
            book8.Authors.Add("stephen king");
            book8.Publisher = "Doubleday";
            book8.Genres.Add("Classic");
            book8.Genres.Add("Drama");
            book8.Genres.Add("fiction");
            ManagerP.SetSaleEvenHandler += book8.OnSetSale;
            ManagerP.EndSaleEvenHandler += book8.OnEndSale;

            var book9 = new Book("Braiding Sweetgrass", 18.60, new DateTime(2015, 8, 11), "Robin Wall Kimmerer", 8);
            book9.Authors.Add("Robin Wall Kimmerer");
            book9.Publisher = "Harper";
            book9.Genres.Add("Nature");
            ManagerP.SetSaleEvenHandler += book9.OnSetSale;
            ManagerP.EndSaleEvenHandler += book9.OnEndSale;

            var book10 = new Book("The Passenger", 27.90, new DateTime(2022, 10, 25), "Cormac McCarthy", 9);
            book10.Authors.Add("Cormac McCarthy");
            book10.Publisher = "Knopf";
            book10.Genres.Add("Action");
            book10.Genres.Add("adventure");
            ManagerP.SetSaleEvenHandler += book10.OnSetSale;
            ManagerP.EndSaleEvenHandler += book10.OnEndSale;

            var book11 = new Book("The World We Make", 27.00, new DateTime(2022, 11, 1), "N. K. Jemisin", 4);
            book11.Authors.Add("N. K. Jemisin");
            book11.Publisher = "Doubleday";
            book11.Genres.Add("fiction");
            ManagerP.SetSaleEvenHandler += book11.OnSetSale;
            ManagerP.EndSaleEvenHandler += book11.OnEndSale;

            var book12 = new Book("An Immense World: How Animal Senses Reveal the Hidden Realms Around Us", 27.00, new DateTime(2022, 6, 21), "Ed Yong", 4);
            book12.Authors.Add("Ed Yong");
            book12.Publisher = "Orbit";
            book12.Genres.Add("Nature");
            ManagerP.SetSaleEvenHandler += book12.OnSetSale;
            ManagerP.EndSaleEvenHandler += book12.OnEndSale;



            var journal1 = new Journal("Billboard ",10 ,new DateTime(1830, 5, 9), "Hannah Karp", 20);
            journal1.Editors.AddRange(new[] { "Hannah Karp" });
            journal1.Genres.AddRange(new[] { "Entertainment" });
            journal1.Contributors.AddRange(new[] { "Jared Bell" });
            ManagerP.SetSaleEvenHandler += journal1.OnSetSale;
            ManagerP.EndSaleEvenHandler += journal1.OnEndSale;


            var journal2 = new Journal("Time ", 4, new DateTime(1923, 3, 3), "Edward Felsenthal", 20);
            journal2.Editors.AddRange(new[] { "Edward Felsenthal" });
            journal2.Genres.AddRange(new[] { "News magazine" });
            journal2.Contributors.AddRange(new[] { "Ai-jen Poo" });
            ManagerP.SetSaleEvenHandler += journal2.OnSetSale;
            ManagerP.EndSaleEvenHandler += journal2.OnEndSale;

            var journal3 = new Journal("Sports Illustrated", 14, new DateTime(1954, 8, 16), "Stephen Cannella", 20);
            journal3.Editors.AddRange(new[] { "Ryan Hunt" });
            journal3.Genres.AddRange(new[] { "Sports" });
            journal3.Contributors.AddRange(new[] { "Pat Forde" });
            ManagerP.SetSaleEvenHandler += journal3.OnSetSale;
            ManagerP.EndSaleEvenHandler += journal3.OnEndSale;

            var journal4 = new Journal("New York", 14, new DateTime(1968, 4, 8), "New York Media", 20);
            journal4.Editors.AddRange(new[] { "David Haskell" });
            journal4.Genres.AddRange(new[] { "General interest" });
            journal4.Contributors.AddRange(new[] { "Vox Media" });
            ManagerP.SetSaleEvenHandler += journal4.OnSetSale;
            ManagerP.EndSaleEvenHandler += journal4.OnEndSale;

            var journal5 = new Journal("People", 11, new DateTime(1974, 3, 4), "Dotdash Meredith", 20);
            journal5.Editors.AddRange(new[] { "Wendy Naugle" });
            journal5.Genres.AddRange(new[] { "General interest" });
            journal5.Contributors.AddRange(new[] { "Dotdash Meredith" });
            ManagerP.SetSaleEvenHandler += journal5.OnSetSale;
            ManagerP.EndSaleEvenHandler += journal5.OnEndSale;

            LibraryItems.AddRange(new Items[] { book1, book2, book3, book4, book5, book6, book7, book8, book9, book10, book11, book12 ,journal1, journal2, journal3, journal4, journal5 });

        }

    }
}
