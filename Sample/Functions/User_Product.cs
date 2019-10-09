using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class User_Product
    {
        public static DataTable RetrieveData(SqlConnection connection)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT a.idUserProduct
                         ,a.idProduct
                         ,a.idUser
                         ,(SELECT b.Product_Name FROM a_Item_Class as b WHERE b.idClass = a.idProduct) as 'Product Name'
                         ,(SELECT c.User_Name FROM a_Users as c WHERE c.idUser = a.idUser) as 'Name'
                         FROM a_User_Product as a");

            var lmodel = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                lmodel.Load(cmd.ExecuteReader());
                //var oreader = cmd.ExecuteReader();

                //while (oreader.Read())
                //{
                //    User_Product_Model oModel = new User_Product_Model
                //    {
                //        idUserProduct = (int)oreader["idUserProduct"],
                //        idProduct = (int)oreader["idProduct"],
                //        idUser = (int)oreader["idUser"]
                //    };
                //    lmodel.Add(oModel);
                //}
                //oreader.Close();
                //cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static bool Save(SqlConnection connection, User_Product_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"INSERT INTO a_User_Product
                             (idProduct
                             ,idUser)
                             VALUES
                             (@idProduct
                             ,@idUser)");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idProduct",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idProduct
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@idUser",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idUser
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

        public static bool Update(SqlConnection connection, User_Product_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_User_Product SET
                             idProduct = @idProduct
                             ,idUser = @idUser
                             WHERE idUserProduct = @idUserProduct ");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idUserProduct",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idUserProduct
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idProduct",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idProduct
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@idUser",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idUser
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

        public static bool Delete(SqlConnection connection, int idUserProduct)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_User_Product ");
            sQuery.Append("WHERE idUserProduct = @idUserProduct");

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
                        ParameterName = "@idUserProduct",
                        SqlDbType = SqlDbType.Int,
                        Value = idUserProduct
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
