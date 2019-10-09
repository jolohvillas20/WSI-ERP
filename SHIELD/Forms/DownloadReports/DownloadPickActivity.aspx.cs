using Microsoft.Reporting.WebForms;
using POSOINV.Functions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOPOINV.Forms
{
    public partial class DownloadPickActivity : System.Web.UI.Page
    {
        SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            string searchSO = Session["SONumber"].ToString();

            var lData = Pick_Activity.GetPickActivityRegister(oCon, searchSO);

            int i = 0;

            string strPickListDate = null;
            string strUserId = null;
            string strSiteDescription = null;
            string strSite = null;
            string strPickDate = null;
            string strPickListNumber = null;
            string strCustomerName = null;
            string strCustCode = null;
            string strAddress = null;
            string strExternalComments = null;
            string strSONumber = null;
            string OperationsComment = Session["Pick_Comments"].ToString();

            if (lData.Count != 0)
            {
                strPickListDate = lData[0].PickListDate;
                strUserId = lData[0].UserId.ToString().TrimEnd();

                strSiteDescription = lData[0].SiteDescription;
                strSite = lData[0].Site.ToString().TrimEnd();
                strPickDate = DateTime.Parse(lData[0].PickListDate).ToString("MMM dd, yyyy");
                strPickListNumber = lData[0].PickListNo;
                strCustomerName = lData[0].CustomerName;
                strCustCode = lData[0].CustCode;
                strAddress = lData[0].Address;
                strExternalComments = lData[0].ExternalComments;
                strSONumber = lData[0].SONumber;
            }

            Microsoft.Reporting.WebForms.ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Server.MapPath(@"~\Resources\PickActivityRegister.rdlc");

            ReportParameter p1 = new ReportParameter("PickListDate", strPickListDate);
            ReportParameter p2 = new ReportParameter("UserID", "User ID: " + strUserId);
            ReportParameter p3 = new ReportParameter("SiteDescription", strSite + " - " + strSiteDescription);
            ReportParameter p4 = new ReportParameter("PickDate", strPickDate);
            ReportParameter p5 = new ReportParameter("PickListNumber", strPickListNumber);
            ReportParameter p6 = new ReportParameter("CustomerName", strCustomerName);
            ReportParameter p7 = new ReportParameter("CustCode", strCustCode);
            ReportParameter p8 = new ReportParameter("Address", strAddress);
            ReportParameter p9 = new ReportParameter("ExternalComments", strExternalComments + "(" + OperationsComment + ")");
            ReportParameter p10 = new ReportParameter("SONumber", strSONumber);

            ReportDataSource repDataSource = new ReportDataSource("DataSet1", lData);

            viewer.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 });

            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(repDataSource);

            //Microsoft.Reporting.WebForms.Warning[] warnings = null;
            //string[] streamids = null;
            //string mimeType = null;
            //string encoding = null;
            //string extension = null;
            //string deviceInfo;
            //byte[] bytes;
            //Microsoft.Reporting.WebForms.LocalReport lr = new Microsoft.Reporting.WebForms.LocalReport();

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = "pdf";

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            // byte[] bytes = viewer.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.          
            // System.Web.HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename= " + Session["Pick_Number"].ToString() + "." + extension);

            for (int x = Session.Count - 1; x >= 5; x--)
            {
                Session.RemoveAt(x);
            }

            Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
            Response.Flush(); // send it to the client to download  
            Response.End();
        }
    }
}