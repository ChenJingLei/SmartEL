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
using System.Windows.Threading;
using Elysium.Controls;
using SmartEL.Controls;
using SmartEL.SQL;
using SmartEL.Model;

namespace SmartEL.UI.Page
{
    /// <summary>
    /// DevicePage.xaml 的交互逻辑
    /// </summary>
    public partial class DevicePage : System.Windows.Controls.Page
    {
        private Frame pageFrame;
        private List<Classroom> allClassrooms;
        private StatisticsData statisticsData;

        public DevicePage()
        {
            InitializeComponent();
        }

        public DevicePage(Frame pageFrame, List<Classroom> allClassrooms)
        {
            this.pageFrame = pageFrame;
            InitializeComponent();
            this.allClassrooms = allClassrooms;
        }

        private void DeviceGrid_Loaded(object sender, RoutedEventArgs e)
        {
            CommandButton button = (pageFrame.Parent as Grid)?.FindName("BtnBack") as CommandButton;
            if (button != null)
            {
                button.Visibility = Visibility.Visible;
                button.Click += BtnBack_Click;
            }

            //获取所有教室
            Button[] devicebuttons = new Button[allClassrooms.Count];
            int i = 0;
            foreach (Classroom classroom in allClassrooms)
            {
                devicebuttons[i] = new Button();
                devicebuttons[i].Margin = new Thickness(20 + (i % 5) * 110, 10 + (i / 5) * 160, 0, 0);
                devicebuttons[i].HorizontalAlignment = HorizontalAlignment.Left;
                devicebuttons[i].VerticalAlignment = VerticalAlignment.Top;
                devicebuttons[i].Width = 100;
                devicebuttons[i].Height = 150;
                devicebuttons[i].Content = classroom.Name + "\n温度：xxx\n湿度：xxx\n光强：xxx\n雨水：xxx\nxxxx：xxx";
                device.Children.Add(devicebuttons[i]);
                device.RegisterName("devicebtn_" + classroom.Id, devicebuttons[i]);
                i++;
            }
            device.Height = 10 + (((i + 1) / 5)+1) * 160 + 10;

            //统计开始
            DispatcherTimer deviceTimer = new DispatcherTimer();
            deviceTimer.Tick += DeviceStatus;
            deviceTimer.Interval = new TimeSpan(0, 0, 0, 1);

            statisticsData = new StatisticsData(allClassrooms, this);

            deviceTimer.Start();

        }

        private void DeviceStatus(object sender, EventArgs e)
        {
            statisticsData.run();
        }

        public void modifyData(string classId, string data)
        {
            //WPF：Dispatcher.Invoke 方法，只有在其上创建 Dispatcher 的线程才可以直接访问DispatcherObject。若要从不同于在其上创建 DispatcherObject 的线程的某个线程访问 DispatcherObject，请对与 DispatcherObject 关联的 Dispatcher 调用 Invoke 或 BeginInvoke。需要强制线程安全的 DispatcherObject 的子类可以通过对所有公共方法调用 VerifyAccess 来强制线程安全。这样可以保证调用线程是在其上创建 DispatcherObject 的线程。
            Dispatcher.Invoke(new Action(
                delegate
                {
                    Button btn = device.FindName(classId) as Button;

                    btn.Content = data;
                }
                ));
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
