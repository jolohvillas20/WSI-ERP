using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Models
{
    public class Pick_Header_Model
    {
        public int idPickHeader { get; set; }
        public int idSOHeader { get; set; }
        public string Pick_Number { get; set; }
        public string CustPONum { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DueDate { get; set; }
        public string CustomerCode { get; set; }
        public string user_id_chg_by { get; set; }
        public DateTime Pick_Date { get; set; }
    }
}
