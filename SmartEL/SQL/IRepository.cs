using System.Collections.Generic;

namespace SmartEL.SQL
{
    public interface IRepository<S,T>
    {
        List<S> FindAll();

        S FindOne(T t);

        void Save(S s);
    }
}