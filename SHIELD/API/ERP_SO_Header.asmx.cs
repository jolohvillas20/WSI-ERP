using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace SOPOINV.API
{
    /// <summary>
    /// Summary description for ERP_SO_Header
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ERP_SO_Header : System.Web.Services.WebService
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);
        
        [WebMethod()]
        public int Save_SO_Header(
            string SO_Number,
            int idPOHeader,
            DateTime Order_Date,
            DateTime Due_Date,
            string ERPCustomerCode,
            string Customer_PO,
            string Salesman,
            int Ship_Code,
            decimal Gross_Amount,
            decimal Final_Discount,
            decimal Freight_Charges,
            decimal Other_Charges,
            decimal Net_Amount,
            decimal Tax_Amount,
            //string credit_term,
            int idSite,
            string Remarks,
            string currency_code,
            string Pick_Status,
            string Special_Concession,
            string transaction_ID,
            string SO_Status,
            string Stock_Status,
            string CreatedBy,
            string End_User,
            string End_User_City
            )
        {
            int returnValue = 0;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append(@"INSERT INTO a_SO_Header
                             (SO_Number
                             ,idPOHeader
                             ,idCustomer
                             ,Due_Date
,Order_Date
                             ,Customer_PO
                             ,Salesman
                             ,Ship_Code
                             ,Gross_Amount
                             ,Final_Discount
                             ,Freight_Charges
                             ,Other_Charges
                             ,Net_Amount
                             ,Tax_Amount
                             ,credit_term
							 ,Remarks
							 ,idSite
							 ,currency_code
                             ,Pick_Status
							 ,Special_Concession
                             ,transaction_ID
                             ,SO_Status
							 ,Stock_Status
                             ,CreatedBy
,End_User
,End_User_City)
                             VALUES
                             (@SO_Number
                             ,@idPOHeader
                             ,(SELECT idCustomer FROM a_Customer_Details WHERE Customer_Code = @ERPCustomerCode)
,@Order_Date
                             ,@Due_Date
                             ,@Customer_PO
                             ,(SELECT idUser FROM a_Users WHERE User_Name = @Salesman)
                             ,@Ship_Code
                             ,@Gross_Amount
                             ,@Final_Discount
                             ,@Freight_Charges
                             ,@Other_Charges
                             ,@Net_Amount
                             ,@Tax_Amount
                             ,(SELECT credit_term FROM a_Customer_Details WHERE Customer_Code = @ERPCustomerCode)
							 ,@Remarks
							 ,@idSite
							 ,@currency_code
                             ,'N'
							 ,@Special_Concession
                             ,@transaction_ID
                             ,@SO_Status
							 ,@Stock_Status
                             ,@CreatedBy
,@End_User
,@End_User_City)

SELECT SCOPE_IDENTITY() as 'ID'

");
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@SO_Number",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = SO_Number
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@idPOHeader",
                        SqlDbType = SqlDbType.Int,
                        Value = idPOHeader
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@ERPCustomerCode",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = ERPCustomerCode
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter();
                    parm5.ParameterName = "@Order_Date";
                    parm5.SqlDbType = SqlDbType.NVarChar;
                    parm5.Value = Order_Date;
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@Due_Date",
                        SqlDbType = SqlDbType.DateTime,
                        Value = Due_Date
                    };
                    cmd.Parameters.Add(parm6);

                    SqlParameter parm7 = new SqlParameter
                    {
                        ParameterName = "@Customer_PO",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Customer_PO
                    };
                    cmd.Parameters.Add(parm7);

                    SqlParameter parm8 = new SqlParameter
                    {
                        ParameterName = "@Salesman",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Salesman
                    };
                    cmd.Parameters.Add(parm8);

                    SqlParameter parm9 = new SqlParameter
                    {
                        ParameterName = "@Ship_Code",
                        SqlDbType = SqlDbType.Int,
                        Value = Ship_Code
                    };
                    cmd.Parameters.Add(parm9);

                    SqlParameter parm10 = new SqlParameter
                    {
                        ParameterName = "@Gross_Amount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = Gross_Amount
                    };
                    cmd.Parameters.Add(parm10);

                    SqlParameter parm11 = new SqlParameter
                    {
                        ParameterName = "@Final_Discount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = Final_Discount
                    };
                    cmd.Parameters.Add(parm11);

                    SqlParameter parm12 = new SqlParameter
                    {
                        ParameterName = "@Freight_Charges",
                        SqlDbType = SqlDbType.Decimal,
                        Value = Freight_Charges
                    };
                    cmd.Parameters.Add(parm12);

                    SqlParameter parm13 = new SqlParameter
                    {
                        ParameterName = "@Other_Charges",
                        SqlDbType = SqlDbType.Decimal,
                        Value = Other_Charges
                    };
                    cmd.Parameters.Add(parm13);

                    SqlParameter parm14 = new SqlParameter
                    {
                        ParameterName = "@Net_Amount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = Net_Amount
                    };
                    cmd.Parameters.Add(parm14);

                    SqlParameter parm15 = new SqlParameter
                    {
                        ParameterName = "@Tax_Amount",
                        SqlDbType = SqlDbType.Decimal,
                        Value = Tax_Amount
                    };
                    cmd.Parameters.Add(parm15);

                    //SqlParameter parm16 = new SqlParameter
                    //{
                    //    ParameterName = "@credit_term",
                    //    SqlDbType = SqlDbType.NVarChar,
                    //    Value = credit_term
                    //};
                    //cmd.Parameters.Add(parm16);

                    SqlParameter parm17 = new SqlParameter
                    {
                        ParameterName = "@Remarks",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Remarks
                    };
                    cmd.Parameters.Add(parm17);

                    SqlParameter parm18 = new SqlParameter
                    {
                        ParameterName = "@idSite",
                        SqlDbType = SqlDbType.Int,
                        Value = idSite
                    };
                    cmd.Parameters.Add(parm18);

                    SqlParameter parm19 = new SqlParameter
                    {
                        ParameterName = "@currency_code",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = currency_code
                    };
                    cmd.Parameters.Add(parm19);

                    SqlParameter parm20 = new SqlParameter
                    {
                        ParameterName = "@Special_Concession",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Special_Concession
                    };
                    cmd.Parameters.Add(parm20);

                    SqlParameter parm21 = new SqlParameter
                    {
                        ParameterName = "@transaction_ID",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = transaction_ID
                    };
                    cmd.Parameters.Add(parm21);

                    SqlParameter parm22 = new SqlParameter
                    {
                        ParameterName = "@SO_Status",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = SO_Status
                    };
                    cmd.Parameters.Add(parm22);

                    SqlParameter parm23 = new SqlParameter
                    {
                        ParameterName = "@Stock_Status",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = Stock_Status
                    };
                    cmd.Parameters.Add(parm23);

                    SqlParameter parm24 = new SqlParameter
                    {
                        ParameterName = "@CreatedBy",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = CreatedBy
                    };
                    cmd.Parameters.Add(parm24);

                    SqlParameter parm25 = new SqlParameter
                    {
                        ParameterName = "@End_User",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = End_User
                    };
                    cmd.Parameters.Add(parm25);

                    SqlParameter parm26 = new SqlParameter
                    {
                        ParameterName = "@End_User_City",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = End_User_City
                    };
                    cmd.Parameters.Add(parm26);

                    var oreader = cmd.ExecuteReader();

                    while (oreader.Read())
                    {
                        returnValue = Convert.ToInt32(oreader["ID"].ToString());
                    }

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

        [WebMethod()]
        public string GetLastSONumber()
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("SELECT TOP 1 SO_Number FROM a_SO_Header ");
            sQuery.Append("ORDER BY SO_Number DESC");

            string resultValue = null;

            connection.Open();
            try
            {              
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;
                                        
                    var oreader = cmd.ExecuteReader();

                    while (oreader.Read())
                    {
                        resultValue = oreader["SO_Number"].ToString();
                    }

                    cmd.Dispose();
                }           
            }
            catch
            {
                
            }
            finally
            {
                connection.Close();
            }

            return resultValue;
        }
    }
}
