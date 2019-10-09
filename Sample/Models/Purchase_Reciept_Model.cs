using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Models
{
  public  class Purchase_Reciept_Model
    {
        public string Item_Number { get; set; }
        public string Description { get; set; }
        public string Recieved_Qty { get; set; }
        public string UM { get; set; }
        public string Grade { get; set; }
        public string ForQC { get; set; }
        public string Serial_No { get; set; }       
    }
}
