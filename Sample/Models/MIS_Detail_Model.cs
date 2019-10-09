using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Models
{
    public class MIS_Detail_Model
    {
        public int idMISDetail  { get; set; }
        public int idMISHeader { get; set; }
        public int idItem { get; set; }
        public int Quantity { get; set; }
        public decimal Cost{ get; set; }

    }
}
