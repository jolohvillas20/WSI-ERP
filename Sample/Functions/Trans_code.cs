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
    public class Trans_code
    {
        public static DataTable RetrieveData(SqlConnection connection)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT Trans_Code
                          ,Trans_Desc
                          FROM a_Trans_Code
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

        public static bool Save(SqlConnection connection, Trans_Code_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"INSERT INTO a_Trans_Code
                             (Trans_Code
                             ,Trans_Desc
                             ,Trans_Type)
                             VALUES
                             (@Trans_Code
                             ,@Trans_Desc
                             ,@Trans_Type)");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Trans_Code",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Trans_Code
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Trans_Desc",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Trans_Desc
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@Trans_Type",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Trans_Type
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

        public static bool Update(SqlConnection connection, Trans_Code_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_Trans_Code SET
                              Trans_Desc = @Trans_Desc
                              ,Trans_Type = @Trans_Type
                              WHERE Trans_Code = @Trans_Code ");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@Trans_Code",
                        SqlDbType = SqlDbType.Int,
                        Value = model.Trans_Code
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Trans_Desc",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Trans_Desc
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@Trans_Type",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Trans_Type
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

        public static bool Delete(SqlConnection connection, int Trans_Code)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Trans_Code ");
            sQuery.Append("WHERE Trans_Code = @Trans_Code");

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
                        ParameterName = "@Trans_Code",
                        SqlDbType = SqlDbType.Int,
                        Value = Trans_Code
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
