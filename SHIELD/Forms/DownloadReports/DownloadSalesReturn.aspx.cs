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
    public partial class DownloadSalesReturn : System.Web.UI.Page
    {
        SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            string SLRNumber = "";
            HttpRequest q = Request;
            NameValueCollection n = q.QueryString;
            if (n.HasKeys())
            {
                if (n.GetKey(0) != "")
                    SLRNumber = n.Get(0);
                //string SLRNumber = Session["SLRNumber"].ToString();
                if (SLRNumber != "")
                {
                    var lData = Sales_Return.RetreiveDataForPrinting(oCon, SLRNumber);

                    string DocNumber = "";
                    string RefNum = "";
                    string isReplacement = "";
                    string Site = "";
                    string TransDate = "";
                    string Address1 = "";
                    string Address2 = "";
                    string Address3 = "";
                    string Address4 = "";
                    string SupplierName = "";
                    string Remarks = "";

                    var listData = Return_Header.RetrieveData(oCon, "", SLRNumber);
                    RefNum = listData[0].Document_Number;
                    isReplacement = listData[0].isReplacement;
                    DocNumber = listData[0].Auth_Number;
                    TransDate = listData[0].Date_Returned.ToShortDateString();
                    Remarks = listData[0].Remarks;

                    var listSite = Site_Loc.RetrieveData(oCon, listData[0].Site);
                    Site = listSite.Rows[0][2].ToString();
                  
                    var reselleraddress = Customer_Details.RetrieveData(oCon, 0, listData[0].Customer_Code);
                    SupplierName = reselleraddress[0].Company_Name;
                    Address1 = reselleraddress[0].Address1;
                    Address2 = reselleraddress[0].Address2;
                    Address3 = reselleraddress[0].Address3;
                    Address4 = reselleraddress[0].Address4;

                    Microsoft.Reporting.WebForms.ReportViewer viewer = new ReportViewer();
                    viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                    viewer.LocalReport.ReportPath = Server.MapPath(@"~\Resources\SalesReturn.rdlc");

                    if (Address1 == "")
                    {
                        Address1 = "N/A";
                    }
                    if (Address2 == "")
                    {
                        Address2 = "N/A";
                    }
                    if (Address3 == "")
                    {
                        Address3 = "N/A";
                    }
                    if (Address4 == "")
                    {
                        Address4 = "N/A";
                    }
                    if (SupplierName == "")
                    {
                        SupplierName = "N/A";
                    }
                    if (Remarks == "")
                    {
                        Remarks = "N/A";
                    }

                    ReportParameter p1 = new ReportParameter("DocNumber", DocNumber);
                    ReportParameter p2 = new ReportParameter("RefNum", RefNum);
                    ReportParameter p3 = new ReportParameter("isReplacement", isReplacement);
                    ReportParameter p4 = new ReportParameter("Site", Site);
                    ReportParameter p5 = new ReportParameter("TransDate", TransDate);
                    ReportParameter p6 = new ReportParameter("Address1", Address1);
                    ReportParameter p7 = new ReportParameter("Address2", Address2);
                    ReportParameter p8 = new ReportParameter("Address3", Address3);
                    ReportParameter p9 = new ReportParameter("Address4", Address4);
                    ReportParameter p10 = new ReportParameter("SupplierName", SupplierName);
                    ReportParameter p11 = new ReportParameter("Remarks", Remarks);
                    ReportParameter p12 = new ReportParameter("SLRNumber", SLRNumber);

                    ReportDataSource repDataSource = new ReportDataSource("DataSet1", lData);

                    viewer.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12 });

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
                        Response.AddHeader("content-disposition", "attachment; filename= " + SLRNumber + "." + extension);
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

                    //Microsoft.Reporting.WebForms.Warning[] warnings = null;
                    //string[] streamids = null;
                    //string mimeType = null;
                    //string encoding = null;
                    //string extension = null;
                    //string deviceInfo;
                    //byte[] bytes;
                    //Microsoft.Reporting.WebForms.LocalReport lr = new Microsoft.Reporting.WebForms.LocalReport();

                    //Warning[] warnings;
                    //string[] streamIds;
                    //string mimeType = string.Empty;
                    //string encoding = string.Empty;
                    //string extension = "pdf";

                 
                    //// byte[] bytes = viewer.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                    //// Now that you have all the bytes representing the PDF report, buffer it and send it to the client.          
                    //// System.Web.HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //Response.Buffer = true;
                    //Response.Clear();
                    //Response.ContentType = mimeType;
                    //Response.AddHeader("content-disposition", "attachment; filename= " + SLRNumber + "." + extension);
                    //Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
                    //Response.Flush(); // send it to the client to download  
                    //Response.End();
                }
            }
        }
    }
}