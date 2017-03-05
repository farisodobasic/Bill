using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billingg.Database
{
    public class Product:Basic
    {
        public Product()
        {
            Items = new List<Item>();
            Procurements = new List<Procurement>();
            Stock = new Stock();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }

        public virtual Category Category { get; set; }
        public virtual List<Item>Items { get; set; }
        public virtual List<Procurement> Procurements { get; set; }
        public virtual Stock Stock { get; set; }
    }
}
