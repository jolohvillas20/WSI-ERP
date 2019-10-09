using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Models
{
    public class MIR_Detail_Model
    {
        public int idMIRDetail  { get; set; }
        public int idMIRHeader { get; set; }
        public int idItem { get; set; }
        public int Quantity { get; set; }
        public decimal Cost{ get; set; }

    }
}
