using POSOINV.Functions;
using POSOINV.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOPOINV.Forms
{
    public partial class Announcement : System.Web.UI.Page
    {
        SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {

            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            //lblDomainLogin.Text = System.Convert.ToString(Session.Contents["Login"]);
            //Session["User_Name"].ToString() = System.Convert.ToString(Session.Contents["FName"]) + " " + System.Convert.ToString(Session.Contents["LName"]);

            if (!Page.IsPostBack)
            {
                string username;
                string access = "";

                for (int x = Session.Count - 1; x >= 5; x--)
                {
                    Session.RemoveAt(x);
                }

                try
                {
                    username = Session["User_Domain"].ToString();
                    access = Session["User_Access"].ToString();
                }
                catch
                {
                    username = "";
                }

                if (username != "")
                {
                    if (access == "IT" || access == "PR")
                    {
                        defaultSettings();
                        GetPO();
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
            //btnEdit.Visible = true;
            btnBack.Visible = true;
            btnSave.Visible = true;
            POPanel.Visible = false;
            pnlAllDetails.Visible = true;
            //btnPrint.Visible = true;
            tblSearch.Visible = false;

            GridViewRow row = grvMainPO.SelectedRow;
            Session["idPOHeader"] = row.Cells[0].Text.Trim();
            lblPONumber.Text = row.Cells[1].Text.Trim();
            txtTerms.Text = row.Cells[4].Text.Trim();
            txtPOQuantity.Text = row.Cells[9].Text.Trim();
            txtForexRate.Text = row.Cells[12].Text.Trim();
            txtPOAmount.Text = row.Cells[10].Text.Trim();
            txtTotalCharges.Text = row.Cells[11].Text.Trim();
            txtRemarks.Text = row.Cells[15].Text.Trim();
            txtImpShpNum.Text = row.Cells[17].Text.Trim();
            GetSavedItems(row.Cells[16].Text.Trim());
            lblPONumber.Visible = true;
            lblPONumber_.Visible = true;
            //gvItems.Enabled = false;
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
                e.Row.Cells[16].Visible = true; //Status
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
                e.Row.Cells[16].Visible = true; //Status
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;//idItem
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;//idPODetail
                e.Row.Cells[1].Visible = false;//idPOHeader
                e.Row.Cells[2].Visible = false;//idItem
                e.Row.Cells[3].Text = "Item Description";
                e.Row.Cells[9].Text = "Final Cost (PHP)";
                e.Row.Cells[10].Text = "Unit Computation";
            }
        }
        protected void grvMainPO_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvMainPO.PageIndex = e.NewPageIndex;
            GetPO();
        }

        protected void GetPO()
        {
            var ldata = PO_Header.RetrieveData(oCon, txtSearch.Text.Trim());
            grvMainPO.DataSource = ldata;
            grvMainPO.DataBind();
            grvMainPO.GridLines = GridLines.None;
        }

        protected void GetSavedItems(string status)
        {
            var ldata = PO_Detail.RetrieveData(oCon, Session["idPOHeader"].ToString(), "");
            //Session["Data"] = ldata;
            gvItems.DataSource = ldata;
            gvItems.DataBind();
            gvItems.GridLines = GridLines.None;

            if (status == "Recieved" || status == "Announced")
            {
                var ldata2 = PO_Charges.RetrieveData(oCon, Session["idPOHeader"].ToString());
                txtBrokerage.Text = ldata2[0].Brokerage.ToString();
                txtCEDEC.Text = ldata2[0].CEDEC.ToString();
                txtCustomsStamps.Text = ldata2[0].CustomsStamps.ToString();
                txtDeliveryCharges.Text = ldata2[0].DeliveryCharges.ToString();
                txtDocumentaryStamps.Text = ldata2[0].DocumentaryStamps.ToString();
                txtDocumentationCharges.Text = ldata2[0].DocumentationCharges.ToString();
                txtForkliftCost.Text = ldata2[0].ForkliftCost.ToString();
                txtFreightCharges.Text = ldata2[0].FreightCharges.ToString();
                txtHandlingFee.Text = ldata2[0].HandlingFee.ToString();
                txtImportDuties.Text = ldata2[0].ImportDuties.ToString();
                txtImportProcessingFee.Text = ldata2[0].ImportProcessingFee.ToString();
                txtImportationInsurance.Text = ldata2[0].ImportationInsurance.ToString();
                txtMiscellaneous.Text = ldata2[0].Miscellaneous.ToString();
                txtNotarialFee.Text = ldata2[0].NotarialFee.ToString();
                txtOtherCharges.Text = ldata2[0].OtherCharges.ToString();
                txtProcessingFee.Text = ldata2[0].ProcessingFee.ToString();
                txtWarehouseStorageCharges.Text = ldata2[0].WarehouseStorageCharges.ToString();
                txtXerox.Text = ldata2[0].Xerox.ToString();

                //var detail = Import_Shipment_Detail.RetrieveData(oCon, 0, Convert.ToInt32(Session["idPOHeader"].ToString()));
                //var head = Import_Shipment_Header.RetrieveData(oCon, detail[0].idImpShpHead, "");
                //txtImpShpNum.Text = head[0].ImportShipmentNumber;

            }
            else if (status == "Sent")
            {
                txtBrokerage.Text = "0";
                txtCEDEC.Text = "0";
                txtCustomsStamps.Text = "0";
                txtDeliveryCharges.Text = "0";
                txtDocumentaryStamps.Text = "0";
                txtDocumentationCharges.Text = "0";
                txtForkliftCost.Text = "0";
                txtFreightCharges.Text = "0";
                txtHandlingFee.Text = "0";
                txtImportDuties.Text = "0";
                txtImportProcessingFee.Text = "0";
                txtImportationInsurance.Text = "0";
                txtMiscellaneous.Text = "0";
                txtNotarialFee.Text = "0";
                txtOtherCharges.Text = "0";
                txtProcessingFee.Text = "0";
                txtWarehouseStorageCharges.Text = "0";
                txtXerox.Text = "0";
            }
        }

        protected void defaultSettings()
        {
            POPanel.Visible = true;
            pnlAllDetails.Visible = false;
            tblSearch.Visible = true;
            btnBack.Visible = false;
            btnSave.Visible = false;
            lblPONumber.Visible = false;
            lblPONumber_.Visible = false;
            Session["ComputeClick"] = false;
            //Session.Remove("Data");
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
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetPO();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetPO();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(Session["ComputeClick"]) == true)
            {
                foreach (GridViewRow row in gvItems.Rows)
                {
                    var objPODetail = new PO_Detail_Model
                    {
                        idPODetail = int.Parse(row.Cells[0].Text),
                        idPOHeader = int.Parse(row.Cells[1].Text),
                        idItem = int.Parse(row.Cells[2].Text),
                        Quantity = decimal.Parse(row.Cells[5].Text),
                        Price = decimal.Parse(row.Cells[6].Text),
                        Tax = decimal.Parse(row.Cells[7].Text),
                        Amount = decimal.Parse(row.Cells[8].Text),
                        Final_Cost = decimal.Parse(row.Cells[9].Text),
                        Unit_Comp = row.Cells[10].Text,
                        isReceived = "No",
                        Partial_Remaining = 0
                    };
                    PO_Detail.Update(oCon, objPODetail);
                }

                var pO_Charges_Model = new PO_Charges_Model
                {
                    idPOHeader = Convert.ToInt32(Session["idPOHeader"].ToString()),
                    Brokerage = Convert.ToDecimal(txtBrokerage.Text),
                    CEDEC = Convert.ToDecimal(txtCEDEC.Text),
                    CustomsStamps = Convert.ToDecimal(txtCustomsStamps.Text),
                    DeliveryCharges = Convert.ToDecimal(txtDeliveryCharges.Text),
                    DocumentaryStamps = Convert.ToDecimal(txtDocumentaryStamps.Text),
                    DocumentationCharges = Convert.ToDecimal(txtDocumentationCharges.Text),
                    ForkliftCost = Convert.ToDecimal(txtForkliftCost.Text),
                    FreightCharges = Convert.ToDecimal(txtFreightCharges.Text),
                    HandlingFee = Convert.ToDecimal(txtHandlingFee.Text),
                    ImportationInsurance = Convert.ToDecimal(txtImportationInsurance.Text),
                    ImportDuties = Convert.ToDecimal(txtImportDuties.Text),
                    ImportProcessingFee = Convert.ToDecimal(txtImportProcessingFee.Text),
                    Miscellaneous = Convert.ToDecimal(txtMiscellaneous.Text),
                    NotarialFee = Convert.ToDecimal(txtNotarialFee.Text),
                    OtherCharges = Convert.ToDecimal(txtOtherCharges.Text),
                    ProcessingFee = Convert.ToDecimal(txtProcessingFee.Text),
                    WarehouseStorageCharges = Convert.ToDecimal(txtWarehouseStorageCharges.Text),
                    Xerox = Convert.ToDecimal(txtXerox.Text)
                };
                PO_Charges.Save(oCon, pO_Charges_Model);

                var poheader = new PO_Header_Model
                {
                    idPOHeader = Convert.ToInt32(Session["idPOHeader"].ToString()),
                    POStatus = "Announced",
                    Total_Charges = Convert.ToDecimal(txtTotalCharges.Text),
                    ImportShipmentNumber = txtImpShpNum.Text
                };
                PO_Header.UpdatePOStatus(oCon, poheader);
                PO_Header.UpdateTotalCharges(oCon, poheader);

                defaultSettings();
                GetPO();
            }
            else
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Cost not yet computed!" + ControlChars.Quote + ");</script>");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            defaultSettings();
            clearFlds();
            //Session.Remove("Data");
            GetPO();
        }

        private decimal computeCost(string idItem, string itemQuantity, string itemCost)
        {
            decimal dCharges = 0;
            decimal dFinalPOAmount = 0;
            decimal dPercentage = 0;
            decimal dComputedCharge = 0;
            decimal dItemCost = 0;
            decimal dFinalItemCost = 0;
            decimal dItemCostQuantity = 0;
            decimal dItemQuantity = Convert.ToDecimal(itemQuantity);
            decimal dPOQuantity = Convert.ToDecimal(txtPOQuantity.Text);

            //Convert.ToDecimal(txtBrokerage.Text);
            //Convert.ToDecimal(txtCEDEC.Text);
            //Convert.ToDecimal(txtCustomsStamps.Text);
            //Convert.ToDecimal(txtDeliveryCharges.Text);
            //Convert.ToDecimal(txtDocumentaryStamps.Text);
            //Convert.ToDecimal(txtDocumentationCharges.Text);
            //Convert.ToDecimal(txtForkliftCost.Text);
            //Convert.ToDecimal(txtFreightCharges.Text);
            //Convert.ToDecimal(txtHandlingFee.Text);
            //Convert.ToDecimal(txtImportDuties.Text);
            //Convert.ToDecimal(txtImportProcessingFee.Text);
            //Convert.ToDecimal(txtImportationInsurance.Text);
            //Convert.ToDecimal(txtMiscellaneous.Text);
            //Convert.ToDecimal(txtNotarialFee.Text);
            //Convert.ToDecimal(txtOtherCharges.Text);
            //Convert.ToDecimal(txtProcessingFee.Text);
            //Convert.ToDecimal(txtWarehouseStorageCharges.Text);
            //Convert.ToDecimal(txtXerox.Text);

            dCharges = Convert.ToDecimal(txtTotalCharges.Text);

            //Convert.ToDecimal(txtBrokerage.Text) + Convert.ToDecimal(txtCEDEC.Text) + Convert.ToDecimal(txtCustomsStamps.Text) + Convert.ToDecimal(txtDeliveryCharges.Text) + Convert.ToDecimal(txtDocumentaryStamps.Text) + Convert.ToDecimal(txtDocumentationCharges.Text) + Convert.ToDecimal(txtForkliftCost.Text) + Convert.ToDecimal(txtFreightCharges.Text) + Convert.ToDecimal(txtHandlingFee.Text) + Convert.ToDecimal(txtImportDuties.Text) + Convert.ToDecimal(txtImportProcessingFee.Text) + Convert.ToDecimal(txtImportationInsurance.Text) + Convert.ToDecimal(txtMiscellaneous.Text) + Convert.ToDecimal(txtNotarialFee.Text) + Convert.ToDecimal(txtOtherCharges.Text) + Convert.ToDecimal(txtProcessingFee.Text) + Convert.ToDecimal(txtWarehouseStorageCharges.Text) + Convert.ToDecimal(txtXerox.Text);

            if (txtForexRate.Text.Trim() != "" && Decimal.ToInt32(Convert.ToDecimal(txtForexRate.Text)) != 0)
            {
                dFinalPOAmount = Convert.ToDecimal(txtForexRate.Text) * Convert.ToDecimal(txtPOAmount.Text);
                dItemCost = Convert.ToDecimal(txtForexRate.Text) * Convert.ToDecimal(itemCost);
            }
            else
            {
                dFinalPOAmount = Convert.ToDecimal(txtPOAmount.Text);
                dItemCost = Convert.ToDecimal(itemCost);
            }

            if (ddlPurchase.SelectedIndex == 1)
            {
                dPercentage = (dItemCost * dItemQuantity) / dFinalPOAmount;
                dComputedCharge = dPercentage * dCharges;
                dItemCostQuantity = dItemCost * dItemQuantity;
                dFinalItemCost = (dComputedCharge + dItemCostQuantity) / dItemQuantity;
            }
            else if (ddlPurchase.SelectedIndex == 2)
            {
                dComputedCharge = dCharges / dPOQuantity;
                dFinalItemCost = dComputedCharge + dItemCost;
            }

            if (ddlUnitPrice.SelectedIndex == 1)
            {
                dFinalItemCost = Convert.ToDecimal(dFinalItemCost.ToString("N4"));
            }
            else if (ddlUnitPrice.SelectedIndex == 2)
            {
                DataTable averageCost = Item_Serial.AverageCosting(oCon, idItem);
                decimal dTotalInventoryCost = 0;
                //decimal dAverageCost = 0;
                int inventoryStock = 0;

                for (int x = 0; x <= averageCost.Rows.Count - 1; x++)
                {
                    dTotalInventoryCost = dTotalInventoryCost + Convert.ToDecimal(averageCost.Rows[x][3].ToString());
                    inventoryStock = inventoryStock + Convert.ToInt32(averageCost.Rows[x][1].ToString());
                }
                dFinalItemCost = ((dFinalItemCost * dItemQuantity) + dTotalInventoryCost) / (dItemQuantity + inventoryStock);
                dFinalItemCost = Convert.ToDecimal(dFinalItemCost.ToString("N4"));
            }

            dFinalItemCost = Convert.ToDecimal(Convert.ToDouble(dFinalItemCost) * 1.02);

            return dFinalItemCost;
        }

        protected void btnCompute_Click(object sender, EventArgs e)
        {
            if (ddlPurchase.SelectedValue != "0" && ddlUnitPrice.SelectedValue != "0")
            {
                //txtTotalCharges.Text = (Convert.ToDecimal(txtBrokerage.Text) + Convert.ToDecimal(txtCEDEC.Text) + Convert.ToDecimal(txtCustomsStamps.Text) + Convert.ToDecimal(txtDeliveryCharges.Text) + Convert.ToDecimal(txtDocumentaryStamps.Text) + Convert.ToDecimal(txtDocumentationCharges.Text) + Convert.ToDecimal(txtForkliftCost.Text) + Convert.ToDecimal(txtFreightCharges.Text) + Convert.ToDecimal(txtHandlingFee.Text) + Convert.ToDecimal(txtImportDuties.Text) + Convert.ToDecimal(txtImportProcessingFee.Text) + Convert.ToDecimal(txtImportationInsurance.Text) + Convert.ToDecimal(txtMiscellaneous.Text) + Convert.ToDecimal(txtNotarialFee.Text) + Convert.ToDecimal(txtOtherCharges.Text) + Convert.ToDecimal(txtProcessingFee.Text) + Convert.ToDecimal(txtWarehouseStorageCharges.Text) + Convert.ToDecimal(txtXerox.Text)).ToString("N4");

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("idPODetail"));
                dt.Columns.Add(new DataColumn("idPOHeader"));
                dt.Columns.Add(new DataColumn("idItem"));
                dt.Columns.Add(new DataColumn("Item_Number"));
                dt.Columns.Add(new DataColumn("Description"));
                dt.Columns.Add(new DataColumn("Quantity"));
                dt.Columns.Add(new DataColumn("Price"));
                dt.Columns.Add(new DataColumn("Tax"));
                dt.Columns.Add(new DataColumn("Amount"));
                dt.Columns.Add(new DataColumn("Final_Cost"));
                dt.Columns.Add(new DataColumn("Unit_Comp"));

                foreach (GridViewRow row in gvItems.Rows)
                {
                    decimal itemCost = computeCost(row.Cells[2].Text, row.Cells[5].Text, row.Cells[6].Text);

                    DataRow dRow = dt.NewRow();
                    dRow["idPODetail"] = row.Cells[0].Text;
                    dRow["idPOHeader"] = row.Cells[1].Text;
                    dRow["idItem"] = row.Cells[2].Text;
                    dRow["Item_Number"] = row.Cells[3].Text;
                    dRow["Description"] = row.Cells[4].Text;
                    dRow["Quantity"] = row.Cells[5].Text;
                    dRow["Price"] = row.Cells[6].Text;
                    dRow["Tax"] = row.Cells[7].Text;
                    dRow["Amount"] = row.Cells[8].Text;
                    dRow["Final_Cost"] = itemCost;
                    dRow["Unit_Comp"] = ddlUnitPrice.SelectedItem.Text;
                    dt.Rows.Add(dRow);
                }

                gvItems.DataSource = dt;
                gvItems.DataBind();

                Session["ComputeClick"] = true;
            }
            else
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Please select compute mode." + ControlChars.Quote + ");</script>");
            }
        }

        protected void btnSearchImpShpNum_Click(object sender, EventArgs e)
        {
            try
            {
                var ldata2 = Import_Shipment_Header.RetrieveData(oCon, txtImpShpNum.Text.Trim());
                var dtCount = Import_Shipment_Detail.RetrieveData(oCon, ldata2[0].idImpShpHead);
                var impdet = Import_Shipment_Detail.RetrieveData(oCon, ldata2[0].idImpShpHead, Convert.ToInt32(Session["idPOHeader"].ToString()));
                txtTotalCharges.Text = impdet[0].POCharge.ToString();

                var count = dtCount.Rows.Count;

                txtBrokerage.Text = (ldata2[0].Brokerage / count).ToString("N4");
                txtCEDEC.Text = (ldata2[0].CEDEC / count).ToString("N4");
                txtCustomsStamps.Text = (ldata2[0].CustomsStamps / count).ToString("N4");
                txtDeliveryCharges.Text = (ldata2[0].DeliveryCharges / count).ToString("N4");
                txtDocumentaryStamps.Text = (ldata2[0].DocumentaryStamps / count).ToString("N4");
                txtDocumentationCharges.Text = (ldata2[0].DocumentationCharges / count).ToString("N4");
                txtForkliftCost.Text = (ldata2[0].ForkliftCost / count).ToString("N4");
                txtFreightCharges.Text = (ldata2[0].FreightCharges / count).ToString("N4");
                txtHandlingFee.Text = (ldata2[0].HandlingFee / count).ToString("N4");
                txtImportDuties.Text = (ldata2[0].ImportDuties / count).ToString("N4");
                txtImportProcessingFee.Text = (ldata2[0].ImportProcessingFee / count).ToString("N4");
                txtImportationInsurance.Text = (ldata2[0].ImportationInsurance / count).ToString("N4");
                txtMiscellaneous.Text = (ldata2[0].Miscellaneous / count).ToString("N4");
                txtNotarialFee.Text = (ldata2[0].NotarialFee / count).ToString("N4");
                txtOtherCharges.Text = (ldata2[0].OtherCharges / count).ToString("N4");
                txtProcessingFee.Text = (ldata2[0].ProcessingFee / count).ToString("N4");
                txtWarehouseStorageCharges.Text = (ldata2[0].WarehouseStorageCharges / count).ToString("N4");
                txtXerox.Text = (ldata2[0].Xerox / count).ToString("N4");
                txtImpShpNum.Text = ldata2[0].ImportShipmentNumber;
            }
            catch
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "PO not linked to Import Shipment Number!" + ControlChars.Quote + ");</script>");
            }
        }
    }
}