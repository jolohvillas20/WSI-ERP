using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class Stock_Transfer_Detail
    {
        public static List<Stock_Transfer_Detail_Model> RetrieveData(SqlConnection connection, int idSTHeader, int idItem, int Qty)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idSTDetail
                           ,idSTHeader
                           ,idItem
                           ,Qty                         
                           FROM a_Stock_Transfer_Detail WHERE idSTDetail <> 0 
                           ");

            if (idSTHeader != 0)
            {
                sQuery.Append(" AND idSTHeader = @idSTHeader ");
            }

            if (idItem != 0)
            {
                sQuery.Append(" AND idItem = @idItem ");
            }

            if (Qty != 0)
            {
                sQuery.Append(" AND Qty = @Qty ");
            }

            sQuery.Append("  ");

            var lmodel = new List<Stock_Transfer_Detail_Model>();
            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (idSTHeader != 0)
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idSTHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = idSTHeader
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

                if (Qty != 0)
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Qty",
                        SqlDbType = SqlDbType.Int,
                        Value = Qty
                    };
                    cmd.Parameters.Add(parm2);
                }

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Stock_Transfer_Detail_Model oModel = new Stock_Transfer_Detail_Model
                    {
                        idSTDetail = (int)oreader["idSTDetail"],
                        idSTHeader = (int)oreader["idSTHeader"],
                        idItem = (int)oreader["idItem"],
                        Qty = (int)oreader["Qty"]
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }
            connection.Close();
            return lmodel;
        }

        public static int Save(SqlConnection connection, Stock_Transfer_Detail_Model model)
        {
            int returnValue = 0;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_Stock_Transfer_Detail
                            (idSTHeader
                            ,idItem
                            ,Qty
                            )
                            VALUES
                            (@idSTHeader
                            ,@idItem
                            ,@Qty                        
                            )

SELECT SCOPE_IDENTITY() as 'ID'
");

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@idSTHeader",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idSTHeader
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
                    returnValue = 0;
                    oreader.Close();
                    cmd.Dispose();
                    cmd.Parameters.Clear();
                    SQL_Transact.RollbackTransaction(connection, GUID);
                }
            }

            return returnValue;
        }

        public static bool Update(SqlConnection connection, Stock_Transfer_Detail_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_Stock_Transfer_Detail SET
                             idSTHeader = @idSTHeader
                             ,idItem = @idItem
                             ,Qty = @Qty                             
                             WHERE idSTDetail = @idSTDetail ");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idSTDetail",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idSTDetail
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idSTHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idSTHeader
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

        public static bool Delete(SqlConnection connection, int idSTHeader)
        {
            bool boolReturnValue = false;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Stock_Transfer_Detail ");
            sQuery.Append(@"WHERE idSTHeader = @idSTHeader ");

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm = new SqlParameter
                    {
                        ParameterName = "@idSTHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = idSTHeader
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

        public static bool DeleteByID(SqlConnection connection, int idSTDetail)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Stock_Transfer_Detail ");
            sQuery.Append("WHERE idSTDetail = @idSTDetail");

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
                        ParameterName = "@idSTDetail",
                        SqlDbType = SqlDbType.Int,
                        Value = idSTDetail
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

        public static DataTable RetrieveDataForSOCreation(SqlConnection connection, int idSTHeader)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT a.Item_Number, a.Description, a.UM, b.Qty, b.Cost, b.Tax_Amount, b.Amount 
                            FROM a_Item_Master as a 
                            INNER JOIN a_Stock_Transfer_Detail as b 
                            ON a.idItem = b.idItem 
                            WHERE b.idSTHeader = @idSTHeader
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
                    ParameterName = "@idSTHeader",
                    SqlDbType = SqlDbType.Int,
                    Value = idSTHeader
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
