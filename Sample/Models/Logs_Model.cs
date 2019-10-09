using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Models
{
    public class Logs_Model
    {
        public int idLog { get; set; }
        public int idUser { get; set; }
        public string Description { get; set; }
        public string Form { get; set; }
    }
}
