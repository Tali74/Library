using AllPeople;
using LibraryItems;
using BookLib;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BookLib
{
    public class MyDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LibraryItemTemplate { get; set; }
        public DataTemplate PersonTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is Items)
                return LibraryItemTemplate;
            if (item is People)
                return PersonTemplate;

            return base.SelectTemplateCore(item);
        }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) => SelectTemplateCore(item);
    }
}
