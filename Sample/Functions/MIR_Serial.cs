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
    public class MIR_Serial
    {
        public static List<MIR_Serial_Model> RetrieveData(SqlConnection connection, int idMIRDetail, int idSerial)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
                         SELECT idMIRSerial
                         ,idMIRDetail
                         ,idSerial
                         FROM a_MIR_Serial
WHERE idMIRSerial <> 0
                        ");

            if (idMIRDetail != 0)
            {
                sQuery.Append("AND idMIRDetail = @idMIRDetail");
            }
            if (idSerial != 0)
            {
                sQuery.Append("AND idSerial = @idSerial");
            }

            var lmodel = new List<MIR_Serial_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (idSerial != 0)
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idSerial",
                        SqlDbType = SqlDbType.Int,
                        Value = idSerial
                    };
                    cmd.Parameters.Add(parm2);
                }
                if (idMIRDetail != 0)
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idMIRDetail",
                        SqlDbType = SqlDbType.Int,
                        Value = idMIRDetail
                    };
                    cmd.Parameters.Add(parm2);
                }
                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    MIR_Serial_Model oModel = new MIR_Serial_Model
                    {
                        idMIRSerial = (int)oreader["idMIRSerial"],
                        idMIRDetail = (int)oreader["idMIRDetail"],
                        idSerial = (int)oreader["idSerial"]
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static int Save(SqlConnection connection, MIR_Serial_Model model)
        {
            int returnValue = 0;
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_MIR_Serial
                             (idMIRDetail
                             ,idSerial
                             )
                             VALUES
                             (@idMIRDetail
                             ,@idSerial
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
                    ParameterName = "@idMIRDetail",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idMIRDetail
                };
                cmd.Parameters.Add(parm2);

                SqlParameter parm3 = new SqlParameter
                {
                    ParameterName = "@idSerial",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idSerial
                };
                cmd.Parameters.Add(parm3);
                
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
