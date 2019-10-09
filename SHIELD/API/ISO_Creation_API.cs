using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SOPOINV.API
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISO_Creation_API" in both code and config file together.
    [ServiceContract]
    public interface ISO_Creation_API
    {
        [OperationContract]
        byte[] DoWork(string x, string y, string z);
    }

    [DataContract]
    public class SO_Create_Model
    {
        [DataMember]
        public string strSONumber { get; set; }
        public string strOrderDate { get; set; }
        public string strDueDate { get; set; }
        public string strCustomerPOnum { get; set; }
        public string strSalesman { get; set; }
        public string strCreditTerm { get; set; }
        public string strOtherCharges { get; set; }
        public decimal strGrossAmount { get; set; }
        public decimal strFinalDiscount { get; set; }
        public double strCharges { get; set; }
        public decimal strNetAmount { get; set; }
        public string strCustomerCode { get; set; }
        public string strAddress1 { get; set; }
        public string strAddress2 { get; set; }
        public string strAddress3 { get; set; }
        public string strAddress4 { get; set; }
        public string strRemarks { get; set; }
        public string strCompanyName { get; set; }
        public decimal strOutputVat { get; set; }
        public decimal strTaxableSales { get; set; }
        public string strShipAddress1 { get; set; }
        public string strShipAddress2 { get; set; }
        public string strShipAddress3 { get; set; }
        public string strShipAddress4 { get; set; }
    }
}
