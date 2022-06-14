using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerenApp.Core.Model
{
    public abstract class AEntityBase<T>
    {
        public T Id { get; set; }
    }
}
