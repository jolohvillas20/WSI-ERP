using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOPOINV.Forms.InventoryManagement.InventoryTransactions
{
    public partial class RTS : System.Web.UI.Page
    {
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

        protected void btnView_Click(object sender, EventArgs e)
        {
            dvView.Visible = true;
            dvCreate.Visible = false;
            dvRecieve.Visible = false;
            GetView();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            dvView.Visible = false;
            dvCreate.Visible = true;
            dvRecieve.Visible = false;
        }

        protected void btnRecieve_Click(object sender, EventArgs e)
        {
            dvView.Visible = false;
            dvCreate.Visible = false;
            dvRecieve.Visible = true;
        }

        #region View
        private void GetView()
        {
            var dt = "";
            gvView.DataSource = dt;
            gvView.DataBind();
        }

        protected void gvView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        #endregion

        #region Create

        #endregion

        #region Recieve

        #endregion

    }
}