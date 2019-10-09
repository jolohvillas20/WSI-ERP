using System;
using System.Collections.Generic;
using System.Text;

namespace POSOINV.Models
{
    public class SO_Header_Model
    {
        public int idSOHeader { get; set; }
        public string SO_Number { get; set; }
        public int idPOHeader { get; set; }
        public DateTime Order_Date { get; set; }
        public DateTime Due_Date { get; set; }
        public int idCustomer { get; set; }
        public string Customer_PO { get; set; }
        public string Salesman { get; set; }
        public int Ship_Code { get; set; }
        public decimal Gross_Amount { get; set; }
        public decimal Final_Discount { get; set; }
        public decimal Freight_Charges { get; set; }
        public decimal Other_Charges { get; set; }
        public decimal Net_Amount { get; set; }
        public decimal Tax_Amount { get; set; }
        public string credit_term { get; set; }
        public int idSite { get; set; }
        public string Remarks { get; set; }
        public string currency_code { get; set; }
        public string Pick_Status { get; set; }
        public string Special_Concession { get; set; }
        public string transaction_ID { get; set; }
        public string SO_Status { get; set; }
        public string Stock_Status { get; set; }
        public string CreatedBy { get; set; }
        public string End_User { get; set; }
        public string End_User_City { get; set; }
        public decimal Forex_Rate { get; set; }
    }
}
