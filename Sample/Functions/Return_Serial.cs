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
    public class Return_Serial
    {
        public static DataTable RetrieveData(SqlConnection connection, string idReturnDetail)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idReturnSerial
                         ,idReturnDetail
                         ,Serial_No
                         FROM a_Return_Serial
                         WHERE idReturnDetail = @idReturnDetail
                         ");

            var lmodel = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm1 = new SqlParameter
                {
                    ParameterName = "@idReturnDetail",
                    SqlDbType = SqlDbType.Int,
                    Value = idReturnDetail
                };
                cmd.Parameters.Add(parm1);

                lmodel.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }
        public static bool Save(SqlConnection connection, Return_Serial_Model model)
        {
            bool returnValue = false;
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_Return_Serial
                             (idReturnDetail
                             ,Serial_No
                             ,Replacement_Serial
                             )
                             VALUES
                             (@idReturnDetail
                             ,@Serial_No
                             ,@Replacement_Serial
                             )
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
                    ParameterName = "@idReturnDetail",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idReturnDetail
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
                    ParameterName = "@Replacement_Serial",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Replacement_Serial
                };
                cmd.Parameters.Add(parm4);

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
                    returnValue = false;
                    cmd.Dispose();
                    cmd.Parameters.Clear();
                    SQL_Transact.RollbackTransaction(connection, GUID);
                }
            }
            return returnValue;
        }
    }
}
