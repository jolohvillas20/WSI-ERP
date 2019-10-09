using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class SO_Detail
    {
        public static List<SO_Detail_Model> RetrieveData(SqlConnection connection, int idSOHeader, int idItem, int qty)
        {
            StringBuilder sQuery = new StringBuilder();

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

            if (idItem != 0)
            {
                sQuery.Append(" AND idItem = @idItem ");
            }

            if (qty != 0)
            {
                sQuery.Append(" AND qty = @qty ");
            }

            sQuery.Append("  ");

            var lmodel = new List<SO_Detail_Model>();
            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
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

                if (idItem != 0)
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = idItem
                    };
                    cmd.Parameters.Add(parm2);
                }

                if (qty != 0)
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@qty",
                        SqlDbType = SqlDbType.Int,
                        Value = qty
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
            connection.Close();
            return lmodel;
        }

        public static bool Save(SqlConnection connection, SO_Detail_Model model)
        {
            bool returnValue = true;
            
            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

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

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idSOHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idSOHeader
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idItem
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@Qty",
                        SqlDbType = SqlDbType.Int,
                        Value = model.Qty
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@Cost",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Cost
                    };
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@UM",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.UM
                    };
                    cmd.Parameters.Add(parm6);

                    SqlParameter parm7 = new SqlParameter
                    {
                        ParameterName = "@Discount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Discount
                    };
                    cmd.Parameters.Add(parm7);

                    SqlParameter parm8 = new SqlParameter
                    {
                        ParameterName = "@Tax_Amount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Tax_Amount
                    };
                    cmd.Parameters.Add(parm8);

                    SqlParameter parm9 = new SqlParameter
                    {
                        ParameterName = "@Amount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Amount
                    };
                    cmd.Parameters.Add(parm9);

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

        public static bool Update(SqlConnection connection, SO_Detail_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_SO_Detail SET
                             idSOHeader = @idSOHeader
                             ,idItem = @idItem
                             ,Qty = @Qty
                             ,Cost = @Cost
                             ,UM = @UM
                             ,Discount = @Discount
							 ,Tax_Amount = @Tax_Amount
                             ,Amount = @Amount
                             WHERE idSODetail = @idSODetail ");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idSODetail",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idSODetail
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idSOHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idSOHeader
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idItem
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@Qty",
                        SqlDbType = SqlDbType.Int,
                        Value = model.Qty
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@Cost",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Cost
                    };
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@UM",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.UM
                    };
                    cmd.Parameters.Add(parm6);

                    SqlParameter parm7 = new SqlParameter
                    {
                        ParameterName = "@Discount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Discount
                    };
                    cmd.Parameters.Add(parm7);

                    SqlParameter parm8 = new SqlParameter
                    {
                        ParameterName = "@Tax_Amount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Tax_Amount
                    };
                    cmd.Parameters.Add(parm8);

                    SqlParameter parm9 = new SqlParameter
                    {
                        ParameterName = "@Amount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Amount
                    };
                    cmd.Parameters.Add(parm9);

                    if (cmd.ExecuteNonQuery() >= 1)
                        returnValue = true;

                    cmd.Dispose();
                    cmd.Parameters.Clear();
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return returnValue;
        }

        public static bool Delete(SqlConnection connection, int idSOHeader)
        {
            bool boolReturnValue = false;
            
            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_SO_Detail ");
            sQuery.Append(@"WHERE idSOHeader = @idSOHeader ");

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

            return boolReturnValue;
        }

        public static bool DeleteByID(SqlConnection connection, int idSODetail)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_SO_Detail ");
            sQuery.Append("WHERE idSODetail = @idSODetail");

            bool boolReturnValue = false;

            try
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idSODetail",
                        SqlDbType = SqlDbType.Int,
                        Value = idSODetail
                    };

                    cmd.Parameters.Add(parm1);

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

        public static bool UpdateItems(SqlConnection connection, SO_Detail_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_SO_Detail SET
                             Qty = @Qty
                             ,Cost = @Cost
                             ,Discount = @Discount
                             ,Amount = @Amount
                             ,Tax_Amount = @Tax_Amount
                             WHERE idSODetail = @idSODetail ");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idSODetail",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idSODetail
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Qty",
                        SqlDbType = SqlDbType.Int,
                        Value = model.Qty
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Cost",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Cost
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@Discount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Discount
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@Amount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Amount
                    };
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@Tax_Amount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Tax_Amount
                    };
                    cmd.Parameters.Add(parm6);


                    if (cmd.ExecuteNonQuery() >= 1)
                        returnValue = true;

                    cmd.Dispose();
                    cmd.Parameters.Clear();
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return returnValue;
        }

        public static DataTable RetrieveDataForSOCreation(SqlConnection connection, int idSOHeader)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT a.Item_Number, a.Description, a.UM, b.Qty, b.Cost, b.Tax_Amount, b.Amount 
                            FROM a_Item_Master as a 
                            INNER JOIN a_SO_Detail as b 
                            ON a.idItem = b.idItem 
                            WHERE b.idSOHeader = @idSOHeader
                            ");

            DataTable dt = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@idSOHeader",
                    SqlDbType = SqlDbType.Int,
                    Value = idSOHeader
                };
                cmd.Parameters.Add(parm2);

                dt.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return dt;
        }
    }
}
