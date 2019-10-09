using POSOINV.Functions;
using POSOINV.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WSICrytography;

namespace SOPOINV.Forms
{
    public partial class Maintenance : System.Web.UI.Page
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
                    if (access == "AU" || access == "IT")
                    {
                        getSite();
                        getUserView();
                        getCreditTerm();
                        getUserProduct();
                        getProducts();
                        Session["userMode"] = "Save";
                        Session["clMode"] = "Save";
                        Session["siteMode"] = "Save";
                        Session["userProductMode"] = "Save";
                    }
                }
                else
                {
                    Response.Redirect("~/Login.aspx");
                }
            }
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);

                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        #region "user"
        private void getUserView()
        {
            DataTable dt = Users.RetrieveData(oCon);
            gvUsers.DataSource = dt;
            gvUsers.DataBind();
            ddlUsers.DataSource = dt;
            ddlUsers.DataValueField = "idUser";
            ddlUsers.DataTextField = "User_Name";
            ddlUsers.DataBind();
        }

        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsers.PageIndex = e.NewPageIndex;
            getUserView();
        }

        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {
                e.Row.Cells[0].Visible = false;
            }

            if ((e.Row.RowType == DataControlRowType.Header))
            {
                e.Row.Cells[0].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvUsers, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click To Select this row";
            }
        }

        protected void gvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["idUser"] = gvUsers.SelectedRow.Cells[0].Text.Trim();
            txtUserName.Text = gvUsers.SelectedRow.Cells[1].Text.Trim();
            txtUserEmail.Text = gvUsers.SelectedRow.Cells[2].Text.Trim();
            txtUserDomain.Text = gvUsers.SelectedRow.Cells[3].Text.Trim();
            ddUserAccess.SelectedValue = gvUsers.SelectedRow.Cells[4].Text.Trim();
            Session["userMode"] = "Update";
        }

        protected void btnSaveUser_Click(object sender, EventArgs e)
        {
            if (txtPassword1.Text == txtPassword2.Text)
            {
                Users_Model mdlUsers = new Users_Model();

                mdlUsers.User_Name = txtUserName.Text.Trim();
                mdlUsers.User_Email = txtUserEmail.Text.Trim();
                mdlUsers.User_Domain = txtUserDomain.Text.Trim();
                mdlUsers.User_Password = WSICryptography.Encrypt(txtPassword1.Text.Trim());
                mdlUsers.User_Access = ddUserAccess.SelectedValue;

                if (Session["userMode"].ToString() == "Save")
                {
                    Users.Save(oCon, mdlUsers);
                }
                else if (Session["userMode"].ToString() == "Update")
                {
                    mdlUsers.idUser = Convert.ToInt32(Session["idUser"].ToString());
                    Users.Update(oCon, mdlUsers);
                }
                clearUser();
                getUserView();
            }
        }

        protected void btnDeleteUser_Click(object sender, EventArgs e)
        {
            Users.Delete(oCon, Convert.ToInt32(Session["idUser"].ToString()));
            clearUser();
            getUserView();
        }

        protected void btnCancelUser_Click(object sender, EventArgs e)
        {
            clearUser();
            getUserView();
        }

        private void clearUser()
        {
            txtUserName.Text = "";
            txtUserEmail.Text = "";
            txtUserDomain.Text = "";
            ddUserAccess.SelectedValue = "0";
            txtPassword1.Text = "";
            txtPassword2.Text = "";
            Session["userMode"] = "Save";
            Session["idUser"] = "";
        }
        #endregion
        #region "credit term"
        private void getCreditTerm()
        {
            DataTable dt = Credit_Term.RetrieveData(oCon);
            gvCreditLimit.DataSource = dt;
            gvCreditLimit.DataBind();
        }

        protected void gvCreditLimit_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCreditLimit.PageIndex = e.NewPageIndex;
            getCreditTerm();
        }

        protected void gvCreditLimit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvCreditLimit, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click To Select this row";
            }
        }

        protected void gvCreditLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["credit_term"] = gvCreditLimit.SelectedRow.Cells[0].Text.Trim();
            txtCreditTerm.Text = gvCreditLimit.SelectedRow.Cells[0].Text.Trim();
            txtDescription.Text = gvCreditLimit.SelectedRow.Cells[1].Text.Trim();
            txtDays.Text = gvCreditLimit.SelectedRow.Cells[2].Text.Trim();
            Session["clMode"] = "Update";
        }

        protected void btnSaveCL_Click(object sender, EventArgs e)
        {
            Credit_Term_Model mdlCL = new Credit_Term_Model();

            mdlCL.credit_term = txtCreditTerm.Text.Trim();
            mdlCL.term_desc = txtDescription.Text.Trim();
            mdlCL.days_to_pay = txtDays.Text.Trim();

            if (Session["clMode"].ToString() == "Save")
            {
                Credit_Term.Save(oCon, mdlCL);
            }
            else if (Session["clMode"].ToString() == "Update")
            {
                mdlCL.credit_term = Session["credit_term"].ToString();
                Credit_Term.Update(oCon, mdlCL);
            }
            clearCL();
            getCreditTerm();
        }

        protected void btnDeleteCL_Click(object sender, EventArgs e)
        {
            Credit_Term.Delete(oCon, Session["credit_term"].ToString());
            clearCL();
            getCreditTerm();
        }

        protected void btnCancelCL_Click(object sender, EventArgs e)
        {
            clearCL();
        }

        private void clearCL()
        {
            txtUserName.Text = "";
            txtUserEmail.Text = "";
            txtUserDomain.Text = "";
            Session["clMode"] = "Save";
        }
        #endregion
        protected void Button1_Click(object sender, EventArgs e)
        {
            Item_Master.InventoryCheckForError(oCon);
        }
        #region "Site"
        private void getSite()
        {
            DataTable dt = Site_Loc.RetrieveData(oCon, "");
            gvSite.DataSource = dt;
            gvSite.DataBind();
        }

        protected void gvSite_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSite.PageIndex = e.NewPageIndex;
            getSite();
        }

        protected void gvSite_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {
                e.Row.Cells[0].Visible = false;
            }

            if ((e.Row.RowType == DataControlRowType.Header))
            {
                e.Row.Cells[0].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvSite, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click To Select this row";
            }
        }

        protected void gvSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["idSite"] = gvSite.SelectedRow.Cells[0].Text.Trim();
            txtSiteName.Text = gvSite.SelectedRow.Cells[1].Text.Trim();
            txtSiteDesc.Text = gvSite.SelectedRow.Cells[2].Text.Trim();
            Session["siteMode"] = "Update";
        }

        protected void btnSaveSite_Click(object sender, EventArgs e)
        {
            Site_Model mdlCL = new Site_Model
            {
                Site_Desc = txtSiteDesc.Text.Trim(),
                Site_Name = txtSiteName.Text.Trim()
            };
            
            if (Session["siteMode"].ToString() == "Save")
            {
                Site_Loc.Save(oCon, mdlCL);
            }
            else if (Session["siteMode"].ToString() == "Update")
            {
                mdlCL.idSite = Convert.ToInt32(Session["idSite"].ToString());
                Site_Loc.Update(oCon, mdlCL);
            }
            clearSite();
            getSite();
        }

        protected void btnDeleteSite_Click(object sender, EventArgs e)
        {
            Site_Loc.Delete(oCon, Convert.ToInt32(Session["idSite"].ToString()));
            clearSite();
            getSite();
        }

        protected void btnCancelSite_Click(object sender, EventArgs e)
        {
            clearSite();
        }

        private void clearSite()
        {
            Session["idSite"] = 0;
            txtSiteName.Text = "";
            txtSiteDesc.Text = "";
            Session["siteMode"] = "Save";
        }
        #endregion
        #region "UserProduct"
        private void getUserProduct()
        {
            var dt = User_Product.RetrieveData(oCon);
            gvUserProduct.DataSource = dt;
            gvUserProduct.DataBind();
        }
        private void getProducts()
        {
            var dt = Item_Class.RetrieveData(oCon, "", "");
            ddlProduct.DataSource = dt;
            ddlProduct.DataValueField = "idClass";
            ddlProduct.DataTextField = "Product_Name";

            ddlProduct.DataBind();
        }
        protected void gvUserProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUserProduct.PageIndex = e.NewPageIndex;
            getUserProduct();
        }

        protected void gvUserProduct_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
            }

            if ((e.Row.RowType == DataControlRowType.Header))
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvUserProduct, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click To Select this row";
            }
        }

        protected void gvUserProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["idUserProduct"] = gvUserProduct.SelectedRow.Cells[0].Text.Trim();
            ddlProduct.SelectedValue = gvUserProduct.SelectedRow.Cells[1].Text.Trim();
            ddlUsers.SelectedValue = gvUserProduct.SelectedRow.Cells[2].Text.Trim();
            Session["userProductMode"] = "Update";
        }

        protected void btnSaveUserProduct_Click(object sender, EventArgs e)
        {
            User_Product_Model mdlUserProduct = new User_Product_Model();

            mdlUserProduct.idProduct = Convert.ToInt32(ddlProduct.SelectedValue);
            mdlUserProduct.idUser = Convert.ToInt32(ddlUsers.SelectedValue);

            if (Session["userProductMode"].ToString() == "Save")
            {
                User_Product.Save(oCon, mdlUserProduct);
            }
            else if (Session["userProductMode"].ToString() == "Update")
            {
                mdlUserProduct.idUser = Convert.ToInt32(Session["idUserProduct"].ToString());
                User_Product.Update(oCon, mdlUserProduct);
            }

            getUserProduct();
            clearUserProduct();
        }

        protected void btnDeleteUserProduct_Click(object sender, EventArgs e)
        {
            User_Product.Delete(oCon, Convert.ToInt32(Session["idUserProduct"].ToString()));
            getUserProduct();
            clearUserProduct();
        }

        protected void btnCancelUserProduct_Click(object sender, EventArgs e)
        {
            getUserProduct();
            clearUserProduct();
        }
        private void clearUserProduct()
        {
            ddlProduct.SelectedValue = "0";
            ddlUsers.SelectedValue = "0";
            Session["idUserProduct"] = "";
            Session["userProductMode"] = "Save";
        }

        #endregion
    }
}