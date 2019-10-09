using System;
using System.Collections.Generic;
using System.Text;

namespace POSOINV.Models
{
    public class SO_Creation_Model
    {
        public int idSODetail { get; set; }
        public int idSOHeader { get; set; }
        public int idCustomer { get; set; }
        public string SO_Number { get; set; }
        public string Order_Date { get; set; }
        public string Due_Date { get; set; }
        public string Salesman { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_PO { get; set; }
        public decimal Freight_Charges { get; set; }
        public decimal Other_Charges { get; set; }
        public string idPOHeader { get; set; }
        public string Customer_Code { get; set; }
        public string credit_term { get; set; }
        public decimal Gross_Amount { get; set; }
        public decimal Net_Amount { get; set; }
        public decimal Tax_Amount { get; set; }
        public string idSite { get; set; }
        public string Remarks { get; set; }
        public string Currency_Code { get; set; }
        public decimal Final_Discount { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Special_Concession { get; set; }
        public string Company_Name { get; set; }
        public string SO_Status { get; set; }
        public string Stock_Status { get; set; }
        public string AddressShipping1 { get; set; }
        public string AddressShipping2 { get; set; }
        public string AddressShipping3 { get; set; }
        public string AddressShipping4 { get; set; }

        public int idItem { get; set; }
        public int idClass { get; set; }
        public string Product_Name { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public int DaysOpen { get; set; }
        public string CreatedBy { get; set; }

        public string Pick_Status { get; set; }
        public string End_User { get; set; }
        public string End_User_City { get; set; }

    }
}
