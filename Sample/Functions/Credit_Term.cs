using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class Credit_Term
    {
        public static DataTable RetrieveData(SqlConnection con)
        {
            DataTable dt = new DataTable();
            StringBuilder strQuery = new StringBuilder();

            strQuery.Append(@"SELECT [credit_term]
                            ,[term_desc]
                            ,[days_to_pay] 
                            FROM a_Credit_Term");

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

        public static bool Save(SqlConnection connection, Credit_Term_Model model)
        {
            bool returnValue = true;

            StringBuilder sQuery = new StringBuilder();
        
            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            sQuery.Append(@"INSERT INTO a_Credit_Term
                             (credit_term
                             ,term_desc
                             ,days_to_pay)
                             VALUES
                             (@credit_term
                             ,@term_desc
							 ,@days_to_pay)");

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@credit_term",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.credit_term
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@term_desc",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.term_desc
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@days_to_pay",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.days_to_pay
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

        public static bool Update(SqlConnection connection, Credit_Term_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            sQuery.Append(@"UPDATE a_Credit_Term SET
                             credit_term = @credit_term
                             ,term_desc = @term_desc
							 ,days_to_pay = @days_to_pay
                             WHERE credit_term = @credit_term ");

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@credit_term",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.credit_term
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@term_desc",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.term_desc
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@days_to_pay",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.days_to_pay
                    };
                    cmd.Parameters.Add(parm3);

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

        public static bool Delete(SqlConnection connection, string credit_term)
        {
            bool boolReturnValue = false;

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Credit_Term ");
            sQuery.Append("WHERE credit_term = @credit_term");
                   
            var GUID = SQL_Transact.GenerateGUID();

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
                        ParameterName = "@credit_term",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = credit_term
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
                    SQL_Transact.RollbackTransaction(connection, GUID);
                }
            }

            return boolReturnValue;
        }
    }
}
