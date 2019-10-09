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
    public class Trans_History
    {
        public static List<Trans_History_Model> RetrieveData(SqlConnection connection, string Trans_Code)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idTransHist
                         ,Trans_Code
                         ,Item_Number
                         ,Site
                         ,UM
                         ,Doc_No
                         ,Serial_No
                         ,Reason_Code
                         ,Trans_Date
                         ,Order_No
                         ,Invoice_No
                         ,Reference_No
                         ,Trans_Qty
                         ,Trans_Amt
                         ,Remarks
                         ,user_domain
                         FROM a_Trans_History
                         WHERE idTransHist <> 0
                         ");

            if (Trans_Code != "")
            {
                sQuery.Append(" AND Trans_Code = @Trans_Code");
            }

            //if (Item_Number != "")
            //{
            //    sQuery.Append(" AND Item_Number LIKE '%' + @Item_Number + '%' ");
            //}

            sQuery.Append(" ORDER BY Trans_Date ");

            var lmodel = new List<Trans_History_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (Trans_Code != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@Trans_Code",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Trans_Code
                    };
                    cmd.Parameters.Add(parm1);
                }

                //if (Item_Number != "")
                //{
                //    SqlParameter parm1 = new SqlParameter
                //    {
                //        ParameterName = "@Item_Number",
                //        SqlDbType = SqlDbType.NVarChar,
                //        Value = Item_Number
                //    };
                //    cmd.Parameters.Add(parm1);
                //}

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Trans_History_Model oModel = new Trans_History_Model
                    {
                        idTransHist = (int)oreader["idTransHist"],
                        Trans_Code = (string)oreader["Trans_Code"],
                        Doc_No = (string)oreader["Doc_No"],
                        Item_Number = (string)oreader["Item_Number"],
                        Serial_No = (string)oreader["Serial_No"],
                        Reason_Code = (string)oreader["Reason_Code"],
                        Site = (string)oreader["Site"],
                        Trans_Date = (DateTime)oreader["Trans_Date"],
                        Order_No = (string)oreader["Order_No"],
                        UM = (string)oreader["UM"],
                        Invoice_No = (string)oreader["Invoice_No"],
                        Reference_No = (string)oreader["Reference_No"],
                        Trans_Qty = (int)oreader["Trans_Qty"],
                        Trans_Amt = (decimal)oreader["Trans_Amt"],
                        Remarks = (string)oreader["Remarks"],
                        user_domain = (string)oreader["user_domain"]
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static DataTable RetrieveDataInquiry(SqlConnection connection, string Serial_No)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT a.idTransHist
                         ,a.Trans_Code as 'Trans Code'
                         ,a.Item_Number as 'Item Number'
                         ,a.Site
                         ,a.UM
                         ,a.Doc_No as 'Doc No'
                         ,a.Serial_No as 'Serial No'
                         ,a.Trans_Date as 'Trans Date'
                         ,a.Order_No as 'Order No'
                         ,a.Trans_Qty as 'Trans Qty'
                         ,a.Trans_Amt as 'Trans Amt'
                         FROM a_Trans_History as a
                         WHERE idTransHist <> 0
                         ");

            if (Serial_No != "")
            {
                sQuery.Append(" AND Serial_No LIKE '%' + @Serial_No + '%' ");
            }

            var lmodel = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

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

                lmodel.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static bool Save(SqlConnection connection, Trans_History_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_Trans_History
                             (Trans_Code
                             ,Doc_No
                             ,Item_Number
                             ,Serial_No
                             ,Reason_Code
                             ,Site
                             ,Trans_Date
                             ,Order_No
                             ,UM
                             ,Invoice_No
                             ,Reference_No
                             ,Trans_Qty
                             ,Trans_Amt
                             ,Remarks
                         ,user_domain)
                             VALUES
                             (@Trans_Code
                             ,@Doc_No
                             ,@ItemNumber
                             ,@Serial_No
                             ,@Reason_Code
                             ,@Site
                             ,@Trans_Date
                             ,@Order_No
                             ,@UM
                             ,@Invoice_No
                             ,@Reference_No
                             ,@Trans_Qty
                             ,@Trans_Amt
                             ,@Remarks
                         ,@user_domain)
");

            using (SqlCommand cmd = new SqlCommand())
            {

                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@Doc_No",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Doc_No
                };
                cmd.Parameters.Add(parm2);

                SqlParameter parm3 = new SqlParameter
                {
                    ParameterName = "@ItemNumber",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Item_Number
                };
                cmd.Parameters.Add(parm3);

                SqlParameter parm4 = new SqlParameter
                {
                    ParameterName = "@Trans_Code",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Trans_Code
                };
                cmd.Parameters.Add(parm4);

                SqlParameter parm5 = new SqlParameter
                {
                    ParameterName = "@Reason_Code",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Reason_Code
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
                    ParameterName = "@Serial_No",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Serial_No
                };
                cmd.Parameters.Add(parm7);

                SqlParameter parm8 = new SqlParameter
                {
                    ParameterName = "@Trans_Date",
                    SqlDbType = SqlDbType.DateTime,
                    Value = model.Trans_Date
                };
                cmd.Parameters.Add(parm8);

                SqlParameter parm9 = new SqlParameter
                {
                    ParameterName = "@Order_No",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Order_No
                };
                cmd.Parameters.Add(parm9);

                SqlParameter parm10 = new SqlParameter
                {
                    ParameterName = "@UM",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.UM
                };
                cmd.Parameters.Add(parm10);

                SqlParameter parm11 = new SqlParameter
                {
                    ParameterName = "@Invoice_No",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Invoice_No
                };
                cmd.Parameters.Add(parm11);

                SqlParameter parm12 = new SqlParameter
                {
                    ParameterName = "@Reference_No",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Reference_No
                };
                cmd.Parameters.Add(parm12);

                SqlParameter parm13 = new SqlParameter
                {
                    ParameterName = "@Trans_Qty",
                    SqlDbType = SqlDbType.Int,
                    Value = model.Trans_Qty
                };
                cmd.Parameters.Add(parm13);

                SqlParameter parm14 = new SqlParameter
                {
                    ParameterName = "@Trans_Amt",
                    SqlDbType = SqlDbType.Decimal,
                    Value = model.Trans_Amt
                };
                cmd.Parameters.Add(parm14);

                SqlParameter parm15 = new SqlParameter
                {
                    ParameterName = "@Remarks",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Remarks
                };
                cmd.Parameters.Add(parm15);

                SqlParameter parm16 = new SqlParameter
                {
                    ParameterName = "@user_domain",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.user_domain
                };
                cmd.Parameters.Add(parm16);

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

        public static bool Update(SqlConnection connection, Trans_History_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_Trans_History SET
                             Trans_Code = @Trans_Code
                             ,Doc_No = @Doc_No
                             ,Item_Number = @ItemNumber
                             ,Serial_No = @Serial_No
                             ,Reason_Code = @Reason_Code
                             ,Site = @Site
                             ,Trans_Date = @Trans_Date
                             ,Order_No = @Order_No
                             ,UM = @UM
                             ,Invoice_No = @Invoice_No
                             ,Reference_No = @Reference_No
                             ,Trans_Qty = @Trans_Qty
                             ,Trans_Amt = @Trans_Amt
                             ,Remarks = @Remarks
                         ,user_domain = @user_domain
                             WHERE idTransHist = @idTransHist ");

                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idTransHist",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idTransHist
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Doc_No",
                        SqlDbType = SqlDbType.Int,
                        Value = model.Doc_No
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@ItemNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Item_Number
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@Trans_Code",
                        SqlDbType = SqlDbType.Int,
                        Value = model.Trans_Code
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@Reason_Code",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Reason_Code
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
                        ParameterName = "@Serial_No",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Serial_No
                    };
                    cmd.Parameters.Add(parm7);

                    SqlParameter parm8 = new SqlParameter
                    {
                        ParameterName = "@Trans_Date",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Trans_Date
                    };
                    cmd.Parameters.Add(parm8);

                    SqlParameter parm9 = new SqlParameter
                    {
                        ParameterName = "@Order_No",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Order_No
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
                        ParameterName = "@Invoice_No",
                        SqlDbType = SqlDbType.Int,
                        Value = model.Invoice_No
                    };
                    cmd.Parameters.Add(parm11);

                    SqlParameter parm12 = new SqlParameter
                    {
                        ParameterName = "@Reference_No",
                        SqlDbType = SqlDbType.Int,
                        Value = model.Reference_No
                    };
                    cmd.Parameters.Add(parm12);

                    SqlParameter parm13 = new SqlParameter
                    {
                        ParameterName = "@Trans_Qty",
                        SqlDbType = SqlDbType.Int,
                        Value = model.Trans_Qty
                    };
                    cmd.Parameters.Add(parm13);

                    SqlParameter parm14 = new SqlParameter
                    {
                        ParameterName = "@Trans_Amt",
                        SqlDbType = SqlDbType.Int,
                        Value = model.Trans_Amt
                    };
                    cmd.Parameters.Add(parm14);

                    SqlParameter parm15 = new SqlParameter
                    {
                        ParameterName = "@Remarks",
                        SqlDbType = SqlDbType.Int,
                        Value = model.Remarks
                    };
                    cmd.Parameters.Add(parm15);

                    SqlParameter parm16 = new SqlParameter
                    {
                        ParameterName = "@user_domain",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.user_domain
                    };
                    cmd.Parameters.Add(parm16);

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

        public static bool Delete(SqlConnection connection, int idTransHist)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Trans_History ");
            sQuery.Append("WHERE idTransHist = @idTransHist");

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
                        ParameterName = "@idTransHist",
                        SqlDbType = SqlDbType.Int,
                        Value = idTransHist
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

        public static int CheckDuplicate(SqlConnection connection, string SerialNumber, int idItem, string so_number)
        {
            int returnValue = 0;

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT *
                             FROM a_Trans_History
                             WHERE  Item_Number = (SELECT Item_Number FROM a_Item_Master WHERE idItem = @idItem)
                             AND Serial_No = @SerialNumber
                             AND Trans_Code = 'SRE'
                             AND Order_No = @so_number
                             ");

            var lmodel = new List<Item_Serial_Model>();

            connection.Open();

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

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@so_number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = so_number
                };
                cmd.Parameters.Add(parm2);

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    returnValue = returnValue + 1;
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return returnValue;
        }

        public static int GetCustomerID(SqlConnection connection, string pick_number)
        {
            int returnValue = 0;

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
SELECT a.idCustomer 
FROM a_SO_Header as a 
INNER JOIN a_Pick_Header as b 
ON a.idSOHeader = b.idSOHeader 
WHERE b.Pick_Number = @pick_number
");

            var lmodel = new List<Item_Serial_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm1 = new SqlParameter
                {
                    ParameterName = "@pick_number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = pick_number
                };
                cmd.Parameters.Add(parm1);

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    returnValue = (int)oreader["idCustomer"];
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return returnValue;
        }

        public static DataTable RetrieveDataPURReport(SqlConnection connection, DateTime dateFrom, DateTime dateTo)
        {
            StringBuilder sQuery = new StringBuilder();

            DataTable dtReport = new DataTable();
            dtReport.Columns.Add("Item_Number");
            dtReport.Columns.Add("Site");
            dtReport.Columns.Add("Qty");
            //dtReport.Columns.Add("Date");
            dtReport.Columns.Add("Total Cost");
            //--
            dtReport.Columns.Add("Ave Cost");

            sQuery.Append(@"SELECT DISTINCT ITEM_NUMBER 
                            FROM a_Trans_History 
                            WHERE CONVERT(DATE,Trans_Date) BETWEEN CONVERT(DATE,@dateFrom)AND CONVERT(DATE,@dateTo) ");

            var dtItemNumber = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (dateFrom.ToShortDateString() != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@dateFrom",
                        SqlDbType = SqlDbType.DateTime,
                        Value = dateFrom
                    };
                    cmd.Parameters.Add(parm1);
                }

                if (dateTo.ToShortDateString() != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@dateTo",
                        SqlDbType = SqlDbType.DateTime,
                        Value = dateTo
                    };
                    cmd.Parameters.Add(parm1);
                }

                sQuery.Clear();
                dtItemNumber.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            for (int x = 0; x <= dtItemNumber.Rows.Count - 1; x++)
            {
                string item_number = dtItemNumber.Rows[x][0].ToString();

                sQuery.Append(@"SELECT DISTINCT SITE 
                                FROM a_Trans_History 
                                WHERE ITEM_NUMBER = @item_number 
                                AND CONVERT(DATE,Trans_Date) BETWEEN CONVERT(DATE,@dateFrom)AND CONVERT(DATE,@dateTo) ");
                var dtSite = new DataTable();

                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@item_number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = item_number
                    };
                    cmd.Parameters.Add(parm2);

                    if (dateFrom.ToShortDateString() != "")
                    {
                        SqlParameter parm1 = new SqlParameter
                        {
                            ParameterName = "@dateFrom",
                            SqlDbType = SqlDbType.DateTime,
                            Value = dateFrom
                        };
                        cmd.Parameters.Add(parm1);
                    }

                    if (dateTo.ToShortDateString() != "")
                    {
                        SqlParameter parm1 = new SqlParameter
                        {
                            ParameterName = "@dateTo",
                            SqlDbType = SqlDbType.DateTime,
                            Value = dateTo
                        };
                        cmd.Parameters.Add(parm1);
                    }

                    sQuery.Clear();
                    dtSite.Load(cmd.ExecuteReader());

                    cmd.Dispose();
                }

                connection.Close();

                dtSite.Columns.Add(new DataColumn());
                dtSite.Columns.Add(new DataColumn());

                for (int z = 0; z <= dtSite.Rows.Count - 1; z++)
                {
                    dtSite.Rows[z][1] = 0;
                    dtSite.Rows[z][2] = 0.0000;
                }

                    sQuery.Append(@"SELECT DISTINCT SERIAL_NO 
                                FROM a_Trans_History 
                                WHERE ITEM_NUMBER = @item_number 
                                AND CONVERT(DATE,Trans_Date) BETWEEN CONVERT(DATE,@dateFrom) AND CONVERT(DATE,@dateTo) ");

                var dtSerial = new DataTable();

                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@item_number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = item_number
                    };
                    cmd.Parameters.Add(parm2);

                    if (dateFrom.ToShortDateString() != "")
                    {
                        SqlParameter parm1 = new SqlParameter
                        {
                            ParameterName = "@dateFrom",
                            SqlDbType = SqlDbType.DateTime,
                            Value = dateFrom
                        };
                        cmd.Parameters.Add(parm1);
                    }

                    if (dateTo.ToShortDateString() != "")
                    {
                        SqlParameter parm1 = new SqlParameter
                        {
                            ParameterName = "@dateTo",
                            SqlDbType = SqlDbType.DateTime,
                            Value = dateTo
                        };
                        cmd.Parameters.Add(parm1);
                    }

                    sQuery.Clear();
                    dtSerial.Load(cmd.ExecuteReader());

                    cmd.Dispose();
                }

                connection.Close();

                for (int y = 0; y <= dtSerial.Rows.Count - 1; y++)
                {
                    string site = "";
                    string trans_code = "";
                    string Serial_No = dtSerial.Rows[y][0].ToString();
                    decimal trans_amt = 0;

                    sQuery.Append(@"SELECT TOP 1 Trans_Code, Site, Trans_Amt 
                                    FROM a_Trans_History 
                                    WHERE ITEM_NUMBER = @item_number 
                                    AND Serial_No = @Serial_No
                                    AND CONVERT(DATE,Trans_Date) BETWEEN CONVERT(DATE,@dateFrom) AND CONVERT(DATE,@dateTo) 
                                    ORDER BY idTransHist DESC
                                    ");

                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = sQuery.ToString();
                        cmd.CommandType = CommandType.Text;

                        SqlParameter parm2 = new SqlParameter
                        {
                            ParameterName = "@item_number",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = item_number
                        };
                        cmd.Parameters.Add(parm2);

                        SqlParameter parm3 = new SqlParameter
                        {
                            ParameterName = "@Serial_No",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = Serial_No
                        };
                        cmd.Parameters.Add(parm3);

                        if (dateFrom.ToShortDateString() != "")
                        {
                            SqlParameter parm1 = new SqlParameter
                            {
                                ParameterName = "@dateFrom",
                                SqlDbType = SqlDbType.DateTime,
                                Value = dateFrom
                            };
                            cmd.Parameters.Add(parm1);
                        }

                        if (dateTo.ToShortDateString() != "")
                        {
                            SqlParameter parm1 = new SqlParameter
                            {
                                ParameterName = "@dateTo",
                                SqlDbType = SqlDbType.DateTime,
                                Value = dateTo
                            };
                            cmd.Parameters.Add(parm1);
                        }

                        var oreader = cmd.ExecuteReader();

                        while (oreader.Read())
                        {
                            trans_code = (string)oreader["Trans_Code"];
                            site = (string)oreader["Site"];

                            if (trans_code != "SLE" && site != "")
                            {
                                trans_amt = (decimal)oreader["Trans_Amt"];

                                for (int z = 0; z <= dtSite.Rows.Count - 1; z++)
                                {
                                    string dtsite = dtSite.Rows[z][0].ToString();

                                    if (dtsite == site)
                                    {
                                        int count = Convert.ToInt32(dtSite.Rows[z][1].ToString());
                                        decimal dtamt = Convert.ToDecimal(dtSite.Rows[z][2].ToString());

                                        count = count + 1;

                                        dtamt = dtamt + trans_amt;

                                        dtSite.Rows[z][1] = count;
                                        dtSite.Rows[z][2] = dtamt;
                                    }
                                }
                            }
                        }
                        oreader.Close();
                        cmd.Dispose();

                        sQuery.Clear();
                    }
                    connection.Close();
                }

                for (int z = 0; z <= dtSite.Rows.Count - 1; z++)
                {
                    if (dtSite.Rows[z][1].ToString() != "0")
                    {
                        DataRow dr = dtReport.NewRow();
                        dr[0] = dtItemNumber.Rows[x][0];
                        dr[1] = dtSite.Rows[z][0];
                        dr[2] = dtSite.Rows[z][1];
                        dr[3] = dtSite.Rows[z][2];
                        dr[4] = Convert.ToDecimal(dtSite.Rows[z][2]) / Convert.ToDecimal(dtSite.Rows[z][1]);
                        dtReport.Rows.Add(dr);
                    }
                }
            }
            return dtReport;

            #region "OLD CODE"
            //            //            sQuery.Append(@"SELECT DISTINCT (Order_No), Item_number, COUNT(b.Serial_No) as Qty, SUM(Trans_Amt) as Cost, (SELECT TOP 1 CONVERT(DATE,b.Trans_Date) FROM a_Trans_History as b WHERE a.Order_No = b.Order_No) as Date
            //            //FROM a_Trans_History as a 
            //            //INNER JOIN a_Item_Serial as b 
            //            //ON a.Serial_No = b.Serial_No 
            //            //WHERE Trans_Code = 'PUR' 
            //            //                         ");

            //            //            if (dateFrom.ToShortDateString() != "" && dateTo.ToShortDateString() != "")
            //            //            {
            //            //                sQuery.Append(" AND CONVERT(DATE,Trans_Date) BETWEEN CONVERT(DATE,@dateFrom)AND CONVERT(DATE,@dateTo) ");
            //            //            }            

            //            //            sQuery.Append(" GROUP BY Item_Number, Order_No ");

            //            sQuery.Append(@" SELECT DISTINCT Item_Number , Order_No, (SELECT TOP 1 CONVERT(DATE,b.Trans_Date) FROM a_Trans_History as b WHERE a.Order_No = b.Order_No) as Date, Trans_Amt
            //FROM a_Trans_History as a
            //WHERE CONVERT(DATE,Trans_Date) BETWEEN CONVERT(DATE,@dateFrom) AND CONVERT(DATE,@dateTo) 
            //AND Trans_Code = 'PUR' 
            //AND Site = 'WH-JMS'
            //");

            //            var dtItemNumber = new DataTable();

            //            connection.Open();

            //            using (SqlCommand cmd = new SqlCommand())
            //            {
            //                cmd.Connection = connection;
            //                cmd.CommandText = sQuery.ToString();
            //                cmd.CommandType = CommandType.Text;

            //                if (dateFrom.ToShortDateString() != "")
            //                {
            //                    SqlParameter parm1 = new SqlParameter
            //                    {
            //                        ParameterName = "@dateFrom",
            //                        SqlDbType = SqlDbType.DateTime,
            //                        Value = dateFrom
            //                    };
            //                    cmd.Parameters.Add(parm1);
            //                }

            //                if (dateTo.ToShortDateString() != "")
            //                {
            //                    SqlParameter parm1 = new SqlParameter
            //                    {
            //                        ParameterName = "@dateTo",
            //                        SqlDbType = SqlDbType.DateTime,
            //                        Value = dateTo
            //                    };
            //                    cmd.Parameters.Add(parm1);
            //                }

            //                sQuery.Clear();
            //                dtItemNumber.Load(cmd.ExecuteReader());

            //                cmd.Dispose();
            //            }

            //            connection.Close();

            //            for (int x = 0; x <= dtItemNumber.Rows.Count - 1; x++)
            //            {

            //                string LotSerial = "";

            //                sQuery.Append(@"SELECT TOP 1 Serial_No
            //                             FROM a_Trans_History 
            //                             WHERE Item_Number = @Item_Number 
            //                             AND CONVERT(DATE,Trans_Date) BETWEEN CONVERT(DATE,@dateFrom) AND CONVERT(DATE,@dateTo) 
            //                             AND Trans_Code = 'PUR'
            //AND Site = 'WH-JMS'
            //                             ORDER BY Serial_No DESC
            //                             ");
            //                connection.Open();
            //                using (SqlCommand cmd = new SqlCommand())
            //                {
            //                    cmd.Connection = connection;
            //                    cmd.CommandText = sQuery.ToString();
            //                    cmd.CommandType = CommandType.Text;
            //                    cmd.CommandTimeout = 2147483647;

            //                    if (dateFrom.ToShortDateString() != "")
            //                    {
            //                        SqlParameter parm1 = new SqlParameter
            //                        {
            //                            ParameterName = "@dateFrom",
            //                            SqlDbType = SqlDbType.DateTime,
            //                            Value = dateFrom
            //                        };
            //                        cmd.Parameters.Add(parm1);
            //                    }

            //                    if (dateTo.ToShortDateString() != "")
            //                    {
            //                        SqlParameter parm1 = new SqlParameter
            //                        {
            //                            ParameterName = "@dateTo",
            //                            SqlDbType = SqlDbType.DateTime,
            //                            Value = dateTo
            //                        };
            //                        cmd.Parameters.Add(parm1);
            //                    }

            //                    if (dtItemNumber.Rows[x][0].ToString() != "")
            //                    {
            //                        SqlParameter parm1 = new SqlParameter
            //                        {
            //                            ParameterName = "@Item_Number",
            //                            SqlDbType = SqlDbType.NVarChar,
            //                            Value = dtItemNumber.Rows[x][0].ToString()
            //                        };
            //                        cmd.Parameters.Add(parm1);
            //                    }

            //                    var oreader = cmd.ExecuteReader();

            //                    while (oreader.Read())
            //                    {
            //                        LotSerial = oreader["Serial_No"].ToString();
            //                    }

            //                    oreader.Close();
            //                    cmd.Dispose();
            //                    sQuery.Clear();
            //                }
            //                connection.Close();

            //                int serialcount = 0;
            //                int soldcount = 0;

            //                if (LotSerial.Substring(0, 3) == "AAA")
            //                {
            //                    //lot serial
            //                    sQuery.Append(@"SELECT Trans_Qty as serialcount 
            //                                    FROM a_Trans_History 
            //                                    WHERE Item_Number = @Item_Number 
            //                                    AND Order_No = @PO_number");
            //                    connection.Open();

            //                    using (SqlCommand cmd = new SqlCommand())
            //                    {
            //                        cmd.Connection = connection;
            //                        cmd.CommandText = sQuery.ToString();
            //                        cmd.CommandType = CommandType.Text;
            //                        cmd.CommandTimeout = 2147483647;

            //                        if (dtItemNumber.Rows[x][0].ToString() != "")
            //                        {
            //                            SqlParameter parm1 = new SqlParameter
            //                            {
            //                                ParameterName = "@Item_Number",
            //                                SqlDbType = SqlDbType.NVarChar,
            //                                Value = dtItemNumber.Rows[x][0].ToString()
            //                            };
            //                            cmd.Parameters.Add(parm1);
            //                        }

            //                        if (dtItemNumber.Rows[x][1].ToString() != "")
            //                        {
            //                            SqlParameter parm1 = new SqlParameter
            //                            {
            //                                ParameterName = "@PO_Number",
            //                                SqlDbType = SqlDbType.NVarChar,
            //                                Value = dtItemNumber.Rows[x][1].ToString()
            //                            };
            //                            cmd.Parameters.Add(parm1);
            //                        }

            //                        sQuery.Clear();
            //                        var oreader = cmd.ExecuteReader();

            //                        while (oreader.Read())
            //                        {
            //                            serialcount = (int)oreader["serialcount"];
            //                        }
            //                        oreader.Close();
            //                        cmd.Dispose();
            //                    }

            //                    connection.Close();
            //                    //////////////
            //                    //seriallist//
            //                    //////////////


            //                    sQuery.Append(@"SELECT COUNT(Serial_No) as soldcount
            //                                        FROM a_Trans_History 
            //                                        WHERE Serial_No LIKE '% + (SELECT Serial_No FROM a_Trans_History WHERE Item_Number = @Item_Number AND Order_No = @PO_Number ) + %'
            //                                        AND Trans_Code = 'SLE'                             
            //                                        AND CONVERT(DATE,Trans_Date) BETWEEN CONVERT(DATE,@dateFrom) AND CONVERT(DATE,@dateTo) 
            //                                        ");

            //                    connection.Open();
            //                    using (SqlCommand cmd = new SqlCommand())
            //                    {
            //                        cmd.Connection = connection;
            //                        cmd.CommandText = sQuery.ToString();
            //                        cmd.CommandType = CommandType.Text;
            //                        cmd.CommandTimeout = 2147483647;

            //                        if (dtItemNumber.Rows[x][1].ToString() != "")
            //                        {
            //                            SqlParameter parm1 = new SqlParameter
            //                            {
            //                                ParameterName = "@PO_Number",
            //                                SqlDbType = SqlDbType.NVarChar,
            //                                Value = dtItemNumber.Rows[x][1].ToString()
            //                            };
            //                            cmd.Parameters.Add(parm1);
            //                        }

            //                        if (dtItemNumber.Rows[x][0].ToString() != "")
            //                        {
            //                            SqlParameter parm1 = new SqlParameter
            //                            {
            //                                ParameterName = "@Item_Number",
            //                                SqlDbType = SqlDbType.NVarChar,
            //                                Value = dtItemNumber.Rows[x][0].ToString()
            //                            };
            //                            cmd.Parameters.Add(parm1);
            //                        }

            //                        if (dateFrom.ToShortDateString() != "")
            //                        {
            //                            SqlParameter parm1 = new SqlParameter
            //                            {
            //                                ParameterName = "@dateFrom",
            //                                SqlDbType = SqlDbType.DateTime,
            //                                Value = dateFrom
            //                            };
            //                            cmd.Parameters.Add(parm1);
            //                        }

            //                        if (dateTo.ToShortDateString() != "")
            //                        {
            //                            SqlParameter parm1 = new SqlParameter
            //                            {
            //                                ParameterName = "@dateTo",
            //                                SqlDbType = SqlDbType.DateTime,
            //                                Value = dateTo
            //                            };
            //                            cmd.Parameters.Add(parm1);
            //                        }

            //                        var oreader = cmd.ExecuteReader();

            //                        while (oreader.Read())
            //                        {
            //                            soldcount = (int)oreader["soldcount"];
            //                        }

            //                        oreader.Close();
            //                        cmd.Dispose();
            //                        sQuery.Clear();
            //                    }
            //                    connection.Close();
            //                    //sQuery.Append(@"SELECT COUNT(DISTINCT Serial_No) as soldcount
            //                    //                FROM a_Trans_History 
            //                    //                WHERE CONVERT(DATE,Trans_Date) BETWEEN CONVERT(DATE,@dateFrom) AND CONVERT(DATE,@dateTo) 
            //                    //                AND Trans_Code = 'SLE' 
            //                    //                AND Item_Number = @Item_Number
            //                    //                ");
            //                    //connection.Open();

            //                    //using (SqlCommand cmd = new SqlCommand())
            //                    //{
            //                    //    cmd.Connection = connection;
            //                    //    cmd.CommandText = sQuery.ToString();
            //                    //    cmd.CommandType = CommandType.Text;
            //                    //    cmd.CommandTimeout = 2147483647;

            //                    //    if (dateFrom.ToShortDateString() != "")
            //                    //    {
            //                    //        SqlParameter parm1 = new SqlParameter
            //                    //        {
            //                    //            ParameterName = "@dateFrom",
            //                    //            SqlDbType = SqlDbType.DateTime,
            //                    //            Value = dateFrom
            //                    //        };
            //                    //        cmd.Parameters.Add(parm1);
            //                    //    }

            //                    //    if (dateTo.ToShortDateString() != "")
            //                    //    {
            //                    //        SqlParameter parm1 = new SqlParameter
            //                    //        {
            //                    //            ParameterName = "@dateTo",
            //                    //            SqlDbType = SqlDbType.DateTime,
            //                    //            Value = dateTo
            //                    //        };
            //                    //        cmd.Parameters.Add(parm1);
            //                    //    }

            //                    //    if (dtItemNumber.Rows[x][0].ToString() != "")
            //                    //    {
            //                    //        SqlParameter parm1 = new SqlParameter
            //                    //        {
            //                    //            ParameterName = "@Item_Number",
            //                    //            SqlDbType = SqlDbType.NVarChar,
            //                    //            Value = dtItemNumber.Rows[x][0].ToString()
            //                    //        };
            //                    //        cmd.Parameters.Add(parm1);
            //                    //    }

            //                    //    sQuery.Clear();
            //                    //    var oreader = cmd.ExecuteReader();

            //                    //    while (oreader.Read())
            //                    //    {
            //                    //        soldcount = (int)oreader["soldcount"];
            //                    //    }
            //                    //    oreader.Close();
            //                    //    cmd.Dispose();
            //                    //}
            //                    //connection.Close();
            //                    /////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                }
            //                else
            //                {
            //                    //normal serial
            //                    sQuery.Append(@"SELECT COUNT(Serial_No) as serialcount
            //                                    FROM a_Trans_History 
            //                                    WHERE CONVERT(DATE,Trans_Date) BETWEEN CONVERT(DATE,@dateFrom) AND CONVERT(DATE,@dateTo)
            //                                    AND Trans_Code = 'PUR' 
            //                                    AND Item_Number = @Item_Number 
            //                                    AND Order_No = @Order_No ");
            //                    connection.Open();

            //                    using (SqlCommand cmd = new SqlCommand())
            //                    {
            //                        cmd.Connection = connection;
            //                        cmd.CommandText = sQuery.ToString();
            //                        cmd.CommandType = CommandType.Text;
            //                        cmd.CommandTimeout = 2147483647;

            //                        if (dateFrom.ToShortDateString() != "")
            //                        {
            //                            SqlParameter parm1 = new SqlParameter
            //                            {
            //                                ParameterName = "@dateFrom",
            //                                SqlDbType = SqlDbType.DateTime,
            //                                Value = dateFrom
            //                            };
            //                            cmd.Parameters.Add(parm1);
            //                        }

            //                        if (dateTo.ToShortDateString() != "")
            //                        {
            //                            SqlParameter parm1 = new SqlParameter
            //                            {
            //                                ParameterName = "@dateTo",
            //                                SqlDbType = SqlDbType.DateTime,
            //                                Value = dateTo
            //                            };
            //                            cmd.Parameters.Add(parm1);
            //                        }

            //                        if (dtItemNumber.Rows[x][0].ToString() != "")
            //                        {
            //                            SqlParameter parm1 = new SqlParameter
            //                            {
            //                                ParameterName = "@Item_Number",
            //                                SqlDbType = SqlDbType.NVarChar,
            //                                Value = dtItemNumber.Rows[x][0].ToString()
            //                            };
            //                            cmd.Parameters.Add(parm1);
            //                        }

            //                        if (dtItemNumber.Rows[x][1].ToString() != "")
            //                        {
            //                            SqlParameter parm1 = new SqlParameter
            //                            {
            //                                ParameterName = "@Order_No",
            //                                SqlDbType = SqlDbType.NVarChar,
            //                                Value = dtItemNumber.Rows[x][1].ToString()
            //                            };
            //                            cmd.Parameters.Add(parm1);
            //                        }

            //                        sQuery.Clear();
            //                        var oreader = cmd.ExecuteReader();

            //                        while (oreader.Read())
            //                        {
            //                            serialcount = (int)oreader["serialcount"];
            //                        }
            //                        oreader.Close();
            //                        cmd.Dispose();
            //                    }
            //                    connection.Close();

            //                    //sQuery.Append(@"SELECT COUNT(DISTINCT Serial_No) as soldcount
            //                    //                FROM a_Trans_History 
            //                    //                WHERE CONVERT(DATE,Trans_Date) BETWEEN CONVERT(DATE,@dateFrom) AND CONVERT(DATE,@dateTo) 
            //                    //                AND Trans_Code = 'SLE' 
            //                    //                AND Item_Number = @Item_Number ");
            //                    DataTable dtSerialList = new DataTable();

            //                    sQuery.Append(@"SELECT Serial_No
            //                                    FROM a_Trans_History 
            //                                    WHERE Trans_Code = 'PUR' 
            //                                    AND Item_Number = @Item_Number
            //                                    AND Order_No = @Order_No");
            //                    connection.Open();

            //                    using (SqlCommand cmd = new SqlCommand())
            //                    {
            //                        cmd.Connection = connection;
            //                        cmd.CommandText = sQuery.ToString();
            //                        cmd.CommandType = CommandType.Text;
            //                        cmd.CommandTimeout = 2147483647;

            //                        if (dateFrom.ToShortDateString() != "")
            //                        {
            //                            SqlParameter parm1 = new SqlParameter
            //                            {
            //                                ParameterName = "@dateFrom",
            //                                SqlDbType = SqlDbType.DateTime,
            //                                Value = dateFrom
            //                            };
            //                            cmd.Parameters.Add(parm1);
            //                        }

            //                        if (dateTo.ToShortDateString() != "")
            //                        {
            //                            SqlParameter parm1 = new SqlParameter
            //                            {
            //                                ParameterName = "@dateTo",
            //                                SqlDbType = SqlDbType.DateTime,
            //                                Value = dateTo
            //                            };
            //                            cmd.Parameters.Add(parm1);
            //                        }

            //                        if (dtItemNumber.Rows[x][0].ToString() != "")
            //                        {
            //                            SqlParameter parm1 = new SqlParameter
            //                            {
            //                                ParameterName = "@Item_Number",
            //                                SqlDbType = SqlDbType.NVarChar,
            //                                Value = dtItemNumber.Rows[x][0].ToString()
            //                            };
            //                            cmd.Parameters.Add(parm1);
            //                        }

            //                        if (dtItemNumber.Rows[x][1].ToString() != "")
            //                        {
            //                            SqlParameter parm1 = new SqlParameter
            //                            {
            //                                ParameterName = "@Order_No",
            //                                SqlDbType = SqlDbType.NVarChar,
            //                                Value = dtItemNumber.Rows[x][1].ToString()
            //                            };
            //                            cmd.Parameters.Add(parm1);
            //                        }
            //                        sQuery.Clear();

            //                        dtSerialList.Load(cmd.ExecuteReader());

            //                        cmd.Dispose();
            //                    }

            //                    connection.Close();

            //                    for (int y = 0; y <= dtSerialList.Rows.Count - 1; y++)
            //                    {
            //                        string serial_no = "";
            //                        sQuery.Append(@"SELECT TOP 1 Serial_No
            //                                        FROM a_Trans_History 
            //                                        WHERE Serial_No = @Serial_no 
            //                                        AND Trans_Code = 'SLE'                             
            //                                        AND CONVERT(DATE,Trans_Date) BETWEEN CONVERT(DATE,@dateFrom) AND CONVERT(DATE,@dateTo) 
            //                                        ");

            //                        connection.Open();
            //                        using (SqlCommand cmd = new SqlCommand())
            //                        {
            //                            cmd.Connection = connection;
            //                            cmd.CommandText = sQuery.ToString();
            //                            cmd.CommandType = CommandType.Text;
            //                            cmd.CommandTimeout = 2147483647;

            //                            SqlParameter parm5 = new SqlParameter
            //                            {
            //                                ParameterName = "@Serial_No",
            //                                SqlDbType = SqlDbType.NVarChar,
            //                                Value = dtSerialList.Rows[y][0].ToString()
            //                            };
            //                            cmd.Parameters.Add(parm5);

            //                            if (dateFrom.ToShortDateString() != "")
            //                            {
            //                                SqlParameter parm1 = new SqlParameter
            //                                {
            //                                    ParameterName = "@dateFrom",
            //                                    SqlDbType = SqlDbType.DateTime,
            //                                    Value = dateFrom
            //                                };
            //                                cmd.Parameters.Add(parm1);
            //                            }

            //                            if (dateTo.ToShortDateString() != "")
            //                            {
            //                                SqlParameter parm1 = new SqlParameter
            //                                {
            //                                    ParameterName = "@dateTo",
            //                                    SqlDbType = SqlDbType.DateTime,
            //                                    Value = dateTo
            //                                };
            //                                cmd.Parameters.Add(parm1);
            //                            }

            //                            var oreader = cmd.ExecuteReader();

            //                            while (oreader.Read())
            //                            {
            //                                serial_no = oreader["Serial_No"].ToString();

            //                                if (serial_no != "")
            //                                {
            //                                    soldcount = soldcount + 1;
            //                                }
            //                            }

            //                            oreader.Close();
            //                            cmd.Dispose();
            //                            sQuery.Clear();
            //                        }
            //                        connection.Close();
            //                    }

            //                }
            //                int notInStock = 0;
            //                sQuery.Clear();
            //                sQuery.Append(@"SELECT COUNT(DISTINCT Serial_No) as notInStock
            //                                    FROM a_Trans_History 
            //                                    WHERE CONVERT(DATE,Trans_Date) BETWEEN CONVERT(DATE,@dateFrom) AND CONVERT(DATE,@dateTo)
            //                                    AND Trans_Code = 'STO' AND Trans_Code != 'SLE' 
            //                                    AND Item_Number = @Item_Number 
            //                                    AND Order_No = @Order_No ");
            //                connection.Open();

            //                using (SqlCommand cmd = new SqlCommand())
            //                {
            //                    cmd.Connection = connection;
            //                    cmd.CommandText = sQuery.ToString();
            //                    cmd.CommandType = CommandType.Text;
            //                    cmd.CommandTimeout = 2147483647;

            //                    if (dateFrom.ToShortDateString() != "")
            //                    {
            //                        SqlParameter parm1 = new SqlParameter
            //                        {
            //                            ParameterName = "@dateFrom",
            //                            SqlDbType = SqlDbType.DateTime,
            //                            Value = dateFrom
            //                        };
            //                        cmd.Parameters.Add(parm1);
            //                    }

            //                    if (dateTo.ToShortDateString() != "")
            //                    {
            //                        SqlParameter parm1 = new SqlParameter
            //                        {
            //                            ParameterName = "@dateTo",
            //                            SqlDbType = SqlDbType.DateTime,
            //                            Value = dateTo
            //                        };
            //                        cmd.Parameters.Add(parm1);
            //                    }

            //                    if (dtItemNumber.Rows[x][0].ToString() != "")
            //                    {
            //                        SqlParameter parm1 = new SqlParameter
            //                        {
            //                            ParameterName = "@Item_Number",
            //                            SqlDbType = SqlDbType.NVarChar,
            //                            Value = dtItemNumber.Rows[x][0].ToString()
            //                        };
            //                        cmd.Parameters.Add(parm1);
            //                    }

            //                    if (dtItemNumber.Rows[x][1].ToString() != "")
            //                    {
            //                        SqlParameter parm1 = new SqlParameter
            //                        {
            //                            ParameterName = "@Order_No",
            //                            SqlDbType = SqlDbType.NVarChar,
            //                            Value = dtItemNumber.Rows[x][1].ToString()
            //                        };
            //                        cmd.Parameters.Add(parm1);
            //                    }

            //                    sQuery.Clear();
            //                    var oreader = cmd.ExecuteReader();

            //                    while (oreader.Read())
            //                    {
            //                        notInStock = (int)oreader["notInStock"];
            //                    }
            //                    oreader.Close();
            //                    cmd.Dispose();
            //                }
            //                connection.Close();

            //                int remainingStock = serialcount - (soldcount + notInStock);

            ////if (remainingStock < 0)
            ////    remainingStock = 0;

            //DataRow dr = dtReport.NewRow();
            //dr[0] = dtItemNumber.Rows[x][0];
            //dr[1] = dtItemNumber.Rows[x][1];
            //dr[2] = remainingStock;
            //dr[3] = Convert.ToDateTime(dtItemNumber.Rows[x][2]).ToShortDateString();
            //dr[4] = Convert.ToDecimal(dtItemNumber.Rows[x][3].ToString()) * Convert.ToDecimal(remainingStock);
            //dtReport.Rows.Add(dr);
            //}
            #endregion
        }

        public static DataTable RetrieveDataForStockTransfer(SqlConnection connection, string Item_Number, string Serial_No)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idTransHist
                         ,Trans_Code
                         ,Item_Number
                         ,Site
                         ,UM
                         ,Doc_No
                         ,Serial_No
                         ,Reason_Code
                         ,CONVERT(DATE,Trans_Date) as Date
                         ,Order_No
                         ,Invoice_No
                         ,Reference_No                       
                         FROM a_Trans_History
                         WHERE idTransHist <> 0
                         ");

            if (Serial_No != "")
            {
                sQuery.Append(" AND Serial_No LIKE '%' + @Serial_No + '%' ");
            }

            if (Item_Number != "")
            {
                sQuery.Append(" AND Item_Number LIKE '%' + @Item_Number + '%' ");
            }

            sQuery.Append(" ORDER BY Trans_Date ");

            var lmodel = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

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

                if (Item_Number != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@Item_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Item_Number
                    };
                    cmd.Parameters.Add(parm1);
                }

                lmodel.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static string GetItemLastState(SqlConnection connection, string Item_Number, string Serial_Number)
        {
            string returnValue = "";

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
                            SELECT TOP 1 Site FROM a_Trans_History WHERE Serial_No = @Serial_Number 
                            ");
            if (Item_Number != "")
            {
                sQuery.Append(@" AND Item_Number = @Item_Number");
            }

            sQuery.Append(@" ORDER BY Trans_Date DESC ");
            var lmodel = new List<Item_Serial_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm1 = new SqlParameter
                {
                    ParameterName = "@Item_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = Item_Number
                };
                cmd.Parameters.Add(parm1);

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@Serial_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = Serial_Number
                };
                cmd.Parameters.Add(parm2);

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    returnValue = (string)oreader["Site"];
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return returnValue;
        }

        public static decimal GetSerialCost(SqlConnection connection, string Item_Number, string Serial_Number)
        {
            decimal returnValue = 0;

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT TOP 1 Trans_Amt                        
                         FROM a_Trans_History
                         WHERE idTransHist <> 0
                         ");

            if (Serial_Number != "")
            {
                sQuery.Append(" AND Serial_No LIKE '%' + @Serial_No + '%' ");
            }

            if (Item_Number != "")
            {
                sQuery.Append(" AND Item_Number LIKE '%' + @Item_Number + '%' ");
            }

            sQuery.Append(" ORDER BY Trans_Date DESC ");

            var lmodel = new List<Item_Serial_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (Serial_Number != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@Serial_No",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Serial_Number
                    };
                    cmd.Parameters.Add(parm1);
                }

                if (Item_Number != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@Item_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Item_Number
                    };
                    cmd.Parameters.Add(parm1);
                }


                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    returnValue = (decimal)oreader["Trans_Amt"];
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return returnValue;
        }
    }
}
