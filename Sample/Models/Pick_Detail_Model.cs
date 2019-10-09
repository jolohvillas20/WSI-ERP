using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Models
{
   public class Pick_Detail_Model
    {
        public int idPickDetail { get; set; }
        public int idPickHeader { get; set; }
        public int idItem { get; set; }
        public string Item_Number { get; set; }
        public string Description { get; set; }        
        public int Qty { get; set; }
        public int Items_Picked { get; set; }
    }
}
