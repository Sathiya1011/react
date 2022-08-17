using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReactProject.Model
{
    public class ElectronicProducts
    {
        [Key]
        public int productId { get; set; }
        public string productname { get; set; }
        public string brand { get; set; }
        public int availablecount { get; set; }
        public int price { get; set; }

       
    }
}
