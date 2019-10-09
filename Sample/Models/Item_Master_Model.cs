using System;
using System.Collections.Generic;
using System.Text;

namespace POSOINV.Models
{
    public class Item_Master_Model
    {
        public int idItem { get; set; }
        public int idClass { get; set; }
        public int idSubClass { get; set; }
        public string ItemNumber { get; set; }
        public string Principal_SKU { get; set; }
        public string Description { get; set; }
        public string Site { get; set; }
        public decimal Unit_Weight { get; set; }
        public string Weight_UM { get; set; }
        public string UM { get; set; }
        public int OnHand_Qty { get; set; }
        public int Alloc_Qty { get; set; }
        public int Total_Qty { get; set; }
        public decimal Ave_Cost { get; set; }
        public decimal Total_Cost { get; set; }
        /// <summary>
        /// 
        /// </summary>
    }
}
