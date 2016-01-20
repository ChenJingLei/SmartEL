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
using SmartEL.Model;
using SmartEL.SQL;

namespace SmartEL.UI.Page
{
    /// <summary>
    /// MenuPage.xaml 的交互逻辑
    /// </summary>
    public partial class MenuPage
    {
        private Frame pageFrame;
        private List<Classroom> allClassrooms;
        public MenuPage(Frame pageContext)
        {
            InitializeComponent();
            this.pageFrame = pageContext;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ClassRoomRepository roomRepository = new ClassRoomRepository();
            allClassrooms = roomRepository.FindAll();
        }

        private void Btnplan_Click(object sender, RoutedEventArgs e)
        {
            PlanPage planPage = new PlanPage(pageFrame,allClassrooms);
            pageFrame.NavigationService.Navigate(planPage);
        }

        private void BtnData_Click(object sender, RoutedEventArgs e)
        {
            DataPage dataPage = new DataPage(pageFrame, allClassrooms);
            pageFrame.NavigationService.Navigate(dataPage);
        }

        private void BtnDevice_Click(object sender, RoutedEventArgs e)
        {
            DevicePage devicePage = new DevicePage(pageFrame, allClassrooms);
            pageFrame.NavigationService.Navigate(devicePage);
        }

        private void BtnUserSet_Click(object sender, RoutedEventArgs e)
        {
            UserSetPage userSetPage = new UserSetPage(pageFrame);
            pageFrame.NavigationService.Navigate(userSetPage);
        }

       
    }
}
