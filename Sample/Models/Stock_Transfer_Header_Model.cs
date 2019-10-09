using System;
using System.Collections.Generic;
using System.Text;

namespace POSOINV.Models
{
    public class Stock_Transfer_Header_Model
    {
        public int idSTHeader { get; set; }
        public string Doc_Number { get; set; }
        public string STR_Number { get; set; }
        public string Site_From { get; set; }
        public string Site_To { get; set; }   
        public DateTime timestamp { get; set; }        
    }
}
