using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using POSOINV.Functions;
using POSOINV.Models;

namespace SHIELD.Forms
{
    public partial class PickActivity : System.Web.UI.Page
    {
        SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);
        //public static DataSet dataCollection;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnUploadSerial);

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
                for (int x = Session.Count - 1; x >= 6; x--)
                {
                    Session.RemoveAt(x);
                }

                if (username != "")
                {
                    if (access == "OP" || access == "IT" || access == "AU")
                    {
                        GetSOtoGrid_(txtSearchSO.Text.Trim(), "Open");

                        if (Session["dataCollection"] == null)
                        {
                            DataSet dataCollection = new DataSet();
                            Session["dataCollection"] = dataCollection;
                        }
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


        private void GetSOHeaderDetails(string SONumber)
        {
            txtSONumber.Text = SONumber;
            Session["pickStatus"] = Pick_Activity.checkPickedSO(oCon, SONumber);
            if (Session["pickStatus"].ToString() == "Y")
            {
                List<Pick_Header_Model> PickHeaderDetails = Pick_Header.RetrieveData(oCon, SONumber, "");
                Session["idSOHeader"] = PickHeaderDetails[0].idSOHeader;
                txtCustPONum.Text = PickHeaderDetails[0].CustPONum;
                txtOrderDate.Text = PickHeaderDetails[0].OrderDate.ToString();
                txtDueDate.Text = PickHeaderDetails[0].DueDate.ToString();
                //txtSalesman.Text = PickHeaderDetails[0].;
                txtCustomerCode.Text = PickHeaderDetails[0].CustomerCode;
                lblPickNumber.Text = PickHeaderDetails[0].Pick_Number + " (Processed)";

                var SODetails = Pick_Detail.RetrieveData(oCon, PickHeaderDetails[0].idPickHeader.ToString());
                gvItems.DataSource = SODetails;
                gvItems.DataBind();

                btnDownload.Visible = true;
            }
            else if (Session["pickStatus"].ToString() == "N")
            {
                lblPickNumber.Text = "Assigned after save (Open)";
                List<SO_Header_Model> SOHeaderDetails = SO_Header.RetrieveData(oCon, SONumber, true);
                Session["idSOHeader"] = SOHeaderDetails[0].idSOHeader;
                txtCustPONum.Text = SOHeaderDetails[0].Customer_PO;
                txtOrderDate.Text = SOHeaderDetails[0].Order_Date.ToString();
                txtDueDate.Text = SOHeaderDetails[0].Due_Date.ToString();
                //txtSalesman.Text = SOHeaderDetails[0].Salesman;
                List<Customer_Details_Model> customer_Details = Customer_Details.RetrieveData(oCon, SOHeaderDetails[0].idCustomer, "");
                txtCustomerCode.Text = customer_Details[0].Customer_Code;

                var SODetails = Pick_Activity.RetrieveData(oCon, SONumber);
                SODetails.Columns.Add("Items Picked");
                gvItems.DataSource = SODetails;
                gvItems.DataBind();
                btnDownload.Visible = false;
            }
            else
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "SO not found" + ControlChars.Quote + ");</script>");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetSOtoGrid_(txtSearchSO.Text.Trim(), ddlSOStatus.SelectedValue.ToString());
        }

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvItems, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click To Select this row";
            }
        }

        protected void gvItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow rowItem in gvItems.Rows)
            {
                if (rowItem.RowIndex == gvItems.SelectedIndex)
                {
                    rowItem.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                }
                else
                {
                    rowItem.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                }
            }
            Session["gvItemIndex"] = gvItems.SelectedIndex.ToString();
            Session["idItem"] = gvItems.Rows[gvItems.SelectedIndex].Cells[1].Text.ToString();
            Session["ItemName"] = gvItems.Rows[gvItems.SelectedIndex].Cells[2].Text.ToString();
            Session["Quantity"] = gvItems.Rows[gvItems.SelectedIndex].Cells[4].Text.ToString();
            Session["Picked"] = gvItems.Rows[gvItems.SelectedIndex].Cells[5].Text.ToString();

            if (Session["Picked"].ToString() == "&nbsp;")
                Session["Picked"] = "0";

            DataTable dt = new DataTable();
            bindSKUList();
            if (Session["pickStatus"].ToString() == "Y")
            {
                dt = Pick_Serial.RetrieveData(oCon, Convert.ToInt32(gvItems.Rows[gvItems.SelectedIndex].Cells[0].Text.ToString()));
            }
            else if (Session["pickStatus"].ToString() == "N")
            {
                if (Convert.ToInt32(Session["Quantity"].ToString()) != Convert.ToInt32(Session["Picked"].ToString()))
                {
                    dvPickItems.Visible = true;

                    DataSet dataCollection = (DataSet)Session["dataCollection"];
                    try
                    {
                        if (dataCollection.Tables.Contains(Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()))
                        {
                            dt = dataCollection.Tables[Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()];
                            ViewState["vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()] = dt;
                        }
                        else
                        {
                            dt = new DataTable();
                            dt.Columns.Add("idSerial");
                            dt.Columns.Add("Serial_No");
                            dt.Columns.Add("PO_Number");
                            dt.TableName = Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString();
                            dt.AcceptChanges();
                            ViewState["vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()] = dt;
                        }
                    }
                    catch
                    {
                        dt = new DataTable();
                        dt.Columns.Add("idSerial");
                        dt.Columns.Add("Serial_No");
                        dt.Columns.Add("PO_Number");
                        dt.TableName = Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString();
                        dt.AcceptChanges();
                        ViewState["vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()] = dt;
                    }
                }
                else
                {
                    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Finished Picking" + ControlChars.Quote + ");</script>");
                }
            }
            gvSelectedSKU.DataSource = dt;
            gvSelectedSKU.DataBind();

        }

        //protected void gvItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    gvItems.PageIndex = e.NewPageIndex;
        //    GetSOHeaderDetails(Session["SONumber"].ToString());
        //}


        //DataTable dt = new DataTable();
        //for (int i = 0; i < gvItems.HeaderRow.Cells.Count; i++)
        //{
        //    dt.Columns.Add(gvItems.HeaderRow.Cells[i].Text);
        //}

        //foreach (GridViewRow row in gvItems.Rows)
        //{
        //    DataRow dr;
        //    dr = dt.NewRow();

        //    for (int i = 0; i < row.Cells.Count; i++)
        //    {
        //        dr[i] = row.Cells[i].Text.Replace(" ", "");
        //    }
        //    dt.Rows.Add(dr);
        //}

        //DataRow dr2 = dt.Rows[Convert.ToInt32(Session["gvItemIndex"])];
        //dr2[4] = gvSelectedSKU.Rows.Count;

        //gvItems.DataSource = dt;
        //gvItems.DataBind();

        //if (ViewState["vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()] == null)
        //    ViewState.Add("vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString(), new DataTable());


        //DataSet dataCollection = (DataSet)Session["dataCollection"];
        //DataTable selectedPick = (DataTable)ViewState["vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()];
        //selectedPick.TableName = Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString();

        //if (!dataCollection.Tables.Contains(Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()))
        //    dataCollection.Tables.Add(selectedPick);

        //dvPickItems.Visible = false;
        //Session["dataCollection"] = dataCollection;

        protected void btnSavePick_Click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            for (int i = 0; i < gvItems.HeaderRow.Cells.Count; i++)
            {
                dt.Columns.Add(gvItems.HeaderRow.Cells[i].Text);
            }

            foreach (GridViewRow row in gvItems.Rows)
            {
                DataRow dr;
                dr = dt.NewRow();

                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (i == 5)
                    {
                        if (row.RowIndex == Convert.ToInt32(Session["gvItemIndex"]))
                            dr[i] = gvSelectedSKU.Rows.Count;
                        else if (row.Cells[i].Text == "&nbsp;")
                            dr[i] = "0";
                        else
                            dr[i] = row.Cells[i].Text.Trim();
                    }
                    else
                        dr[i] = row.Cells[i].Text.Trim();
                }
                dt.Rows.Add(dr);
            }

            //DataRow dr2 = dt.Rows[Convert.ToInt32(Session["gvItemIndex"])];
            //dr2[5] = gvSelectedSKU.Rows.Count;

            gvItems.DataSource = dt;
            gvItems.DataBind();

            if (ViewState["vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()] == null)
                ViewState.Add("vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString(), new DataTable());

            DataSet dataCollection = (DataSet)Session["dataCollection"];
            DataTable selectedPick = (DataTable)ViewState["vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()];
            selectedPick.TableName = Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString();

            if (!dataCollection.Tables.Contains(Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()))
                dataCollection.Tables.Add(selectedPick);

            dvPickItems.Visible = false;
            Session["dataCollection"] = dataCollection;

            gvSelectedSKU.DataSource = null;
            gvSelectedSKU.DataBind();
            txtSearchPO.Text = "";

        }

        protected void btnCancelPick_Click(object sender, EventArgs e)
        {
            dvPickItems.Visible = false;
            DataSet dataCollection = (DataSet)Session["dataCollection"];

            if (dataCollection.Tables.Contains(Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()))
            {
                dataCollection.Tables.Remove(Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString());
                ViewState.Remove("vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString());
                gvSelectedSKU.DataSource = null;
                gvSelectedSKU.DataBind();
            }
            txtSearchPO.Text = "";
        }

        public void bindSKUList()
        {
            var x = Item_Serial.RetrieveData(oCon, Session["idItem"].ToString(), txtSearchPO.Text.Trim(), txtSearchSerial.Text.Trim());
            gvSKUList.DataSource = x;
            gvSKUList.DataBind();
        }

        protected void gvSKUList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;

                e.Row.Cells[5].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;//idSerial
                e.Row.Cells[2].Text = "Serial Number";
                e.Row.Cells[3].Text = "PO Number";
                e.Row.Cells[4].Text = "Date Added";
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Text = "Computation";
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvSKUList, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click To Select this row";
            }
        }

        protected void gvSKUList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //SetData("vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString());
            //GetData("vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString());
            gvSKUList.PageIndex = e.NewPageIndex;
            bindSKUList();
        }

        protected void btnSaveAllPick_Click(object sender, EventArgs e)
        {
            btnSaveAllPick.Visible = false;

            DataSet dataCollection = (DataSet)Session["dataCollection"];
            DataSet checkDataCollection = (DataSet)Session["dataCollection"];
            int duplicateCount = 0;
            for (int cdsc = 0; cdsc <= checkDataCollection.Tables.Count - 1; cdsc++)
            {
                for (int dsc = 0; dsc <= dataCollection.Tables.Count - 1; dsc++)
                {
                    for (int cdst = 0; cdst <= checkDataCollection.Tables[cdsc].Rows.Count - 1; cdst++)
                    {
                        int counter = 0;
                        for (int dst = 0; dst <= dataCollection.Tables[dsc].Rows.Count - 1; dst++)
                        {
                            if (dataCollection.Tables[dsc].Rows[dst][1].ToString() == checkDataCollection.Tables[cdsc].Rows[cdst][1].ToString())
                            {
                                counter = counter + 1;
                                if (counter > 1)
                                {
                                    duplicateCount = duplicateCount + 1;
                                }
                            }
                        }
                    }
                }
            }

            if (duplicateCount == 0)
            {
                lblPickNumber.Text = Pick_Activity.SetPickNumber(oCon);

                Pick_Header_Model saveHeader = new Pick_Header_Model
                {
                    idSOHeader = Convert.ToInt32(Session["idSOHeader"].ToString()),
                    CustomerCode = txtCustomerCode.Text.Trim(),
                    CustPONum = txtCustPONum.Text.Trim(),
                    DueDate = Convert.ToDateTime(txtDueDate.Text),
                    OrderDate = Convert.ToDateTime(txtOrderDate.Text),
                    Pick_Number = lblPickNumber.Text.Trim(),
                    Pick_Date = DateTime.Now,
                    user_id_chg_by = Session["User_Domain"].ToString()
                };

                int idPickHeader = Pick_Header.Save(oCon, saveHeader);

                foreach (GridViewRow row in gvItems.Rows)
                {
                    Pick_Detail_Model saveDetail = new Pick_Detail_Model
                    {
                        idPickHeader = idPickHeader,
                        idItem = Convert.ToInt32(row.Cells[1].Text),
                        Item_Number = row.Cells[2].Text.Trim(),
                        Description = row.Cells[3].Text.Trim(),
                        Qty = Convert.ToInt32(row.Cells[4].Text),
                        Items_Picked = Convert.ToInt32(row.Cells[5].Text)
                    };

                    int idPickDetail = Pick_Detail.Save(oCon, saveDetail);

                    DataTable selectedTable = dataCollection.Tables[row.Cells[2].Text + "-" + row.RowIndex.ToString()];

                    for (int x = 0; x <= selectedTable.Rows.Count - 1; x++)
                    {
                        Item_Serial.Delete(oCon, Convert.ToInt32(selectedTable.Rows[x][0].ToString()), Convert.ToInt32(row.Cells[1].Text));

                        Pick_Serial_Model savePickSerial = new Pick_Serial_Model
                        {
                            idPickDetail = idPickDetail,
                            idSerial = Convert.ToInt32(selectedTable.Rows[x][0].ToString()),//0
                            Serial_No = selectedTable.Rows[x][1].ToString(),//1
                            PO_Number = selectedTable.Rows[x][2].ToString()//2
                        };
                        Pick_Serial.Save(oCon, savePickSerial);

                        var model = SO_Detail.RetrieveData(oCon, Convert.ToInt32(Session["idSOHeader"].ToString()), Convert.ToInt32(row.Cells[1].Text), Convert.ToInt32(row.Cells[4].Text));
                        decimal transAmt = Convert.ToDecimal(model[0].Cost.ToString());

                        Trans_History_Model trans_History_Model = new Trans_History_Model
                        {
                            Trans_Code = "SLE",
                            Item_Number = row.Cells[2].Text.Trim(),
                            Site = "WH-JMS",
                            UM = "UT",
                            Doc_No = lblPickNumber.Text.Trim(),
                            Serial_No = selectedTable.Rows[x][1].ToString(),
                            Reason_Code = "",
                            Trans_Date = DateTime.Now,
                            Order_No = txtSONumber.Text,
                            Invoice_No = "",
                            Reference_No = "",
                            Trans_Qty = -1,
                            Trans_Amt = transAmt,
                            Remarks = "",
                            user_domain = Session["User_Domain"].ToString()
                        };
                        Trans_History.Save(oCon, trans_History_Model);
                    }
                    // CLEAR CREATED SESSIONS
                    var itemNum = selectedTable.TableName.ToString();
                    Session.Remove("vs" + itemNum);
                    Session.Remove(itemNum);
                    ViewState.Remove("vs" + itemNum);

                    Item_Master.RecomputeItemCost(oCon, Convert.ToInt32(row.Cells[1].Text));
                }

                SO_Header.UpdateStatus(oCon, "Y", "Closed", "Y", Convert.ToInt32(Session["idSOHeader"].ToString()));

                //var objSOHeader = new SO_Header_Model();
                //objSOHeader.idSOHeader = int.Parse(Session["idSOHeader"].ToString());
                //objSOHeader.SO_Status = "Closed";
                //SO_Header.UpdateSOStatus(oCon, objSOHeader);

                //Session["SONumber"] = txtSearchSO.Text;
                Session["Pick_Comments"] = txtComments.Text;
                Session["Pick_Number"] = lblPickNumber.Text;

                clearAll();

                lblConfirmMsg.Text = "Download Pick Activity Register?";
                myModalConfirmation.Visible = true;
                //ClientScript.RegisterStartupScript(this.GetType(), "alert", @"window.location.href='DownloadPickActivity.aspx", true);

                //HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Picking saved! Pick Number : "
                //    + lblPickNumber.Text + " for SO number " + txtSearchSO.Text.Trim() + ControlChars.Quote + ");</script>");           



                //SavePickActivityRegisterFile();

                //btnSaveAllPick.Visible = true;           
            }
        }

        private void clearAll()
        {
            tblSearch.Visible = true;
            btnBack.Visible = false;
            txtComments.Text = "";
            txtCustomerCode.Text = "";
            txtCustPONum.Text = "";
            txtDueDate.Text = "";
            txtLotSerial.Text = "";
            txtOrderDate.Text = "";
            //txtSalesman.Text = "";
            txtSearchPO.Text = "";
            txtSearchSerial.Text = "";
            txtSearchSO.Text = "";
            try
            {
                DataSet dataCollection = (DataSet)Session["dataCollection"];

                foreach (GridViewRow row in gvItems.Rows)
                {
                    DataTable selectedTable = dataCollection.Tables[row.Cells[2].Text + "-" + row.RowIndex.ToString()];
                    if (selectedTable != null)
                    {
                        for (int x = 0; x <= selectedTable.Rows.Count - 1; x++)
                        {
                            // CLEAR CREATED SESSIONS
                            var itemNum = selectedTable.TableName.ToString();
                            Session.Remove("vs" + itemNum);
                            Session.Remove(itemNum);
                            ViewState.Remove("vs" + itemNum);
                        }
                    }
                }

                dataCollection = new DataSet();
                Session["dataCollection"] = dataCollection;
            }
            catch
            {
            }

            DataSet dataCollect = new DataSet();
            Session["dataCollection"] = dataCollect;

            ViewState.Clear();

            gvItems.DataSource = null;
            gvItems.DataBind();
            SOPanel.Visible = true;
            pnlAllDetails.Visible = false;
        }

        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            myModalConfirmation.Visible = false;
            Response.Write("<script>window.open ('/Forms/DownloadReports/DownloadPickActivity.aspx','_blank');</script>");
        }

        protected void btnCancelDelete_Click(object sender, EventArgs e)
        {
            myModalConfirmation.Visible = false;
        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkAll = (CheckBox)sender;
            DataSet dataCollection = (DataSet)Session["dataCollection"];
            DataTable selectedPick = (DataTable)ViewState["vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()];
            // selectedPick.TableName = Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString();
            string tableName = selectedPick.TableName;

            foreach (GridViewRow row in gvSKUList.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("chkOne");
                if (cb != null)
                {
                    if (chkAll.Checked)
                    {
                        int duplicateCount = 0;

                        for (int dsc = 0; dsc <= dataCollection.Tables.Count - 1; dsc++)
                        {
                            for (int dst = 0; dst <= dataCollection.Tables[dsc].Rows.Count - 1; dst++)
                            {
                                if (dataCollection.Tables[dsc].Rows[dst][1].ToString() == row.Cells[2].Text.ToString())
                                {
                                    duplicateCount = duplicateCount + 1;
                                }
                            }
                        }

                        if (duplicateCount == 0)
                        {
                            cb.Checked = true;
                            DataRow toInsert = selectedPick.NewRow();
                            toInsert[0] = row.Cells[1].Text.ToString();
                            toInsert[1] = row.Cells[2].Text.ToString();
                            toInsert[2] = row.Cells[3].Text.ToString();
                            selectedPick.Rows.Add(toInsert);
                        }
                    }
                    else
                    {
                        cb.Checked = false;
                        DataRow[] dr = selectedPick.Select("idSerial = '" + row.Cells[1].Text + "'");
                        if (dr.Length > 0)
                        {
                            selectedPick.Rows.Remove(dr[0]);
                        }
                    }
                }
            }

            var x = Session["Quantity"].ToString();
            if (selectedPick.Rows.Count <= Convert.ToInt32(x))
            {
                if (!dataCollection.Tables.Contains(Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()))
                {
                    dataCollection.Tables.Add(selectedPick);
                }
                else
                {
                    dataCollection.Tables.Remove(tableName);
                    dataCollection.Tables.Add(selectedPick);
                }

                ViewState["vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()] = selectedPick;
                Session["dataCollection"] = dataCollection;

                gvSelectedSKU.DataSource = selectedPick;
                gvSelectedSKU.DataBind();
            }
            else
            {
                chkAll.Checked = false;
                foreach (GridViewRow row in gvSKUList.Rows)
                {
                    CheckBox cb = (CheckBox)row.FindControl("chkOne");
                    if (cb != null)
                    {
                        cb.Checked = false;
                    }

                    DataRow[] dr = selectedPick.Select("idSerial = '" + row.Cells[1].Text + "'");
                    if (dr.Length > 0)
                    {
                        selectedPick.Rows.Remove(dr[0]);
                    }
                }
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Quantity doesn't match" + ControlChars.Quote + ");</script>");
            }
        }

        protected void chkOne_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int x = gvRow.RowIndex;

            DataSet dataCollection = (DataSet)Session["dataCollection"];
            DataTable selectedPick = (DataTable)ViewState["vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()];
            selectedPick.TableName = Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString();
            string tableName = selectedPick.TableName;

            if (selectedPick.Rows.Count + 1 <= Convert.ToInt32(Session["Quantity"].ToString()))
            {
                if (cb.Checked == true)
                {
                    int duplicateCount = 0;

                    for (int dsc = 0; dsc <= dataCollection.Tables.Count - 1; dsc++)
                    {
                        for (int dst = 0; dst <= dataCollection.Tables[dsc].Rows.Count - 1; dst++)
                        {
                            if (dataCollection.Tables[dsc].Rows[dst][1].ToString() == gvRow.Cells[2].Text.ToString())
                            {
                                duplicateCount = duplicateCount + 1;
                            }
                        }
                    }

                    if (duplicateCount == 0)
                    {
                        DataRow toInsert = selectedPick.NewRow();
                        toInsert[0] = gvRow.Cells[1].Text.ToString();
                        toInsert[1] = gvRow.Cells[2].Text.ToString();
                        toInsert[2] = gvRow.Cells[3].Text.ToString();
                        selectedPick.Rows.Add(toInsert);
                    }
                }
                else if (cb.Checked == false)
                {
                    DataRow[] dr = selectedPick.Select("idSerial = '" + gvRow.Cells[1].Text + "'");
                    if (dr.Length > 0)
                    {
                        selectedPick.Rows.Remove(dr[0]);
                        selectedPick.AcceptChanges();
                    }
                }

                if (!dataCollection.Tables.Contains(Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()))
                {
                    dataCollection.Tables.Add(selectedPick);
                }
                else
                {
                    dataCollection.Tables.Remove(tableName);
                    dataCollection.Tables.Add(selectedPick);
                }
                ViewState["vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()] = selectedPick;
                Session["dataCollection"] = dataCollection;

                gvSelectedSKU.DataSource = selectedPick;
                gvSelectedSKU.DataBind();
            }
            else
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Maximum number of quantity reached!" + ControlChars.Quote + ");</script>");
                cb.Checked = false;
            }

            //DataRow toInsert = selectedPick.NewRow();
            //toInsert[0] = gvSKUList.Rows[x].Cells[1].Text.ToString();
            //toInsert[1] = gvSKUList.Rows[x].Cells[2].Text.ToString();
            //toInsert[2] = gvSKUList.Rows[x].Cells[3].Text.ToString();
            //toInsert[3] = gvSKUList.Rows[x].Cells[4].Text.ToString();
            //toInsert[4] = gvSKUList.Rows[x].Cells[5].Text.ToString();
            //selectedPick.Rows.Add(toInsert);
        }

        protected void gvSelectedSKU_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;
                //e.Row.Cells[1].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
                //e.Row.Cells[1].Visible = false;
            }
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvItems, "Select$" + e.Row.RowIndex);
            //    e.Row.ToolTip = "Click To Select this row";
            //}
        }

        protected void btnSearchPO_Click(object sender, EventArgs e)
        {
            bindSKUList();
        }

        protected void btnUploadSerial_Click(object sender, EventArgs e)
        {
            StreamReader SR = new StreamReader(uploadExcel.PostedFile.InputStream);
            string line = SR.ReadLine();
            string[] strArray = line.Split(',');
            DataTable dtUpload = new DataTable();
            DataRow rowUpload;
            DataTable dtCheck = new DataTable();
            DataRow rowCheck;
            dtUpload.Columns.Add(new DataColumn());
            dtCheck.Columns.Add(new DataColumn());
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

            DataSet dataCollection = (DataSet)Session["dataCollection"];
            int duplicateCount = 0;
            int notInDatabaseCount = 0;
            int notInStockCount = 0;

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

                if (Item_Serial.CheckInStock(oCon, dtCheck.Rows[x][0].ToString(), Convert.ToInt32(Session["idItem"].ToString())) == 0)
                {
                    notInStockCount = notInStockCount + 1;
                }

                try
                {
                    for (int dsc = 0; dsc <= dataCollection.Tables.Count - 1; dsc++)
                    {
                        for (int dst = 0; dst <= dataCollection.Tables[dsc].Rows.Count - 1; dst++)
                        {
                            if (dataCollection.Tables[dsc].Rows[dst][1].ToString() == dtCheck.Rows[x][0].ToString())
                            {
                                duplicateCount = duplicateCount + 1;
                            }
                        }
                    }
                }
                catch
                { }
            }

            if (notInStockCount == 0)
            {
                if (duplicateCount == 0)
                {
                    if (notInDatabaseCount == 0)
                    {
                        DataTable selectedPick = (DataTable)ViewState["vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()];
                        // selectedPick.TableName = Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString();
                        string tableName = selectedPick.TableName;

                        var x = Session["Quantity"].ToString();
                        if (selectedPick.Rows.Count <= Convert.ToInt32(x))
                        {
                            DataTable dtSerial = new DataTable();

                            for (var y = 0; y <= dtUpload.Rows.Count - 1; y++)
                            {
                                DataTable dataTable = Item_Serial.RetrieveData(oCon, Session["idItem"].ToString(), "", dtUpload.Rows[y][0].ToString());
                                dtSerial.Merge(dataTable);
                            }

                            for (var a = 0; a <= dtSerial.Rows.Count - 1; a++)
                            {
                                DataRow toInsert = selectedPick.NewRow();
                                toInsert[0] = dtSerial.Rows[a][0].ToString();
                                toInsert[1] = dtSerial.Rows[a][1].ToString();
                                toInsert[2] = dtSerial.Rows[a][2].ToString();
                                selectedPick.Rows.Add(toInsert);
                            }

                            if (!dataCollection.Tables.Contains(Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()))
                            {
                                dataCollection.Tables.Add(selectedPick);
                            }
                            else
                            {
                                dataCollection.Tables.Remove(tableName);
                                dataCollection.Tables.Add(selectedPick);
                            }

                            ViewState["vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()] = selectedPick;
                            Session["dataCollection"] = dataCollection;

                            gvSelectedSKU.DataSource = selectedPick;
                            gvSelectedSKU.DataBind();

                            HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "File Upload Complete!" + ControlChars.Quote + ");</script>");
                        }
                        else
                        {
                            HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Quantity doesn't match" + ControlChars.Quote + ");</script>");
                        }
                    }
                    else
                    {
                        HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "There are " + notInDatabaseCount.ToString() + " serial numbers that is not in the database." + ControlChars.Quote + ");</script>");
                    }
                }
                else
                {
                    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Duplicate Serial Numbers Detected : " + duplicateCount.ToString() + ControlChars.Quote + ");</script>");
                }
            }
            else
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Serial Numbers not in stock : " + notInStockCount.ToString() + ControlChars.Quote + ");</script>");
            }
        }

        //public bool SavePickActivityRegisterFile()
        //{
        //    bool resultValue = false;

        //    var lData = Pick_Activity.GetPickActivityRegister(oCon, txtSearchSO.Text.Trim());

        //    int i = 0;

        //    string strPickListDate = null;
        //    string strUserId = null;
        //    string strSiteDescription = null;
        //    string strSite = null;
        //    string strPickDate = null;
        //    string strPickListNumber = null;
        //    string strCustomerName = null;
        //    string strCustCode = null;
        //    string strAddress = null;
        //    string strExternalComments = null;
        //    string strSONumber = null;
        //    string OperationsComment = txtComments.Text.Trim();

        //    if (lData.Count != 0)
        //    {
        //        strPickListDate = lData[0].PickListDate;
        //        strUserId = lData[0].UserId.ToString().TrimEnd();

        //        strSiteDescription = lData[0].SiteDescription;
        //        strSite = lData[0].Site.ToString().TrimEnd();
        //        strPickDate = DateTime.Parse(lData[0].PickListDate).ToString("MMM dd, yyyy");
        //        strPickListNumber = lData[0].PickListNo;
        //        strCustomerName = lData[0].CustomerName;
        //        strCustCode = lData[0].CustCode;
        //        strAddress = lData[0].Address;
        //        strExternalComments = lData[0].ExternalComments;
        //        strSONumber = lData[0].SONumber;
        //    }

        //    Microsoft.Reporting.WebForms.ReportViewer viewer = new ReportViewer();
        //    viewer.ProcessingMode = ProcessingMode.Local;
        //    viewer.LocalReport.ReportPath = Server.MapPath(@"~\Resources\PickActivityRegister.rdlc");

        //    ReportParameter p1 = new ReportParameter("PickListDate", strPickListDate);
        //    ReportParameter p2 = new ReportParameter("UserID", "User ID: " + strUserId);
        //    ReportParameter p3 = new ReportParameter("SiteDescription", strSite + " - " + strSiteDescription);
        //    ReportParameter p4 = new ReportParameter("PickDate", strPickDate);
        //    ReportParameter p5 = new ReportParameter("PickListNumber", strPickListNumber);
        //    ReportParameter p6 = new ReportParameter("CustomerName", strCustomerName);
        //    ReportParameter p7 = new ReportParameter("CustCode", strCustCode);
        //    ReportParameter p8 = new ReportParameter("Address", strAddress);
        //    ReportParameter p9 = new ReportParameter("ExternalComments", strExternalComments + "(" + OperationsComment + ")");
        //    ReportParameter p10 = new ReportParameter("SONumber", strSONumber);

        //    ReportDataSource repDataSource = new ReportDataSource("DataSet1", lData);

        //    viewer.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 });

        //    viewer.LocalReport.DataSources.Clear();
        //    viewer.LocalReport.DataSources.Add(repDataSource);

        //    //Microsoft.Reporting.WebForms.Warning[] warnings = null;
        //    //string[] streamids = null;
        //    //string mimeType = null;
        //    //string encoding = null;
        //    //string extension = null;
        //    //string deviceInfo;
        //    //byte[] bytes;
        //    //Microsoft.Reporting.WebForms.LocalReport lr = new Microsoft.Reporting.WebForms.LocalReport();

        //    Warning[] warnings;
        //    string[] streamIds;
        //    string mimeType = string.Empty;
        //    string encoding = string.Empty;
        //    string extension = "pdf";

        //    byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
        //    // byte[] bytes = viewer.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
        //    // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.          
        //    // System.Web.HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.Buffer = true;
        //    Response.Clear();
        //    Response.ContentType = mimeType;
        //    Response.AddHeader("content-disposition", "attachment; filename= " + lblPickNumber.Text + "." + extension);
        //    Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
        //    Response.Flush(); // send it to the client to download  
        //    Response.End();
        //    return resultValue;
        //}

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            //Session["SONumber"] = txtSearchSO.Text;
            Session["Pick_Comments"] = txtComments.Text;
            Session["Pick_Number"] = lblPickNumber.Text;

            clearAll();
            lblConfirmMsg.Text = "Download Pick Activity Register?";
            myModalConfirmation.Visible = true;

            //ClientScript.RegisterStartupScript(this.GetType(), "alert", @"window.location.href='DownloadPickActivity.aspx", true);

            //SavePickActivityRegisterFile();
        }

        protected void btnUploadLotSerial_Click(object sender, EventArgs e)
        {
            DataTable dtUpload = Item_Serial.RetrieveLotSerial(oCon, Session["idItem"].ToString(), Session["Quantity"].ToString(), txtLotSerial.Text.Trim());
            DataTable dtSerial = Item_Serial.RetrieveLotSerial(oCon, Session["idItem"].ToString(), Session["Quantity"].ToString(), txtLotSerial.Text.Trim());

            DataSet dataCollection = (DataSet)Session["dataCollection"];
            DataTable selectedPick = (DataTable)ViewState["vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()];
            // selectedPick.TableName = Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString();
            string tableName = selectedPick.TableName;
            int duplicateCount = 0;
            var x = Session["Quantity"].ToString();

            if (dtSerial.Rows.Count == Convert.ToInt32(x))
            {
                for (var a = 0; a <= dtSerial.Rows.Count - 1; a++)
                {
                    for (int dsc = 0; dsc <= dataCollection.Tables.Count - 1; dsc++)
                    {
                        for (int dst = 0; dst <= dataCollection.Tables[dsc].Rows.Count - 1; dst++)
                        {
                            if (dataCollection.Tables[dsc].Rows[dst][1].ToString() == dtSerial.Rows[a][1].ToString())
                            {
                                duplicateCount = duplicateCount + 1;
                            }
                        }
                    }

                    if (duplicateCount == 0)
                    {
                        DataRow toInsert = selectedPick.NewRow();
                        toInsert[0] = dtSerial.Rows[a][0].ToString();
                        toInsert[1] = dtSerial.Rows[a][1].ToString();
                        toInsert[2] = dtSerial.Rows[a][2].ToString();
                        selectedPick.Rows.Add(toInsert);
                    }
                }

                if (!dataCollection.Tables.Contains(Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()))
                {
                    dataCollection.Tables.Add(selectedPick);
                }
                else
                {
                    dataCollection.Tables.Remove(tableName);
                    dataCollection.Tables.Add(selectedPick);
                }

                ViewState["vs" + Session["ItemName"].ToString() + "-" + Session["gvItemIndex"].ToString()] = selectedPick;
                Session["dataCollection"] = dataCollection;

                gvSelectedSKU.DataSource = selectedPick;
                gvSelectedSKU.DataBind();
            }
            else
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Insufficient Stock" + ControlChars.Quote + ");</script>");
            }
        }

        protected void btnSearchSerial_Click(object sender, EventArgs e)
        {
            bindSKUList();
        }

        protected void GetSOtoGrid_(string SONumber, string soStatus)
        {
            var ldata = SO_Header.RetrieveDataForPick(oCon, txtSearchSO.Text.Trim(), soStatus);
            grvMainSO.DataSource = ldata;

            grvMainSO.DataBind();
            grvMainSO.GridLines = GridLines.None;
        }

        protected void grvMainSO_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int x = Session.Count - 1; x >= 6; x--)
            {
                Session.RemoveAt(x);
            }

            clearAll();
            btnBack.Visible = true;
            GridViewRow row = grvMainSO.SelectedRow;
            Session["SONumber"] = row.Cells[1].Text.Trim();
            GetSOHeaderDetails(row.Cells[1].Text.Trim());
            btnSaveAllPick.Visible = true;
            SOPanel.Visible = false;
            pnlAllDetails.Visible = true;
            tblSearch.Visible = false;
        }

        protected void grvMainSO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            foreach (GridViewRow rowProduct in grvMainSO.Rows)
            {
                if (rowProduct.Cells[8].Text == "DELETED" || rowProduct.Cells[8].Text == "EXPIRED")
                {
                    rowProduct.BackColor = ColorTranslator.FromHtml("#F08080");
                    rowProduct.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
                }
                if (rowProduct.Cells[8].Text == "Open")
                {
                    rowProduct.BackColor = ColorTranslator.FromHtml("#98FB98");
                    rowProduct.ForeColor = ColorTranslator.FromHtml("#000000");
                }
            }

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
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grvMainSO, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.ToolTip = "Click to select this row.";
            }
        }

        protected void grvMainSO_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvMainSO.PageIndex = e.NewPageIndex;
            GetSOtoGrid_(txtSearchSO.Text.Trim(), ddlSOStatus.SelectedValue.ToString());
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            clearAll();

            for (int x = Session.Count - 1; x >= 6; x--)
            {
                Session.RemoveAt(x);
            }

            if (Session["dataCollection"] == null)
            {
                DataSet dataCollection = new DataSet();
                Session["dataCollection"] = dataCollection;
            }
        }

        protected void ddlSOStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetSOtoGrid_(txtSearchSO.Text.Trim(), ddlSOStatus.SelectedValue.ToString());
        }
    }
}