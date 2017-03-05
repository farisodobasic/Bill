using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billingg.Database
{
    public class Supplier:Basic
    {
        public Supplier()
        {
            Procurements = new List<Procurement>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public virtual Town Town { get; set; }
        public virtual List<Procurement> Procurements { get; set; }
    }
}
