using Fluent;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Shoping.Presentation.View
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : RibbonWindow
    {
        public HomeWindow()
        {
            InitializeComponent();
        }
        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var productTabs = new ObservableCollection<TabItem>()
            {
                new TabItem() { Content = new Control.ProductUserControl()},
            };
            tabsProduct.ItemsSource = productTabs;

            var categoryTabs = new ObservableCollection<TabItem>()
            {
                new TabItem() { Content = new Control.CategoryUserControl()},
            };
            tabsCategory.ItemsSource = categoryTabs;

            var orderTabs = new ObservableCollection<TabItem>()
            {
                new TabItem() { Content = new Control.OrderUserControl()},
            };
            tabsOrder.ItemsSource = orderTabs;
            
            var statisticTabs = new ObservableCollection<TabItem>()
            {
                new TabItem() { Content = new Control.StatisticUserControl()},
            };
            tabsStatistic.ItemsSource = statisticTabs;

            var settingTabs = new ObservableCollection<TabItem>()
            {
                new TabItem() { Content = new Control.SettingUserControl()},
            };
            tabsSetting.ItemsSource = settingTabs;
        }
        private void Quit_Handle(object sender, MouseButtonEventArgs e)
        {
            var result = MessageBox.Show("Quit?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }     
    }
}
