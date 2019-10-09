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
using SHIELD.ERP;
using System.Configuration;

namespace SOPOINV.Forms
{
    public partial class POCreation : System.Web.UI.Page
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

                Session["idProduct"] = Users.GetidProduct((int)Session["idUser"], oCon);

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
                    if (access == "IT" || access == "PR" || access == "OP")
                    {
                        defaultSettings();
                        GetPO();
                        GetItemMaster();
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


        protected void grvMainPO_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["Mode"] = "View";

            GridViewRow row = grvMainPO.SelectedRow;
            Session["idPOHeader"] = row.Cells[0].Text.Trim();
            lblPONumber.Text = row.Cells[1].Text.Trim();
            txtTerms.Text = row.Cells[4].Text.Trim();
            txtPOQuantity.Text = row.Cells[9].Text.Trim();
            txtForexRate.Text = row.Cells[12].Text.Trim();
            txtPOAmount.Text = row.Cells[10].Text.Trim();
            //txtTotalCharges.Text = row.Cells[11].Text.Trim();
            txtRemarks.Text = row.Cells[15].Text.Trim();
            string status = row.Cells[16].Text.Trim();

            if (status == "Created")
            {
                btnEdit.Visible = true;
            }
            else
            {
                btnEdit.Visible = false;
            }

            btnPrintPO.Visible = true;
            btnAdd.Visible = false;
            btnBack.Visible = true;
            POPanel.Visible = false;
            pnlAllDetails.Visible = true;
            btnAddNew.Visible = false;
            //btnPrint.Visible = true;
            tblSearch.Visible = false;

            GetSavedItems();
            lblPONumber.Visible = true;
            lblPONumber_.Visible = true;
            //gvItems.Enabled = false;
            btnSave.Visible = false;
        }

        protected void grvMainPO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false; //id
                e.Row.Cells[1].Visible = true; //po number
                e.Row.Cells[2].Visible = false; //po date
                e.Row.Cells[3].Visible = false; //delivery date
                e.Row.Cells[4].Visible = true; //terms
                e.Row.Cells[5].Visible = false; //FOB Point
                e.Row.Cells[6].Visible = false; //Shipping Via
                e.Row.Cells[7].Visible = false; //Currency
                e.Row.Cells[8].Visible = false; //idSupplier
                e.Row.Cells[9].Visible = true; //quantity
                e.Row.Cells[10].Visible = true; //price
                e.Row.Cells[11].Visible = false; //total
                e.Row.Cells[12].Visible = false; //forex rate
                e.Row.Cells[13].Visible = false; //PR number
                e.Row.Cells[14].Visible = false; //Created by
                e.Row.Cells[15].Visible = false; //remarks
                e.Row.Cells[16].Visible = true; //status
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false; //id
                e.Row.Cells[1].Visible = true; //po number
                e.Row.Cells[2].Visible = false; //po date
                e.Row.Cells[3].Visible = false; //delivery date
                e.Row.Cells[4].Visible = true; //terms
                e.Row.Cells[5].Visible = false; //FOB Point
                e.Row.Cells[6].Visible = false; //Shipping Via
                e.Row.Cells[7].Visible = false; //Currency
                e.Row.Cells[8].Visible = false; //idSupplier
                e.Row.Cells[9].Visible = true; //quantity
                e.Row.Cells[10].Visible = true; //price
                e.Row.Cells[11].Visible = false; //total charges
                e.Row.Cells[12].Visible = false; //forex rate
                e.Row.Cells[13].Visible = false; //PR number
                e.Row.Cells[14].Visible = false; //Created by
                e.Row.Cells[15].Visible = false; //remarks
                e.Row.Cells[16].Visible = true; //status
                e.Row.Cells[1].Text = "PO Number";
                e.Row.Cells[4].Text = "Terms";
                e.Row.Cells[9].Text = "Quantity";
                e.Row.Cells[10].Text = "Price";
                e.Row.Cells[14].Text = "Created By";
                e.Row.Cells[16].Text = "Status";
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grvMainPO, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.ToolTip = "Click to select this row.";
            }
        }
        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var lnkRemove = (LinkButton)e.Row.FindControl("lnkRemove");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Cells[0].Visible = false;
                if (Session["Mode"].ToString() == "View")
                {
                    lnkRemove.Visible = false;
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[1].Visible = false;
                }
                if (Session["Mode"].ToString() == "Add")
                {
                    lnkRemove.Visible = true;
                    e.Row.Cells[1].Visible = false;
                }
                if (Session["Mode"].ToString() == "Edit")
                {
                    lnkRemove.Visible = false;
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[1].Visible = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //e.Row.Cells[0].Visible = false;
                if (Session["Mode"].ToString() == "View")
                {
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[1].Visible = false;
                }
                if (Session["Mode"].ToString() == "Add")
                {
                    e.Row.Cells[1].Visible = false;
                }
                e.Row.Cells[2].Text = "Item Description";
                if (Session["Mode"].ToString() == "Edit")
                {
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[1].Visible = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["Mode"].ToString() == "Edit")
                {
                    e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvItems, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.ToolTip = "Click to select this row.";
                }
            }
        }
        protected void grvMainPO_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvMainPO.PageIndex = e.NewPageIndex;
            GetPO();
        }

        protected void grvItemClass_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvItemClass.PageIndex = e.NewPageIndex;
            GetItemMaster();
        }
        protected void grvItemClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = grvItemClass.SelectedRow;

            Session["idItem"] = row.Cells[1].Text.Trim();
            txtItemName.Text = row.Cells[3].Text.Trim() + " " + row.Cells[5].Text.Trim();
            Session["ItemNo"] = row.Cells[4].Text.Trim();
            Session["Description"] = row.Cells[5].Text.Trim();

            txtItemName.ReadOnly = true;
            myModal2.Visible = true;

            txtQuantity.Text = "";
            txtPrice.Text = "";
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
        protected void GetPO()
        {
            var ldata = PO_Header.RetrieveData(oCon, txtSearch.Text.Trim());
            grvMainPO.DataSource = ldata;
            grvMainPO.DataBind();
            grvMainPO.GridLines = GridLines.None;
        }
        protected void GetItemMaster()
        {
            var oSOCreation = new clsSOCreation();
            var ldata = oSOCreation.RetrieveItemMaster(ddItemSearchItem.Text.Trim(), txtSearchItem.Text, "");
            grvItemClass.DataSource = ldata;
            grvItemClass.DataBind();
            grvItemClass.GridLines = GridLines.None;
        }

        protected void GetSavedItems()
        {
            var ldata = PO_Detail.RetrieveForPOItems(oCon, Session["idPOHeader"].ToString());
            Session["Data"] = ldata;
            gvItems.DataSource = ldata;
            gvItems.DataBind();
            gvItems.GridLines = GridLines.None;
        }

        protected void defaultSettings()
        {
            POPanel.Visible = true;
            pnlAllDetails.Visible = false;
            btnAddNew.Visible = true;
            tblSearch.Visible = true;
            btnBack.Visible = false;
            btnSave.Visible = false;
            btnEdit.Visible = false;
            myModalWarning.Visible = false;
            btnPrintPO.Visible = false;
            lblPONumber.Visible = false;
            lblPONumber_.Visible = false;

            Session["Mode"] = "Add";
            Session.Remove("Data");
            Session.Remove("idPOHeader");
            Session.Remove("ItemNo");
            Session.Remove("Description");

            gvItems.DataSource = null;
            gvItems.DataBind();
        }

        protected void clearFlds()
        {
            txtTerms.Text = "0";
            txtPOQuantity.Text = "0";
            txtForexRate.Text = "0";
            txtPOAmount.Text = "0";
            txtItemName.Text = "";
            txtQuantity.Text = "";
            txtPrice.Text = "";
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetPO();
        }

        private void ddItemSearchItem_SelectedIndexChanged(object sender)
        {
            GetItemMaster();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetPO();
        }

        protected void btnSearchItem_Click(object sender, EventArgs e)
        {
            GetItemMaster();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Session["Mode"] = "Add";
            POPanel.Visible = false;
            pnlAllDetails.Visible = true;
            btnAddNew.Visible = false;
            tblSearch.Visible = false;
            btnBack.Visible = true;
            btnSave.Visible = true;
            btnAdd.Visible = true;
            clearFlds();
            //gvItems.Enabled = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["Mode"].ToString() == "Add")
            {
                string PONum = PO_Header.GetLastPONumber(oCon);
                if (PONum == null)
                {
                    PONum = "PJ000000";
                }
                PONum = PONum.Substring(2);
                int PONum_ = int.Parse(PONum) + 1;

                string po_number = "PJ" + PONum_.ToString("000000");

                var poHeader = new PO_Header_Model
                {
                    PO_Number = po_number,
                    PO_Date = DateTime.Now,
                    Delivery_Date = DateTime.Now,
                    Terms = txtTerms.Text,
                    FOB_Point = "",
                    Shipping_Via = "",
                    Currency = "USD",
                    idSupplier = 0,
                    PO_Total = Convert.ToDecimal(txtPOAmount.Text),
                    PO_Quantity = Convert.ToInt32(txtPOQuantity.Text),
                    Total_Charges = 0,
                    Forex_Rate = Convert.ToDecimal(txtForexRate.Text),
                    PR_Number = "",
                    Created_By = Session["User_Domain"].ToString(),
                    Remarks = txtRemarks.Text,
                    POStatus = "Created",
                    ImportShipmentNumber = ""
                };
                int idPOHeader = PO_Header.Save(oCon, poHeader);

                foreach (GridViewRow row in gvItems.Rows)
                {
                    int idItem = int.Parse(row.Cells[1].Text);
                    decimal Quantity = decimal.Parse(row.Cells[4].Text);
                    decimal Price = decimal.Parse(row.Cells[5].Text);
                    decimal Extended = decimal.Parse(row.Cells[6].Text);

                    var objPODetail = new PO_Detail_Model
                    {
                        idPOHeader = idPOHeader,
                        idItem = idItem,
                        Quantity = Quantity,
                        Price = Price,
                        Amount = Extended,
                        Tax = 0,
                        Final_Cost = 0,
                        Unit_Comp = "",
                        isReceived = "No",
                        Partial_Remaining = 0
                    };
                    PO_Detail.Save(oCon, objPODetail);
                }
                POUpload(po_number, idPOHeader);
            }
            else if (Session["Mode"].ToString() == "Edit")
            {
                int idPOHeader = Convert.ToInt32(Session["idPOHeader"].ToString());

                var poHeader = new PO_Header_Model
                {
                    PO_Number = lblPONumber.Text,
                    Terms = txtTerms.Text,
                    FOB_Point = "",
                    Shipping_Via = "",
                    Currency = "USD",
                    idSupplier = 0,
                    PO_Total = Convert.ToDecimal(txtPOAmount.Text),
                    PO_Quantity = Convert.ToInt32(txtPOQuantity.Text),
                    Total_Charges = 0,
                    Forex_Rate = Convert.ToDecimal(txtForexRate.Text),
                    Created_By = Session["User_Domain"].ToString(),
                    Remarks = txtRemarks.Text,
                    POStatus = "Created",
                    idPOHeader = idPOHeader,
                    ImportShipmentNumber = "" 
                };
                PO_Header.Update(oCon, poHeader);
                            
                bool result = PO_Detail.DeleteAllDetail(oCon, idPOHeader);

                if (result == true)
                {
                    foreach (GridViewRow row in gvItems.Rows)
                    {
                        int idItem = int.Parse(row.Cells[1].Text);
                        decimal Quantity = decimal.Parse(row.Cells[4].Text);
                        decimal Price = decimal.Parse(row.Cells[5].Text);
                        decimal Extended = decimal.Parse(row.Cells[6].Text);

                        var objPODetail = new PO_Detail_Model
                        {
                            idPOHeader = idPOHeader,
                            idItem = idItem,
                            Quantity = Quantity,
                            Price = Price,
                            Amount = Extended,
                            Tax = 0,
                            Final_Cost = 0,
                            Unit_Comp = "",
                            isReceived = "No",
                            Partial_Remaining = 0
                        };
                        PO_Detail.Save(oCon, objPODetail);
                    }
                    POUpload(lblPONumber.Text, idPOHeader);
                }
                else
                {
                    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Error in deleting po detail of " + lblPONumber.Text + ControlChars.Quote + ");</script>");
                }
            }

            defaultSettings();
            GetPO();
        }
        protected void btnAddToTempGv_Click(object sender, EventArgs e)
        {

            if (txtQuantity.Text == "" || txtPrice.Text == "")
            {
                lblItemWarning.Text = "Please fill all required fields.";
                myModalWarning.Visible = true;
            }
            else
            {
                decimal Quantity = decimal.Parse(txtQuantity.Text);
                decimal Price = decimal.Parse(txtPrice.Text);

                decimal Extended = Price * Quantity;

                if (Session["Data"] == null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("idItem"));
                    dt.Columns.Add(new DataColumn("Item_Number"));
                    dt.Columns.Add(new DataColumn("Description"));
                    dt.Columns.Add(new DataColumn("Quantity"));
                    dt.Columns.Add(new DataColumn("Price"));
                    dt.Columns.Add(new DataColumn("Amount"));

                    DataRow dRow = dt.NewRow();
                    dRow["idItem"] = Session["idItem"].ToString();
                    dRow["Item_Number"] = Session["ItemNo"].ToString();
                    dRow["Description"] = Session["Description"].ToString();
                    dRow["Quantity"] = Quantity.ToString("N4");
                    dRow["Price"] = Price.ToString("N4");
                    dRow["Amount"] = Extended.ToString("N4");
                    dt.Rows.Add(dRow);

                    if (Session["Mode"].ToString() == "Add")
                        Session["Mode"] = "Add";
                    else if (Session["Mode"].ToString() == "AddEdit")
                        Session["Mode"] = "Edit";

                    gvItems.DataSource = dt;
                    gvItems.DataBind();

                    txtPOQuantity.Text = (Convert.ToInt32(txtPOQuantity.Text) + Convert.ToInt32(Quantity)).ToString();
                    txtPOAmount.Text = (Convert.ToDecimal(txtPOAmount.Text) + Convert.ToDecimal(Extended.ToString("N4"))).ToString();

                    Session["Data"] = dt;
                }
                else
                {
                    if (Session["Mode"].ToString() == "Add" || Session["Mode"].ToString() == "AddEdit")
                    {
                        DataTable dt = new DataTable();
                        dt = (DataTable)Session["Data"];

                        DataRow dRow = dt.NewRow();
                        dRow["idItem"] = Session["idItem"].ToString();
                        dRow["Item_Number"] = Session["ItemNo"].ToString();
                        dRow["Description"] = Session["Description"].ToString();
                        dRow["Quantity"] = Quantity.ToString("N4");
                        dRow["Price"] = Price.ToString("N4");
                        dRow["Amount"] = Extended.ToString("N4");
                        dt.Rows.Add(dRow);

                        if (Session["Mode"].ToString() == "Add")
                            Session["Mode"] = "Add";
                        else if (Session["Mode"].ToString() == "AddEdit")
                            Session["Mode"] = "Edit";

                        gvItems.DataSource = dt;
                        gvItems.DataBind();

                        txtPOQuantity.Text = (Convert.ToInt32(txtPOQuantity.Text) + Convert.ToInt32(Quantity)).ToString();
                        txtPOAmount.Text = (Convert.ToDecimal(txtPOAmount.Text) + Convert.ToDecimal(Extended.ToString("N4"))).ToString();

                        Session.Remove("Data");
                        Session["Data"] = dt;
                    }
                    else if (Session["Mode"].ToString() == "Edit")
                    {
                        int rowindex = Convert.ToInt32(Session["RowIndex"].ToString());
                        DataTable dt = new DataTable();
                        dt = (DataTable)Session["Data"];
                        int inititalQty = decimal.ToInt32(Convert.ToDecimal(dt.Rows[rowindex][3].ToString()));
                        decimal initialAmount = Convert.ToDecimal(dt.Rows[rowindex][5].ToString());

                        dt.Rows[rowindex][3] = Quantity.ToString("N4");
                        dt.Rows[rowindex][4] = Price.ToString("N4");
                        dt.Rows[rowindex][5] = Extended.ToString("N4");

                        gvItems.DataSource = dt;
                        gvItems.DataBind();

                        txtPOQuantity.Text = (Convert.ToInt32(txtPOQuantity.Text) + Convert.ToInt32(Quantity) - inititalQty).ToString();
                        txtPOAmount.Text = (Convert.ToDecimal(txtPOAmount.Text) + Convert.ToDecimal(Extended.ToString("N4")) - initialAmount).ToString();

                        Session.Remove("Data");
                        Session["Data"] = dt;
                        Session["Mode"] = "Edit";
                    }
                }

                myModal.Visible = false;
                myModal2.Visible = false;
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            myModal.Visible = true;
            txtQuantity.Text = "";
            txtPrice.Text = "";
            if (Session["Mode"].ToString() == "Add")
                Session["Mode"] = "Add";
            else if (Session["Mode"].ToString() == "Edit")
                Session["Mode"] = "AddEdit";
        }

        protected void btnPrintPO_Click(object sender, EventArgs e)
        {
            POUpload(lblPONumber.Text, Convert.ToInt32(Session["idPOHeader"].ToString()));
            var objLogs = new Logs_Model();
            objLogs.idUser = Users.GetUserIDByDomainLogin(oCon, Session["User_Domain"].ToString());
            objLogs.Form = "POCreation";
            objLogs.Description = "Download PO Number : " + lblPONumber.Text + "";
            Logs.Save(oCon, objLogs);
        }
        protected void btnCancel1_Click(object sender, EventArgs e)
        {
            myModal.Visible = false;
        }
        protected void btnCancel2_Click(object sender, EventArgs e)
        {
            myModal2.Visible = false;
            if (Session["Mode"].ToString() == "Add")
                Session["Mode"] = "Add";
            else if (Session["Mode"].ToString() == "AddEdit")
                Session["Mode"] = "Edit";
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

            int quantity = decimal.ToInt32(Convert.ToDecimal(dt.Rows[rowindex][3].ToString()));
            decimal amount = Convert.ToDecimal(dt.Rows[rowindex][5].ToString());

            txtPOQuantity.Text = (Convert.ToInt32(txtPOQuantity.Text) - quantity).ToString();
            txtPOAmount.Text = (Convert.ToDecimal(txtPOAmount.Text) - amount).ToString();

            dt.Rows.Remove(dt.Rows[rowindex]);

            gvItems.DataSource = dt;
            gvItems.DataBind();

            Session.Remove("Data");
            Session["Data"] = dt;
        }

        public void hideButtons()
        {
            myModalConfirmation.Visible = false;
            btnEdit.Visible = false;
            btnPrintPO.Visible = false;
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
            Session.Remove("Data");
            GetPO();
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Session["Mode"] = "Edit";

            btnAdd.Visible = true;
            btnEdit.Visible = false;
            btnPrintPO.Visible = false;
            btnSave.Visible = true;
            GetSavedItems();

            //btnPrint.Visible = false;
            //DataTable dt = new DataTable();
            //string access = "";
            //access = Session["User_Access"].ToString();

            //btnSave.Visible = true;
            //dt.Columns.Add(new DataColumn("idItem"));
            //dt.Columns.Add(new DataColumn("Item_Number"));
            //dt.Columns.Add(new DataColumn("Description"));
            //dt.Columns.Add(new DataColumn("Qty"));
            //dt.Columns.Add(new DataColumn("Price"));
            //dt.Columns.Add(new DataColumn("Extended"));

            //foreach (GridViewRow row in gvItems.Rows)
            //{
            //    int idItem = int.Parse(row.Cells[1].Text);
            //    string Item_No = row.Cells[2].Text.Trim();
            //    string Description = row.Cells[3].Text.Trim();
            //    int Quantity = int.Parse(row.Cells[4].Text);
            //    decimal Price = decimal.Parse(row.Cells[5].Text);
            //    decimal Extended = decimal.Parse(row.Cells[6].Text);

            //    DataRow dRow = dt.NewRow();
            //    dRow["idItem"] = idItem;
            //    dRow["Item_Number"] = Item_No;
            //    dRow["Description"] = Description;
            //    dRow["Qty"] = Quantity;
            //    dRow["Price"] = Price.ToString("n", CultureInfo.GetCultureInfo("en-US")); ;
            //    dRow["Extended"] = Extended.ToString("n", CultureInfo.GetCultureInfo("en-US")); ;

            //    dt.Rows.Add(dRow);
            //}

            //gvItems.DataSource = dt;
            //gvItems.DataBind();

            //Session.Remove("Data");
            //Session["Data"] = dt;
        }

        private void POUpload(string po_number, int idPOHeader)
        {
            string remarks = "";

            if (txtRemarks.Text == "" || txtRemarks.Text == "&nbsp;" || txtRemarks.Text == "&amp;nbsp;")
            {
                remarks = " ";
            }
            else
            {
                remarks = txtRemarks.Text.Replace("\r\n", "_-_-");
            }

            string Terms = "";

            if (txtTerms.Text == "" || txtTerms.Text == "&nbsp;" || txtTerms.Text == "&amp;nbsp;")
            {
                Terms = " ";
            }
            else
            {
                Terms = txtTerms.Text;
            }

            string POAmount = "";

            if (txtPOAmount.Text == "" || txtPOAmount.Text == "&nbsp;" || txtPOAmount.Text == "&amp;nbsp;")
            {
                POAmount = " ";
            }
            else
            {
                POAmount = txtPOAmount.Text;
            }

            Response.Write(@"<script>window.open ('/Forms/DownloadReports/DownloadPurchaseOrder.aspx?po_number=" + po_number +
               "&Remarks=" + remarks +
               "&Terms=" + Terms +
               "&POAmount=" + POAmount +
               "&idPOHeader=" + idPOHeader +
               "','_blank');</script>");

            //ClientScript.RegisterStartupScript(this.GetType(), "alert", @"window.location.href='DownloadPurchaseOrder.aspx?po_number=" + po_number +
            //   "&Remarks=" + remarks +
            //   "&Terms=" + Terms +
            //   "&POAmount=" + POAmount +
            //   "&idPOHeader=" + idPOHeader +
            //   "';", true);

            //var lDataAdd = Session["Data"];

            //Microsoft.Reporting.WebForms.ReportViewer viewer = new ReportViewer();
            //viewer.ProcessingMode = ProcessingMode.Local;
            //viewer.LocalReport.ReportPath = Server.MapPath(@"~\Resources\LSIPurchaseOrder.rdlc");

            //ReportParameter p1 = new ReportParameter("Remarks", txtRemarks.Text);
            //ReportParameter p2 = new ReportParameter("Date", DateTime.Now.ToShortDateString());
            //ReportParameter p3 = new ReportParameter("PONumber", po_number);
            //ReportParameter p4 = new ReportParameter("SupplierName", "GN AUDIO SINGAPORE PTE LTD");
            //ReportParameter p5 = new ReportParameter("SupplierAddress", "150 Beach Road, #15-05/06, Gateway West, Singapore 189720");
            //ReportParameter p6 = new ReportParameter("ShipToName", "LSI Leading Technologies Inc.");
            //ReportParameter p7 = new ReportParameter("ShipToAddress", "4842 Valenzuela St., Zone 060 Brgy. 603 Sampaloc, Manila 1008");
            //ReportParameter p8 = new ReportParameter("SupplierTelNo", "-");
            //ReportParameter p9 = new ReportParameter("SupplierFaxNo", "-");
            //ReportParameter p10 = new ReportParameter("ShipToTelNo", "Tel. 632-727-0285, 713-0513");
            //ReportParameter p11 = new ReportParameter("ShipToFaxNo", "Fax: 632-721-8474");
            //ReportParameter p12 = new ReportParameter("Terms", txtTerms.Text);
            //ReportParameter p13 = new ReportParameter("SaleAmount", txtPOAmount.Text);
            //ReportParameter p14 = new ReportParameter("TotalAmount", txtPOAmount.Text);
            //ReportParameter p15 = new ReportParameter("VAT", "0.00");
            //ReportParameter p16 = new ReportParameter("Salesman", Session["User_Domain"].ToString());

            //viewer.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7,
            //p8, p9, p10, p11, p12, p13, p14, p15, p16});

            //ReportDataSource repDataSource1 = new ReportDataSource("POSet", lDataAdd);
            //viewer.LocalReport.DataSources.Clear();
            //viewer.LocalReport.DataSources.Add(repDataSource1);

            //Warning[] warnings;
            //string[] streamIds;
            //string mimeType = string.Empty;
            //string encoding = string.Empty;
            //string extension = "pdf";

            //byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            //Response.Buffer = true;
            //Response.Clear();
            //Response.ContentType = mimeType;
            //Response.AddHeader("content-disposition", "attachment; filename= " + po_number + "." + extension);
            //Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
            //Response.Flush(); // send it to the client to download  
            //Response.End();
        }

        protected void gvItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["Mode"] = "Edit";
            myModal2.Visible = true;
            GridViewRow row = gvItems.SelectedRow;
            Session["RowIndex"] = gvItems.SelectedRow.RowIndex;
            txtItemName.Text = "Jabra " + row.Cells[3].Text.Trim();
            txtQuantity.Text = row.Cells[4].Text.Trim();
            txtPrice.Text = row.Cells[5].Text.Trim();
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            int rowindex = Convert.ToInt32(Session["RowIndex"].ToString());
            DataTable dt = new DataTable();
            dt = (DataTable)Session["Data"];

            int quantity = decimal.ToInt32(Convert.ToDecimal(dt.Rows[rowindex][3].ToString()));
            decimal amount = Convert.ToDecimal(dt.Rows[rowindex][5].ToString());

            txtPOQuantity.Text = (decimal.ToInt32(Convert.ToDecimal(txtPOQuantity.Text)) - quantity ).ToString();
            //+ decimal.ToInt32(Convert.ToDecimal(txtQuantity.Text))
            txtPOAmount.Text = (Convert.ToDecimal(txtPOAmount.Text) - amount ).ToString("N4");
            //+(Convert.ToDecimal(txtPOQuantity.Text) * Convert.ToDecimal(txtPrice.Text))
            dt.Rows.Remove(dt.Rows[rowindex]);

            gvItems.DataSource = dt;
            gvItems.DataBind();

            Session.Remove("Data");
            Session["Data"] = dt;

            myModal2.Visible = false;
        }
    }
}