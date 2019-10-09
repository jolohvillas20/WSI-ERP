using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POSOINV.Models;
using POSOINV.Functions;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
namespace SOPOINV.Forms
{
    public partial class InventoryHistory : System.Web.UI.Page
    {
        SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                for (int x = Session.Count - 1; x >= 5; x--)
                {
                    Session.RemoveAt(x);
                }

                string username;
                string access = "";
                try
                {
                    username = Session["User_Domain"].ToString();
                    access = Session["User_Access"].ToString();
                }
                catch
                {
                    username = "";
                    access = "";
                }

                if (username != "")
                {
                    if (access == "AU" || access == "OP" || access == "IT")
                    {
                    }
                    else
                    {
                        Response.Redirect("~/LandingPage.aspx");
                    }
                }
                else
                {
                    Response.Redirect("~/Login.aspx");
                }
            }
        }
        protected void btnCloseMenu_Click(object sender, EventArgs e)
        {
            dvMenu.Visible = false;
        }

        protected void btnMenu_Click(object sender, EventArgs e)
        {
            dvMenu.Visible = true;
        }
        #region  "Sales Return" 

        private void getInventoryHistory()
        {
            var ds = Trans_History.RetrieveDataInquiry(oCon, txtSearchInput.Text);
            grvInventoryHistory.DataSource = ds;
            grvInventoryHistory.DataBind();

            for (int x = 0; x <= ds.Rows.Count - 1; x++)
            {
                string transcode = ds.Rows[x][1].ToString();
                if (transcode == "SLE")
                {
                    string picknumber = ds.Rows[x][5].ToString();

                    int idCustomer = Trans_History.GetCustomerID(oCon, picknumber);

                    var cd = Customer_Details.RetrieveData(oCon, idCustomer, "");

                    gvCustomerDetail.DataSource = cd;
                    gvCustomerDetail.DataBind();
                }
            }
        }

        //protected void grvInventoryHistory_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    GridViewRow row = grvInventoryHistory.SelectedRow;
        //    Session["rowIndex"] = row.RowIndex.ToString();
        //    Session["idItem"] = row.Cells[0].Text.Trim();
        //    //txtSONumber.Text = row.Cells[1].Text.Trim();
        //    Session["Item_Number"] = row.Cells[2].Text.Trim();
        //    Session["Cost"] = row.Cells[4].Text.Trim();
        //}

        protected void grvInventoryHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;//idItem
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;//idItem         
            }
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grvInventoryHistory, "Select$" + e.Row.RowIndex);
            //    e.Row.ToolTip = "Click To Select this row";
            //}
        }

        protected void grvInventoryHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvInventoryHistory.PageIndex = e.NewPageIndex;
            getInventoryHistory();
        }

        protected void btnSearchInput_Click(object sender, EventArgs e)
        {
            getInventoryHistory();
        }


        private void clearReturns()
        {
            grvInventoryHistory.DataSource = new DataTable();
            grvInventoryHistory.DataBind();
        }


        protected void btnClearSRE_Click(object sender, EventArgs e)
        {
            clearReturns();
        }

        protected void gvCustomerDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;//idItem
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;//idItem         
            }
        }
        #endregion
    }
}