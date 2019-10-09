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
   public class Reason_Code
    {
        public static DataTable RetrieveData(SqlConnection connection)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT reason_code
                          ,reason_desc
                          FROM a_Reason_Code
                          ORDER BY reason_desc
                          ");

            DataTable dt = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                dt.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return dt;
        }

        public static bool Save(SqlConnection connection, Reason_Code_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"INSERT INTO a_Reason_Code
                             (reason_code
                             ,reason_desc
                             ,reason_type)
                             VALUES
                             (@reason_code
                             ,@reason_desc
                             ,@reason_type)");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@reason_code",
                        SqlDbType = SqlDbType.Int,
                        Value = model.reason_code
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@reason_desc",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.reason_desc
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@reason_type",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.reason_type
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

        public static bool Update(SqlConnection connection, Reason_Code_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_Reason_Code SET
                              reason_desc = @reason_desc
                              ,reason_type = @reason_type
                              WHERE reason_code = @reason_code ");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@reason_code",
                        SqlDbType = SqlDbType.Int,
                        Value = model.reason_code
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@reason_desc",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.reason_desc
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@reason_type",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.reason_type
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

        public static bool Delete(SqlConnection connection, int reason_code)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Reason_Code ");
            sQuery.Append("WHERE reason_code = @reason_code");

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
                        ParameterName = "@reason_code",
                        SqlDbType = SqlDbType.Int,
                        Value = reason_code
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
