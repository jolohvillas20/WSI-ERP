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
    public class Return_Header
    {
        public static List<Return_Header_Model> RetrieveData(SqlConnection connection, string SO_Number, string SLR_Number)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
                         SELECT idReturnHeader
                         ,SLR_Number
                         ,SO_Number
                         ,Customer_Code
                         ,Auth_Number
                         ,isReplacement
                         ,Document_Number
,Site
,Reason_Code
,Date_Returned
,Remarks
                         FROM a_Return_Header
WHERE idReturnHeader <> 0
                        ");

            if (SLR_Number != "")
            {
                sQuery.Append("AND SLR_Number = @SLR_Number");
            }
            if (SO_Number != "")
            {
                sQuery.Append("AND SO_Number = @SO_Number");
            }

            var lmodel = new List<Return_Header_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

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
                if (SLR_Number != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@SLR_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = SLR_Number
                    };
                    cmd.Parameters.Add(parm2);
                }
                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Return_Header_Model oModel = new Return_Header_Model
                    {
                        idReturnHeader = (int)oreader["idReturnHeader"],
                        SLR_Number = (string)oreader["SLR_Number"],
                        SO_Number = (string)oreader["SO_Number"],
                        Customer_Code = (string)oreader["Customer_Code"],
                        Auth_Number = (string)oreader["Auth_Number"],
                        isReplacement = (string)oreader["isReplacement"],
                        Document_Number = (string)oreader["Document_Number"],
                        Site = ((int)oreader["Site"]).ToString(),
                        Reason_Code = (string)oreader["Reason_Code"],
                        Date_Returned = (DateTime)oreader["Date_Returned"],
                        Remarks = (string)oreader["Remarks"]
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static int Save(SqlConnection connection, Return_Header_Model model)
        {
            int returnValue = 0;
            StringBuilder sQuery = new StringBuilder();
            
            sQuery.Append(@"INSERT INTO a_Return_Header
                             (SLR_Number
                             ,SO_Number
                             ,Customer_Code
                             ,Auth_Number
                             ,isReplacement
                             ,Document_Number
                             ,Site
                             ,Reason_Code
                             ,Date_Returned
,Remarks
                             )
                             VALUES
                             (@SLR_Number
                             ,@SO_Number
                             ,@Customer_Code
                             ,@Auth_Number
                             ,@isReplacement
                             ,@Document_Number
                             ,@Site
                             ,@Reason_Code
                             ,@Date_Returned
,@Remarks
                             )

                             SELECT SCOPE_IDENTITY() as 'ID'

                             ");
            
            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@SLR_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.SLR_Number
                };
                cmd.Parameters.Add(parm2);

                SqlParameter parm3 = new SqlParameter
                {
                    ParameterName = "@SO_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.SO_Number
                };
                cmd.Parameters.Add(parm3);

                SqlParameter parm4 = new SqlParameter
                {
                    ParameterName = "@Customer_Code",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Customer_Code
                };
                cmd.Parameters.Add(parm4);

                SqlParameter parm5 = new SqlParameter
                {
                    ParameterName = "@Auth_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Auth_Number
                };
                cmd.Parameters.Add(parm5);

                SqlParameter parm6 = new SqlParameter
                {
                    ParameterName = "@isReplacement",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.isReplacement
                };
                cmd.Parameters.Add(parm6);

                SqlParameter parm7 = new SqlParameter
                {
                    ParameterName = "@Document_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Document_Number
                };
                cmd.Parameters.Add(parm7);

                SqlParameter parm8 = new SqlParameter
                {
                    ParameterName = "@Site",
                    SqlDbType = SqlDbType.Int,
                    Value = model.Site
                };
                cmd.Parameters.Add(parm8);

                SqlParameter parm9 = new SqlParameter
                {
                    ParameterName = "@Reason_Code",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Reason_Code
                };
                cmd.Parameters.Add(parm9);

                SqlParameter parm10 = new SqlParameter
                {
                    ParameterName = "@Date_Returned",
                    SqlDbType = SqlDbType.DateTime,
                    Value = model.Date_Returned
                };
                cmd.Parameters.Add(parm10);

                SqlParameter parm11 = new SqlParameter
                {
                    ParameterName = "@Remarks",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Remarks
                };
                cmd.Parameters.Add(parm11);

                //if (cmd.ExecuteNonQuery() >= 1)
                //    returnValue = true;

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

        public static bool Update(SqlConnection connection, Return_Header_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_Return_Header SET
                             SLR_Number = @SLR_Number
                             ,SO_Number = @SO_Number
                             ,Customer_Code = @Customer_Code
                             ,Auth_Number = @Auth_Number
                             ,isReplacement = @isReplacement
                             ,Document_Number = @Document_Number
                             ,Site = @Site
                             ,Reason_Code = @Reason_Code
                             ,Date_Returned = @Date_Returned
,Remarks = @Remarks
                             WHERE idReturnHeader = @idReturnHeader ");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idReturnHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idReturnHeader
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@SLR_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.SLR_Number
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@SO_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.SO_Number
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@Customer_Code",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Customer_Code
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@Auth_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Auth_Number
                    };
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@isReplacement",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.isReplacement
                    };
                    cmd.Parameters.Add(parm6);

                    SqlParameter parm7 = new SqlParameter
                    {
                        ParameterName = "@Document_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Document_Number
                    };
                    cmd.Parameters.Add(parm7);

                    SqlParameter parm8 = new SqlParameter
                    {
                        ParameterName = "@Site",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Site
                    };
                    cmd.Parameters.Add(parm8);

                    SqlParameter parm9 = new SqlParameter
                    {
                        ParameterName = "@Reason_Code",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Reason_Code
                    };
                    cmd.Parameters.Add(parm9);

                    SqlParameter parm10 = new SqlParameter
                    {
                        ParameterName = "@Date_Returned",
                        SqlDbType = SqlDbType.DateTime,
                        Value = model.Date_Returned
                    };
                    cmd.Parameters.Add(parm10);

                    SqlParameter parm11 = new SqlParameter
                    {
                        ParameterName = "@Remarks",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Remarks
                    };
                    cmd.Parameters.Add(parm11);

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

        public static bool Delete(SqlConnection connection, int idReturnHeader)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Return_Header ");
            sQuery.Append("WHERE idReturnHeader = @idReturnHeader");

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
                        ParameterName = "@idReturnHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = idReturnHeader
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

        public static string GetLastSLRNumber(SqlConnection connection)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("SELECT TOP 1 SLR_Number FROM a_Return_Header ");
            sQuery.Append("ORDER BY SLR_Number DESC");

            string resultValue = "";

            try
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;
                    var oreader = cmd.ExecuteReader();

                    while (oreader.Read())
                    {
                        resultValue = oreader["SLR_Number"].ToString();
                    }
                    oreader.Close();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return resultValue;
        }
    }
}
