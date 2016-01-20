using System;
using System.Collections.Generic;
using System.Threading;

using SmartEL.Event;
using SmartEL.Model;
using SmartEL.SQL;
using SmartEL.UI.Page;


namespace SmartEL.Controls
{
    internal class TaskScheduler
    {
        private PlanRepository repository;

        public TaskScheduler()
        {
            repository = new PlanRepository();
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
                string command = "";
                //cid、Pclassindex，Ptime
                //查询当前时间所在教室的电器设备状态
                int isWork = 0;
                if (DateTime.Now < Config.Config.Courses[num - 1, 0])
                {
                    command = repository.FindCommandByCidAndPtimeAndClassIndex(classroom, DateTime.Now, e.Num);
                    isWork = 1;
                }
                else if (DateTime.Now > Config.Config.Courses[num - 1, 1])
                {
                    command = repository.FindCloseCommand(classroom);
                }

                Thr t = new Thr(classroom, num, isWork, sender);
                ThreadPool.QueueUserWorkItem(t.ThreadProc, command);

                //                int i = control.GetStatus(classroom.Id, DateTime.Now.ToString("yyyy-MM-dd"), num);
                //                string command = control.GetControlCommand(classroom.Id, DateTime.Now.ToString("yyyy-MM-dd"), num);
                //                Thr t = new Thr(classroom, num, sender, i);
                //                ThreadPool.QueueUserWorkItem(t.ThreadProc, command);
            }
        }
    }

    internal class Thr
    {
        private delegate void ModifySatus(string classId, int color);
        private int num; //第几节课
        private string Ip;
        private ushort port;
        private PlanPage window;
        private string name; //教室名称
        ModifySatus modify = null;
        private int isWork = 0;
        public Thr(Classroom classroom, int num, int isWork, object UI)
        {
            Ip = classroom.Ip;
            port = classroom.Port;
            this.num = num - 1;
            window = (PlanPage)UI;
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
            Client client = new Client(Ip, port);
            //            client.Sendmessage(state.ToString());
            modify("btn_" + name + "_" + (num + 1), isWork);
        }
    }
}