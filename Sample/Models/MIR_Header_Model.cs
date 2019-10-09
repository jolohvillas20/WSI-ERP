using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Models
{
    public class MIR_Header_Model
    {
        public int idMIRHeader { get; set; }
        public string RequestNo { get; set; }
        public string Requestor { get; set; }
        public string ReferenceNo { get; set; }
        public string POCMNumber { get; set; }
        public DateTime RequestDate { get; set; }
        public string PreparedBy { get; set; }
        public string Remarks { get; set; }
    }
}
