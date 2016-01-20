using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ClassRoomPlan;
using SmartEL.Controls;
using SmartEL.Model;
using Visifire.Charts;
using Classroom = ClassRoomPlan.Classroom;
using Window = Elysium.Controls.Window;

namespace SmartEL
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer ShowTimer;//获取当前时间，并显示
//        public event EventHandler<ClassrooomEventArgs> TimeToDo; //自定义一个事件，用于给设备发送信息
        private ClassRoomPlanControl control;
        private List<Classroom> allClassrooms;
        private TaskScheduler task;

        public MainWindow()
        {
            InitializeComponent();

            

            ShowTimer = new DispatcherTimer();
            control = new ClassRoomPlanControl();
            
            ShowTimer.Tick += ShowCurTimer;
            ShowTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowTimer.Start();
            //获取所有教室信息
            allClassrooms = control.GetAllClassrooms();
            

            //设置控件
            for (int j = 1; j <= 5; j++)
            {
                for (int k = 1; k <= 2; k++)
                {
                    ComboBox cB1 = UserSetGrid.FindName("D" + j + "H"+k) as ComboBox;
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
            

            Button[,] planbuttons = new Button[allClassrooms.Count, 6];
            Button[] devicebuttons = new Button[allClassrooms.Count];
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
                    if (j!=0) planbuttons[i, j].Click += planbtn_Click;
                    planbuttons[i, j].Content = j == 0 ? classroom.Name : $"第{j}节课";
                    classPlans.Children.Add(planbuttons[i, j]);
                    classPlans.RegisterName("btn_" + classroom.Name + "_" + j, planbuttons[i, j]);
                }
                devicebuttons[i] = new Button();
                devicebuttons[i].Margin = new Thickness(20 + (i % 5) * 110, 10 + (i / 5) * 110, 0, 0);
                devicebuttons[i].HorizontalAlignment = HorizontalAlignment.Left;
                devicebuttons[i].VerticalAlignment = VerticalAlignment.Top;
                devicebuttons[i].Width = 100;
                devicebuttons[i].Height = 100;
                devicebuttons[i].Content = classroom.Name + "\n温度：xxx\n湿度：xxx\n光强：xxx\n雨水：xxx\nxxxx：xxx";
                device.Children.Add(devicebuttons[i]);
                classPlans.RegisterName("devicebtn_" + classroom.Id, devicebuttons[i]);

                devicelist.Items.Add(classroom.Name);
                i++;
            }
            classPlans.Height = 36 + i * 60 + 36;
            device.Height = 10 + ((i + 1) / 5) * 110 + 10;
        }

        private void planbtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string[] param = btn.Name.Split('_');
            //btn_A101_1
            lbDevice.Content = param[1] + " 第" + param[2] + "节课";
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(deviceTime.Text))
            {
                time = deviceTime.Text;
            }
            //控件集
            Control[] controls = {light1, light2, light3, T1dev, T2dev, H1dev, F1dev, F2dev, I1dev};

            string id = "0000";
            foreach (Classroom classroom in allClassrooms)
            {
                if (classroom.Name.Equals(param[1]))
                {
                    id = classroom.Id;
                    break;
                }
            }

            //命令
            string command = control.GetControlCommand(id, time, int.Parse(param[2]));
            //每一电器命令
            //L01#0/L02#0/L03#1/T01#20.4/T02#25.6/H01#45.7/F01#1
            string[] devicecommand = command.Split('/');
            for (int i = 0; i < 9; i++)
            {
                string[] comm = devicecommand[i].Split('#');
                if (comm[1].Equals("1"))
                {
                    ((CheckBox) controls[i]).IsChecked = true;
                }
                else if(comm[1].Equals("0"))
                {
                    ((CheckBox)controls[i]).IsChecked = false;
                }
                else
                {
//                    TextBox t = (TextBox) controls[i];
                    ((TextBox) controls[i]).Text = comm[1];
                }

            }

            PlanGrid.Visibility = Visibility.Hidden;
            BtnBack.Visibility = Visibility.Hidden;

            classDetail.Visibility = Visibility.Visible;

            deviceTime.SelectedDate = DateTime.Now;
            deviceTime.BlackoutDates.Add(new CalendarDateRange(new DateTime(),(DateTime.Now.Date).AddDays(-1)));

        }

        private void deviceTime_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            string time = DateTime.Parse(deviceTime.SelectedDate.ToString()).ToString("yyyy-MM-dd");
//                time = deviceTime.Text;
            
            //控件集
            Control[] controls = { light1, light2, light3, T1dev, T2dev, H1dev, F1dev, F2dev, I1dev };

            string id = "0000";
            string name = lbDevice.Content.ToString().Substring(0, 4);
            foreach (Classroom classroom in allClassrooms)
            {
                if (classroom.Name.Equals(name))
                {
                    id = classroom.Id;
                    break;
                }
            }

            int num = int.Parse(lbDevice.Content.ToString().Substring(6, 1));

            //命令
            string command = control.GetControlCommand(id, time, num);
            //每一电器命令
            //L01#0/L02#0/L03#1/T01#20.4/T02#25.6/H01#45.7/F01#1
            string[] devicecommand = command.Split('/');
            for (int i = 0; i < 9; i++)
            {
                string[] comm = devicecommand[i].Split('#');
                if (comm[1].Equals("1"))
                {
                    ((CheckBox)controls[i]).IsChecked = true;
                }
                else if (comm[1].Equals("0"))
                {
                    ((CheckBox)controls[i]).IsChecked = false;
                }
                else
                {
                    //                    TextBox t = (TextBox) controls[i];
                    ((TextBox)controls[i]).Text = comm[1];
                }

            }
        }

        private void deviceBtn_Click(object sender, RoutedEventArgs e)
        {
            string id = "0000";
            string name = lbDevice.Content.ToString().Substring(0, 4);
            foreach (Classroom classroom in allClassrooms)
            {
                if (classroom.Name.Equals(name))
                {
                    id = classroom.Id;
                    break;
                }
            }
            string value = "";
            //控件集
            Control[] controls = { light1, light2, light3, T1dev, T2dev, H1dev, F1dev, F2dev, I1dev };
            for (int i = 0; i < 3; i++)
            {
                if (((CheckBox)controls[i]).IsChecked == true)
                {
                    value += "L0" + (i+1) + "#" + 1 + "/";
                }
                else
                {
                    value += "L0" + (i+1) + "#" + 0 + "/";
                }

            }

            for (int i = 0; i < 2; i++)
            {
                value += "T0" + (i+1) + "#" + T1dev.Text + "/";
            }

            value += "H01" + "#" + H1dev.Text + "/";
            for (int i = 0; i < 2; i++)
            {
                if (((CheckBox)controls[i]).IsChecked == true)
                {
                    value += "F0" + (i+1) + "#" + 1 + "/";
                }
                else
                {
                    value += "F0" + (i+1) + "#" + 0 + "/";
                }
            }
            value += "I01" + "#" + I1dev.Text;

            int num = int.Parse(lbDevice.Content.ToString().Substring(6, 1));
            string feild = "";
            if (num == 1) feild = "Plan12";
            else if (num == 2) feild = "Plan34";
            else if (num == 3) feild = "Plan56";
            else if (num == 4) feild = "Plan78";
            else if (num == 5) feild = "Plan912";

            control.SetRoomPlan(id,feild,value, DateTime.Parse(deviceTime.SelectedDate.ToString()).ToString("yyyy-MM-dd"));
        }

        private void cancleBtn_Click(object sender, RoutedEventArgs e)
        {
            PlanGrid.Visibility = Visibility.Visible;
            BtnBack.Visibility = Visibility.Visible;

            classDetail.Visibility = Visibility.Hidden;
        }

       

        public void modifyData(string classId, string data)
        {
            //WPF：Dispatcher.Invoke 方法，只有在其上创建 Dispatcher 的线程才可以直接访问DispatcherObject。若要从不同于在其上创建 DispatcherObject 的线程的某个线程访问 DispatcherObject，请对与 DispatcherObject 关联的 Dispatcher 调用 Invoke 或 BeginInvoke。需要强制线程安全的 DispatcherObject 的子类可以通过对所有公共方法调用 VerifyAccess 来强制线程安全。这样可以保证调用线程是在其上创建 DispatcherObject 的线程。
            Dispatcher.Invoke(new Action(
                delegate
                {
                    Button btn = classPlans.FindName(classId) as Button;

                    btn.Content = data;
                }
                ));
        }

        private void ShowCurTimer(object sender, EventArgs e)
        {
            Tt.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss dddd", new CultureInfo("zh-cn"));
        }

        private void Btnplan_Click(object sender, RoutedEventArgs e)
        {

            BtnGrid.Visibility = Visibility.Hidden;
            PlanGrid.Visibility = Visibility.Visible;
            BtnBack.Visibility = Visibility.Visible;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            BtnBack.Visibility = Visibility.Hidden;
            BtnGrid.Visibility = Visibility.Visible;
            PlanGrid.Visibility = Visibility.Hidden;
            DeviceGrid.Visibility = Visibility.Hidden;
            DataGrid.Visibility = Visibility.Hidden;
            UserSetGrid.Visibility = Visibility.Hidden;
        }

        private DispatcherTimer timeToOper = new DispatcherTimer();
        private DispatcherTimer deviceTimer = new DispatcherTimer();

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            //设置内容
            int[] hms_config = Config.Config.chcektime;
            
            //计划开始
            timeToOper.Tick += CurTimeToOper;
            timeToOper.Interval = new TimeSpan(hms_config[0], hms_config[1], hms_config[2]);
            timeToOper.Start();

            //统计开始
            DispatcherTimer deviceTimer = new DispatcherTimer();
            deviceTimer.Tick += DeviceStatus;
            deviceTimer.Interval = new TimeSpan(0, 0, 0, 1);


            task = new TaskScheduler();
//            TimeToDo += task.TimeToDo;


            deviceTimer.Start();

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

        private void DeviceStatus(object sender, EventArgs e)
        {
     
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            timeToOper.Stop();
            deviceTimer.Stop();
        }

        private void Btndevice_Click(object sender, RoutedEventArgs e)
        {
            BtnGrid.Visibility = Visibility.Hidden;
            BtnBack.Visibility = Visibility.Visible;
            DeviceGrid.Visibility = Visibility.Visible;

        }

        private void CurTimeToOper(object sender, EventArgs e)
        {
            Console.WriteLine(DateTime.Now.ToString("HH.mm"));
            double now = Convert.ToDouble(DateTime.Now.ToString("HH.mm"));
            int num = 0;
            for (int i = 0; i < Config.Config.course.Length / Config.Config.course.Rank; i++)
            {
                if (Config.Config.course[i, 0] < now && Config.Config.course[i, 1] > now)
                {
                    num = i + 1;
                }
            }
//            ClassrooomEventArgs args = new ClassrooomEventArgs();
//            args.Rooms = allClassrooms;
//            args.Num = num;
//
//            if (TimeToDo != null) TimeToDo(this, args);
        }
        
        #region testSPine
        private List<DateTime> LsTime = new List<DateTime>()
            {
               new DateTime(2012,1,1),
               new DateTime(2012,2,1),
               new DateTime(2012,3,1),
               new DateTime(2012,4,1),
               new DateTime(2012,5,1),
               new DateTime(2012,6,1),
               new DateTime(2012,7,1),
               new DateTime(2012,8,1),
               new DateTime(2012,9,1),
               new DateTime(2012,10,1),
               new DateTime(2012,11,1),
               new DateTime(2012,12,1),
            };
        private List<string> cherry = new List<string>() { "33", "75", "60", "98", "67", "88", "39", "45", "13", "22", "45", "80" };
        private List<string> pineapple = new List<string>() { "13", "34", "38", "12", "45", "76", "36", "80", "97", "22", "76", "39" };
        #endregion

        private void BtnTimeSubmit_Click(object sender, RoutedEventArgs e)
        {
            string[] time = new string[2];
            time[0] = startDateTime.Text+" 00:00:00";
            time[1] = endDateTime.Text + " 00:00:00";
            if (time[0].Equals(time[1]))
            {
                time[1] = endDateTime.Text + " 23:59:00";
            }

//            object[] data = statistics.AnalystData(time, devicelist.Text);

            Data.Children.Clear();
            CreateChartSpline(devicelist.Text + "教室温度、湿度、光照强度", LsTime, cherry, pineapple);

            //            CreateChartSpline(devicelist.Text+"教室温度、湿度、光照强度", (List<DateTime>)data[0], (List<string>)data[1], (List<string>)data[2]);

        }

        private void BtnUserSet_Click(object sender, RoutedEventArgs e)
        {
            BtnGrid.Visibility = Visibility.Hidden;
            BtnBack.Visibility = Visibility.Visible;
            UserSetGrid.Visibility = Visibility.Visible;

            //解析刷新时间内容
            int[] hms_config = Config.Config.chcektime;

            for (int i = 0; i < hms_config.Length; i++)
            {
                if (hms_config[i] != 0)
                {
                    refreshTime.Text = hms_config[i].ToString();
                    cBtimeformate.SelectedIndex = i;
                    
                }
            }

            //设置数据库的界面
            db_address.Text = Config.Config.dbserveraddress;
            db_port.Text = Config.Config.dbport;
            db_username.Text = Config.Config.dbuserID;
            db_password.Password = Config.Config.dbpassword;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    string[] time = (Config.Config.course[i, j].ToString("00.00")).Split('.');

                    ComboBox cB1 = UserSetGrid.FindName("D" + (i+1) + "H" + (j+1)) as ComboBox;
                    cB1.Text = time[0];
                    ComboBox cB2 = UserSetGrid.FindName("D" + (i+1) + "M" + (j+1)) as ComboBox;
                    cB2.Text = time[1];
                }
            }
        }

        private void BtnComfire_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    string time = "";
                    ComboBox cB1 = UserSetGrid.FindName("D" + (i + 1) + "H" + (j + 1)) as ComboBox;
                    time += cB1.Text;
                    ComboBox cB2 = UserSetGrid.FindName("D" + (i + 1) + "M" + (j + 1)) as ComboBox;
                    time += ("." + cB2.Text);

                    Config.Config.course[i, j] = Convert.ToDouble(time);
                }
            }
            int[] hms_config = default(int[]);
            switch (cBtimeformate.SelectedIndex)
            {
                case 0:
                    hms_config = new[] {int.Parse(refreshTime.Text), 0, 0};
                    break;
                case 1:
                    hms_config = new[] {0, int.Parse(refreshTime.Text), 0};
                    break;
                case 2:
                    hms_config = new[] {0, 0, int.Parse(refreshTime.Text)};
                    break;
            }
            Config.Config.chcektime = hms_config;

            Config.Config.dbuserID = db_username.Text;
            Config.Config.dbpassword = db_password.Password;
            Config.Config.dbserveraddress = db_address.Text;
            Config.Config.dbport = db_port.Text;


            BtnBack.Visibility = Visibility.Hidden;
            BtnGrid.Visibility = Visibility.Visible;
            UserSetGrid.Visibility = Visibility.Hidden;

        }

        private void BtnData_Click(object sender, RoutedEventArgs e)
        {
            BtnGrid.Visibility = Visibility.Hidden;
            DataGrid.Visibility = Visibility.Visible;
            BtnBack.Visibility = Visibility.Visible;
        }



        #region ChartSpline
        public void CreateChartSpline(string name, List<DateTime> lsTime, List<string> cherry, List<string> pineapple)
        {
            //创建一个图标
            Chart chart = new Chart();

            //设置图标的宽度和高度
            chart.Width = 580;
            chart.Height = 380;
            chart.Margin = new Thickness(10, 5, 10, 5);
            //是否启用打印和保持图片
            chart.ToolBarEnabled = false;

            //设置图标的属性
            chart.ScrollingEnabled = false;//是否启用或禁用滚动
            chart.View3D = true;//3D效果显示

            //创建一个标题的对象
            Title title = new Title();

            //设置标题的名称
            title.Text = name;
            title.Padding = new Thickness(0, 10, 5, 0);

            //向图标添加标题
            chart.Titles.Add(title);

            //初始化一个新的Axis
            Axis xaxis = new Axis();
            //设置Axis的属性
            //图表的X轴坐标按什么来分类，如时分秒
            xaxis.IntervalType = IntervalTypes.Months;
            //图表的X轴坐标间隔如2,3,20等，单位为xAxis.IntervalType设置的时分秒。
            xaxis.Interval = 1;
            //设置X轴的时间显示格式为7-10 11：20           
            xaxis.ValueFormatString = "MM月";
            //给图标添加Axis            
            chart.AxesX.Add(xaxis);

            Axis yAxis = new Axis();
            //设置图标中Y轴的最小值永远为0           
            yAxis.AxisMinimum = 0;
            //设置图表中Y轴的后缀          
            yAxis.Suffix = "";
            chart.AxesY.Add(yAxis);


            // 创建一个新的数据线。               
            DataSeries dataSeries = new DataSeries();
            // 设置数据线的格式。               
            dataSeries.LegendText = "樱桃";

            dataSeries.RenderAs = RenderAs.Spline;//折线图

            dataSeries.XValueType = ChartValueTypes.DateTime;
            // 设置数据点              
            DataPoint dataPoint;
            for (int i = 0; i < lsTime.Count; i++)
            {
                // 创建一个数据点的实例。                   
                dataPoint = new DataPoint();
                // 设置X轴点                    
                dataPoint.XValue = lsTime[i];
                //设置Y轴点                   
                dataPoint.YValue = double.Parse(cherry[i]);
                dataPoint.MarkerSize = 8;
                //dataPoint.Tag = tableName.Split('(')[0];
                //设置数据点颜色                  
                // dataPoint.Color = new SolidColorBrush(Colors.LightGray);                   
                dataPoint.MouseLeftButtonDown += new MouseButtonEventHandler(dataPoint_MouseLeftButtonDown);
                //添加数据点                   
                dataSeries.DataPoints.Add(dataPoint);
            }

            // 添加数据线到数据序列。                
            chart.Series.Add(dataSeries);


            // 创建一个新的数据线。               
            DataSeries dataSeriesPineapple = new DataSeries();
            // 设置数据线的格式。         

            dataSeriesPineapple.LegendText = "菠萝";

            dataSeriesPineapple.RenderAs = RenderAs.Spline;//折线图

            dataSeriesPineapple.XValueType = ChartValueTypes.DateTime;
            // 设置数据点              

            DataPoint dataPoint2;
            for (int i = 0; i < lsTime.Count; i++)
            {
                // 创建一个数据点的实例。                   
                dataPoint2 = new DataPoint();
                // 设置X轴点                    
                dataPoint2.XValue = lsTime[i];
                //设置Y轴点                   
                dataPoint2.YValue = double.Parse(pineapple[i]);
                dataPoint2.MarkerSize = 8;
                //dataPoint2.Tag = tableName.Split('(')[0];
                //设置数据点颜色                  
                // dataPoint.Color = new SolidColorBrush(Colors.LightGray);                   
                dataPoint2.MouseLeftButtonDown += new MouseButtonEventHandler(dataPoint_MouseLeftButtonDown);
                //添加数据点                   
                dataSeriesPineapple.DataPoints.Add(dataPoint2);
            }
            // 添加数据线到数据序列。                
            chart.Series.Add(dataSeriesPineapple);

            //将生产的图表增加到Grid，然后通过Grid添加到上层Grid.           
            Grid gr = new Grid();
            gr.Children.Add(chart);

            Data.Children.Add(gr);
        }
        #endregion

        #region ClickEvent
        //点击事件
        void dataPoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //DataPoint dp = sender as DataPoint;
            //MessageBox.Show(dp.YValue.ToString());
        }



        #endregion

        #region

        //        private void BtnSelectFile_Click(object sender, RoutedEventArgs e)
        //        {
        //            OpenFileDialog dialog = new OpenFileDialog();
        //            dialog.InitialDirectory = @"C:\Users\cjl20\Documents\Visual Studio 2015\Projects\SmartEL\SmartEL\bin\Debug";
        //            //"C:\\Users\\JohnChen\\Documents\\Visual Studio 2013\\Projects\\LightSchedule\\test";
        //            dialog.Filter = "Sheet files (*.xls,*.xlsx)|*.xls;*.xlsx|All files (*.*)|*.*";
        //            ExcelOper excel = new ExcelOper();
        //            if (dialog.ShowDialog() == true)
        //            {
        //                Console.WriteLine(dialog.FileName);
        //                try
        //                {
        //                    DataTable dt = excel.GetExcelDatatable(dialog.FileName, "time");
        //                    ExcelDataGrid.ItemsSource = dt.AsDataView();
        //                    ExcelDataGrid.AutoGenerateColumns = true;
        //                }
        //                catch (Exception)
        //                {
        //
        //                    MessageBox.Show("xls打开失败", "警告！！！");
        //                }
        //            }
        //        }

        //        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        //        {
        //            ExcelGrid.Visibility = Visibility.Hidden;
        //            //            _currentGrid.Visibility = Visibility.Hidden;
        //            BtnGrid.Visibility = Visibility.Visible;
        //            BtnBack.Visibility = Visibility.Hidden;
        //            if (!IsdataGridNull())
        //            {
        //                Btnplan.IsEnabled = true;
        //            }
        //        }

        //        private bool IsdataGridNull()
        //        {
        //            if (ExcelDataGrid.Items.Count == 0)
        //            {
        //                return true;
        //            }
        //            return false;
        //        }



        //        public static T GetVisualChild<T>(Visual parent) where T : Visual
        //        {
        //            T childContent = default(T);
        //            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
        //            for (int i = 0; i < numVisuals; i++)
        //            {
        //                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
        //                childContent = v as T;
        //                if (childContent == null)
        //                {
        //                    childContent = GetVisualChild<T>(v);
        //                }
        //                if (childContent != null)
        //                { break; }
        //            }
        //            return childContent;
        //        }



        //        private void timer_tick(object sender, EventArgs e)
        //        {
        //            Client client = new Client("localhost", 48569);
        //            //            LbStatus.Content = client.Readmessage();
        //            //            LbStatus.FontSize = 18;
        //        }











        //        public TimeS GeTimeS(String dst)
        //        {
        //            String[] ds = dst.Split(':');
        //            return new TimeS(int.Parse(ds[0]), int.Parse(ds[1]));
        //        }
        //
        //        private void BtnComfire_Click(object sender, RoutedEventArgs e)
        //        {
        //            task.D1start = GeTimeS(d1start.Text);
        //            task.D1stop = GeTimeS(d1stop.Text);
        //
        //            task.D2start = GeTimeS(d2start.Text);
        //            task.D2stop = GeTimeS(d2stop.Text);
        //
        //            task.D3start = GeTimeS(d3start.Text);
        //            task.D3stop = GeTimeS(d3stop.Text);
        //
        //            task.D4start = GeTimeS(d4start.Text);
        //            task.D5stop = GeTimeS(d4stop.Text);
        //
        //            task.D5start = GeTimeS(d5start.Text);
        //            task.D5stop = GeTimeS(d5stop.Text);
        //        }
        #endregion
    }
}

