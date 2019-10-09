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
    public class MIS_Detail
    {
        public static List<MIS_Detail_Model> RetrieveData(SqlConnection connection,  int idMISHeader,int idItem)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
                         SELECT idMISDetail
                         ,idMISHeader
                         ,idItem
                         ,Quantity
                         ,Cost
                         FROM a_MIS_Detail
WHERE idMISDetail <> 0
                        ");

            if (idMISHeader != 0)
            {
                sQuery.Append("AND idMISHeader = @idMISHeader");
            }
            if (idItem != 0)
            {
                sQuery.Append("AND idItem = @idItem");
            }

            var lmodel = new List<MIS_Detail_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (idItem != 0)
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idItem",
                        SqlDbType = SqlDbType.Int,
                        Value = idItem
                    };
                    cmd.Parameters.Add(parm2);
                }
                if (idMISHeader != 0)
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idMISHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = idMISHeader
                    };
                    cmd.Parameters.Add(parm2);
                }
                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    MIS_Detail_Model oModel = new MIS_Detail_Model
                    {
                        idMISDetail = (int)oreader["idMISDetail"],
                        idMISHeader = (int)oreader["idMISHeader"],
                        idItem = (int)oreader["idItem"],
                        Quantity = (int)oreader["Quantity"],
                        Cost = (decimal)oreader["Cost"]
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static int Save(SqlConnection connection, MIS_Detail_Model model)
        {
            int returnValue = 0;
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_MIS_Detail
                             (idMISHeader
                             ,idItem
                             ,Quantity
                             ,Cost
                             )
                             VALUES
                             (@idMISHeader
                             ,@idItem
                             ,@Quantity
                             ,@Cost
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
                    ParameterName = "@idMISHeader",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idMISHeader
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
                    ParameterName = "@Quantity",
                    SqlDbType = SqlDbType.Int,
                    Value = model.Quantity
                };
                cmd.Parameters.Add(parm4);

                SqlParameter parm5 = new SqlParameter
                {
                    ParameterName = "@Cost",
                    SqlDbType = SqlDbType.Decimal,
                    Value = model.Cost
                };
                cmd.Parameters.Add(parm5);

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
    }
}
