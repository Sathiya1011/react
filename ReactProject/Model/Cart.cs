using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReactProject.Model
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public string UserName { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 0;
    }
}
