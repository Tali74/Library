using AllPeople;
using Datastor;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;


namespace BookLib
{
    public sealed partial class Manager : Page//עמוד של מנהל
    {
        IPersone<People> _repo = new PeopleData();

        public Manager()
        {
            this.InitializeComponent();

        }
      
        private void SearchBtn_Tapped(object sender, DoubleTappedRoutedEventArgs e)//יכול לחפש עצם
        {
            SearchBtn.IsEnabled = false;
            Frame.Navigate(typeof(Guest));
        }

        private void AddBtn_Tapped(object sender, DoubleTappedRoutedEventArgs e)//להוסיף עצם
        {

            AddBtn.IsEnabled = false;
            Frame.Navigate(typeof(AddBook));
        }

         async void SaleStrBtn_Tapped(object sender, DoubleTappedRoutedEventArgs e)//להתחיל מבצע
        {
            if (MainPage.WebSurfer is ManagerP)
            {
                _repo.SetSale();
                await new MessageDialog("Sale On", "Sale On").ShowAsync();
            }
        }

         async void SaleEndBtn_Tapped(object sender, DoubleTappedRoutedEventArgs e)//לסיים מבצע
        {
            if (MainPage.WebSurfer is ManagerP)
            {
                _repo.EndSale();
                await new MessageDialog("End sale", "End sale").ShowAsync();
            }
        }
    
        private void ViewBorrowedItemsBtn_Click(object sender, RoutedEventArgs e)//מנהל יכול לראות איזה עצמים נרכשו או נלוו
        {
            Frame.Navigate(typeof(borrowed));
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));

        }
    }
}
