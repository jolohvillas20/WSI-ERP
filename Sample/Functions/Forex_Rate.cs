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
    public class Forex_Rate
    {
        public static decimal RetrieveData(SqlConnection connection, string currency_code)
        {
            decimal forex = 0;
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
                SELECT TOP(1) Amount 
                FROM a_Forex_Rate ");

            sQuery.Append(@"
                    WHERE Currency_Codes = @currency_code 
                ");

            sQuery.Append(@"
                    ORDER BY DateChange DESC
                ");

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;
                if (currency_code != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@currency_code",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = currency_code
                    };
                    cmd.Parameters.Add(parm2);
                }
                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    forex = (decimal)oreader["Amount"];
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            SqlConnection.ClearAllPools();
            return forex;
        }


        public static DataTable RetrieveData(SqlConnection connection)
        {
            DataTable forex = new DataTable();
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
                SELECT  * 
                FROM a_Forex_Rate 
                ORDER BY DateChange DESC
                ");

            connection.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;
                forex.Load(cmd.ExecuteReader());
                cmd.Dispose();
            }
            connection.Close();
            SqlConnection.ClearAllPools();
            return forex;
        }

        public static bool Save(SqlConnection connection, Forex_Rate_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"INSERT INTO a_Forex_Rate
                             (Currency_Codes
                             ,Amount
							 ,DateChange)
                             VALUES
                             (@Currency_Code
                             ,@Amount
							 ,@DateChange)");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Currency_Code",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Currency_Code
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Amount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Amount
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@DateChange",
                        SqlDbType = SqlDbType.DateTime,
                        Value = model.DateChange
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

        public static bool Update(SqlConnection connection, Forex_Rate_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_Forex_Rate SET
                             Currency_Code = @Currency_Code
                             ,Amount = @Amount
							 ,DateChange = @DateChange
                             WHERE idForex = @idForex ");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@Currency_Code",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Currency_Code
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Amount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Amount
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@DateChange",
                        SqlDbType = SqlDbType.DateTime,
                        Value = model.DateChange
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@idForex",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idForex
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

        public static bool Delete(SqlConnection connection, string Currency_Code)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Forex_Rate ");
            sQuery.Append("WHERE idForex = @idForex");

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
                        ParameterName = "@idForex",
                        SqlDbType = SqlDbType.Int,
                        Value = Currency_Code
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
