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
    public class Pick_Activity
    {
        public static string SetPickNumber(SqlConnection connection)
        {
            string returnValue = "";
            string PickNumber = "";
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT TOP 1 Pick_Number
                             FROM a_Pick_Header ORDER BY Pick_Number DESC
                             ");
            connection.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    PickNumber = oreader["Pick_Number"].ToString();
                }
                oreader.Close();
                cmd.Dispose();
            }
            connection.Close();
            //string prefix = "PK";

            if (PickNumber == null)
            {
                PickNumber = "PK000000";
            }
            else
            {
                PickNumber = PickNumber.Substring(2);
                int PickNumber_ = int.Parse(PickNumber) + 1;
                string pick_Number = "PK" + PickNumber_.ToString("000000");
                returnValue = pick_Number;
            }

            //if (count >= 0 && count <= 8)
            //    returnValue = prefix + "00000" + (count + 1).ToString();
            //else if (count >= 9 && count <= 98)
            //    returnValue = prefix + "0000" + (count + 1).ToString();
            //else if (count >= 99 && count <= 998)
            //    returnValue = prefix + "000" + (count + 1).ToString();
            //else if (count >= 999 && count <= 9998)
            //    returnValue = prefix + "00" + (count + 1).ToString();
            //else if (count >= 9999 && count <= 99998)
            //    returnValue = prefix + "0" + (count + 1).ToString();
            //else if (count >= 99999)
            //    returnValue = prefix + (count + 1).ToString();

            return returnValue;
        }


        public static string checkPickedSO(SqlConnection connection, string SO_Number)
        {
            string returnValue = "";
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT Pick_Status
                             FROM a_SO_Header
                             WHERE SO_Number = @SO_Number
                             ");

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm1 = new SqlParameter
                {
                    ParameterName = "@SO_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = SO_Number
                };
                cmd.Parameters.Add(parm1);

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    returnValue = (string)oreader["Pick_Status"];
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return returnValue;
        }

        public static DataTable RetrieveData(SqlConnection connection, string SO_Number)
        {
            StringBuilder sQuery = new StringBuilder();

            if (SO_Number != "")
            {
                sQuery.Append(@"DECLARE @idSOH int

                                SELECT @idSOH = idSOHeader
                                FROM a_SO_Header
                                WHERE SO_Number = @SO_Number
                                ");
            }

            sQuery.Append(@"SELECT a.idSODetail                      
                           ,a.idItem
                           ,b.Item_Number
                           ,b.Description
                           ,a.Qty                           
                           FROM a_SO_Detail as a
                           INNER JOIN a_Item_Master as b
                           ON a.idItem = b.idItem
                           ");

            if (SO_Number != "")
            {
                sQuery.Append("WHERE a.idSOHeader = @idSOH");
            }

            DataTable dt = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (SO_Number != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@SO_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = SO_Number.Trim()
                    };
                    cmd.Parameters.Add(parm2);
                }

                dt.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return dt;
        }

        public static List<Pick_Activity_Model> GetPickActivityRegister(SqlConnection connection, string SO_Number)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
SELECT DISTINCT IM.Item_Number, SOH.SO_Number, PH.Pick_Number, PS.Serial_No, ST.Site_Name, IM.UM, PH.user_id_chg_by, PH.Pick_Date,
                         CONVERT(nvarchar(150),CD.AddressShipping1 + ' ' + CD.AddressShipping2 + ' ' + CD.AddressShipping3 + ' ' + CD.AddressShipping4) As Address, 
                         CD.Customer_Code, ST.Site_Desc, IM.Description, CD.Company_Name, SOH.Remarks
FROM            dbo.a_SO_Header as SOH INNER JOIN
                         dbo.a_Site as ST ON SOH.idSite = ST.idSite INNER JOIN
                         dbo.a_Pick_Header as PH ON SOH.idSOHeader = PH.idSOHeader INNER JOIN
                         dbo.a_Customer_Details as CD ON SOH.idCustomer = CD.idCustomer INNER JOIN
                         dbo.a_SO_Detail as SOD ON SOH.idSOHeader = SOD.idSOHeader INNER JOIN
                         dbo.a_Item_Master as IM ON SOD.idItem = IM.idItem INNER JOIN
                         dbo.a_Pick_Detail as PD ON PH.idPickHeader = PD.idPickHeader AND IM.idItem = PD.idItem INNER JOIN
                         dbo.a_Pick_Serial as PS ON PD.idPickDetail = PS.idPickDetail
WHERE         SO_Number = @SO_Number ");


            List<Pick_Activity_Model> lData = new List<Pick_Activity_Model>();

            connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@SO_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = SO_Number
                    };
                    cmd.Parameters.Add(parm2);

                    var oreader = cmd.ExecuteReader();

                    string strPreviousItemNumber = null;

                    while (oreader.Read())
                    {
                        Pick_Activity_Model obj = new Pick_Activity_Model();
                        obj.SONumber = (string)oreader["SO_Number"].ToString();

                        if (oreader["Item_Number"].ToString() != strPreviousItemNumber)
                        {
                            strPreviousItemNumber = oreader["Item_Number"].ToString();
                            obj.ItemNumber = strPreviousItemNumber;
                            obj.QtyDelivered = "1";
                            obj.UM = oreader["UM"].ToString();
                            obj.ItemDescription = oreader["Description"].ToString() + Environment.NewLine + oreader["Serial_No"].ToString();
                        }
                        else
                        {
                            obj.ItemDescription = oreader["Serial_No"].ToString();
                            obj.QtyDelivered = "1";
                            obj.UM = oreader["UM"].ToString();
                        }

                        obj.CustCode = oreader["Customer_Code"].ToString();
                        obj.Site = oreader["Site_Name"].ToString();
                        obj.UserId = oreader["user_id_chg_by"].ToString();
                        obj.PickListNo = oreader["Pick_Number"].ToString();
                        obj.PickListDate = oreader["Pick_Date"].ToString();
                        obj.Address = oreader["Address"].ToString();
                        obj.SerialNumber = oreader["Serial_No"].ToString();
                        obj.CustomerName = oreader["Company_Name"].ToString();
                        obj.ExternalComments = oreader["Remarks"].ToString();
                        obj.SiteDescription = oreader["Site_Desc"].ToString();

                        lData.Add(obj);
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

    }
}
