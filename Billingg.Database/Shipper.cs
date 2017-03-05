using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billingg.Database
{
    public class Shipper:Basic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        
        public virtual List<Invoice> Invoices { get; set; }
        public virtual Town Town { get; set; }
    }
}
