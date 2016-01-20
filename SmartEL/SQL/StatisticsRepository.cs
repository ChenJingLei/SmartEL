using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SmartEL.Model;

namespace SmartEL.SQL
{
    class StatisticsRepository
    {
        private SqLcon sql;

        public StatisticsRepository()
        {
            sql = new SqLcon();
        }

        public List<Statistics> FindStatisticsByTimeAndName(string[] time, string name)
        {
            List<Statistics> list = new List<Statistics>();
            DataRowCollection dr = sql.Query(@"SELECT * FROM `statistics`,`classroom` WHERE classroom.Cid= '" + name + "' AND statistics.Cid = classroom.Cid AND `date` BETWEEN '" + time[0] + "' AND '" + time[1] + "' ", "statistics");
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

        public void SaveStatisticsData(Statistics statistics)
        {
            string sqlcommand = "INSERT INTO statistics(Cid, date, temperature, humidity) VALUE(@classroomId, @date, @temperature, @humidity)";
            MySqlParameter ClassRoomdId = new MySqlParameter("@classroomId", MySqlDbType.VarChar);
            ClassRoomdId.Value = statistics.ClassroomId;
            MySqlParameter Date = new MySqlParameter("@date", MySqlDbType.DateTime);
            Date.Value = statistics.Date.ToString("yyyy-MM-dd HH:mm:ss");
            MySqlParameter Temperature = new MySqlParameter("@temperature", MySqlDbType.Double);
            Temperature.Value = statistics.Temperature;
            MySqlParameter Humidity = new MySqlParameter("@humidity", MySqlDbType.Double);
            Humidity.Value = statistics.Humidity;
            MySqlCommand cmd = new MySqlCommand(sqlcommand,sql.Conn);
            cmd.Parameters.Add(ClassRoomdId);
            cmd.Parameters.Add(Date);
            cmd.Parameters.Add(Temperature);
            cmd.Parameters.Add(Humidity);
            try
            {
                sql.Conn.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                sql.Conn.Close();
            }
        }


    }
}
