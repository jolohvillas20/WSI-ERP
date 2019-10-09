using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Models
{
    public class Import_Shipment_Detail_Model
    {
        public int idImpShpDet { get; set; }
        public int idImpShpHead { get; set; }
        public int idPOHeader { get; set; }
        public decimal POCharge { get; set; }
    }
}
