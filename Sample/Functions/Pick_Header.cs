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
    public class Pick_Header
    {
        public static List<Pick_Header_Model> RetrieveData(SqlConnection connection, string SO_Number, string pick_number)
        {
            StringBuilder sQuery = new StringBuilder();

            if (SO_Number != "")
            {
                sQuery.Append(@"DECLARE @idSOHead int = 0


                         SELECT @idSOHead = idSOHeader
                         FROM a_SO_Header
                         WHERE SO_Number = @SO_Number
");
            }

            sQuery.Append(@"
                         SELECT idPickHeader
                         ,idSOHeader
                         ,Pick_Number
                         ,CustPONum
                         ,OrderDate
                         ,DueDate
                         ,CustomerCode
                         FROM a_Pick_Header
                        ");

            if (SO_Number != "")
            {
                sQuery.Append("WHERE idSOHeader = @idSOHead");
            }

            if (pick_number != "")
            {
                sQuery.Append("WHERE Pick_Number = @pick_number");
            }

            var lmodel = new List<Pick_Header_Model>();

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

                if (pick_number != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@pick_number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = pick_number
                    };
                    cmd.Parameters.Add(parm2);
                }

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Pick_Header_Model oModel = new Pick_Header_Model
                    {
                        idPickHeader = (int)oreader["idPickHeader"],
                        idSOHeader = (int)oreader["idSOHeader"],
                        Pick_Number = (string)oreader["Pick_Number"],
                        CustPONum = (string)oreader["CustPONum"],
                        OrderDate = (DateTime)oreader["OrderDate"],
                        DueDate = (DateTime)oreader["DueDate"],
                        CustomerCode = (string)oreader["CustomerCode"]
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static int Save(SqlConnection connection, Pick_Header_Model model)
        {
            int returnValue = 0;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_Pick_Header
                             (idSOHeader
                             ,Pick_Number
                             ,OrderDate
                             ,DueDate
                             ,CustomerCode
                             ,CustPONum
,user_id_chg_by
,Pick_Date
                             )
                             VALUES
                             (@idSOHeader
                             ,@Pick_Number
                             ,@OrderDate
                             ,@DueDate
                             ,@CustomerCode
                             ,@CustPONum
,@user_id_chg_by
,@Pick_Date
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
                    ParameterName = "@idSOHeader",
                    SqlDbType = SqlDbType.Int,
                    Value = model.idSOHeader
                };
                cmd.Parameters.Add(parm2);

                SqlParameter parm3 = new SqlParameter
                {
                    ParameterName = "@Pick_Number",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Pick_Number
                };
                cmd.Parameters.Add(parm3);

                //SqlParameter parm4 = new SqlParameter
                //{
                //    ParameterName = "@idCustomer",
                //    SqlDbType = SqlDbType.Int,
                //    Value = model.idCustomer
                //};
                //cmd.Parameters.Add(parm4);

                SqlParameter parm5 = new SqlParameter();
                parm5.ParameterName = "@CustPONum";
                parm5.SqlDbType = SqlDbType.NVarChar;
                parm5.Value = model.CustPONum;
                cmd.Parameters.Add(parm5);

                SqlParameter parm6 = new SqlParameter
                {
                    ParameterName = "@OrderDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = model.OrderDate
                };
                cmd.Parameters.Add(parm6);

                SqlParameter parm7 = new SqlParameter
                {
                    ParameterName = "@DueDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = model.DueDate
                };
                cmd.Parameters.Add(parm7);

                SqlParameter parm8 = new SqlParameter
                {
                    ParameterName = "@CustomerCode",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.CustomerCode
                };
                cmd.Parameters.Add(parm8);

                SqlParameter parm9 = new SqlParameter
                {
                    ParameterName = "@user_id_chg_by",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.user_id_chg_by
                };
                cmd.Parameters.Add(parm9);

                SqlParameter parm10 = new SqlParameter
                {
                    ParameterName = "@Pick_Date",
                    SqlDbType = SqlDbType.DateTime,
                    Value = model.Pick_Date
                };
                cmd.Parameters.Add(parm10);
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
                    oreader.Close();
                    cmd.Dispose();
                    cmd.Parameters.Clear();
                    SQL_Transact.RollbackTransaction(connection, GUID);
                }
            }
            return returnValue;
        }

        public static bool Update(SqlConnection connection, Pick_Header_Model model)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"UPDATE a_Pick_Header SET
                             idSOHeader = @idSOHeader
                             ,Pick_Number = @Pick_Number
                             ,idCustomer = @idCustomer
                             ,OrderDate = @OrderDate
                             ,DueDate = @DueDate
                             ,CustomerCode = @CustomerCode     
,user_id_chg_by = @user_id_chg_by
,Pick_Date = @Pick_Date
                             WHERE idPickHeader = @idPickHeader ");
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
                        Value = model.idPickHeader
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@idSOHeader",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.idSOHeader
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Pick_Number",
                        SqlDbType = SqlDbType.Int,
                        Value = model.Pick_Number
                    };
                    cmd.Parameters.Add(parm3);

                    //SqlParameter parm4 = new SqlParameter
                    //{
                    //    ParameterName = "@idCustomer",
                    //    SqlDbType = SqlDbType.Int,
                    //    Value = model.idCustomer
                    //};
                    //cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter();
                    parm5.ParameterName = "@CustPONum";
                    parm5.SqlDbType = SqlDbType.NVarChar;
                    parm5.Value = model.CustPONum;
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@OrderDate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = model.OrderDate
                    };
                    cmd.Parameters.Add(parm6);

                    SqlParameter parm7 = new SqlParameter
                    {
                        ParameterName = "@DueDate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = model.DueDate
                    };
                    cmd.Parameters.Add(parm7);

                    SqlParameter parm8 = new SqlParameter
                    {
                        ParameterName = "@CustomerCode",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.CustomerCode
                    };
                    cmd.Parameters.Add(parm8);

                    SqlParameter parm9 = new SqlParameter
                    {
                        ParameterName = "@user_id_chg_by",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.user_id_chg_by
                    };
                    cmd.Parameters.Add(parm9);

                    SqlParameter parm10 = new SqlParameter
                    {
                        ParameterName = "@Pick_Date",
                        SqlDbType = SqlDbType.DateTime,
                        Value = model.Pick_Date
                    };
                    cmd.Parameters.Add(parm10);

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

        public static bool Delete(SqlConnection connection, int idPickHeader)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Pick_Header ");
            sQuery.Append("WHERE idPickHeader = @idPickHeader");

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
                        ParameterName = "@idPickHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = idPickHeader
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
