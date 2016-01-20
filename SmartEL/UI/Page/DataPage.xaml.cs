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
using SmartEL.Model;
using SmartEL.SQL;

namespace SmartEL.UI.Page
{
    /// <summary>
    /// DataPage.xaml 的交互逻辑
    /// </summary>
    public partial class DataPage
    {
        private Frame pageFrame;
        private List<Classroom> allClassrooms;
        public DataPage()
        {
            InitializeComponent();
        }

        public DataPage(Frame pageFrame, List<Classroom> allClassrooms)
        {
            InitializeComponent();
            this.pageFrame = pageFrame;
            this.allClassrooms = allClassrooms;
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            CommandButton button = (pageFrame.Parent as Grid)?.FindName("BtnBack") as CommandButton;
            if (button != null)
            {
                button.Visibility = Visibility.Visible;
                button.Click += BtnBack_Click;
            }

            //获取所有教室
            foreach (Classroom classroom in allClassrooms)
            {
                devicelist.Items.Add(classroom.Name);
            }
        }

        private void BtnTimeSubmit_Click(object sender, RoutedEventArgs e)
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
