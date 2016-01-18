using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassRoomPlan
{
    public class Config
    {

        public static double[,] course = { {08.10, 10.0 }, { 10.05, 11.55 }, { 12.50, 14.40 }, { 14.45, 16.35 }, { 17.50, 20.35 } };
        public static int[] chcektime = { 0, 1, 0 };
        //        public static string chcektime = "0:1:0";
        public static string dbserveraddress = "cjl2020cjl.mysql.rds.aliyuncs.com";
        public static string dbport = "3306";
        public static string dbuserID = "cjl2020";
        public static string dbpassword = "39350178";
        public static int onclasstime = 10;
        public static int downclasstime = 5;

        //        public static double[] one = { 8.10, 10.00 };
        //        public static double[] two = { 10.05, 12.00 };
        //        public static double[] three = { 12.50, 14.40 };
        //        public static double[] four = { 14.45, 16.35 };
        //        public static double[] five = { 17.50, 20.35 };
    }
}
