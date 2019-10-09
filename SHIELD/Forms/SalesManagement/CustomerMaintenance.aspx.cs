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
using SHIELD.ERP;
using System.Web;
using System.Configuration;

namespace SHIELD.ERP
{
    public partial class CustomerMaintenance : System.Web.UI.Page
    {
        SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
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

            if (!Page.IsPostBack)
            {
                for (int x = Session.Count - 1; x >= 5; x--)
                {
                    Session.RemoveAt(x);
                }

                if (username != "")
                {
                    if (access == "PM" || access == "IT" || access == "BCC")
                    {
                        defaultSettings();
                        GetCustomerDetails();
                        GetCreditTermToDD();
                        GetUserDetailsToDD();
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

        protected void grvCustomerDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["Mode"] = "E";

            pnlAllDetails.Visible = true;
            CustomerPanel.Visible = false;
            pnlCustomerDetails.Visible = true;
            tblSearch.Visible = false;

            btnEdit.Visible = true;
            btnAddNew.Visible = false;
            btnBack.Visible = true;
            btnSave.Visible = false;

            GridViewRow row = grvCustomerDetails.SelectedRow;
            Session["idCustomer"] = row.Cells[0].Text;
            Session["CustomerCode"] = row.Cells[1].Text;

            if (row.Cells[2].Text != "&nbsp;")
            {
                txtCustomerName.Text = row.Cells[2].Text;
            }
            if (row.Cells[3].Text != "&nbsp;")
            {
                txtCompanyName.Text = row.Cells[3].Text;
            }
            if (row.Cells[4].Text != "&nbsp;")
            {
                txtAddress1.Text = row.Cells[4].Text;
            }
            if (row.Cells[5].Text != "&nbsp;")
            {
                txtAddress2.Text = row.Cells[5].Text;
            }
            if (row.Cells[6].Text != "&nbsp;")
            {
                txtAddress3.Text = row.Cells[6].Text;
            }
            if (row.Cells[7].Text != "&nbsp;")
            {
                txtAddress4.Text = row.Cells[7].Text;
            }
            if (row.Cells[12].Text != "&nbsp;")
            {
                txtShipAddress1.Text = row.Cells[12].Text;
            }
            if (row.Cells[13].Text != "&nbsp;")
            {
                txtShipAddress2.Text = row.Cells[13].Text;
            }
            if (row.Cells[14].Text != "&nbsp;")
            {
                txtShipAddress3.Text = row.Cells[14].Text;
            }
            if (row.Cells[15].Text != "&nbsp;")
            {
                txtShipAddress4.Text = row.Cells[15].Text;
            }

            if (row.Cells[9].Text != "&nbsp;")
            {
                txtCreditTerm.Text = row.Cells[9].Text;
                ddCreditTerm.SelectedValue = txtCreditTerm.Text;
            }
            else
            {
                txtCreditTerm.Text = "No Credit Term";
            }

            if (row.Cells[16].Text != "&nbsp;")
            {
                txtCustomerType.Text = row.Cells[16].Text;
                ddCustomerType.SelectedValue = txtCustomerType.Text;
            }
            else
            {
                txtCustomerType.Text = "No Customer Type";
            }

            if (row.Cells[17].Text != "&nbsp;")
            {
                txtCreditLimit.Text = row.Cells[17].Text;
            }

            if (row.Cells[18].Text != "&nbsp;")
            {
                txtContactNo.Text = row.Cells[18].Text;
            }

            if (row.Cells[10].Text != "&nbsp;")
            {
                txtPosition.Text = row.Cells[10].Text;
            }

            if (row.Cells[19].Text != "&nbsp;")
            {
                txtTINNo.Text = row.Cells[19].Text;
            }

            if (row.Cells[20].Text != "&nbsp;")
            {
                txtSalesman.Text = row.Cells[20].Text;
                ddSalesman.SelectedValue = txtSalesman.Text;
            }
            else
            {
                txtSalesman.Text = "No Salesman";
            }

            if (row.Cells[21].Text != "&nbsp;")
            {
                txtEmailAddress.Text = row.Cells[21].Text;
            }
            else
            {
                txtEmailAddress.Text = "No Email Address";
            }


            statusdiv.Visible = true;

            txtStatus.Text = row.Cells[11].Text;
            ddStatus.SelectedValue = txtStatus.Text;

            txtCustomerName.ReadOnly = true;
            txtCompanyName.ReadOnly = true;
            txtAddress1.ReadOnly = true;
            txtAddress2.ReadOnly = true;
            txtAddress3.ReadOnly = true;
            txtAddress4.ReadOnly = true;
            txtCreditTerm.ReadOnly = true;
            txtCustomerType.ReadOnly = true;
            txtSalesman.ReadOnly = true;
            txtStatus.ReadOnly = true;
            ddCreditTerm.Visible = false;
            ddCustomerType.Visible = false;
            ddSalesman.Visible = false;
            ddStatus.Visible = false;
            txtCreditTerm.Visible = true;
            txtCustomerType.Visible = true;
            txtSalesman.Visible = true;
            txtStatus.Visible = true;
            txtShipAddress1.ReadOnly = true;
            txtShipAddress2.ReadOnly = true;
            txtShipAddress3.ReadOnly = true;
            txtShipAddress4.ReadOnly = true;
            txtCreditLimit.ReadOnly = true;
            txtEmailAddress.ReadOnly = true;
            txtPosition.ReadOnly = true;
            txtContactNo.ReadOnly = true;
            txtTINNo.ReadOnly = true;
            statusdiv.Visible = true;
            chckCopy.Visible = false;
        }
        protected void grvCustomerDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false; //idCustomer
                e.Row.Cells[2].Visible = false; //CustomerName
                e.Row.Cells[4].Visible = false; //Address1
                e.Row.Cells[5].Visible = false; //Address2
                e.Row.Cells[6].Visible = false; //Address3
                e.Row.Cells[7].Visible = false; //Address4
                e.Row.Cells[8].Visible = false; //FullAddress
                e.Row.Cells[10].Visible = false; //credit_term
                e.Row.Cells[12].Visible = false; //AddressShipping1
                e.Row.Cells[13].Visible = false; //AddressShipping2
                e.Row.Cells[14].Visible = false; //AddressShipping3
                e.Row.Cells[15].Visible = false; //AddressShipping4
                e.Row.Cells[17].Visible = false; //Credit_Limit
                e.Row.Cells[18].Visible = false; //Contact_Number
                e.Row.Cells[19].Visible = false; //TIN_Number
                e.Row.Cells[20].Visible = false; //Salesman
                e.Row.Cells[21].Visible = false; //Email_Address
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false; //idCustomer
                e.Row.Cells[2].Visible = false; //CustomerName
                e.Row.Cells[4].Visible = false; //Address1
                e.Row.Cells[5].Visible = false; //Address2
                e.Row.Cells[6].Visible = false; //Address3
                e.Row.Cells[7].Visible = false; //Address4
                e.Row.Cells[8].Visible = false; //FullAddress
                e.Row.Cells[10].Visible = false; //credit_term
                e.Row.Cells[12].Visible = false; //AddressShipping1
                e.Row.Cells[13].Visible = false; //AddressShipping2
                e.Row.Cells[14].Visible = false; //AddressShipping3
                e.Row.Cells[15].Visible = false; //AddressShipping4
                e.Row.Cells[17].Visible = false; //Credit_Limit
                e.Row.Cells[18].Visible = false; //Contact_Number
                e.Row.Cells[19].Visible = false; //TIN_Number
                e.Row.Cells[20].Visible = false; //Salesman
                e.Row.Cells[21].Visible = false; //Email_Address
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grvCustomerDetails, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.ToolTip = "Click to select this row.";
            }

        }
                protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetCustomerDetails();
        }
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Session["Mode"] = "A";

            pnlCustomerDetails.Visible = true;
            CustomerPanel.Visible = false;
            tblSearch.Visible = false;
            btnAddNew.Visible = false;
            btnBack.Visible = true;
            btnSave.Visible = true;

            txtCustomerName.ReadOnly = false;
            txtCompanyName.ReadOnly = false;
            txtAddress1.ReadOnly = false;
            txtAddress2.ReadOnly = false;
            txtAddress3.ReadOnly = false;
            txtAddress4.ReadOnly = false;
            txtCreditTerm.ReadOnly = false;
            txtCustomerType.ReadOnly = false;
            txtSalesman.ReadOnly = false;
            txtEmailAddress.ReadOnly = false;
            txtTINNo.ReadOnly = false;
            ddCreditTerm.Visible = true;
            ddCustomerType.Visible = true;
            ddSalesman.Visible = true;
            ddStatus.Visible = true;
            txtCreditTerm.Visible = false;
            txtCustomerType.Visible = false;
            txtStatus.Visible = false;
            pnlAllDetails.Visible = true;
            txtShipAddress1.ReadOnly = false;
            txtShipAddress2.ReadOnly = false;
            txtShipAddress3.ReadOnly = false;
            txtShipAddress4.ReadOnly = false;
            txtCreditLimit.ReadOnly = false;
            txtPosition.ReadOnly = false;
            txtContactNo.ReadOnly = false;
            txtSalesman.Visible = false;

            statusdiv.Visible = false;
            ddStatus.SelectedValue = "Active";
            chckCopy.Visible = true;

            GetCreditTermToDD();

        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            defaultSettings();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            var objCustomerDetails = new Customer_Details_Model();

            if (txtCompanyName.Text != "")
            {
                string CustCode = Customer_Details.GetLastCustomerCode(oCon);
                if (CustCode == null)
                {
                    CustCode = "CC000000";
                }
                CustCode = CustCode.Substring(2);
                int CustCode_ = int.Parse(CustCode) + 1;

                string Customer_Code = "CC" + CustCode_.ToString("000000");

                objCustomerDetails.Customer_Name = txtCustomerName.Text;
                objCustomerDetails.Address1 = txtAddress1.Text;
                objCustomerDetails.Address2 = txtAddress2.Text;
                objCustomerDetails.Address3 = txtAddress3.Text;
                objCustomerDetails.Address4 = txtAddress4.Text;
                objCustomerDetails.credit_term = ddCreditTerm.SelectedItem.Text;
                objCustomerDetails.Company_Name = txtCompanyName.Text;
                objCustomerDetails.Status = ddStatus.SelectedItem.Text;
                objCustomerDetails.AddressShipping1 = txtShipAddress1.Text;
                objCustomerDetails.AddressShipping2 = txtShipAddress2.Text;
                objCustomerDetails.AddressShipping3 = txtShipAddress3.Text;
                objCustomerDetails.AddressShipping4 = txtShipAddress4.Text;
                objCustomerDetails.user_chg_by = Session["User_Domain"].ToString();
                objCustomerDetails.Customer_Type = ddCustomerType.SelectedItem.Text;
                objCustomerDetails.Credit_Limit = txtCreditLimit.Text;
                objCustomerDetails.Position = txtPosition.Text;
                objCustomerDetails.Contact_Number = txtContactNo.Text;
                objCustomerDetails.TIN_Number = txtTINNo.Text;
                objCustomerDetails.Salesman = ddSalesman.SelectedItem.Text;
                objCustomerDetails.Email_Address = txtEmailAddress.Text;



                if (Session["Mode"].ToString() == "A")
                {
                    objCustomerDetails.Customer_Code = Customer_Code;
                    Customer_Details.Save(oCon, objCustomerDetails);
                }
                else if (Session["Mode"].ToString() == "E")
                {
                    objCustomerDetails.idCustomer = Convert.ToInt32(Session["idCustomer"]);
                    objCustomerDetails.Customer_Code = Session["CustomerCode"].ToString();
                    Customer_Details.Update(oCon, objCustomerDetails);
                }
                var objLogs = new Logs_Model();

                objLogs.idUser = Users.GetUserIDByDomainLogin(oCon, Session["User_Domain"].ToString());

                objLogs.Form = "Customer Maintenance";

                if (Session["Mode"].ToString() == "A")
                {
                    objLogs.Description = "Save Record: Customer_Code = " + Customer_Code + "";
                }
                if (Session["Mode"].ToString() == "E")
                {
                    objLogs.Description = "Update Record: Customer_Code = " + Session["CustomerCode"].ToString() + "";
                }
                Logs.Save(oCon, objLogs);


                defaultSettings();
                GetCustomerDetails();

                if (Session["Mode"].ToString() == "A")
                {
                    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Customer record has been saved. Customer Code : " + Customer_Code + ControlChars.Quote + ");</script>");
                }
                else if (Session["Mode"].ToString() == "E")
                {
                    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Update Successful. Customer Code : " + Session["CustomerCode"].ToString() + ControlChars.Quote + ");</script>");
                }
            }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {

            btnEdit.Visible = false;
            btnSave.Visible = true;

            txtCustomerName.ReadOnly = false;
            txtCompanyName.ReadOnly = false;
            txtAddress1.ReadOnly = false;
            txtAddress2.ReadOnly = false;
            txtAddress3.ReadOnly = false;
            txtAddress4.ReadOnly = false;
            txtCreditTerm.ReadOnly = false;
            txtCustomerType.ReadOnly = false;
            txtCreditLimit.ReadOnly = false;
            txtPosition.ReadOnly = false;
            txtContactNo.ReadOnly = false;
            txtTINNo.ReadOnly = false;
            txtSalesman.ReadOnly = false;
            ddCreditTerm.Visible = true;
            ddCustomerType.Visible = true;
            txtCustomerType.Visible = false;
            ddSalesman.Visible = true;
            txtSalesman.Visible = false;
            txtCreditTerm.Visible = false;
            ddStatus.Visible = true;
            txtStatus.Visible = false;
            txtShipAddress1.ReadOnly = false;
            txtShipAddress2.ReadOnly = false;
            txtShipAddress3.ReadOnly = false;
            txtShipAddress4.ReadOnly = false;
            txtEmailAddress.ReadOnly = false;
            statusdiv.Visible = true;

        }
        protected void GetCustomerDetails()
        {
            var oCustomerDetails = new clsCustomerMaintenance();
            var ldata = oCustomerDetails.RetrieveCustomerDetails(ddSearchBy.Text.Trim(), txtSearch.Text);

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
            var lData = Users.GetUserWithProduct(oCon);

            ddSalesman.Items.Clear();
            ddSalesman.DataSource = lData;
            ddSalesman.DataTextField = "User_Name";
            ddSalesman.DataValueField = "User_Name";

            ddSalesman.DataBind();
        }
        protected void defaultSettings()
        {
            CustomerPanel.Visible = true;
            pnlCustomerDetails.Visible = false;
            pnlAllDetails.Visible = false;
            tblSearch.Visible = true;
            btnAddNew.Visible = true;
            btnSave.Visible = false;
            btnBack.Visible = false;
            btnEdit.Visible = false;
            tblSearch.Visible = true;

            txtSearch.Text = "";
            txtCustomerName.Text = "";
            txtCompanyName.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtAddress4.Text = "";
            ddCreditTerm.SelectedIndex = 0;
            ddCustomerType.SelectedIndex = 0;
            txtShipAddress1.Text = "";
            txtShipAddress2.Text = "";
            txtShipAddress3.Text = "";
            txtShipAddress4.Text = "";
            txtCreditLimit.Text = "";
            txtPosition.Text = "";
            txtContactNo.Text = "";
            txtTINNo.Text = "";
            txtSalesman.Text = "";
            txtEmailAddress.Text = "";
            chckCopy.Checked = false;

        }
        protected void grvCustomerDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvCustomerDetails.PageIndex = e.NewPageIndex;
            GetCustomerDetails();
        }
        protected void chckCopy_CheckChanged(object sender, EventArgs e)
        {
            if (chckCopy.Checked == true)
            {
                txtShipAddress1.Text = txtAddress1.Text;
                txtShipAddress2.Text = txtAddress2.Text;
                txtShipAddress3.Text = txtAddress3.Text;
                txtShipAddress4.Text = txtAddress4.Text;
            }
            else
            {
                txtShipAddress1.Text = "";
                txtShipAddress2.Text = "";
                txtShipAddress3.Text = "";
                txtShipAddress4.Text = "";
            }
        }
    }
}