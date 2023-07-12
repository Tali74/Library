using System;
using LibraryItems;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Datastor;
using Windows.UI.Popups;

namespace BookLib
{
    public sealed partial class AddBook : Page//הוספת עצם חדש
    {
      //עצם חדש לפי IData
        IData<Items> _repo = new LibraryData();
        //לעדכן עצם
        Items _editItem;
        //סימנים להתעלם מהם
        readonly char[] _delimiterChars = { ' ', ',', '.', ':', '\t' };

        //ערכים שצריכים להיות לעצם חדש
        string _title;
        double _price;
        string _auther;
        int _amount;
        int _sale = 0;
        int _serialNumber = 0;
        int _revision = new Random().Next(10);
        string _country = "Israel";

        DateTime _publicationDate;

        public AddBook()
        {
            this.InitializeComponent();          
        }
        
        //יש לנו בקומבו בוקס 2 אופציות- אם בוחרים את הספר אז חלון הוספת ספר מופיע ואם בוחרים ג'ורנל אז חלון הג'ורנל יופיע
        //אם מדובר במנהל שבא לערוך את הספר אז יופיע לון העריכה
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Init_BookGenresAndCountriesComboBox();
            Init_JournalGenresComboBox();
            if (e.Parameter is Items)
            {
                _editItem = (Items)e.Parameter;
                AddItemBtn.Visibility = Visibility.Collapsed;
                EditItemBtn.Visibility = Visibility.Visible;
                Init_ItemTypeComboBox(_editItem.GetType());
                if (_editItem is Book)
                    Init_Book_TextBoxesWithKnownProperties();
                else if (_editItem is Journal)
                    Init_Journal_TextBoxesWithKnownProperties();
            }
            else
                Init_ItemTypeComboBox();

            base.OnNavigatedTo(e);
        }
        //אוצפיות ערכים לשינוי
        void Init_MustPropertiesItem()
        {
            NameTB.Text = _editItem.Title;
            PriceTB.Text = $"{_editItem.Price}";
            AmountTB.Text = $"{_editItem.Amount}";
            PublicationDate.Date = _publicationDate = _editItem.PublicationDate;
        }
        //פרמטרים לערכון ג'ואנל
        void Init_Journal_TextBoxesWithKnownProperties()
        {
            JournalSP.Visibility = Visibility.Visible;
            Init_MustPropertiesItem();

            var item = _editItem as Journal;
            foreach (var str in item.Contributors) ContributorsTB1.Text += $"{str.Replace(" ", "-")}\t";
            foreach (var editor in item.Editors) EditorsTB1.Text += $"{editor.Replace(" ", "-")}\t";
            JournalGenresCB.SelectedItem = item.Genres.FirstOrDefault();
        }
        //בחירת ג'ורנל בקומבו בוקס
        void Init_JournalGenresComboBox()
        {
            JournalGenresCB.ItemsSource = Journal.JournalGenres;
        }
        //בחירת ספר קומבו בוקס
        void Init_BookGenresAndCountriesComboBox()
        {
            BookGenresCB.ItemsSource = Book.BookGenres;
            CountryCB.ItemsSource = ISBN.Countries.Keys;
        }
        //הקומבו בוקס קשר לקלאס עצמים
        void Init_ItemTypeComboBox(Type itemType = null) 
        {
            List<Type> listOfLibraryItems;
            if (itemType == null)
            {
                listOfLibraryItems = (
                    from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                    from type in domainAssembly.GetTypes()
                    where typeof(Items).IsAssignableFrom(type)
                    where type != typeof(Items)
                    select type).ToList();
            }
            else listOfLibraryItems = new List<Type>() { itemType };

            TypeCombo.ItemsSource = listOfLibraryItems.Select(t => t.Name).ToList();
            if (itemType != null) TypeCombo.SelectedItem = itemType.Name;
            TypeCombo.SelectionChanged += ItemTypeCB_SelectionChanged;

        }
        //יש לנו בקומבו בוקס 2 אופציות- אם בוחרים את הספר אז חלון הוספת ספר מופיע ואם בוחרים ג'ורנל אז חלון הג'ורנל יופיע
        void ItemTypeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = TypeCombo.SelectedItem.ToString();
            if (selection == typeof(Book).Name)
            {
                BookSP.Visibility = Visibility.Visible;
                JournalSP.Visibility = Visibility.Collapsed;
            }
            else if (selection == typeof(Journal).Name)
            {
                JournalSP.Visibility = Visibility.Visible;
                BookSP.Visibility = Visibility.Collapsed;
            }
        }
        //מילוי המידע של ספר
        void Init_Book_TextBoxesWithKnownProperties()
        {
            BookSP.Visibility = Visibility.Visible;
            Init_MustPropertiesItem();

            var item = _editItem as Book;
            foreach (var author in item.Authors) AuthorsTB1.Text += $"{author.Replace(" ", "-")}\t";
            PublisherTB1.Text = item.Publisher;
            BookGenresCB.SelectedItem = item.Genres.FirstOrDefault();
            CountryCB.SelectedItem = item.ISBN.Country;
            RevisionTB1.Text = item.Revision.ToString();
            SerialNumberTB1.Text = item.ISBN.SerialNumber.ToString();
        }
  
       //יצירת ספר
        void CreateBook()
        {
            if (int.TryParse(SerialNumberTB1.Text, out int serialNumber)) _serialNumber = serialNumber;
            if (int.TryParse(RevisionTB1.Text, out int revision)) _revision = revision;
            if (CountryCB.SelectedIndex >= 0) _country = CountryCB.SelectionBoxItem.ToString();

            var tempBook = new Book(_title, _price, _publicationDate, _auther, _amount) ;

            if (BookGenresCB.SelectedIndex >= 0) tempBook.Genres.Add(BookGenresCB.SelectionBoxItem.ToString());
            else tempBook.Genres.Add("Other");

            if (PublisherTB1.Text != string.Empty)
            {
                if (!ISBN.Publishers.ContainsKey(PublisherTB1.Text))
                    ISBN.Publishers.Add(PublisherTB1.Text, new Random().Next());
                tempBook.Publisher = PublisherTB1.Text;
            }
            else tempBook.Publisher = "Anonymus";

            if (AuthorsTB1.Text != string.Empty) tempBook.Authors.AddRange(AuthorsTB1.Text.Split(_delimiterChars));
            else tempBook.Authors.Add("Anonymus");

            if (_editItem != null) _repo.Update(_editItem, tempBook);
            else _repo.Add(tempBook); 
        }
        //יצירת ג'ורנל
        void CreateJournal()
        {
            var tempJournal = new Journal(_title, _price, _publicationDate,_auther , _sale);

            if (JournalGenresCB.SelectedIndex >= 0) tempJournal.Genres.Add(JournalGenresCB.SelectionBoxItem.ToString());

            if (ContributorsTB1.Text != string.Empty) tempJournal.Contributors.AddRange(ContributorsTB1.Text.Split(_delimiterChars));

            if (EditorsTB1.Text != string.Empty) tempJournal.Editors.AddRange(EditorsTB1.Text.Split(_delimiterChars));

            if (_editItem != null) _repo.Update(_editItem, tempJournal); 
            else _repo.Add(tempJournal);
        }
       //כפתור העדכון הוא כמו ההוספה
         void EditItemBtn_Click(object sender, RoutedEventArgs e)
        {
            AddItem_Click(this, e);
        }
        //שדות חובה
        bool AddItemMustCondition()
        {
            if (NameTB.Text != string.Empty && double.TryParse(PriceTB.Text, out double price) && TypeCombo.SelectedIndex >= 0
                         && _publicationDate != DateTime.MinValue)
            {
                _title = NameTB.Text;
                _price = price;
                return true;
            }
            return false;
        }
        //כפתורים של בחירת תאריך
        private void PublicationDatePicker_SelectedDateChanged(DatePicker sender, DatePickerSelectedValueChangedEventArgs args)
        {
            if (PublicationDate.SelectedDate != null)
                _publicationDate = new DateTime(args.NewDate.Value.Year, args.NewDate.Value.Month, args.NewDate.Value.Day);
        
    }
        //כפתור איפוס התאריך
        private void ClearDateButton_Click(object sender, RoutedEventArgs e)
        {
             PublicationDate.SelectedDate = null;
            _publicationDate = DateTime.MinValue;
        }
        //כפתור הוספת הספר
         async void AddItem_Click(object sender, RoutedEventArgs e)
        {
            if (!AddItemMustCondition()) return;

            string content = TypeCombo.SelectionBoxItem.ToString();

            if (content == typeof(Book).Name) CreateBook();
            else if (content == typeof(Journal).Name) CreateJournal();

            await new MessageDialog("Item was added", "Item added").ShowAsync();

            if (_editItem != null)
                Frame.Navigate(typeof(Guest));
            else Frame.Navigate(typeof(MainPage));
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));

        }
    }
}
