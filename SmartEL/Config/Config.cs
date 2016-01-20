using System;

namespace SmartEL.Config
{
    public static class Config
    {
        public static DateTime[,] Courses =
        {
            {
                new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,8,20,0), new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,9,55,0)
            },
            {
                new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,10,15,0), new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,11,50,0)
            },
            {
                new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,13,00,0), new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,14,35,0)
            },
            {
                new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,14,55,0), new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,16,30,0)
            },
            {
                new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,18,00,0), new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,20,30,0)
            },
        };

        public static double[,] course = { { 08.10, 10.0 }, { 10.05, 11.55 }, { 12.50, 14.40 }, { 14.45, 16.35 }, { 17.50, 20.35 } };
        public static int[] chcektime = { 0, 0, 1 };


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
