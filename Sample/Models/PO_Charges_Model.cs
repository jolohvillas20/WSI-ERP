using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Models
{
    public class PO_Charges_Model
    {
        public int idPOCharges { get; set; }
        public int idPOHeader { get; set; }
        public decimal Brokerage { get; set; }
        public decimal CEDEC { get; set; }
        public decimal CustomsStamps { get; set; }
        public decimal DeliveryCharges { get; set; }
        public decimal DocumentaryStamps { get; set; }
        public decimal DocumentationCharges { get; set; }
        public decimal ForkliftCost { get; set; }
        public decimal FreightCharges { get; set; }
        public decimal HandlingFee { get; set; }
        public decimal ImportDuties { get; set; }
        public decimal ImportProcessingFee { get; set; }
        public decimal ImportationInsurance { get; set; }
        public decimal Miscellaneous { get; set; }
        public decimal NotarialFee { get; set; }
        public decimal OtherCharges { get; set; }
        public decimal ProcessingFee { get; set; }
        public decimal WarehouseStorageCharges { get; set; }
        public decimal Xerox { get; set; }
    }
}
