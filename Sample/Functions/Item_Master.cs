using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class Item_Master
    {
        public static DataTable RetrieveData(SqlConnection connection, string idClass, string idSubclass, string searchItem, int idItem)
        {
            StringBuilder sQuery = new StringBuilder();
            //= (SELECT b.Site_Name FROM a_Site b WHERE b.idSite = a.Site)
            sQuery.Append(@"SELECT DISTINCT a.idItem
                         ,a.idClass
                         ,a.idSubClass
                         ,a.Item_Number
                         ,a.Principal_SKU
                         ,a.Description
                         ,Site = (SELECT b.Site_Name FROM a_Site b WHERE b.idSite = a.Site)
                         ,a.Unit_Weight
                         ,a.Weight_UM
                         ,a.UM
                         ,a.OnHand_Qty
                         ,a.Alloc_Qty
                         ,a.Total_Qty
                         ,a.Ave_Cost
                         ,a.Total_Cost
                         ,Age = (DATEDIFF(DAY, (SELECT TOP 1 timestamp FROM a_Item_Serial as b WHERE a.idItem = b.idItem AND Pick_Status = 'N' AND InStock = 'Y'), GETDATE()))
                         FROM a_Item_Master as a
                         WHERE a.idItem <> 0
                         ");

            if (idClass != "")
            {
                sQuery.Append(" AND a.idClass = @idClass ");
            }

            if (idSubclass != "")
            {
                sQuery.Append(" AND a.idSubclass = @idSubclass ");
            }

            if (searchItem != "")
            {
                sQuery.Append(" AND a.Item_Number LIKE '%' + @searchItem + '%' ");
            }

            if (idItem != 0)
            {
                sQuery.Append(" AND a.idItem = @idItem ");
            }

            sQuery.Append(" ORDER BY a.Item_Number ");

            var lmodel = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (idClass != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idClass",
                        SqlDbType = SqlDbType.Int,
                        Value = idClass
                    };
                    cmd.Parameters.Add(parm2);
                }

                if (idSubclass != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idSubClass",
                        SqlDbType = SqlDbType.Int,
                        Value = idSubclass
                    };
                    cmd.Parameters.Add(parm2);
                }

                if (searchItem != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@searchItem",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = searchItem
                    };
                    cmd.Parameters.Add(parm2);
                }

                if (idItem != 0)
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = idItem
                    };
                    cmd.Parameters.Add(parm2);
                }


                lmodel.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        private static DataTable RetrieveData(SqlConnection connection, string idSubclass, string Principal_SKU)
        {
            StringBuilder sQuery = new StringBuilder();
            //= (SELECT b.Site_Name FROM a_Site b WHERE b.idSite = a.Site)
            sQuery.Append(@"SELECT DISTINCT a.idItem
                         ,a.idClass
                         ,a.idSubClass
                         ,a.Item_Number
                         ,a.Principal_SKU
                         ,a.Description
                         ,Site = (SELECT b.Site_Name FROM a_Site b WHERE b.idSite = a.Site)
                         ,a.Unit_Weight
                         ,a.Weight_UM
                         ,a.UM
                         ,a.OnHand_Qty
                         ,a.Alloc_Qty
                         ,a.Total_Qty
                         ,a.Ave_Cost
                         ,a.Total_Cost
                         FROM a_Item_Master as a
                         WHERE a.idItem <> 0
                         ");

            if (idSubclass != "")
            {
                sQuery.Append(" AND a.idSubclass = @idSubclass ");
            }

            if (Principal_SKU != "")
            {
                sQuery.Append(" AND a.Principal_SKU = @Principal_SKU ");
            }

            sQuery.Append(" ORDER BY a.Item_Number ");

            var lmodel = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (idSubclass != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idSubClass",
                        SqlDbType = SqlDbType.Int,
                        Value = idSubclass
                    };
                    cmd.Parameters.Add(parm2);
                }

                if (Principal_SKU != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Principal_SKU",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Principal_SKU
                    };
                    cmd.Parameters.Add(parm2);
                }


                lmodel.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static List<Item_Master_Model> RetrieveData(SqlConnection connection, string Item_Number)
        {
            StringBuilder sQuery = new StringBuilder();
            sQuery.Append(@"SELECT DISTINCT idItem
                         ,idClass
                         ,idSubClass
                         ,Item_Number
                         ,Principal_SKU
                         ,Description
                         ,Site
                         ,Unit_Weight
                         ,Weight_UM
                         ,UM
                         ,OnHand_Qty
                         ,Alloc_Qty
                         ,Total_Qty
                         ,Ave_Cost
                         ,Total_Cost
                         FROM a_Item_Master
                         WHERE idItem <> 0
                         ");

            if (Item_Number != "")
            {
                sQuery.Append(" AND Item_Number LIKE '%' + @Item_Number + '%' ");
            }

            sQuery.Append(" ORDER BY Item_Number ");

            var lmodel = new List<Item_Master_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (Item_Number != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Item_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Item_Number
                    };
                    cmd.Parameters.Add(parm2);
                }

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Item_Master_Model oModel = new Item_Master_Model
                    {
                        idClass = (int)oreader["idClass"],
                        idSubClass = (int)oreader["idSubClass"],
                        idItem = (int)oreader["idItem"],
                        ItemNumber = (string)oreader["Item_Number"],
                        Principal_SKU = (string)oreader["Principal_SKU"],
                        Description = (string)oreader["Description"],
                        Site = (string)oreader["Site"],
                        Unit_Weight = (decimal)oreader["Unit_Weight"],
                        Weight_UM = (string)oreader["Weight_UM"],
                        UM = (string)oreader["UM"],
                        OnHand_Qty = (int)oreader["OnHand_Qty"],
                        Alloc_Qty = (int)oreader["Alloc_Qty"],
                        Total_Qty = (int)oreader["Total_Qty"],
                        Ave_Cost = oreader["Ave_Cost"].ToString() == "" ? 0 : (decimal)oreader["Ave_Cost"],
                        Total_Cost = oreader["Total_Cost"].ToString() == "" ? 0 : (decimal)oreader["Total_Cost"]
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();

                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static List<Item_Master_Model> RetrieveData(SqlConnection connection, int idItem)
        {
            StringBuilder sQuery = new StringBuilder();
            sQuery.Append(@"SELECT DISTINCT idItem
                         ,idClass
                         ,idSubClass
                         ,Item_Number
                         ,Principal_SKU
                         ,Description
                         ,Site
                         ,Unit_Weight
                         ,Weight_UM
                         ,UM
                         ,OnHand_Qty
                         ,Alloc_Qty
                         ,Total_Qty
                         ,Ave_Cost
                         ,Total_Cost
                         FROM a_Item_Master
                         WHERE idItem <> 0
                         ");

            if (idItem != 0)
            {
                sQuery.Append(" AND idItem = @idItem ");
            }

            sQuery.Append(" ORDER BY Item_Number ");

            var lmodel = new List<Item_Master_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (idItem != 0)
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = idItem
                    };
                    cmd.Parameters.Add(parm2);
                }

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Item_Master_Model oModel = new Item_Master_Model
                    {
                        idClass = (int)oreader["idClass"],
                        idSubClass = (int)oreader["idSubClass"],
                        idItem = (int)oreader["idItem"],
                        ItemNumber = (string)oreader["Item_Number"],
                        Principal_SKU = (string)oreader["Principal_SKU"],
                        Description = (string)oreader["Description"],
                        Site = (string)oreader["Site"],
                        Unit_Weight = (decimal)oreader["Unit_Weight"],
                        Weight_UM = (string)oreader["Weight_UM"],
                        UM = (string)oreader["UM"],
                        OnHand_Qty = (int)oreader["OnHand_Qty"],
                        Alloc_Qty = (int)oreader["Alloc_Qty"],
                        Total_Qty = (int)oreader["Total_Qty"],
                        Ave_Cost = oreader["Ave_Cost"].ToString() == "" ? 0 : (decimal)oreader["Ave_Cost"],
                        Total_Cost = oreader["Total_Cost"].ToString() == "" ? 0 : (decimal)oreader["Total_Cost"]
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();

                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static int Save(SqlConnection connection, Item_Master_Model model)
        {
            int returnValue = 0;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_Item_Master
                             (idClass
                             ,idSubClass
                             ,Item_Number
                             ,Principal_SKU
                             ,Description
                             ,Site
                             ,Unit_Weight
                             ,Weight_UM
                             ,UM
                             ,OnHand_Qty
                             ,Alloc_Qty
                             ,Total_Qty
                             ,Ave_Cost
                             ,Total_Cost)
                             VALUES
                             (@idClass
                             ,@idSubClass
                             ,@ItemNumber
                             ,@Principal_SKU
                             ,@Description
                             ,@Site
                             ,@Unit_Weight
                             ,@Weight_UM
                             ,@UM
                             ,@OnHand_Qty
                             ,@Alloc_Qty
                             ,@Total_Qty
                             ,@Ave_Cost
                             ,@Total_Cost)


                             SELECT SCOPE_IDENTITY() as 'ID'

");

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@idSubClass",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idSubClass
                };
                cmd.Parameters.Add(parm2);

                SqlParameter parm3 = new SqlParameter
                {
                    ParameterName = "@ItemNumber",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.ItemNumber
                };
                cmd.Parameters.Add(parm3);

                SqlParameter parm4 = new SqlParameter
                {
                    ParameterName = "@idClass",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idClass
                };
                cmd.Parameters.Add(parm4);

                SqlParameter parm5 = new SqlParameter
                {
                    ParameterName = "@Description",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Description
                };
                cmd.Parameters.Add(parm5);

                SqlParameter parm6 = new SqlParameter
                {
                    ParameterName = "@Site",
                    SqlDbType = SqlDbType.Int,
                    Value = model.Site
                };
                cmd.Parameters.Add(parm6);

                SqlParameter parm7 = new SqlParameter
                {
                    ParameterName = "@Principal_SKU",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Principal_SKU
                };
                cmd.Parameters.Add(parm7);

                SqlParameter parm8 = new SqlParameter
                {
                    ParameterName = "@Unit_Weight",
                    SqlDbType = SqlDbType.Decimal,
                    Value = model.Unit_Weight
                };
                cmd.Parameters.Add(parm8);

                SqlParameter parm9 = new SqlParameter
                {
                    ParameterName = "@Weight_UM",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Weight_UM
                };
                cmd.Parameters.Add(parm9);

                SqlParameter parm10 = new SqlParameter
                {
                    ParameterName = "@UM",
                    SqlDbType = SqlDbType.NChar,
                    Value = model.UM
                };
                cmd.Parameters.Add(parm10);

                SqlParameter parm11 = new SqlParameter
                {
                    ParameterName = "@OnHand_Qty",
                    SqlDbType = SqlDbType.Int,
                    Value = model.OnHand_Qty
                };
                cmd.Parameters.Add(parm11);

                SqlParameter parm12 = new SqlParameter
                {
                    ParameterName = "@Alloc_Qty",
                    SqlDbType = SqlDbType.Int,
                    Value = model.Alloc_Qty
                };
                cmd.Parameters.Add(parm12);

                SqlParameter parm13 = new SqlParameter
                {
                    ParameterName = "@Total_Qty",
                    SqlDbType = SqlDbType.Int,
                    Value = model.Total_Qty
                };
                cmd.Parameters.Add(parm13);

                SqlParameter parm14 = new SqlParameter
                {
                    ParameterName = "@Ave_Cost",
                    SqlDbType = SqlDbType.Int,
                    Value = model.Ave_Cost
                };
                cmd.Parameters.Add(parm14);

                SqlParameter parm15 = new SqlParameter
                {
                    ParameterName = "@Total_Cost",
                    SqlDbType = SqlDbType.Int,
                    Value = model.Total_Cost
                };
                cmd.Parameters.Add(parm15);

                //if (cmd.ExecuteNonQuery() >= 1)
                //{
                //    returnValue = true;
                //    cmd.Dispose();
                //    cmd.Parameters.Clear();
                //    SQL_Transact.CommitTransaction(connection, GUID);
                //}
                var oreader = cmd.ExecuteReader();
                try
                {
                    while (oreader.Read())
                    {
                        returnValue = Convert.ToInt32(oreader["ID"].ToString());
                    }
                    oreader.Close();
                    cmd.Dispose();
                    cmd.Parameters.Clear();
                    SQL_Transact.CommitTransaction(connection, GUID);
                }
                catch
                {
                    oreader.Close();
                    cmd.Dispose();
                    cmd.Parameters.Clear();
                    SQL_Transact.RollbackTransaction(connection, GUID);
                }
            }

            return returnValue;
        }

        public static bool Update(SqlConnection connection, Item_Master_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_Item_Master SET
                             idClass = @idClass
                             ,idSubClass = @idSubClass
                             ,Item_Number = @ItemNumber
                             ,Principal_SKU = @Principal_SKU
                             ,Description = @Description
                             ,Site = @Site
                             ,Unit_Weight = @Unit_Weight
                             ,Weight_UM = @Weight_UM
                             ,UM = @UM
                             ,OnHand_Qty = @OnHand_Qty
                             ,Alloc_Qty = @Alloc_Qty
                             ,Total_Qty = @Total_Qty
                             ,Ave_Cost = @Ave_Cost
                             ,Total_Cost = @Total_Cost
                             WHERE idItem = @idItem ");

                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idItem
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idSubClass",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idSubClass
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@ItemNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.ItemNumber
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@idClass",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idClass
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@Description",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Description
                    };
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@Site",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Site
                    };
                    cmd.Parameters.Add(parm6);

                    SqlParameter parm7 = new SqlParameter
                    {
                        ParameterName = "@Principal_SKU",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Principal_SKU
                    };
                    cmd.Parameters.Add(parm7);

                    SqlParameter parm8 = new SqlParameter
                    {
                        ParameterName = "@Unit_Weight",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Unit_Weight
                    };
                    cmd.Parameters.Add(parm8);

                    SqlParameter parm9 = new SqlParameter
                    {
                        ParameterName = "@Weight_UM",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Weight_UM
                    };
                    cmd.Parameters.Add(parm9);

                    SqlParameter parm10 = new SqlParameter
                    {
                        ParameterName = "@UM",
                        SqlDbType = SqlDbType.NChar,
                        Value = model.UM
                    };
                    cmd.Parameters.Add(parm10);

                    SqlParameter parm11 = new SqlParameter
                    {
                        ParameterName = "@OnHand_Qty",
                        SqlDbType = SqlDbType.Int,
                        Value = model.OnHand_Qty
                    };
                    cmd.Parameters.Add(parm11);

                    SqlParameter parm12 = new SqlParameter
                    {
                        ParameterName = "@Alloc_Qty",
                        SqlDbType = SqlDbType.Int,
                        Value = model.Alloc_Qty
                    };
                    cmd.Parameters.Add(parm12);

                    SqlParameter parm13 = new SqlParameter
                    {
                        ParameterName = "@Total_Qty",
                        SqlDbType = SqlDbType.Int,
                        Value = model.Total_Qty
                    };
                    cmd.Parameters.Add(parm13);

                    SqlParameter parm14 = new SqlParameter
                    {
                        ParameterName = "@Ave_Cost",
                        SqlDbType = SqlDbType.Int,
                        Value = model.Ave_Cost
                    };
                    cmd.Parameters.Add(parm14);

                    SqlParameter parm15 = new SqlParameter
                    {
                        ParameterName = "@Total_Cost",
                        SqlDbType = SqlDbType.Int,
                        Value = model.Total_Cost
                    };
                    cmd.Parameters.Add(parm15);

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

        public static bool Delete(SqlConnection connection, int idItem)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Item_Master ");
            sQuery.Append("WHERE idItem = @idItem");

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
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = idItem
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

        public static bool UpdateQtyOnly(SqlConnection connection, Item_Master_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"UPDATE a_Item_Master SET
                             Alloc_Qty = @Alloc_Qty
                             ,Total_Qty = @Total_Qty
                             WHERE idItem = @idItem ");

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
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idItem
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Alloc_Qty",
                        SqlDbType = SqlDbType.Int,
                        Value = model.Alloc_Qty
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Total_Qty",
                        SqlDbType = SqlDbType.Int,
                        Value = model.Total_Qty
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

        public static bool RecomputeItemCost(SqlConnection connection, int idItem)
        {
            bool returnvalue = false;
            decimal Ave_Cost = 0;
            decimal Total_Cost = 0;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT Unit_Cost
                           FROM a_Item_Serial 
                           WHERE idItem = @idItem 
                           AND Pick_Status = 'N'
AND InStock = 'Y'
                           ");

            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = idItem
                    };
                    cmd.Parameters.Add(parm2);

                    dt.Load(cmd.ExecuteReader());

                    cmd.Dispose();
                    SQL_Transact.CommitTransaction(connection, GUID);
                }
                catch
                {
                    cmd.Dispose();
                    SQL_Transact.RollbackTransaction(connection, GUID);
                }
            }

            if (dt.Rows.Count > 0)
            {
                for (int x = 0; x <= dt.Rows.Count - 1; x++)
                {
                    Total_Cost = Total_Cost + Convert.ToDecimal(dt.Rows[x][0].ToString());
                }
                sQuery.Clear();
                Ave_Cost = Total_Cost / dt.Rows.Count;

                sQuery.Append(@"UPDATE a_Item_Master SET
                             Ave_Cost = @Ave_Cost
                             ,Total_Cost = @Total_Cost
                             WHERE idItem = @idItem 
");

                GUID = SQL_Transact.GenerateGUID();

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
                            ParameterName = "@Ave_Cost",
                            SqlDbType = SqlDbType.Decimal,
                            Value = Ave_Cost
                        };
                        cmd.Parameters.Add(parm1);

                        SqlParameter parm2 = new SqlParameter
                        {
                            ParameterName = "@Total_Cost",
                            SqlDbType = SqlDbType.Decimal,
                            Value = Total_Cost
                        };
                        cmd.Parameters.Add(parm2);

                        SqlParameter parm3 = new SqlParameter
                        {
                            ParameterName = "@idItem",
                            SqlDbType = SqlDbType.Int,
                            Value = idItem
                        };
                        cmd.Parameters.Add(parm3);

                        if (cmd.ExecuteNonQuery() >= 1)
                        {
                            returnvalue = true;
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
            }

            return returnvalue;
        }

        public static bool checkItemNumber(SqlConnection connection, string Item_Number, string Description)
        {
            bool resultValue = false;

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT *
                            FROM a_Item_Master
                            WHERE Item_Number = @Item_Number AND 
                            Description = @Description
                           ");

            var lmodel = new List<Item_Master_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm = new SqlParameter
                {
                    ParameterName = "@Item_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = Item_Number
                };
                cmd.Parameters.Add(parm);

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@Description",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = Description
                };
                cmd.Parameters.Add(parm2);


                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    resultValue = true;
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return resultValue;
        }

        public static DataTable RetreiveAllocation(SqlConnection connection, int idItem)
        {
            DataTable resultValue = new DataTable();

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
                           SELECT idItem, OnHand_Qty, Alloc_Qty, Total_Qty FROM a_Item_Master WHERE idItem = @idItem
                           ");

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm3 = new SqlParameter
                {
                    ParameterName = "@idItem",
                    SqlDbType = SqlDbType.Int,
                    Value = idItem
                };
                cmd.Parameters.Add(parm3);

                resultValue.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return resultValue;
        }

        private static int SO_Qty(string idItem, SqlConnection oCon)
        {
            int resultValue = 0;

            var sQuery = new StringBuilder();

            sQuery.Append(@"
                           SELECT SUM(b.Qty) as Alloc_Qty 
                           FROM a_SO_Header as a 
                           INNER JOIN a_SO_Detail as b
                           ON a.idSOHeader = b.idSOHeader
                           INNER JOIN a_Item_Master as c 
                           ON b.idItem = c.idItem
                           WHERE c.idItem = @idItem
                           AND a.Pick_Status = 'N' AND a.Active = 'Y'

                           ");

            var oCommand = new SqlCommand(sQuery.ToString(), oCon);

            SqlParameter PidItem = new SqlParameter();
            PidItem.ParameterName = "@idItem";
            PidItem.SqlDbType = SqlDbType.Int;
            PidItem.Value = Convert.ToInt32(idItem);
            oCommand.Parameters.Add(PidItem);

            var oreader = oCommand.ExecuteReader();

            while (oreader.Read())
            {
                if (oreader.IsDBNull(0))
                {
                    resultValue = 0;
                }
                else
                {
                    resultValue = Convert.ToInt32(oreader[0].ToString());
                }
            }
            oreader.Close();

            return resultValue;
        }

        private static int OnHandQty(string idItem, SqlConnection oCon)
        {
            int resultValue = 0;

            var sQuery = new StringBuilder();

            sQuery.Append(@"
                           SELECT COUNT(idSerial) as On_hand_qty 
                           FROM a_Item_Serial 
                           WHERE idItem = @idItem 
                           AND Pick_Status = 'N'
AND InStock = 'Y'
                           ");

            var oCommand = new SqlCommand(sQuery.ToString(), oCon);

            SqlParameter PidItem = new SqlParameter();
            PidItem.ParameterName = "@idItem";
            PidItem.SqlDbType = SqlDbType.Int;
            PidItem.Value = Convert.ToInt32(idItem);
            oCommand.Parameters.Add(PidItem);

            var oreader = oCommand.ExecuteReader();

            while (oreader.Read())
            {
                if (oreader.IsDBNull(0))
                {
                    resultValue = 0;
                }
                else
                {
                    resultValue = Convert.ToInt32(oreader[0].ToString());
                }
            }

            oreader.Close();

            return resultValue;
        }

        private static DataTable ItemMaster(SqlConnection oCon)
        {
            DataTable resultValue = new DataTable();

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
                           SELECT idItem, Item_Number, OnHand_Qty, Alloc_Qty, Total_Qty FROM a_Item_Master

                           ");

            //SqlConnection connection = new SqlConnection(strConstr);
            oCon.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = oCon;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                resultValue.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }
            oCon.Close();
            return resultValue;
        }

        private static DataTable PrincipalSKU(SqlConnection oCon)
        {
            DataTable resultValue = new DataTable();

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
                           SELECT DISTINCT a.Principal_SKU, (SELECT TOP 1 idSubClass FROM a_Item_Master as b WHERE a.Principal_SKU = b.Principal_SKU) as idSubClass FROM a_Item_Master as a
                           ");

            //SqlConnection connection = new SqlConnection(strConstr);
            oCon.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = oCon;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                resultValue.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }
            oCon.Close();
            return resultValue;
        }

        private static bool CheckItemSite(SqlConnection oCon, string PrincipalSKU, string site)
        {
            bool resultValue = false;

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
                           SELECT * FROM a_Item_Master WHERE Principal_SKU = @PrincipalSKU AND Site = @site
                           ");

            //SqlConnection connection = new SqlConnection(strConstr);
            oCon.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = oCon;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@PrincipalSKU",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = PrincipalSKU
                };
                cmd.Parameters.Add(parm2);

                SqlParameter parm3 = new SqlParameter
                {
                    ParameterName = "@site",
                    SqlDbType = SqlDbType.Int,
                    Value = site
                };
                cmd.Parameters.Add(parm3);

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    resultValue = true;
                }
                oreader.Close();
            }
            oCon.Close();
            return resultValue;
        }

        public static void InventoryCheckForError(SqlConnection oCon)
        {
            DataTable itemMaster = ItemMaster(oCon);
            string idItem = "";
            //string ItemNumber = "";
            for (int x = 0; x <= itemMaster.Rows.Count - 1; x++)
            {
                var GUID = SQL_Transact.GenerateGUID();
                SQL_Transact.BeginTransaction(oCon, GUID);
                idItem = itemMaster.Rows[x][0].ToString();
                int SO_Alloc_Qty = SO_Qty(idItem, oCon);
                int Serial_On_Hand_Qty = OnHandQty(idItem, oCon);
                int Total_Qty = Serial_On_Hand_Qty - SO_Alloc_Qty;

                //if ((Master_OnHand != Serial_On_Hand_Qty) || (Master_Alloc != SO_Alloc_Qty) || (Master_Total != Total_Qty))
                //{
                var sQuery = new StringBuilder();

                sQuery.Append(" UPDATE a_Item_Master SET OnHand_Qty = @OnHandQty, Alloc_Qty = @AllocQty, Total_Qty = @TotalQty WHERE idItem = @idItem ");

                //SqlConnection oConnection = new SqlConnection(strConstr);
                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = oCon;
                        cmd.CommandText = sQuery.ToString();
                        cmd.CommandType = CommandType.Text;

                        SqlParameter parm1 = new SqlParameter
                        {
                            ParameterName = "@OnHandQty",
                            SqlDbType = SqlDbType.Int,
                            Value = Serial_On_Hand_Qty
                        };
                        cmd.Parameters.Add(parm1);

                        SqlParameter parm2 = new SqlParameter
                        {
                            ParameterName = "@AllocQty",
                            SqlDbType = SqlDbType.Int,
                            Value = SO_Alloc_Qty
                        };
                        cmd.Parameters.Add(parm2);

                        SqlParameter parm3 = new SqlParameter
                        {
                            ParameterName = "@TotalQty",
                            SqlDbType = SqlDbType.Int,
                            Value = Total_Qty
                        };
                        cmd.Parameters.Add(parm3);

                        SqlParameter parm4 = new SqlParameter
                        {
                            ParameterName = "@idItem",
                            SqlDbType = SqlDbType.Int,
                            Value = idItem
                        };
                        cmd.Parameters.Add(parm4);

                        if (cmd.ExecuteNonQuery() >= 1)
                            cmd.Dispose();
                        else
                            cmd.Dispose();

                    }
                    SQL_Transact.CommitTransaction(oCon, GUID);
                }
                catch
                {
                    SQL_Transact.RollbackTransaction(oCon, GUID);
                }
                decimal Ave_Cost = 0;
                decimal Total_Cost = 0;

                sQuery = new StringBuilder();

                sQuery.Append(@"SELECT Unit_Cost
                                    FROM a_Item_Serial 
                                    WHERE idItem = @idItem 
                                    AND Pick_Status = 'N'
                                    AND InStock = 'Y'
                                    ");

                DataTable dt = new DataTable();
                oCon.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = oCon;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = idItem
                    };
                    cmd.Parameters.Add(parm2);

                    dt.Load(cmd.ExecuteReader());

                    cmd.Dispose();
                }
                sQuery.Clear();
                oCon.Close();

                if (dt.Rows.Count > 0)
                {
                    for (int y = 0; y <= dt.Rows.Count - 1; y++)
                    {
                        Total_Cost = Total_Cost + Convert.ToDecimal(dt.Rows[y][0].ToString());
                    }

                    Ave_Cost = Total_Cost / dt.Rows.Count;

                    try
                    {
                        GUID = SQL_Transact.GenerateGUID();
                        SQL_Transact.BeginTransaction(oCon, GUID);
                        sQuery.Append(@"UPDATE a_Item_Master SET
                                        Ave_Cost = @Ave_Cost
                                        ,Total_Cost = @Total_Cost
                                        WHERE idItem = @idItem 
                                        ");

                        using (SqlCommand cmd = new SqlCommand())
                        {
                            try
                            {
                                cmd.Connection = oCon;
                                cmd.CommandText = sQuery.ToString();
                                cmd.CommandType = CommandType.Text;

                                SqlParameter parm1 = new SqlParameter
                                {
                                    ParameterName = "@Ave_Cost",
                                    SqlDbType = SqlDbType.Decimal,
                                    Value = Ave_Cost
                                };
                                cmd.Parameters.Add(parm1);

                                SqlParameter parm2 = new SqlParameter
                                {
                                    ParameterName = "@Total_Cost",
                                    SqlDbType = SqlDbType.Decimal,
                                    Value = Total_Cost
                                };
                                cmd.Parameters.Add(parm2);

                                SqlParameter parm3 = new SqlParameter
                                {
                                    ParameterName = "@idItem",
                                    SqlDbType = SqlDbType.Int,
                                    Value = idItem
                                };
                                cmd.Parameters.Add(parm3);

                                if (cmd.ExecuteNonQuery() >= 1)
                                    //boolReturnValue = true;
                                    cmd.Dispose();
                                cmd.Parameters.Clear();
                            }
                            catch
                            {
                                cmd.Dispose();
                                cmd.Parameters.Clear();
                            }
                        }

                        SQL_Transact.CommitTransaction(oCon, GUID);
                    }
                    catch
                    {
                        SQL_Transact.RollbackTransaction(oCon, GUID);
                    }
                }

            }

            DataTable prinSKU = PrincipalSKU(oCon);
            var site = Site_Loc.RetrieveData(oCon, 0);

            for (int x = 0; x <= prinSKU.Rows.Count - 1; x++)
            {
                for (int y = 0; y <= site.Count - 1; y++)
                {
                    string sku = prinSKU.Rows[x][0].ToString();
                    int ssite = site[y].idSite;
                    bool check = CheckItemSite(oCon, sku, ssite.ToString());
                    if (check == false)
                    {
                        DataTable itemmaster = RetrieveData(oCon, prinSKU.Rows[x][1].ToString(), prinSKU.Rows[x][0].ToString());
                        var isc = Item_Subclass.RetrieveData(oCon, itemmaster.Rows[0][1].ToString(), "", itemmaster.Rows[0][2].ToString());
                        if (ssite == 9)
                        {
                            Item_Master_Model imm = new Item_Master_Model()
                            {
                                idClass = Convert.ToInt32(itemmaster.Rows[0][1].ToString()),
                                idSubClass = 12,
                                ItemNumber = "JBW-" + itemmaster.Rows[0][4].ToString(),
                                Principal_SKU = itemmaster.Rows[0][4].ToString(),
                                Description = itemmaster.Rows[0][5].ToString(),
                                Site = ssite.ToString(),
                                Unit_Weight = 0,
                                Weight_UM = "",
                                UM = "UT",
                                OnHand_Qty = 0,
                                Alloc_Qty = 0,
                                Total_Qty = 0,
                                Ave_Cost = 0,
                                Total_Cost = 0
                            };
                            Save(oCon, imm);
                        }
                        else
                        {
                            Item_Master_Model imm = new Item_Master_Model()
                            {
                                idClass = Convert.ToInt32(itemmaster.Rows[0][1].ToString()),
                                idSubClass = Convert.ToInt32(itemmaster.Rows[0][2].ToString()),
                                ItemNumber = isc[0].Subclass_Name.Substring(0, 4) + "-" + itemmaster.Rows[0][4].ToString(),
                                Principal_SKU = itemmaster.Rows[0][4].ToString(),
                                Description = itemmaster.Rows[0][5].ToString(),
                                Site = ssite.ToString(),
                                Unit_Weight = 0,
                                Weight_UM = "",
                                UM = "UT",
                                OnHand_Qty = 0,
                                Alloc_Qty = 0,
                                Total_Qty = 0,
                                Ave_Cost = 0,
                                Total_Cost = 0
                            };
                            Save(oCon, imm);
                        }           
                    }
                }
            }
        }

        public static DataTable RetreiveSOByItemNumber(SqlConnection connection, string Item_Number)
        {
            DataTable resultValue = new DataTable();

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
SELECT          dbo.a_Item_Master.Item_Number, dbo.a_SO_Header.SO_Number, dbo.a_SO_Header.Order_Date, dbo.a_SO_Header.Due_Date, dbo.a_SO_Detail.Qty, dbo.a_SO_Detail.Cost, dbo.a_SO_Detail.Amount
FROM            dbo.a_Item_Master INNER JOIN
                dbo.a_SO_Detail ON dbo.a_Item_Master.idItem = dbo.a_SO_Detail.idItem INNER JOIN
                dbo.a_SO_Header ON dbo.a_SO_Detail.idSOHeader = dbo.a_SO_Header.idSOHeader
WHERE           dbo.a_Item_Master.Item_Number LIKE '%' + @Item_Number + '%' AND dbo.a_SO_Header.SO_Status = 'Open'
                           ");

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm3 = new SqlParameter
                {
                    ParameterName = "@Item_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = Item_Number
                };
                cmd.Parameters.Add(parm3);

                resultValue.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return resultValue;
        }

        public static int RetrieveIdItem(SqlConnection oCon, string Item_Number, int site)
        {
            int resultValue = 0;

            var sQuery = new StringBuilder();

            sQuery.Append(@"
                           SELECT idItem FROM a_Item_Master WHERE Item_Number = @Item_Number AND Site = @site

                           ");
            oCon.Open();
            var oCommand = new SqlCommand(sQuery.ToString(), oCon);

            SqlParameter PidItem = new SqlParameter();
            PidItem.ParameterName = "@Item_Number";
            PidItem.SqlDbType = SqlDbType.NVarChar;
            PidItem.Value = Item_Number;
            oCommand.Parameters.Add(PidItem);

            SqlParameter PSite = new SqlParameter();
            PSite.ParameterName = "@site";
            PSite.SqlDbType = SqlDbType.Int;
            PSite.Value = site;
            oCommand.Parameters.Add(PSite);

            var oreader = oCommand.ExecuteReader();

            while (oreader.Read())
            {
                if (oreader.IsDBNull(0))
                {
                    resultValue = 0;
                }
                else
                {
                    resultValue = Convert.ToInt32(oreader[0].ToString());
                }
            }
            oreader.Close();
            oCon.Close();
            return resultValue;
        }

        public static int RetrieveSite(SqlConnection oCon, int idItem)
        {
            int resultValue = 0;

            var sQuery = new StringBuilder();

            sQuery.Append(@"
                           SELECT Site FROM a_Item_Master WHERE idItem = @idItem
                           ");
            oCon.Open();
            var oCommand = new SqlCommand(sQuery.ToString(), oCon);

            SqlParameter PidItem = new SqlParameter();
            PidItem.ParameterName = "@idItem";
            PidItem.SqlDbType = SqlDbType.Int;
            PidItem.Value = idItem;
            oCommand.Parameters.Add(PidItem);

            var oreader = oCommand.ExecuteReader();

            while (oreader.Read())
            {
                if (oreader.IsDBNull(0))
                {
                    resultValue = 0;
                }
                else
                {
                    resultValue = Convert.ToInt32(oreader[0].ToString());
                }
            }
            oreader.Close();
            oCon.Close();
            return resultValue;
        }

        public static bool CheckIfExist(SqlConnection oCon, string Item_Number, int idSite)
        {
            bool resultValue = false;

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT *
                            FROM a_Item_Master
                            WHERE Principal_SKU = (SELECT Principal_SKU FROM a_Item_Master WHERE Item_Number = @Item_Number) AND 
                            Site = @Site
                           ");

            var lmodel = new List<Item_Master_Model>();

            oCon.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = oCon;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm = new SqlParameter
                {
                    ParameterName = "@Item_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = Item_Number
                };
                cmd.Parameters.Add(parm);

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@Site",
                    SqlDbType = SqlDbType.Int,
                    Value = idSite
                };
                cmd.Parameters.Add(parm2);


                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    resultValue = true;
                }
                oreader.Close();
                cmd.Dispose();
            }

            oCon.Close();

            return resultValue;
        }
    }
}
