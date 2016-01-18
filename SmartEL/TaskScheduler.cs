using System;
using System.Collections.Generic;
using System.Threading;
using ClassRoomPlan;

namespace SmartEL
{
    internal class TaskScheduler
    {
        private ClassRoomPlanControl control;

        public TaskScheduler()
        {
            control = new ClassRoomPlanControl();
        }

        public void TimeToDo(object sender, ClassrooomEventArgs e)
        {
            List<Classroom> list = e.Rooms;
            int num = e.Num;
            ManualResetEvent eventX = new ManualResetEvent(false);
            int Count = list.Count;
            ThreadPool.SetMaxThreads(Count, Count);
            foreach (Classroom classroom in list)
            {
                int i = control.GetStatus(classroom.Id, DateTime.Now.ToString("yyyy-MM-dd"), num);
                string command = control.GetControlCommand(classroom.Id, DateTime.Now.ToString("yyyy-MM-dd"), num);
                Thr t = new Thr(classroom, num, sender, i);
                ThreadPool.QueueUserWorkItem(t.ThreadProc, command);
            }
        }
    }

    internal class Thr
    {
        private delegate void ModifySatus(string classId, int color);
        private int num; //第几节课
        private string Ip;
        private ushort port;
        private MainWindow window;
        private string name; //教室名称
        ModifySatus modify = null;
        private int isWork = 0;
        public Thr(Classroom classroom, int num, object UI, int isWork)
        {
            Ip = classroom.Ip;
            port = classroom.Port;
            this.num = num - 1;
            window = (MainWindow)UI;
            name = classroom.Name;
            modify += window.modifySatus;
            this.isWork = isWork;
        }

        /// <summary>
        /// 发送开灯协议
        /// </summary>
        /// <param name="state">发送内容</param>
        public void ThreadProc(object state)
        {
            double now = Convert.ToDouble(DateTime.Now.ToString("HH.mm"));
            //时间在课前5分钟 开灯，时间在课后5分钟关灯 如果是不上课，发送关闭命令
            //            int isWork = int.Parse(state.ToString());
            Client client = new Client(Ip, port);
            if (isWork == 1 && (now >= Config.course[num, 0] && now <= Config.course[num, 0] + 0.10))
            {
               client.Sendmessage(name + "=" + state.ToString());

                modify("btn_" + name + "_" + (num + 1), 1);
            }
            else
            {
                client.Sendmessage(name + "=" + "L01#0/L02#0/L03#0/T01#0/T02#0/H01#0/F01#0/F02#0/I01#0");

                modify("btn_" + name + "_" + (num + 1), 0);
            }
        }
    }
}