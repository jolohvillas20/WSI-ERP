using POSOINV.Functions;
using POSOINV.Models;
using SHIELD.ERP;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOPOINV.Forms
{
    public partial class InvoiceManagement : System.Web.UI.Page
    {
        SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

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
                    if (access == "AU" || access == "IT" || access == "BCC" || access == "PM")
                    {
                        getSOInvoiceList("");
                        defaultSettings();
                        GetSiteToDD();
                    }
                }
                else
                {
                    Response.Redirect("~/Login.aspx");
                }
            }
        }

        private void getSOInvoiceList(string so_number)
        {
            var ds = SO_Header.RetrieveDataForInvoice(oCon, so_number);
            grvSOInvoiceList.DataSource = ds;
            grvSOInvoiceList.DataBind();
        }

        protected void grvSOInvoiceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["Mode"] = "View";
            btnBack.Visible = true;
            btnSave.Visible = true;
            InvoicePanel.Visible = false;
            pnlAllDetails.Visible = true;
            //btnPrint.Visible = true;
            tblSearch.Visible = false;
            grossamt.Visible = true;
            outputvat.Visible = true;
            netamt.Visible = true;
            pnlOrderReqlbl.Visible = true;
            chckTaxExempt.Enabled = false;

            GridViewRow row = grvSOInvoiceList.SelectedRow;

            Session["idSOHeader"] = row.Cells[0].Text.Trim();

            var soheader = SO_Header.RetrieveData(oCon, row.Cells[1].Text.Trim(), false);

            Session["idCustomer"] = soheader[0].idCustomer;
            lblSONumber.Text = soheader[0].SO_Number;
            txtOrderDate_.Text = soheader[0].Order_Date.ToShortDateString();
            txtDueDate.Text = soheader[0].Due_Date.ToShortDateString();
            txtDueDate_.Text = soheader[0].Due_Date.ToShortDateString();
            ddSite.SelectedValue = soheader[0].idSite.ToString();
            txtSite.Text = ddSite.SelectedItem.Text;

            double output_vat = double.Parse(soheader[0].Tax_Amount.ToString());

            if (output_vat == 0.00)
            {
                chckTaxExempt.Checked = true;
            }
            else
            {
                chckTaxExempt.Checked = false;
            }

            SO_Creation model = new SO_Creation();
            string salesman = model.GetSalesmanByID(int.Parse(soheader[0].Salesman), oCon);
            txtSalesman.Text = salesman;
            txtCustPONum.Text = soheader[0].Customer_PO;
            double freight_chrgs = double.Parse(soheader[0].Freight_Charges.ToString());
            txtFreightCharges.Text = freight_chrgs.ToString("n", CultureInfo.GetCultureInfo("en-US"));
            double other_chrgs = double.Parse(soheader[0].Other_Charges.ToString());
            txtOtherCharges.Text = other_chrgs.ToString("n", CultureInfo.GetCultureInfo("en-US"));
            double gross_amt = double.Parse(soheader[0].Gross_Amount.ToString());
            txtGrossAmt.Text = gross_amt.ToString("n", CultureInfo.GetCultureInfo("en-US"));
            double net_amt = double.Parse(soheader[0].Net_Amount.ToString());
            txtNetAmt.Text = net_amt.ToString("n", CultureInfo.GetCultureInfo("en-US")); ;
            txtOutputVat.Text = output_vat.ToString("n", CultureInfo.GetCultureInfo("en-US"));
            txtRemarks.Text = Server.HtmlDecode(soheader[0].Remarks);
            txtCurrency.Text = soheader[0].currency_code;
            txtEndUser.Text = soheader[0].End_User;
            txtEndUserCity.Text = soheader[0].End_User_City;
            txtStockStatus.Text = soheader[0].Stock_Status;
            Session["SOStatus"] = soheader[0].SO_Status;
            txtSpecialConc.Text = soheader[0].Special_Concession;

            var customerdetails = Customer_Details.RetrieveData(oCon, soheader[0].idCustomer, "");
            txtCustomerCode.Text = customerdetails[0].Customer_Code;
            Session["CompanyName"] = customerdetails[0].Company_Name;
            txtCustomerName.Text = customerdetails[0].Company_Name;
            txtCreditTerm.Text = customerdetails[0].credit_term;
            Session["Address1"] = customerdetails[0].Address1;
            Session["Address2"] = customerdetails[0].Address2;
            Session["Address3"] = customerdetails[0].Address3;
            Session["Address4"] = customerdetails[0].Address4;
            Session["AddressShipping1"] = customerdetails[0].AddressShipping1;
            Session["AddressShipping2"] = customerdetails[0].AddressShipping2;
            Session["AddressShipping3"] = customerdetails[0].AddressShipping3;
            Session["AddressShipping4"] = customerdetails[0].AddressShipping4;

            if (row.Cells[2].Text.Trim() != "" && row.Cells[2].Text.Trim() != "&nbsp;")
            {
                txtInvoiceDate.TextMode = TextBoxMode.SingleLine;
                var invoicedetails = Invoice.RetrieveData(oCon, row.Cells[2].Text, row.Cells[1].Text);
                txtInvoiceNumber.Text = invoicedetails[0].Invoice_Number;
                txtInvoiceDate.Text = invoicedetails[0].Invoice_Date.ToShortDateString();
                txtDelDate.Text = invoicedetails[0].Del_Date.ToShortDateString();
                txtInvoiceAmount.Text = invoicedetails[0].Amount.ToString();
                txtDRNumber.Text = invoicedetails[0].DR_Number;
                txtORNumber.Text = invoicedetails[0].OR_Number;
                txtInvoiceNumber.ReadOnly = true;
                txtInvoiceDate.ReadOnly = true;
                txtInvoiceAmount.ReadOnly = true;
                txtDRNumber.ReadOnly = true;
                btnSave.Visible = false;
            }
            else
            {
                txtInvoiceNumber.Text = "";
                txtInvoiceDate.Text = "";
                txtInvoiceAmount.Text = "";
                txtDRNumber.Text = "";
                txtInvoiceDate.TextMode = TextBoxMode.Date;
                txtInvoiceNumber.ReadOnly = false;
                txtInvoiceDate.ReadOnly = false;
                txtInvoiceAmount.ReadOnly = false;
                txtDRNumber.ReadOnly = false;
                btnSave.Visible = true;
            }

            GetSavedItems();

            txtCustomerCode.Visible = true;
            ddSalesman.Visible = false;
            txtOrderDate.ReadOnly = true;
            txtCustPONum.ReadOnly = true;
            txtFreightCharges.ReadOnly = true;
            txtOtherCharges.ReadOnly = true;
            txtOrderDate_.Visible = true;
            txtOrderDate.Visible = false;
            txtDueDate.Visible = false;
            txtDueDate.ReadOnly = true;
            txtDueDate_.Visible = true;
            txtDueDate_.ReadOnly = true;
            txtEndUser.ReadOnly = true;
            txtEndUserCity.ReadOnly = true;
            txtCreditTerm.Visible = true;
            txtCurrency.Visible = true;
            txtSalesman.Visible = true;
            txtStockStatus.Visible = true;
            txtOrderDate_.ReadOnly = true;
            txtCreditTerm.ReadOnly = true;
            txtCustomerCode.ReadOnly = true;
            txtSite.ReadOnly = true;
            txtCurrency.ReadOnly = true;
            txtSalesman.ReadOnly = true;
            txtStockStatus.ReadOnly = true;
            txtRemarks.ReadOnly = true;
            txtSpecialConc.ReadOnly = true;
            lblSONumber.Visible = true;
            lblSONumber_.Visible = true;
            ddSite.Visible = false;
            txtSite.Visible = true;
            //gvItems.Enabled = false;

            lblOrderDate.Visible = true;
            lblOrderDate.Text = "Order Date (dd/mm/yyyy)";
            lblDueDate.Text = "Due Date (dd/mm/yyyy)";

        }

        protected void grvSOInvoiceList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grvSOInvoiceList, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click To Select this row";
            }
        }

        protected void grvSOInvoiceList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvSOInvoiceList.PageIndex = e.NewPageIndex;
            getSOInvoiceList("");
        }

        //protected void GetSOtoGrid_()
        //{
        //    string access = Session["User_Access"].ToString();

        //    if (access == "AE")
        //    {
        //        var ldata = SO_Creation.RetrieveSO(oCon, txtSearch.Text.Trim(), Session["User_Name"].ToString(), Session["User_Access"].ToString(), ddlSOStatus.SelectedValue.ToString());
        //        grvMainSO.DataSource = ldata;
        //    }
        //    else if (access == "IT" || access == "BCC" || access == "PM")
        //    {
        //        var ldata = SO_Creation.RetrieveSO(oCon, txtSearch.Text.Trim(), Session["User_Name"].ToString(), "BCC", ddlSOStatus.SelectedValue.ToString());
        //        grvMainSO.DataSource = ldata;
        //    }

        //    grvMainSO.DataBind();
        //    grvMainSO.GridLines = GridLines.None;
        //}

        protected void GetSavedItems()
        {
            var oSOCreation = new clsSOCreation();
            var ldata = oSOCreation.RetrieveItemsByidSOHeader(int.Parse(Session["idSOHeader"].ToString()));

            gvItems.DataSource = null;
            gvItems.DataBind();

            gvItems.DataSource = ldata;
            gvItems.DataBind();
            gvItems.GridLines = GridLines.None;
        }

        protected void GetSiteToDD()
        {
            var lData = Site_Loc.RetrieveData(oCon, "");

            ddSite.Items.Clear();
            ddSite.DataSource = lData;
            ddSite.DataTextField = "Site_Desc";
            ddSite.DataValueField = "idSite";

            ddSite.DataBind();
        }

        protected void defaultSettings()
        {
            InvoicePanel.Visible = true;
            pnlAllDetails.Visible = false;
            tblSearch.Visible = true;
            btnBack.Visible = false;
            btnSave.Visible = false;
            myModalWarning.Visible = false;
            grossamt.Visible = false;
            netamt.Visible = false;
            outputvat.Visible = false;
            lblSONumber.Visible = false;
            lblSONumber_.Visible = false;

            Session["idSOHeader"] = "";
            txtDueDate.Text = "";
            txtCustPONum.Text = "";
            txtCustomerCode.Text = "";
            txtFreightCharges.Text = "";
            txtOtherCharges.Text = "";
            txtSite.Text = "";
            txtRemarks.Text = "";
            txtCurrency.Text = "";
            txtCustomerName.Text = "";
            txtSpecialConc.Text = "";

            txtDueDate_.Visible = false;
            txtDueDate.ReadOnly = false;
            txtCustPONum.ReadOnly = false;
            txtFreightCharges.ReadOnly = false;
            txtOtherCharges.ReadOnly = false;
            txtDueDate.TextMode = TextBoxMode.Date;
            txtGrossAmt.ReadOnly = true;
            txtNetAmt.ReadOnly = true;
            txtOutputVat.ReadOnly = true;
            txtSite.ReadOnly = false;
            txtRemarks.ReadOnly = false;
            txtSite.Visible = false;
            ddSite.Visible = true;
            txtCurrency.Visible = false;
            txtStockStatus.Visible = false;
            chckTaxExempt.Checked = false;

            txtCustomerCode.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            chckTaxExempt.Checked = false;

            Session["Mode"] = "";

            var oSOCreation = new clsSOCreation();
            decimal Tax = oSOCreation.GetTaxFromDB();
            Session["Tax"] = Convert.ToString(Tax);

            Session.Remove("Data");

            Session.Remove("idSOHeader");
            Session.Remove("idCustomer");
            Session.Remove("ItemNo");
            Session.Remove("ItemQty");
            Session.Remove("CurrentAllocQty");
            Session.Remove("CurrentTotalQty");
            Session.Remove("Description");
            Session.Remove("Mode");
            Session.Remove("Address1");
            Session.Remove("Address2");
            Session.Remove("Address3");
            Session.Remove("Address4");
            Session.Remove("CompanyName");
            Session.Remove("SOStatus");

            gvItems.DataSource = null;
            gvItems.DataBind();

            for (int x = Session.Count - 1; x >= 6; x--)
            {
                Session.RemoveAt(x);
            }
        }

        protected void clearFlds()
        {
            ddSite.SelectedValue = "2";
            txtCreditTerm.Text = "";
            txtDueDate.Text = "";
            txtCustPONum.Text = "";
            txtFreightCharges.Text = "";
            txtOtherCharges.Text = "";
            txtInvoiceNumber.Text = "";
            txtInvoiceDate.Text = "";
            txtInvoiceAmount.Text = "";
            txtDRNumber.Text = "";
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            getSOInvoiceList(txtSearch.Text);
        }

        protected void btnCloseWarning_Click(object sender, EventArgs e)
        {
            myModalWarning.Visible = false;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            defaultSettings();
            clearFlds();
            getSOInvoiceList(txtSearch.Text);

        }

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var lnkRemove = (LinkButton)e.Row.FindControl("lnkRemove");
            var lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
            var lnkEdit = (LinkButton)e.Row.FindControl("lnkEdit");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
                if (Session["Mode"].ToString() == "Edit") //EDIT
                {

                }
                else if (Session["Mode"].ToString() == "View") //VIEW ONLY
                {

                }
                if (Session["Mode"].ToString() == "Add") //ADD
                {
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[10].Visible = false;
                }

            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
            }

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (validateFields() == true)
            {

                Invoice_Model invmdl = new Invoice_Model();
                invmdl.idSOHeader = Convert.ToInt32(Session["idSOHeader"]);
                invmdl.Invoice_Number = txtInvoiceNumber.Text;
                invmdl.Invoice_Date = Convert.ToDateTime(txtInvoiceDate.Text);
                invmdl.Amount = Convert.ToDecimal(txtInvoiceAmount.Text.Replace(",",""));
                invmdl.DR_Number = txtDRNumber.Text;
                invmdl.Del_Date = Convert.ToDateTime(txtDelDate.Text);
                invmdl.OR_Number = txtORNumber.Text;

                if (Invoice.Save(oCon, invmdl) == true)
                {
                    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Saved Succesfully!" + ControlChars.Quote + ");</script>");
                    defaultSettings();
                    clearFlds();
                    getSOInvoiceList(txtSearch.Text);
                }
                else
                {
                    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Error in saving." + ControlChars.Quote + ");</script>");
                }

            }
            else
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Please complete all fields." + ControlChars.Quote + ");</script>");
            }
        }

        private bool validateFields()
        {
            bool returnvalue;

            if (txtInvoiceNumber.Text == "")
                returnvalue = false;
            else
                returnvalue = true;

            if (txtInvoiceDate.Text == "")
                returnvalue = false;
            else
                returnvalue = true;

            if (txtInvoiceAmount.Text == "")
                returnvalue = false;
            else
                returnvalue = true;

            if (txtDRNumber.Text == "")
                returnvalue = false;
            else
                returnvalue = true;
            decimal invamt = Convert.ToDecimal(txtInvoiceAmount.Text.Replace(",", "").Replace(".", ""));
            decimal grssamt = Convert.ToDecimal(txtGrossAmt.Text.Replace(",", "").Replace(".", ""));
            if (invamt != grssamt)
                returnvalue = false;
            else
                returnvalue = true;

            return returnvalue;
        }
    }
}