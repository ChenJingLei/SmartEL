using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using SmartEL.Model;
using Elysium.Controls;
using SmartEL.Event;
using SmartEL.Controls;
using SmartEL.SQL;

namespace SmartEL.UI.Page
{
    /// <summary>
    /// PlanPage.xaml 的交互逻辑
    /// </summary>
    public partial class PlanPage
    {

        private Frame pageFrame;
        private List<Classroom> allClassrooms;

        public PlanPage(Frame pageFrame, List<Classroom> allClassrooms)
        {
            InitializeComponent();
            this.pageFrame = pageFrame;
            this.allClassrooms = allClassrooms;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CommandButton button = (pageFrame.Parent as Grid)?.FindName("BtnBack") as CommandButton;
            if (button != null)
            {
                button.Visibility = Visibility.Visible;
                button.Click += BtnBack_Click;
            }

            //获取所有教室

            Button[,] planbuttons = new Button[allClassrooms.Count, 6];
            int i = 0;
            foreach (Classroom classroom in allClassrooms)
            {
                for (int j = 0; j < 6; j++)
                {
                    //HorizontalAlignment="Left" VerticalAlignment="Top"
                    planbuttons[i, j] = new Button();
                    planbuttons[i, j].Name = "btn_" + classroom.Name + "_" + j;
                    planbuttons[i, j].Margin = new Thickness(10 + j * 90, 36 + i * 60, 0, 0);
                    planbuttons[i, j].HorizontalAlignment = HorizontalAlignment.Left;
                    planbuttons[i, j].VerticalAlignment = VerticalAlignment.Top;
                    planbuttons[i, j].Width = 80;
                    planbuttons[i, j].Height = 50;
                    planbuttons[i, j].IsEnabled = false;
                    if (j != 0) planbuttons[i, j].Click += planbtn_Click;
                    planbuttons[i, j].Content = j == 0 ? classroom.Name : $"第{j}节课";
                    classPlans.Children.Add(planbuttons[i, j]);
                    classPlans.RegisterName("btn_" + classroom.Name + "_" + j, planbuttons[i, j]);
                }
                i++;
            }
            classPlans.Height = 36 + i * 60 + 36;
        }

        private void planbtn_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
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

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {

        }

        private DispatcherTimer timeToOper = new DispatcherTimer();

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            //设置内容
            int[] hms_config = Config.Config.chcektime;

            //计划开始
            timeToOper.Tick += CurTimeToOper;
            timeToOper.Interval = new TimeSpan(hms_config[0], hms_config[1], hms_config[2]);
            timeToOper.Start();

            TaskScheduler task = new TaskScheduler();
            TimeToDo += task.TimeToDo;

            int i = 0;
            foreach (Classroom classroom in allClassrooms)
            {
                for (int j = 0; j < 6; j++)
                {
                    Button btn = classPlans.FindName("btn_" + classroom.Name + "_" + j) as Button;
                    btn.IsEnabled = true;
                }
                i++;
            }
        }

        public event EventHandler<ClassrooomEventArgs> TimeToDo; //自定义一个事件，用于给设备发送信息
        private void CurTimeToOper(object sender, EventArgs e)
        {
            Console.WriteLine(DateTime.Now.ToString("HH.mm"));
            DateTime now = DateTime.Now;

            //确定是哪节课
            for (int i = 0; i < Config.Config.course.Length / Config.Config.course.Rank; i++)
            {
                if (Config.Config.Courses[i, 0] - new TimeSpan(0, 0, 10, 0) < now && Config.Config.Courses[i, 1] + new TimeSpan(0, 0, 5, 0) > now)
                {
                    int classindex = i + 1;
                    //是否是课前5分钟内或者是课后5分钟内
                    if (((Config.Config.Courses[i, 0] - new TimeSpan(0, 0, 10, 0) < now && Config.Config.Courses[i, 0] > now)) ||
                        ((Config.Config.Courses[i, 1] < now && Config.Config.Courses[i, 1] + new TimeSpan(0, 0, 5, 0) > now)))
                    {
                        ClassrooomEventArgs args = new ClassrooomEventArgs();
                        args.Rooms = allClassrooms;
                        args.Num = classindex;
                        TimeToDo?.Invoke(this, args);
                        break;
                    }
                }
            }


        }

        //更新UI
        public void modifySatus(string classId, int isWork)
        {
            //WPF：Dispatcher.Invoke 方法，只有在其上创建 Dispatcher 的线程才可以直接访问DispatcherObject。若要从不同于在其上创建 DispatcherObject 的线程的某个线程访问 DispatcherObject，请对与 DispatcherObject 关联的 Dispatcher 调用 Invoke 或 BeginInvoke。需要强制线程安全的 DispatcherObject 的子类可以通过对所有公共方法调用 VerifyAccess 来强制线程安全。这样可以保证调用线程是在其上创建 DispatcherObject 的线程。
            Dispatcher.Invoke(new Action(
                delegate
                {
                    Button btn = classPlans.FindName(classId) as Button;

                    btn.Background = isWork == 1 ? Brushes.Black : BtnStart.Background;
                }
                ));

        }
    }
}
