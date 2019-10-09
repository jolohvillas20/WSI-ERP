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
    public class Pick_Detail
    {
        public static DataTable RetrieveData(SqlConnection connection, string idPickHeader)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idPickDetail
                         ,idItem
                         ,Item_Number
                         ,Description
                         ,Qty
                         ,Items_Picked
                         FROM a_Pick_Detail
                         WHERE idPickHeader = @idPickHeader
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
                    ParameterName = "@idPickHeader",
                    SqlDbType = SqlDbType.Int,
                    Value = idPickHeader
                };
                cmd.Parameters.Add(parm1);

                lmodel.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }
        public static int Save(SqlConnection connection, Pick_Detail_Model model)
        {
            int returnValue = 0;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_Pick_Detail
                             (idPickHeader
                             ,idItem 
                             ,Item_Number
                             ,Description 
                             ,Qty 
                             ,Items_Picked 
                             )
                             VALUES
                             (@idPickHeader
                             ,@idItem 
                             ,@Item_Number
                             ,@Description 
                             ,@Qty 
                             ,@Items_Picked 
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
                    ParameterName = "@idPickHeader",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idPickHeader
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
                    ParameterName = "@Item_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Item_Number
                };
                cmd.Parameters.Add(parm4);

                SqlParameter parm5 = new SqlParameter
                {
                    ParameterName = "@Description",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Description
                };
                cmd.Parameters.Add(parm5);

                SqlParameter parm6 = new SqlParameter
                {
                    ParameterName = "@Qty",
                    SqlDbType = SqlDbType.Int,
                    Value = model.Qty
                };
                cmd.Parameters.Add(parm6);

                SqlParameter parm7 = new SqlParameter
                {
                    ParameterName = "@Items_Picked",
                    SqlDbType = SqlDbType.Int,
                    Value = model.Items_Picked
                };
                cmd.Parameters.Add(parm7);

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
