using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class PO_Detail
    {
        public static DataTable RetrieveForPOItems(SqlConnection connection, string idPOHeader)
        {
            StringBuilder sQuery = new StringBuilder();
            DataTable dt = new DataTable();

            sQuery.Append(@"SELECT idItem
                           ,Item_Number = (SELECT b.Item_Number FROM a_Item_Master as b WHERE b.idItem = a.idItem)
                           ,Description = (SELECT b.Description FROM a_Item_Master as b WHERE b.idItem = a.idItem)
                           ,Quantity
                           ,Price
                           ,Amount
                           FROM a_PO_Detail as a");

            if (idPOHeader != "")
            {
                sQuery.Append(" WHERE idPOHeader = @idPOHeader");
            }

            var lmodel = new List<PO_Detail_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (idPOHeader != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idPOHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = idPOHeader
                    };
                    cmd.Parameters.Add(parm2);
                }

                dt.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return dt;
        }
        public static DataTable RetrieveData(SqlConnection connection, string idPOHeader, string idItem)
        {
            StringBuilder sQuery = new StringBuilder();

            DataTable dt = new DataTable();

            sQuery.Append(@"
SELECT idPODetail
,idPOHeader   
,idItem
,Item_Number = (SELECT b.Item_Number FROM a_Item_Master as b WHERE b.idItem = a.idItem)
,Description = (SELECT b.Description FROM a_Item_Master as b WHERE b.idItem = a.idItem)
,Quantity
,Price
,Tax
,Amount
,Final_Cost
,Unit_Comp
,isReceived
,Partial_Remaining
FROM a_PO_Detail as a
WHERE idPOHeader = @idPOHeader
");

            if (idItem != "")
            {
                sQuery.Append(" AND idItem = @idItem ");
            }

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (idPOHeader != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idPOHeader",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = idPOHeader
                    };
                    cmd.Parameters.Add(parm2);
                }

                if (idItem != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = idItem
                    };
                    cmd.Parameters.Add(parm2);
                }

                dt.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return dt;
        }

        public static bool Save(SqlConnection connection, PO_Detail_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_PO_Detail
                             (idPOHeader
                             ,idItem
                             ,Quantity
                             ,Price
                             ,Tax
                             ,Amount
,Final_Cost
,Unit_Comp
,isReceived
,Partial_Remaining)
                             VALUES
                             (@idPOHeader
                             ,@idItem
                             ,@Quantity
                             ,@Price
                             ,@Tax
                             ,@Amount
,@Final_Cost
,@Unit_Comp
,@isReceived
,@Partial_Remaining)
");

            using (SqlCommand cmd = new SqlCommand())
            {
                try
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

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Price",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Price
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idItem
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@isReceived",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.isReceived
                    };
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@Quantity",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Quantity
                    };
                    cmd.Parameters.Add(parm6);

                    SqlParameter parm7 = new SqlParameter
                    {
                        ParameterName = "@Unit_Comp",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Unit_Comp
                    };
                    cmd.Parameters.Add(parm7);

                    SqlParameter parm8 = new SqlParameter
                    {
                        ParameterName = "@Tax",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Tax
                    };
                    cmd.Parameters.Add(parm8);

                    SqlParameter parm9 = new SqlParameter
                    {
                        ParameterName = "@Amount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Amount
                    };
                    cmd.Parameters.Add(parm9);

                    SqlParameter parm10 = new SqlParameter
                    {
                        ParameterName = "@Final_Cost",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Final_Cost
                    };
                    cmd.Parameters.Add(parm10);

                    SqlParameter parm11 = new SqlParameter
                    {
                        ParameterName = "@Partial_Remaining",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Partial_Remaining
                    };
                    cmd.Parameters.Add(parm11);

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

        public static bool Update(SqlConnection connection, PO_Detail_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"UPDATE a_PO_Detail SET
                             idPOHeader = @idPOHeader
                             ,idItem = @idItem
                             ,Quantity = @Quantity
                             ,Price = @Price
                             ,Tax = @Tax
                             ,Amount = @Amount
,Final_Cost = @Final_Cost
,Unit_Comp = @Unit_Comp
,isReceived  = @isReceived
,Partial_Remaining = @Partial_Remaining
                             WHERE idPODetail = @idPODetail ");


            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idPODetail",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idPODetail
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
                        ParameterName = "@Price",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Price
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idItem
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@isReceived",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.isReceived
                    };
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@Quantity",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Quantity
                    };
                    cmd.Parameters.Add(parm6);

                    SqlParameter parm7 = new SqlParameter
                    {
                        ParameterName = "@Unit_Comp",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Unit_Comp
                    };
                    cmd.Parameters.Add(parm7);

                    SqlParameter parm8 = new SqlParameter
                    {
                        ParameterName = "@Tax",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Tax
                    };
                    cmd.Parameters.Add(parm8);

                    SqlParameter parm9 = new SqlParameter
                    {
                        ParameterName = "@Amount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Amount
                    };
                    cmd.Parameters.Add(parm9);

                    SqlParameter parm10 = new SqlParameter
                    {
                        ParameterName = "@Final_Cost",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Final_Cost
                    };
                    cmd.Parameters.Add(parm10);

                    SqlParameter parm11 = new SqlParameter
                    {
                        ParameterName = "@Partial_Remaining",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Partial_Remaining
                    };
                    cmd.Parameters.Add(parm11);

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

        public static bool Delete(SqlConnection connection, int idPODetail)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_PO_Detail ");
            sQuery.Append("WHERE idPODetail = @idPODetail");

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
                        ParameterName = "@idPODetail",
                        SqlDbType = SqlDbType.Int,
                        Value = idPODetail
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

        public static bool UpdateReceivedStatus(SqlConnection connection, PO_Detail_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"UPDATE a_PO_Detail SET
                                isReceived = @isReceived
,Partial_Remaining = @Partial_Remaining
                                WHERE idPODetail = @idPODetail ");

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idPODetail",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idPODetail
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm17 = new SqlParameter
                    {
                        ParameterName = "@isReceived",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.isReceived
                    };
                    cmd.Parameters.Add(parm17);

                    SqlParameter parm11 = new SqlParameter
                    {
                        ParameterName = "@Partial_Remaining",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Partial_Remaining
                    };
                    cmd.Parameters.Add(parm11);

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

        public static bool DeleteAllDetail(SqlConnection connection, int idPOHeader)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_PO_Detail ");
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
    }
}
