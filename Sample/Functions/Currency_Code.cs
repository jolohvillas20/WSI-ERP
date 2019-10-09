using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class Currency_Code
    {
        public static DataTable RetrieveData(SqlConnection con)
        {
            DataTable dt = new DataTable();
            StringBuilder strQuery = new StringBuilder();

            strQuery.Append(@"SELECT Currency_Code,Currency_Name FROM a_Currency_Codes");

            con.Open();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(strQuery.ToString(), con))
                {
                    using (DataTable ds = new DataTable())
                    {
                        da.Fill(ds);
                        dt = ds;
                    }
                }
            }
            catch 
            {
                SqlConnection.ClearAllPools();
            }
            con.Close();
            SqlConnection.ClearAllPools();
            return dt;
        }
              
        public static bool Save(SqlConnection connection, Currency_Code_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"INSERT INTO a_Currency_Codes
                             (Currency_Code
                             ,Country
							 ,Currency_Name
							 ,Currency_Symbol)
                             VALUES
                             (@Currency_Code
                             ,@Country
							 ,@Currency_Name
							 ,@Currency_Symbol)");
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
                        ParameterName = "@Country",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Country
                    };
                    cmd.Parameters.Add(parm3);
					
					SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@Currency_Name",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Currency_Name
                    };
                    cmd.Parameters.Add(parm4);
					
					SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@Currency_Symbol",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Currency_Symbol
                    };
                    cmd.Parameters.Add(parm5);

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

        public static bool Update(SqlConnection connection, Currency_Code_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_Currency_Codes SET
                             Currency_Code = @Currency_Code
                             ,Country = @Country
							 ,Currency_Name = @Currency_Name
							 ,Currency_Symbol = @Currency_Symbol
                             WHERE Currency_Code = @Currency_Code ");
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
                        ParameterName = "@Country",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Country
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Currency_Name",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Currency_Name
                    };
                    cmd.Parameters.Add(parm3);

					SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@Currency_Symbol",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Currency_Symbol
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

            sQuery.Append("DELETE FROM a_Currency_Codes ");
            sQuery.Append("WHERE Currency_Code = @Currency_Code");

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
                        ParameterName = "@Currency_Code",
                        SqlDbType = SqlDbType.NVarChar,
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
