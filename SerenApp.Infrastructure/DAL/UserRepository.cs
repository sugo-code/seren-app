using Microsoft.EntityFrameworkCore;
using SerenApp.Core.Interfaces;
using SerenApp.Core.Model;
using SerenApp.Infrastructure.DAL.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Infrastructure.DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly AContextBase context;
        public UserRepository(AContextBase context)
        {
            this.context = context;
        }

        public async Task<User> Delete(User item)
        {
            context.Remove(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User> GetById(Guid id)
        {
            return await context.Users.FirstAsync(d => d.Id == id);
        }

        public async Task<User> Insert(User item)
        {
            await context.AddAsync(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<User> Update(User item)
        {
            context.Update(item);
            await context.SaveChangesAsync();
            return item;
        }
    }
}
