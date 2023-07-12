using Datastor;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using AllPeople;

namespace BookLib
{ 
    public sealed partial class Employee : Page// עמוד של העובד
    {
        IPersone<People> _repo = new PeopleData();

        public Employee()
        {
            this.InitializeComponent();
        }
        //לעובד יש את אוםציה לחפש עצם
        private void SearchBtn_Tapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            SearchBtn.IsEnabled = false;
            Frame.Navigate(typeof(Guest));
        }
        //להוסיף עצם
        private void AddBtn_Tapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            AddBtn.IsEnabled = false;
            Frame.Navigate(typeof(AddBook));
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));

        }
    }
}
