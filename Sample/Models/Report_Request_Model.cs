using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Models
{
    public class Report_Request_Model
    {
        public int idReportRequest { get; set; }
        public string Request_Type { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string SendToEmail { get; set; }
        public string isFinished { get; set; }
        public DateTime TimeStamp { get; set; }
        public string CreatedBy { get; set; }
    }
}
