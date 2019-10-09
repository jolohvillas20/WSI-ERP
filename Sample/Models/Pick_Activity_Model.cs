using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Models
{
   public  class Pick_Activity_Model
    {
        public string SONumber { get; set; }
        public string ItemNumber { get; set; }
        public string UM { get; set; }
        public string CustCode { get; set; }
        public string QtyDelivered { get; set; }
        public string Site { get; set; }
        public string UserId { get; set; }
        public string PickListNo { get; set; }
        public string PickListDate { get; set; }
        public string Address { get; set; }
        public string SerialNumber { get; set; }
        public string ItemDescription { get; set; }
        public string CustomerName { get; set; }
        public string ExternalComments { get; set; }
        public string SiteDescription { get; set; }
    }
}
