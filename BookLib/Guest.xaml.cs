using Datastor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LibraryItems;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Popups;
using System.Threading.Tasks;
using AllPeople;

namespace BookLib
{
    public sealed partial class Guest : Page//למרות השם זה עמוד חיפש הספר אליו האורח יכול להיכנס ללא סיסמה
    {
        //העמוד מציג את רשימת עצמים הקיימים 
        Type[] _typesOfLibraryItems; //ספר או ג'ורנל
        IData<Items> _repo = new LibraryData();
        IComparer<Items> _chosenComparer;
        List<Items> _resultItems;

        public Guest()
        {
            this.InitializeComponent();
            Init_PageUIelements();


        }
        void Init_PageUIelements()//מיון לפי שם מחיר ומה שמוגדר
        {
            comboSort.ItemsSource = DbService.SortLibraryItem;
            Init_ItemTypeComboBox();
        }
        void Init_ItemTypeComboBox()
        {
            _typesOfLibraryItems = (
                from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in domainAssembly.GetTypes()
                where typeof(Items).IsAssignableFrom(type)
                where type != typeof(Items)
                select type).ToArray();

            comboType.ItemsSource = _typesOfLibraryItems.Select(t => t.Name).ToList();
        }
        void Init_ListViewResultItems(IComparer<Items> comparer = null, string title = null, Guid guid = default(Guid), ISBN isbn = null)// הצגת הנתונים לפי קומפרר
        {
            ResultLV.ItemsSource = null;
            _resultItems = new List<Items>();

            if (isbn != null)
                try { _resultItems.Add(((LibraryData)_repo)[isbn]); }
                catch (SearchExp ex) { CreateErrorMassage(ex.Message); }

            else if (guid != default(Guid))
                try { _resultItems.Add(((LibraryData)_repo)[guid]); }
                catch (SearchExp ex) { CreateErrorMassage(ex.Message); }

            else _resultItems = CreateResultsBySelectedCriterias(comparer, title, _resultItems);

            ResultLV.ItemsSource = _resultItems;
        }
        private void TypeG_Changed(object sender, SelectionChangedEventArgs e)//אם נחבר ספר אז יופיע ISBN
        {
            if (comboType.SelectedItem?.ToString() == typeof(Book).Name) ISBNSP.Visibility = Visibility.Visible;
            else ISBNSP.Visibility = Visibility.Collapsed;
            ResultLV.ItemsSource = null;
        }

        private void SortG_Changed(object sender, SelectionChangedEventArgs e)
        {
            int index = comboSort.SelectedIndex;
            if (index >= 0) _chosenComparer = DbService.SortLibraryItem[index];
            ResultLV.ItemsSource = null;
        }

        private void SearchNameG_TextChanged(object sender, TextChangedEventArgs e)//חיפוש על פי שם
        {
            ResultLV.ItemsSource = null;
        }
        void CreateErrorMassage(string message) //הודעת שגיאה
        {
            ErrorMessage.Visibility = Visibility.Visible;
            ErrorMessage.Text = message;
        }
        List<Items> CreateResultsBySelectedCriterias(IComparer<Items> comparer, string title, List<Items> allItems) 
        {
            Type typeSelected = null;
            if (comboType.SelectedIndex >= 0)
                typeSelected = Type.GetType($"{typeof(Items).Namespace}.{comboType.SelectedItem}, {typeof(Items).Assembly}");
            if (typeSelected != null)
            {
                if (typeSelected.Name == typeof(Book).Name)
                    allItems = ((LibraryData)_repo).FindType(typeof(Book));
                else if (typeSelected.Name == typeof(Journal).Name)
                    allItems = ((LibraryData)_repo).FindType(typeof(Journal));
            }
            else allItems = ((LibraryData)_repo).FindType(typeof(Items));

            if (comparer != null) allItems.Sort(comparer);

            if (title != null)
                try { allItems = ((LibraryData)_repo)[allItems, title]; }
                catch (SearchExp ex) { CreateErrorMassage(ex.Message); }

            return allItems;
        }

         void ResultLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AdminSP.Visibility = Visibility.Collapsed;
            if (ResultLV.SelectedIndex >= 0)
            {
                DataPackage dp = new DataPackage();
                dp.SetText($"{{{_resultItems[ResultLV.SelectedIndex].Id}}}");
                Clipboard.SetContent(dp);

                ContinueBtn.Visibility = Visibility.Visible;
                if (MainPage.WebSurfer is ManagerP)
                    AdminSP.Visibility = Visibility.Visible;
            }
            else
            {
                ContinueBtn.Visibility = Visibility.Collapsed;
                if (MainPage.WebSurfer is ManagerP)
                    AdminSP.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddBook), _resultItems[ResultLV.SelectedIndex]);
        }

        async Task<IUICommand> VerifyDelete()//בדיקה לפני מחיקה
        {
            MessageDialog dialog = new MessageDialog("Are you sure?", "Delete Item");
            dialog.Commands.Add(new UICommand("Yes"));
            dialog.Commands.Add(new UICommand("No", delegate (IUICommand command) { return; }));
            return await dialog.ShowAsync();
        }
        private void ContinueBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BlankPage1), _resultItems[ResultLV.SelectedIndex]);
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)//בלחיצה על כפתור חיפש זה מחפש בהתאם למה שמילאנו
        {
            ErrorMessage.Visibility = Visibility.Collapsed;

            if (ISBNCondition(out int country, out int publisher, out int serNum, out int control))
                SearchByIsbn(country, publisher, serNum);

            else
            {
                if (_chosenComparer != null && SearchNameG.Text != string.Empty) Init_ListViewResultItems(_chosenComparer, SearchNameG.Text);

                else if (_chosenComparer != null) Init_ListViewResultItems(_chosenComparer);

                else if (SearchNameG.Text != string.Empty) Init_ListViewResultItems(title: SearchNameG.Text);

                else Init_ListViewResultItems();
            }
        }
        void SearchByIsbn(int country, int publisher, int serNum) 
        {
            try
            {
                ISBN isbn = new ISBN() { Publisher = ISBN.Publishers.First(x => x.Value == publisher).Key, Country = ISBN.Countries.First(x => x.Value == country).Key, SerialNumber = serNum };
                Init_ListViewResultItems(isbn: isbn);
            }
            catch (IsbnException exIsbn) { CreateErrorMassage(exIsbn.Message); }
            catch (Exception) { CreateErrorMassage("Not valid ISBN"); }
        }
        bool ISBNCondition(out int country, out int publisher, out int serNum, out int control)// ISBN תלוי במדינה, סופר ומספרים אחרים ולכן הם חייבים להיו בו
        {
            country = 0;
            publisher = 0;
            serNum = 0;
            control = 0;
            if (int.TryParse(Prefix.Text, out int pref) && pref == 978 && int.TryParse(CountryNum.Text, out country) && country > 0 && country < 1000
                && int.TryParse(PublisherTB.Text, out publisher) && publisher < 1000 && publisher > 0
                 && int.TryParse(SerialNumberTB.Text, out serNum) && serNum >= 0 && serNum < 1000 && int.TryParse(ControlTB.Text, out control) && control % 10 < 10)
                return true;
            return false;
        }
        async void DeleteItem_Click(object sender, RoutedEventArgs e)//מנהל יכול למחוק עצם
        {
            IUICommand resultDialog = await VerifyDelete();
            if (resultDialog.Label == "Yes")
            {
                var temp = _repo.Delete(_resultItems[ResultLV.SelectedIndex]);
                ResultLV.ItemsSource = null;
                await new MessageDialog($"{temp.Title} [{temp.GetType().Name}] has been deleted from the repository", "Delete message").ShowAsync();
            }
        }


        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void Prefix_TextChanged(object sender, TextChangedEventArgs e)// אחד ממספרי החובה שיש בISBN
        {
            if (Prefix.Text == ISBN.Prefix.ToString())
            {
                SearchNameG.IsEnabled = false;
                comboType.IsEnabled = false;
                comboSort.IsEnabled = false;
            }
            else
            {
                SearchNameG.IsEnabled = true;
                comboType.IsEnabled = true;
                comboSort.IsEnabled = true;
            }
        }

    }
}