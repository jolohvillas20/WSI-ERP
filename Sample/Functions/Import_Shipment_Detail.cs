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
    public class Import_Shipment_Detail
    {
        public static List<Import_Shipment_Detail_Model> RetrieveData(SqlConnection connection, int idImpShpHead, int idPOHeader)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idImpShpDet
                         ,idImpShpHead
                         ,idPOHeader
                         ,POCharge
                         FROM a_Import_Shipment_Detail
                         WHERE idImpShpHead = @idImpShpHead AND idPOHeader = @idPOHeader
                         ");

            var lmodel = new List<Import_Shipment_Detail_Model>();

            DataTable dataTable = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                //if (impshphead != 0)
                //{
                SqlParameter parm1 = new SqlParameter
                {
                    ParameterName = "@idImpShpHead",
                    SqlDbType = SqlDbType.Int,
                    Value = idImpShpHead
                };
                cmd.Parameters.Add(parm1);
                //}                     

                SqlParameter parm2 = new SqlParameter
                {
                    ParameterName = "@idPOHeader",
                    SqlDbType = SqlDbType.Int,
                    Value = idPOHeader
                };
                cmd.Parameters.Add(parm2);

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Import_Shipment_Detail_Model oModel = new Import_Shipment_Detail_Model
                    {
                        idImpShpDet = (int)oreader["idImpShpDet"],
                        idImpShpHead = (int)oreader["idImpShpHead"],
                        idPOHeader = (int)oreader["idPOHeader"],
                        POCharge = (decimal)oreader["POCharge"]
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static DataTable RetrieveData(SqlConnection connection, int idImpShpHead)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT a.idPOHeader
                         ,b.PO_Number
                         ,a.POCharge
                         FROM a_Import_Shipment_Detail as a
INNER JOIN a_PO_Header as b 
ON a.idPOHeader = b.idPOHeader
                         WHERE a.idImpShpHead = @idImpShpHead
                         ");

            DataTable dataTable = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter parm1 = new SqlParameter
                {
                    ParameterName = "@idImpShpHead",
                    SqlDbType = SqlDbType.Int,
                    Value = idImpShpHead
                };
                cmd.Parameters.Add(parm1);

                dataTable.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return dataTable;
        }


        public static bool Save(SqlConnection connection, Import_Shipment_Detail_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_Import_Shipment_Detail
                            (idImpShpHead
                            ,idPOHeader
                            ,POCharge)
                            VALUES
                            (@idImpShpHead
                            ,@idPOHeader
                            ,@POCharge)
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
                        ParameterName = "@idImpShpHead",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idImpShpHead
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@idPOHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idPOHeader
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@POCharge",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.POCharge
                    };
                    cmd.Parameters.Add(parm4);

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

        public static bool Update(SqlConnection connection, Import_Shipment_Detail_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"UPDATE a_Import_Shipment_Detail SET
                              idImpShpHead = @idImpShpHead
                            ,idPOHeader = @idPOHeader
                            ,POCharge = @POCharge
                             WHERE idImpShpDet = @idImpShpDet ");


            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idImpShpDet",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idImpShpDet
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idImpShpHead",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idImpShpHead
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@idPOHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idPOHeader
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@POCharge",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.POCharge
                    };
                    cmd.Parameters.Add(parm4);

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

        public static bool Delete(SqlConnection connection, int idImpShpHead)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Import_Shipment_Detail ");
            sQuery.Append("WHERE idImpShpHead = @idImpShpHead");

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
                        ParameterName = "@idImpShpHead",
                        SqlDbType = SqlDbType.Int,
                        Value = idImpShpHead
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
