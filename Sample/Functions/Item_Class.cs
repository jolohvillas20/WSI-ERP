using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class Item_Class
    {
        public static List<Item_Class_Model> RetrieveData(SqlConnection connection, string searchProduct, string domain)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT IC.idClass, IC.Product_Name
                         FROM a_Item_Class as IC
                         ");


            if (domain != "")
            {
                sQuery.Append(@"
                         INNER JOIN a_User_Product as UP ON IC.idClass = UP.idProduct
                         INNER JOIN a_Users as U ON UP.idUser = U.idUser
                         WHERE IC.idClass <> 0 AND U.User_Domain = @domain ");
                if (searchProduct != "")
                {
                    sQuery.Append(" AND IC.Product_Name LIKE '%' + @searchProduct + '%' ");
                }
            }
            else
            {
                if (searchProduct != "")
                {
                    sQuery.Append(" WHERE IC.Product_Name LIKE '%' + @searchProduct + '%' ");
                }
            }

            sQuery.Append(" ORDER BY IC.Product_Name ");

            var lmodel = new List<Item_Class_Model>();

            DataTable dataTable = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (searchProduct != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@searchProduct",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = searchProduct
                    };
                    cmd.Parameters.Add(parm1);
                }

                if (domain != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@domain",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = domain
                    };
                    cmd.Parameters.Add(parm2);
                }

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Item_Class_Model oModel = new Item_Class_Model
                    {
                        idClass = (int)oreader["idClass"],
                        Product_Name = (string)oreader["Product_Name"]
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static bool Save(SqlConnection connection, Item_Class_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_Item_Class
                             (Product_Name)
                             VALUES
                             (@Product_Name)");

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Product_Name",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Product_Name
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

        public static bool Update(SqlConnection connection, Item_Class_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"UPDATE a_Item_Class SET
                              Product_Name = @Product_Name
                             WHERE idClass = @idClass ");


            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idClass",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idClass
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Product_Name",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Product_Name
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

        public static bool Delete(SqlConnection connection, int idClass)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Item_Class ");
            sQuery.Append("WHERE idClass = @idClass");

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
                        ParameterName = "@idClass",
                        SqlDbType = SqlDbType.Int,
                        Value = idClass
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
