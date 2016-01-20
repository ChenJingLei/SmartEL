using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartEL.Config
{
    class FileLog
    {
        //指定日志文件的目录
        private String path;

        public FileLog(string path)
        {
            this.path = path;
        }

        public void WriteLogFile(string input)
        {
            //定义文件信息对象
            FileInfo finfo = new FileInfo(path);
            //判断文件是否存在以及是否大于2K
            if (finfo.Exists && finfo.Length>2048)
            {
                finfo.Delete();
            }
            //创建只写文件流
            using (FileStream fs = finfo.OpenWrite())
            {
                //根据上面创建的文件流创建写数据流
                StreamWriter sw = new StreamWriter(fs);
                //设置写数据流的起始位置为文件流的末尾
                sw.BaseStream.Seek(0, SeekOrigin.End);
                //写入“Log Entry : ”
                sw.Write("\nLog Entry : ");
                //写入当前系统时间并换行
                sw.Write("{0} {1} \r\n", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
                //写入日志内容并换行
                sw.Write(input + "\r\n");        
                //写入------------------------------------“并换行
                sw.Write("------------------------------------\r\n");        
                //清空缓冲区内容，并把缓冲区内容写入基础流
                sw.Flush();       
                //关闭写数据流
                sw.Close();
            }
        }
    }
}
