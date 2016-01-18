using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ClassRoomPlan
{
    class ClassRoomPlanService
    {
        private SqLcon sqLcon;

        public ClassRoomPlanService()
        {
            sqLcon = new SqLcon();
        }

        public int GetStatusByClassRoomIdAndDate(string id, string time, int num)
        {
            string s = "*";
            switch (num)
            {
                case 1:
                    s = "Plan12";
                    break;
                case 2:
                    s = "Plan34";
                    break;
                case 3:
                    s = "Plan56";
                    break;
                case 4:
                    s = "Plan78";
                    break;
                case 5:
                    s = "Plan912";
                    break;
            }
            DataRowCollection dr = sqLcon.Query(@"SELECT " + s + " FROM `classroom` ,`plan` ,`smartcp` WHERE classroom.ClassroomId='" + id + "' AND classroom.ClassroomId = smartcp.ClassroomId AND plan.PlanId = smartcp.PlanId AND `date` BETWEEN '" + time + "' AND '" + time + "' ", "plan");
            if (dr.Count < 1)
            {
                return 0;
            }
            return int.Parse(dr[0][0].ToString());
        }

        public List<Classroom> GetAllClassroomsIsNull()
        {
            List<Classroom> list = new List<Classroom>();
            DataRowCollection dr = sqLcon.Query(@"SELECT * FROM `classroom` WHERE `Ip` IS NOT NULL AND `Port` IS NOT NULL ", "ClassRooM");
            foreach (DataRow dataRow in dr)
            {
                Classroom classroom = new Classroom();
                classroom.Id = dataRow["ClassroomId"].ToString();
                classroom.Name = dataRow["ClassroomName"].ToString();
                classroom.Ip = dataRow["Ip"].ToString();
                classroom.Port = ushort.Parse(dataRow["Port"].ToString());
                list.Add(classroom);
            }
            return list;
        }

        public List<Plan> getClassPlan(string[] time)
        {
            List<Plan> list = new List<Plan>();
            DataRowCollection dr = sqLcon.Query(@"SELECT * FROM `classroom` ,`plan` ,`smartcp` WHERE classroom.ClassroomId = smartcp.ClassroomId AND plan.PlanId = smartcp.PlanId AND `date` BETWEEN '" + time[0] + "' AND '" + time[1] + "' ", "plan");
            foreach (DataRow dataRow in dr)
            {
                Plan plan = new Plan();
                Classroom room = new Classroom();
                room.Id = dataRow["ClassroomId"].ToString();
                room.Name = dataRow["ClassroomName"].ToString();
                room.Ip = dataRow["Ip"].ToString();
                room.Port = ushort.Parse(dataRow["Port"].ToString());

                plan.Room = room;
                plan.Time = DateTime.Parse(dataRow["date"].ToString());
                plan.One = int.Parse(dataRow["Plan12"].ToString());
                plan.Two = int.Parse(dataRow["Plan34"].ToString());
                plan.Three = int.Parse(dataRow["Plan56"].ToString());
                plan.Four = int.Parse(dataRow["Plan78"].ToString());
                plan.Five = int.Parse(dataRow["Plan912"].ToString());

                list.Add(plan);
            }
            return list;
        }

        public List<Statistics> GetStatisticsByTimeAndName(string[] time,string name)
        {
            List<Statistics> list = new List<Statistics>();
            DataRowCollection dr = sqLcon.Query(@"SELECT * FROM `statistics`,`classroom` WHERE classroom.ClassroomName= '"+ name +"' AND statistics.ClassroomId = classroom.ClassroomId AND `date` BETWEEN '" + time[0] + "' AND '" + time[1] + "' ", "statistics");
            foreach (DataRow dataRow in dr)
            {
                Statistics statistics = new Statistics();
                statistics.Id = Convert.ToInt32(dataRow["Id"].ToString());
                statistics.ClassroomId = dataRow["ClassroomId"].ToString();
                statistics.ClassroomName = dataRow["ClassroomName"].ToString();
                statistics.Date = DateTime.Parse(dataRow["date"].ToString());
                statistics.Temperature = Convert.ToDouble(dataRow["temperature"].ToString());
                statistics.Humidity = Convert.ToDouble(dataRow["humidity"].ToString());
                list.Add(statistics);
            }
            return list;
        }

        private void SetPlanControl(RoomControl control)
        {
            
        }

        public void UpdatePlanControl(string classroomId,string field, string value,string date)
        {
            string sqlcommand = "UPDATE roomplan SET "+field+" = '"+value+ "'  WHERE ClassRoomId = @classroomId AND date = @date";
            MySqlParameter ClassRoomdId = new MySqlParameter("@classroomId", MySqlDbType.VarChar);
            ClassRoomdId.Value = classroomId;
            MySqlParameter Date = new MySqlParameter("@date", MySqlDbType.DateTime);
            Date.Value = date;
            MySqlCommand cmd = new MySqlCommand(sqlcommand);
            sqLcon.Oper(cmd);
        }

        public RoomControl GetRoomControlByIdAndDate(string classroomId, string date)
        {
            DataRowCollection dr = sqLcon.Query(@"SELECT * FROM roomplan WHERE ClassRoomId = '"+ classroomId +"' AND date =  '"+ date +"'", "plan");
            RoomControl roomControl = new RoomControl();
            foreach (DataRow dataRow in dr)
            {
                roomControl.ClassroomId = dataRow["ClassroomId"].ToString();
                roomControl.Date = DateTime.Parse(dataRow["date"].ToString());
                roomControl.Plan12 = dataRow["Plan12"].ToString();
                roomControl.Plan34 = dataRow["Plan34"].ToString();
                roomControl.Plan56 = dataRow["Plan56"].ToString();
                roomControl.Plan78 = dataRow["Plan78"].ToString();
                roomControl.Plan912 = dataRow["Plan912"].ToString();
            }
            return roomControl;
        }


        public void SetStatisticsData(Statistics statistics)
        {
            //"insert into statistics value('', '11', '2015-11-21 21:28:44', 11, 11)";
            string sqlcommand = "INSERT INTO statistics(ClassroomId, date, temperature, humidity) VALUE(@classroomId, @date, @temperature, @humidity)";
            //            string sqlcommand = "insert into processedpapers(dc_ID,dc_title,dc_titleTokens,dc_descriptionTokens,dc_len,dc_description)values(@mydcid,@dmydctitle,@mydctitletokens,@mydcdescriptiontokens,@mydclen,@mydcdescription)";
            MySqlParameter ClassRoomdId = new MySqlParameter("@classroomId", MySqlDbType.VarChar);
            ClassRoomdId.Value = statistics.ClassroomId;
            MySqlParameter Date = new MySqlParameter("@date", MySqlDbType.DateTime);
            Date.Value = statistics.Date.ToString("yyyy-MM-dd HH:mm:ss");
            MySqlParameter Temperature = new MySqlParameter("@temperature", MySqlDbType.Double);
            Temperature.Value = statistics.Temperature;
            MySqlParameter Humidity = new MySqlParameter("@humidity", MySqlDbType.Double);
            Humidity.Value = statistics.Humidity;
            MySqlCommand cmd = new MySqlCommand(sqlcommand);
            cmd.Parameters.Add(ClassRoomdId);
            cmd.Parameters.Add(Date);
            cmd.Parameters.Add(Temperature);
            cmd.Parameters.Add(Humidity);
            sqLcon.Oper(cmd);
        }
    }
}
