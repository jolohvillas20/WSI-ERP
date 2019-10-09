using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class Site_Loc
    {
        public static DataTable RetrieveData(SqlConnection con, string idSite)
        {
            DataTable dt = new DataTable();
            StringBuilder strQuery = new StringBuilder();

            strQuery.Append(@"SELECT idSite
                            ,Site_Name
                            ,Site_Desc
                            FROM a_Site");
            if (idSite != "")
                strQuery.Append(@" WHERE idSite = '" + idSite + "' ");

            strQuery.Append(@" ORDER BY idSite");

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

        public static List<Site_Model> RetrieveData(SqlConnection connection, int idSite)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idSite
                         ,Site_Name
                         ,Site_Desc
                         FROM a_Site ");

            if (idSite != 0)
            {
                sQuery.Append(" WHERE idSite = @idSite ");
            }
            var lmodel = new List<Site_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (idSite != 0)
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idSite",
                        SqlDbType = SqlDbType.Int,
                        Value = idSite
                    };
                    cmd.Parameters.Add(parm2);
                }

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Site_Model oModel = new Site_Model
                    {
                        idSite = (int)oreader["idSite"],
                        Site_Name = (string)oreader["Site_Name"],
                        Site_Desc = (string)oreader["Site_Desc"]
                    };

                    lmodel.Add(oModel);
                }

                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static bool Save(SqlConnection connection, Site_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"INSERT INTO a_Site
                             (Site_Name
                             ,Site_Desc)
                             VALUES
                             (@Site_Name
                             ,@Site_Desc)");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Site_Name",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Site_Name
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Site_Desc",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Site_Desc
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

        public static bool Update(SqlConnection connection, Site_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_Site SET
                             Site_Name = @Site_Name
                             ,Site_Desc = @Site_Desc
                             WHERE idSite = @idSite ");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idSite",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idSite
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Site_Name",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Site_Name
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Site_Desc",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Site_Desc
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

        public static bool Delete(SqlConnection connection, int idSite)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Site ");
            sQuery.Append("WHERE idSite = @idSite");

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
                        ParameterName = "@idSite",
                        SqlDbType = SqlDbType.Int,
                        Value = idSite
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
