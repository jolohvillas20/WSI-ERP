using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class PO_Header
    {
        public static List<PO_Header_Model> RetrieveData(SqlConnection connection, string PO_Number)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idPOHeader
                         ,PO_Number
                         ,PO_Date
                         ,Delivery_Date
                         ,Terms
                         ,FOB_Point
                         ,Shipping_Via
                         ,Currency
                         ,idSupplier
                         ,PO_Quantity
                         ,PO_Total
                         ,Total_Charges
                         ,Forex_Rate
                         ,PR_Number
                         ,Created_By
,Remarks
,POStatus
,ImportShipmentNumber
                         FROM a_PO_Header 
                         ");

            if (PO_Number != "")
            {
                sQuery.Append(" WHERE PO_Number LIKE '%' + @PO_Number + '%'");
            }

            sQuery.Append(" ORDER BY PO_Number DESC ");

            var lmodel = new List<PO_Header_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (PO_Number != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@PO_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = PO_Number
                    };
                    cmd.Parameters.Add(parm2);
                }

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    PO_Header_Model oModel = new PO_Header_Model
                    {
                        idPOHeader = (int)oreader["idPOHeader"],
                        PO_Number = (string)oreader["PO_Number"],
                        PO_Date = (DateTime)oreader["PO_Date"],
                        Delivery_Date = (DateTime)oreader["Delivery_Date"],
                        Terms = (string)oreader["Terms"],
                        FOB_Point = (string)oreader["FOB_Point"],
                        Shipping_Via = (string)oreader["Shipping_Via"],
                        Currency = (string)oreader["Currency"],
                        PO_Total = (decimal)oreader["PO_Total"],
                        idSupplier = (int)oreader["idSupplier"],
                        PO_Quantity = (int)oreader["PO_Quantity"],
                        Total_Charges = (decimal)oreader["Total_Charges"],
                        Forex_Rate = (decimal)oreader["Forex_Rate"],
                        PR_Number = (string)oreader["PR_Number"],
                        Created_By = (string)oreader["Created_By"],
                        Remarks = (string)oreader["Remarks"],
                        POStatus = (string)oreader["POStatus"],
                        ImportShipmentNumber = (string)oreader["ImportShipmentNumber"]
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static int Save(SqlConnection connection, PO_Header_Model model)
        {
            int returnValue = 0;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_PO_Header
                             (PO_Number
                             ,PO_Date
                             ,Delivery_Date
                             ,Terms
                             ,FOB_Point
                             ,Shipping_Via
                             ,Currency
                             ,idSupplier
                             ,PO_Total
                             ,PO_Quantity
                             ,Total_Charges
,Forex_Rate
,PR_Number
,Created_By
,Remarks
,POStatus
,ImportShipmentNumber)
                             VALUES
                             (@PO_Number
                             ,@PO_Date
                             ,@Delivery_Date
                             ,@Terms
                             ,@FOB_Point
                             ,@Shipping_Via
                             ,@Currency
                             ,@idSupplier
                             ,@PO_Total
                             ,@PO_Quantity
                             ,@Total_Charges
,@Forex_Rate
,@PR_Number
,@Created_By
,@Remarks
,@POStatus
,@ImportShipmentNumber)

SELECT SCOPE_IDENTITY() as 'ID'");

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@PO_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.PO_Number
                };
                cmd.Parameters.Add(parm2);

                SqlParameter parm3 = new SqlParameter
                {
                    ParameterName = "@idSupplier",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idSupplier
                };
                cmd.Parameters.Add(parm3);

                SqlParameter parm4 = new SqlParameter
                {
                    ParameterName = "@PO_Date",
                    SqlDbType = SqlDbType.DateTime,
                    Value = model.PO_Date
                };
                cmd.Parameters.Add(parm4);

                SqlParameter parm5 = new SqlParameter
                {
                    ParameterName = "@Delivery_Date",
                    SqlDbType = SqlDbType.DateTime,
                    Value = model.Delivery_Date
                };
                cmd.Parameters.Add(parm5);

                SqlParameter parm6 = new SqlParameter
                {
                    ParameterName = "@Terms",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Terms
                };
                cmd.Parameters.Add(parm6);

                SqlParameter parm7 = new SqlParameter
                {
                    ParameterName = "@FOB_Point",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.FOB_Point
                };
                cmd.Parameters.Add(parm7);

                SqlParameter parm8 = new SqlParameter
                {
                    ParameterName = "@Shipping_Via",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Shipping_Via
                };
                cmd.Parameters.Add(parm8);

                SqlParameter parm9 = new SqlParameter
                {
                    ParameterName = "@Currency",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Currency
                };
                cmd.Parameters.Add(parm9);

                SqlParameter parm10 = new SqlParameter
                {
                    ParameterName = "@PO_Total",
                    SqlDbType = SqlDbType.Decimal,
                    Value = model.PO_Total
                };
                cmd.Parameters.Add(parm10);

                SqlParameter parm11 = new SqlParameter
                {
                    ParameterName = "@PO_Quantity",
                    SqlDbType = SqlDbType.Int,
                    Value = model.PO_Quantity
                };
                cmd.Parameters.Add(parm11);

                SqlParameter parm12 = new SqlParameter
                {
                    ParameterName = "@Total_Charges",
                    SqlDbType = SqlDbType.Decimal,
                    Value = model.Total_Charges
                };
                cmd.Parameters.Add(parm12);

                SqlParameter parm13 = new SqlParameter
                {
                    ParameterName = "@Forex_Rate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = model.Forex_Rate
                };
                cmd.Parameters.Add(parm13);

                SqlParameter parm14 = new SqlParameter
                {
                    ParameterName = "@PR_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.PR_Number
                };
                cmd.Parameters.Add(parm14);

                SqlParameter parm15 = new SqlParameter
                {
                    ParameterName = "@Created_By",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Created_By
                };
                cmd.Parameters.Add(parm15);

                SqlParameter parm16 = new SqlParameter
                {
                    ParameterName = "@Remarks",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Remarks
                };
                cmd.Parameters.Add(parm16);

                SqlParameter parm17 = new SqlParameter
                {
                    ParameterName = "@POStatus",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.POStatus
                };
                cmd.Parameters.Add(parm17);
                
                SqlParameter parm18 = new SqlParameter
                {
                    ParameterName = "@ImportShipmentNumber",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.ImportShipmentNumber
                };
                cmd.Parameters.Add(parm18);

                var oreader = cmd.ExecuteReader();

                try
                {
                    while (oreader.Read())
                    {
                        returnValue = Convert.ToInt32(oreader["ID"].ToString());
                    }

                    oreader.Close();
                    cmd.Dispose();
                    cmd.Parameters.Clear();
                    SQL_Transact.CommitTransaction(connection, GUID);

                }
                catch
                {
                    oreader.Close();
                    cmd.Dispose();
                    cmd.Parameters.Clear();
                    SQL_Transact.RollbackTransaction(connection, GUID);
                }
            }
            return returnValue;
        }

        public static bool Update(SqlConnection connection, PO_Header_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"UPDATE a_PO_Header SET
                             PO_Number = @PO_Number
                             ,Terms = @Terms
                             ,FOB_Point = @FOB_Point
                             ,Shipping_Via = @Shipping_Via
                             ,Currency = @Currency
                             ,idSupplier = @idSupplier
                             ,PO_Total = @PO_Total
                             ,PO_Quantity = @PO_Quantity
                             ,Total_Charges = @Total_Charges
,Forex_Rate = @Forex_Rate
,Created_By = @Created_By
,Remarks = @Remarks
,POStatus = @POStatus
,ImportShipmentNumber = @ImportShipmentNumber
                             WHERE idPOHeader = @idPOHeader ");


            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@PO_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.PO_Number
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idPOHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idPOHeader
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@idSupplier",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.idSupplier
                    };
                    cmd.Parameters.Add(parm3);

                    //SqlParameter parm4 = new SqlParameter
                    //{
                    //    ParameterName = "@PO_Date",
                    //    SqlDbType = SqlDbType.DateTime,
                    //    Value = model.PO_Date
                    //};
                    //cmd.Parameters.Add(parm4);

                    //SqlParameter parm5 = new SqlParameter
                    //{
                    //    ParameterName = "@Delivery_Date",
                    //    SqlDbType = SqlDbType.DateTime,
                    //    Value = model.Delivery_Date
                    //};
                    //cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@Terms",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Terms
                    };
                    cmd.Parameters.Add(parm6);

                    SqlParameter parm7 = new SqlParameter
                    {
                        ParameterName = "@FOB_Point",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.FOB_Point
                    };
                    cmd.Parameters.Add(parm7);

                    SqlParameter parm8 = new SqlParameter
                    {
                        ParameterName = "@Shipping_Via",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Shipping_Via
                    };
                    cmd.Parameters.Add(parm8);

                    SqlParameter parm9 = new SqlParameter
                    {
                        ParameterName = "@Currency",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Currency
                    };
                    cmd.Parameters.Add(parm9);

                    SqlParameter parm10 = new SqlParameter
                    {
                        ParameterName = "@PO_Total",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.PO_Total
                    };
                    cmd.Parameters.Add(parm10);

                    SqlParameter parm11 = new SqlParameter
                    {
                        ParameterName = "@PO_Quantity",
                        SqlDbType = SqlDbType.Int,
                        Value = model.PO_Quantity
                    };
                    cmd.Parameters.Add(parm11);

                    SqlParameter parm12 = new SqlParameter
                    {
                        ParameterName = "@Total_Charges",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Total_Charges
                    };
                    cmd.Parameters.Add(parm12);

                    SqlParameter parm13 = new SqlParameter
                    {
                        ParameterName = "@Forex_Rate",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Forex_Rate
                    };
                    cmd.Parameters.Add(parm13);

                    //SqlParameter parm14 = new SqlParameter
                    //{
                    //    ParameterName = "@PR_Number",
                    //    SqlDbType = SqlDbType.NVarChar,
                    //    Value = model.PR_Number
                    //};
                    //cmd.Parameters.Add(parm14);

                    SqlParameter parm15 = new SqlParameter
                    {
                        ParameterName = "@Created_By",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Created_By
                    };
                    cmd.Parameters.Add(parm15);

                    SqlParameter parm16 = new SqlParameter
                    {
                        ParameterName = "@Remarks",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Remarks
                    };
                    cmd.Parameters.Add(parm16);

                    SqlParameter parm17 = new SqlParameter
                    {
                        ParameterName = "@POStatus",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.POStatus
                    };
                    cmd.Parameters.Add(parm17);

                    SqlParameter parm18 = new SqlParameter
                    {
                        ParameterName = "@ImportShipmentNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.ImportShipmentNumber
                    };
                    cmd.Parameters.Add(parm18);

                    if (cmd.ExecuteNonQuery() >= 1)
                    {
                        returnValue = true;
                        cmd.Dispose();
                        cmd.Parameters.Clear();
                        SQL_Transact.CommitTransaction(connection, GUID);
                    }
                }
                catch
                {
                    cmd.Dispose();
                    cmd.Parameters.Clear();
                    SQL_Transact.RollbackTransaction(connection, GUID);
                }
            }

            return returnValue;
        }

        public static bool Delete(SqlConnection connection, int idPOHeader)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_PO_Header ");
            sQuery.Append("WHERE idPOHeader = @idPOHeader");

            bool boolReturnValue = false;

            try
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm = new SqlParameter
                    {
                        ParameterName = "@idPOHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = idPOHeader
                    };
                    cmd.Parameters.Add(parm);


                    if (cmd.ExecuteNonQuery() >= 1)
                        boolReturnValue = true;

                    cmd.Dispose();
                    cmd.Parameters.Clear();
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            return boolReturnValue;
        }

        public static string CheckDuplicate(SqlConnection connection, string PO_Number)
        {
            string returnValue = "";

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idPOHeader
                             FROM a_PO_Header
                             WHERE PO_Number = @PO_Number
                             ");

            var lmodel = new List<Item_Serial_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm1 = new SqlParameter
                {
                    ParameterName = "@PO_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = PO_Number
                };
                cmd.Parameters.Add(parm1);

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    returnValue = oreader["idPOHeader"].ToString();
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return returnValue;
        }

        public static string GetLastPONumber(SqlConnection connection)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT TOP 1 PO_Number FROM a_PO_Header 
                            WHERE PO_Number Like '%PJ%'
                            ORDER BY PO_Number DESC");

            string resultValue = null;
            connection.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    resultValue = oreader["PO_Number"].ToString();
                }
                oreader.Close();
                cmd.Dispose();
            }
            connection.Close();
            return resultValue;
        }

        public static bool UpdatePOStatus(SqlConnection connection, PO_Header_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);
            try
            {

                StringBuilder sQuery = new StringBuilder();

                sQuery.Append(@"UPDATE a_PO_Header SET
                                POStatus = @POStatus                                
                                WHERE idPOHeader = @idPOHeader ");

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idPOHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idPOHeader
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm17 = new SqlParameter
                    {
                        ParameterName = "@POStatus",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.POStatus
                    };
                    cmd.Parameters.Add(parm17);

                    if (cmd.ExecuteNonQuery() >= 1)
                        returnValue = true;

                    cmd.Dispose();
                    cmd.Parameters.Clear();
                }
                SQL_Transact.CommitTransaction(connection, GUID);

            }
            catch
            {
                SQL_Transact.RollbackTransaction(connection, GUID);
            }
            return returnValue;
        }

        public static bool UpdateTotalCharges(SqlConnection connection, PO_Header_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);
            try
            {
                StringBuilder sQuery = new StringBuilder();

                sQuery.Append(@"UPDATE a_PO_Header SET
                                Total_Charges = @Total_Charges,
ImportShipmentNumber = @ImportShipmentNumber
                                WHERE idPOHeader = @idPOHeader ");

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idPOHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idPOHeader
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm17 = new SqlParameter
                    {
                        ParameterName = "@Total_Charges",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Total_Charges
                    };
                    cmd.Parameters.Add(parm17);

                    SqlParameter parm18 = new SqlParameter
                    {
                        ParameterName = "@ImportShipmentNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.ImportShipmentNumber
                    };
                    cmd.Parameters.Add(parm18);

                    if (cmd.ExecuteNonQuery() >= 1)
                        returnValue = true;

                    cmd.Dispose();
                    cmd.Parameters.Clear();
                }
                SQL_Transact.CommitTransaction(connection, GUID);

            }
            catch
            {
                SQL_Transact.RollbackTransaction(connection, GUID);
            }
            return returnValue;
        }

    }
}
