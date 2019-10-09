using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Models
{
    public class Pick_Serial_Model
    {
        public int idPickSerial { get; set; }
        public int idPickDetail { get; set; }
        public int idSerial { get; set; }
        public string Serial_No { get; set; }
        public string PO_Number { get; set; }
    }
}
