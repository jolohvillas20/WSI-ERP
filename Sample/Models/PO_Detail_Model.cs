using System;
using System.Collections.Generic;
using System.Text;

namespace POSOINV.Models
{
    public class PO_Detail_Model
    {
        public int idPODetail { get; set; }
        public int idPOHeader { get; set; }
        public int idItem { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public decimal Amount { get; set; }
        public decimal Final_Cost { get; set; }
        public string Unit_Comp { get; set; }
        public string isReceived { get; set; }
        public decimal Partial_Remaining { get; set; }
    }
}
