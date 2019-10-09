using POSOINV.Functions;
using POSOINV.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOPOINV.Forms.InventoryManagement.InventoryTransactions
{
    public partial class MIS : System.Web.UI.Page
    {
        SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
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
                        GetView();
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

        protected void btnCloseMenu_Click(object sender, EventArgs e)
        {
            dvMenu.Visible = false;
        }

        protected void btnMenu_Click(object sender, EventArgs e)
        {
            dvMenu.Visible = true;
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            dvView.Visible = true;
            dvCreate.Visible = false;
            GetView();

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            dvView.Visible = false;
            dvCreate.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = true;
            btnAddItemModal.Visible = true;

            dvItemDetails.Visible = false;
            gvSerialList.Visible = false;
            fuUploadSerial.Visible = true;
            btnRemoveItemDetail.Visible = true;
            btnAddToTempGv.Visible = true;

            clearAll();

            Session["WorkMode"] = "Create";
        }

        #region View

        protected void btnSearchRequest_Click(object sender, EventArgs e)
        {
            GetView();
        }

        private void GetView()
        {
            //var dt = new DataTable();
            //dt.Columns.Add("Trans Code");
            //dt.Columns.Add("Item Number");
            //dt.Columns.Add("Site");
            //dt.Columns.Add("Doc Number");
            //dt.Columns.Add("Serial Number");
            //dt.Columns.Add("Transaction Date");

            //var mdl = Trans_History.RetrieveData(oCon, "MIS");

            var misheader = MIS_Header.RetrieveData(oCon, txtSearchRequest.Text);


            //for (int x = 0; x <= mdl.Count - 1; x++)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr[0] = mdl[x].Trans_Code;
            //    dr[1] = mdl[x].Item_Number;
            //    dr[2] = mdl[x].Site;
            //    dr[3] = mdl[x].Doc_No;
            //    dr[4] = mdl[x].Serial_No;
            //    dr[5] = mdl[x].Trans_Date;
            //    dt.Rows.Add(dr);
            //}

            gvView.DataSource = misheader;
            gvView.DataBind();
        }

        protected void gvView_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["WorkMode"] = "View";

            dvView.Visible = false;
            dvCreate.Visible = true;

            GridViewRow row = gvView.SelectedRow;
            int idMISHeader = Convert.ToInt32(row.Cells[0].Text.Trim());
            txtRequestNo.Text = row.Cells[1].Text.Trim();
            txtRequestor.Text = row.Cells[2].Text.Trim();
            txtRefNo.Text = row.Cells[3].Text.Trim();
            txtPOCMNum.Text = row.Cells[4].Text.Trim();
            txtDate.Text = Convert.ToDateTime(row.Cells[5].Text.Trim()).ToShortDateString();
            txtPreparedBy.Text = row.Cells[6].Text.Trim();
            txtRemarks.Text = row.Cells[7].Text.Trim();

            var source = new DataTable();
            source.Columns.Add("idItem");
            source.Columns.Add("Item Number");
            source.Columns.Add("Qty");
            source.Columns.Add("Cost");

            var detail = MIS_Detail.RetrieveData(oCon, idMISHeader, 0);

            for (int x = 0; x <= detail.Count - 1; x++)
            {
                int iditem = detail[x].idItem;
                var master = Item_Master.RetrieveData(oCon, iditem);
                string itemnumber = master[0].ItemNumber;

                DataRow dr = source.NewRow();
                dr[0] = iditem;
                dr[1] = itemnumber;
                dr[2] = detail[x].Quantity;
                dr[3] = detail[x].Cost;
                source.Rows.Add(dr);

                var serial = MIS_Serial.RetrieveData(oCon, detail[x].idMISDetail, 0);

                var serialList = new DataTable();
                serialList.Columns.Add("Serial Number");

                for (int y = 0; y <= serial.Count - 1;y++)
                {
                    var dtSerial = Item_Serial.RetrieveData(oCon, serial[y].idSerial);
                    DataRow dr2 = serialList.NewRow();
                    dr2[0] = dtSerial[0].Serial_No;
                    serialList.Rows.Add(dr2);
                }

                string dtName = "Ser" + iditem + "-" + itemnumber;
                Session[dtName] = serialList;
            }

            gvTempGv.DataSource = source;
            gvTempGv.DataBind();

            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnAddItemModal.Visible = false;
        }

        protected void gvView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[7].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[7].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvView, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.ToolTip = "Click to select this row.";
            }
        }

        protected void gvView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvView.PageIndex = e.NewPageIndex;
            GetView();
        }
        #endregion

        #region Create  

        #region ItemMaster
        private void GetItemMaster()
        {
            var dt = new DataTable();
            dt.Columns.Add("idItem");
            dt.Columns.Add("Item Number");
            dt.Columns.Add("Site");

            var itemmstr = Item_Master.RetrieveData(oCon, txtSearchItem.Text);

            for (int x = 0; x <= itemmstr.Count - 1; x++)
            {
                var site = Site_Loc.RetrieveData(oCon, Convert.ToInt32(itemmstr[x].Site));

                DataRow dr = dt.NewRow();
                dr[0] = itemmstr[x].idItem;
                dr[1] = itemmstr[x].ItemNumber;
                dr[2] = site[0].Site_Name;
                dt.Rows.Add(dr);
            }

            grvItemMaster.DataSource = dt;
            grvItemMaster.DataBind();
        }

        protected void btnSearchItem_Click(object sender, EventArgs e)
        {
            GetItemMaster();
        }

        protected void btnAddItemModal_Click(object sender, EventArgs e)
        {
            dvAddNewProduct.Visible = true;
            GetItemMaster();
            btnRemoveItemDetail.Visible = false;
        }

        protected void btnCloseDvAddNewProduct_ServerClick(object sender, EventArgs e)
        {
            dvAddNewProduct.Visible = false;
            clearItemDetails();
        }

        protected void btnCancelAddNewProduct_Click(object sender, EventArgs e)
        {
            dvAddNewProduct.Visible = false;
            clearItemDetails();
        }

        protected void grvItemMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = grvItemMaster.SelectedRow;
            Session["idItem"] = row.Cells[0].Text.Trim();
            txtItemNumber.Text = row.Cells[1].Text.Trim();
            dvItemDetails.Visible = true;
        }

        protected void grvItemMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvItemMaster.PageIndex = e.NewPageIndex;
            GetItemMaster();
        }

        protected void grvItemMaster_RowDataBound(object sender, GridViewRowEventArgs e)
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
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grvItemMaster, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.ToolTip = "Click to select this row.";
            }
        }

        protected void btnCloseItemDetails_ServerClick(object sender, EventArgs e)
        {
            dvItemDetails.Visible = false;
            clearItemDetails();
        }

        protected void btnAddToTempGv_Click(object sender, EventArgs e)
        {
            try
            {
                string extension = System.IO.Path.GetExtension(fuUploadSerial.FileName);
                if (extension == ".csv")
                {
                    int qty = Convert.ToInt32(txtQuantity.Text);

                    StreamReader SR = new StreamReader(fuUploadSerial.PostedFile.InputStream);
                    string line = SR.ReadLine();
                    string[] strArray = line.Split(',');
                    DataTable dtUpload = new DataTable();
                    DataRow rowUpload;
                    DataTable dtCheck = new DataTable();
                    DataRow rowCheck;
                    dtUpload.Columns.Add(new DataColumn());
                    dtCheck.Columns.Add(new DataColumn());

                    DataTable dtForSave = new DataTable();
                    dtForSave.Columns.Add("idSerial");
                    dtForSave.Columns.Add("Serial_No");

                    try
                    {
                        do
                        {
                            line = SR.ReadLine().Trim();
                            if (line != string.Empty)
                            {
                                rowUpload = dtUpload.NewRow();
                                rowCheck = dtCheck.NewRow();
                                line = line.ToString().Replace(",", "_");
                                line = line.ToString().Replace("'", "");
                                line = line.Trim();
                                rowUpload.ItemArray = line.Split(',');
                                dtUpload.Rows.Add(rowUpload);
                                rowCheck.ItemArray = line.Split(',');
                                dtCheck.Rows.Add(rowCheck);
                            }
                            else
                                break;
                        }
                        while (true);
                    }
                    catch
                    {

                    }

                    if (qty == dtUpload.Rows.Count)
                    {
                        int duplicateCount = 0;
                        int notInDatabaseCount = 0;
                        int notInStock = 0;

                        for (var x = 0; x <= dtCheck.Rows.Count - 1; x++)
                        {
                            int counter = 0;
                            for (var y = 0; y <= dtUpload.Rows.Count - 1; y++)
                            {
                                if (dtCheck.Rows[x][0].ToString() == dtUpload.Rows[y][0].ToString())
                                {
                                    counter = counter + 1;
                                    if (counter > 1)
                                    {
                                        duplicateCount = duplicateCount + 1;
                                    }
                                }
                            }

                            if (Item_Serial.CheckDuplicate(oCon, dtCheck.Rows[x][0].ToString(), Convert.ToInt32(Session["idItem"].ToString())) == 0)
                            {
                                notInDatabaseCount = notInDatabaseCount + 1;
                            }

                            if (Item_Serial.CheckIfInStock(oCon, dtCheck.Rows[x][0].ToString()) == 0)
                            {
                                notInStock = notInStock + 1;
                            }

                            if (duplicateCount == 0)
                            {
                                if (notInStock == 0)
                                {
                                    if (notInDatabaseCount == 0)
                                    {
                                        int idSerial = Item_Serial.RetrieveIdSerial(oCon, dtUpload.Rows[x][0].ToString());

                                        DataRow dr = dtForSave.NewRow();
                                        dr[0] = idSerial;
                                        dr[1] = dtCheck.Rows[x][0].ToString();
                                        dtForSave.Rows.Add(dr);
                                    }
                                }
                            }
                        }

                        if (notInStock == 0)
                        {
                            if (duplicateCount == 0)
                            {
                                if (notInDatabaseCount == 0)
                                {
                                    var dt = (DataTable)Session["TempData"];

                                    DataRow dr2 = dt.NewRow();
                                    int iditem = Convert.ToInt32(Session["idItem"].ToString());
                                    dr2[0] = iditem;
                                    dr2[1] = txtItemNumber.Text;
                                    dr2[2] = txtQuantity.Text;
                                    dr2[3] = txtCost.Text;
                                    dt.Rows.Add(dr2);

                                    Session.Remove("TempData");
                                    Session["TempData"] = dt;
                                    gvTempGv.DataSource = dt;
                                    gvTempGv.DataBind();

                                    string dtName = "Ser" + Session["idItem"].ToString() + "-" + txtItemNumber.Text;
                                    Session[dtName] = dtForSave;
                                    clearItemDetails();

                                    dvItemDetails.Visible = false;
                                    dvAddNewProduct.Visible = false;

                                }
                                else
                                {
                                    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "There are " + notInDatabaseCount.ToString() + " serial numbers that is not in the database." + ControlChars.Quote + ");</script>");
                                }
                            }
                            else
                            {
                                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Duplicate serial numbers detected in file : " + duplicateCount.ToString() + ControlChars.Quote + ");</script>");
                            }
                        }
                        else
                        {
                            HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Serial Numbers not in stock : " + notInStock.ToString() + ControlChars.Quote + ");</script>");
                        }
                    }
                    else
                    {
                        HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Uploaded file quantity is not equal to quantity entered! " + ControlChars.Quote + ");</script>");
                    }
                }
                else
                {
                    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Please upload a CSV file! " + ControlChars.Quote + ");</script>");
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Please select a file! " + ControlChars.Quote + ");</script>");
            }
        }

        protected void btnCancelItemDetails_Click(object sender, EventArgs e)
        {
            dvItemDetails.Visible = false;
            clearItemDetails();
        }

        private void clearItemDetails()
        {
            txtQuantity.Text = "";
            txtItemNumber.Text = "";
            txtCost.Text = "";
            Session.Remove("idItem");
        }

        #endregion

        protected void gvTempGv_RowDataBound(object sender, GridViewRowEventArgs e)
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
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvTempGv, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.ToolTip = "Click to select this row.";
            }
        }

        protected void gvTempGv_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gvTempGv.SelectedRow;
            Session["TempGvRowIndex"] = row.RowIndex;
            Session["dtName"] = "Ser" + row.Cells[0].Text + "-" + row.Cells[1].Text;
            string WorkMode = Session["WorkMode"].ToString();

            //if (WorkMode == "Create")
            //{
            //    dvItemDetails.Visible = true;
            //    gvSerialList.Visible = false;
            //    fuUploadSerial.Visible = true;
            //    btnRemoveItemDetail.Visible = true;
            //    btnAddToTempGv.Visible = true;
            //}
            //else
            if (WorkMode == "View")
            {
                txtItemNumber.Text = row.Cells[1].Text;
                txtQuantity.Text = row.Cells[2].Text;
                txtCost.Text = row.Cells[3].Text;

                var dtSerial = (DataTable)Session[Session["dtName"].ToString()];
                gvSerialList.DataSource = dtSerial;
                gvSerialList.DataBind();
                gvSerialList.Visible = true;
                dvItemDetails.Visible = true;
                fuUploadSerial.Visible = false;
                btnRemoveItemDetail.Visible = false;
                btnAddToTempGv.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            MIS_Header_Model mIS_Header_Model = new MIS_Header_Model
            {
                RequestNo = txtRequestNo.Text,
                ReferenceNo = txtRefNo.Text,
                POCMNumber = txtPOCMNum.Text,
                PreparedBy = txtPreparedBy.Text,
                Remarks = txtRemarks.Text,
                RequestDate = Convert.ToDateTime(txtDate.Text, System.Globalization.CultureInfo.InvariantCulture),
                Requestor = txtRequestor.Text
            };
            int idmisheader = MIS_Header.Save(oCon, mIS_Header_Model);

            DataTable dtTempData = (DataTable)Session["TempData"];

            for (int x = 0; x <= dtTempData.Rows.Count - 1; x++)
            {
                int idItem = Convert.ToInt32(dtTempData.Rows[x][0].ToString());
                string item_number = dtTempData.Rows[x][1].ToString();
                int qty = Convert.ToInt32(dtTempData.Rows[x][2].ToString());
                decimal cost = Convert.ToDecimal(dtTempData.Rows[x][3].ToString());

                MIS_Detail_Model mIS_Detail_Model = new MIS_Detail_Model
                {
                    idMISHeader = idmisheader,
                    Cost = cost,
                    Quantity = qty,
                    idItem = idItem
                };
                int idmisdetail = MIS_Detail.Save(oCon, mIS_Detail_Model);

                string dtname = "Ser" + idItem.ToString() + "-" + item_number;

                var dt = (DataTable)Session[dtname];

                for (int y = 0; y <= dt.Rows.Count - 1; y++)
                {
                    Item_Serial.UpdateStockStatus(oCon, dt.Rows[y][1].ToString(), "N");
                    MIS_Serial_Model mIS_Serial_Model = new MIS_Serial_Model
                    {
                        idMISDetail = idmisdetail,
                        idSerial = Convert.ToInt32(dt.Rows[y][0].ToString())
                    };
                    MIS_Serial.Save(oCon, mIS_Serial_Model);

                    Trans_History_Model trans_History_Model = new Trans_History_Model
                    {
                        Trans_Code = "MIS",
                        Item_Number = item_number,
                        Site = "WH-JMS",
                        UM = "UT",
                        Doc_No = txtRequestNo.Text,
                        Serial_No = dt.Rows[y][1].ToString(),
                        Reason_Code = "",
                        Trans_Date = DateTime.Now,
                        Order_No = "",
                        Invoice_No = "",
                        Reference_No = txtRefNo.Text,
                        Trans_Qty = -1,
                        Trans_Amt = cost,
                        Remarks = txtRemarks.Text,
                        user_domain = Session["User_Domain"].ToString()
                    };
                    Trans_History.Save(oCon, trans_History_Model);
                }
            }

            Item_Master.InventoryCheckForError(oCon);
            clearAll();
            dvView.Visible = true;
            dvCreate.Visible = false;
            GetView();

            HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "MIS Saved!" + ControlChars.Quote + ");</script>");

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            dvView.Visible = true;
            dvCreate.Visible = false;
        }

        private void clearAll()
        {
            txtCost.Text = "";
            txtDate.Text = "";
            txtItemNumber.Text = "";
            txtPOCMNum.Text = "";
            txtPreparedBy.Text = "";
            txtQuantity.Text = "";
            txtRefNo.Text = "";
            txtRemarks.Text = "";
            txtRequestNo.Text = "";
            txtRequestor.Text = "";
            txtSearchItem.Text = "";

            for (int x = Session.Count - 1; x >= 5; x--)
            {
                Session.RemoveAt(x);
            }

            var dt = new DataTable();
            dt.Columns.Add("idItem");
            dt.Columns.Add("Item Number");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Cost");

            Session["TempData"] = dt;

            gvTempGv.DataSource = dt;
            gvTempGv.DataBind();
        }

        protected void btnRemoveItemDetail_Click(object sender, EventArgs e)
        {
            int rowindex = Convert.ToInt32(Session["TempGvRowIndex"]);

            DataTable dt = (DataTable)Session["TempData"];

            dt.Rows.Remove(dt.Rows[rowindex]);

            Session.Remove(Session["dtName"].ToString());
            Session.Remove("dtName");
            Session.Remove("Data");
            Session["Data"] = dt;
            gvTempGv.DataSource = dt;
            gvTempGv.DataBind();

            dvItemDetails.Visible = false;
        }
        #endregion
    }
}