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
    public class Additional_Charge
    {
        public static DataTable RetrieveData(SqlConnection connection)
        {
            StringBuilder sQuery = new StringBuilder();
                   
            sQuery.Append(@"SELECT idAddCharge
                          ,Additional_Charge
                          FROM a_Additional_Charge
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

        public static bool Save(SqlConnection connection, Additional_Charge_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"INSERT INTO a_Additional_Charge
                             (Additional_Charge)
                             VALUES
                             (@Additional_Charge)");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Additional_Charge",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Additional_Charge
                    };
                    cmd.Parameters.Add(parm3);

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

        public static bool Update(SqlConnection connection, Additional_Charge_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_Additional_Charge SET
                              Additional_Charge = @Additional_Charge
                             WHERE idAddCharge = @idAddCharge ");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idAddCharge",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idAddCharge
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Additional_Charge",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Additional_Charge
                    };
                    cmd.Parameters.Add(parm3);

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

        public static bool Delete(SqlConnection connection, int idAddCharge)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Additional_Charge ");
            sQuery.Append("WHERE idAddCharge = @idAddCharge");

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
                        ParameterName = "@idAddCharge",
                        SqlDbType = SqlDbType.Int,
                        Value = idAddCharge
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
