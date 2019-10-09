using ClosedXML.Excel;
using POSOINV.Functions;
using POSOINV.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WSIExchange;

namespace SOPOINV.Forms
{
    public partial class InventoryReports : System.Web.UI.Page
    {
        SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            //scriptManager.RegisterPostBackControl(this.btnSaveNewSerial);
            //scriptManager.RegisterPostBackControl(this.gvSupplier);
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            //paneProd.Value = Request.Form[paneProd.UniqueID];
            //paneSub.Value = Request.Form[paneSub.UniqueID];
            //paneItem.Value = Request.Form[paneItem.UniqueID];
            //paneSerial.Value = Request.Form[paneSerial.UniqueID];

            string username = "";
            string access = "";
            try
            {
                username = Session["User_Domain"].ToString();
                access = Session["User_Access"].ToString();
            }
            catch
            {
                Response.Redirect("~/Login.aspx");
            }

            if (!this.IsPostBack)
            {
                for (int x = Session.Count - 1; x >= 5; x--)
                {
                    Session.RemoveAt(x);
                }

                if (username != "")
                {

                }
                else
                {
                    Response.Redirect("~/Login.aspx");
                }
            }
        }
        
        protected void btnDownloadInventorySerialReport_Click(object sender, EventArgs e)
        {
            //DataTable dtReport = new DataTable();
            //dtReport.Columns.Add("PO Number");
            //dtReport.Columns.Add("Item Number");
            //dtReport.Columns.Add("Serial Number");
            //dtReport.Columns.Add("Time Stamp");
            //dtReport.Columns.Add("Cost");

            //DataTable dtSO = Item_Serial.InventorySerialReport(oCon);

            //for (int x = 0; x <= dtSO.Rows.Count - 1; x++)
            //{
            //    DataRow dr = dtReport.NewRow();
            //    dr[0] = dtSO.Rows[x][0];
            //    dr[1] = dtSO.Rows[x][1];
            //    dr[2] = dtSO.Rows[x][2];
            //    dr[3] = dtSO.Rows[x][3];
            //    dr[4] = dtSO.Rows[x][4];
            //    dtReport.Rows.Add(dr);
            //}

            //using (XLWorkbook wb = new XLWorkbook())
            //{
            //    wb.Worksheets.Add(dtReport, "Inventory Report");

            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.Charset = "";
            //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    Response.AddHeader("content-disposition", "attachment;filename=Inventory Serial Report " + DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Year.ToString() + ".xlsx");
            //    using (MemoryStream MyMemoryStream = new MemoryStream())
            //    {
            //        wb.SaveAs(MyMemoryStream);
            //        MyMemoryStream.WriteTo(Response.OutputStream);
            //        Response.Flush();
            //        Response.End();
            //    }
            //}

            //DateTime dateTo;
            //DateTime dateFrom;

            //dateFrom = Convert.ToDateTime("1/1/1753");

            //if (txtToDate.Text != "")
            //{
            //    dateTo = Convert.ToDateTime(txtToDate.Text);
            //}
            //else
            //{
            //    dateTo = DateTime.Now;
            //}

            //Report_Request_Model report_Request_Model = new Report_Request_Model
            //{
            //    Request_Type = "IRSerial",
            //    DateFrom = dateFrom,
            //    DateTo = dateTo,
            //    SendToEmail = Session["User_Email"].ToString(),
            //    isFinished = "N",
            //    TimeStamp = DateTime.Now,
            //    CreatedBy = Session["User_Domain"].ToString(),
            //};
            //bool result = Report_Request.Save(oCon, report_Request_Model);
            //if (result == true)
            //{
            //    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Success! Kindly wait for the report to be sent in your email. Thank you." + ControlChars.Quote + ");</script>");
            //}
            //else
            //{
            //    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "There is an error in processing your report. Please contact helpdesk." + ControlChars.Quote + ");</script>");
            //}
            Session["ReportType"] = "IRSerial";
            dvDateRange.Visible = true;
        }
        
        protected void btnShowDownloadModal_Click(object sender, EventArgs e)
        {
            Session["ReportType"] = "IRSummary";
            dvDateRange.Visible = true;
        }

        protected void btnDownloadInventoryReport_Click(object sender, EventArgs e)
        {
            DateTime dateTo;
            DateTime dateFrom;

            dateFrom = Convert.ToDateTime("1/1/1753");

            if (txtToDate.Text != "")
            {
                dateTo = Convert.ToDateTime(txtToDate.Text);
            }
            else
            {
                dateTo = DateTime.Now;
            }

            //DataTable dtSO = Trans_History.RetrieveDataPURReport(oCon, dateFrom, dateTo);

            //dvDateRange.Visible = false;

            ////using (XLWorkbook wb = new XLWorkbook())
            ////{
            ////    wb.Worksheets.Add(dtSO, "Inventory Report");

            ////    Response.Clear();
            ////    Response.Buffer = true;
            ////    Response.Charset = "";
            ////    Response.ContentType = "application/vnd.ms-excel";
            ////    Response.AddHeader("content-disposition", "attachment;filename=Inventory Report (PUR)" + DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Year.ToString() + ".xlsx");
            ////    using (MemoryStream MyMemoryStream = new MemoryStream())
            ////    {
            ////        wb.SaveAs(MyMemoryStream);
            ////        MyMemoryStream.WriteTo(Response.OutputStream);
            ////        Response.Flush();
            ////        Response.End();
            ////    }
            ////}

            //ExporttoExcel(dtSO);
            string request_mode = Session["ReportType"].ToString();
            
            Report_Request_Model report_Request_Model = new Report_Request_Model
            {
                Request_Type = request_mode,
                DateFrom = dateFrom,
                DateTo = dateTo,
                SendToEmail = Session["User_Email"].ToString(),
                isFinished = "N",
                TimeStamp = DateTime.Now,
                CreatedBy = Session["User_Domain"].ToString(),
            };
            bool result = Report_Request.Save(oCon, report_Request_Model);
            if (result == true)
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Success! Kindly wait for the report to be sent in your email. Thank you." + ControlChars.Quote + ");</script>");
            }
            else
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "There is an error in processing your report. Please contact helpdesk." + ControlChars.Quote + ");</script>");
            }
        }

        private void ExporttoExcel(DataTable table)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            //HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.xls");
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=Inventory Report (PUR)" + DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Year.ToString() + ".xls");

            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR><BR><BR>");
            //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
              "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            //am getting my grid's column headers
            int columnscount = table.Columns.Count;

            for (int j = 0; j < columnscount; j++)
            {      //write in new column
                HttpContext.Current.Response.Write("<Td>");
                //Get column headers  and make it as bold in excel columns
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(table.Columns[j].ColumnName.ToString());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
            }
            HttpContext.Current.Response.Write("</TR>");
            foreach (DataRow row in table.Rows)
            {//write in new row
                HttpContext.Current.Response.Write("<TR>");
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write("<Td>");
                    HttpContext.Current.Response.Write(row[i].ToString());
                    HttpContext.Current.Response.Write("</Td>");
                }

                HttpContext.Current.Response.Write("</TR>");
            }
            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        protected void btnCloseModal_Click(object sender, EventArgs e)
        {
            dvDateRange.Visible = false;
        }

        protected void btnCloseMenu_Click(object sender, EventArgs e)
        {
            dvMenu.Visible = false;
        }

        protected void btnMenu_Click(object sender, EventArgs e)
        {
            dvMenu.Visible = true;
        }
    }
}