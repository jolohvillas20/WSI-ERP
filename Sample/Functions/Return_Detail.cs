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
    public class Return_Detail
    {
        public static DataTable RetrieveData(SqlConnection connection, string idReturnHeader)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idReturnDetail
                         ,idReturnHeader
                         ,idItem
                         ,Returned_Qty
                         ,Returned_Cost
                         FROM a_Return_Detail
                         WHERE idReturnHeader = @idReturnHeader
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
                    ParameterName = "@idReturnHeader",
                    SqlDbType = SqlDbType.Int,
                    Value = idReturnHeader
                };
                cmd.Parameters.Add(parm1);

                lmodel.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }
        public static int Save(SqlConnection connection, Return_Detail_Model model)
        {
            int returnValue = 0;
            StringBuilder sQuery = new StringBuilder();


            sQuery.Append(@"INSERT INTO a_Return_Detail
                             (idReturnHeader
                             ,idItem 
                             ,Returned_Qty
                             ,Returned_Cost
                             )
                             VALUES
                             (@idReturnHeader
                             ,@idItem 
                             ,@Returned_Qty
                             ,@Returned_Cost
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
                    ParameterName = "@idReturnHeader",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idReturnHeader
                };
                cmd.Parameters.Add(parm2);

                SqlParameter parm3 = new SqlParameter
                {
                    ParameterName = "@idItem",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idItem
                };
                cmd.Parameters.Add(parm3);

                SqlParameter parm4 = new SqlParameter
                {
                    ParameterName = "@Returned_Qty",
                    SqlDbType = SqlDbType.Int,
                    Value = model.Returned_Qty
                };
                cmd.Parameters.Add(parm4);

                SqlParameter parm5 = new SqlParameter
                {
                    ParameterName = "@Returned_Cost",
                    SqlDbType = SqlDbType.Decimal,
                    Value = model.Returned_Cost
                };
                cmd.Parameters.Add(parm5);

                //if (cmd.ExecuteNonQuery() >= 1)
                //    returnValue = 0;

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
                    oreader.Close();
                    cmd.Dispose();
                    cmd.Parameters.Clear();
                    SQL_Transact.RollbackTransaction(connection, GUID);
                }
            }

            return returnValue;
        }
    }
}
