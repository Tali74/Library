using AllPeople;
using Datastor;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BookLib
{

    public sealed partial class registration : Page
    {
        IPersone<People> _repo = new PeopleData();

        public registration()
        {
            this.InitializeComponent();
        }

        private async void SubmitBtn_Click(object sender, RoutedEventArgs e)//הוספת לקוח חדש
        {
            _repo.Add(new CustomerP(UserNameTB.Text, PasswordTB.Password));
            await new MessageDialog("Customer was added").ShowAsync();
            Frame.Navigate(typeof(MainPage));

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));

        }
    }
}
