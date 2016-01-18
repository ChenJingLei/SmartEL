using System;
using System.Collections.Generic;
using System.Threading;
using ClassRoomPlan;

namespace SmartEL
{
    public class StatisticsData
    {
        private ClassRoomPlanControl control;
        private List<Classroom> allClassrooms;
        private MainWindow window;

        public StatisticsData(List<Classroom> allClassrooms, MainWindow window)
        {
            this.allClassrooms = allClassrooms;
            this.window = window;
            control = new ClassRoomPlanControl();
        }

        public void run()
        {
            int Count = allClassrooms.Count;
            ThreadPool.SetMaxThreads(Count, Count);
            foreach (Classroom classroom in allClassrooms)
            {
                Thr t = new Thr(classroom, window);
                ThreadPool.QueueUserWorkItem(t.ThreadProc);
            }
        }
        public object[] AnalystData(string[] time, string text)
        {
            
            List<Statistics> list = control.AnalystData(time, text);
            List<DateTime> times = new List<DateTime>();
            List<string> tem = new List<string>();
            List<string> hum = new List<string>();
            List<string> lux = new List<string>();
            foreach (Statistics statistics in list)
            {
                times.Add(statistics.Date);
                tem.Add(statistics.Temperature.ToString("0.00"));
                hum.Add(statistics.Humidity.ToString("0.00"));
            }
            object[] lists = {times,tem,hum};
            return lists;
        }

        internal class Thr
        {
            private ClassRoomPlanControl control;
            private string Ip;
            private ushort port;
            private MainWindow window;
            private string id; //教室名称
            private string name;

            public Thr(Classroom classroom, object UI)
            {
                Ip = classroom.Ip;
                port = classroom.Port;
                window = (MainWindow)UI;
                id = classroom.Id;
                control = new ClassRoomPlanControl();
                name = classroom.Name;
            }

            /// <summary>
            /// 收取设备数据
            /// </summary>
            /// <param name="state">发送内容</param>
            public void ThreadProc(object state)
            {
                Client client = new Client(Ip, port);

                //收到的设备的信息
//                string data = client.Readmessage();


                //收取数据解析
                
//                double tem = 11.11;
//                double hum = 11.2;
                double lux = 11.1;
                double tem = new Random().NextDouble() * 100;
                double hum = new Random().NextDouble() * 100;
                string rain = "Yes";


                //更新UI
                window.modifyData("devicebtn_" + id, name + $"\n温度：{tem}\n湿度：{hum}\n光强：{lux}\n雨水：{rain}");


                Statistics statistics = new Statistics(id, DateTime.Now, tem, hum);
                control.StatisticsData(statistics);

            }
        }
       
    }
}
