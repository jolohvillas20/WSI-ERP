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
    public class Purchase_Reciept
    {
        public static List<Purchase_Reciept_Model> GetPurchaseReciept(SqlConnection connection, string PONumber, int idItem)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
SELECT          IM.Item_Number, 
				IM.Description,
				Recieved_Qty = (
								SELECT COUNT(Serial_No) 
								FROM a_Item_Serial as ItmS 
								WHERE ItmS.PO_Number = @PONumber AND ItmS.idItem = @idItem AND Pick_Status = 'N' AND InStock = 'Y'),
				IM.UM, 
				ItS.Serial_No
FROM            dbo.a_Item_Master as IM INNER JOIN
                dbo.a_Item_Serial as ItS ON IM.idItem = ItS.idItem
WHERE			ItS.PO_Number = @PONumber AND ItS.idItem = @idItem");


            List<Purchase_Reciept_Model> lData = new List<Purchase_Reciept_Model>();

            connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm = new SqlParameter
                    {
                        ParameterName = "@PONumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = PONumber
                    };
                    cmd.Parameters.Add(parm);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = idItem
                    };
                    cmd.Parameters.Add(parm2);

                    var oreader = cmd.ExecuteReader();

                    string strPreviousItemNumber = null;

                    while (oreader.Read())
                    {      
                        if (oreader["Item_Number"].ToString() != strPreviousItemNumber)
                        {
                            Purchase_Reciept_Model obj = new Purchase_Reciept_Model();
                            strPreviousItemNumber = oreader["Item_Number"].ToString();
                            obj.Item_Number = oreader["Item_Number"].ToString();
                            obj.Description = oreader["Description"].ToString();
                            obj.Recieved_Qty = oreader["Recieved_Qty"].ToString();
                            obj.UM = oreader["UM"].ToString();
                            obj.Grade = "";
                            obj.ForQC = "No";
                            obj.Serial_No = "1.00000";
                            lData.Add(obj);
                        }

                        Purchase_Reciept_Model obj2 = new Purchase_Reciept_Model();
                        obj2.Item_Number = " ";
                        obj2.Description = " ";
                        obj2.Recieved_Qty = " ";
                        obj2.UM = " ";
                        obj2.Grade = " ";
                        obj2.ForQC = " ";
                        obj2.Serial_No = oreader["Serial_No"].ToString() + " - 1.000";
                        lData.Add(obj2);
                    }
                    oreader.Close();
                    cmd.Dispose();
                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return lData;
        }

        public static string getPRnumber(SqlConnection connection)
        {
            int count = 0;
            string suffix = "";

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
                           SELECT COUNT(DISTINCT(Doc_No)) as PURcount
                           FROM a_Trans_History 
                           WHERE trans_code = 'PUR' 
                           AND CONVERT(VARCHAR(10), trans_date, 111) = CONVERT(VARCHAR(10), GETDATE(), 111);
                           ");


            List<Purchase_Reciept_Model> lData = new List<Purchase_Reciept_Model>();

            connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    var oreader = cmd.ExecuteReader();

                    while (oreader.Read())
                    {
                        count = Convert.ToInt32(oreader["PURcount"].ToString());
                    }

                    oreader.Close();
                    cmd.Dispose();
                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            if (count >= 0 & count <= 8)
                suffix = "00" + (count + 1).ToString();
            else if (count >= 9 & count <= 98)
                suffix = "0" + (count + 1).ToString();
            else if (count >= 99)
                suffix = (count + 1).ToString();

            return suffix;
        }
    }
}
