using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOPOINV.Forms
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
                try
                {
                    username = Session["User_Domain"].ToString();
                }
                catch
                {
                    username = "";
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
    }
}