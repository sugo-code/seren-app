using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Core.Model
{
    public class Admin : AEntityBase<Guid>
    {
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}
