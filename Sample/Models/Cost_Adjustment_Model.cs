using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Models
{
    public class Cost_Adjustment_Model
    {
        public int idCostAdjust { get; set; }
        public string CostAdjustNumber { get; set; }
        public int idItem { get; set; }
        public decimal InitialCost { get; set; }
        public int InitialQuantity { get; set; }
        public decimal AdjustedCostPerUnit { get; set; }
        public int AdjustedQuantity { get; set; }
        public decimal AdjustedAmount { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime Transaction_Date { get; set; }
        public string Remarks { get; set; }
        public string user_id_chg_by { get; set; }
    }
}
