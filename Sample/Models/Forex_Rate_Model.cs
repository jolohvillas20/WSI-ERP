using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Models
{
    public class Forex_Rate_Model
    {
        public int idForex { get; set; }
        public string Currency_Code { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateChange { get; set; }
    }
}
