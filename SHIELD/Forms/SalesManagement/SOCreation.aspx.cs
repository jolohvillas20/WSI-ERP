using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using POSOINV.Functions;
using POSOINV.Models;
using System.Data;
using System.Globalization;
using Microsoft.Reporting.WebForms;
using System.Web.Services;
using System.Net;
using System.IO;
using System.Web;
using System.Configuration;
using ClosedXML.Excel;

namespace SHIELD.ERP
{
    public partial class SOCreation : System.Web.UI.Page
    {
        SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {

            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            //lblDomainLogin.Text = System.Convert.ToString(Session.Contents["Login"]);
            //Session["User_Name"].ToString() = System.Convert.ToString(Session.Contents["FName"]) + " " + System.Convert.ToString(Session.Contents["LName"]);

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

            if (!Page.IsPostBack)
            {
                for (int x = Session.Count - 1; x >= 7; x--)
                {
                    Session.RemoveAt(x);
                }

                Session["idProduct"] = Users.GetidProduct((int)Session["idUser"], oCon);

                if (username != "")
                {
                    if (access == "AE" || access == "PM" || access == "IT" || access == "BCC" || access == "AU")
                    {
                        ddlSOStatus.SelectedValue = "Open";
                        defaultSettings();
                        GetSOtoGrid_();
                        GetItemMaster();
                        GetCustomerListGv();
                        GetCreditTermToDD();
                        GetSiteToDD();
                        GetCurrencyToDD();
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
        protected void grvMainSO_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["Mode"] = "View";
            btnAdd.Visible = false;

            btnDownloadSalesReport.Visible = false;
            btnShowItemSOModal.Visible = false;
            btnBack.Visible = true;
            btnSave.Visible = true;
            SOPanel.Visible = false;
            pnlAllDetails.Visible = true;
            btnAddNew.Visible = false;
            //btnPrint.Visible = true;
            tblSearch.Visible = false;
            tblSort.Visible = false;
            grossamt.Visible = true;
            outputvat.Visible = true;
            netamt.Visible = true;
            pnlOrderReqlbl.Visible = true;
            chckTaxExempt.Enabled = false;

            //var oSOCreation = new clsSOCreation();
            GridViewRow row = grvMainSO.SelectedRow;
            string chckidSite = row.Cells[18].Text.Trim();
            string chckfreightchrge = row.Cells[8].Text.Trim();
            string chckotherchrge = row.Cells[9].Text.Trim();
            string chckcreditterm = row.Cells[12].Text.Trim();
            string chckgrossamt = row.Cells[13].Text.Trim();
            double net_amt = double.Parse(row.Cells[14].Text);
            double output_vat = double.Parse(row.Cells[15].Text);
            string chckcurrency = row.Cells[18].Text.Trim();
            string chckspecialconc = row.Cells[24].Text.Trim();
            string chckStockStatus = row.Cells[27].Text.Trim();

            if (output_vat == 0.00)
            {
                chckTaxExempt.Checked = true;
            }
            else
            {
                chckTaxExempt.Checked = false;
            }

            Session["idSOHeader"] = row.Cells[0].Text.Trim();
            Session["idCustomer"] = row.Cells[1].Text.Trim();
            lblSONumber.Text = row.Cells[2].Text.Trim();
            txtOrderDate_.Text = row.Cells[3].Text.Trim();
            txtDueDate.Text = row.Cells[4].Text.Trim();
            txtDueDate_.Text = row.Cells[4].Text.Trim();

            string access = "";
            access = Session["User_Access"].ToString();
            GetUserDetailsToDD();

            if ((int)Session["idProduct"] != 0 && access == "PM" || access == "PA")
            {
                ddSalesman.SelectedValue = row.Cells[5].Text.Trim();
                txtSalesman.Text = ddSalesman.SelectedItem.ToString();
            }

            if (access == "BCC" || access == "AE" || (int)Session["idProduct"] == 0)
            {

                SO_Creation model = new SO_Creation();
                string salesman = model.GetSalesmanByID(int.Parse(row.Cells[5].Text), oCon);
                ddSalesman.SelectedValue = row.Cells[5].Text;
                txtSalesman.Text = salesman;
            }

            //txtCustomerName.Text = row.Cells[6].Text.Trim();
            txtCustomerName.Text = row.Cells[25].Text.Trim();

            txtCustPONum.Text = row.Cells[7].Text.Trim();
            if (chckfreightchrge != "&nbsp;")
            {
                double freight_chrgs = double.Parse(row.Cells[8].Text);
                txtFreightCharges.Text = freight_chrgs.ToString("n", CultureInfo.GetCultureInfo("en-US"));
            }
            if (chckotherchrge != "&nbsp;")
            {
                double other_chrgs = double.Parse(row.Cells[9].Text);
                txtOtherCharges.Text = other_chrgs.ToString("n", CultureInfo.GetCultureInfo("en-US"));
            }
            txtCustomerCode.Text = row.Cells[11].Text.Trim();
            if (chckcreditterm != "&nbsp;")
            {
                txtCreditTerm.Text = row.Cells[12].Text.Trim();
                ddCreditTerm.SelectedValue = txtCreditTerm.Text.Trim();
            }
            if (chckgrossamt != "&nbsp;")
            {
                double gross_amt = double.Parse(row.Cells[13].Text);
                txtGrossAmt.Text = gross_amt.ToString("n", CultureInfo.GetCultureInfo("en-US"));
            }
            txtNetAmt.Text = net_amt.ToString("n", CultureInfo.GetCultureInfo("en-US")); ;
            txtOutputVat.Text = output_vat.ToString("n", CultureInfo.GetCultureInfo("en-US"));
            if (chckidSite != "&nbsp;")
            {
                //string Site_Desc = oSOCreation.GetSiteDescByID(int.Parse(row.Cells[16].Text));
                //txtSite.Text = Site_Desc;
                //ddSite.SelectedValue = row.Cells[16].Text.Trim();
                ddSite.SelectedValue = row.Cells[16].Text.Trim();
                txtSite.Text = ddSite.SelectedItem.Text;
            }
            txtRemarks.Text = Server.HtmlDecode(row.Cells[17].Text);
            if (chckcurrency != "&nbsp;")
            {
                ddCurrency.SelectedValue = row.Cells[18].Text.Trim();
                txtCurrency.Text = ddCurrency.SelectedValue;
            }
            //lblFinalDiscount.Text = row.Cells[19].Text.Trim();
            Session["Address1"] = row.Cells[20].Text.Trim();
            Session["Address2"] = row.Cells[21].Text.Trim();
            Session["Address3"] = row.Cells[22].Text.Trim();
            Session["Address4"] = row.Cells[23].Text.Trim();
            Session["AddressShipping1"] = row.Cells[28].Text.Trim();
            Session["AddressShipping2"] = row.Cells[29].Text.Trim();
            Session["AddressShipping3"] = row.Cells[30].Text.Trim();
            Session["AddressShipping4"] = row.Cells[31].Text.Trim();

            txtEndUser.Text = row.Cells[35].Text;
            txtEndUserCity.Text = row.Cells[36].Text;

            if (chckspecialconc == "&nbsp;")
            {
                txtSpecialConc.Text = "";
            }
            else
            {
                txtSpecialConc.Text = row.Cells[24].Text.Trim();
            }

            if (chckStockStatus != "&nbsp;")
            {
                txtStockStatus.Text = row.Cells[27].Text.Trim();
                ddStockStatus.SelectedValue = row.Cells[27].Text.Trim();
            }

            Session["CompanyName"] = row.Cells[25].Text.Trim();
            Session["SOStatus"] = row.Cells[26].Text.Trim();

            //string PO_Number = oSOCreation.GetPONumberByID(int.Parse(lblidSOHeader.Text));

            if (Session["SOStatus"].ToString() == "Open")
            {
                //btnPrintSO.Visible = true;
                btnEdit.Visible = true;
                access = Session["User_Access"].ToString();
                if (access == "BCC" || access == "IT" || access == "PM" || access == "AU")
                {
                    //btnPrintSO.Visible = true;
                    btnDelete.Visible = true;
                }
                else
                {
                    btnDelete.Visible = false;
                    //btnPrintSO.Visible = false;
                    //btnEdit.Visible = false;
                }

            }
            else
            {
                //access = Session["User_Access"].ToString();

                btnEdit.Visible = false;
                //if (access == "BCC" || access == "IT" || access == "PM")
                //{
                //    //btnPrintSO.Visible = true;
                //    btnEdit.Visible = true;
                //}
                //else
                //{
                //    //btnPrintSO.Visible = false;
                //    //btnEdit.Visible = false;
                //}
            }
            btnPrintSO.Visible = true;

            GetSavedItems();

            txtCustomerCode.Visible = true;
            ddCreditTerm.Visible = false;
            ddCurrency.Visible = false;
            ddSalesman.Visible = false;
            ddStockStatus.Visible = false;
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
            lnkBtnSearchCust.Enabled = false;
            lblSONumber.Visible = true;
            lblSONumber_.Visible = true;
            ddSite.Visible = false;
            txtSite.Visible = true;
            //gvItems.Enabled = false;
            btnSave.Visible = false;

            lblOrderDate.Visible = true;
            lblOrderDate.Text = "Order Date (dd/mm/yyyy)";
            lblDueDate.Text = "Due Date (dd/mm/yyyy)";

        }
        protected void grvCustomerDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = grvCustomerDetails.SelectedRow;
            Session["idCustomer"] = row.Cells[1].Text.Trim();
            txtCustomerCode.Text = row.Cells[2].Text.Trim();
            txtCustomerName.Text = row.Cells[4].Text.Trim();
            Session["CompanyName"] = row.Cells[4].Text.Trim();
            Session["Address1"] = row.Cells[5].Text.Trim();
            Session["Address2"] = row.Cells[6].Text.Trim();
            Session["Address3"] = row.Cells[7].Text.Trim();
            Session["Address4"] = row.Cells[8].Text.Trim();
            Session["AddressShipping1"] = row.Cells[9].Text.Trim();
            Session["AddressShipping2"] = row.Cells[10].Text.Trim();
            Session["AddressShipping3"] = row.Cells[11].Text.Trim();
            Session["AddressShipping4"] = row.Cells[12].Text.Trim();
            txtCreditTerm.Text = row.Cells[13].Text;
            myModalCustomerDetails.Visible = false;
        }
        protected void grvCustomerDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grvCustomerDetails, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.ToolTip = "Click to select this row.";
            }
        }
        protected void grvMainSO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false;
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[14].Visible = false;
                e.Row.Cells[15].Visible = false;
                e.Row.Cells[16].Visible = false;
                e.Row.Cells[17].Visible = false;
                e.Row.Cells[18].Visible = false;
                e.Row.Cells[19].Visible = false;
                e.Row.Cells[20].Visible = false;
                e.Row.Cells[21].Visible = false;
                e.Row.Cells[22].Visible = false;
                e.Row.Cells[23].Visible = false;
                e.Row.Cells[24].Visible = false;
                e.Row.Cells[27].Visible = false;
                e.Row.Cells[28].Visible = false;
                e.Row.Cells[29].Visible = false;
                e.Row.Cells[30].Visible = false;
                e.Row.Cells[31].Visible = false;
                e.Row.Cells[35].Visible = false;
                e.Row.Cells[36].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false;
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[14].Visible = false;
                e.Row.Cells[15].Visible = false;
                e.Row.Cells[16].Visible = false;
                e.Row.Cells[17].Visible = false;
                e.Row.Cells[18].Visible = false;
                e.Row.Cells[19].Visible = false;
                e.Row.Cells[20].Visible = false;
                e.Row.Cells[21].Visible = false;
                e.Row.Cells[22].Visible = false;
                e.Row.Cells[23].Visible = false;
                e.Row.Cells[24].Visible = false;
                e.Row.Cells[27].Visible = false;
                e.Row.Cells[28].Visible = false;
                e.Row.Cells[29].Visible = false;
                e.Row.Cells[30].Visible = false;
                e.Row.Cells[31].Visible = false;
                e.Row.Cells[35].Visible = false;
                e.Row.Cells[36].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grvMainSO, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.ToolTip = "Click to select this row.";
            }
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
                    lnkRemove.Visible = false;
                    lnkDelete.Visible = true;
                    lnkEdit.Visible = true;
                }
                else if (Session["Mode"].ToString() == "View") //VIEW ONLY
                {
                    lnkRemove.Visible = false;
                    lnkDelete.Visible = false;
                    lnkEdit.Visible = false;
                }
                if (Session["Mode"].ToString() == "Add") //ADD
                {
                    lnkRemove.Visible = true;
                    lnkDelete.Visible = false;
                    lnkEdit.Visible = false;
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
        protected void grvMainSO_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvMainSO.PageIndex = e.NewPageIndex;
            GetSOtoGrid_();
        }
        protected void grvCustomerDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvCustomerDetails.PageIndex = e.NewPageIndex;
            GetCustomerListGv();
        }
        protected void grvItemClass_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvItemClass.PageIndex = e.NewPageIndex;
            GetItemMaster();
        }
        protected void grvItemClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = grvItemClass.SelectedRow;

            lblidItem.Text = row.Cells[1].Text.Trim();
            txtItemName.Text = row.Cells[3].Text.Trim();
            txtItemNo.Text = row.Cells[4].Text.Trim();
            txtDescription.Text = row.Cells[5].Text.Trim();
            //Session["ItemNo"] = row.Cells[4].Text.Trim();
            //Session["Description"] = row.Cells[5].Text.Trim();
            txtUM.Text = row.Cells[6].Text.Trim();

            txtItemName.ReadOnly = true;
            txtItemNo.ReadOnly = true;
            txtDescription.ReadOnly = true;
            myModal2.Visible = true;

            txtQuantity.Text = "";
            txtCost.Text = "";
            txtDiscount.Text = "";
        }
        protected void grvItemClass_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[6].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[6].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grvItemClass, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.ToolTip = "Click to select this row.";
            }
        }
        protected void GetSOtoGrid_()
        {
            string access = Session["User_Access"].ToString();

            if (access == "AE")
            {
                var ldata = SO_Creation.RetrieveSO(oCon, txtSearch.Text.Trim(), Session["User_Name"].ToString(), Session["User_Access"].ToString(), ddlSOStatus.SelectedValue.ToString());
                grvMainSO.DataSource = ldata;
            }
            else if (access == "IT" || access == "BCC" || access == "PM" || access == "AU")
            {
                var ldata = SO_Creation.RetrieveSO(oCon, txtSearch.Text.Trim(), Session["User_Name"].ToString(), "BCC", ddlSOStatus.SelectedValue.ToString());
                grvMainSO.DataSource = ldata;
            }

            grvMainSO.DataBind();
            grvMainSO.GridLines = GridLines.None;
        }
        protected void GetItemMaster()
        {
            var oSOCreation = new clsSOCreation();
            var ldata = oSOCreation.RetrieveItemMaster(ddItemSearchItem.Text.Trim(), txtSearchItem.Text,ddlItemSite.SelectedValue.ToString());

            grvItemClass.DataSource = null;
            grvItemClass.DataBind();

            grvItemClass.DataSource = ldata;
            grvItemClass.DataBind();
            grvItemClass.GridLines = GridLines.None;
        }
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
        protected void GetCustomerListGv()
        {
            var oSOCreation = new clsSOCreation();
            var ldata = oSOCreation.RetrieveCustomerCodeList(ddSearchByCustomer.Text.Trim(), txtSearchCustomer.Text);

            grvCustomerDetails.DataSource = null;
            grvCustomerDetails.DataBind();

            grvCustomerDetails.DataSource = ldata;
            grvCustomerDetails.DataBind();
            grvCustomerDetails.GridLines = GridLines.None;
        }
        protected void GetCreditTermToDD()
        {
            clsSOCreation oSOCreation = new clsSOCreation();
            List<clsSOCreation> lData = oSOCreation.RetrieveCreditTerm();

            ddCreditTerm.Items.Clear();
            ddCreditTerm.DataSource = lData;
            ddCreditTerm.DataTextField = "credit_term";
            ddCreditTerm.DataValueField = "credit_term";

            ddCreditTerm.DataBind();
        }
        protected void GetUserDetailsToDD()
        {
            var lData = Users.dropDownUser(oCon, Convert.ToInt32(Session["idProduct"].ToString()));

            ddSalesman.Items.Clear();
            ddSalesman.DataSource = lData;
            ddSalesman.DataTextField = "User_Name";
            ddSalesman.DataValueField = "idUser";

            ddSalesman.DataBind();
        }
        protected void GetCurrencyToDD()
        {
            var dt = Currency_Code.RetrieveData(oCon);

            ddCurrency.Items.Clear();
            ddCurrency.DataSource = dt;
            ddCurrency.DataTextField = "Currency_Name";
            ddCurrency.DataValueField = "Currency_Code";

            ddCurrency.DataBind();
        }
        protected void GetSiteToDD()
        {
            var lData = Site_Loc.RetrieveData(oCon, "");

            ddSite.Items.Clear();
            ddSite.DataSource = lData;
            ddSite.DataTextField = "Site_Desc";
            ddSite.DataValueField = "idSite";
            ddSite.DataBind();

            ddlItemSite.DataSource = lData;
            ddlItemSite.DataTextField = "Site_Name";
            ddlItemSite.DataValueField = "idSite";
            ddlItemSite.DataBind();
        }
        protected void defaultSettings()
        {
            if (Session["User_Access"].ToString() == "PM" || Session["User_Access"].ToString() == "IT" || Session["User_Access"].ToString() == "BCC" || Session["User_Access"].ToString() == "AU")
            {
                btnDownloadSalesReport.Visible = true;
                btnShowItemSOModal.Visible = true;
            }
            else
            {
                btnDownloadSalesReport.Visible = false;
                btnShowItemSOModal.Visible = false;
            }

            for (int x = Session.Count - 1; x >= 7; x--)
            {
                Session.RemoveAt(x);
            }

            SOPanel.Visible = true;
            pnlAllDetails.Visible = false;
            btnAddNew.Visible = true;
            tblSearch.Visible = true;
            tblSort.Visible = true;
            btnBack.Visible = false;
            btnSave.Visible = false;
            btnEdit.Visible = false;
            myModalWarning.Visible = false;
            btnPrintSO.Visible = false;
            grossamt.Visible = false;
            netamt.Visible = false;
            outputvat.Visible = false;
            lblSONumber.Visible = false;
            lblSONumber_.Visible = false;

            Session["idSOHeader"] = "";
            txtDueDate.Text = "";
            txtCustPONum.Text = "";
            GetCustomerListGv();
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
            ddCurrency.Visible = true;
            txtStockStatus.Visible = false;
            ddStockStatus.Visible = true;
            chckTaxExempt.Checked = false;

            txtCustomerCode.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            lnkBtnSearchCust.Enabled = true;
            chckTaxExempt.Checked = false;

            var oSOCreation = new clsSOCreation();
            decimal Tax = oSOCreation.GetTaxFromDB();
            Session["Tax"] = Convert.ToString(Tax);

            gvItems.DataSource = null;
            gvItems.DataBind();

            string access = "";
            access = Session["User_Access"].ToString();

            if (access == "BCC")
            {
                btnAddNew.Visible = false;
            }
            btnDelete.Visible = false;
        }
        protected void clearFlds()
        {
            ddSite.SelectedValue = "2";
            txtCreditTerm.Text = "";
            txtDueDate.Text = "";
            txtCustPONum.Text = "";
            txtItemName.Text = "";
            txtQuantity.Text = "";
            txtCost.Text = "";
            txtFreightCharges.Text = "";
            txtOtherCharges.Text = "";
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetSOtoGrid_();
        }
        private void ddItemSearchItem_SelectedIndexChanged(object sender)
        {
            GetItemMaster();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetSOtoGrid_();
        }
        protected void btnSearchItem_Click(object sender, EventArgs e)
        {
            GetItemMaster();
        }
        protected void lnkBtnSearchCustomer_Click(object sender, EventArgs e)
        {
            GetCustomerListGv();
        }
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            for (int x = Session.Count - 1; x >= 7; x--)
            {
                Session.RemoveAt(x);
            }

            Session["Mode"] = "Add";
            btnDownloadSalesReport.Visible = false;
            btnShowItemSOModal.Visible = false;
            SOPanel.Visible = false;
            pnlAllDetails.Visible = true;
            btnAddNew.Visible = false;
            tblSearch.Visible = false;
            tblSort.Visible = false;
            btnBack.Visible = true;
            btnSave.Visible = true;
            btnAdd.Visible = true;
            txtDueDate.ReadOnly = false;
            txtCustPONum.ReadOnly = false;
            txtFreightCharges.ReadOnly = false;
            txtOtherCharges.ReadOnly = false;
            txtSpecialConc.ReadOnly = false;
            ddCreditTerm.Visible = true;
            txtCreditTerm.Visible = true;
            ddCreditTerm.Visible = false;
            pnlOrderReqlbl.Visible = false;
            clearFlds();
            ddCurrency.SelectedValue = "PHP";
            //gvItems.Enabled = true;
            txtDueDate.Visible = true;
            txtOrderDate.Visible = false;
            txtOrderDate_.Visible = false;
            lblOrderDate.Visible = false;
            lblOrderDate.Text = "Order Date";
            lblDueDate.Text = "Due Date";
            chckTaxExempt.Checked = false;
            chckTaxExempt.Enabled = true;
            txtCreditTerm.ReadOnly = true;

            string access = "";
            access = Session["User_Access"].ToString();

            if (access == "PM" || access == "PA")
            {
                GetUserDetailsToDD();
                ddSalesman.Visible = true;
                txtSalesman.Visible = false;
            }
            else
            {
                txtSalesman.Text = Session["User_Name"].ToString();
                ddSalesman.Visible = false;
            }

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("idItem"));
            dt.Columns.Add(new DataColumn("Item_Number"));
            dt.Columns.Add(new DataColumn("Description"));
            dt.Columns.Add(new DataColumn("Qty"));
            dt.Columns.Add(new DataColumn("Cost"));
            dt.Columns.Add(new DataColumn("UM"));
            dt.Columns.Add(new DataColumn("Discount"));
            dt.Columns.Add(new DataColumn("Tax_Amount"));
            dt.Columns.Add(new DataColumn("Amount"));
            dt.Columns.Add(new DataColumn("Item_Status"));
            dt.Columns.Add(new DataColumn("idSODetail"));

            gvItems.DataSource = dt;
            gvItems.DataBind();

            Session.Remove("Data");
            Session["Data"] = dt;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            var oSOCreation = new clsSOCreation();

            if (txtCustPONum.Text == "" || txtCustomerCode.Text == "" || gvItems.Rows.Count == 0 || txtDueDate.Text == "")
            {
                myModalWarning.Visible = true;
                lblItemWarning.Text = "Please fill all required fields.";
            }
            else
            {

                if (txtFreightCharges.Text == "")
                {
                    txtFreightCharges.Text = "0";
                }
                if (txtOtherCharges.Text == "")
                {
                    txtOtherCharges.Text = "0";
                }
                decimal grossamount = gvItems.Rows.Cast<GridViewRow>().Sum(t => Convert.ToDecimal(t.Cells[8].Text));
                decimal finaldiscount = gvItems.Rows.Cast<GridViewRow>().Sum(t => Convert.ToDecimal(t.Cells[6].Text));
                decimal taxamount = 0;

                if (chckTaxExempt.Checked == true)
                {
                    taxamount = 0;
                }
                else
                {
                    taxamount = grossamount * Convert.ToDecimal(1 - (1 / 1.12));
                }

                decimal netamount = grossamount - decimal.Parse(txtFreightCharges.Text) - decimal.Parse(txtOtherCharges.Text) - taxamount;

                string idSO = SO_Header.GetLastSONumber(oCon);
                string so_number = "";
                if (idSO == null)
                {
                    idSO = "SJ000000";
                }
                idSO = idSO.Substring(2);
                int idSO_ = int.Parse(idSO) + 1;

                so_number = "SJ" + idSO_.ToString("000000");

                var transactionID = Guid.NewGuid();

                string access = "";
                access = Session["User_Access"].ToString();
                var objSOHeader = new SO_Header_Model();
                if (Session["Mode"].ToString() == "Add")
                {
                    objSOHeader.SO_Number = so_number;
                    objSOHeader.Due_Date = DateTime.Parse(txtDueDate.Text);

                    if (access == "PM" || access == "PA" || access == "BCC")
                    {
                        objSOHeader.Salesman = ddSalesman.SelectedValue.ToString();
                    }
                    //else if (access == "BCC" || access == "IT")
                    //{
                    //    string idUser = row.Cells[5].Text.Trim();
                    //    objSOHeader.Salesman = idUser;
                    //}
                    else if (access == "AE" || access == "IT")
                    {
                        objSOHeader.Salesman = Session["idUser"].ToString();
                    }
                }
                else if (Session["Mode"].ToString() == "Edit")
                {
                    objSOHeader.idSOHeader = int.Parse(Session["idSOHeader"].ToString());
                    objSOHeader.SO_Number = lblSONumber.Text.Trim();
                    objSOHeader.Due_Date = DateTime.ParseExact(txtDueDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (access == "PM" || access == "PA" || access == "BCC" || access == "IT")
                    {
                        objSOHeader.Salesman = ddSalesman.SelectedValue.ToString();
                        //string idUser = row.Cells[5].Text.Trim();
                        //objSOHeader.Salesman = idUser;
                    }
                    //else if (access == "BCC" || access == "IT")
                    //{

                    //}
                    else if (access == "AE")
                    {
                        objSOHeader.Salesman = Session["idUser"].ToString();
                    }
                }
                if (ddCurrency.SelectedValue.ToString() == "PHP")
                {
                    objSOHeader.Forex_Rate = 0;
                }
                else
                {
                    decimal forex_rate = 0;
                    forex_rate = Forex_Rate.RetrieveData(oCon, ddCurrency.SelectedValue.ToString());
                    objSOHeader.Forex_Rate = forex_rate;
                }

                objSOHeader.Customer_PO = txtCustPONum.Text.Trim();

                if (ddCreditTerm.Visible == true)
                {
                    objSOHeader.credit_term = ddCreditTerm.Text.Trim();
                }
                else
                {
                    objSOHeader.credit_term = txtCreditTerm.Text;
                }

                objSOHeader.idCustomer = int.Parse(Session["idCustomer"].ToString());
                objSOHeader.Ship_Code = 0;
                objSOHeader.Gross_Amount = grossamount;
                objSOHeader.Final_Discount = finaldiscount;
                objSOHeader.Freight_Charges = Decimal.Parse(txtFreightCharges.Text);
                objSOHeader.Other_Charges = Decimal.Parse(txtOtherCharges.Text);
                objSOHeader.Net_Amount = netamount;
                objSOHeader.Tax_Amount = taxamount;
                objSOHeader.Remarks = txtRemarks.Text.Trim();
                objSOHeader.idSite = int.Parse(ddSite.SelectedValue);
                objSOHeader.currency_code = ddCurrency.SelectedValue;
                objSOHeader.Special_Concession = txtSpecialConc.Text.Trim();
                objSOHeader.transaction_ID = transactionID.ToString();
                objSOHeader.SO_Status = "Open";
                objSOHeader.Stock_Status = ddStockStatus.Text.Trim();
                objSOHeader.CreatedBy = Session["User_Name"].ToString();
                objSOHeader.End_User = txtEndUser.Text;
                objSOHeader.End_User_City = txtEndUserCity.Text;

                if (Session["Mode"].ToString() == "Add")
                {
                    string duplicate = SO_Header.CheckDuplicateCustPO(oCon, txtCustPONum.Text);
                    if (duplicate == "")
                    {
                        SO_Header.Save(oCon, objSOHeader);
                    }
                    else
                    {
                        HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Duplicate Customer PO Number! SO Number : " + duplicate + ControlChars.Quote + ");</script>");
                    }
                }
                if (Session["Mode"].ToString() == "Edit")
                {
                    SO_Header.Update(oCon, objSOHeader);
                    SO_Detail.Delete(oCon, int.Parse(Session["idSOHeader"].ToString()));
                }

                //DataTable dt = new DataTable();
                //dt.Columns.Add(new DataColumn("idItem"));
                //dt.Columns.Add(new DataColumn("Item_Number"));
                //dt.Columns.Add(new DataColumn("Description"));
                //dt.Columns.Add(new DataColumn("Qty"));
                //dt.Columns.Add(new DataColumn("Cost"));
                //dt.Columns.Add(new DataColumn("UM"));
                //dt.Columns.Add(new DataColumn("Discount"));
                //dt.Columns.Add(new DataColumn("Tax_Amount"));
                //dt.Columns.Add(new DataColumn("Amount"));
                //dt.Columns.Add(new DataColumn("Item_Status"));
                //dt.Columns.Add(new DataColumn("idSODetail"));

                DataTable dt = new DataTable();
                dt = (DataTable)Session["Data"];

                for (int x = 0; x <= dt.Rows.Count - 1; x++)
                {
                    int idItem = int.Parse(dt.Rows[x][0].ToString());
                    int Quantity = int.Parse(dt.Rows[x][3].ToString());
                    decimal Cost = decimal.Parse(dt.Rows[x][4].ToString());
                    string UM = dt.Rows[x][5].ToString();
                    decimal Discount = decimal.Parse(dt.Rows[x][6].ToString());
                    decimal Tax_Amount = decimal.Parse(dt.Rows[x][7].ToString());
                    decimal Amount = decimal.Parse(dt.Rows[x][8].ToString());

                    var objSODetail = new SO_Detail_Model();

                    if (Session["Mode"].ToString() == "Add")
                    {
                        objSODetail.idSOHeader = SO_Header.GetLastidSOHeader(oCon);
                    }
                    if (Session["Mode"].ToString() == "Edit")
                    {
                        //objSODetail.idSODetail = int.Parse(lblidSODetail.Text);
                        objSODetail.idSOHeader = int.Parse(Session["idSOHeader"].ToString());
                    }

                    objSODetail.idItem = idItem;
                    objSODetail.Qty = Quantity;
                    objSODetail.Cost = Cost;
                    objSODetail.UM = UM;
                    objSODetail.Discount = Discount;
                    objSODetail.Tax_Amount = Tax_Amount;
                    objSODetail.Amount = Amount;

                    SO_Detail.Save(oCon, objSODetail);
                }


                //foreach (GridViewRow row in gvItems.Rows)
                //{
                //    int idItem = int.Parse(row.Cells[0].Text);
                //    int Quantity = int.Parse(row.Cells[3].Text);
                //    decimal Cost = decimal.Parse(row.Cells[4].Text);
                //    string UM = row.Cells[5].Text.Trim();
                //    decimal Discount = decimal.Parse(row.Cells[6].Text);
                //    decimal Tax_Amount = decimal.Parse(row.Cells[7].Text);
                //    decimal Amount = decimal.Parse(row.Cells[8].Text);

                //    var objSODetail = new SO_Detail_Model();

                //    if (Session["Mode"].ToString() == "Add")
                //    {
                //        objSODetail.idSOHeader = SO_Header.GetLastidSOHeader(oCon);
                //    }
                //    if (Session["Mode"].ToString() == "Edit")
                //    {
                //        //objSODetail.idSODetail = int.Parse(lblidSODetail.Text);
                //        objSODetail.idSOHeader = int.Parse(Session["idSOHeader"].ToString());
                //    }

                //    objSODetail.idItem = idItem;
                //    objSODetail.Qty = Quantity;
                //    objSODetail.Cost = Cost;
                //    objSODetail.UM = UM;
                //    objSODetail.Discount = Discount;
                //    objSODetail.Tax_Amount = Tax_Amount;
                //    objSODetail.Amount = Amount;

                //    SO_Detail.Save(oCon, objSODetail);
                //}

                SOUpload();

                var objLogs = new Logs_Model();

                objLogs.idUser = Users.GetUserIDByDomainLogin(oCon, Session["User_Domain"].ToString());

                objLogs.Form = "SOCreation";

                if (Session["Mode"].ToString() == "Add")
                {
                    objLogs.Description = "Save Record: SO Number = " + so_number + "";
                }
                if (Session["Mode"].ToString() == "Edit")
                {
                    objLogs.Description = "Update Record: SO Number = " + lblSONumber.Text + "";
                }
                Logs.Save(oCon, objLogs);

                Item_Master.InventoryCheckForError(oCon);

                defaultSettings();
                GetSOtoGrid_();
            }
        }
        protected void btnAddToTempGv_Click(object sender, EventArgs e)
        {
            var oSOCreation = new clsSOCreation();

            if (Session["Item Mode"].ToString() == "Edit Item")
            {

                int Quantity = int.Parse(txtQuantity.Text);
                decimal Cost = decimal.Parse(txtCost.Text);
                decimal Discount = decimal.Parse(txtDiscount.Text);
                decimal Tax = decimal.Parse(Session["Tax"].ToString());
                decimal Amount = Cost * Quantity - Discount;
                decimal Tax_Amount;

                if (chckTaxExempt.Checked == true)
                {
                    Tax_Amount = 0;
                }
                else
                {
                    Tax_Amount = Amount * Convert.ToDecimal(1 - (1 / 1.12));
                }

                //var lnkEdit = (LinkButton)sender;
                //var gvrItems = (GridViewRow)lnkEdit.Parent.Parent;
                var rowindex = Convert.ToInt32(Session["rowindex"]);

                DataTable dt = new DataTable();
                dt = (DataTable)Session["Data"];

                dt.Rows[rowindex][3] = Quantity;
                dt.Rows[rowindex][4] = Cost.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                dt.Rows[rowindex][6] = Discount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                dt.Rows[rowindex][7] = Tax_Amount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                dt.Rows[rowindex][8] = Amount.ToString("n", CultureInfo.GetCultureInfo("en-US"));

                gvItems.DataSource = dt;
                gvItems.DataBind();

                Session.Remove("Data");
                Session["Data"] = dt;

                myModal.Visible = false;
                myModal2.Visible = false;

                //var objSODetail = new SO_Detail_Model();
                //objSODetail.idSODetail = int.Parse(Session["idSODetail"].ToString());
                //objSODetail.Qty = Convert.ToDecimal(Quantity);
                //objSODetail.Cost = Cost;
                //objSODetail.Discount = Discount;
                //objSODetail.Tax_Amount = Tax_Amount;
                //objSODetail.Amount = Amount;
                //SO_Detail.UpdateItems(oConnection, objSODetail);

                //GetSavedItems();                               

                //int oldQty = int.Parse(Session["OldQty"].ToString());

                //string QtyMode = "";
                //int QtyToUpdate;

                //if (Quantity > oldQty) //Increased Quantity
                //{
                //    QtyMode = "ADD";
                //    QtyToUpdate = Quantity - oldQty;
                //}
                //else if (Quantity < oldQty) //Decreased Quantity
                //{
                //    QtyMode = "SUBTRACT";
                //    QtyToUpdate = oldQty - Quantity;
                //}
                //else //Same Quantity
                //{
                //    QtyToUpdate = oldQty;
                //}

                //int idItem = int.Parse(lblidItem.Text);
                //int currentTotal = oSOCreation.GetTotalQtyByidItem(idItem);
                //int currentAllocQty = oSOCreation.GetAllocQtyByidItem(idItem);

                //int totalAllocQty; //updates Alloc_Qty
                //int totalTotalqty; //updates Total_Qty

                //if (QtyMode == "ADD")
                //{
                //    totalAllocQty = currentAllocQty + QtyToUpdate;
                //    totalTotalqty = currentTotal - QtyToUpdate;
                //}
                //else if (QtyMode == "SUBTRACT")
                //{
                //    totalAllocQty = currentAllocQty - QtyToUpdate;
                //    totalTotalqty = currentTotal + QtyToUpdate;
                //}
                //else
                //{
                //    totalAllocQty = currentAllocQty;
                //    totalTotalqty = currentTotal;
                //}

                //if (totalTotalqty < 0) //Total Quantity Can't be negative
                //{
                //    totalTotalqty = 0;
                //}

                //var objItemMaster = new Item_Master_Model();
                //objItemMaster.idItem = idItem;
                //objItemMaster.Alloc_Qty = totalAllocQty;
                //objItemMaster.Total_Qty = totalTotalqty;

                //Item_Master.UpdateQtyOnly(oConnection, objItemMaster);

                //var ldata = oSOCreation.RetrieveItemsByidSOHeader(int.Parse(Session["idSOHeader"].ToString()));

                //Session.Remove("Data");
                //Session["Data"] = ldata;


            }
            else if (Session["Item Mode"].ToString() == "Add Item")
            {
                decimal Tax_ = oSOCreation.GetTaxFromDB();
                Session["Tax"] = Convert.ToString(Tax_);

                int currentTotal = oSOCreation.GetTotalQtyByidItem(int.Parse(lblidItem.Text));

                if (txtDiscount.Text == "")
                {
                    txtDiscount.Text = "0";
                }

                if (txtQuantity.Text == "" || txtCost.Text == "")
                {
                    lblItemWarning.Text = "Please fill all required fields.";
                    myModalWarning.Visible = true;
                }
                else
                {

                    if (ddStockStatus.Text == "On Stock")
                    {
                        if (currentTotal < int.Parse(txtQuantity.Text))
                        {
                            lblItemWarning.Text = "Insufficient Stock. Available Stock : " + currentTotal + "";
                            myModalWarning.Visible = true;
                        }
                        else
                        {
                            decimal Quantity = decimal.Parse(txtQuantity.Text);
                            decimal Cost = decimal.Parse(txtCost.Text);
                            decimal Discount = decimal.Parse(txtDiscount.Text);
                            decimal Tax = decimal.Parse(Session["Tax"].ToString());

                            decimal Amount = Cost * Quantity - Discount;

                            decimal Tax_Amount;

                            if (chckTaxExempt.Checked == true)
                            {
                                Tax_Amount = 0;
                            }
                            else
                            {
                                Tax_Amount = Amount * Convert.ToDecimal(1 - (1 / 1.12));
                            }

                            if (Session["Data"] == null)
                            {
                                DataTable dt = new DataTable();
                                dt.Columns.Add(new DataColumn("idItem"));
                                dt.Columns.Add(new DataColumn("Item_Number"));
                                dt.Columns.Add(new DataColumn("Description"));
                                dt.Columns.Add(new DataColumn("Qty"));
                                dt.Columns.Add(new DataColumn("Cost"));
                                dt.Columns.Add(new DataColumn("UM"));
                                dt.Columns.Add(new DataColumn("Discount"));
                                dt.Columns.Add(new DataColumn("Tax_Amount"));
                                dt.Columns.Add(new DataColumn("Amount"));
                                dt.Columns.Add(new DataColumn("Item_Status"));
                                dt.Columns.Add(new DataColumn("idSODetail"));

                                DataRow dRow = dt.NewRow();
                                dRow["idItem"] = lblidItem.Text.Trim();
                                dRow["Item_Number"] = txtItemNo.Text;
                                dRow["Description"] = txtDescription.Text;
                                dRow["Qty"] = Quantity;
                                dRow["Cost"] = Cost.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                                dRow["UM"] = txtUM.Text.Trim();
                                dRow["Discount"] = Discount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                                dRow["Tax_Amount"] = Tax_Amount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                                dRow["Amount"] = Amount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                                dRow["Item_Status"] = 0; //Record to be saved in a_SO_Detail
                                dRow["idSODetail"] = 0;
                                //dRow["Tax"] = Tax;

                                dt.Rows.Add(dRow);

                                gvItems.DataSource = dt;
                                gvItems.DataBind();

                                Session["Data"] = dt;
                            }
                            else
                            {
                                DataTable dt = new DataTable();
                                dt = (DataTable)Session["Data"];

                                DataRow dRow = dt.NewRow();
                                dRow["idItem"] = lblidItem.Text.Trim();
                                dRow["Item_Number"] = txtItemNo.Text;
                                dRow["Description"] = txtDescription.Text;
                                dRow["Qty"] = Quantity;
                                dRow["Cost"] = Cost.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                                dRow["UM"] = txtUM.Text.Trim();
                                dRow["Discount"] = Discount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                                dRow["idSODetail"] = 0;

                                if (chckTaxExempt.Checked == true)
                                {
                                    dRow["Tax_Amount"] = 0;
                                }
                                else
                                {
                                    dRow["Tax_Amount"] = Tax_Amount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                                }

                                dRow["Amount"] = Amount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                                dRow["Item_Status"] = 0; //Record to be saved in a_SO_Detail

                                dt.Rows.Add(dRow);

                                gvItems.DataSource = dt;
                                gvItems.DataBind();

                                Session.Remove("Data");
                                Session["Data"] = dt;

                                //if (Session["Mode"].ToString() == "E")
                                //{
                                //    Session["Mode"] = "EI"; 
                                //}
                            }
                        }
                    }
                    else if (ddStockStatus.Text == "Order Basis")
                    {
                        decimal Quantity = decimal.Parse(txtQuantity.Text);
                        decimal Cost = decimal.Parse(txtCost.Text);
                        decimal Discount = decimal.Parse(txtDiscount.Text);
                        decimal Tax = decimal.Parse(Session["Tax"].ToString());

                        decimal Amount = Cost * Quantity - Discount;
                        decimal Tax_Amount = 0;

                        if (chckTaxExempt.Checked == true)
                        {
                            Tax_Amount = 0;
                        }
                        else
                        {
                            Tax_Amount = Amount * Convert.ToDecimal(1 - (1 / 1.12));
                        }

                        if (Session["Data"] == null)
                        {
                            DataTable dt = new DataTable();
                            dt.Columns.Add(new DataColumn("idItem"));
                            dt.Columns.Add(new DataColumn("Item_Number"));
                            dt.Columns.Add(new DataColumn("Description"));
                            dt.Columns.Add(new DataColumn("Qty"));
                            dt.Columns.Add(new DataColumn("Cost"));
                            dt.Columns.Add(new DataColumn("UM"));
                            dt.Columns.Add(new DataColumn("Discount"));
                            dt.Columns.Add(new DataColumn("Tax_Amount"));
                            dt.Columns.Add(new DataColumn("Amount"));
                            dt.Columns.Add(new DataColumn("Item_Status"));
                            dt.Columns.Add(new DataColumn("idSODetail"));

                            DataRow dRow = dt.NewRow();
                            dRow["idItem"] = lblidItem.Text.Trim();
                            dRow["Item_Number"] = txtItemNo.Text;
                            dRow["Description"] = txtDescription.Text;
                            dRow["Qty"] = Quantity;
                            dRow["Cost"] = Cost.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                            dRow["UM"] = txtUM.Text.Trim();
                            dRow["Discount"] = Discount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                            dRow["Tax_Amount"] = Tax_Amount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                            dRow["Amount"] = Amount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                            dRow["Item_Status"] = 0; //Record to be saved in a_SO_Detail
                            dRow["idSODetail"] = 0;
                            //dRow["Tax"] = Tax;

                            dt.Rows.Add(dRow);

                            gvItems.DataSource = dt;
                            gvItems.DataBind();

                            Session["Data"] = dt;
                        }
                        else
                        {
                            DataTable dt = new DataTable();
                            dt = (DataTable)Session["Data"];

                            DataRow dRow = dt.NewRow();
                            dRow["idItem"] = lblidItem.Text.Trim();
                            dRow["Item_Number"] = txtItemNo.Text;
                            dRow["Description"] = txtDescription.Text;
                            dRow["Qty"] = Quantity;
                            dRow["Cost"] = Cost.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                            dRow["UM"] = txtUM.Text.Trim();
                            dRow["Discount"] = Discount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                            dRow["Tax_Amount"] = Tax_Amount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                            dRow["Amount"] = Amount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                            dRow["Item_Status"] = 0; //Record to be saved in a_SO_Detail
                            dRow["idSODetail"] = 0;
                            dt.Rows.Add(dRow);

                            gvItems.DataSource = dt;
                            gvItems.DataBind();

                            Session.Remove("Data");
                            Session["Data"] = dt;
                        }
                    }

                    myModal.Visible = false;
                    myModal2.Visible = false;

                    txtStockStatus.Text = ddStockStatus.Text.Trim();
                    txtStockStatus.Visible = true;
                    txtStockStatus.ReadOnly = true;
                    ddStockStatus.Visible = false;
                }
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            myModal.Visible = true;
            txtQuantity.Text = "";
            txtCost.Text = "";
            txtUM.Text = "";
            txtDiscount.Text = "";
            Session["Tax"] = "";
            productnamediv.Visible = true;

            btnAddToTempGv.Text = "Add";
            Session["Item Mode"] = "Add Item";
        }
        protected void btnPrintSO_Click(object sender, EventArgs e)
        {
            string access = "";
            access = Session["User_Access"].ToString();

            //if (access == "BCC" || access == "IT")
            //{
                Session["SOHeaderID"] = Session["idSOHeader"];
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "window.location.href='/Forms/DownloadReports/DownloadSO.aspx';", true);

                var objLogs = new Logs_Model();

                objLogs.idUser = Users.GetUserIDByDomainLogin(oCon, Session["User_Domain"].ToString());
                objLogs.Form = "SOCreation";
                objLogs.Description = "Download SO Number : " + lblSONumber.Text + "";
                Logs.Save(oCon, objLogs);
            //}
            //else
            //{
            //    myModalConfirmation.Visible = true;
            //    lblConfirmMsg.Text = "You cannot edit this SO after. Are you sure you want to proceed?";
            //}
        }
        protected void btnCancel1_Click(object sender, EventArgs e)
        {
            myModal.Visible = false;
        }
        protected void btnCancel2_Click(object sender, EventArgs e)
        {
            myModal2.Visible = false;
        }
        protected void btnCancelCustModal_Click(object sender, EventArgs e)
        {
            myModalCustomerDetails.Visible = false;
        }
        protected void btnCloseCustModal_Click(object sender, EventArgs e)
        {
            myModalCustomerDetails.Visible = false;
        }
        protected void lnkBtnSearchCust_Click(object sender, EventArgs e)
        {
            myModalCustomerDetails.Visible = true;
            txtSearchCustomer.Text = "";
        }
        protected void btnCloseWarning_Click(object sender, EventArgs e)
        {
            myModalWarning.Visible = false;
        }
        protected void btnCancelDelete_Click(object sender, EventArgs e)
        {
            myModalConfirmation.Visible = false;
        }
        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            var lnkRemove = (LinkButton)sender;
            var gvrItems = (GridViewRow)lnkRemove.Parent.Parent;
            var gvItems = (GridView)lnkRemove.Parent.Parent.Parent.Parent;
            var rowindex = gvrItems.RowIndex;


            DataTable dt = new DataTable();
            dt = (DataTable)Session["Data"];

            dt.Rows.Remove(dt.Rows[rowindex]);

            gvItems.DataSource = dt;
            gvItems.DataBind();

            Session.Remove("Data");
            Session["Data"] = dt;
        }
        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            var lnkDelete = (LinkButton)sender;
            var gvrItems = (GridViewRow)lnkDelete.Parent.Parent;
            //var gvItems = (GridView)lnkDelete.Parent.Parent.Parent.Parent;
            var rowindex = gvrItems.RowIndex;

            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("idItem"));
            dt.Columns.Add(new DataColumn("Item_Number"));
            dt.Columns.Add(new DataColumn("Description"));
            dt.Columns.Add(new DataColumn("Qty"));
            dt.Columns.Add(new DataColumn("Cost"));
            dt.Columns.Add(new DataColumn("UM"));
            dt.Columns.Add(new DataColumn("Discount"));
            dt.Columns.Add(new DataColumn("Tax_Amount"));
            dt.Columns.Add(new DataColumn("Amount"));
            dt.Columns.Add(new DataColumn("Item_Status"));
            dt.Columns.Add(new DataColumn("idSODetail"));

            foreach (GridViewRow row in gvItems.Rows)
            {
                int idItem = int.Parse(row.Cells[0].Text);
                string Item_No = row.Cells[1].Text.Trim();
                string Description = row.Cells[2].Text.Trim();
                int Quantity = int.Parse(row.Cells[3].Text);
                decimal Cost = decimal.Parse(row.Cells[4].Text);
                string UM = row.Cells[5].Text.Trim();
                decimal Discount = decimal.Parse(row.Cells[6].Text);
                decimal Tax_Amount = decimal.Parse(row.Cells[7].Text);
                decimal Amount = decimal.Parse(row.Cells[8].Text);
                string Item_Status = row.Cells[9].Text.Trim();
                int idSODetail = int.Parse(row.Cells[10].Text);

                DataRow dRow = dt.NewRow();
                dRow["idItem"] = idItem;
                dRow["Item_Number"] = Item_No;
                dRow["Description"] = Description;
                dRow["Qty"] = Quantity;
                dRow["Cost"] = Cost.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                dRow["UM"] = txtUM.Text.Trim();
                dRow["Discount"] = Discount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                dRow["Tax_Amount"] = Tax_Amount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                dRow["Amount"] = Amount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                dRow["Item_Status"] = Item_Status;
                dRow["idSODetail"] = idSODetail;

                dt.Rows.Add(dRow);
            }

            dt.Rows.RemoveAt(rowindex);

            gvItems.DataSource = dt;
            gvItems.DataBind();

            Session.Remove("Data");
            Session["Data"] = dt;

            //var oSOCreation = new clsSOCreation();

            //int itemcount = oSOCreation.CountCurrentItem(int.Parse(Session["idSOHeader"].ToString()));

            //if (itemcount == 1)
            //{
            //    myModalWarning.Visible = true;
            //    lblItemWarning.Text = "No. of items can't be null. Cannot delete record.";
            //}
            //else
            //{
            //var lnkDelete = (LinkButton)sender;
            ////var gvrItems = (GridViewRow)lnkDelete.Parent.Parent;
            ////var gvItems = (GridView)lnkDelete.Parent.Parent.Parent.Parent;
            ////var rowindex = gvrItems.RowIndex;

            //GridViewRow row = (GridViewRow)lnkDelete.NamingContainer;

            //lblidItem.Text = row.Cells[0].Text.Trim();
            //Session["ItemQty"] = row.Cells[3].Text.Trim();
            //Session["CurrentAllocQty"] = oSOCreation.GetAllocQtyByidItem(int.Parse(row.Cells[0].Text)).ToString();
            //Session["CurrentTotalQty"] = oSOCreation.GetTotalQty(int.Parse(row.Cells[0].Text)).ToString();
            //Session["idSODetail"] = row.Cells[10].Text.Trim();

            //myModalConfirmation.Visible = true;
            //lblConfirmMsg.Text = "Record will be permanently deleted. Are you sure you want to proceed?";
            //}
        }
        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            myModal2.Visible = true;
            btnAddToTempGv.Text = "Save";
            var lnkEdit = (LinkButton)sender;
            var gvrItems = (GridViewRow)lnkEdit.Parent.Parent;
            var gvItems = (GridView)lnkEdit.Parent.Parent.Parent.Parent;
            var rowindex = gvrItems.RowIndex;

            GridViewRow row = (GridViewRow)lnkEdit.NamingContainer;

            lblidItem.Text = row.Cells[0].Text.Trim();
            //txtItemName.Text = row.Cells[0].Text.Trim();
            productnamediv.Visible = false;
            txtItemNo.Text = row.Cells[1].Text.Trim();
            txtDescription.Text = row.Cells[2].Text.Trim();
            txtQuantity.Text = row.Cells[3].Text.Trim();
            Session["OldQty"] = row.Cells[3].Text.Trim();
            txtCost.Text = row.Cells[4].Text.Trim();
            txtDiscount.Text = row.Cells[6].Text.Trim();

            txtItemNo.ReadOnly = true;
            txtDescription.ReadOnly = true;

            Session["idSODetail"] = row.Cells[10].Text.Trim();
            Session["rowindex"] = row.RowIndex;
            Session["Item Mode"] = "Edit Item";
        }
        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            //if (lblConfirmMsg.Text == "Record will be permanently deleted. Are you sure you want to proceed?")
            //{
            //    var objItemMaster = new Item_Master_Model();

            //    int updatedAllocQty = int.Parse(Session["CurrentAllocQty"].ToString()) - int.Parse(Session["ItemQty"].ToString());
            //    int updatedTotalQty = int.Parse(Session["CurrentTotalQty"].ToString()) + int.Parse(Session["ItemQty"].ToString());

            //    objItemMaster.idItem = int.Parse(lblidItem.Text);
            //    objItemMaster.Alloc_Qty = updatedAllocQty;
            //    objItemMaster.Total_Qty = updatedTotalQty;

            //    Item_Master.UpdateQtyOnly(oCon, objItemMaster);
            //    SO_Detail.DeleteByID(oCon, int.Parse(Session["idSODetail"].ToString()));

            //    myModalConfirmation.Visible = false;

            //    GetSavedItems();

            //    DataTable dt = new DataTable();

            //    dt.Columns.Add(new DataColumn("idItem"));
            //    dt.Columns.Add(new DataColumn("Item_Number"));
            //    dt.Columns.Add(new DataColumn("Description"));
            //    dt.Columns.Add(new DataColumn("Qty"));
            //    dt.Columns.Add(new DataColumn("Cost"));
            //    dt.Columns.Add(new DataColumn("UM"));
            //    dt.Columns.Add(new DataColumn("Discount"));
            //    dt.Columns.Add(new DataColumn("Tax_Amount"));
            //    dt.Columns.Add(new DataColumn("Amount"));
            //    dt.Columns.Add(new DataColumn("Item_Status"));
            //    dt.Columns.Add(new DataColumn("idSODetail"));

            //    foreach (GridViewRow row in gvItems.Rows)
            //    {
            //        int idItem = int.Parse(row.Cells[0].Text);
            //        string Item_No = row.Cells[1].Text.Trim();
            //        string Description = row.Cells[2].Text.Trim();
            //        int Quantity = int.Parse(row.Cells[3].Text);
            //        decimal Cost = decimal.Parse(row.Cells[4].Text);
            //        string UM = row.Cells[5].Text.Trim();
            //        decimal Discount = decimal.Parse(row.Cells[6].Text);
            //        decimal Tax_Amount = decimal.Parse(row.Cells[7].Text);
            //        decimal Amount = decimal.Parse(row.Cells[8].Text);
            //        string Item_Status = row.Cells[9].Text.Trim();
            //        int idSODetail = int.Parse(row.Cells[10].Text);

            //        DataRow dRow = dt.NewRow();
            //        dRow["idItem"] = idItem;
            //        dRow["Item_Number"] = Item_No;
            //        dRow["Description"] = Description;
            //        dRow["Qty"] = Quantity;
            //        dRow["Cost"] = Cost.ToString("n", CultureInfo.GetCultureInfo("en-US"));
            //        dRow["UM"] = txtUM.Text.Trim();
            //        dRow["Discount"] = Discount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
            //        dRow["Tax_Amount"] = Tax_Amount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
            //        dRow["Amount"] = Amount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
            //        dRow["Item_Status"] = Item_Status;
            //        dRow["idSODetail"] = idSODetail;

            //        dt.Rows.Add(dRow);
            //    }

            //    gvItems.DataSource = dt;
            //    gvItems.DataBind();

            //    Session.Remove("Data");
            //    Session["Data"] = dt;

            //    var objLogs = new Logs_Model();
            //    objLogs.idUser = Users.GetUserIDByDomainLogin(oCon, Session["User_Domain"].ToString());
            //    objLogs.Form = "SOCreation";
            //    objLogs.Description = "Remove idItem : " + int.Parse(lblidItem.Text) + " from SO Number : " + lblSONumber.Text + "";
            //    Logs.Save(oCon, objLogs);

            //}
            //else
            if (lblConfirmMsg.Text == "You cannot edit this SO after. Are you sure you want to proceed?")
            {

                //var objSOHeader = new SO_Header_Model();

                //objSOHeader.idSOHeader = int.Parse(Session["idSOHeader"].ToString());
                //objSOHeader.SO_Status = "Closed";

                //SO_Header.UpdateSOStatus(oCon, objSOHeader);

                Session["SOHeaderID"] = Session["idSOHeader"];

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "window.location.href='/Forms/DownloadReports/DownloadSO.aspx';", true);


                var objLogs = new Logs_Model();

                objLogs.idUser = Users.GetUserIDByDomainLogin(oCon, Session["User_Domain"].ToString());
                objLogs.Form = "SOCreation";
                objLogs.Description = "Download Record : SO Number = " + lblSONumber.Text + "";
                Logs.Save(oCon, objLogs);

                myModalConfirmation.Visible = false;
                //btnEdit.Visible = false;
                //btnPrintSO.Visible = false;

                Session["oParent"] = this;

            }
            else if (lblConfirmMsg.Text == "You cannot retrieve this SO after. Are you sure you want to delete?")
            {
                myModalConfirmation.Visible = false;
                var objLogs = new Logs_Model();

                objLogs.idUser = Users.GetUserIDByDomainLogin(oCon, Session["User_Domain"].ToString());
                objLogs.Form = "SOCreation";
                objLogs.Description = "DELETE Record: SO Number = " + lblSONumber.Text + "";
                Logs.Save(oCon, objLogs);

                SO_Header.Delete(oCon, Convert.ToInt32(Session["idSOHeader"].ToString()));
                defaultSettings();
                clearFlds();
                Session.Remove("Data");
                GetSOtoGrid_();
            }

        }
        public void hideButtons()
        {
            myModalConfirmation.Visible = false;
            btnEdit.Visible = false;
            //btnPrintSO.Visible = false;
        }
        protected void btnMyModalClose_Click(object sender, EventArgs e)
        {
            myModal.Visible = false;
        }
        protected void btnMyModalClose2_Click(object sender, EventArgs e)
        {
            myModal2.Visible = false;
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            defaultSettings();
            clearFlds();
            GetSOtoGrid_();
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Session["Mode"] = "Edit";

            string access = "";
            access = Session["User_Access"].ToString();

            if (access == "BCC")
            {
                txtSpecialConc.ReadOnly = false;
                ddCreditTerm.Visible = true;
                txtCreditTerm.Visible = false;
                txtRemarks.ReadOnly = false;
                txtCustPONum.ReadOnly = false;
            }
            else
            {
                txtDueDate_.Visible = false;
                txtDueDate.Visible = true;
                txtDueDate.TextMode = TextBoxMode.SingleLine;
                txtDueDate.Text = txtDueDate.Text.Trim();

                txtDueDate.ReadOnly = false;
                txtCustPONum.ReadOnly = false;
                txtCustomerCode.ReadOnly = true;
                txtFreightCharges.ReadOnly = false;
                txtOtherCharges.ReadOnly = false;
                txtRemarks.ReadOnly = false;
                //ddCustomerCode.Visible = true;
                ddCreditTerm.Visible = false;
                txtCreditTerm.Visible = true;
                txtCreditTerm.ReadOnly = true;
                txtSite.Visible = false;
                txtCurrency.Visible = false;
                ddSite.Visible = true;
                //gvItems.Enabled = true;
                lnkBtnSearchCust.Enabled = true;
                txtCurrency.Visible = false;
                ddCurrency.Visible = true;
                txtStockStatus.Visible = true;
                ddStockStatus.Visible = false;
                txtSpecialConc.ReadOnly = false;
                chckTaxExempt.Enabled = true;

                btnAdd.Visible = true;

                if (access == "PM" || access == "PA")
                {
                    GetUserDetailsToDD();
                    ddCreditTerm.SelectedValue = txtCreditTerm.Text.Trim();
                    ddSalesman.SelectedValue = grvMainSO.SelectedRow.Cells[5].Text.Trim();
                    ddSalesman.Visible = true;
                    txtSalesman.Visible = false;
                }
                else
                {
                    ddSalesman.Visible = false;
                    txtSalesman.Visible = true;
                }
            }

            btnSave.Visible = true;
            btnEdit.Visible = false;
            btnPrintSO.Visible = false;


            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("idItem"));
            dt.Columns.Add(new DataColumn("Item_Number"));
            dt.Columns.Add(new DataColumn("Description"));
            dt.Columns.Add(new DataColumn("Qty"));
            dt.Columns.Add(new DataColumn("Cost"));
            dt.Columns.Add(new DataColumn("UM"));
            dt.Columns.Add(new DataColumn("Discount"));
            dt.Columns.Add(new DataColumn("Tax_Amount"));
            dt.Columns.Add(new DataColumn("Amount"));
            dt.Columns.Add(new DataColumn("Item_Status"));
            dt.Columns.Add(new DataColumn("idSODetail"));

            foreach (GridViewRow row in gvItems.Rows)
            {
                int idItem = int.Parse(row.Cells[0].Text);
                string Item_No = row.Cells[1].Text.Trim();
                string Description = row.Cells[2].Text.Trim();
                int Quantity = int.Parse(row.Cells[3].Text);
                decimal Cost = decimal.Parse(row.Cells[4].Text);
                string UM = row.Cells[5].Text.Trim();
                decimal Discount = decimal.Parse(row.Cells[6].Text);
                decimal Tax_Amount = decimal.Parse(row.Cells[7].Text);
                decimal Amount = decimal.Parse(row.Cells[8].Text);
                string Item_Status = row.Cells[9].Text.Trim();
                int idSODetail = int.Parse(row.Cells[10].Text);

                DataRow dRow = dt.NewRow();
                dRow["idItem"] = idItem;
                dRow["Item_Number"] = Item_No;
                dRow["Description"] = Description;
                dRow["Qty"] = Quantity;
                dRow["Cost"] = Cost.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                dRow["UM"] = UM;
                dRow["Discount"] = Discount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                dRow["Tax_Amount"] = Tax_Amount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                dRow["Amount"] = Amount.ToString("n", CultureInfo.GetCultureInfo("en-US"));
                dRow["Item_Status"] = Item_Status;  //Record already saved in a_SO_Detail
                dRow["idSODetail"] = idSODetail;


                //HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + dRow["Status"].ToString() + ControlChars.Quote + ");</script>");

                dt.Rows.Add(dRow);
            }

            gvItems.DataSource = dt;
            gvItems.DataBind();

            Session.Remove("Data");
            Session["Data"] = dt;


        }
        private void SOUpload()
        {
            var oSOCreation = new clsSOCreation();
            var lDataAdd = Session["Data"];

            Microsoft.Reporting.WebForms.ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            viewer.LocalReport.ReportPath = Server.MapPath(@"~\Resources\LSISalesOrder.rdlc");
            //viewer.LocalReport.ReportPath = "SalesOrder.rdlc";

            string strSONumber = "";
            string strOrderDate = null;
            string strDueDate = null;
            string strCustomerPOnum = txtCustPONum.Text.Trim();
            string access = "";
            string strSalesman = "";
            string strCurrency = ddCurrency.SelectedValue.ToString();
            if (Session["Mode"].ToString() == "Add")
            {
                strSONumber = SO_Header.GetLastSONumber(oCon);
                strOrderDate = (DateTime.Now).ToString("dd/MM/yyyy");
                strDueDate = DateTime.Parse(txtDueDate.Text.ToString()).ToString("dd/MM/yyyy");
            }
            else if (Session["Mode"].ToString() == "Edit")
            {
                strSONumber = lblSONumber.Text;
                strOrderDate = txtOrderDate_.Text;
                strDueDate = txtDueDate.Text;
            }

            access = Session["User_Access"].ToString();

            if (access == "PM" || access == "PA")
            {
                strSalesman = ddSalesman.SelectedItem.Text;
            }
            else
            {
                strSalesman = txtSalesman.Text.Trim();
            }

            string strCreditTerm = txtCreditTerm.Text.Trim();
            string strOtherCharges = txtOtherCharges.Text.Trim();

            decimal grossamount = gvItems.Rows.Cast<GridViewRow>().Sum(t => Convert.ToDecimal(t.Cells[8].Text));
            decimal finaldiscount = gvItems.Rows.Cast<GridViewRow>().Sum(t => Convert.ToDecimal(t.Cells[6].Text));
            decimal netamount = grossamount - Decimal.Parse(txtFreightCharges.Text) - Decimal.Parse(txtOtherCharges.Text);
            decimal taxamount = grossamount * Convert.ToDecimal(1 - (1 / 1.12));

            decimal strGrossAmount = grossamount;
            decimal strFinalDiscount = finaldiscount;
            double strCharges = double.Parse(txtFreightCharges.Text) + double.Parse(txtOtherCharges.Text);
            decimal strNetAmount = netamount;
            string strCustomerCode = txtCustomerCode.Text.Trim();
            string strAddress1 = Session["Address1"].ToString();
            string strAddress2 = Session["Address2"].ToString();
            string strAddress3 = Session["Address3"].ToString();
            string strAddress4 = Session["Address4"].ToString();
            string strRemarks = txtRemarks.Text.Trim();
            string strCompanyName = Session["CompanyName"].ToString();
            decimal strOutputVat = taxamount;
            decimal strTaxableSales = Convert.ToDecimal(netamount) / Convert.ToDecimal(1.12);
            string strShipAddress1 = Session["AddressShipping1"].ToString();
            string strShipAddress2 = Session["AddressShipping2"].ToString();
            string strShipAddress3 = Session["AddressShipping3"].ToString();
            string strShipAddress4 = Session["AddressShipping4"].ToString();

            ReportParameter p1 = new ReportParameter("Order_Date", strOrderDate);
            ReportParameter p2 = new ReportParameter("Due_Date", strDueDate);
            ReportParameter p3 = new ReportParameter("Customer_PO", strCustomerPOnum);
            ReportParameter p4 = new ReportParameter("Salesman", strSalesman);
            ReportParameter p5 = new ReportParameter("credit_term", strCreditTerm);
            ReportParameter p6 = new ReportParameter("SO_Number", strSONumber);
            ReportParameter p7 = new ReportParameter("Gross_Amount", strGrossAmount.ToString("n", CultureInfo.GetCultureInfo("en-US")));
            ReportParameter p8 = new ReportParameter("Final_Discount", strFinalDiscount.ToString("n", CultureInfo.GetCultureInfo("en-US")));
            ReportParameter p9 = new ReportParameter("Charges", strCharges.ToString("n", CultureInfo.GetCultureInfo("en-US")));
            ReportParameter p10 = new ReportParameter("Net_Amount", strCurrency + " " + strNetAmount.ToString("n", CultureInfo.GetCultureInfo("en-US")));
            ReportParameter p11 = new ReportParameter("Customer_Code", strCustomerCode);
            ReportParameter p12 = new ReportParameter("Address1", strAddress1);
            ReportParameter p13 = new ReportParameter("Address2", strAddress2);
            ReportParameter p14 = new ReportParameter("Address3", strAddress3);
            ReportParameter p15 = new ReportParameter("Address4", strAddress4);
            ReportParameter p16 = new ReportParameter("Remarks", strRemarks);
            ReportParameter p17 = new ReportParameter("Company_Name", strCompanyName);


            ReportParameter p18;
            ReportParameter p19;
            if (chckTaxExempt.Checked == true)
            {
                p18 = new ReportParameter("Output_Vat", "0.00");
                p19 = new ReportParameter("Taxable_Sales", "0.00");
            }
            else
            {
                p18 = new ReportParameter("Output_Vat", strOutputVat.ToString("n", CultureInfo.GetCultureInfo("en-US")));
                p19 = new ReportParameter("Taxable_Sales", strTaxableSales.ToString("n", CultureInfo.GetCultureInfo("en-US")));
            }

            ReportParameter p20 = new ReportParameter("AddressShipping1", strShipAddress1);
            ReportParameter p21 = new ReportParameter("AddressShipping2", strShipAddress2);
            ReportParameter p22 = new ReportParameter("AddressShipping3", strShipAddress3);

            if (strAddress1 == "&nbsp;")
            {
                strAddress1 = " ";
            }
            if (strAddress2 == "&nbsp;")
            {
                strAddress2 = " ";
            }
            if (strAddress3 == "&nbsp;")
            {
                strAddress3 = " ";
            }
            if (strAddress4 == "&nbsp;")
            {
                strAddress4 = " ";
            }
            if (strShipAddress1 == "&nbsp;")
            {
                strShipAddress1 = " ";
            }
            if (strShipAddress2 == "&nbsp;")
            {
                strShipAddress2 = " ";
            }
            if (strShipAddress3 == "&nbsp;")
            {
                strShipAddress3 = " ";
            }

            viewer.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7,
            p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22 });


            ReportDataSource repDataSource1 = new ReportDataSource("DataSet1", lDataAdd);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(repDataSource1);

            byte[] bytes = viewer.LocalReport.Render("PDF");

            if (Session["Mode"].ToString() == "Edit")
            {
                DeleteFile();
            }

            // insert data
            string strQuery = "INSERT INTO a_SO_Attachment" + "(idSOHeader, FileName, content_type, data)" + " values (@idSOHeader, @FileName, @content_type, @data)";
            SqlCommand cmd = new SqlCommand(strQuery);
            if (Session["Mode"].ToString() == "Add")
            {
                cmd.Parameters.Add("@idSOHeader", SqlDbType.VarChar).Value = SO_Header.GetLastidSOHeader(oCon);
            }
            else if (Session["Mode"].ToString() == "Edit")
            {
                cmd.Parameters.Add("@idSOHeader", SqlDbType.VarChar).Value = Session["idSOHeader"];
            }
            cmd.Parameters.Add("@FileName", SqlDbType.VarChar).Value = strSONumber;
            cmd.Parameters.Add("@content_type", SqlDbType.VarChar).Value = ContentType;
            cmd.Parameters.Add("@data", SqlDbType.Binary).Value = bytes;

            InsertUpdateData(cmd);
        }
        protected void DeleteFile()
        {
            string strQuery = "DELETE FROM a_SO_Attachment WHERE idSOHeader=@idSOHeader";
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add("@idSOHeader", SqlDbType.Int).Value = Session["idSOHeader"];
            InsertUpdateData(cmd);
        }
        public bool InsertUpdateData(SqlCommand cmd)
        {
            oCon.Open();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = oCon;
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                return false;
            }
            finally
            {
                cmd.Dispose();
                oCon.Close();
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            myModalConfirmation.Visible = true;
            lblConfirmMsg.Text = "You cannot retrieve this SO after. Are you sure you want to delete?";
        }
        protected void ddlSOStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetSOtoGrid_();
        }
        protected void btnDownloadSalesReport_Click(object sender, EventArgs e)
        {
            dvDownloadSalesReport.Visible = true;
        }
        protected void btnDownloadReport_Click(object sender, EventArgs e)
        {
            DataTable dtReport = new DataTable();
            dtReport.Columns.Add("SALESMAN");
            dtReport.Columns.Add("SO NUMBER");
            dtReport.Columns.Add("Partner Name or Partner ID");
            dtReport.Columns.Add("Transaction date / Due Date");
            dtReport.Columns.Add("GN Audio Part Number");
            dtReport.Columns.Add("GN Audio Part Number Description");
            dtReport.Columns.Add("Sales Quantity");
            dtReport.Columns.Add("ERP COST");
            dtReport.Columns.Add("DP PRICE");
            dtReport.Columns.Add("IS VAT-INC");
            dtReport.Columns.Add("Bill to Party Name (Reseller)");
            dtReport.Columns.Add("Bill to party city");
            dtReport.Columns.Add("Ship to Party Country");
            dtReport.Columns.Add("Ship to Party Name (End User)");
            dtReport.Columns.Add("Ship to party city");
            dtReport.Columns.Add("ERP Part Number");
            dtReport.Columns.Add("Pick Status");
            dtReport.Columns.Add("PO Number");
            dtReport.Columns.Add("Invoice Number");
            dtReport.Columns.Add("Invoice Date");
            dtReport.Columns.Add("Delivery Date");
            dtReport.Columns.Add("OR Number");
            dtReport.Columns.Add("Credit Term");

            DataTable dtSO = SO_Report.getSOByDateRange(oCon, txtFromDate.Text, txtToDate.Text);

            for (int x = 0; x <= dtSO.Rows.Count - 1; x++)
            {
                DataTable dtCost = SO_Report.getCostPerSOPerItemNumber(oCon, Convert.ToInt32(dtSO.Rows[x][0].ToString()), Convert.ToInt32(dtSO.Rows[x][1].ToString()), Convert.ToInt32(dtSO.Rows[x][8].ToString()));

                DataRow dr = dtReport.NewRow();

                double dpPrice = Convert.ToDouble(dtSO.Rows[x][9].ToString());
                double tax_amount = Convert.ToDouble(dtSO.Rows[x][17].ToString());
                double newDpPrice = 0;
                double forex_rate = decimal.ToDouble(Convert.ToDecimal(dtSO.Rows[x][23].ToString()));

                if (forex_rate != 0)
                {
                    dpPrice = dpPrice * forex_rate;
                }

                if (Convert.ToInt32(tax_amount) != 0)
                {
                    newDpPrice = dpPrice - (dpPrice * (1 - (1 / 1.12)));
                    dr[9] = "Y";
                }
                else
                {
                    newDpPrice = dpPrice;
                    dr[9] = "N";
                }

                dr[0] = dtSO.Rows[x][2];
                dr[1] = dtSO.Rows[x][3];
                dr[2] = dtSO.Rows[x][4];
                dr[3] = dtSO.Rows[x][5];
                dr[4] = dtSO.Rows[x][6];
                dr[5] = dtSO.Rows[x][7];
                dr[6] = dtSO.Rows[x][8];
                dr[7] = dtCost.Rows[0][0];
                dr[8] = newDpPrice;
                dr[10] = dtSO.Rows[x][10];
                dr[11] = dtSO.Rows[x][11];
                dr[12] = dtSO.Rows[x][12];
                dr[13] = dtSO.Rows[x][13];
                dr[14] = dtSO.Rows[x][14];
                dr[15] = dtSO.Rows[x][15];
                dr[16] = dtSO.Rows[x][16];
                dr[17] = dtSO.Rows[x][18];
                dr[18] = dtSO.Rows[x][19];
                dr[19] = dtSO.Rows[x][20];
                dr[20] = dtSO.Rows[x][21];
                dr[21] = "";
                dr[22] = dtSO.Rows[x][24];
                dtReport.Rows.Add(dr);
            }

            ExporttoExcel(dtReport);
            //using (XLWorkbook wb = new XLWorkbook())
            //{
            //    wb.Worksheets.Add(dtReport, "Sales Report");

            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.Charset = "";
            //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    Response.AddHeader("content-disposition", "attachment;filename=LSI JABRA SALES " + DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Year.ToString() + ".xlsx");
            //    using (MemoryStream MyMemoryStream = new MemoryStream())
            //    {
            //        wb.SaveAs(MyMemoryStream);
            //        MyMemoryStream.WriteTo(Response.OutputStream);
            //        Response.Flush();
            //        Response.End();
            //    }
            //}

            //string attachment = "attachment; filename=LSI JABRA SALES " + DateTime.Now.Month.ToString() + " " + DateTime.Now.Year.ToString() + ".xlsx";
            //Response.ClearContent();
            //Response.AddHeader("content-disposition", attachment);
            //Response.ContentType = "application/vnd.ms-excel";
            //string tab = "";
            //foreach (DataColumn dc in dtReport.Columns)
            //{
            //    Response.Write(tab + dc.ColumnName);
            //    tab = "\t";
            //}
            //Response.Write("\n");
            //int i;
            //foreach (DataRow dr in dtReport.Rows)
            //{
            //    tab = "";
            //    for (i = 0; i < dtReport.Columns.Count; i++)
            //    {
            //        Response.Write(tab + dr[i].ToString());
            //        tab = "\t";
            //    }
            //    Response.Write("\n");
            //}
            //Response.End();
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
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=LSI JABRA SALES " + DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Year.ToString() + ".xls");

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

        protected void btnCancelDownload_Click(object sender, EventArgs e)
        {
            dvDownloadSalesReport.Visible = false;
        }
        protected void chckTaxExempt_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["Data"];

            for (int x = 0; x <= dt.Rows.Count - 1; x++)
            {
                dt.Rows[x][4] = (0).ToString("n", CultureInfo.GetCultureInfo("en-US"));
                dt.Rows[x][6] = (0).ToString("n", CultureInfo.GetCultureInfo("en-US"));
                dt.Rows[x][7] = (0).ToString("n", CultureInfo.GetCultureInfo("en-US"));
                dt.Rows[x][8] = (0).ToString("n", CultureInfo.GetCultureInfo("en-US"));
            }

            gvItems.DataSource = dt;
            gvItems.DataBind();

            Session.Remove("Data");
            Session["Data"] = dt;
        }

        protected void lbtnSearchItemNumberSO_Click(object sender, EventArgs e)
        {
            var ds = Item_Master.RetreiveSOByItemNumber(oCon, txtSearchItemNumberSO.Text);
            gvItemNumberSO.DataSource = ds;
            gvItemNumberSO.DataBind();
        }

        protected void btnCloseItemNumberSO_ServerClick(object sender, EventArgs e)
        {
            dvItemNumberSO.Visible = false;
        }

        protected void btnShowItemSOModal_Click(object sender, EventArgs e)
        {
            dvItemNumberSO.Visible = true;
        }
    }
}