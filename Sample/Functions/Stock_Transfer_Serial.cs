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
    public class Stock_Transfer_Serial
    {
        public static DataTable RetrieveData(SqlConnection connection, int idSTDetail)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT 
                         idSerial
                         FROM a_Stock_Transfer_Serial
                         WHERE idSTDetail = '" + idSTDetail + "'");

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

        public static bool Save(SqlConnection connection, Stock_Transfer_Serial_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_Stock_Transfer_Serial
                             (idSTDetail
                             ,idSerial
                             )
                             VALUES
                             (@idSTDetail
                             ,@idSerial
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
                        ParameterName = "@idSTDetail",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idSTDetail
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@idSerial",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idSerial
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
