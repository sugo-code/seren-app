using SerenApp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Core.Interfaces
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        public Task<User> GetByPhoneNumber(string phoneNumber);
    }
}
