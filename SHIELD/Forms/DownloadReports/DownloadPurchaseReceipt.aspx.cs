using Microsoft.Reporting.WebForms;
using POSOINV.Functions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOPOINV.Forms
{
    public partial class DownloadPurchaseReceipt : System.Web.UI.Page
    {
        SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            string ponumber = "";
            string idItem = "";
            string docno = "";
            string sitename = "";
            string PRNumber = "";

            HttpRequest q = Request;
            NameValueCollection n = q.QueryString;
            if (n.HasKeys())
            {
                if (n.GetKey(0) != "")
                    ponumber = n.Get(0);

                if (n.GetKey(1) != "")
                    idItem = n.Get(1);

                if (n.GetKey(2) != "")
                    docno = n.Get(2);

                if (n.GetKey(3) != "")
                    sitename = n.Get(3);

                if (n.GetKey(4) != "")
                    PRNumber = n.Get(4);

                var lData = Purchase_Reciept.GetPurchaseReciept(oCon, ponumber, Convert.ToInt32(idItem));

                Microsoft.Reporting.WebForms.ReportViewer viewer = new ReportViewer();
                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.ReportPath = Server.MapPath(@"~\Resources\PurchaseReceipt.rdlc");

                ReportParameter p1 = new ReportParameter("PONumber", ponumber);
                ReportParameter p2 = new ReportParameter("DocNumber", docno);
                ReportParameter p3 = new ReportParameter("Site", sitename);
                ReportParameter p4 = new ReportParameter("TransDate", DateTime.Now.ToShortDateString());
                ReportParameter p5 = new ReportParameter("SupplierName", "GN AUDIO SINGAPORE PTE LTD");
                ReportParameter p6 = new ReportParameter("Address1", "150 Beach Road,");
                ReportParameter p7 = new ReportParameter("Address2", "#15-05/06, Gateway West");
                ReportParameter p8 = new ReportParameter("Address3", "Singapore");
                ReportParameter p9 = new ReportParameter("Address4", "189720");
                //ReportParameter p5 = new ReportParameter("SupplierName", Session["SupplierName"].ToString());
                //ReportParameter p6 = new ReportParameter("Address1", Session["Address1"].ToString());
                //ReportParameter p7 = new ReportParameter("Address2", Session["Address2"].ToString());
                //ReportParameter p8 = new ReportParameter("Address3", Session["Address3"].ToString());
                //ReportParameter p9 = new ReportParameter("Address4", Session["Address4"].ToString());
                ReportParameter p10 = new ReportParameter("PRNumber", PRNumber);

                ReportDataSource repDataSource = new ReportDataSource("DataSet2", lData);

                viewer.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 });

                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(repDataSource);

                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = "pdf";

                try
                {
                    byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename= " + "PR_" + PRNumber + "." + extension);
                    Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
                    Response.Flush(); // send it to the client to download  
                    Response.End();
                }
                catch
                {
                    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Download not successful!" + ControlChars.Quote + ");</script>");
                }
                finally
                {

                }
            }
        }
    }
}