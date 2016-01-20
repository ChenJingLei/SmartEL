using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elysium.Notifications;
using MySql.Data.MySqlClient;
using SmartEL.Model;

namespace SmartEL.SQL
{
    class PlanRepository
    {
        private SqLcon sql;

        public PlanRepository()
        {
            sql = new SqLcon();
        }

        public string FindCommandByCidAndPtimeAndClassIndex(Classroom c, DateTime ptime, int classindex)
        {
            MySqlCommand comm = new MySqlCommand("SELECT `device`.`Dname`,`plan`.`Pdvalues` FROM `plan`,`device`  WHERE `plan`.`Did`=`device`.`Did` AND  `Ptime` ='" + ptime.ToString("yyyy-MM-dd") + "' AND `Pclassindex` = " + classindex.ToString() + " AND `Cid` = " + c.Id, sql.Conn);
            string command = c.Name + "=";
            try
            {
                if (sql.Exists("plan", "cid", c.Id))
                {
                    sql.Conn.Open();
                    MySqlDataReader dr = comm.ExecuteReader();
                    while (dr.Read())
                    {
                        //"L01#0/L02#0/L03#0/T01#0/T02#0/H01#0/F01#0/F02#0/I01#0");
                        command += dr.GetString(0) + "#" + dr.GetString(1) + "/";
                    }
                    if (command.Length != 0)
                    {
                        command = command.Substring(0, command.Length - 1);
                    }
                    else
                    {
                        throw new Exception("课表计划未完整");
                    }
                }
                else
                {
                    throw new Exception("未计划");
                    //未计划
                }
            }
            finally
            {
                if (sql.Conn.State == ConnectionState.Open)
                {
                    sql.Conn.Close();
                }
            }
            return command;
        }

        public string FindCloseCommand(Classroom c)
        {
            string command = c.Name + "=";
            //查找当前教室设备
            try
            {
                if (sql.Exists("devicelist", "cid", c.Id))
                {
                    MySqlCommand comm = new MySqlCommand("SELECT `device`.`Dname` FROM `device` ,`devicelist` WHERE `device`.`Did` =`devicelist`.`Did` AND `devicelist`.`Cid`= @Cid",sql.Conn);
                    comm.Parameters.Add(new MySqlParameter("@Cid", MySqlDbType.Int32) {Value = c.Id});
                   sql.Conn.Open();
                    MySqlDataReader dr = comm.ExecuteReader();
                    while (dr.Read())
                    {
                        //"L01#0/L02#0/L03#0/T01#0/T02#0/H01#0/F01#0/F02#0/I01#0");
                        command += dr.GetString(0) + "#" + "0/";
                    }
                    if (command.Length != 0)
                    {
                        command = command.Substring(0, command.Length - 1);
                    }
                    else
                    {
                        throw new Exception("数据库错误");
                    }
                }
                else
                {
                    throw new Exception("无此教室设备信息");
                    //未计划
                }
            }
            finally
            {
                if (sql.Conn.State == ConnectionState.Open)
                {
                    sql.Conn.Close();
                }
            }
            //形成设备关闭命令

            return command;
        }
    }
}
