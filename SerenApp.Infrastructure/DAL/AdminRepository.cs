using Microsoft.EntityFrameworkCore;
using SerenApp.Core.Interfaces;
using SerenApp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Infrastructure.DAL
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext context;
        public AdminRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Admin> Delete(Admin item)
        {
            context.Remove(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Admin>> GetAll()
        {
            return await context.Admins.ToListAsync();
        }

        public async Task<Admin> GetById(Guid id)
        {
            return await context.Admins.FirstAsync(d => d.ID == id);
        }

        public async Task<Admin> Insert(Admin item)
        {
            await context.AddAsync(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<Admin> Update(Admin item)
        {
            context.Update(item);
            await context.SaveChangesAsync();
            return item;
        }
    }
}
