using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class Customer_Details
    {
        public static List<Customer_Details_Model> RetrieveData(SqlConnection connection, int idCustomer, string Customer_Code)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idCustomer
                         ,Customer_Code
                         ,Customer_Name
                         ,Address1
                         ,Address2
                         ,Address3
                         ,Address4
  ,AddressShipping1
,AddressShipping2
,AddressShipping3
,AddressShipping4
                         ,credit_term
                         ,Company_Name
                         FROM a_Customer_Details
WHERE idCustomer <> 0
                         ");

            if (idCustomer != 0)
            {
                sQuery.Append(" AND idCustomer = @idCustomer ");
            }

            if (Customer_Code.ToString() != "")
            {
                sQuery.Append(" AND Customer_Code = @Customer_Code ");
            }

            var lmodel = new List<Customer_Details_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (idCustomer.ToString() != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idCustomer",
                        SqlDbType = SqlDbType.Int,
                        Value = idCustomer
                    };
                    cmd.Parameters.Add(parm1);
                }

                if (Customer_Code.ToString() != "")
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@Customer_Code",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Customer_Code
                    };
                    cmd.Parameters.Add(parm1);
                }

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Customer_Details_Model oModel = new Customer_Details_Model
                    {
                        idCustomer = (int)oreader["idCustomer"],
                        Customer_Code = (string)oreader["Customer_Code"],
                        Customer_Name = (string)oreader["Customer_Name"],
                        Address1 = (string)oreader["Address1"],
                        Address2 = (string)oreader["Address2"],
                        Address3 = (string)oreader["Address3"],
                        Address4 = (string)oreader["Address4"],
                        credit_term = (string)oreader["credit_term"],
                        Company_Name = (string)oreader["Company_Name"],
                        AddressShipping1 = (string)oreader["AddressShipping1"],
                        AddressShipping2 = (string)oreader["AddressShipping2"],
                        AddressShipping3 = (string)oreader["AddressShipping3"],
                        AddressShipping4 = (string)oreader["AddressShipping4"]
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static bool Save(SqlConnection connection, Customer_Details_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_Customer_Details
                             (Customer_Code
                             ,Customer_Name
                             ,Address1
                             ,Address2
                             ,Address3
                             ,Address4
                             ,credit_term
                             ,Company_Name
							 ,Status
                             ,AddressShipping1
                             ,AddressShipping2
                             ,AddressShipping3
                             ,AddressShipping4
							 ,timestamp
							 ,user_chg_by
						 	,Customer_Type
							,Credit_Limit
							,Position
							,Contact_Number
                            ,TIN_Number
                            ,Salesman
                            ,Email_Address)
                             VALUES
                             (@Customer_Code
                             ,@Customer_Name
                             ,@Address1
                             ,@Address2
                             ,@Address3
                             ,@Address4
                             ,@credit_term
                             ,@Company_Name
							 ,@Status
                             ,@AddressShipping1
                             ,@AddressShipping2
                             ,@AddressShipping3
                             ,@AddressShipping4
							,GETDATE()
							,@user_chg_by
							,@Customer_Type
							,@Credit_Limit
							,@Position
							,@Contact_Number
                            ,@TIN_Number
                            ,@Salesman
                            ,@Email_Address)
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
                        ParameterName = "@Customer_Code",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Customer_Code
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Customer_Name",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Customer_Name
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@Address1",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Address1
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@Address2",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Address2
                    };
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@Address3",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Address3
                    };
                    cmd.Parameters.Add(parm6);

                    SqlParameter parm7 = new SqlParameter
                    {
                        ParameterName = "@Address4",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Address4
                    };
                    cmd.Parameters.Add(parm7);

                    SqlParameter parm8 = new SqlParameter
                    {
                        ParameterName = "@credit_term",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.credit_term
                    };
                    cmd.Parameters.Add(parm8);

                    SqlParameter parm9 = new SqlParameter
                    {
                        ParameterName = "@Company_Name",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Company_Name
                    };
                    cmd.Parameters.Add(parm9);

                    SqlParameter parm10 = new SqlParameter
                    {
                        ParameterName = "@Status",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Status
                    };
                    cmd.Parameters.Add(parm10);

                    SqlParameter parm11 = new SqlParameter
                    {
                        ParameterName = "@AddressShipping1",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.AddressShipping1
                    };
                    cmd.Parameters.Add(parm11);


                    SqlParameter parm12 = new SqlParameter
                    {
                        ParameterName = "@AddressShipping2",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.AddressShipping2
                    };
                    cmd.Parameters.Add(parm12);

                    SqlParameter parm13 = new SqlParameter
                    {
                        ParameterName = "@AddressShipping3",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.AddressShipping3
                    };
                    cmd.Parameters.Add(parm13);

                    SqlParameter parm14 = new SqlParameter
                    {
                        ParameterName = "@AddressShipping4",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.AddressShipping4
                    };
                    cmd.Parameters.Add(parm14);

                    SqlParameter parm15 = new SqlParameter
                    {
                        ParameterName = "@user_chg_by",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.user_chg_by
                    };
                    cmd.Parameters.Add(parm15);

                    SqlParameter parm16 = new SqlParameter
                    {
                        ParameterName = "@Customer_Type",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Customer_Type
                    };
                    cmd.Parameters.Add(parm16);

                    SqlParameter parm17 = new SqlParameter
                    {
                        ParameterName = "@Credit_Limit",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Credit_Limit
                    };
                    cmd.Parameters.Add(parm17);

                    SqlParameter parm18 = new SqlParameter
                    {
                        ParameterName = "@Position",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Position
                    };
                    cmd.Parameters.Add(parm18);

                    SqlParameter parm19 = new SqlParameter
                    {
                        ParameterName = "@Contact_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Contact_Number
                    };
                    cmd.Parameters.Add(parm19);

                    SqlParameter parm20 = new SqlParameter
                    {
                        ParameterName = "@TIN_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.TIN_Number
                    };
                    cmd.Parameters.Add(parm20);

                    SqlParameter parm21 = new SqlParameter
                    {
                        ParameterName = "@Salesman",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Salesman
                    };
                    cmd.Parameters.Add(parm21);

                    SqlParameter parm22 = new SqlParameter
                    {
                        ParameterName = "@Email_Address",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Email_Address
                    };
                    cmd.Parameters.Add(parm22);

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

        public static bool Update(SqlConnection connection, Customer_Details_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"UPDATE a_Customer_Details SET
                             Customer_Code = @Customer_Code
                             ,Customer_Name = @Customer_Name
                             ,Address1 = @Address1
                             ,Address2 = @Address2
                             ,Address3 = @Address3
                             ,Address4 = @Address4
							 ,credit_term = @credit_term
							 ,Company_Name = @Company_Name
							 ,Status = @Status
                             ,AddressShipping1 = @AddressShipping1
                             ,AddressShipping2 = @AddressShipping2
                             ,AddressShipping3 = @AddressShipping3
                             ,AddressShipping4 = @AddressShipping4
							,user_chg_by = @user_chg_by
							,Customer_Type = @Customer_Type
							,Credit_Limit = @Credit_Limit
							,Position = @Position
							,Contact_Number = @Contact_Number
                            ,TIN_Number = @TIN_Number
                            ,Salesman = @Salesman
                            ,Email_Address = @Email_Address
                             WHERE idCustomer = @idCustomer ");

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idCustomer",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idCustomer
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@Customer_Code",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Customer_Code
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Customer_Name",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Customer_Name
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@Address1",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Address1
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@Address2",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Address2
                    };
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@Address3",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Address3
                    };
                    cmd.Parameters.Add(parm6);

                    SqlParameter parm7 = new SqlParameter
                    {
                        ParameterName = "@Address4",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Address4
                    };
                    cmd.Parameters.Add(parm7);

                    SqlParameter parm8 = new SqlParameter
                    {
                        ParameterName = "@credit_term",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.credit_term
                    };
                    cmd.Parameters.Add(parm8);

                    SqlParameter parm9 = new SqlParameter
                    {
                        ParameterName = "@Company_Name",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Company_Name
                    };
                    cmd.Parameters.Add(parm9);

                    SqlParameter parm10 = new SqlParameter
                    {
                        ParameterName = "@Status",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Status
                    };
                    cmd.Parameters.Add(parm10);

                    SqlParameter parm11 = new SqlParameter
                    {
                        ParameterName = "@AddressShipping1",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.AddressShipping1
                    };
                    cmd.Parameters.Add(parm11);

                    SqlParameter parm12 = new SqlParameter
                    {
                        ParameterName = "@AddressShipping2",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.AddressShipping2
                    };
                    cmd.Parameters.Add(parm12);

                    SqlParameter parm13 = new SqlParameter
                    {
                        ParameterName = "@AddressShipping3",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.AddressShipping3
                    };
                    cmd.Parameters.Add(parm13);

                    SqlParameter parm14 = new SqlParameter
                    {
                        ParameterName = "@AddressShipping4",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.AddressShipping4
                    };
                    cmd.Parameters.Add(parm14);

                    SqlParameter parm15 = new SqlParameter
                    {
                        ParameterName = "@user_chg_by",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.user_chg_by
                    };
                    cmd.Parameters.Add(parm15);

                    SqlParameter parm16 = new SqlParameter
                    {
                        ParameterName = "@Customer_Type",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Customer_Type
                    };
                    cmd.Parameters.Add(parm16);

                    SqlParameter parm17 = new SqlParameter
                    {
                        ParameterName = "@Credit_Limit",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Credit_Limit
                    };
                    cmd.Parameters.Add(parm17);

                    SqlParameter parm18 = new SqlParameter
                    {
                        ParameterName = "@Position",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Position
                    };
                    cmd.Parameters.Add(parm18);

                    SqlParameter parm19 = new SqlParameter
                    {
                        ParameterName = "@Contact_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Contact_Number
                    };
                    cmd.Parameters.Add(parm19);

                    SqlParameter parm20 = new SqlParameter
                    {
                        ParameterName = "@TIN_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.TIN_Number
                    };
                    cmd.Parameters.Add(parm20);

                    SqlParameter parm21 = new SqlParameter
                    {
                        ParameterName = "@Salesman",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Salesman
                    };
                    cmd.Parameters.Add(parm21);

                    SqlParameter parm22 = new SqlParameter
                    {
                        ParameterName = "@Email_Address",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.Email_Address
                    };
                    cmd.Parameters.Add(parm22);

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

        public static bool Delete(SqlConnection connection, int idCustomer)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Customer_Details ");
            sQuery.Append("WHERE idCustomer = @idCustomer");

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
                        ParameterName = "@idCustomer",
                        SqlDbType = SqlDbType.Int,
                        Value = idCustomer
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

        public static string GetLastCustomerCode(SqlConnection connection)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("SELECT TOP 1 Customer_Code FROM a_Customer_Details ");
            sQuery.Append("ORDER BY Customer_Code DESC");

            string resultValue = null;
            connection.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    resultValue = oreader["Customer_Code"].ToString();
                }
                oreader.Close();
                cmd.Dispose();
                cmd.Parameters.Clear();
            }
            connection.Close();
            return resultValue;
        }
    }
}