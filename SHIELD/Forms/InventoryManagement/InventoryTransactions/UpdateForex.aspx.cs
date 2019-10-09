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

namespace SOPOINV.Forms.InventoryManagement.InventoryTransactions
{
    public partial class UpdateForex : System.Web.UI.Page
    {
        SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            //   scriptManager.RegisterAsyncPostBackControl(this.btnVerifySerial);
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

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
                        GetCurrencyToDD(); GetForexRates();
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

        #region "Cost Adjustment"
        protected void GetCurrencyToDD()
        {
            var dt = Currency_Code.RetrieveData(oCon);
            ddlCurrency.DataSource = dt;
            ddlCurrency.DataTextField = "Currency_Name";
            ddlCurrency.DataValueField = "Currency_Code";
            ddlCurrency.DataBind();
        }

        public void GetForexRates()
        {
            var dt = Forex_Rate.RetrieveData(oCon);
            gvForexRate.DataSource = dt;
            gvForexRate.DataBind();
        }

        private void ClearAdj()
        {
            txtForexRate.Text = "";
        }

        #endregion

        protected void btnClearFE_Click(object sender, EventArgs e)
        {
            ClearAdj();
        }

        protected void btnSaveFE_Click(object sender, EventArgs e)
        {
            Forex_Rate_Model forex_Rate_Model = new Forex_Rate_Model
            {
                Amount = Convert.ToDecimal(txtForexRate.Text),
                Currency_Code = ddlCurrency.SelectedValue.ToString(),
                DateChange = DateTime.Now
            };
            Forex_Rate.Save(oCon, forex_Rate_Model);
            ClearAdj();
            GetForexRates();
        }

        protected void gvForexRate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
            }
        }

        protected void gvForexRate_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvForexRate.PageIndex = e.NewPageIndex;
            GetForexRates();
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