using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Models
{
    public class Return_Header_Model
    {
        public int idReturnHeader { get; set; }
        public string SLR_Number { get; set; }
        public string SO_Number { get; set; }
        public string Customer_Code { get; set; }
        public string Auth_Number { get; set; }
        public string isReplacement { get; set; }
        public string Document_Number { get; set; }
        public string Site { get; set; }
        public string Reason_Code { get; set; }
        public DateTime Date_Returned { get; set; }
        public string Remarks { get; set; }
    }
}
