using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartEL.Model;

namespace SmartEL.SQL
{
    class ClassRoomRepository:IRepository<Classroom,String>
    {
        private SqLcon sql;
        public ClassRoomRepository()
        {
            sql = new SqLcon();
        }

        public List<Classroom> FindAll()
        {
            List<Classroom> list = new List<Classroom>();
            DataRowCollection dr = sql.Query(@"SELECT * FROM `classroom` WHERE `Cip` IS NOT NULL AND `Cport` IS NOT NULL ", "ClassRooM");
            foreach (DataRow dataRow in dr)
            {
                Classroom classroom = new Classroom
                {
                    Id = dataRow["Cid"].ToString(),
                    Name = dataRow["Cname"].ToString(),
                    Ip = dataRow["Cip"].ToString(),
                    Port = ushort.Parse(dataRow["Cport"].ToString())
                };
                list.Add(classroom);
            }
            return list;
        }

        public Classroom FindOne(string t)
        {
            throw new NotImplementedException();
        }

        public void Save(Classroom s)
        {
//            if (sql.Exists("classroom", s.Id))
//            {
//
//            }
//            else
//            {
//                
//            }

            throw new NotImplementedException();
        }
    }
}
