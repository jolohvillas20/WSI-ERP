using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class Login1
    {
        public string GetEncryptedPassword(SqlConnection con, string User_Domain)
        {
            string encPass = "";
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT User_Password
                         FROM a_Users 
                         WHERE User_Domain = @User_Domain 
                         ");

            con.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@User_Domain",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = User_Domain
                    };
                    cmd.Parameters.Add(parm2);
                    var oreader = cmd.ExecuteReader();

                    while (oreader.Read())
                    {
                        encPass = (string)oreader["User_Password"];
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

            return encPass;
        }

        //public bool LoginAuthentication(string username, string password, SqlConnection connection)
        //{
        //    bool returnValue = false;

        //    StringBuilder sQuery = new StringBuilder();

        //    sQuery.Append(@"SELECT *
        //                 FROM a_Users 
        //                 WHERE User_Domain = @User_Domain 
        //                 AND User_Password = @User_Password");

        //    var lmodel = new List<Customer_Details_Model>();

        //    connection.Open();
        //    try
        //    {
        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            cmd.Connection = connection;
        //            cmd.CommandText = sQuery.ToString();
        //            cmd.CommandType = CommandType.Text;

        //            SqlParameter parm2 = new SqlParameter
        //            {
        //                ParameterName = "@User_Domain",
        //                SqlDbType = SqlDbType.NVarChar,
        //                Value = username
        //            };
        //            cmd.Parameters.Add(parm2);

        //            SqlParameter parm3 = new SqlParameter
        //            {
        //                ParameterName = "@User_Password",
        //                SqlDbType = SqlDbType.NVarChar,
        //                Value = password
        //            };
        //            cmd.Parameters.Add(parm3);

        //            var oreader = cmd.ExecuteReader();

        //            while (oreader.Read())
        //            {
        //                returnValue = true;                        
        //            }

        //            cmd.Dispose();
        //        }
        //    }
        //    catch
        //    {
        //        returnValue = false;
        //    }

        //    connection.Close();

        //    return returnValue;
        //}

        public string GetUserAccess(string User_Domain, SqlConnection connection)
        {
            string User_Access = "";

            StringBuilder sQuery = new StringBuilder();


            sQuery.Append(@"SELECT b.User_Access
                         FROM a_Users a
                         INNER JOIN a_User_Access b
                         ON a.idUser = b.idUser
                         WHERE a.User_Domain = @User_Domain");

            var lmodel = new List<Customer_Details_Model>();

            connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@User_Domain",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = User_Domain
                    };
                    cmd.Parameters.Add(parm2);

                    var oreader = cmd.ExecuteReader();

                    while (oreader.Read())
                    {
                        User_Access = (string)oreader["User_Access"];
                    }
                    oreader.Close();
                    cmd.Dispose();
                }
            }
            catch
            {
                User_Access = "";
            }

            connection.Close();

            return User_Access;
        }

        public static List<Login_Model> getUserInformation(SqlConnection connection, string user_domain)
        {
            var returnvalue = new List<Login_Model>();

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT u.User_Name
                           ,u.User_Email                           
                           ,u.User_Access
                           ,u.idUser
                           FROM a_Users as u LEFT JOIN a_User_Product as up ON u.idUser = up.idUser
                           WHERE User_Domain = @user_domain");

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm1 = new SqlParameter
                {
                    ParameterName = "@user_domain",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = user_domain
                };
                cmd.Parameters.Add(parm1);

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Login_Model oModel = new Login_Model
                    {
                        User_Name = (string)oreader["User_Name"],
                        User_Email = (string)oreader["User_Email"],
                        User_Access = (string)oreader["User_Access"],
                        idUser = (int)oreader["idUser"]
                    };

                    returnvalue.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();


            return returnvalue;

        }

        public int GetUserID(string User_Domain, String User_Name, SqlConnection connection)
        {
            int idUser = 0;

            StringBuilder sQuery = new StringBuilder();


            sQuery.Append(@"SELECT idUser
                         FROM a_Users 
                         WHERE User_Domain = @User_Domain AND User_Name = @User_Name");

            var lmodel = new List<Customer_Details_Model>();

            connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@User_Domain",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = User_Domain
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@User_Name",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = User_Name
                    };
                    cmd.Parameters.Add(parm2);

                    var oreader = cmd.ExecuteReader();

                    while (oreader.Read())
                    {
                        idUser = (int)oreader["idUser"];
                    }
                    oreader.Close();
                    cmd.Dispose();
                }
            }
            catch
            {
                idUser = 0;
            }

            connection.Close();

            return idUser;
        }

    }
}
