using System;
using LibraryItems;
using AllPeople;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Datastor;
using Windows.UI.Popups;

namespace BookLib
{

    public sealed partial class MainPage : Page//העמוד הראשי
    {
        public static People WebSurfer { get; private set; }
        List<People> _people;
        IData<Items> _repo = new LibraryData();
        IPersone<People> _repoPeople = new PeopleData();

        public MainPage()
        {
            this.InitializeComponent();
            _people = _repoPeople.Get().ToList();
        }

        private void GuestBtn_Click(object sender, RoutedEventArgs e)//אם נכנסים כאורח אז אין סיסמה וזה ישר עובד לחיפוש
        {
            GuestBtn.IsEnabled = false;
            CustomerBtn.IsEnabled = true;
            ManagerBtn.IsEnabled = true;
            EmployeeBtn.IsEnabled = true;
            LogInSP.Visibility = Visibility.Collapsed;

        }

        private void EmployeeBtn_Click(object sender, RoutedEventArgs e)// אם נכנסים כעובד אז צריך סיסמה וזה בודק
        {

            GuestBtn.IsEnabled = true;
            CustomerBtn.IsEnabled = true;
            ManagerBtn.IsEnabled = true;
            EmployeeBtn.IsEnabled = false;
            LogInSP.Visibility = Visibility.Visible;
        }

        private void ManagerBtn_Click(object sender, RoutedEventArgs e)// אם נכנסים כמנהל אז צריך סיסמה וזה בודק
        {
            GuestBtn.IsEnabled = true;
            CustomerBtn.IsEnabled = true;
            ManagerBtn.IsEnabled = false;
            EmployeeBtn.IsEnabled = true;
            LogInSP.Visibility = Visibility.Visible;

        }

        private void CustomerBtn_Click(object sender, RoutedEventArgs e)// אם נכנסים כלקוח אז צריך סיסמה וזה בודק
        {

            GuestBtn.IsEnabled = true;
            CustomerBtn.IsEnabled = false;
            ManagerBtn.IsEnabled = true;
            EmployeeBtn.IsEnabled = true;
            LogInSP.Visibility = Visibility.Visible;

        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)//יש אופציה להירשם להיות לקוח
        {
            Frame.Navigate(typeof(registration));
        }
        bool NameAndPasswordCheckValidation(People people) => people.Password == PasswordBox.Password && people.Name == UserNameTB.Text;

         async void ContinueBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!GuestBtn.IsEnabled)
            {
                WebSurfer = new GuestP();
                Frame.Navigate(typeof(Guest), _repo);
                return;
            }
            else if (!CustomerBtn.IsEnabled)
            {
                foreach (var people in _people)
                    if (NameAndPasswordCheckValidation(people))
                    {
                        WebSurfer = people;
                        Frame.Navigate(typeof(Guest), _repo);
                        return;
                    }
            }
            else if (!EmployeeBtn.IsEnabled)
            { 
                foreach (var people in _people)
                    if (NameAndPasswordCheckValidation(people) && (people is EmployeeP))
                    {
                        WebSurfer = people;
                        Frame.Navigate(typeof(Employee));
                        return;
                    }
            }
            else if (!ManagerBtn.IsEnabled)
            { 
                        foreach (var people in _people)
                            if (NameAndPasswordCheckValidation(people) && (people is ManagerP))
                            {
                                WebSurfer = people;
                                Frame.Navigate(typeof(Manager));
                                return;
                            }
            }

            await new MessageDialog("UserName / Password are inCorrect", "Error LogIn").ShowAsync();//הודעת שגיאה אם הסיסמה או שם משתמש לא נכון
        }
    }
}
