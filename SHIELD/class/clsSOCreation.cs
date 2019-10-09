using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace SHIELD.ERP
{

    public class clsSOCreation
    {
        //private string strConStr = ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString.ToString();
        //public string strConstr = "Password=bl@ckb3rry;User ID=Test_User;Initial Catalog=WSIInventoryV2;Data Source=wsimkt-dt656; Persist Security Info=true; Connect Timeout=1000";
        //public string strConstr = @"Server= DESKTOP-AMDNFRS\SQLEXPRESS; Initial Catalog= WSIInventoryV2; Integrated Security=True;";
        SqlConnection oConnection;
        SqlCommand oCommand;
        public string strConstr = ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString.ToString();

        public string idSODetail { get; set; }
        public string idSOHeader { get; set; }
        public string idCustomer { get; set; }
        public string idItem { get; set; }
        public string SO_Number { get; set; }
        public string Order_Date { get; set; }
        public string Due_Date { get; set; }
        public string Salesman { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Code { get; set; }
        public string idClass { get; set; }
        public string Product_Name { get; set; }

        public string idPOHeader { get; set; }
        public string PO_Number { get; set; }

        public string Item_Number { get; set; }
        public string Description { get; set; }
        public string Customer_PO { get; set; }
        public string Freight_Charges { get; set; }
        public string Other_Charges { get; set; }
        public string Final_Discount { get; set; }

        public string Qty { get; set; }
        public string Cost { get; set; }
        public string UM { get; set; }
        public string Discount { get; set; }
        public string Tax { get; set; }
        public string Amount { get; set; }
        public string Item_Status { get; set; }
        public string credit_term { get; set; }
        public string Gross_Amount { get; set; }
        public string Net_Amount { get; set; }
        public string Tax_Amount { get; set; }
        public string idSite { get; set; }
        public string Remarks { get; set; }
        public string Currency_Code { get; set; }
        public string Currency_Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }

        public string AddressShipping1 { get; set; }
        public string AddressShipping2 { get; set; }
        public string AddressShipping3 { get; set; }
        public string AddressShipping4 { get; set; }
        public string Special_Concession { get; set; }
        public string Company_Name { get; set; }
        public DataTable RetrieveItemMaster(string searchby, string searchname, string site)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT a.idItem,a.idClass,b.Product_Name,a.Item_Number,a.Description,a.UM,C.Site_Name, a.OnHand_Qty 
FROM a_Item_Master a INNER JOIN a_Item_Class b ON a.idClass = b.idClass INNER JOIN a_Site as c ON a.Site = c.idSite 
WHERE a.idItem <> 0 ");

            if (searchby == "Product Name")
            {
                sQuery.Append("AND b.Product_Name lIKE '%" + searchname + "%' ");
            }
            else if (searchby == "Item Number")
            {
                sQuery.Append("AND a.Item_Number lIKE '%" + searchname + "%' ");
            }
            if (site != "")
                sQuery.Append(" AND a.Site = " + site );
            
            sQuery.Append(" ORDER BY b.Product_Name ASC");
            //List<clsSOCreation> lData = new List<clsSOCreation>();

            DataTable dt = new DataTable();
            try
            {
                dt.Load(execReader(sQuery.ToString()));

                //var oreader = execReader(sQuery.ToString());

                //while (oreader.Read())
                //{
                //    clsSOCreation obj = new clsSOCreation();

                //    obj.idItem = oreader["idItem"].ToString();
                //    obj.idClass = oreader["idClass"].ToString();
                //    obj.Product_Name = oreader["Product_Name"].ToString();
                //    obj.Item_Number = oreader["Item_Number"].ToString();
                //    obj.Description = oreader["Description"].ToString();
                //    obj.UM = oreader["UM"].ToString();
                //    lData.Add(obj);
                //}
            }
            catch (Exception ex)
            {
                //message
            }
            finally
            {
                CloseNewConnection();
            }
            return dt;
        }
        public List<clsSOCreation> RetrieveCustomerCodeList(string searchby, string searchname)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idCustomer,Customer_Code,Customer_Name,Company_Name,Address1,Address2,Address3,Address4, 
                AddressShipping1,AddressShipping2,AddressShipping3,AddressShipping4,credit_term FROM a_Customer_Details 
                WHERE Status = 'Active' ");

            if (searchby == "Company Name")
            {
                sQuery.Append("AND Company_Name lIKE '%" + searchname + "%' ");
            }
            else if (searchby == "Customer Code")
            {
                sQuery.Append("AND Customer_Code lIKE '%" + searchname + "%' ");
            }

            sQuery.Append("ORDER BY Customer_Code ASC");

            List<clsSOCreation> lData = new List<clsSOCreation>();
            try
            {
                var oreader = execReader(sQuery.ToString());

                while (oreader.Read())
                {
                    clsSOCreation obj = new clsSOCreation();

                    obj.idCustomer = oreader["idCustomer"].ToString();
                    obj.Customer_Code = oreader["Customer_Code"].ToString();
                    obj.Customer_Name = oreader["Customer_Name"].ToString();
                    obj.Company_Name = oreader["Company_Name"].ToString();
                    obj.Address1 = oreader["Address1"].ToString();
                    obj.Address2 = oreader["Address2"].ToString();
                    obj.Address3 = oreader["Address3"].ToString();
                    obj.Address4 = oreader["Address4"].ToString();
                    obj.AddressShipping1 = oreader["AddressShipping1"].ToString();
                    obj.AddressShipping2 = oreader["AddressShipping2"].ToString();
                    obj.AddressShipping3 = oreader["AddressShipping3"].ToString();
                    obj.AddressShipping4 = oreader["AddressShipping4"].ToString();
                    obj.credit_term = oreader["credit_term"].ToString();
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
        public List<clsSOCreation> RetrieveCreditTerm()
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("SELECT credit_term FROM a_Credit_Term");

            List<clsSOCreation> lData = new List<clsSOCreation>();
            try
            {
                var oreader = execReader(sQuery.ToString());

                while (oreader.Read())
                {
                    clsSOCreation obj = new clsSOCreation();

                    obj.credit_term = oreader["credit_term"].ToString();

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
        //public List<clsSOCreation> RetrieveCurrency()
        //{
        //    StringBuilder sQuery = new StringBuilder();

        //    sQuery.Append("SELECT Currency_Code,Currency_Name FROM a_Currency_Codes ");

        //    List<clsSOCreation> lData = new List<clsSOCreation>();
        //    try
        //    {
        //        var oreader = execReader(sQuery.ToString());

        //        while (oreader.Read())
        //        {
        //            clsSOCreation obj = new clsSOCreation();

        //            obj.Currency_Code = oreader["Currency_Code"].ToString();
        //            obj.Currency_Name = oreader["Currency_Name"].ToString();

        //            lData.Add(obj);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //message
        //    }
        //    finally
        //    {
        //        CloseNewConnection();
        //    }
        //    return lData;
        //}
        //public List<clsSOCreation> RetrievePOHeader()
        //{
        //    StringBuilder sQuery = new StringBuilder();

        //    sQuery.Append("SELECT idPOHeader,PO_Number FROM a_PO_Header");

        //    List<clsSOCreation> lData = new List<clsSOCreation>();
        //    try
        //    {
        //        var oreader = execReader(sQuery.ToString());

        //        while (oreader.Read())
        //        {
        //            clsSOCreation obj = new clsSOCreation();

        //            obj.idPOHeader = oreader["idPOHeader"].ToString();
        //            obj.PO_Number = oreader["PO_Number"].ToString();

        //            lData.Add(obj);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //message
        //    }
        //    finally
        //    {
        //        CloseNewConnection();
        //    }
        //    return lData;
        //}
        public List<clsSOCreation> RetrieveItemsByidSOHeader(int idSOHeader)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("SELECT b.idSODetail,b.idItem,c.Item_Number,c.Description,b.Qty,b.Cost,b.UM, ");
            sQuery.Append("b.Discount,b.Tax_Amount,b.Amount,b.Item_Status FROM a_SO_Header a ");
            sQuery.Append("INNER JOIN a_SO_Detail b ON a.idSOHeader = b.idSOHeader ");
            sQuery.Append("INNER JOIN a_Item_Master c ON b.idItem = c.idItem ");
            sQuery.Append("INNER JOIN a_Item_Class d ON c.idClass = d.idClass ");
            sQuery.Append("WHERE a.idSOHeader ='" + idSOHeader + "'");

            List<clsSOCreation> lData = new List<clsSOCreation>();
            try
            {
                var oreader = execReader(sQuery.ToString());

                while (oreader.Read())
                {
                    clsSOCreation obj = new clsSOCreation();

                    obj.idItem = oreader["idItem"].ToString();
                    obj.Item_Number = oreader["Item_Number"].ToString();
                    obj.Description = oreader["Description"].ToString();
                    obj.Qty = oreader["Qty"].ToString();
                    obj.Cost = double.Parse(oreader["Cost"].ToString()).ToString("n", CultureInfo.GetCultureInfo("en-US"));
                    obj.UM = oreader["UM"].ToString();
                    obj.Discount = double.Parse(oreader["Discount"].ToString()).ToString("n", CultureInfo.GetCultureInfo("en-US"));
                    obj.Tax_Amount = double.Parse(oreader["Tax_Amount"].ToString()).ToString("n", CultureInfo.GetCultureInfo("en-US"));
                    obj.Amount = double.Parse(oreader["Amount"].ToString()).ToString("n", CultureInfo.GetCultureInfo("en-US"));
                    obj.Item_Status = oreader["Item_Status"].ToString();
                    obj.idSODetail = oreader["idSODetail"].ToString();

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


        public int GetTotalQty(int idItem)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("SELECT Total_Qty FROM a_Item_Master ");
            sQuery.Append("WHERE idItem = '" + idItem + "'");

            int resultValue = 0;

            try
            {
                var oreader = execReader(sQuery.ToString());

                while (oreader.Read())
                    resultValue = (int)oreader["Total_Qty"];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                CloseNewConnection();
            }

            return resultValue;
        }
        public decimal GetTaxFromDB()
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("SELECT TOP 1 Tax FROM a_Item_Tax ");
            sQuery.Append("ORDER BY idTax DESC");

            decimal resultValue = 0;

            try
            {
                var oreader = execReader(sQuery.ToString());

                while (oreader.Read())
                    resultValue = (decimal)oreader["Tax"];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                CloseNewConnection();
            }

            return resultValue;
        }
        //public string GetPONumberByID(int idSOHeader)
        //{
        //    StringBuilder sQuery = new StringBuilder();

        //    sQuery.Append("SELECT PO_Number FROM a_PO_Header a ");
        //    sQuery.Append("INNER JOIN a_SO_Header b ON a.idPOHeader = b.idPOHeader  ");
        //    sQuery.Append("WHERE b.idSOHeader ='" + idSOHeader + "'");

        //    string resultValue = null;

        //    try
        //    {
        //        var oreader = execReader(sQuery.ToString());

        //        while (oreader.Read())
        //            resultValue = (string)oreader["PO_Number"];
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    finally
        //    {
        //        CloseNewConnection();
        //    }

        //    return resultValue;
        //}
        //public string GetSiteDescByID(int idSite)
        //{
        //    StringBuilder sQuery = new StringBuilder();

        //    sQuery.Append("SELECT Site_Desc FROM a_Site ");
        //    sQuery.Append("WHERE idSite ='" + idSite + "'");

        //    string resultValue = null;

        //    try
        //    {
        //        var oreader = execReader(sQuery.ToString());

        //        while (oreader.Read())
        //            resultValue = (string)oreader["Site_Desc"];
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    finally
        //    {
        //        CloseNewConnection();
        //    }

        //    return resultValue;
        //}
        public int GetTotalQtyByidItem(int idItem)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("SELECT Total_Qty FROM a_Item_Master ");
            sQuery.Append("WHERE idItem ='" + idItem + "'");

            int resultValue = 0;

            try
            {
                var oreader = execReader(sQuery.ToString());

                while (oreader.Read())
                    resultValue = (int)oreader["Total_Qty"];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                CloseNewConnection();
            }

            return resultValue;
        }
        //public int GetAllocQtyByidItem(int idItem)
        //{
        //    StringBuilder sQuery = new StringBuilder();

        //    sQuery.Append("SELECT Alloc_Qty FROM a_Item_Master ");
        //    sQuery.Append("WHERE idItem ='" + idItem + "'");

        //    int resultValue = 0;

        //    try
        //    {
        //        var oreader = execReader(sQuery.ToString());

        //        while (oreader.Read())
        //            resultValue = (int)oreader["Alloc_Qty"];
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    finally
        //    {
        //        CloseNewConnection();
        //    }

        //    return resultValue;
        //}
        //public int CountCurrentItem(int idSOHeader)
        //{
        //    StringBuilder sQuery = new StringBuilder();

        //    sQuery.Append("SELECT COUNT(b.idItem) AS idItem FROM a_SO_Header a ");
        //    sQuery.Append("INNER JOIN a_SO_Detail b ON a.idSOHeader = b.idSOHeader ");
        //    sQuery.Append("WHERE a.idSOHeader ='" + idSOHeader + "'");

        //    int resultValue = 0;

        //    try
        //    {
        //        var oreader = execReader(sQuery.ToString());

        //        while (oreader.Read())
        //            resultValue = (int)oreader["idItem"];
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    finally
        //    {
        //        CloseNewConnection();
        //    }

        //    return resultValue;
        //}
        //public DataTable RetrieveItemsToPrint(int idSOHeader)
        //{
        //    StringBuilder sQuery = new StringBuilder();
        //    DataTable dt = new DataTable();

        //    sQuery.Append("SELECT c.Item_Number,c.Description,b.UM,b.Qty,b.Cost,b.Discount,b.Tax_Amount,b.Amount FROM a_SO_Header a ");
        //    sQuery.Append("INNER JOIN a_SO_Detail b ON a.idSOHeader = b.idSOHeader ");
        //    sQuery.Append("INNER JOIN a_Item_Master c ON b.idItem = c.idItem ");
        //    sQuery.Append("INNER JOIN a_Item_Class d ON c.idClass = d.idClass ");
        //    sQuery.Append("WHERE a.idSOHeader ='" + idSOHeader + "'");
        //    dt = execDT(sQuery.ToString());

        //    return dt;
        //}


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
        public DataTable execDT(string sQuery)
        {
            DataTable dtb = new DataTable();
            using (SqlConnection cnn = new SqlConnection(strConstr))
            {
                cnn.Open();
                using (SqlDataAdapter dad = new SqlDataAdapter(sQuery, cnn))
                {
                    dad.Fill(dtb);
                }
                cnn.Close();
            }
            return dtb;
        }
        public SqlCommand CallCommand()
        {
            SqlCommand newCommand = new SqlCommand();

            return newCommand;
        }
        public SqlConnection CallConnection()
        {
            SqlConnection newConnection = new SqlConnection(strConstr);
            return newConnection;
        }


    }
}