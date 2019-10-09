using POSOINV.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Configuration;

namespace SOPOINV.API
{
    /// <summary>
    /// Summary description for ERP_Customer_Details
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ERP_Customer_Details : System.Web.Services.WebService
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        [WebMethod()]
        public List<Customer_Details_Model> RetrieveData(int idCustomer, string Customer_Code)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idCustomer
                         ,Customer_Code
                         ,Customer_Name
                         ,Address1
                         ,Address2
                         ,Address3
                         ,Address4
                         ,credit_term
                         ,Company_Name
                         FROM a_Customer_Details
WHERE idCustomer <> 0
                         ");

            if (idCustomer != 0)
            {
                sQuery.Append(" AND idCustomer = @idCustomer ");
            }

            if (Customer_Code.ToString() != "")
            {
                sQuery.Append(" AND Customer_Code = @Customer_Code ");
            }

            var lmodel = new List<Customer_Details_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (idCustomer.ToString() != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idCustomer",
                        SqlDbType = SqlDbType.Int,
                        Value = idCustomer
                    };
                    cmd.Parameters.Add(parm1);
                }

                if (Customer_Code.ToString() != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@Customer_Code",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Customer_Code
                    };
                    cmd.Parameters.Add(parm1);
                }

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Customer_Details_Model oModel = new Customer_Details_Model
                    {
                        idCustomer = (int)oreader["idCustomer"],
                        Customer_Code = (string)oreader["Customer_Code"],
                        Customer_Name = (string)oreader["Customer_Name"],
                        Address1 = (string)oreader["Address1"],
                        Address2 = (string)oreader["Address2"],
                        Address3 = (string)oreader["Address3"],
                        Address4 = (string)oreader["Address4"],
                        credit_term = (string)oreader["credit_term"],
                        Company_Name = (string)oreader["Company_Name"]
                    };
                    lmodel.Add(oModel);
                }

                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        [WebMethod()]
        public string GetCreditTerm(string Customer_Code)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT 
                         ,credit_term
                         FROM a_Customer_Details
WHERE idCustomer <> 0
                         ");
      
            if (Customer_Code.ToString() != "")
            {
                sQuery.Append(" AND Customer_Code = @Customer_Code ");
            }

            string resultValue = "";

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (Customer_Code.ToString() != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@Customer_Code",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Customer_Code
                    };
                    cmd.Parameters.Add(parm1);
                }

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                { 
                    resultValue = (string)oreader["credit_term"];
                }
                cmd.Dispose();
            }

            connection.Close();

            return resultValue;
        }
    }
}
