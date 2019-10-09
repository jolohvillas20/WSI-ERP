using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;



namespace SHIELD.ERP
{

    public class clsCustomerMaintenance
    {
        public string strConstr = System.Configuration.ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString.ToString();
        // public string strConstr = "Password=bl@ckb3rry;User ID=Test_User;Initial Catalog=WSIInventoryV2;Data Source=wsimkt-dt656; Persist Security Info=true; Connect Timeout=1000";
        SqlConnection oConnection;
        SqlCommand oCommand;

        public string Customer_Type { get; set; }

        public string idCustomer { get; set; }
        public string Customer_Code { get; set; }
        public string Customer_Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string FullAddress { get; set; }
        public string credit_term { get; set; }
        public string Company_Name { get; set; }
        public string Status { get; set; }
        public string AddressShipping1 { get; set; }
        public string AddressShipping2 { get; set; }
        public string AddressShipping3 { get; set; }
        public string AddressShipping4 { get; set; }
        public string Credit_Limit { get; set; }
        public string Position { get; set; }
        public string Contact_Number { get; set; }
        public string TIN_Number { get; set; }
        public string Salesman { get; set; }
        public string Email_Address { get; set; }

        public List<clsCustomerMaintenance> RetrieveCustomerDetails(string searchby, string searchname)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("SELECT idCustomer, Customer_Code, Customer_Name, Address1, Address2, Address3, ");
            sQuery.Append("Address4, credit_term, Company_Name, Status, AddressShipping1, AddressShipping2, ");
            sQuery.Append("AddressShipping3, AddressShipping4, Customer_Type, Credit_Limit, ");
            sQuery.Append("Position, Contact_Number, TIN_Number, Salesman, Email_Address FROM a_Customer_Details ");

            if (searchby == "Customer Code")
            {
                sQuery.Append("WHERE Customer_Code LIKE '%" + searchname + "%' ");
            }
            else if (searchby == "Customer Name")
            {
                sQuery.Append("WHERE Customer_Name LIKE '%" + searchname + "%' ");
            }
            else if (searchby == "Company Name")
            {
                sQuery.Append("WHERE Company_Name LIKE '%" + searchname + "%' ");
            }

            sQuery.Append("ORDER BY Customer_Code ASC");

            List<clsCustomerMaintenance> lData = new List<clsCustomerMaintenance>();
            try
            {
                var oreader = execReader(sQuery.ToString());

                while (oreader.Read())
                {
                    clsCustomerMaintenance obj = new clsCustomerMaintenance();

                    obj.idCustomer = oreader["idCustomer"].ToString();
                    obj.Customer_Code = oreader["Customer_Code"].ToString();
                    obj.Customer_Name = oreader["Customer_Name"].ToString();
                    obj.Address1 = oreader["Address1"].ToString();
                    obj.Address2 = oreader["Address2"].ToString();
                    obj.Address3 = oreader["Address3"].ToString();
                    obj.Address4 = oreader["Address4"].ToString();
                    obj.FullAddress = oreader["Address1"].ToString() + ", " + oreader["Address2"].ToString();
                    obj.credit_term = oreader["credit_term"].ToString();
                    obj.Company_Name = oreader["Company_Name"].ToString();
                    obj.Status = oreader["Status"].ToString();
                    obj.AddressShipping1 = oreader["AddressShipping1"].ToString();
                    obj.AddressShipping2 = oreader["AddressShipping2"].ToString();
                    obj.AddressShipping3 = oreader["AddressShipping3"].ToString();
                    obj.AddressShipping4 = oreader["AddressShipping4"].ToString();
                    obj.Customer_Type = oreader["Customer_Type"].ToString();
                    obj.Credit_Limit = oreader["Credit_Limit"].ToString();
                    obj.Position = oreader["Position"].ToString();
                    obj.Contact_Number = oreader["Contact_Number"].ToString();
                    obj.TIN_Number = oreader["TIN_Number"].ToString();
                    obj.Salesman = oreader["Salesman"].ToString();
                    obj.Email_Address = oreader["Email_Address"].ToString();
                    lData.Add(obj);

                }
            }
            catch (Exception ex)
            {
                //message
            }
            finally
            {
                CloseNewConnection();
            }
            return lData;

        }
        
        public SqlDataReader execReader(string sQuery)
        {
            SqlDataReader oReader = null;
            oConnection = new SqlConnection(strConstr);

            oCommand = new SqlCommand(sQuery, oConnection);

            try
            {
                OpenNewConnection();
                oReader = oCommand.ExecuteReader();
            }
            catch (Exception ex)
            {

            }
            return oReader;
        }
        public void OpenNewConnection()
        {
            oConnection.Open();
        }
        public void CloseNewConnection()
        {
            oConnection.Close();
        }
    }
}