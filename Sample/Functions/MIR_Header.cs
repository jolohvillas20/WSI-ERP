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
   public class MIR_Header
    {
        public static List<MIR_Header_Model> RetrieveData(SqlConnection connection, string RequestNo)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
                         SELECT idMIRHeader
                         ,RequestNo
                         ,Requestor
                         ,ReferenceNo
                         ,POCMNumber
                         ,RequestDate
                         ,PreparedBy
,Remarks
                         FROM a_MIR_Header
WHERE idMIRHeader <> 0
                        ");

            if (RequestNo != "")
            {
                sQuery.Append("AND RequestNo LIKE '%' + @RequestNo + '%'");
            }      

            var lmodel = new List<MIR_Header_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (RequestNo != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@RequestNo",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = RequestNo
                    };
                    cmd.Parameters.Add(parm2);
                }
                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    MIR_Header_Model oModel = new MIR_Header_Model
                    {
                        idMIRHeader = (int)oreader["idMIRHeader"],
                        RequestNo = (string)oreader["RequestNo"],
                        Requestor = (string)oreader["Requestor"],
                        ReferenceNo = (string)oreader["ReferenceNo"],
                        POCMNumber = (string)oreader["POCMNumber"],
                        RequestDate = (DateTime)oreader["RequestDate"],
                        PreparedBy = (string)oreader["PreparedBy"],
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

        public static List<MIR_Header_Model> RetrieveData(SqlConnection connection, int idMIRHeader)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
                         SELECT idMIRHeader
                         ,RequestNo
                         ,Requestor
                         ,ReferenceNo
                         ,POCMNumber
                         ,RequestDate
                         ,PreparedBy
,Remarks
                         FROM a_MIR_Header
WHERE idMIRHeader <> 0
                        ");

            if (idMIRHeader != 0)
            {
                sQuery.Append("AND idMIRHeader = @idMIRHeader");
            }

            var lmodel = new List<MIR_Header_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (idMIRHeader != 0)
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idMIRHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = idMIRHeader
                    };
                    cmd.Parameters.Add(parm2);
                }
                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    MIR_Header_Model oModel = new MIR_Header_Model
                    {
                        idMIRHeader = (int)oreader["idMIRHeader"],
                        RequestNo = (string)oreader["RequestNo"],
                        Requestor = (string)oreader["Requestor"],
                        ReferenceNo = (string)oreader["ReferenceNo"],
                        POCMNumber = (string)oreader["POCMNumber"],
                        RequestDate = (DateTime)oreader["RequestDate"],
                        PreparedBy = (string)oreader["PreparedBy"],
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

        public static int Save(SqlConnection connection, MIR_Header_Model model)
        {
            int returnValue = 0;
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_MIR_Header
                             (RequestNo
                             ,Requestor
                             ,ReferenceNo
                             ,POCMNumber
                             ,RequestDate
                             ,PreparedBy
,Remarks
                             )
                             VALUES
                             (@RequestNo
                             ,@Requestor
                             ,@ReferenceNo
                             ,@POCMNumber
                             ,@RequestDate
                             ,@PreparedBy
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
                    ParameterName = "@RequestNo",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.RequestNo
                };
                cmd.Parameters.Add(parm2);

                SqlParameter parm3 = new SqlParameter
                {
                    ParameterName = "@Requestor",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Requestor
                };
                cmd.Parameters.Add(parm3);

                SqlParameter parm4 = new SqlParameter
                {
                    ParameterName = "@ReferenceNo",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.ReferenceNo
                };
                cmd.Parameters.Add(parm4);

                SqlParameter parm5 = new SqlParameter
                {
                    ParameterName = "@POCMNumber",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.POCMNumber
                };
                cmd.Parameters.Add(parm5);

                SqlParameter parm6 = new SqlParameter
                {
                    ParameterName = "@RequestDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = model.RequestDate
                };
                cmd.Parameters.Add(parm6);

                SqlParameter parm7 = new SqlParameter
                {
                    ParameterName = "@PreparedBy",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.PreparedBy
                };
                cmd.Parameters.Add(parm7);

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

        public static bool Update(SqlConnection connection, MIR_Header_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_MIR_Header SET
                             RequestNo = @RequestNo
                             ,Requestor = @Requestor
                             ,ReferenceNo = @ReferenceNo
                             ,POCMNumber = @POCMNumber
                             ,RequestDate = @RequestDate
                             ,PreparedBy = @PreparedBy
,Remarks = @Remarks
                             WHERE idMIRHeader = @idMIRHeader ");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idMIRHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idMIRHeader
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@RequestNo",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.RequestNo
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Requestor",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Requestor
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@ReferenceNo",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.ReferenceNo
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@POCMNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.POCMNumber
                    };
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@RequestDate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = model.RequestDate
                    };
                    cmd.Parameters.Add(parm6);

                    SqlParameter parm7 = new SqlParameter
                    {
                        ParameterName = "@PreparedBy",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.PreparedBy
                    };
                    cmd.Parameters.Add(parm7);
                    
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

        public static bool Delete(SqlConnection connection, int idMIRHeader)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_MIR_Header ");
            sQuery.Append("WHERE idMIRHeader = @idMIRHeader");

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
                        ParameterName = "@idMIRHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = idMIRHeader
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
