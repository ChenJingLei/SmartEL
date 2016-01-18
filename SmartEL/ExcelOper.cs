using System;
using System.Data;
using System.Data.OleDb;

namespace SmartEL
{
    class ExcelOper
    {

        /// <summary>
        /// Excel数据导入Datable
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public System.Data.DataTable GetExcelDatatable(string fileUrl, string table)
        {
            //office2007之前 仅支持.xls
            //const string cmdText = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;IMEX=1';";
            //支持.xls和.xlsx，即包括office2010等版本的   HDR=Yes代表第一行是标题，不是数据；
            const string cmdText =
                "Provider=Microsoft.Ace.OleDb.12.0;Data Source={0};Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";
            System.Data.DataTable dt = null;
            //建立连接
            OleDbConnection conn = new OleDbConnection(string.Format(cmdText, fileUrl));
            try
            {
                //打开连接
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                System.Data.DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //获取Excel的第一个Sheet名称
                string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString().Trim();
                //查询sheet中的数据
                //string strSql = "select distinct 上课时间和地点 from [" + sheetName + "]";
                string strSql = "select * from [" + sheetName + "]";
                OleDbDataAdapter da = new OleDbDataAdapter(strSql, conn);
                DataSet ds = new DataSet();
                da.Fill(ds, table);
                dt = ds.Tables[0];
                return dt;
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
