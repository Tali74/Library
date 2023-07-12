using AllPeople;
using Datastor;
using LibraryItems;
using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BookLib
{

    public sealed partial class borrowed : Page//מטרת העמוד להציג את הפריטים המושאלים ואלו שנרכשו
    {
        //העמוד מציג את רשימת העצמים והאנשים שלקחו אותם
        ILonable<Items> _repoLibrary = new LibraryData();
        IPersone<People> _repoPeople = new PeopleData();
        Dictionary<Items, People> _dicLoaned;

        public borrowed()
        {
            this.InitializeComponent();
            _dicLoaned = _repoLibrary.GetLoaned();

        }
        //לחיצה על העצם שנרכש מחזירה אותו למאגר
        private async void ListViewLoanedItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Items temp = _repoLibrary.RetrieveItem(ListViewLoanedItems.SelectedIndex);
            await new MessageDialog($"{temp.Title} is back in stock!").ShowAsync();
            Frame.Navigate(typeof(borrowed));

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Manager));

        }
    }
}
