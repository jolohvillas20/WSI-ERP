using System;
using System.Collections.Generic;
using System.Text;

namespace POSOINV.Models
{
    public class SO_Detail_Model
    {
        public int idSODetail { get; set; }
        public int idSOHeader { get; set; }
        public int idItem { get; set; }
        public decimal Qty { get; set; }
        public decimal Cost { get; set; }
        public string UM { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax_Amount { get; set; }
    }
}
