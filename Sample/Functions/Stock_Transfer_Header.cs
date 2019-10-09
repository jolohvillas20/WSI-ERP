using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class Stock_Transfer_Header
    {
        public static List<Stock_Transfer_Header_Model> RetrieveData(SqlConnection connection, string Doc_Number)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idSTHeader
                            ,Doc_Number
                            ,STR_Number
                            ,Site_From
                            ,Site_To
                            ,timestamp      
                            FROM a_Stock_Transfer_Header
                            WHERE idSTHeader <> 0
                            ");

            if (Doc_Number != "")
            {
                sQuery.Append(" AND Doc_Number = @Doc_Number ");
            }            

            var lmodel = new List<Stock_Transfer_Header_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (Doc_Number != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Doc_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Doc_Number.Trim()
                    };
                    cmd.Parameters.Add(parm2);
                }
                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Stock_Transfer_Header_Model oModel = new Stock_Transfer_Header_Model
                    {
                        idSTHeader = (int)oreader["idSTHeader"],
                        Doc_Number = (string)oreader["Doc_Number"],
                        STR_Number = (string)oreader["STR_Number"],
                        Site_From = (string)oreader["Site_From"],
                        Site_To = (string)oreader["Site_To"],
                        timestamp = (DateTime)oreader["timestamp"]                        
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static int Save(SqlConnection connection, Stock_Transfer_Header_Model model)
        {
            int returnValue = 9;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_Stock_Transfer_Header
                             (Doc_Number
                             ,STR_Number
                             ,Site_From
                             ,Site_To
                             ,timestamp
                             )
                             VALUES
                             (@Doc_Number
                             ,@STR_Number
                             ,@Site_From
                             ,@Site_To
                             ,@timestamp
                             )

SELECT SCOPE_IDENTITY() as 'ID'
");
            
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@Doc_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Doc_Number
                };
                cmd.Parameters.Add(parm2);

                SqlParameter parm3 = new SqlParameter
                {
                    ParameterName = "@STR_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.STR_Number
                };
                cmd.Parameters.Add(parm3);

                SqlParameter parm4 = new SqlParameter
                {
                    ParameterName = "@Site_From",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Site_From
                };
                cmd.Parameters.Add(parm4);

                SqlParameter parm6 = new SqlParameter
                {
                    ParameterName = "@Site_To",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Site_To
                };
                cmd.Parameters.Add(parm6);

                SqlParameter parm8 = new SqlParameter
                {
                    ParameterName = "@timestamp",
                    SqlDbType = SqlDbType.DateTime,
                    Value = model.timestamp
                };
                cmd.Parameters.Add(parm8);

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
                    returnValue = 0;
                    oreader.Close();
                    cmd.Dispose();
                    cmd.Parameters.Clear();
                    SQL_Transact.RollbackTransaction(connection, GUID);
                }
            }

            return returnValue;
        }

        public static bool Update(SqlConnection connection, Stock_Transfer_Header_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"UPDATE a_Stock_Transfer_Header SET
                             Doc_Number = @Doc_Number
                             ,STR_Number = @STR_Number
                             ,idCustomer = @idCustomer
                             ,Site_From = @Site_From 
                             ,Site_To = @Site_To
                             ,timestamp = @timestamp                            
                             WHERE idSTHeader = @idSTHeader
");

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm1 = new SqlParameter
                {
                    ParameterName = "@idSTHeader",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idSTHeader
                };
                cmd.Parameters.Add(parm1);

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@Doc_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Doc_Number
                };
                cmd.Parameters.Add(parm2);

                SqlParameter parm3 = new SqlParameter
                {
                    ParameterName = "@STR_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.STR_Number
                };
                cmd.Parameters.Add(parm3);              

                SqlParameter parm5 = new SqlParameter();
                parm5.ParameterName = "@Site_From";
                parm5.SqlDbType = SqlDbType.NVarChar;
                parm5.Value = model.Site_From;
                cmd.Parameters.Add(parm5);

                SqlParameter parm6 = new SqlParameter
                {
                    ParameterName = "@Site_To",
                    SqlDbType = SqlDbType.DateTime,
                    Value = model.Site_To
                };
                cmd.Parameters.Add(parm6);
                
                SqlParameter parm8 = new SqlParameter
                {
                    ParameterName = "@timestamp",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.timestamp
                };
                cmd.Parameters.Add(parm8);
                
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
                }
            }

            return returnValue;
        }
         
        public static bool Delete(SqlConnection connection, int idSTHeader)
        {
            bool boolReturnValue = false;

            var GUID = SQL_Transact.GenerateGUID();

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(" UPDATE a_Stock_Transfer_Header SET Active = 'N' , SO_Status = 'DELETED' ");
            sQuery.Append(" WHERE idSTHeader = @idSTHeader ");

            SQL_Transact.BeginTransaction(connection, GUID);

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm = new SqlParameter
                    {
                        ParameterName = "@idSTHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = idSTHeader
                    };
                    cmd.Parameters.Add(parm);

                    if (cmd.ExecuteNonQuery() >= 1)
                    {
                        boolReturnValue = true;
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

            SqlConnection con2 = new SqlConnection(connection.ConnectionString);

            sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idSODetail
                           ,idSTHeader
                           ,idItem
                           ,Cost
                           ,UM
                           ,Discount
                           ,Amount
                           FROM a_SO_Detail WHERE idSODetail <> 0 
                           ");

            if (idSTHeader != 0)
            {
                sQuery.Append(" AND idSTHeader = @idSTHeader ");
            }                     

            return boolReturnValue;
        }
        
        public static int GetLastidSTHeader(SqlConnection con)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("SELECT TOP 1 idSTHeader FROM a_Stock_Transfer_Header ");
            sQuery.Append("ORDER BY idSTHeader DESC");

            int resultValue = 0;
            con.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    if (oreader.IsDBNull(0))
                    {
                        resultValue = 0;
                    }
                    else
                    {
                        resultValue = (int)oreader[0];
                    }
                }
                oreader.Close();
                cmd.Dispose();
            }
            con.Close();
            return resultValue;
        }

        public static string GetLastDocNumber(SqlConnection con)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("SELECT TOP 1 Doc_Number FROM a_Stock_Transfer_Header ");
            sQuery.Append("ORDER BY Doc_Number DESC");

            string resultValue = null;
            con.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    if (oreader.IsDBNull(0))
                    {
                        resultValue = "";
                    }
                    else
                    {
                        resultValue = (string)oreader[0];
                    }
                }
                oreader.Close();
                cmd.Dispose();
            }
            con.Close();
            return resultValue;
        }

        public static List<STDownload_Model> GetDetailsForDownload(SqlConnection con, string Doc_Number)
        {
            StringBuilder sQuery = new StringBuilder();

            string item_number = "";

            sQuery.Append(@"
SELECT        dbo.a_Item_Master.Item_Number, dbo.a_Item_Master.Description, dbo.a_Item_Master.UM, dbo.a_Stock_Transfer_Detail.Qty, dbo.a_Item_Serial.Serial_No
FROM            dbo.a_Stock_Transfer_Detail INNER JOIN
                         dbo.a_Stock_Transfer_Header ON dbo.a_Stock_Transfer_Detail.idSTHeader = dbo.a_Stock_Transfer_Header.idSTHeader INNER JOIN
                         dbo.a_Stock_Transfer_Serial ON dbo.a_Stock_Transfer_Detail.idSTDetail = dbo.a_Stock_Transfer_Serial.idSTDetail INNER JOIN
                         dbo.a_Item_Master ON dbo.a_Stock_Transfer_Detail.idItem = dbo.a_Item_Master.idItem INNER JOIN
                         dbo.a_Item_Serial ON dbo.a_Stock_Transfer_Serial.idSerial = dbo.a_Item_Serial.idSerial AND dbo.a_Item_Master.idItem = dbo.a_Item_Serial.idItem
WHERE       dbo.a_Stock_Transfer_Header.Doc_Number = @Doc_Number ");

            List<STDownload_Model> resultValue = new List<STDownload_Model>();
            con.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;
                
                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@Doc_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = Doc_Number
                };
                cmd.Parameters.Add(parm2);

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    STDownload_Model obj = new STDownload_Model();

                    if (oreader["Item_Number"].ToString() != item_number)
                    {
                        item_number = oreader["Item_Number"].ToString();
                        obj.Item_Number = item_number;
                        obj.Qty = oreader["Qty"].ToString();
                        obj.UM = oreader["UM"].ToString();
                        obj.Description = oreader["Description"].ToString();
                        obj.Serial_No = oreader["Serial_No"].ToString();
                    }
                    else
                    {
                        obj.Serial_No = oreader["Serial_No"].ToString();
                    }


                    resultValue.Add(obj);
                }
                oreader.Close();
                cmd.Dispose();
            }
            con.Close();
            return resultValue;
        }
    }
}
