using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SOPOINV.API
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SO_Creation_API" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SO_Creation_API.svc or SO_Creation_API.svc.cs at the Solution Explorer and start debugging.
    public class SO_Creation_API : ISO_Creation_API
    {
        public byte[] DoWork(string x, string y, string z)
        {
            byte[] returnvalue = null;

            return returnvalue;
        }

        public SO_Create_Model _Model(SO_Create_Model sO_Create_)
        {
            return sO_Create_;
        }
    }
}
