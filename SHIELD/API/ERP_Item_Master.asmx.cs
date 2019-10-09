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
    /// Summary description for ERP_Item_Master
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ERP_Item_Master : System.Web.Services.WebService
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        [WebMethod]
        public List<Item_Master_Model> retrieveItemMaster(string Item_Number)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idItem
                         ,Item_Number
                         ,Principal_SKU
                         ,Description                         
                         FROM a_Item_Master
                         WHERE Item_Number LIKE '%' + @Item_Number + '%'
                         ");

            var lmodel = new List<Item_Master_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@Item_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = Item_Number
                };
                cmd.Parameters.Add(parm2);


                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Item_Master_Model oModel = new Item_Master_Model
                    {
                        idItem = (int)oreader["idItem"],
                        ItemNumber = (string)oreader["Item_Number"],
                        Principal_SKU = (string)oreader["Principal_SKU"],
                        Description = (string)oreader["Description"]
                    };
                    lmodel.Add(oModel);
                }

                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        [WebMethod()]
        public int GetTotalQtyByidItem(int idItem)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("SELECT Total_Qty FROM a_Item_Master ");
            sQuery.Append("WHERE idItem = @idItem");

            int resultValue = 0;

            try
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = idItem
                    };
                    cmd.Parameters.Add(parm2);


                    var oreader = cmd.ExecuteReader();

                    while (oreader.Read())
                        resultValue = (int)oreader["Total_Qty"];

                    cmd.Dispose();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return resultValue;
        }
    }
}
