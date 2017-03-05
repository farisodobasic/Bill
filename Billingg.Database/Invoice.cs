using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billingg.Database
{
    public class Invoice:Basic
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime Date { get; set; }
        public DateTime ShippedOn { get; set; }
        [NotMapped]
        public double SubTotal
        {
            get
            {
                double sum = 0;
                foreach(Item it in Items)
                {
                    sum += it.SubTotal;
                }
                return sum;
            }
        }
        public double Vat { get; set; }
        [NotMapped]
        public double VatAmount
        {
            get
            {
                return (SubTotal * Vat / 100);
            }
        }
        public double Shipping { get; set; }
        [NotMapped]
        public double Total
        {
            get
            {
                return Shipping + VatAmount + SubTotal;
            }
        }

        public virtual Customer Customer { get; set; }
        public virtual Shipper Shipper { get; set; }
        public virtual Agent Agent { get; set; }
        public virtual List<Item> Items { get; set; }


    }
}
