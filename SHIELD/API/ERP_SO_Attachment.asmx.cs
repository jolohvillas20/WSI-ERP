using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Services;
using POSOINV.Functions;
using POSOINV.Models;
using System.Configuration;

namespace SOPOINV.API
{
    /// <summary>
    /// Summary description for ERP_SO_Attachment
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ERP_SO_Attachment : System.Web.Services.WebService
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        [WebMethod()]
        public byte[] SOUpload(string soNumber)
        {
            Microsoft.Reporting.WebForms.ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            viewer.LocalReport.ReportPath = Server.MapPath(@"~\Resources\LSISalesOrder.rdlc");
            //viewer.LocalReport.ReportPath = "SalesOrder.rdlc";

            List<SO_Header_Model> soheadermodel = SO_Header.RetrieveData(connection, soNumber, false);

            string strSONumber = soNumber;
            string strOrderDate = soheadermodel[0].Order_Date.ToShortDateString();
            string strDueDate = soheadermodel[0].Due_Date.ToShortDateString();
            string strCustomerPOnum = soheadermodel[0].Customer_PO;

            string strSalesman = soheadermodel[0].Salesman;

            string strCreditTerm = soheadermodel[0].credit_term;
            string strOtherCharges = soheadermodel[0].Other_Charges.ToString();

            decimal grossamount = soheadermodel[0].Gross_Amount;
            decimal finaldiscount = soheadermodel[0].Final_Discount;
            decimal netamount = grossamount - soheadermodel[0].Freight_Charges - soheadermodel[0].Other_Charges;
            decimal taxamount = grossamount * Convert.ToDecimal(1 - (1 / 1.12));

            decimal strGrossAmount = grossamount;
            decimal strFinalDiscount = finaldiscount;
            decimal strCharges = soheadermodel[0].Freight_Charges + soheadermodel[0].Other_Charges;
            decimal strNetAmount = netamount;
            string strRemarks = soheadermodel[0].Remarks;

            List<Customer_Details_Model> customerDetails = Customer_Details.RetrieveData(connection, soheadermodel[0].idCustomer, "");

            string strCustomerCode = customerDetails[0].Customer_Code;
            string strAddress1 = customerDetails[0].Address1;
            string strAddress2 = customerDetails[0].Address2;
            string strAddress3 = customerDetails[0].Address3;
            string strAddress4 = customerDetails[0].Address4;

            string strCompanyName = customerDetails[0].Company_Name;
            decimal strOutputVat = taxamount;
            decimal strTaxableSales = Convert.ToDecimal(netamount) / Convert.ToDecimal(1.12);
            string strShipAddress1 = customerDetails[0].AddressShipping1;
            string strShipAddress2 = customerDetails[0].AddressShipping2;
            string strShipAddress3 = customerDetails[0].AddressShipping3;
            string strShipAddress4 = customerDetails[0].AddressShipping4;

            var lDataAdd = SO_Detail.RetrieveDataForSOCreation(connection, soheadermodel[0].idSOHeader);

            if (strAddress1 == "&nbsp;")
            {
                strAddress1 = " ";
            }
            if (strAddress2 == "&nbsp;")
            {
                strAddress2 = " ";
            }
            if (strAddress3 == "&nbsp;")
            {
                strAddress3 = " ";
            }
            if (strAddress4 == "&nbsp;")
            {
                strAddress4 = " ";
            }
            if (strShipAddress1 == "&nbsp;")
            {
                strShipAddress1 = " ";
            }
            if (strShipAddress2 == "&nbsp;")
            {
                strShipAddress2 = " ";
            }
            if (strShipAddress3 == "&nbsp;")
            {
                strShipAddress3 = " ";
            }

            ReportParameter p1 = new ReportParameter("Order_Date", strOrderDate);
            ReportParameter p2 = new ReportParameter("Due_Date", strDueDate);
            ReportParameter p3 = new ReportParameter("Customer_PO", strCustomerPOnum);
            ReportParameter p4 = new ReportParameter("Salesman", strSalesman);
            ReportParameter p5 = new ReportParameter("credit_term", strCreditTerm);
            ReportParameter p6 = new ReportParameter("SO_Number", strSONumber);
            ReportParameter p7 = new ReportParameter("Gross_Amount", strGrossAmount.ToString("n", CultureInfo.GetCultureInfo("en-US")));
            ReportParameter p8 = new ReportParameter("Final_Discount", strFinalDiscount.ToString("n", CultureInfo.GetCultureInfo("en-US")));
            ReportParameter p9 = new ReportParameter("Charges", strCharges.ToString("n", CultureInfo.GetCultureInfo("en-US")));
            ReportParameter p10 = new ReportParameter("Net_Amount", strNetAmount.ToString("n", CultureInfo.GetCultureInfo("en-US")));
            ReportParameter p11 = new ReportParameter("Customer_Code", strCustomerCode);
            ReportParameter p12 = new ReportParameter("Address1", strAddress1);
            ReportParameter p13 = new ReportParameter("Address2", strAddress2);
            ReportParameter p14 = new ReportParameter("Address3", strAddress3);
            ReportParameter p15 = new ReportParameter("Address4", strAddress4);
            ReportParameter p16 = new ReportParameter("Remarks", strRemarks);
            ReportParameter p17 = new ReportParameter("Company_Name", strCompanyName);
            ReportParameter p18 = new ReportParameter("Output_Vat", strOutputVat.ToString("n", CultureInfo.GetCultureInfo("en-US")));
            ReportParameter p19 = new ReportParameter("Taxable_Sales", strTaxableSales.ToString("n", CultureInfo.GetCultureInfo("en-US")));
            ReportParameter p20 = new ReportParameter("AddressShipping1", strShipAddress1);
            ReportParameter p21 = new ReportParameter("AddressShipping2", strShipAddress2);
            ReportParameter p22 = new ReportParameter("AddressShipping3", strShipAddress3);

            viewer.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7,
            p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22 });


            ReportDataSource repDataSource1 = new ReportDataSource("DataSet1", lDataAdd);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(repDataSource1);

            byte[] bytes = viewer.LocalReport.Render("PDF");

            DeleteFile(soheadermodel[0].idSOHeader);

            // insert data
            string strQuery = "INSERT INTO a_SO_Attachment" + "(idSOHeader, FileName, content_type, data)" + " values (@idSOHeader, @FileName, @content_type, @data)";
            SqlCommand cmd = new SqlCommand(strQuery);

            cmd.Parameters.Add("@idSOHeader", SqlDbType.VarChar).Value = soheadermodel[0].idSOHeader;

            cmd.Parameters.Add("@FileName", SqlDbType.VarChar).Value = strSONumber;
            cmd.Parameters.Add("@content_type", SqlDbType.VarChar).Value = "text/html";
            cmd.Parameters.Add("@data", SqlDbType.Binary).Value = bytes;

            InsertUpdateData(cmd);

            return bytes;
        }

        protected void DeleteFile(int idsoheader)
        {
            string strQuery = "DELETE FROM a_SO_Attachment WHERE idSOHeader=@idSOHeader";
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add("@idSOHeader", SqlDbType.Int).Value = idsoheader;
            InsertUpdateData(cmd);
        }

        private bool InsertUpdateData(SqlCommand cmd)
        {
            SqlConnection con = new SqlConnection(connection.ConnectionString);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
}
