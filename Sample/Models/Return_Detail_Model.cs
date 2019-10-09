using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Models
{
    public class Return_Detail_Model
    {
        public int idReturnDetail { get; set; }
        public int idReturnHeader { get; set; }
        public int idItem { get; set; }
        public int Returned_Qty { get; set; }
        public decimal Returned_Cost { get; set; }
    }
}
