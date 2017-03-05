using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billingg.Database
{
    public class Procurement:Basic
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Document { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        [NotMapped]
        public double Total
        {
            get
            {
                return Quantity * Price;
            }
        }

        public virtual Supplier Supplier { get; set;}
        public virtual Product Product { get; set; }
    }
}
