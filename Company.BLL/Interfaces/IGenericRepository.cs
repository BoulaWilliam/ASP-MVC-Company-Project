using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task <IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task Add(T item);
        void Delete(T item);
        void Update(T item);
    }
}
