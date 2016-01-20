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
using Elysium.Controls;

namespace SmartEL.UI
{
    /// <summary>
    /// UserSetPage.xaml 的交互逻辑
    /// </summary>
    public partial class UserSetPage
    {
        private Frame pageFrame;

        public UserSetPage()
        {
            InitializeComponent();
        }

        public UserSetPage(Frame pageFrame)
        {
            InitializeComponent();
            this.pageFrame = pageFrame;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CommandButton button = (pageFrame.Parent as Grid)?.FindName("BtnBack") as CommandButton;
            if (button != null)
            {
                button.Visibility = Visibility.Visible;
                button.Click += BtnBack_Click;
            }

            //设置控件
            for (int j = 1; j <= 5; j++)
            {
                for (int k = 1; k <= 2; k++)
                {
                    ComboBox cB1 = UserSetGrid.FindName("D" + j + "H" + k) as ComboBox;
                    for (int l = 0; l < 24; l++)
                    {
                        cB1.Items.Add(l.ToString("00"));
                    }
                    ComboBox cB2 = UserSetGrid.FindName("D" + j + "M" + k) as ComboBox;
                    for (int l = 0; l < 60; l++)
                    {
                        cB2.Items.Add(l.ToString("00"));
                    }
                }
            }
        }



        private void BtnComfire_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            CommandButton btn = e.Source as CommandButton;
            if (btn != null) btn.Visibility = Visibility.Hidden;
            if (pageFrame.NavigationService.CanGoBack)
            {
                pageFrame.NavigationService.GoBack();
            }
        }

        
    }
}
