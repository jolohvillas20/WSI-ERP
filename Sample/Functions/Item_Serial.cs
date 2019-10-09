using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class Item_Serial
    {
        public static DataTable RetrieveData(SqlConnection connection, string idItem, string searchPO_Number, string Serial_No)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idSerial
                         ,Serial_No
                         ,PO_Number
                         ,timestamp
                         ,Unit_Cost
                         ,Unit_Comp
                         ,user_change
                         FROM a_Item_Serial
                         WHERE idSerial <> 0
                         AND Pick_Status = 'N'
AND InStock = 'Y'
                         ");

            if (idItem != "")
            {
                sQuery.Append(" AND idItem = @idItem ");
            }

            if (searchPO_Number != "")
            {
                sQuery.Append(" AND PO_Number LIKE '%' + @searchPO_Number + '%' ");
            }

            if (Serial_No != "")
            {
                sQuery.Append(" AND Serial_No LIKE '%' + @Serial_No + '%' ");
            }

            sQuery.Append(" ORDER BY timestamp ASC ");

            var lmodel = new List<Item_Serial_Model>();

            connection.Open();

            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (idItem != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = idItem
                    };
                    cmd.Parameters.Add(parm1);
                }

                if (searchPO_Number != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@searchPO_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = searchPO_Number
                    };
                    cmd.Parameters.Add(parm1);
                }

                if (Serial_No != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@Serial_No",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Serial_No
                    };
                    cmd.Parameters.Add(parm1);
                }

                dt.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return dt;
        }

        public static List<Item_Serial_Model> RetrieveData(SqlConnection connection, int idSerial)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idSerial
      ,idItem
      ,Serial_No
      ,PO_Number
      ,timestamp
      ,Pick_Status
      ,Unit_Cost
      ,Unit_Comp
      ,user_change
      ,InStock
                         FROM a_Item_Serial
                         WHERE idSerial <> 0
                         ");                      

            if (idSerial != 0)
            {
                sQuery.Append(" AND idSerial = @idSerial ");
            }

            sQuery.Append(" ORDER BY timestamp ASC ");

            var lmodel = new List<Item_Serial_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (idSerial != 0)
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idSerial",
                        SqlDbType = SqlDbType.Int,
                        Value = idSerial
                    };
                    cmd.Parameters.Add(parm1);
                }                

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Item_Serial_Model oModel = new Item_Serial_Model
                    {
                        idSerial = (int)oreader["idSerial"],
                        Serial_No = (string)oreader["Serial_No"],
                        PO_Number = (string)oreader["PO_Number"],
                        timestamp = (DateTime)oreader["timestamp"],
                        Unit_Cost = (decimal)oreader["Unit_Cost"],
                        Unit_Comp = (string)oreader["Unit_Comp"],
                        idItem = (int)oreader["idItem"],
                        InStock = (string)oreader["InStock"],
                        user_change = (string)oreader["user_change"]
                    };
                    lmodel.Add(oModel);
                }

                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static bool Save(SqlConnection connection, Item_Serial_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_Item_Serial
                             (idItem
                             ,Serial_No
                             ,PO_Number
                             ,timestamp
                             ,Pick_Status
                             ,Unit_Cost
                             ,Unit_Comp
,user_change
,InStock
                             )
                             VALUES
                             (@idItem
                             ,@Serial_No
                             ,@PO_Number
                             ,@timestamp
                             ,'N'        
                             ,@Unit_Cost
                             ,@Unit_Comp
,@user_change
,@InStock
                             )

                             DECLARE @OnHandQty int = 0
                             DECLARE @AllocQty int = 0
                             DECLARE @TotalQty int = 0
                             DECLARE @NewOnHandQty int = 0
                             DECLARE @NewTotalQty int = 0
DECLARE @NewAverageCost decimal = 0.0000
DECLARE @NewTotalCost decimal = 0.0000


                             SELECT @OnHandQty = OnHand_Qty,
                             @AllocQty = Alloc_Qty
                             FROM a_Item_Master
                             WHERE idItem = @idItem
        
                             SET @NewOnHandQty = @OnHandQty + 1
                             SET @NewTotalQty = @NewOnHandQty - @AllocQty
SET @NewAverageCost = (SELECT AVG(Unit_Cost) FROM a_Item_Serial WHERE idItem = @idItem AND Pick_Status = 'N' AND InStock = 'Y')
SET @NewTotalCost = (SELECT SUM(Unit_Cost) FROM a_Item_Serial WHERE idItem = @idItem AND Pick_Status = 'N' AND InStock = 'Y')

                             UPDATE a_Item_Master
                             SET OnHand_Qty = @NewOnHandQty
                             ,Total_Qty = @NewTotalQty
                             ,Ave_Cost = @NewAverageCost
                             ,Total_Cost = @NewTotalCost
                             WHERE idItem = @idItem
                            ");


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
                        Value = model.idItem
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Serial_No",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Serial_No
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@PO_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.PO_Number
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@timestamp",
                        SqlDbType = SqlDbType.DateTime,
                        Value = model.timestamp
                    };
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@Unit_Cost",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Unit_Cost
                    };
                    cmd.Parameters.Add(parm6);

                    SqlParameter parm7 = new SqlParameter
                    {
                        ParameterName = "@Unit_Comp",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Unit_Comp
                    };
                    cmd.Parameters.Add(parm7);

                    SqlParameter parm8 = new SqlParameter
                    {
                        ParameterName = "@user_change",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.user_change
                    };
                    cmd.Parameters.Add(parm8);

                    SqlParameter parm9 = new SqlParameter
                    {
                        ParameterName = "@InStock",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.InStock
                    };
                    cmd.Parameters.Add(parm9);

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

        public static bool Update(SqlConnection connection, Item_Serial_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_Item_Serial SET                             
                             Serial_No = @Serial_No        
                             ,PO_Number = @PO_Number
                             WHERE idSerial = @idSerial
                             AND idItem = @idItem");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idSerial",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idSerial
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idItem
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Serial_No",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Serial_No
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@PO_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.PO_Number
                    };
                    cmd.Parameters.Add(parm4);

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

        public static bool Delete(SqlConnection connection, int idSerial, int idItem)
        {
            bool boolReturnValue = false;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"UPDATE a_Item_Serial 
                            SET Pick_Status = 'Y', InStock = 'N'
                            WHERE idSerial = @idSerial

                            DECLARE @OnHandQty int = 0
                            DECLARE @AllocQty int = 0
                            DECLARE @TotalQty int = 0
                            DECLARE @NewOnHandQty int = 0
                            DECLARE @NewTotalQty int = 0
                            DECLARE @NewAllocQty int = 0
DECLARE @NewAverageCost decimal = 0.0000
DECLARE @NewTotalCost decimal = 0.0000

                            SELECT @OnHandQty = OnHand_Qty,
                            @AllocQty = Alloc_Qty
                            FROM a_Item_Master
                            WHERE idItem = @idItem
        
                            SET @NewOnHandQty = @OnHandQty - 1
                            SET @NewAllocQty = @AllocQty - 1
                            SET @NewTotalQty = @NewOnHandQty - @NewAllocQty
SET @NewAverageCost = (SELECT AVG(Unit_Cost) FROM a_Item_Serial WHERE idItem = @idItem AND Pick_Status = 'N' AND InStock = 'Y')
SET @NewTotalCost = (SELECT SUM(Unit_Cost) FROM a_Item_Serial WHERE idItem = @idItem AND Pick_Status = 'N' AND InStock = 'Y')

                            UPDATE a_Item_Master
                            SET OnHand_Qty = @NewOnHandQty
                            ,Total_Qty = @NewTotalQty
                            ,Alloc_Qty = @NewAllocQty
                            ,Ave_Cost = @NewAverageCost
                            ,Total_Cost = @NewTotalCost
                            WHERE idItem = @idItem
                            ");

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm = new SqlParameter
                    {
                        ParameterName = "@idSerial",
                        SqlDbType = SqlDbType.Int,
                        Value = idSerial
                    };
                    cmd.Parameters.Add(parm);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = idItem
                    };
                    cmd.Parameters.Add(parm2);

                    if (cmd.ExecuteNonQuery() >= 1)
                    {
                        boolReturnValue = true;
                        cmd.Dispose();
                        cmd.Parameters.Clear();
                        SQL_Transact.CommitTransaction(connection, GUID);
                        SqlConnection newcon = new SqlConnection(connection.ConnectionString);
                        AverageCosting(newcon, idItem.ToString());
                    }
                }
                catch
                {
                    cmd.Dispose();
                    cmd.Parameters.Clear();
                    SQL_Transact.RollbackTransaction(connection, GUID);
                }
            }

            return boolReturnValue;
        }

        public static bool UpdateAllocQuantity(SqlConnection connection, int idItem, int ItemQuantity)
        {
            bool returnValue = false;

            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"
                                BEGIN TRAN
                                DECLARE @OnHandQty int = 0
                                DECLARE @AllocQty int = 0
                                DECLARE @TotalQty int = 0
                                DECLARE @NewTotalQty int = 0
                                DECLARE @NewAllocQty int = 0

                                SELECT @OnHandQty = OnHand_Qty
                                ,@AllocQty = Alloc_Qty
                                ,@TotalQty = Total_Qty
                                FROM a_Item_Master
                                WHERE idItem = @idItem

                                @NewAllocQty = @AllocQty + @ItemQuantity
                                @NewTotalQty = @OnHandQty - @NewAllocQty

                                UPDATE a_Item_Master SET
                                Alloc_Qty = @NewAllocQty
                                ,Total_Qty = @NewTotalQty
                                WHERE idItem = @idItem

                                COMMIT TRAN
                                ");
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
                        Value = idItem
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@ItemQuantity",
                        SqlDbType = SqlDbType.Int,
                        Value = ItemQuantity
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

        public static DataTable AverageCosting(SqlConnection connection, string idItem)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT DISTINCT(PO_Number)
                           FROM a_Item_Serial
                           WHERE idItem = @idItem
                           ");

            DataTable dtPONumber = new DataTable();
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("PO_Number");
            dtResult.Columns.Add("Stock");
            dtResult.Columns.Add("Average_Cost");
            dtResult.Columns.Add("Total_Cost");

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@idItem",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = idItem.Trim()
                };
                cmd.Parameters.Add(parm2);


                dtPONumber.Load(cmd.ExecuteReader());

                cmd.Dispose();

                sQuery.Clear();
            }

            connection.Close();

            for (int x = 0; x < dtPONumber.Rows.Count - 1; x++)
            {
                string PONumber = dtPONumber.Rows[x][0].ToString();

                sQuery.Append(@"SELECT
                                PO_Number,
                                COUNT(Serial_No) as Stock,
                                AVG(Unit_Cost) as 'Average_Cost',
                                SUM(Unit_Cost) as 'Total_Cost'
                                FROM a_Item_Serial
                                WHERE PO_Number = @PONumber
                                AND Pick_Status = 'N'
                                AND idItem = @idItem
AND InStock = 'Y'
                                GROUP BY PO_Number
                                ");

                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = idItem.Trim()
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@PONumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = PONumber.Trim()
                    };
                    cmd.Parameters.Add(parm2);

                    var oreader = cmd.ExecuteReader();

                    while (oreader.Read())
                    {
                        DataRow dr;
                        dr = dtResult.NewRow();
                        dr["PO_Number"] = (string)oreader["PO_Number"];
                        dr["Stock"] = (int)oreader["Stock"];
                        dr["Average_Cost"] = (decimal)oreader["Average_Cost"];
                        dr["Total_Cost"] = (decimal)oreader["Total_Cost"];
                        dtResult.Rows.Add(dr);
                    }

                    //dtLoop.Load(cmd.ExecuteReader());
                    //dtResult.Merge(dtLoop);
                    oreader.Close();
                    cmd.Dispose();

                    sQuery.Clear();
                }

                connection.Close();
            }

            return dtResult;
        }

        public static int CheckInStock(SqlConnection connection, string SerialNumber, int idItem)
        {
            int returnValue = 0;

            connection.Open();
            try
            {
                StringBuilder sQuery = new StringBuilder();

                sQuery.Append(@"SELECT *
                             FROM a_Item_Serial
                             WHERE InStock = 'Y'
                             AND idItem = @idItem
                             AND Serial_No = @SerialNumber
                             ");

                var lmodel = new List<Item_Serial_Model>();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@SerialNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = SerialNumber
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = idItem
                    };
                    cmd.Parameters.Add(parm);

                    var oreader = cmd.ExecuteReader();

                    while (oreader.Read())
                    {
                        returnValue = returnValue + 1;
                    }
                    oreader.Close();
                    cmd.Dispose();
                }
                connection.Close();
            }
            catch
            {
                connection.Close();
            }

            return returnValue;
        }

        public static int CheckDuplicate(SqlConnection connection, string SerialNumber, int idItem, string itemNumber, int destSite)
        {
            int returnValue = 0;
                     
            connection.Open();
            try
            {
                StringBuilder sQuery = new StringBuilder();

                sQuery.Append(@"SELECT *
                             FROM a_Item_Serial
                             WHERE idSerial <> 0                            
                             AND Serial_No = @SerialNumber
                             ");
                if (idItem != 0)
                    sQuery.Append(@" AND idItem = @idItem ");

                if (itemNumber != "" && destSite != 0)
                    sQuery.Append(@" AND idItem = (SELECT idItem FROM a_Item_Master WHERE Item_Number = @itemNumber AND Site = @destSite");

                sQuery.Append(")");

                var lmodel = new List<Item_Serial_Model>();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@SerialNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = SerialNumber
                    };
                    cmd.Parameters.Add(parm1);

                    if (idItem != 0)
                    {
                        SqlParameter parm = new SqlParameter
                        {
                            ParameterName = "@idItem",
                            SqlDbType = SqlDbType.Int,
                            Value = idItem
                        };
                        cmd.Parameters.Add(parm);
                    }

                    if (itemNumber != "")
                    {
                        SqlParameter parm = new SqlParameter
                        {
                            ParameterName = "@itemNumber",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = itemNumber
                        };
                        cmd.Parameters.Add(parm);
                    }

                    if (destSite != 0)
                    {
                        SqlParameter parm = new SqlParameter
                        {
                            ParameterName = "@destSite",
                            SqlDbType = SqlDbType.Int,
                            Value = destSite
                        };
                        cmd.Parameters.Add(parm);
                    }

                    var oreader = cmd.ExecuteReader();

                    while (oreader.Read())
                    {
                        returnValue = returnValue + 1;
                    }
                    oreader.Close();
                    cmd.Dispose();
                }
                //SQL_Transact.CommitTransaction(connection, GUID);
                connection.Close();
            }
            catch
            {
                connection.Close();
                //SQL_Transact.RollbackTransaction(connection, GUID);
            }

            return returnValue;
        }

        public static int CheckDuplicate(SqlConnection connection, string SerialNumber, int idItem)
        {
            int returnValue = 0;

            connection.Open();
            try
            {
                StringBuilder sQuery = new StringBuilder();

                sQuery.Append(@"SELECT *
                             FROM a_Item_Serial
                             WHERE idSerial <> 0                            
                             AND Serial_No = @SerialNumber
                             ");
                if (idItem != 0)
                    sQuery.Append(@" AND idItem = @idItem ");

                //if (itemNumber != "" && destSite != 0)
                //    sQuery.Append(@" AND idItem = (SELECT idItem FROM a_Item_Master WHERE Item_Number = @itemNumber AND Site = @destSite)");

                var lmodel = new List<Item_Serial_Model>();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@SerialNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = SerialNumber
                    };
                    cmd.Parameters.Add(parm1);

                    if (idItem != 0)
                    {
                        SqlParameter parm = new SqlParameter
                        {
                            ParameterName = "@idItem",
                            SqlDbType = SqlDbType.Int,
                            Value = idItem
                        };
                        cmd.Parameters.Add(parm);
                    }

                    //if (itemNumber != "")
                    //{
                    //    SqlParameter parm = new SqlParameter
                    //    {
                    //        ParameterName = "@itemNumber",
                    //        SqlDbType = SqlDbType.NVarChar,
                    //        Value = itemNumber
                    //    };
                    //    cmd.Parameters.Add(parm);
                    //}

                    //if (destSite != 0)
                    //{
                    //    SqlParameter parm = new SqlParameter
                    //    {
                    //        ParameterName = "@destSite",
                    //        SqlDbType = SqlDbType.Int,
                    //        Value = destSite
                    //    };
                    //    cmd.Parameters.Add(parm);
                    //}

                    var oreader = cmd.ExecuteReader();

                    while (oreader.Read())
                    {
                        returnValue = returnValue + 1;
                    }
                    oreader.Close();
                    cmd.Dispose();
                }
                //SQL_Transact.CommitTransaction(connection, GUID);
                connection.Close();
            }
            catch
            {
                connection.Close();
                //SQL_Transact.RollbackTransaction(connection, GUID);
            }

            return returnValue;
        }

        public static int CheckIfInStock(SqlConnection connection, string SerialNumber)
        {
            int returnValue = 0;

            connection.Open();
            try
            {
                StringBuilder sQuery = new StringBuilder();

                sQuery.Append(@"SELECT *
                             FROM a_Item_Serial
                             WHERE idSerial <> 0                            
                             AND Serial_No = @SerialNumber AND InStock = 'Y'
                             ");


                var lmodel = new List<Item_Serial_Model>();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@SerialNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = SerialNumber
                    };
                    cmd.Parameters.Add(parm1);

                    var oreader = cmd.ExecuteReader();

                    while (oreader.Read())
                    {
                        returnValue = returnValue + 1;
                    }
                    oreader.Close();
                    cmd.Dispose();
                }
                connection.Close();
            }
            catch
            {
                connection.Close();
            }

            return returnValue;
        }

        public static DataTable RetrieveLotSerial(SqlConnection connection, string idItem, string quantity, string Serial_No)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT TOP(" + quantity + @") idSerial
                         ,Serial_No
                         ,PO_Number
                         FROM a_Item_Serial
                         WHERE idSerial <> 0
                         AND Pick_Status = 'N'
AND InStock = 'Y'
                         ");

            if (idItem != "")
            {
                sQuery.Append(" AND idItem = @idItem ");
            }

            if (Serial_No != "")
            {
                sQuery.Append(" AND Serial_No LIKE '%' + @Serial_No + '%' ");
            }

            sQuery.Append(" ORDER BY idSerial ASC ");

            var lmodel = new List<Item_Serial_Model>();

            connection.Open();

            DataTable dt = new DataTable();


            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (idItem != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = idItem
                    };
                    cmd.Parameters.Add(parm1);
                }

                if (Serial_No != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@Serial_No",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Serial_No
                    };
                    cmd.Parameters.Add(parm1);
                }

                dt.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return dt;
        }

        public static int LotSerialCount(SqlConnection oConnection, string Lot_Serial)
        {
            int resultValue = 0;

            var sQuery = new StringBuilder();

            sQuery.Append(@"
                           SELECT COUNT(idSerial) as On_hand_qty 
                           FROM a_Item_Serial 
                           WHERE Serial_No LIKE '%' + @Lot_Serial + '%'
                           ");

            try
            {
                oConnection.Open();

                var oCommand = new SqlCommand(sQuery.ToString(), oConnection);

                SqlParameter PidItem = new SqlParameter();
                PidItem.ParameterName = "@Lot_Serial";
                PidItem.SqlDbType = SqlDbType.NVarChar;
                PidItem.Value = Lot_Serial;
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
            }
            catch 
            {

            }
            finally
            {
                oConnection.Close();
            }

            return resultValue;
        }

        public static DataTable GetDetailsForCostAdjustment(SqlConnection connection, string itemNumber)
        {
            StringBuilder sQuery = new StringBuilder();
            DataTable dt = new DataTable();

            sQuery.Append(@"SELECT COUNT(idSerial) as Qty, AVG(Unit_Cost) as Cost 
                            FROM a_Item_Serial 
                            WHERE Pick_Status = 'N' AND InStock = 'Y'
                           ");

            if (itemNumber != "")
            {
                sQuery.Append(" AND idItem = (SELECT idItem FROM a_Item_Master WHERE Item_Number = @itemNumber) ");
            }

            var lmodel = new List<Item_Serial_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (itemNumber != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@itemNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = itemNumber
                    };
                    cmd.Parameters.Add(parm1);
                }

                dt.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return dt;
        }

        public static bool UpdateCost(SqlConnection connection, int idItem, decimal Unit_Cost)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_Item_Serial SET                             
                             Unit_Cost = @Unit_Cost
                             WHERE idItem = @idItem");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter sqUnit_Cost = new SqlParameter
                    {
                        ParameterName = "@Unit_Cost",
                        SqlDbType = SqlDbType.Decimal,
                        Value = Unit_Cost
                    };
                    cmd.Parameters.Add(sqUnit_Cost);

                    SqlParameter sqidItem = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = idItem
                    };
                    cmd.Parameters.Add(sqidItem);

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

        public static bool ReturnStock(SqlConnection connection, int idItem, string Serial_No)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"UPDATE a_Item_Serial SET     
                             Pick_Status = 'N', InStock = 'Y'
                             WHERE Serial_No = @Serial_No
                             AND idItem = @idItem");

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm1 = new SqlParameter
                {
                    ParameterName = "@Serial_No",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = Serial_No
                };
                cmd.Parameters.Add(parm1);

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@idItem",
                    SqlDbType = SqlDbType.Int,
                    Value = idItem
                };
                cmd.Parameters.Add(parm2);
                try
                {
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
                    returnValue = false;
                }
            }

            return returnValue;
        }

        public static bool ReturnStock(SqlConnection connection, string Serial_No, string Item_Number)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"DECLARE @idItem INT = 0

                            SELECT @idItem = idItem FROM a_Item_Master 
                            WHERE Principal_SKU = (SELECT Principal_SKU FROM a_Item_Master WHERE Item_Number = @Item_Number) 
                            AND Site = (SELECT idSite FROM a_Site WHERE Site_Name = 'DOA')

                            IF @idItem = 0
                            BEGIN
                            	DECLARE @idClass int
	                            DECLARE @idSubclass int
	                            DECLARE @New_Item_Number nvarchar(max)
	                            DECLARE @Principal_SKU nvarchar(max)
	                            DECLARE @Description nvarchar(max)
	                            DECLARE @Site nvarchar(50) = (SELECT idSite FROM a_Site WHERE Site_Name = 'DOA')
	                            DECLARE @Unit_Weight decimal(18,2) = 0
	                            DECLARE @Weight_UM nvarchar(max) = ''
	                            DECLARE @UM nchar(10) = 'UT'
	                            DECLARE @OnHand_Qty int = 0
	                            DECLARE @Alloc_Qty int = 0
	                            DECLARE @Total_Qty int = 0
	                            DECLARE @Ave_Cost decimal(18,4) = 0
	                            DECLARE @Total_Cost decimal(18,4) = 0
	                            DECLARE @Subclass_Name NVARCHAR(MAX)

	                            SELECT
	                            @idClass = idClass
	                            ,@idSubclass = idSubclass
	                            ,@Principal_SKU = Principal_SKU
	                            ,@Description = Description
	                            FROM a_Item_Master
	                            WHERE Item_Number = @Item_Number
	                            AND Site = (SELECT idSite FROM a_Site WHERE Site_Name = 'WH-JMS')

	                            SELECT @Subclass_Name = Subclass_Name FROM a_Item_Subclass WHERE idClass = @idClass AND idSubclass = @idSubclass
	
	                            SET @Subclass_Name = SUBSTRING(@Subclass_Name,1,4)

	                            SET @New_Item_Number = @Subclass_Name + '-' + @Principal_SKU
	
	                            INSERT INTO [dbo].[a_Item_Master]
	                            ([idClass]
	                            ,[idSubclass]
	                            ,[Item_Number]
	                            ,[Principal_SKU]
	                            ,[Description]
	                            ,[Site]
                            	,[Unit_Weight]
                            	,[Weight_UM]
	                            ,[UM]
	                            ,[OnHand_Qty]
	                            ,[Alloc_Qty]
                            	,[Total_Qty]
                            	,[Ave_Cost]
                            	,[Total_Cost])
	                            VALUES
	                            (@idClass
                            	,@idSubclass
                            	,@New_Item_Number
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

                            	SELECT @idItem = idItem FROM a_Item_Master 
                                WHERE Principal_SKU = (SELECT Principal_SKU FROM a_Item_Master WHERE Item_Number = @Item_Number) 
                                AND Site = (SELECT idSite FROM a_Site WHERE Site_Name = 'DOA')
                            END

                            UPDATE a_Item_Serial SET     
                            idItem = @idItem, Pick_Status = 'N', InStock = 'Y'
                            WHERE Serial_No = @Serial_No
                           ");

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm1 = new SqlParameter
                {
                    ParameterName = "@Serial_No",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = Serial_No
                };
                cmd.Parameters.Add(parm1);

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@Item_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = Item_Number
                };
                cmd.Parameters.Add(parm2);

                try
                {
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
                    returnValue = false;
                }
            }

            return returnValue;
        }

        public static bool ReturnStock(SqlConnection connection, int idItem, string Serial_No, string replacement_Serial)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"UPDATE a_Item_Serial SET     
                            Pick_Status = 'N', InStock = 'Y'                            
                            WHERE Serial_No = @Serial_No
                            AND idItem = @idItem                                                       

                            UPDATE a_Pick_Serial SET
                            idSerial = (SELECT idSerial FROM a_Item_Serial WHERE Serial_No = @replacement_serial),
                            Serial_No = @replacement_serial
                            WHERE Serial_No = @Serial_No

                            DECLARE @Trans_Code nvarchar(50) 
                            DECLARE @Item_Number nvarchar(50) 
                            DECLARE @Site nvarchar(50) 
                            DECLARE @um nvarchar(50) 
                            DECLARE @Doc_No nvarchar(50) 
                            DECLARE @Reason_Code nvarchar(50) 
                            DECLARE @Order_No nvarchar(50) 
                            DECLARE @Invoice_No nvarchar(50) 
                            DECLARE @Reference_No nvarchar(50) 
	                        DECLARE @Trans_Qty int 
                            DECLARE @Trans_Amt decimal(18, 4) 
                            DECLARE @Remarks nvarchar(max) 
	                        DECLARE @user_domain nvarchar(50)

                            SELECT
                            @Trans_Code = Trans_Code,
                            @Item_Number = Item_Number,
                            @Site = Site,
                            @um = um,
                            @Doc_No = Doc_No,
                            @Reason_Code = Reason_Code,
                            @Order_No = Order_No,
                            @Invoice_No = Invoice_No,
                            @Reference_No = Reference_No,
	                        @Trans_Qty = Trans_Qty,
                            @Trans_Amt = Trans_Amt,
                            @Remarks  = Remarks,
	                        @user_domain = user_domain
                            FROM a_Trans_History WHERE Serial_No = @Serial_No AND Trans_Code = 'SLE'


                            INSERT INTO [dbo].[a_Trans_History]
                            ([Trans_Code]
                            ,[Item_Number]
                            ,[Site]
                            ,[um]
                            ,[Doc_No]
                            ,[Serial_No]
                            ,[Reason_Code]
                            ,[Trans_Date]
                            ,[Order_No]
                            ,[Invoice_No]
                            ,[Reference_No]
                            ,[Trans_Qty]
                            ,[Trans_Amt]
                            ,[Remarks]
                            ,[user_domain])
                            VALUES
                            (@Trans_Code
                            ,@Item_Number
                            ,@Site
                            ,@um
                            ,@Doc_No
                            ,@replacement_serial
                            ,@Reason_Code
                            ,(SELECT GETDATE())
                            ,@Order_No
                            ,@Invoice_No
                            ,@Reference_No
                            ,@Trans_Qty
                            ,@Trans_Amt
                            ,@Remarks
                            ,@user_domain)

");

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm1 = new SqlParameter
                {
                    ParameterName = "@Serial_No",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = Serial_No
                };
                cmd.Parameters.Add(parm1);

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@idItem",
                    SqlDbType = SqlDbType.Int,
                    Value = idItem
                };
                cmd.Parameters.Add(parm2);

                SqlParameter parm3 = new SqlParameter
                {
                    ParameterName = "@replacement_Serial",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = replacement_Serial
                };
                cmd.Parameters.Add(parm3);
                try
                {
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
                    returnValue = false;
                }
            }

            return returnValue;
        }

        public static DataTable InventorySerialReport(SqlConnection connection)
        {
            StringBuilder sQuery = new StringBuilder();
            DataTable dt = new DataTable();

            sQuery.Append(@"
SELECT a.PO_Number, b.Item_Number, a.Serial_no, a.timestamp, a.Unit_Cost
FROM a_Item_Serial as a 
INNER JOIN a_Item_Master as b 
ON a.idItem = b.idItem 
WHERE a.Pick_Status = 'N'
AND a.InStock = 'Y'
ORDER BY Item_Number
                           ");

            var lmodel = new List<Item_Serial_Model>();

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

        public static DataTable InventoryAgingReport(SqlConnection connection)
        {
            StringBuilder sQuery = new StringBuilder();
            DataTable dt = new DataTable();

            sQuery.Append(@"
SELECT DISTINCT 
b.Item_Number, 
b.Principal_SKU, 
b.Description, 
COUNT(idSerial) as OnHandQty,
AVG(Unit_Cost) as AveCost,
Age = ( SELECT DATEDIFF(DAY, (SELECT TOP 1 timestamp FROM a_Item_Serial as c WHERE a.PO_Number = c.PO_Number), GETDATE()))
FROM a_Item_Serial as a 
INNER JOIN a_Item_Master as b 
ON  a.idItem = b.idItem 
WHERE a.Pick_Status = 'N'
AND a.InStock = 'Y'
GROUP BY PO_Number,  b.Item_Number, b.Principal_SKU, b.Description
                           ");

            var lmodel = new List<Item_Serial_Model>();

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

        public static bool UpdateStockStatus(SqlConnection connection, string Serial_No, string Stock_Status)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_Item_Serial SET                             
                             InStock = @Stock_Status
                             WHERE Serial_No = @Serial_No");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter sqUnit_Cost = new SqlParameter
                    {
                        ParameterName = "@Stock_Status",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Stock_Status
                    };
                    cmd.Parameters.Add(sqUnit_Cost);

                    SqlParameter sqidItem = new SqlParameter
                    {
                        ParameterName = "@Serial_No",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Serial_No
                    };
                    cmd.Parameters.Add(sqidItem);

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

        public static int RetrieveIdSerial(SqlConnection oCon, string Serial_No)
        {
            int resultValue = 0;

            var sQuery = new StringBuilder();

            sQuery.Append(@"
                           SELECT idSerial FROM a_Item_Serial WHERE Serial_No = @Serial_No
                           ");
            oCon.Open();
            var oCommand = new SqlCommand(sQuery.ToString(), oCon);

            SqlParameter PidItem = new SqlParameter();
            PidItem.ParameterName = "@Serial_No";
            PidItem.SqlDbType = SqlDbType.NVarChar;
            PidItem.Value = Serial_No;
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

        public static bool UpdateIdItem(SqlConnection connection, string Serial_No, int idItem)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_Item_Serial SET                             
                             idItem = @idItem        
                             WHERE Serial_No = @Serial_No");
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
                        Value = idItem
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Serial_No",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Serial_No
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
    }
}
