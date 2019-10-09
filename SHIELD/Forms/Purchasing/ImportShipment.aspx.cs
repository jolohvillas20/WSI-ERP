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

namespace SOPOINV.Forms.Purchasing
{
    public partial class ImportShipment : System.Web.UI.Page
    {
        SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {

            Page.Form.Attributes.Add("enctype", "multipart/form-data");


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
                        GetIS();
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

        protected void GetIS()
        {
            var ldata = Import_Shipment_Header.RetrieveData(oCon, txtSearch.Text.Trim());
            grvImpShp.DataSource = ldata;
            grvImpShp.DataBind();
            grvImpShp.GridLines = GridLines.None;
        }

        protected void GetSavedItems()
        {
            var ldata = Import_Shipment_Detail.RetrieveData(oCon, Convert.ToInt32(Session["idImpShpHead"].ToString()));
            //Session["Data"] = ldata;
            gvPO.DataSource = ldata;
            gvPO.DataBind();
            gvPO.GridLines = GridLines.None;

            var ldata2 = Import_Shipment_Header.RetrieveData(oCon, Convert.ToInt32(Session["idImpShpHead"].ToString()));
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
            txtImpShipNum.Text = ldata2[0].ImportShipmentNumber.ToString();
            txtTotalCharges.Text = ldata2[0].Total_Charges.ToString();

            POPanel.Visible = false;
            pnlAllDetails.Visible = true;
            btnBack.Visible = true;
            btnAddNew.Visible = false;
            btnCompute.Visible = false;
            tblSearch.Visible = false;
        }

        protected void defaultSettings()
        {
            POPanel.Visible = true;
            pnlAllDetails.Visible = false;
            tblSearch.Visible = true;
            btnBack.Visible = false;
            btnSave.Visible = false;
            btnAddNew.Visible = true;
            lblPONumber.Visible = false;
            lblPONumber_.Visible = false;
            Session["ComputeClick"] = false;
            //Session.Remove("Data");
            btnAdd.Visible = false;
            Session.Remove("idPOHeader");
            Session.Remove("ItemNo");
            Session.Remove("Description");
            Session["Data"] = null;
            gvPO.DataSource = null;
            gvPO.DataBind();
        }

        protected void clearFlds()
        {
            txtImpShipNum.Text = "";
            txtTotalCharges.Text = "0";
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetIS();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var import_Shipment_Header_Model = new Import_Shipment_Header_Model
            {
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
                Xerox = Convert.ToDecimal(txtXerox.Text),
                ImportShipmentNumber = txtImpShipNum.Text,
                Total_Charges = Convert.ToDecimal(txtTotalCharges.Text)
            };
            var ismHead = Import_Shipment_Header.Save(oCon, import_Shipment_Header_Model);

            DataTable dt = (DataTable)Session["Data"];

            for (int x = 0; x <= dt.Rows.Count - 1; x++)
            {
                var detail = new Import_Shipment_Detail_Model
                {
                    idImpShpHead = ismHead,
                    idPOHeader = Convert.ToInt32(dt.Rows[x][0].ToString()),
                    POCharge = Convert.ToDecimal(dt.Rows[x][2].ToString())
                };
                Import_Shipment_Detail.Save(oCon, detail);
            }

            defaultSettings();
            clearFlds();
            //Session.Remove("Data");
            GetIS();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            defaultSettings();
            clearFlds();
            //Session.Remove("Data");
            GetIS();
        }

        protected void grvImpShp_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = grvImpShp.SelectedRow;
            Session["idImpShpHead"] = row.Cells[0].Text.Trim();
            GetSavedItems();
        }

        protected void grvImpShp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false; //idImpShpHead
                e.Row.Cells[1].Visible = true; //    ,ImportShipmentNumber
                e.Row.Cells[2].Visible = false; //    ,Brokerage
                e.Row.Cells[3].Visible = false; //  ,CEDEC
                e.Row.Cells[4].Visible = false; //  ,CustomsStamps
                e.Row.Cells[5].Visible = false; //  ,DeliveryCharges
                e.Row.Cells[6].Visible = false; //  ,DocumentaryStamps
                e.Row.Cells[7].Visible = false; //  ,DocumentationCharges
                e.Row.Cells[8].Visible = false; //   ,ForkliftCost
                e.Row.Cells[9].Visible = false; //   ,FreightCharges
                e.Row.Cells[10].Visible = false; //   ,HandlingFee
                e.Row.Cells[11].Visible = false; //  ,ImportDuties
                e.Row.Cells[12].Visible = false; //  ,ImportProcessingFee
                e.Row.Cells[13].Visible = false; //   ,ImportationInsurance
                e.Row.Cells[14].Visible = false; //   ,Miscellaneous
                e.Row.Cells[15].Visible = false; //  ,NotarialFee
                e.Row.Cells[16].Visible = false; //   ,OtherCharges
                e.Row.Cells[17].Visible = false; //   ,ProcessingFee
                e.Row.Cells[18].Visible = false; //   ,WarehouseStorageCharges
                e.Row.Cells[19].Visible = false; //   ,Xerox
                e.Row.Cells[20].Visible = true; //   ,Total_Charges
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false; //idImpShpHead
                e.Row.Cells[1].Visible = true; //    ,ImportShipmentNumber
                e.Row.Cells[2].Visible = false; //    ,Brokerage
                e.Row.Cells[3].Visible = false; //  ,CEDEC
                e.Row.Cells[4].Visible = false; //  ,CustomsStamps
                e.Row.Cells[5].Visible = false; //  ,DeliveryCharges
                e.Row.Cells[6].Visible = false; //  ,DocumentaryStamps
                e.Row.Cells[7].Visible = false; //  ,DocumentationCharges
                e.Row.Cells[8].Visible = false; //   ,ForkliftCost
                e.Row.Cells[9].Visible = false; //   ,FreightCharges
                e.Row.Cells[10].Visible = false; //   ,HandlingFee
                e.Row.Cells[11].Visible = false; //  ,ImportDuties
                e.Row.Cells[12].Visible = false; //  ,ImportProcessingFee
                e.Row.Cells[13].Visible = false; //   ,ImportationInsurance
                e.Row.Cells[14].Visible = false; //   ,Miscellaneous
                e.Row.Cells[15].Visible = false; //  ,NotarialFee
                e.Row.Cells[16].Visible = false; //   ,OtherCharges
                e.Row.Cells[17].Visible = false; //   ,ProcessingFee
                e.Row.Cells[18].Visible = false; //   ,WarehouseStorageCharges
                e.Row.Cells[19].Visible = false; //   ,Xerox
                e.Row.Cells[20].Visible = true; //   ,Total_Charges    
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grvImpShp, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.ToolTip = "Click to select this row.";
            }
        }

        protected void grvImpShp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvImpShp.PageIndex = e.NewPageIndex;
            GetIS();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {

        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            POPanel.Visible = false;
            pnlAllDetails.Visible = true;
            tblSearch.Visible = false;
            btnBack.Visible = true;
            btnSave.Visible = true;
            btnAddNew.Visible = false;
        }

        protected void gvPO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false; //id   
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false; //id               
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvPO, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.ToolTip = "Click to select this row.";
            }
        }

        protected void gvPO_SelectedIndexChanged(object sender, EventArgs e)
        {
            var rowindex = gvPO.SelectedIndex;

            DataTable dt = (DataTable)Session["Data"];

            dt.Rows.RemoveAt(rowindex);

            decimal pocharge = Convert.ToDecimal(txtTotalCharges.Text) / dt.Rows.Count;

            for (int x = 0; x <= dt.Rows.Count - 1; x++)
            {
                dt.Rows[x][2] = pocharge.ToString("N4");
            }

            gvPO.DataSource = dt;
            gvPO.DataBind();

            Session.Remove("Data");
            Session["Data"] = dt;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            myModal.Visible = true;
            BindPOList();
            Session["Mode"] = "Add";
        }

        protected void btnMyModalClose_ServerClick(object sender, EventArgs e)
        {
            myModal.Visible = false;
        }

        protected void btnCancel1_ServerClick(object sender, EventArgs e)
        {
            myModal.Visible = false;
        }

        protected void btnSearchItem_Click(object sender, EventArgs e)
        {
            BindPOList();
        }

        protected void grvPOList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvPOList.PageIndex = e.NewPageIndex;
            BindPOList();
        }

        protected void grvPOList_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = grvPOList.SelectedRow;
            if (Session["Data"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("idPOHeader"));
                dt.Columns.Add(new DataColumn("PO_Number"));
                dt.Columns.Add(new DataColumn("POCharge"));

                DataRow dRow = dt.NewRow();
                dRow["idPOHeader"] = row.Cells[0].Text;
                dRow["PO_Number"] = row.Cells[1].Text;
                dRow["POCharge"] = txtTotalCharges.Text;
                dt.Rows.Add(dRow);

                if (Session["Mode"].ToString() == "Add")
                    Session["Mode"] = "Add";

                Session["Data"] = dt;
            }
            else
            {
                DataTable dt = (DataTable)Session["Data"];

                if (Session["Mode"].ToString() == "Add")
                {
                    DataRow dRow = dt.NewRow();
                    dRow["idPOHeader"] = row.Cells[0].Text;
                    dRow["PO_Number"] = row.Cells[1].Text;
                    dRow["POCharge"] = txtTotalCharges.Text;
                    dt.Rows.Add(dRow);

                    if (Session["Mode"].ToString() == "Add")
                        Session["Mode"] = "Add";

                    Session["Data"] = dt;
                }
            }

            DataTable dt2 = (DataTable)Session["Data"];
            decimal pocharge = Convert.ToDecimal(txtTotalCharges.Text) / dt2.Rows.Count;

            for (int x = 0; x <= dt2.Rows.Count - 1; x++)
            {
                dt2.Rows[x][2] = pocharge.ToString("N4");
            }

            gvPO.DataSource = dt2;
            gvPO.DataBind();

            Session.Remove("Data");
            Session["Data"] = dt2;
        }

        protected void grvPOList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false; //id   
                e.Row.Cells[3].Visible = false; //id   
                e.Row.Cells[4].Visible = false; //id   
                e.Row.Cells[5].Visible = false; //id   
                e.Row.Cells[6].Visible = false; //id   
                e.Row.Cells[7].Visible = false; //id   
                e.Row.Cells[8].Visible = false; //id   
                e.Row.Cells[9].Visible = false; //id   
                e.Row.Cells[10].Visible = false; //id   
                e.Row.Cells[11].Visible = false; //id   
                e.Row.Cells[12].Visible = false; //id   
                e.Row.Cells[13].Visible = false; //id   
                e.Row.Cells[14].Visible = false; //id   
                e.Row.Cells[15].Visible = false; //id   
                e.Row.Cells[16].Visible = false; //id   
                //e.Row.Cells[17].Visible = false; //id   
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false; //id   
                e.Row.Cells[3].Visible = false; //id   
                e.Row.Cells[4].Visible = false; //id   
                e.Row.Cells[5].Visible = false; //id   
                e.Row.Cells[6].Visible = false; //id   
                e.Row.Cells[7].Visible = false; //id   
                e.Row.Cells[8].Visible = false; //id   
                e.Row.Cells[9].Visible = false; //id   
                e.Row.Cells[10].Visible = false; //id   
                e.Row.Cells[11].Visible = false; //id   
                e.Row.Cells[12].Visible = false; //id   
                e.Row.Cells[13].Visible = false; //id   
                e.Row.Cells[14].Visible = false; //id   
                e.Row.Cells[15].Visible = false; //id   
                e.Row.Cells[16].Visible = false; //id   
                //e.Row.Cells[17].Visible = false; //id              
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grvPOList, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.ToolTip = "Click to select this row.";
            }
        }

        private void BindPOList()
        {
            var dt = PO_Header.RetrieveData(oCon, txtSearchItem.Text);
            grvPOList.DataSource = dt;
            grvPOList.DataBind();
        }

        protected void btnCompute_Click(object sender, EventArgs e)
        {
            txtTotalCharges.Text = (Convert.ToDecimal(txtBrokerage.Text) + Convert.ToDecimal(txtCEDEC.Text) + Convert.ToDecimal(txtCustomsStamps.Text) + Convert.ToDecimal(txtDeliveryCharges.Text) + Convert.ToDecimal(txtDocumentaryStamps.Text) + Convert.ToDecimal(txtDocumentationCharges.Text) + Convert.ToDecimal(txtForkliftCost.Text) + Convert.ToDecimal(txtFreightCharges.Text) + Convert.ToDecimal(txtHandlingFee.Text) + Convert.ToDecimal(txtImportDuties.Text) + Convert.ToDecimal(txtImportProcessingFee.Text) + Convert.ToDecimal(txtImportationInsurance.Text) + Convert.ToDecimal(txtMiscellaneous.Text) + Convert.ToDecimal(txtNotarialFee.Text) + Convert.ToDecimal(txtOtherCharges.Text) + Convert.ToDecimal(txtProcessingFee.Text) + Convert.ToDecimal(txtWarehouseStorageCharges.Text) + Convert.ToDecimal(txtXerox.Text)).ToString("N4");
            btnAdd.Visible = true;
        }
    }
}