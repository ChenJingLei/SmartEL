using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SmartEL.UI.Page;
using Window = Elysium.Controls.Window;

namespace SmartEL.UI.Windows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
//            this.PageContext.Source = new Uri("../Page/MenuPage.xaml", UriKind.Relative);
//            this.PageContext.Content = new MenuPage();
                MenuPage menuPage = new MenuPage(this.PageContext);
            this.PageContext.NavigationService.Navigate(menuPage);
//                NavigationService.GetNavigationService()


        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
