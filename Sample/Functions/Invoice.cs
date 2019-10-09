using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class Invoice
    {
        public static List<Invoice_Model> RetrieveData(SqlConnection connection, string Invoice_Number, string SO_Number)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idInvoice
      ,Invoice_Number
      ,idSOHeader
      ,DR_Number
      ,Invoice_Date
      ,Amount
,Del_Date
,OR_Number
                         FROM a_Invoice
WHERE idInvoice <> 0 
                         ");


            if (Invoice_Number != "")
            {
                sQuery.Append(" AND Invoice_number = @Invoice_Number ");
            }

            if (SO_Number != "")
            {
                sQuery.Append(" AND idSOHeader = (SELECT idSOHeader FROM a_SO_Header WHERE SO_Number = @SO_Number) ");
            }

            var lmodel = new List<Invoice_Model>();

            DataTable dataTable = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (Invoice_Number != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@Invoice_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Invoice_Number
                    };
                    cmd.Parameters.Add(parm1);
                }

                if (SO_Number != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@SO_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = SO_Number
                    };
                    cmd.Parameters.Add(parm2);
                }

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Invoice_Model oModel = new Invoice_Model
                    {
                        idInvoice = (int)oreader["idInvoice"],
                        idSOHeader = (int)oreader["idSOHeader"],
                        Invoice_Number = (string)oreader["Invoice_Number"],
                        DR_Number = (string)oreader["DR_Number"],
                        Invoice_Date = (DateTime)oreader["Invoice_Date"],
                        Amount = (decimal)oreader["Amount"],
                        Del_Date = (DateTime)oreader["Del_Date"],
                        OR_Number = (string)oreader["OR_Number"],
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static bool Save(SqlConnection connection, Invoice_Model model)
        {
            bool returnValue = true;
            
            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
INSERT INTO dbo.a_Invoice
           (Invoice_Number
           ,idSOHeader
           ,Invoice_Date
           ,DR_Number
           ,Amount
,Del_Date
,OR_Number)
     VALUES
           (@Invoice_Number
           ,@idSOHeader
           ,@Invoice_Date
           ,@DR_Number
           ,@Amount
,@Del_Date
,@OR_Number)
");

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@Invoice_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Invoice_Number
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idSOHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idSOHeader
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Invoice_Date",
                        SqlDbType = SqlDbType.DateTime,
                        Value = model.Invoice_Date
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@Del_Date",
                        SqlDbType = SqlDbType.DateTime,
                        Value = model.Del_Date
                    };
                    cmd.Parameters.Add(parm6);
                    
                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@DR_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.DR_Number
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@Amount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Amount
                    };
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm7 = new SqlParameter
                    {
                        ParameterName = "@OR_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.OR_Number
                    };
                    cmd.Parameters.Add(parm7);

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

        public static bool Update(SqlConnection connection, Invoice_Model model)
        {
            bool returnValue = true;
            
            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
UPDATE a_Invoice
   SET Invoice_Number = @Invoice_Number
      ,idSOHeader = @idSOHeader
      ,DR_Number = @DR_Number
      ,Invoice_Date = @Invoice_Date
      ,Amount = @Amount
,Del_Date = @Del_Date
,OR_Number = @OR_Number
 WHERE idInvoice = @idInvoice
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
                        ParameterName = "@idInvoice",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idInvoice
                    };
                    cmd.Parameters.Add(parm);

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@Invoice_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Invoice_Number
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idSOHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idSOHeader
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Invoice_Date",
                        SqlDbType = SqlDbType.DateTime,
                        Value = model.Invoice_Date
                    };
                    cmd.Parameters.Add(parm3);
                    
                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@Del_Date",
                        SqlDbType = SqlDbType.DateTime,
                        Value = model.Del_Date
                    };
                    cmd.Parameters.Add(parm6);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@DR_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.DR_Number
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@Amount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Amount
                    };
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm7 = new SqlParameter
                    {
                        ParameterName = "@OR_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.OR_Number
                    };
                    cmd.Parameters.Add(parm7);

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

        public static bool Delete(SqlConnection connection, int idInvoice)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Invoice ");
            sQuery.Append("WHERE idInvoice = @idInvoice");

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
                        ParameterName = "@idInvoice",
                        SqlDbType = SqlDbType.Int,
                        Value = idInvoice
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
