using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class Cost_Adjustment
    {
        public static List<Cost_Adjustment_Model> RetreiveData(SqlConnection connection, string CostAdjustNumber)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idCostAdjust
                         ,CostAdjustNumber
                         ,idItem
                         ,InitialCost
                         ,InitialQuantity
                         ,AdjustedCostPerUnit
                         ,AdjustedQuantity
                         ,AdjustedAmount
                         ,DocumentNumber
                         ,Transaction_Date
                         ,Remarks                  
                         ,user_id_chg_by
                         FROM a_Cost_Adjustment
                         WHERE CostAdjustNumber = @CostAdjustNumber
                         ");

            var lmodel = new List<Cost_Adjustment_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@CostAdjustNumber",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = CostAdjustNumber
                };
                cmd.Parameters.Add(parm2);


                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Cost_Adjustment_Model oModel = new Cost_Adjustment_Model
                    {
                        idCostAdjust = (int)oreader["idCostAdjust"],
                        CostAdjustNumber = (string)oreader["CostAdjustNumber"],
                        idItem = (int)oreader["idItem"],
                        InitialCost = (decimal)oreader["InitialCost"],
                        InitialQuantity = (int)oreader["InitialQuantity"],
                        AdjustedCostPerUnit = (decimal)oreader["AdjustedCostPerUnit"],
                        AdjustedQuantity = (int)oreader["AdjustedQuantity"],
                        AdjustedAmount = (decimal)oreader["AdjustedAmount"],
                        DocumentNumber = (string)oreader["DocumentNumber"],
                        Transaction_Date = (DateTime)oreader["Transaction_Date"],
                        Remarks = (string)oreader["Remarks"],
                        user_id_chg_by = (string)oreader["user_id_chg_by"]
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static bool Save(SqlConnection connection, Cost_Adjustment_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"INSERT INTO a_Cost_Adjustment
                             (CostAdjustNumber
                             ,idItem
                             ,InitialCost
                             ,InitialQuantity
                             ,AdjustedCostPerUnit
                             ,AdjustedQuantity
                             ,AdjustedAmount
                             ,DocumentNumber
                             ,Transaction_Date
                             ,Remarks
                             ,user_id_chg_by)
                             VALUES
                             (@CostAdjustNumber
                             ,@idItem
                             ,@InitialCost
                             ,@InitialQuantity
                             ,@AdjustedCostPerUnit
                             ,@AdjustedQuantity
                             ,@AdjustedAmount
                             ,@DocumentNumber
                             ,@Transaction_Date
                             ,@Remarks
                             ,@user_id_chg_by )");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter CostAdjustNumber = new SqlParameter
                    {
                        ParameterName = "@CostAdjustNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.CostAdjustNumber
                    };
                    cmd.Parameters.Add(CostAdjustNumber);

                    SqlParameter idItem = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idItem
                    };
                    cmd.Parameters.Add(idItem);

                    SqlParameter InitialCost = new SqlParameter
                    {
                        ParameterName = "@InitialCost",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.InitialCost
                    };
                    cmd.Parameters.Add(InitialCost);

                    SqlParameter InitialQuantity = new SqlParameter
                    {
                        ParameterName = "@InitialQuantity",
                        SqlDbType = SqlDbType.Int,
                        Value = model.InitialQuantity
                    };
                    cmd.Parameters.Add(InitialQuantity);

                    SqlParameter AdjustedCostPerUnit = new SqlParameter
                    {
                        ParameterName = "@AdjustedCostPerUnit",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.AdjustedCostPerUnit
                    };
                    cmd.Parameters.Add(AdjustedCostPerUnit);

                    SqlParameter AdjustedQuantity = new SqlParameter
                    {
                        ParameterName = "@AdjustedQuantity",
                        SqlDbType = SqlDbType.Int,
                        Value = model.AdjustedQuantity
                    };
                    cmd.Parameters.Add(AdjustedQuantity);

                    SqlParameter AdjustedAmount = new SqlParameter
                    {
                        ParameterName = "@AdjustedAmount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.AdjustedAmount
                    };
                    cmd.Parameters.Add(AdjustedAmount);

                    SqlParameter DocumentNumber = new SqlParameter
                    {
                        ParameterName = "@DocumentNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.DocumentNumber
                    };
                    cmd.Parameters.Add(DocumentNumber);

                    SqlParameter Transaction_Date = new SqlParameter
                    {
                        ParameterName = "@Transaction_Date",
                        SqlDbType = SqlDbType.DateTime,
                        Value = model.Transaction_Date
                    };
                    cmd.Parameters.Add(Transaction_Date);

                    SqlParameter Remarks = new SqlParameter
                    {
                        ParameterName = "@Remarks",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Remarks
                    };
                    cmd.Parameters.Add(Remarks);

                    SqlParameter user_id_chg_by = new SqlParameter
                    {
                        ParameterName = "@user_id_chg_by",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.user_id_chg_by
                    };
                    cmd.Parameters.Add(user_id_chg_by);

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

        public static bool Update(SqlConnection connection, Cost_Adjustment_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_Cost_Adjustment SET
                             CostAdjustNumber = @CostAdjustNumber
                             ,idItem = @idItem
                             ,InitialCost = @ItemNumber
                             ,InitialQuantity = @InitialQuantity
                             ,AdjustedCostPerUnit = @AdjustedCostPerUnit
                             ,AdjustedQuantity = @AdjustedQuantity
                             ,AdjustedAmount = @AdjustedAmount
                             ,DocumentNumber = @DocumentNumber
                             ,Transaction_Date = @Transaction_Date
                             ,Remarks = @Remarks
                             ,user_id_chg_by = @user_id_chg_by
                             WHERE idCostAdjust = @idCostAdjust ");

                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter CostAdjustNumber = new SqlParameter
                    {
                        ParameterName = "@CostAdjustNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.CostAdjustNumber
                    };
                    cmd.Parameters.Add(CostAdjustNumber);

                    SqlParameter idItem = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idItem
                    };
                    cmd.Parameters.Add(idItem);

                    SqlParameter InitialCost = new SqlParameter
                    {
                        ParameterName = "@InitialCost",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.InitialCost
                    };
                    cmd.Parameters.Add(InitialCost);

                    SqlParameter InitialQuantity = new SqlParameter
                    {
                        ParameterName = "@InitialQuantity",
                        SqlDbType = SqlDbType.Int,
                        Value = model.InitialQuantity
                    };
                    cmd.Parameters.Add(InitialQuantity);

                    SqlParameter AdjustedCostPerUnit = new SqlParameter
                    {
                        ParameterName = "@AdjustedCostPerUnit",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.AdjustedCostPerUnit
                    };
                    cmd.Parameters.Add(AdjustedCostPerUnit);

                    SqlParameter AdjustedQuantity = new SqlParameter
                    {
                        ParameterName = "@AdjustedQuantity",
                        SqlDbType = SqlDbType.Int,
                        Value = model.AdjustedQuantity
                    };
                    cmd.Parameters.Add(AdjustedQuantity);

                    SqlParameter AdjustedAmount = new SqlParameter
                    {
                        ParameterName = "@AdjustedAmount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.AdjustedAmount
                    };
                    cmd.Parameters.Add(AdjustedAmount);

                    SqlParameter DocumentNumber = new SqlParameter
                    {
                        ParameterName = "@DocumentNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.DocumentNumber
                    };
                    cmd.Parameters.Add(DocumentNumber);

                    SqlParameter Transaction_Date = new SqlParameter
                    {
                        ParameterName = "@Transaction_Date",
                        SqlDbType = SqlDbType.DateTime,
                        Value = model.Transaction_Date
                    };
                    cmd.Parameters.Add(Transaction_Date);

                    SqlParameter Remarks = new SqlParameter
                    {
                        ParameterName = "@Remarks",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Remarks
                    };
                    cmd.Parameters.Add(Remarks);

                    SqlParameter idCostAdjust = new SqlParameter
                    {
                        ParameterName = "@idCostAdjust",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idCostAdjust
                    };
                    cmd.Parameters.Add(idCostAdjust);

                    SqlParameter user_id_chg_by = new SqlParameter
                    {
                        ParameterName = "@user_id_chg_by",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.user_id_chg_by
                    };
                    cmd.Parameters.Add(user_id_chg_by);

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

        public static bool Delete(SqlConnection connection, int idCostAdjust)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Cost_Adjustment ");
            sQuery.Append("WHERE idCostAdjust = @idCostAdjust");

            bool boolReturnValue = false;

            try
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter sqidCostAdjust = new SqlParameter
                    {
                        ParameterName = "@idCostAdjust",
                        SqlDbType = SqlDbType.Int,
                        Value = idCostAdjust
                    };
                    cmd.Parameters.Add(sqidCostAdjust);

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

        public static string GetLastCostAdjNum(SqlConnection connection)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT TOP 1 CostAdjustNumber FROM a_Cost_Adjustment
                            ORDER BY CostAdjustNumber DESC");

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
                    resultValue = oreader["CostAdjustNumber"].ToString();
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return resultValue;
        }
    }
}
