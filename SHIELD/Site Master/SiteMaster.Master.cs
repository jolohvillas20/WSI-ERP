using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SHIELD.Site_Master
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = "";
            try
            {
                username = Session["User_Domain"].ToString();
            }
            catch
            {
                username = "";
            }

            if (!Page.IsPostBack)
            {
                if (username != "")
                {
                    LoginName1.Text = Session["User_Name"].ToString();
                }
                else
                {
                    Response.Redirect("~/Login.aspx");
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.RemoveAll();
            Response.Redirect("~/Login.aspx", false);
        }
    }
}
