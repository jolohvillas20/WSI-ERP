using POSOINV.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Functions
{
    public class Sales_Return
    {
        public static DataTable RetrieveData(SqlConnection connection, string SearchInput)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT a.idItem, b.SO_Number, d.Item_Number, a.Qty, a.Cost, a.UM, a.Discount, a.Amount, a.Tax_Amount, c.Pick_Number
                            FROM a_SO_Detail AS a 
                            INNER JOIN a_SO_Header AS b 
                            ON a.idSOHeader = b.idSOHeader 
                            INNER JOIN a_Pick_Header AS c 
                            ON b.idSOHeader = c.idSOHeader 
                            INNER JOIN a_Item_Master d 
                            ON a.idItem = d.idItem
                            WHERE b.SO_Number = @SearchInput
                            OR c.Pick_Number = @SearchInput
                           ");

            var lmodel = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm1 = new SqlParameter
                {
                    ParameterName = "@SearchInput",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = SearchInput
                };
                cmd.Parameters.Add(parm1);

                lmodel.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static List<Sales_Return_Model> RetreiveDataForPrinting(SqlConnection connection, string SLR_Number)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
SELECT          dbo.a_Return_Header.SO_Number, dbo.a_Item_Master.Item_Number, dbo.a_Item_Master.Description, dbo.a_Item_Master.UM, dbo.a_Return_Detail.Returned_Qty, (dbo.a_Reason_Code.reason_code + ' ' +dbo.a_Reason_Code.reason_desc) as reason, dbo.a_Return_Serial.Serial_No
FROM            dbo.a_Item_Master INNER JOIN
                dbo.a_Return_Detail ON dbo.a_Item_Master.idItem = dbo.a_Return_Detail.idItem INNER JOIN
                dbo.a_Return_Header ON dbo.a_Return_Detail.idReturnHeader = dbo.a_Return_Header.idReturnHeader INNER JOIN
                dbo.a_Return_Serial ON dbo.a_Return_Detail.idReturnDetail = dbo.a_Return_Serial.idReturnDetail INNER JOIN
                dbo.a_Reason_Code ON dbo.a_Return_Header.Reason_Code = dbo.a_Reason_Code.reason_code
WHERE           dbo.a_Return_Header.SLR_Number = @SLR_Number
                         ");

            List<Sales_Return_Model> lmodel = new List<Sales_Return_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@SLR_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = SLR_Number
                };
                cmd.Parameters.Add(parm2);

                //lmodel.Load(cmd.ExecuteReader());

                var oreader = cmd.ExecuteReader();

                string strPreviousItemNumber = null;

                while (oreader.Read())
                {
                    Sales_Return_Model obj = new Sales_Return_Model();
                    
                    if (oreader["Item_Number"].ToString() != strPreviousItemNumber)
                    {
                        strPreviousItemNumber = oreader["Item_Number"].ToString();
                        obj.SO_Number = oreader["SO_Number"].ToString();
                        obj.Item_Number = strPreviousItemNumber;
                        obj.Description = oreader["Description"].ToString();
                        obj.Grade = "";
                        obj.UM = oreader["UM"].ToString();
                        obj.Quantity = oreader["Returned_Qty"].ToString();
                        obj.Reason = oreader["reason_desc"].ToString();
                        obj.Serial_No = oreader["Serial_No"].ToString();
                    }
                    else
                    {
                        obj.Serial_No = oreader["Serial_No"].ToString();                        
                    }


                    lmodel.Add(obj);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }
    }
}
