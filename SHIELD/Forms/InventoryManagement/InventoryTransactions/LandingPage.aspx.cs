using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOPOINV.Forms.InventoryManagement.InventoryTransactions
{
    public partial class LandingPage : System.Web.UI.Page
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
    }
}