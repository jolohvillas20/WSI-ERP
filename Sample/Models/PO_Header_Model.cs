using System;
using System.Collections.Generic;
using System.Text;

namespace POSOINV.Models
{
    public class PO_Header_Model
    {
        public int idPOHeader { get; set; }
        public string PO_Number { get; set; }
        public DateTime PO_Date { get; set; }
        public DateTime Delivery_Date { get; set; }
        public string Terms { get; set; }
        public string FOB_Point { get; set; }
        public string Shipping_Via { get; set; }
        public string Currency { get; set; }
        public int idSupplier { get; set; }
        public int PO_Quantity { get; set; }
        public decimal PO_Total  { get; set; }
        public decimal Total_Charges { get; set; }
        public decimal Forex_Rate { get; set; }
        public string PR_Number { get; set; }
        public string Created_By { get; set; }
        public string Remarks { get; set; }
        public string POStatus { get; set; }
        public string ImportShipmentNumber { get; set; }        
    }
}
