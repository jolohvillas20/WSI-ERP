using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class Item_Subclass
    {
        public static List<Item_Subclass_Model> RetrieveData(SqlConnection connection, string idClass, string searchSubclass, string idSubclass)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idSubclass
                         ,Subclass_Name
                         FROM a_Item_Subclass
                         WHERE idSubclass <> 0
                         ");

            if (idClass != "")
            {
                sQuery.Append(" AND idClass = @idClass ");
            }

            if (idSubclass != "")
            {
                sQuery.Append(" AND idSubclass = @idSubclass ");
            }

            if (searchSubclass != "")
            {
                sQuery.Append(" AND Subclass_Name LIKE '%' + @searchSubclass + '%' ");
            }

            sQuery.Append(" ORDER BY Subclass_Name ");

            var lmodel = new List<Item_Subclass_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (idClass != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idClass",
                        SqlDbType = SqlDbType.Int,
                        Value = idClass
                    };
                    cmd.Parameters.Add(parm1);
                }

                if (idSubclass != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idSubclass",
                        SqlDbType = SqlDbType.Int,
                        Value = idSubclass
                    };
                    cmd.Parameters.Add(parm1);
                }


                if (searchSubclass != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@searchSubclass",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = searchSubclass
                    };
                    cmd.Parameters.Add(parm1);
                }

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Item_Subclass_Model oModel = new Item_Subclass_Model
                    {

                        //oModel.idClass = (int)oreader["idClass"];
                        idSubclass = (int)oreader["idSubclass"],
                        Subclass_Name = (string)oreader["Subclass_Name"]
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static bool Save(SqlConnection connection, Item_Subclass_Model model)
        {
            bool returnValue = true;


            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_Item_Subclass
                             (Subclass_Name
                             ,idClass)
                             VALUES
                             (@Subclass_Name
                             ,@idClass)");

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Subclass_Name",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Subclass_Name
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@idClass",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idClass
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

        public static bool Update(SqlConnection connection, Item_Subclass_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"UPDATE a_Item_Subclass SET
                             Subclass_Name = @Subclass_Name                             
                             WHERE idSubclass = @idSubclass
                             AND idClass = @idClass ");

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idSubclass",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idSubclass
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Subclass_Name",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Subclass_Name
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@idClass",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idClass
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

        public static bool Delete(SqlConnection connection, int idSubclass)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Item_Subclass ");
            sQuery.Append("WHERE idSubclass = @idSubclass");

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
                        ParameterName = "@idSubclass",
                        SqlDbType = SqlDbType.Int,
                        Value = idSubclass
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
