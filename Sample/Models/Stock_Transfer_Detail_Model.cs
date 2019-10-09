using System;
using System.Collections.Generic;
using System.Text;

namespace POSOINV.Models
{
    public class Stock_Transfer_Detail_Model
    {
        public int idSTDetail { get; set; }
        public int idSTHeader { get; set; }
        public int idItem { get; set; }
        public int Qty { get; set; }
        //public int idSerial { get; set; }
    }
}
