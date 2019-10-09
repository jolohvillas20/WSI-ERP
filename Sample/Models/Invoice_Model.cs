using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Models
{
    public class Invoice_Model
    {
        public int idInvoice { get; set; }
        public int idSOHeader { get; set; }
        public string Invoice_Number { get; set; }
        public string DR_Number { get; set; }
        public DateTime Del_Date { get; set; }
        public DateTime Invoice_Date { get; set; }
        public decimal Amount { get; set; }
        public string OR_Number { get; set; }
        
    }
}
