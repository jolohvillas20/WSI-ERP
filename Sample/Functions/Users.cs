using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class Users
    {
        public static DataTable RetrieveData(SqlConnection connection)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idUser
                         ,User_Name
                         ,User_Email
                         ,User_Domain
                         ,User_Access
                         FROM a_Users");

            var dt = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                //var oreader = cmd.ExecuteReader();

                dt.Load(cmd.ExecuteReader());

                //while (oreader.Read())
                //{
                //    Users_Model oModel = new Users_Model
                //    {
                //        idUser = (int)oreader["idUser"],
                //        User_Name = (string)oreader["User_Name"],
                //        User_Email = (string)oreader["User_Email"],
                //        User_Domain = (string)oreader["User_Domain"],
                //        User_Access = (string)oreader["User_Access"]
                //    };
                //    lmodel.Add(oModel);
                //}

                cmd.Dispose();
            }

            connection.Close();

            return dt;
        }

        public static bool Save(SqlConnection connection, Users_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();


            sQuery.Append(@"INSERT INTO a_Users
                             (User_Name
                             ,User_Email
                             ,User_Access
                             ,User_Domain
                             ,User_Password)
                             VALUES
                             (@User_Name
                             ,@User_Email
                             ,@User_Access
                             ,@User_Domain
                             ,@User_Password)");

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);


            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@User_Name",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.User_Name
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@User_Email",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.User_Email
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@User_Access",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.User_Access
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@User_Domain",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.User_Domain
                    };
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@User_Password",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.User_Password
                    };
                    cmd.Parameters.Add(parm6);

                    if (cmd.ExecuteNonQuery() >= 1)
                    {
                        returnValue = true;
                        cmd.Dispose();
                        cmd.Parameters.Clear();
                        SQL_Transact.CommitTransaction(connection, GUID);

                    }
                }
                catch (Exception ex)
                {
                    cmd.Dispose();
                    cmd.Parameters.Clear();
                    SQL_Transact.RollbackTransaction(connection, GUID);
                    throw new ArgumentException(ex.Message);
                }
            }

            return returnValue;
        }

        public static bool Update(SqlConnection connection, Users_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();


            sQuery.Append(@"UPDATE a_Users SET
                             User_Name = @User_Name
                             ,User_Email = @User_Email
                             ,User_Access = @User_Access
                             ,User_Domain = @User_Domain
                             ,User_Password = @User_Password
                             WHERE idUser = @idUser ");

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idUser",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idUser
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@User_Name",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.User_Name
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@User_Email",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.User_Email
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@User_Access",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.User_Access
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@User_Domain",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.User_Domain
                    };
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@User_Password",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.User_Password
                    };
                    cmd.Parameters.Add(parm6);

                    if (cmd.ExecuteNonQuery() >= 1)
                    {
                        returnValue = true;
                        cmd.Dispose();
                        cmd.Parameters.Clear();
                        SQL_Transact.CommitTransaction(connection, GUID);
                    }
                }
                catch (Exception ex)
                {
                    cmd.Dispose();
                    cmd.Parameters.Clear();
                    SQL_Transact.RollbackTransaction(connection, GUID);
                    throw new ArgumentException(ex.Message);
                }
            }

            return returnValue;
        }

        public static bool Delete(SqlConnection connection, int idUser)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Users ");
            sQuery.Append("WHERE idUser = @idUser");

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
                        ParameterName = "@idUser",
                        SqlDbType = SqlDbType.Int,
                        Value = idUser
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

        public static DataTable dropDownUser(SqlConnection connection, int User_Product)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT u.idUser
                         ,u.User_Name                        
                         FROM a_Users as u 
                         INNER JOIN a_User_Product as up 
                         ON u.idUser = up.idUser 
                         ");

            if (User_Product != 0)
            {
                sQuery.Append(@" WHERE up.idProduct = @User_Product ");
            }

            var dt = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;
                if (User_Product != 0)
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@User_Product",
                        SqlDbType = SqlDbType.Int,
                        Value = User_Product
                    };
                    cmd.Parameters.Add(parm1);
                }
                dt.Load(cmd.ExecuteReader());

                //while (oreader.Read())
                //{
                //    Users_Model oModel = new Users_Model
                //    {
                //        idUser = (int)oreader["idUser"],
                //        User_Name = (string)oreader["User_Name"],
                //        User_Email = (string)oreader["User_Email"],
                //        User_Domain = (string)oreader["User_Domain"],
                //        User_Access = (string)oreader["User_Access"]
                //    };
                //    lmodel.Add(oModel);
                //}

                cmd.Dispose();
            }

            connection.Close();

            return dt;
        }

        public static DataTable GetUserWithProduct(SqlConnection connection)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT u.idUser
                         ,u.User_Name                        
                         FROM a_Users as u 
                         INNER JOIN a_User_Product as up 
                         ON u.idUser = up.idUser 
                         ");

            var dt = new DataTable();

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

        public static int GetidProduct(int idUser, SqlConnection connection)
        {
            int idProduct = 0;

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT up.idProduct                       
                          FROM a_Users as u 
                          INNER JOIN a_User_Product as up 
                          ON u.idUser = up.idUser 
                          WHERE u.idUser = @idUser");

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
                        ParameterName = "@idUser",
                        SqlDbType = SqlDbType.Int,
                        Value = idUser
                    };
                    cmd.Parameters.Add(parm1);

                    var oreader = cmd.ExecuteReader();

                    while (oreader.Read())
                    {
                        idProduct = (int)oreader["idProduct"];
                    }
                    oreader.Close();
                    cmd.Dispose();
                }
            }
            catch
            {
                idProduct = 0;
            }

            connection.Close();

            return idProduct;
        }

        public static int GetUserIDByDomainLogin(SqlConnection connection, string User_Domain)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idUser FROM a_Users
                            WHERE User_Domain = @User_Domain ");

            int resultValue = 0;
            connection.Open();
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

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    resultValue = (int)oreader["idUser"];
                }
                oreader.Close();
                cmd.Dispose();
            }
            connection.Close();
            return resultValue;
        }
    }
}
