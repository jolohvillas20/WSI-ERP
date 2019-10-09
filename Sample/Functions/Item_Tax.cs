using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class Item_Tax
    {
        public static List<Item_Tax_Model> RetrieveData(SqlConnection con)
        {
            List<Item_Tax_Model> dt = new List<Item_Tax_Model>();
            StringBuilder strQuery = new StringBuilder();

            strQuery.Append(@"SELECT Tax FROM a_Item_Tax");

            con.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = strQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    var oreader = cmd.ExecuteReader();

                    while (oreader.Read())
                    {
                        Item_Tax_Model oModel = new Item_Tax_Model
                        {
                            Tax = (decimal)oreader["Tax"]
                        };

                        dt.Add(oModel);
                    }
                    oreader.Close();
                    cmd.Dispose();
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


        public static bool Save(SqlConnection connection, Item_Tax_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"INSERT INTO a_Item_Tax
                             (Tax)
                             VALUES
                             (@Tax)");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Tax",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Tax
                    };
                    cmd.Parameters.Add(parm2);

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

        public static bool Update(SqlConnection connection, Item_Tax_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_Item_Tax SET
                             Tax = @Tax
                             WHERE idTax = @idTax ");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idTax",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idTax
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Tax",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Tax
                    };
                    cmd.Parameters.Add(parm2);

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

        public static bool Delete(SqlConnection connection, int idTax)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Item_Tax ");
            sQuery.Append("WHERE idTax = @idTax");

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
                        ParameterName = "@idTax",
                        SqlDbType = SqlDbType.Int,
                        Value = idTax
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
