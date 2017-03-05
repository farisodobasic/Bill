using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billingg.Database
{
    public class Stock:Basic
    {
        public int Id { get; set; }
        public int Input { get; set; }
        public int Output { get; set; }
        [NotMapped]
        public int Inventory
        {
            get
            {
                return Input - Output;
            }
        }
        [Required]
       public virtual Product Product { get; set; }
    }
}
