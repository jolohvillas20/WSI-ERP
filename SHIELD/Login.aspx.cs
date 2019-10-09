using System;
using System.Data.SqlClient;
using System.Configuration;
//using POSOINV.Functions;
using POSOINV.Functions;
using POSOINV.Models;
using System.Web;
using System.Collections.Generic;
using WSICrytography;

namespace SHIELD
{
    public partial class Login : System.Web.UI.Page
    {
        SqlConnection oConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["User_Domain"].ToString() != "")
                {
                    Response.Redirect("~/LandingPage.aspx",false);
                }
            }
            catch
            {
                Session.Clear();
                Session.RemoveAll();
            }
            //Session.Clear();
            //Session.RemoveAll();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

            Login1 login = new Login1();

            //string strTest = login.GetUserAccess(txtUsername.Text.Trim(), oConnection);

            if (txtUsername.Text != "" && txtPassword.Text != "")
            {
                var encPass = login.GetEncryptedPassword(oConnection, txtUsername.Text.Trim());
                var password = WSICryptography.Decrypt(encPass);

                bool boolValue = false;

                if (password == txtPassword.Text.Trim())
                    boolValue = true;

                if (boolValue == true)
                {
                    string user_access = login.GetUserAccess(txtUsername.Text.Trim(), oConnection);
                    List<Login_Model> loginModel = Login1.getUserInformation(oConnection, txtUsername.Text.Trim());
                    Session["User_Domain"] = txtUsername.Text.Trim();
                    Session["User_Access"] = loginModel[0].User_Access;
                    Session["User_Email"] = loginModel[0].User_Email;
                    Session["User_Name"] = loginModel[0].User_Name;
                    //Session["User_Product"] = loginModel[0].User_Product;
                    Session["idUser"] = loginModel[0].idUser;
                    Response.Redirect("~/LandingPage.aspx");
                }
                else
                {
                    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Incorrect username or password" + ControlChars.Quote + ");</script>");
                }
            }
            else
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Incorrect username or password" + ControlChars.Quote + ");</script>");
            }
        }
    }
}