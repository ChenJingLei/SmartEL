using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace ClassRoomPlan
{
    public class SqLcon
    {
//        private static string serverName = "cjl2020cjl.mysql.rds.aliyuncs.com";
        
        //        private static string serverName = "localhost";
//        static string port = "3306";
        static string db = "smartel";
        //        static string userID = "xue1";
        //        static string password = "xuefeng2015.";
//        static string userID = "cjl2020";
//        static string password = "39350178";
        string mysqlcon = "Server=" + Config.dbserveraddress + ";Port=" + Config.dbport + ";Database=" + db + ";Uid=" + Config.dbuserID + ";password=" + Config.dbpassword;

        private DataSet dsall;
        private MySqlConnection conn;
        private MySqlDataAdapter mdap;

        public SqLcon()
        {
            conn = new MySqlConnection(mysqlcon);
        }


        public List<Classroom> select()
        {
            List<Classroom> classrooms = new List<Classroom>();
            //            classrooms.Add(new Classroom("1000", "A101", 1, 0, 1, 1, 1));
            //            classrooms.Add(new Classroom("1001", "E101", 0, 1, 1, 1, 1));
            return classrooms;
        }

        public DataRowCollection Query(string command, string tableName)
        {
            DataRowCollection dr = null;
            try
            {
                // MySqlCommand cmd = new MySqlCommand("select ");
                mdap = new MySqlDataAdapter(command, conn);
                dsall = new DataSet();
                mdap.Fill(dsall, tableName);
                DataTable dataTable = dsall.Tables[tableName];
                //                mdap.Fill(dsall, "plan");
                //                DataTable dataTable = dsall.Tables["plan"];
                dr = dataTable.Rows;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dr;
        }

        public int Oper(MySqlCommand cmd)
        {
            int result = 0;
            cmd.Connection = conn;
            conn.Open();
            try
            {
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
    }
}
