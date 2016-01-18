using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ClassRoomPlan
{
    public class ClassRoomPlanControl
    {
        private ClassRoomPlanService service;

        public ClassRoomPlanControl()
        {
            service = new ClassRoomPlanService();
        }

        public List<Classroom> GetAllClassrooms()
        {
            return service.GetAllClassroomsIsNull();
        }

        public int GetStatus(string id, string time, int num)
        {
            return service.GetStatusByClassRoomIdAndDate(id, time, num);
        }

        public void StatisticsData(Statistics statistics)
        {
            service.SetStatisticsData(statistics);
        }

        public List<Statistics> AnalystData(string[] time, string name)
        {
            return service.GetStatisticsByTimeAndName(time, name);
        }

        public void SetRoomPlan(string classroomId, string field, string value, string date)
        {
            service.UpdatePlanControl(classroomId, field, value, date);
        }

        private List<RoomControl> temp = new List<RoomControl>();
        public string GetControlCommand(string classroomId, string date, int num)
        {
            bool isExists = false;
            RoomControl destControl = default(RoomControl);
            foreach (RoomControl control in temp)
            {
                if (control.ClassroomId.Equals(classroomId) && control.Date.ToString("yyyy-MM-dd").Equals(date))
                {
                    isExists = true;
                    destControl = control;
                    break;
                }
            }
            if (!isExists)
            {
                destControl = service.GetRoomControlByIdAndDate(classroomId, date);
                temp.Add(destControl);
                
            }
            string command = "L01#0/L02#0/L03#0/T01#0.0/T02#0.0/H01#0.0/F01#0/F02#0/I01#0.0";
            switch (num)
            {
                case 1:
                    command = destControl.Plan12;
                    break;
                case 2:
                    command = destControl.Plan34;
                    break;
                case 3:
                    command = destControl.Plan56;
                    break;
                case 4:
                    command = destControl.Plan78;
                    break;
                case 5:
                    command = destControl.Plan912;
                    break;
            }
            return command;
        }

    }
}
