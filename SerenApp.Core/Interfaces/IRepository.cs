using SerenApp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Core.Interfaces
{
    public interface IRepository<T,K> where T: AEntityBase<K>
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<T> GetById(K id);
        public Task<T> Insert(T item);
        public Task<T> Update(T item);
        public Task<T> Delete(T item);
    }
}
