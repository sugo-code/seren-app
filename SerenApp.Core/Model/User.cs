using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Core.Model
{
    public class User : AEntityBase<Guid>
    {
        //public string MailAddress { get; set; }
        public byte[] PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string[] AlertPhoneNumbers { get; set; }
        public Device[] Devices { get; set; }
    }
}
