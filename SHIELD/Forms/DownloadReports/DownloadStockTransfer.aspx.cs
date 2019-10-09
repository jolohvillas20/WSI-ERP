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
    public partial class DownloadStockTransfer : System.Web.UI.Page
    {
        SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            string DocumentNumber = "";
            string STRNumber = "";
            string SourceSite = "";
            string DestinationSite = "";

            HttpRequest q = Request;
            NameValueCollection n = q.QueryString;
            if (n.HasKeys())
            {
                if (n.GetKey(0) != "")
                    DocumentNumber = n.Get(0);

                if (n.GetKey(1) != "")
                    STRNumber = n.Get(1);

                if (n.GetKey(2) != "")
                    SourceSite = n.Get(2);

                if (n.GetKey(3) != "")
                    DestinationSite = n.Get(3);

                var lDataAdd = Stock_Transfer_Header.GetDetailsForDownload(oCon, DocumentNumber);

                Microsoft.Reporting.WebForms.ReportViewer viewer = new ReportViewer();
                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.ReportPath = Server.MapPath(@"~\Resources\StockTransfer.rdlc");

                ReportParameter p1 = new ReportParameter("DocumentNumber", DocumentNumber);
                ReportParameter p2 = new ReportParameter("DateIssued", DateTime.Now.ToShortDateString());
                ReportParameter p3 = new ReportParameter("STRNumber", STRNumber);
                ReportParameter p4 = new ReportParameter("SourceSite", SourceSite);
                ReportParameter p5 = new ReportParameter("DestinationSite", DestinationSite);

                viewer.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5});

                ReportDataSource repDataSource1 = new ReportDataSource("dsSTR", lDataAdd);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(repDataSource1);

                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = "pdf";

                byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename= " + STRNumber + "." + extension);
                Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
                Response.Flush(); // send it to the client to download  
                Response.End();
            }
        }
    }
}