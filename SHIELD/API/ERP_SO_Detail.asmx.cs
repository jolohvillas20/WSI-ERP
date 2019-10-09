using POSOINV.Functions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace SOPOINV.API
{
    /// <summary>
    /// Summary description for ERP_SO_Detail
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ERP_SO_Detail : System.Web.Services.WebService
    {
        SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);


        [WebMethod()]
        public bool Save_SO_Detail(int idSOHeader, int idItem, decimal Qty, decimal Cost, string UM, decimal Discount, decimal Amount, decimal Tax_Amount)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"INSERT INTO a_SO_Detail
                                 (idSOHeader
                                 ,idItem
                                 ,Qty
                                 ,Cost
                                 ,UM
                                 ,Discount
             ,Tax_Amount
             ,Amount)
                                 VALUES
                                 (@idSOHeader
                                 ,@idItem
                                 ,@Qty
                                 ,@Cost
                                 ,@UM
                                 ,@Discount
             ,@Tax_Amount           
             ,@Amount)
");
                oCon.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = oCon;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idSOHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = idSOHeader
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = idItem
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@Qty",
                        SqlDbType = SqlDbType.Int,
                        Value = Qty
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@Cost",
                        SqlDbType = SqlDbType.Decimal,
                        Value = Cost
                    };
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@UM",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = UM
                    };
                    cmd.Parameters.Add(parm6);

                    SqlParameter parm7 = new SqlParameter
                    {
                        ParameterName = "@Discount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = Discount
                    };
                    cmd.Parameters.Add(parm7);

                    SqlParameter parm8 = new SqlParameter
                    {
                        ParameterName = "@Tax_Amount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = Tax_Amount
                    };
                    cmd.Parameters.Add(parm8);

                    SqlParameter parm9 = new SqlParameter
                    {
                        ParameterName = "@Amount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = Amount
                    };
                    cmd.Parameters.Add(parm9);

                    if (cmd.ExecuteNonQuery() >= 1)
                    {
                        bool save = Item_Serial.UpdateAllocQuantity(oCon, idItem, decimal.ToInt32(Qty));
                        if (save == true)                        
                            returnValue = true;                        
                        else
                            returnValue = false;
                    }
                    else
                        returnValue = false;

                    cmd.Dispose();
                    cmd.Parameters.Clear();
                }

                oCon.Close();
            }
            catch
            {
                //throw new ArgumentException(ex.Message);
            }

            return returnValue;
        }

    }
}
