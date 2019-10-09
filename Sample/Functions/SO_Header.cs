using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class SO_Header
    {
        public static List<SO_Header_Model> RetrieveData(SqlConnection connection, string SO_Number, bool forPicking)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idSOHeader
      ,SO_Number
      ,idPOHeader
      ,Order_Date
      ,Due_Date
      ,Customer_PO
      ,Salesman
      ,idCustomer
      ,Ship_Code
      ,Gross_Amount
      ,Final_Discount
      ,Freight_Charges
      ,Other_Charges
      ,Net_Amount
      ,Tax_Amount
      ,credit_term
      ,transaction_ID
      ,Remarks
      ,idSite
      ,currency_code
      ,Pick_Status
      ,Special_Concession
      ,SO_Status
      ,Stock_Status
      ,Active
      ,CreatedBy
      ,End_User
      ,End_User_City
,Forex_Rate
                         FROM a_SO_Header
                         WHERE idSOHeader <> 0
                        ");

            if (SO_Number != "")
            {
                sQuery.Append(" AND SO_Number = @SO_Number ");
            }

            if (forPicking == true)
            {
                sQuery.Append(" AND Pick_Status = 'N' ");
            }

            var lmodel = new List<SO_Header_Model>();

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
                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    SO_Header_Model oModel = new SO_Header_Model
                    {

                        idSOHeader = (int)oreader["idSOHeader"],
                        SO_Number = (string)oreader["SO_Number"],
                        idPOHeader = (int)oreader["idPOHeader"],
                        Order_Date = (DateTime)oreader["Order_Date"],
                        Due_Date = (DateTime)oreader["Due_Date"],
                        Customer_PO = (string)oreader["Customer_PO"],
                        Salesman = (string)oreader["Salesman"],
                        idCustomer = (int)oreader["idCustomer"],
                        Ship_Code = (int)oreader["Ship_Code"],
                        Gross_Amount = (decimal)oreader["Gross_Amount"],
                        Final_Discount = (decimal)oreader["Final_Discount"],
                        Freight_Charges = (decimal)oreader["Freight_Charges"],
                        Other_Charges = (decimal)oreader["Other_Charges"],
                        Net_Amount = (decimal)oreader["Net_Amount"],
                        Tax_Amount = (decimal)oreader["Tax_Amount"],
                        credit_term = (string)oreader["credit_term"],
                        transaction_ID = (string)oreader["transaction_ID"],
                        Remarks = (string)oreader["Remarks"],
                        idSite = (int)oreader["idSite"],
                        currency_code = (string)oreader["currency_code"],
                        Pick_Status = (string)oreader["Pick_Status"],
                        Special_Concession = (string)oreader["Special_Concession"],
                        SO_Status = (string)oreader["SO_Status"],
                        Stock_Status = (string)oreader["Stock_Status"],
                        CreatedBy = (string)oreader["CreatedBy"],
                        End_User = (string)oreader["End_User"],
                        End_User_City = (string)oreader["End_User_City"],
                        Forex_Rate = (decimal)oreader["Forex_Rate"]
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static bool Save(SqlConnection connection, SO_Header_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_SO_Header
                             (SO_Number
                             ,idPOHeader
                             ,idCustomer
                             ,Due_Date
                             ,Customer_PO
                             ,Salesman
                             ,Ship_Code
                             ,Gross_Amount
                             ,Final_Discount
                             ,Freight_Charges
                             ,Other_Charges
                             ,Net_Amount
                             ,Tax_Amount
                             ,credit_term
							 ,Remarks
							 ,idSite
							 ,currency_code
                             ,Pick_Status
							 ,Special_Concession
                             ,transaction_ID
                             ,SO_Status
							 ,Stock_Status
                             ,CreatedBy
,End_User
,End_User_City
,Forex_Rate)
                             VALUES
                             (@SO_Number
                             ,@idPOHeader
                             ,@idCustomer
                             ,@Due_Date
                             ,@Customer_PO
                             ,@Salesman
                             ,@Ship_Code
                             ,@Gross_Amount
                             ,@Final_Discount
                             ,@Freight_Charges
                             ,@Other_Charges
                             ,@Net_Amount
                             ,@Tax_Amount
                             ,@credit_term
							 ,@Remarks
							 ,@idSite
							 ,@currency_code
                             ,'N'
							 ,@Special_Concession
                             ,@transaction_ID
                             ,@SO_Status
							 ,@Stock_Status
                             ,@CreatedBy
,@End_User
,@End_User_City
,@Forex_Rate)


");


            using (SqlCommand cmd = new SqlCommand())
            {

                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@SO_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.SO_Number
                };
                cmd.Parameters.Add(parm2);

                SqlParameter parm3 = new SqlParameter
                {
                    ParameterName = "@idPOHeader",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idPOHeader
                };
                cmd.Parameters.Add(parm3);

                SqlParameter parm4 = new SqlParameter
                {
                    ParameterName = "@idCustomer",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idCustomer
                };
                cmd.Parameters.Add(parm4);

                SqlParameter parm5 = new SqlParameter();
                parm5.ParameterName = "@Forex_Rate";
                parm5.SqlDbType = SqlDbType.Decimal;
                parm5.Value = model.Forex_Rate;
                cmd.Parameters.Add(parm5);

                SqlParameter parm6 = new SqlParameter
                {
                    ParameterName = "@Due_Date",
                    SqlDbType = SqlDbType.DateTime,
                    Value = model.Due_Date
                };
                cmd.Parameters.Add(parm6);

                SqlParameter parm7 = new SqlParameter
                {
                    ParameterName = "@Customer_PO",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Customer_PO
                };
                cmd.Parameters.Add(parm7);

                SqlParameter parm8 = new SqlParameter
                {
                    ParameterName = "@Salesman",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Salesman
                };
                cmd.Parameters.Add(parm8);

                SqlParameter parm9 = new SqlParameter
                {
                    ParameterName = "@Ship_Code",
                    SqlDbType = SqlDbType.Int,
                    Value = model.Ship_Code
                };
                cmd.Parameters.Add(parm9);

                SqlParameter parm10 = new SqlParameter
                {
                    ParameterName = "@Gross_Amount",
                    SqlDbType = SqlDbType.Decimal,
                    Value = model.Gross_Amount
                };
                cmd.Parameters.Add(parm10);

                SqlParameter parm11 = new SqlParameter
                {
                    ParameterName = "@Final_Discount",
                    SqlDbType = SqlDbType.Decimal,
                    Value = model.Final_Discount
                };
                cmd.Parameters.Add(parm11);

                SqlParameter parm12 = new SqlParameter
                {
                    ParameterName = "@Freight_Charges",
                    SqlDbType = SqlDbType.Decimal,
                    Value = model.Freight_Charges
                };
                cmd.Parameters.Add(parm12);

                SqlParameter parm13 = new SqlParameter
                {
                    ParameterName = "@Other_Charges",
                    SqlDbType = SqlDbType.Decimal,
                    Value = model.Other_Charges
                };
                cmd.Parameters.Add(parm13);

                SqlParameter parm14 = new SqlParameter
                {
                    ParameterName = "@Net_Amount",
                    SqlDbType = SqlDbType.Decimal,
                    Value = model.Net_Amount
                };
                cmd.Parameters.Add(parm14);

                SqlParameter parm15 = new SqlParameter
                {
                    ParameterName = "@Tax_Amount",
                    SqlDbType = SqlDbType.Decimal,
                    Value = model.Tax_Amount
                };
                cmd.Parameters.Add(parm15);

                SqlParameter parm16 = new SqlParameter
                {
                    ParameterName = "@credit_term",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.credit_term
                };
                cmd.Parameters.Add(parm16);

                SqlParameter parm17 = new SqlParameter
                {
                    ParameterName = "@Remarks",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Remarks
                };
                cmd.Parameters.Add(parm17);

                SqlParameter parm18 = new SqlParameter
                {
                    ParameterName = "@idSite",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idSite
                };
                cmd.Parameters.Add(parm18);

                SqlParameter parm19 = new SqlParameter
                {
                    ParameterName = "@currency_code",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.currency_code
                };
                cmd.Parameters.Add(parm19);

                SqlParameter parm20 = new SqlParameter
                {
                    ParameterName = "@Special_Concession",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Special_Concession
                };
                cmd.Parameters.Add(parm20);

                SqlParameter parm21 = new SqlParameter
                {
                    ParameterName = "@transaction_ID",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.transaction_ID
                };
                cmd.Parameters.Add(parm21);

                SqlParameter parm22 = new SqlParameter
                {
                    ParameterName = "@SO_Status",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.SO_Status
                };
                cmd.Parameters.Add(parm22);

                SqlParameter parm23 = new SqlParameter
                {
                    ParameterName = "@Stock_Status",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Stock_Status
                };
                cmd.Parameters.Add(parm23);

                SqlParameter parm24 = new SqlParameter
                {
                    ParameterName = "@CreatedBy",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.CreatedBy
                };
                cmd.Parameters.Add(parm24);

                SqlParameter parm25 = new SqlParameter
                {
                    ParameterName = "@End_User",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.End_User
                };
                cmd.Parameters.Add(parm25);

                SqlParameter parm26 = new SqlParameter
                {
                    ParameterName = "@End_User_City",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.End_User_City
                };
                cmd.Parameters.Add(parm26);
                try
                {
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

        public static bool Update(SqlConnection connection, SO_Header_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"UPDATE a_SO_Header SET
                             SO_Number = @SO_Number
                             ,idPOHeader = @idPOHeader
                             ,idCustomer = @idCustomer
                             ,Due_Date = @Due_Date
                             ,Customer_PO = @Customer_PO
                             ,Salesman = @Salesman
                             ,Ship_Code = @Ship_Code
                             ,Gross_Amount = @Gross_Amount
                             ,Final_Discount = @Final_Discount
                             ,Freight_Charges = @Freight_Charges
                             ,Other_Charges = @Other_Charges
                             ,Net_Amount = @Net_Amount
                             ,Tax_Amount = @Tax_Amount
                             ,credit_term = @credit_term
							 ,Remarks = @Remarks
							 ,idSite = @idSite
                             ,currency_code = @currency_code
							 ,Special_Concession = @Special_Concession
							 ,Stock_Status = @Stock_Status
,End_User = @End_User
,End_User_City = @End_User_City
,Forex_Rate = @Forex_Rate
                             WHERE idSOHeader = @idSOHeader

");

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm1 = new SqlParameter
                {
                    ParameterName = "@idSOHeader",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idSOHeader
                };
                cmd.Parameters.Add(parm1);

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@SO_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.SO_Number
                };
                cmd.Parameters.Add(parm2);

                SqlParameter parm3 = new SqlParameter
                {
                    ParameterName = "@idPOHeader",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idPOHeader
                };
                cmd.Parameters.Add(parm3);

                SqlParameter parm4 = new SqlParameter
                {
                    ParameterName = "@idCustomer",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idCustomer
                };
                cmd.Parameters.Add(parm4);

                SqlParameter parm5 = new SqlParameter();
                parm5.ParameterName = "@Forex_Rate";
                parm5.SqlDbType = SqlDbType.Decimal;
                parm5.Value = model.Forex_Rate;
                cmd.Parameters.Add(parm5);

                SqlParameter parm6 = new SqlParameter
                {
                    ParameterName = "@Due_Date",
                    SqlDbType = SqlDbType.DateTime,
                    Value = model.Due_Date
                };
                cmd.Parameters.Add(parm6);

                SqlParameter parm7 = new SqlParameter
                {
                    ParameterName = "@Customer_PO",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Customer_PO
                };
                cmd.Parameters.Add(parm7);

                SqlParameter parm8 = new SqlParameter
                {
                    ParameterName = "@Salesman",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Salesman
                };
                cmd.Parameters.Add(parm8);

                SqlParameter parm9 = new SqlParameter
                {
                    ParameterName = "@Ship_Code",
                    SqlDbType = SqlDbType.Int,
                    Value = model.Ship_Code
                };
                cmd.Parameters.Add(parm9);

                SqlParameter parm11 = new SqlParameter
                {
                    ParameterName = "@Gross_Amount",
                    SqlDbType = SqlDbType.Decimal,
                    Value = model.Gross_Amount
                };
                cmd.Parameters.Add(parm11);

                SqlParameter parm12 = new SqlParameter
                {
                    ParameterName = "@Final_Discount",
                    SqlDbType = SqlDbType.Decimal,
                    Value = model.Final_Discount
                };
                cmd.Parameters.Add(parm12);

                SqlParameter parm13 = new SqlParameter
                {
                    ParameterName = "@Freight_Charges",
                    SqlDbType = SqlDbType.Decimal,
                    Value = model.Freight_Charges
                };
                cmd.Parameters.Add(parm13);

                SqlParameter parm14 = new SqlParameter
                {
                    ParameterName = "@Other_Charges",
                    SqlDbType = SqlDbType.Decimal,
                    Value = model.Other_Charges
                };
                cmd.Parameters.Add(parm14);

                SqlParameter parm15 = new SqlParameter
                {
                    ParameterName = "@Net_Amount",
                    SqlDbType = SqlDbType.Decimal,
                    Value = model.Net_Amount
                };
                cmd.Parameters.Add(parm15);

                SqlParameter parm16 = new SqlParameter
                {
                    ParameterName = "@Tax_Amount",
                    SqlDbType = SqlDbType.Decimal,
                    Value = model.Tax_Amount
                };
                cmd.Parameters.Add(parm16);

                SqlParameter parm17 = new SqlParameter
                {
                    ParameterName = "@credit_term",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.credit_term
                };
                cmd.Parameters.Add(parm17);

                SqlParameter parm18 = new SqlParameter
                {
                    ParameterName = "@Remarks",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Remarks
                };
                cmd.Parameters.Add(parm18);

                SqlParameter parm19 = new SqlParameter
                {
                    ParameterName = "@idSite",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idSite
                };
                cmd.Parameters.Add(parm19);

                SqlParameter parm20 = new SqlParameter
                {
                    ParameterName = "@currency_code",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.currency_code
                };
                cmd.Parameters.Add(parm20);

                SqlParameter parm21 = new SqlParameter
                {
                    ParameterName = "@Special_Concession",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Special_Concession
                };
                cmd.Parameters.Add(parm21);

                SqlParameter parm22 = new SqlParameter
                {
                    ParameterName = "@Stock_Status",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Stock_Status
                };
                cmd.Parameters.Add(parm22);

                SqlParameter parm25 = new SqlParameter
                {
                    ParameterName = "@End_User",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.End_User
                };
                cmd.Parameters.Add(parm25);

                SqlParameter parm26 = new SqlParameter
                {
                    ParameterName = "@End_User_City",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.End_User_City
                };
                cmd.Parameters.Add(parm26);

                try
                {
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

        //public static bool UpdateSOStatus(SqlConnection connection, SO_Header_Model model)
        //{
        //    bool returnValue = true;
        //    StringBuilder sQuery = new StringBuilder();

        //    try
        //    {
        //        sQuery.Append(@"UPDATE a_SO_Header SET
        //                     SO_Status = @SO_Status
        //                     WHERE idSOHeader = @idSOHeader ");
        //        connection.Open();

        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            cmd.Connection = connection;
        //            cmd.CommandText = sQuery.ToString();
        //            cmd.CommandType = CommandType.Text;

        //            SqlParameter parm1 = new SqlParameter
        //            {
        //                ParameterName = "@idSOHeader",
        //                SqlDbType = SqlDbType.Int,
        //                Value = model.idSOHeader
        //            };
        //            cmd.Parameters.Add(parm1);

        //            SqlParameter parm2 = new SqlParameter
        //            {
        //                ParameterName = "@SO_Status",
        //                SqlDbType = SqlDbType.NVarChar,
        //                Value = model.SO_Status
        //            };
        //            cmd.Parameters.Add(parm2);

        //            if (cmd.ExecuteNonQuery() >= 1)
        //                returnValue = true;

        //            cmd.Dispose();
        //            cmd.Parameters.Clear();
        //        }

        //        connection.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ArgumentException(ex.Message);
        //    }

        //    return returnValue;
        //}

        public static bool Delete(SqlConnection connection, int idSOHeader)
        {
            bool boolReturnValue = false;

            var GUID = SQL_Transact.GenerateGUID();

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(" UPDATE a_SO_Header SET Active = 'N' , SO_Status = 'DELETED' ");
            sQuery.Append(" WHERE idSOHeader = @idSOHeader ");

            SQL_Transact.BeginTransaction(connection, GUID);

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm = new SqlParameter
                    {
                        ParameterName = "@idSOHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = idSOHeader
                    };
                    cmd.Parameters.Add(parm);

                    if (cmd.ExecuteNonQuery() >= 1)
                    {
                        boolReturnValue = true;
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

            SqlConnection con2 = new SqlConnection(connection.ConnectionString);

            sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idSODetail
                           ,idSOHeader
                           ,idItem
                           ,Qty
                           ,Cost
                           ,UM
                           ,Discount
                           ,Amount
                           FROM a_SO_Detail WHERE idSODetail <> 0 
                           ");

            if (idSOHeader != 0)
            {
                sQuery.Append(" AND idSOHeader = @idSOHeader ");
            }

            var lmodel = new List<SO_Detail_Model>();
            con2.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con2;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (idSOHeader != 0)
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idSOHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = idSOHeader
                    };
                    cmd.Parameters.Add(parm2);
                }

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    SO_Detail_Model oModel = new SO_Detail_Model
                    {
                        idSODetail = (int)oreader["idSODetail"],
                        idSOHeader = (int)oreader["idSOHeader"],
                        idItem = (int)oreader["idItem"],
                        Qty = (int)oreader["Qty"],
                        Cost = (Decimal)oreader["Cost"],
                        UM = (string)oreader["UM"],
                        Discount = (Decimal)oreader["Discount"],
                        Amount = (Decimal)oreader["Amount"]
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();

            }
            con2.Close();

            for (int x = 0; x <= lmodel.Count - 1; x++)
            {
                int soqty = decimal.ToInt32(lmodel[x].Qty);

                var itemModel = Item_Master.RetreiveAllocation(con2, lmodel[x].idItem);
                int master_alloc = Convert.ToInt32(itemModel.Rows[0][2].ToString());
                int master_total = Convert.ToInt32(itemModel.Rows[0][3].ToString());

                int newAlloc = master_alloc - soqty;
                int newTotal = master_total + soqty;

                Item_Master_Model mastermodel = new Item_Master_Model();
                mastermodel.Alloc_Qty = newAlloc;
                mastermodel.Total_Qty = newTotal;
                mastermodel.idItem = lmodel[x].idItem;
                Item_Master.UpdateQtyOnly(con2, mastermodel);
                boolReturnValue = true;
            }

            return boolReturnValue;
        }

        public static bool UpdateStatus(SqlConnection connection, string Pick_Status, string SO_Status, string Active, int idSOHeader)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"UPDATE a_SO_Header SET
                             Pick_Status = @Pick_Status,
                             SO_Status = @SO_Status,
                             Active = @Active
                             WHERE idSOHeader = @idSOHeader
");

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idSOHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = idSOHeader
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Pick_Status",
                        SqlDbType = SqlDbType.NChar,
                        Value = Pick_Status
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@SO_Status",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = SO_Status
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@Active",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Active
                    };
                    cmd.Parameters.Add(parm4);

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

        public static string CheckDuplicateCustPO(SqlConnection connection, string CustPONum)
        {
            string returnValue = "";

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT SO_Number
                         FROM a_SO_Header 
                         WHERE Customer_PO = @CustPONum AND Active = 'Y'
");

            var lmodel = new List<Customer_Details_Model>();
            connection.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm1 = new SqlParameter
                {
                    ParameterName = "@CustPONum",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = CustPONum
                };
                cmd.Parameters.Add(parm1);

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    if (oreader.IsDBNull(0))
                    {
                        returnValue = "";
                    }
                    else
                    {
                        returnValue = oreader[0].ToString();
                    }
                }
                oreader.Close();
                cmd.Dispose();
            }
            connection.Close();
            return returnValue;
        }

        public static DataTable RetrieveDataForPick(SqlConnection con, string SO_Number, string SOmode)
        {
            DataTable dt = new DataTable();
            StringBuilder strQuery = new StringBuilder();

            strQuery.Append(@"SELECT DISTINCT 
                              a.idSOHeader,
                              a.SO_Number as 'SO Number',
                              b.Pick_Number as 'Pick Number',
                              CONVERT(DATE,a.Order_Date) as 'Order date',
                              CONVERT(DATE,a.Due_Date) as 'Due Date',
                              a.CreatedBy as 'Salesman',
                              a.Pick_Status as 'Pick Status',
                              a.Active,
                              a.SO_Status as 'SO Status',
                              a.Stock_Status as 'Stock Status',
                              DATEDIFF(day, Order_Date, GETDATE()) AS DaysOpen
                              FROM a_SO_Header as a
                              LEFT JOIN a_Pick_Header as b on a.idSOHeader = b.idSOHeader
                              WHERE a.idSOHeader != 0
                              ");

            if (SO_Number != "")
            {
                strQuery.Append(" AND a.SO_Number LIKE '%' + @SO_Number + '%' ");
            }

            if (SOmode != "")
            {
                strQuery.Append(" AND a.SO_Status = @SOmode ");
            }

            strQuery.Append(" ORDER BY SO_Number DESC ");

            con.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = strQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (SO_Number != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@SO_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = SO_Number
                    };
                    cmd.Parameters.Add(parm2);
                }

                if (SOmode != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@SOmode",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = SOmode
                    };
                    cmd.Parameters.Add(parm2);
                }

                dt.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            con.Close();
            SqlConnection.ClearAllPools();
            return dt;
        }

        public static DataTable RetrieveDataForInvoice(SqlConnection con, string SO_Number)
        {
            DataTable dt = new DataTable();
            StringBuilder strQuery = new StringBuilder();

            strQuery.Append(@"SELECT DISTINCT 
                              a.idSOHeader,
                              a.SO_Number as 'SO Number',
                              b.Invoice_Number as 'Invoice Number',
                              CONVERT(DATE,a.Order_Date) as 'Order date',
                              CONVERT(DATE,a.Due_Date) as 'Due Date',
                              a.CreatedBy as 'Salesman',
                              a.Pick_Status as 'Pick Status',
                              a.SO_Status as 'SO Status'
                              FROM a_SO_Header as a
                              LEFT JOIN a_Invoice as b on a.idSOHeader = b.idSOHeader
                              WHERE a.idSOHeader != 0 AND (SO_Status = 'Open' OR SO_Status = 'Closed')
                              ");

            if (SO_Number != "")
            {
                strQuery.Append(" AND a.SO_Number LIKE '%' + @SO_Number + '%' ");
            }

            strQuery.Append(" ORDER BY SO_Number DESC ");

            con.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = strQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (SO_Number != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@SO_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = SO_Number
                    };
                    cmd.Parameters.Add(parm2);
                }

                dt.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            con.Close();
            SqlConnection.ClearAllPools();
            return dt;
        }

        public static int GetLastidSOHeader(SqlConnection con)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("SELECT TOP 1 idSOHeader FROM a_SO_Header ");
            sQuery.Append("ORDER BY idSOHeader DESC");

            int resultValue = 0;
            con.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    if (oreader.IsDBNull(0))
                    {
                        resultValue = 0;
                    }
                    else
                    {
                        resultValue = (int)oreader[0];
                    }
                }
                oreader.Close();
                cmd.Dispose();
            }
            con.Close();
            return resultValue;
        }

        public static string GetLastSONumber(SqlConnection con)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("SELECT TOP 1 SO_Number FROM a_SO_Header ");
            sQuery.Append("ORDER BY SO_Number DESC");

            string resultValue = null;
            con.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    if (oreader.IsDBNull(0))
                    {
                        resultValue = "";
                    }
                    else
                    {
                        resultValue = (string)oreader[0];
                    }
                }
                oreader.Close();
                cmd.Dispose();
            }
            con.Close();
            return resultValue;
        }
    }
}
