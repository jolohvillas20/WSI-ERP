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
    public class Pick_Serial
    {
        public static DataTable RetrieveData(SqlConnection connection, int idPickDetail)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT 
                         idSerial
                         ,Serial_No
                         ,PO_Number
                         FROM a_Pick_Serial
                         WHERE idPickDetail = '" + idPickDetail + "'");

            var dt = new DataTable();

            connection.Open();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(sQuery.ToString(), connection))
                {
                    using (DataTable ds = new DataTable())
                    {
                        da.Fill(ds);
                        dt = ds;
                    }
                }
            }
            catch
            {
                SqlConnection.ClearAllPools();
            }

            connection.Close();

            return dt;
        }

        public static DataTable RetrieveData(SqlConnection connection, int idItem, string SO_Number)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
SELECT        dbo.a_Pick_Serial.idSerial, dbo.a_Pick_Serial.Serial_No
FROM            dbo.a_Pick_Serial INNER JOIN
                         dbo.a_Pick_Detail ON dbo.a_Pick_Serial.idPickDetail = dbo.a_Pick_Detail.idPickDetail INNER JOIN
                         dbo.a_Pick_Header ON dbo.a_Pick_Detail.idPickHeader = dbo.a_Pick_Header.idPickHeader INNER JOIN
                         dbo.a_SO_Header ON dbo.a_Pick_Header.idSOHeader = dbo.a_SO_Header.idSOHeader
WHERE        dbo.a_SO_Header.SO_Number = '" + SO_Number + "' AND idItem =  '" + idItem + "'");

            var dt = new DataTable();

            connection.Open();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(sQuery.ToString(), connection))
                {
                    using (DataTable ds = new DataTable())
                    {
                        da.Fill(ds);
                        dt = ds;
                    }
                }
            }
            catch
            {
                SqlConnection.ClearAllPools();
            }

            connection.Close();

            return dt;
        }

        public static bool Save(SqlConnection connection, Pick_Serial_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_Pick_Serial
                             (idPickDetail
                             ,idSerial
                             ,Serial_No
                             ,PO_Number
                             )
                             VALUES
                             (@idPickDetail
                             ,@idSerial
                             ,@Serial_No
                             ,@PO_Number
                             )

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
                        ParameterName = "@idPickDetail",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idPickDetail
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@idSerial",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idSerial
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@Serial_No",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Serial_No
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@PO_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.PO_Number
                    };
                    cmd.Parameters.Add(parm5);

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
    }
}
