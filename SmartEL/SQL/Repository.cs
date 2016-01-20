using System;
using System.Collections.Generic;
using System.Data;
using ClassRoomPlan;
using SmartEL.SQL;


namespace SmartEL.SQL
{
    public abstract class Repository<S,T> : IRepository<S, T>
    {
        protected SqLcon sql;

        protected Repository()
        {
            sql  = new SqLcon();
        }

        public abstract List<S> FindAll();

        public abstract S FindOne(T t);

        public abstract void Save(S s);
    }
}