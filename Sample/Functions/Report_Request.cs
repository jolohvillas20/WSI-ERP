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
    public class Report_Request
    {
        public static List<Report_Request_Model> RetrieveData(SqlConnection connection, string DateFrom, string Request_Type)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
                         SELECT idReportRequest
                         ,Request_Type
                         ,DateFrom
                         ,DateTo
                         ,SendToEmail
                         ,isFinished
                         ,TimeStamp
,CreatedBy
                         FROM a_Report_Request
WHERE idReportRequest <> 0
                        ");

            if (Request_Type != "")
            {
                sQuery.Append("AND Request_Type = @Request_Type");
            }
            if (DateFrom != "")
            {
                sQuery.Append("AND DateFrom = @DateFrom");
            }

            var lmodel = new List<Report_Request_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (DateFrom != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@DateFrom",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = DateFrom
                    };
                    cmd.Parameters.Add(parm2);
                }
                if (Request_Type != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Request_Type",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Request_Type
                    };
                    cmd.Parameters.Add(parm2);
                }
                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Report_Request_Model oModel = new Report_Request_Model
                    {
                        idReportRequest = (int)oreader["idReportRequest"],
                        Request_Type = (string)oreader["Request_Type"],
                        DateFrom = (DateTime)oreader["DateFrom"],
                        DateTo = (DateTime)oreader["DateTo"],
                        SendToEmail = (string)oreader["SendToEmail"],
                        isFinished = (string)oreader["isFinished"],
                        TimeStamp = (DateTime)oreader["TimeStamp"],
                        CreatedBy = ((int)oreader["CreatedBy"]).ToString()
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static bool Save(SqlConnection connection, Report_Request_Model model)
        {
            bool returnValue = false;
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_Report_Request
                             (Request_Type
                             ,DateFrom
                             ,DateTo
                             ,SendToEmail
                             ,isFinished
                             ,TimeStamp
                             ,CreatedBy
                             )
                             VALUES
                             (@Request_Type
                             ,@DateFrom
                             ,@DateTo
                             ,@SendToEmail
                             ,@isFinished
                             ,@TimeStamp
                             ,@CreatedBy
                             )

                            

                             ");
            // SELECT SCOPE_IDENTITY() as 'ID'
            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@Request_Type",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Request_Type
                };
                cmd.Parameters.Add(parm2);

                SqlParameter parm3 = new SqlParameter
                {
                    ParameterName = "@DateFrom",
                    SqlDbType = SqlDbType.DateTime,
                    Value = model.DateFrom
                };
                cmd.Parameters.Add(parm3);

                SqlParameter parm4 = new SqlParameter
                {
                    ParameterName = "@DateTo",
                    SqlDbType = SqlDbType.DateTime,
                    Value = model.DateTo
                };
                cmd.Parameters.Add(parm4);

                SqlParameter parm5 = new SqlParameter
                {
                    ParameterName = "@SendToEmail",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.SendToEmail
                };
                cmd.Parameters.Add(parm5);

                SqlParameter parm6 = new SqlParameter
                {
                    ParameterName = "@isFinished",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.isFinished
                };
                cmd.Parameters.Add(parm6);

                SqlParameter parm7 = new SqlParameter
                {
                    ParameterName = "@TimeStamp",
                    SqlDbType = SqlDbType.DateTime,
                    Value = model.TimeStamp
                };
                cmd.Parameters.Add(parm7);

                SqlParameter parm8 = new SqlParameter
                {
                    ParameterName = "@CreatedBy",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.CreatedBy
                };
                cmd.Parameters.Add(parm8);
                
               
              //  var oreader = cmd.ExecuteReader();
                try
                {
                    if (cmd.ExecuteNonQuery() >= 1)
                        returnValue = true;
                    else
                        returnValue = false;
                    //while (oreader.Read())
                    //{
                    //    returnValue = Convert.ToInt32(oreader["ID"].ToString());
                    //}
                    //oreader.Close();
                    cmd.Dispose();
                    cmd.Parameters.Clear();
                    SQL_Transact.CommitTransaction(connection, GUID);
                }
                catch
                {
                    returnValue = false;
                   // oreader.Close();
                    cmd.Dispose();
                    cmd.Parameters.Clear();
                    SQL_Transact.RollbackTransaction(connection, GUID);
                }
            }
            return returnValue;
        }

        public static bool Update(SqlConnection connection, Report_Request_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_Report_Request SET
                             Request_Type = @Request_Type
                             ,DateFrom = @DateFrom
                             ,DateTo = @DateTo
                             ,SendToEmail = @SendToEmail
                             ,isFinished = @isFinished
                             ,TimeStamp = @TimeStamp
                             ,CreatedBy = @CreatedBy
                             WHERE idReportRequest = @idReportRequest ");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idReportRequest",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idReportRequest
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Request_Type",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Request_Type
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@DateFrom",
                        SqlDbType = SqlDbType.DateTime,
                        Value = model.DateFrom
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@DateTo",
                        SqlDbType = SqlDbType.DateTime,
                        Value = model.DateTo
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@SendToEmail",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.SendToEmail
                    };
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@isFinished",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.isFinished
                    };
                    cmd.Parameters.Add(parm6);

                    SqlParameter parm7 = new SqlParameter
                    {
                        ParameterName = "@TimeStamp",
                        SqlDbType = SqlDbType.DateTime,
                        Value = model.TimeStamp
                    };
                    cmd.Parameters.Add(parm7);

                    SqlParameter parm8 = new SqlParameter
                    {
                        ParameterName = "@CreatedBy",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.CreatedBy
                    };
                    cmd.Parameters.Add(parm8);
                    
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

        public static bool Delete(SqlConnection connection, int idReportRequest)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Report_Request ");
            sQuery.Append("WHERE idReportRequest = @idReportRequest");

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
                        ParameterName = "@idReportRequest",
                        SqlDbType = SqlDbType.Int,
                        Value = idReportRequest
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
