using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using POSOINV.Models;

namespace POSOINV.Functions
{
    public class SO_Creation
    {

        public static List<SO_Creation_Model> RetrieveData(SqlConnection connection)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idSODetail
                         ,idSOHeader
                         ,idItem
                         FROM a_SO_Detail");

            var lmodel = new List<SO_Creation_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    SO_Creation_Model oModel = new SO_Creation_Model
                    {
                        idSODetail = (int)oreader["idSODetail"],
                        idSOHeader = (int)oreader["idSOHeader"],
                        idItem = (int)oreader["idItem"]
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static List<SO_Creation_Model> RetrieveItemMaster(SqlConnection connection)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT a.idItem,a.idClass,b.Product_Name,a.Item_Number,a.Description
                          FROM a_Item_Master a INNER JOIN a_Item_Class b ON a.idClass = b.idClass");

            var lmodel = new List<SO_Creation_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    SO_Creation_Model oModel = new SO_Creation_Model
                    {
                        idItem = (int)oreader["idItem"],
                        idClass = (int)oreader["idClass"],
                        Product_Name = (string)oreader["Product_Name"],
                        ItemNumber = (string)oreader["Item_Number"],
                        Description = (string)oreader["Description"]
                    };

                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }
        
        public static List<SO_Creation_Model> RetrieveSO(SqlConnection connection, string SO_Number, string Salesman, string Access, string SOStatus)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT DISTINCT 
                            b.idSOHeader,
                            c.idCustomer,
                            b.SO_Number,
                            b.Order_Date,
                            b.Due_Date,
                            b.Salesman,
                            c.Customer_Name,
                            b.Customer_PO,
                            b.Freight_Charges,
                            b.Other_Charges,
                            b.idPOHeader,
                            c.Customer_Code,
                            b.credit_term,
                            b.Gross_Amount,
                            b.Net_Amount,
                            b.Tax_Amount,
                            b.idSite,
                            b.Remarks,
                            b.currency_code,
                            b.Final_Discount,
                            c.Address1,
                            c.Address2,
                            c.Address3,
                            c.Address4,
                            b.Special_Concession,
                            c.Company_Name,
                            b.Pick_Status,
                            b.SO_Status,
                            b.Stock_Status,
                            c.AddressShipping1,
                            c.AddressShipping2,
                            c.AddressShipping3,
                            c.AddressShipping4,
                            DATEDIFF(day, Order_Date, GETDATE()) AS DaysOpen,
                            b.CreatedBy
,End_User
,End_User_City
                            FROM a_SO_Header b 
                            INNER JOIN a_Customer_Details c ON b.idCustomer = c.idCustomer
                            WHERE b.idSOHeader != 0
                            ");

            if (Access == "BCC")
            {
                sQuery.Append(" AND b.SO_Number LIKE '%" + SO_Number + "%' AND b.Active != 'N' ");
            }
            else
            {
                sQuery.Append(" AND b.SO_Number LIKE '%" + SO_Number + "%' AND b.Salesman = (SELECT idUser FROM a_Users WHERE User_Name = '" + Salesman + "') AND b.Active != 'N' ");
            }

            if (SOStatus != "")
            {
                sQuery.Append(" AND b.SO_Status = '" + SOStatus + "' ");
            }

            sQuery.Append("ORDER BY b.SO_Number DESC");

            var lmodel = new List<SO_Creation_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;


                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    SO_Creation_Model oModel = new SO_Creation_Model
                    {
                        idSOHeader = (int)oreader["idSOHeader"],
                        idCustomer = (int)oreader["idCustomer"],
                        SO_Number = (string)oreader["SO_Number"],
                        Order_Date = DateTime.Parse(oreader["Order_Date"].ToString()).ToString("dd/MM/yyyy"),
                        Due_Date = DateTime.Parse(oreader["Due_Date"].ToString()).ToString("dd/MM/yyyy"),
                        Salesman = oreader["Salesman"].ToString(),
                        Customer_Name = oreader["Customer_Name"].ToString(),
                        Customer_PO = oreader["Customer_PO"].ToString(),
                        Freight_Charges = (decimal)oreader["Freight_Charges"],
                        Other_Charges = (decimal)oreader["Other_Charges"],
                        idPOHeader = oreader["idPOHeader"].ToString(),
                        Customer_Code = oreader["Customer_Code"].ToString(),
                        credit_term = oreader["credit_term"].ToString(),
                        Gross_Amount = (decimal)oreader["Gross_Amount"],
                        Net_Amount = (decimal)oreader["Net_Amount"],
                        Tax_Amount = (decimal)oreader["Tax_Amount"],
                        idSite = oreader["idSite"].ToString(),
                        Remarks = oreader["Remarks"].ToString(),
                        Currency_Code = oreader["Currency_Code"].ToString(),
                        Final_Discount = (decimal)oreader["Final_Discount"],
                        Address1 = oreader["Address1"].ToString(),
                        Address2 = oreader["Address2"].ToString(),
                        Address3 = oreader["Address3"].ToString(),
                        Address4 = oreader["Address4"].ToString(),
                        Special_Concession = oreader["Special_Concession"].ToString(),
                        Company_Name = oreader["Company_Name"].ToString(),
                        SO_Status = oreader["SO_Status"].ToString(),
                        Stock_Status = oreader["Stock_Status"].ToString(),
                        AddressShipping1 = oreader["AddressShipping1"].ToString(),
                        AddressShipping2 = oreader["AddressShipping2"].ToString(),
                        AddressShipping3 = oreader["AddressShipping3"].ToString(),
                        AddressShipping4 = oreader["AddressShipping4"].ToString(),
                        DaysOpen = (int)oreader["DaysOpen"],
                        CreatedBy = oreader["CreatedBy"].ToString(),
                        Pick_Status = oreader["Pick_Status"].ToString(),
                        End_User = oreader["End_User"].ToString(),
                        End_User_City = oreader["End_User_City"].ToString()

                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }
        
        public string GetSalesmanByID(int idUser, SqlConnection connection)
        {
            string Salesman = "";

            StringBuilder sQuery = new StringBuilder();


            sQuery.Append(@"SELECT User_Name
                         FROM a_Users 
                         WHERE idUser = @idUser");

            var lmodel = new List<Customer_Details_Model>();

            connection.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idUser",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = idUser
                    };
                    cmd.Parameters.Add(parm1);

                    var oreader = cmd.ExecuteReader();

                    while (oreader.Read())
                    {
                        Salesman = (string)oreader["User_Name"];
                    }
                    oreader.Close();
                    cmd.Dispose();
                }
            }
            catch
            {
                Salesman = "";
            }

            connection.Close();

            return Salesman;
        }

    }
}
