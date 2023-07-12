using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryItems
{
    public abstract class Items//קלאס עצם, כל עצם חייב להכיל שם ומחיר 
    {
        protected double _price;
        public double _sale;

        public Guid Id { get; protected set; } = Guid.NewGuid();
        public string Title { get; protected set; }
        public string Auther { get; protected set; }
        public DateTime PublicationDate { get; protected set; }

        public int Amount { get; set; }

        public double Price { get { return _price - (_price * Sale); } }
        public double Sale
        {
            get => _sale;
            protected set
            {
                if (value >= 0 && value < 50)
                    _sale = value / 100;
                else _sale = 0;
            }
        }
        public bool InStock { get; set; }
        public DateTime outOfStock { get; set; }
        public DateTime returnUntil { get; set; }
        public DateTime PublishDate { get; set; }
        public void SellItem()
        {
            InStock = false;
            outOfStock = DateTime.Now;
        }

        public Items(string title, double price, DateTime publishDate, string author, int amount)
        {
            _price = price;
            Title = title;
            PublicationDate = publishDate;
            Auther = author;
            Amount = amount;
        }
        public abstract string ToString(string format, IFormatProvider formatProvider);

        public abstract void OnSetSale();
        public abstract void OnEndSale();

    }
}
