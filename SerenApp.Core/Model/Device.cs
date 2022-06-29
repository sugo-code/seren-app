using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Core.Model
{
    public class Device : AEntityBase<Guid>
    {
        public string FirmwareVersion { get; set; }
        public string Name { get; set; }
    }
}
