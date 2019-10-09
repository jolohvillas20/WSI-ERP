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
    public class UM
    {
        public static DataTable RetrieveData(SqlConnection con)
        {
            DataTable dt = new DataTable();
            StringBuilder strQuery = new StringBuilder();

            strQuery.Append(@"SELECT UM
                            ,UM_Description
                            FROM a_UM
                            ORDER BY UM_Description");

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

        //public static List<UM_Model> RetrieveData(SqlConnection connection)
        //{
        //    StringBuilder sQuery = new StringBuilder();

        //    sQuery.Append(@"SELECT idUM
        //                 ,UM
        //                 ,UM_Description
        //                 FROM a_UM");

        //    var lmodel = new List<UM_Model>();

        //    connection.Open();

        //    using (SqlCommand cmd = new SqlCommand())
        //    {
        //        cmd.Connection = connection;
        //        cmd.CommandText = sQuery.ToString();
        //        cmd.CommandType = CommandType.Text;

        //        var oreader = cmd.ExecuteReader();

        //        while (oreader.Read())
        //        {
        //            UM_Model oModel = new UM_Model();

        //            oModel.idUM = (int)oreader["idUM"];
        //            oModel.UM = (string)oreader["UM"];
        //            oModel.UM_Description = (string)oreader["UM_Description"];
        //            lmodel.Add(oModel);
        //        }

        //        cmd.Dispose();
        //    }

        //    connection.Close();

        //    return lmodel;
        //}

        public static bool Save(SqlConnection connection, UM_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"INSERT INTO a_UM
                             (UM
                             ,UM_Description)
                             VALUES
                             (@UM
                             ,@UM_Description)");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@UM",
                        SqlDbType = SqlDbType.NChar,
                        Value = model.UM
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@UM_Description",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.UM_Description
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

        public static bool Update(SqlConnection connection, UM_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_UM SET
                             UM = @UM
                             ,UM_Description = @UM_Description
                             WHERE idUM = @idUM ");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idUM",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idUM
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@UM",
                        SqlDbType = SqlDbType.NChar,
                        Value = model.UM
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@UM_Description",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.UM_Description
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

        public static bool Delete(SqlConnection connection, int idUM)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_UM ");
            sQuery.Append("WHERE idUM = @idUM");

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
                        ParameterName = "@idUM",
                        SqlDbType = SqlDbType.Int,
                        Value = idUM
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
