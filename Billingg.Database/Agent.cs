using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billingg.Database
{
    public class Agent:Basic
    {
        public Agent()
        {
            Invoices = new List<Invoice>();
            Towns = new List<Town>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Invoice> Invoices { get; set; }
        public virtual List<Town> Towns { get; set; }
    }
}
