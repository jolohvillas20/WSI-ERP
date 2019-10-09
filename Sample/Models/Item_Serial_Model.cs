using System;
using System.Collections.Generic;
using System.Text;

namespace POSOINV.Models
{
   public class Item_Serial_Model
    {
        public int idSerial { get; set; }
        public int idItem { get; set; }
        public string Serial_No { get; set; }
        public string PO_Number { get; set; }
        public DateTime timestamp { get; set; }
        public decimal Unit_Cost { get; set; }
        public string Unit_Comp { get; set; }
        public string user_change { get; set; }
        public string InStock { get; set; }
    }
}
