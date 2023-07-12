using Datastor;
using LibraryItems;
using AllPeople;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;


namespace BookLib
{

    public sealed partial class BlankPage1 : Page//חלון רכישה
    {
        ILonable<Items> _repo = new LibraryData();
        Items _item;
        readonly People _person = MainPage.WebSurfer;

        public BlankPage1()
        {
            this.InitializeComponent();
        }

         async void PaymentBtn_Click(object sender, RoutedEventArgs e)
        {

            if (_person is GuestP)//אם אורח רוכש רוכש אז זה מוסיף אותו לרשימת רוכשים, מסיר את העצם ורושם לו שהעצם יגיע עוד שעה
            {
                DateTime expDate = DateTime.Now.AddDays(new Random().Next(1, 10));
                _repo.AddLoan(_item, MainPage.WebSurfer, expDate);
                await new MessageDialog($"{_item.Title} - will arrive in 1 hour").ShowAsync();
                }
            else
            {
                DateTime expDate = DateTime.Now.AddDays(new Random().Next(1, 10));//אם לקוח מלווה רוכש אז זה מוסיף אותו לרשימת רוכשים, מסיר את העצם ורושם לו שהעצם יגיע עוד שעה ומתי להחזיר
                _repo.AddLoan(_item, MainPage.WebSurfer, expDate);
                await new MessageDialog($"{_item.Title} - will arrive in 1 hour :)\nYou Shall return the {_item.GetType().Name} until {expDate:f}", "Payment Completed").ShowAsync();
            }
            Frame.Navigate(typeof(MainPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)// אם מדובר באורח אז הכפתור הופך לרכישה מהלוואה
        {
            _item = e.Parameter as Items;
            if (MainPage.WebSurfer is GuestP) BorrowBtn.Content = "Purchase";
            Init_PurchaseSP();
            base.OnNavigatedTo(e);
        }
        void Init_PurchaseSP() 
        {
            TitleTB.Text += $"{_item.Title}";
            PriceTB.Text += $"{_item.Price + "$"}";
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Guest));

        }
    }

}

