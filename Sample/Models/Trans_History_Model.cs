using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Models
{
    public class Trans_History_Model
    {
        public int idTransHist { get; set; }
        public string Trans_Code { get; set; }
        public string Item_Number { get; set; }
        public string Site { get; set; }
        public string UM { get; set; }
        public string Doc_No { get; set; }
        public string Serial_No { get; set; }
        public string Reason_Code { get; set; }
        public DateTime Trans_Date { get; set; }
        public string Order_No { get; set; }
        public string Invoice_No { get; set; }
        public string Reference_No { get; set; }
        public int Trans_Qty { get; set; }
        public decimal Trans_Amt { get; set; }
        public string Remarks { get; set; }
        public string user_domain { get; set; }
    }
}
