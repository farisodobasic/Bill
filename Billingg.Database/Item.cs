using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billingg.Database
{
    public class Item:Basic
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        [NotMapped]
        public double SubTotal
        {
            get
            {
                return Quantity * Price;
            }
        }

        public virtual Invoice Invoice { get; set; }
        public virtual Product Product { get; set; }

    }
}
