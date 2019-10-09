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

namespace SOPOINV.Forms
{
    public partial class SalesReturn : System.Web.UI.Page
    {
        SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnVerifySerial);
            scriptManager.RegisterPostBackControl(this.btnSaveAllReturn);
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

        #region  "Sales Return"
        private void getSite()
        {
            var ds = Site_Loc.RetrieveData(oCon, "");
            ddlSite.DataSource = ds;
            ddlSite.DataTextField = "Site_Desc";
            ddlSite.DataValueField = "idSite";
            ddlSite.DataBind();
        }

        private void getItemDetails()
        {
            var ds = Sales_Return.RetrieveData(oCon, txtSearchInput.Text);
            grvItemDetails.DataSource = ds;
            grvItemDetails.DataBind();
        }

        private void getReasonCode()
        {
            var ds = Reason_Code.RetrieveData(oCon);
            ds.Columns.Add("DisplayText", typeof(string));
            ds.Columns["DisplayText"].Expression = "reason_code + '-' +reason_desc";
            ddlReasonCode.DataSource = ds;
            ddlReasonCode.DataTextField = "DisplayText";
            ddlReasonCode.DataValueField = "reason_code";
            ddlReasonCode.DataBind();
        }

        private void getSODetails()
        {
            if (txtSearchInput.Text.Trim() != "")
            {
                var ds = SO_Header.RetrieveData(oCon, txtSearchInput.Text, false);
                txtSONumber.Text = ds[0].SO_Number;

                var dsCustomer = Customer_Details.RetrieveData(oCon, ds[0].idCustomer, "");
                txtCustomerCode.Text = dsCustomer[0].Customer_Code;
                txtReseller.Text = dsCustomer[0].Company_Name;
            }
            else
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Please enter SO Number to search" + ControlChars.Quote + ");</script>");
            }
        }

        protected void grvItemDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblVerify.Text = "";
            dvUploadReturn.Visible = true;
            GridViewRow row = grvItemDetails.SelectedRow;
            Session["rowIndex"] = row.RowIndex.ToString();
            Session["idItem"] = row.Cells[0].Text.Trim();
            //txtSONumber.Text = row.Cells[1].Text.Trim();
            Session["Item_Number"] = row.Cells[2].Text.Trim();
            Session["Cost"] = row.Cells[4].Text.Trim();
            RetrieveSerialList(Convert.ToInt32(row.Cells[0].Text.Trim()), txtSONumber.Text);
            if (ddlReasonCode.SelectedValue.Trim().ToString() == "210" || ddlReasonCode.SelectedValue.Trim().ToString() == "165")
            { dvReplacement.Visible = true; }
            else { dvReplacement.Visible = false; }
        }

        protected void grvItemDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;//idItem
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;//idItem         
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grvItemDetails, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click To Select this row";
            }
        }

        protected void grvItemDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvItemDetails.PageIndex = e.NewPageIndex;
            getItemDetails();
        }

        protected void btnSearchInput_Click(object sender, EventArgs e)
        {
            Session["ReturnDataSet"] = new DataSet();
            getSite();
            getSODetails();
            getItemDetails();
            getReasonCode();
            txtReturnDate.Text = DateTime.Now.ToShortDateString();
        }

        protected void btnCancelReturn_Click(object sender, EventArgs e)
        {
            dvUploadReturn.Visible = false;
        }

        private void clearReturns()
        {
            dvUploadReturn.Visible = false;
            txtSearchInput.Text = "";
            txtSRERemarks.Text = "";
            txtInvoiceNumber.Text = "";
            txtSONumber.Text = "";
            lblVerify.Text = "";
            txtAuthorizationNumber.Text = "";
            txtCustomerCode.Text = "";
            txtInvoiceNumber.Text = "";
            txtReseller.Text = "";
            txtReturnDate.Text = "";
            getItemDetails();
        }

        protected void btnVerifySerial_Click(object sender, EventArgs e)
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

            int duplicateCount = 0;
            int notInDatabaseCount = 0;
            int TransHistDuplicate = 0;
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

                if (Trans_History.CheckDuplicate(oCon, dtCheck.Rows[x][0].ToString(), Convert.ToInt32(Session["idItem"].ToString()), txtSONumber.Text) == 1)
                {
                    TransHistDuplicate = TransHistDuplicate + 1;
                }
            }

            if (duplicateCount == 0)
            {
                if (notInDatabaseCount == 0)
                {
                    if (TransHistDuplicate == 0)
                    {
                        string dtName = Session["rowIndex"].ToString() + "-" + Session["Item_Number"].ToString();
                        Session[dtName] = dtUpload;
                        lblVerify.Text = "Serial numbers verified!";

                        btnSaveReturn.Visible = true;
                        //HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Serial numbers verified!" + ControlChars.Quote + ");</script>");                    
                    }
                    else
                    {
                        lblVerify.Text = "There are " + TransHistDuplicate.ToString() + " serial numbers that is already returned.";
                        //HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "There are " + notInDatabaseCount.ToString() + " serial numbers that is not in the database." + ControlChars.Quote + ");</script>");
                    }
                }
                else
                {
                    lblVerify.Text = "There are " + notInDatabaseCount.ToString() + " serial numbers that are not included in the specified item number.";
                    //HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "There are " + notInDatabaseCount.ToString() + " serial numbers that is not in the database." + ControlChars.Quote + ");</script>");
                }
            }
            else
            {
                lblVerify.Text = "Duplicate serial numbers detected in file : " + duplicateCount.ToString();
                //HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Duplicate serial numbers detected in file : " + duplicateCount.ToString() + ControlChars.Quote + ");</script>");
            }
        }

        protected void btnSaveReturn_Click(object sender, EventArgs e)
        {
            string dtName = Session["rowIndex"].ToString() + "-" + Session["Item_Number"].ToString();
            DataTable returnTable = (DataTable)Session[dtName];
            returnTable.TableName = dtName;

            DataSet ReturnDataSet = (DataSet)Session["ReturnDataSet"];

            if (ddlReasonCode.SelectedValue.Trim().ToString() == "210" || ddlReasonCode.SelectedValue.Trim().ToString() == "165")
            {
                string dtRepName = "Rep" + Session["rowIndex"].ToString() + "-" + Session["Item_Number"].ToString();
                DataTable returnTableRep = (DataTable)Session[dtRepName];
                returnTable.TableName = dtRepName;

                if (!ReturnDataSet.Tables.Contains(dtRepName))
                {
                    ReturnDataSet.Tables.Add(returnTableRep);
                }
                else
                {
                    ReturnDataSet.Tables.Remove(dtRepName);
                    ReturnDataSet.Tables.Add(returnTableRep);
                }
            }

            if (!ReturnDataSet.Tables.Contains(dtName))
            {
                ReturnDataSet.Tables.Add(returnTable);
            }
            else
            {
                ReturnDataSet.Tables.Remove(dtName);
                ReturnDataSet.Tables.Add(returnTable);
            }

            Session["ReturnDataSet"] = ReturnDataSet;
            dvUploadReturn.Visible = false;
            btnSaveReturn.Visible = false;
        }

        protected void btnClearSRE_Click(object sender, EventArgs e)
        {
            try
            {
                string dtName = Session["rowIndex"].ToString() + "-" + Session["Item_Number"].ToString();
                Session[dtName] = null;
            }
            catch
            {

            }

            clearReturns();
        }

        protected void btnSaveAllReturn_Click(object sender, EventArgs e)
        {
            if (ddlReasonCode.SelectedValue.Trim().ToString() == "210" || ddlReasonCode.SelectedValue.Trim().ToString() == "165")
            {
                string idSLR = Return_Header.GetLastSLRNumber(oCon);
                string SLR_Number = "";
                if (idSLR == "")
                {
                    idSLR = "SLR000000";
                }
                idSLR = idSLR.Substring(3);
                int idSLR_ = int.Parse(idSLR) + 1;

                SLR_Number = "SLR" + idSLR_.ToString("000000");

                string isreplacement = "";
                isreplacement = "Y";

                Return_Header_Model rhm = new Return_Header_Model
                {
                    SLR_Number = SLR_Number,
                    SO_Number = txtSONumber.Text,
                    Customer_Code = txtCustomerCode.Text,
                    Auth_Number = txtAuthorizationNumber.Text,
                    isReplacement = isreplacement,
                    Document_Number = txtInvoiceNumber.Text,
                    Site = ddlSite.SelectedValue,
                    Reason_Code = ddlReasonCode.SelectedValue,
                    Date_Returned = DateTime.Parse(txtReturnDate.Text),
                    Remarks = txtSRERemarks.Text
                };
                int ReturnHeaderid = Return_Header.Save(oCon, rhm);

                DataSet ReturnDataSet = (DataSet)Session["ReturnDataSet"];

                foreach (GridViewRow row in grvItemDetails.Rows)
                {
                    string tablename = "Rep" + row.RowIndex.ToString() + "-" + row.Cells[2].Text;
                    DataTable dtReplacement = ReturnDataSet.Tables[tablename];
                    string tablename2 = row.RowIndex.ToString() + "-" + row.Cells[2].Text;
                    DataTable dtUpload = ReturnDataSet.Tables[tablename];

                    if (ReturnHeaderid != 0)
                    {
                        Return_Detail_Model rdm = new Return_Detail_Model
                        {
                            idReturnHeader = ReturnHeaderid,
                            idItem = Convert.ToInt32(row.Cells[0].Text),
                            Returned_Qty = Convert.ToInt32(dtUpload.Rows.Count),
                            Returned_Cost = Convert.ToDecimal(row.Cells[4].Text)
                        };
                        int returnDetailId = Return_Detail.Save(oCon, rdm);

                        if (returnDetailId != 0)
                        {
                            for (var y = 0; y <= dtUpload.Rows.Count - 1; y++)
                            {
                                Item_Serial.ReturnStock(oCon, Convert.ToInt32(row.Cells[0].Text), dtUpload.Rows[y][0].ToString(), dtReplacement.Rows[y][0].ToString());

                                Return_Serial_Model rsm = new Return_Serial_Model
                                {
                                    idReturnDetail = returnDetailId,
                                    Serial_No = dtUpload.Rows[y][0].ToString(),
                                    Replacement_Serial = dtReplacement.Rows[y][0].ToString()
                                };
                                bool saveValue = Return_Serial.Save(oCon, rsm);

                                if (saveValue == true)
                                {
                                    Trans_History_Model trans_History_Model = new Trans_History_Model
                                    {
                                        Trans_Code = "SRE",
                                        Item_Number = row.Cells[2].Text,
                                        Site = ddlSite.SelectedItem.Text,
                                        UM = "UT",
                                        Doc_No = SLR_Number,
                                        Serial_No = dtUpload.Rows[y][0].ToString(),
                                        Reason_Code = ddlReasonCode.SelectedValue.ToString(),
                                        Trans_Date = DateTime.Now,
                                        Order_No = txtSONumber.Text,
                                        Invoice_No = txtInvoiceNumber.Text,
                                        Reference_No = txtAuthorizationNumber.Text,
                                        Trans_Qty = 1,
                                        Trans_Amt = Convert.ToDecimal(row.Cells[4].Text),
                                        Remarks = txtSRERemarks.Text,
                                        user_domain = Session["User_Domain"].ToString()
                                    };
                                    Trans_History.Save(oCon, trans_History_Model);
                                }
                            }
                        }
                    }
                }

                Item_Master.InventoryCheckForError(oCon);

                clearReturns();

                Response.Write("<script>window.open('/Forms/DownloadReports/DownloadSalesReturn.aspx?SLR_Number=" + SLR_Number + "','_blank');</script>");
            }
            else if(ddlReasonCode.SelectedValue.Trim().ToString() == "169")
            {
                string idSLR = Return_Header.GetLastSLRNumber(oCon);
                string SLR_Number = "";
                if (idSLR == "")
                {
                    idSLR = "SLR000000";
                }
                idSLR = idSLR.Substring(3);
                int idSLR_ = int.Parse(idSLR) + 1;

                SLR_Number = "SLR" + idSLR_.ToString("000000");

                string isreplacement = "";
                isreplacement = "N";

                Return_Header_Model rhm = new Return_Header_Model
                {
                    SLR_Number = SLR_Number,
                    SO_Number = txtSONumber.Text,
                    Customer_Code = txtCustomerCode.Text,
                    Auth_Number = txtAuthorizationNumber.Text,
                    isReplacement = isreplacement,
                    Document_Number = txtInvoiceNumber.Text,
                    Site = ddlSite.SelectedValue,
                    Reason_Code = ddlReasonCode.SelectedValue,
                    Date_Returned = DateTime.Parse(txtReturnDate.Text),
                    Remarks = txtSRERemarks.Text
                };
                int ReturnHeaderid = Return_Header.Save(oCon, rhm);

                DataSet ReturnDataSet = (DataSet)Session["ReturnDataSet"];

                foreach (GridViewRow row in grvItemDetails.Rows)
                {
                    string tablename = row.RowIndex.ToString() + "-" + row.Cells[2].Text;
                    DataTable dtUpload = ReturnDataSet.Tables[tablename];

                    if (ReturnHeaderid != 0)
                    {
                        Return_Detail_Model rdm = new Return_Detail_Model
                        {
                            idReturnHeader = ReturnHeaderid,
                            idItem = Convert.ToInt32(row.Cells[0].Text),
                            Returned_Qty = Convert.ToInt32(dtUpload.Rows.Count),
                            Returned_Cost = Convert.ToDecimal(row.Cells[4].Text)
                        };
                        int returnDetailId = Return_Detail.Save(oCon, rdm);

                        if (returnDetailId != 0)
                        {
                            for (var y = 0; y <= dtUpload.Rows.Count - 1; y++)
                            {
                                Item_Serial.ReturnStock(oCon,  dtUpload.Rows[y][0].ToString(), row.Cells[2].Text);

                                Return_Serial_Model rsm = new Return_Serial_Model
                                {
                                    idReturnDetail = returnDetailId,
                                    Serial_No = dtUpload.Rows[y][0].ToString(),
                                    Replacement_Serial = ""
                                };
                                bool saveValue = Return_Serial.Save(oCon, rsm);

                                if (saveValue == true)
                                {
                                    Trans_History_Model trans_History_Model = new Trans_History_Model
                                    {
                                        Trans_Code = "SRE",
                                        Item_Number = row.Cells[2].Text,
                                        Site = ddlSite.SelectedValue.ToString(),
                                        UM = "UT",
                                        Doc_No = SLR_Number,
                                        Serial_No = dtUpload.Rows[y][0].ToString(),
                                        Reason_Code = ddlReasonCode.SelectedValue.ToString(),
                                        Trans_Date = DateTime.Now,
                                        Order_No = txtSONumber.Text,
                                        Invoice_No = txtInvoiceNumber.Text,
                                        Reference_No = txtAuthorizationNumber.Text,
                                        Trans_Qty = 1,
                                        Trans_Amt = Convert.ToDecimal(row.Cells[4].Text),
                                        Remarks = txtSRERemarks.Text,
                                        user_domain = Session["User_Domain"].ToString()
                                    };
                                    Trans_History.Save(oCon, trans_History_Model);
                                }
                            }
                        }
                    }
                }

                Item_Master.InventoryCheckForError(oCon);

                clearReturns();

                Response.Write("<script>window.open('/Forms/DownloadReports/DownloadSalesReturn.aspx?SLR_Number=" + SLR_Number + "','_blank');</script>");
            }
            else
            {
                string idSLR = Return_Header.GetLastSLRNumber(oCon);
                string SLR_Number = "";
                if (idSLR == "")
                {
                    idSLR = "SLR000000";
                }
                idSLR = idSLR.Substring(3);
                int idSLR_ = int.Parse(idSLR) + 1;

                SLR_Number = "SLR" + idSLR_.ToString("000000");

                string isreplacement = "";
                isreplacement = "N";

                Return_Header_Model rhm = new Return_Header_Model
                {
                    SLR_Number = SLR_Number,
                    SO_Number = txtSONumber.Text,
                    Customer_Code = txtCustomerCode.Text,
                    Auth_Number = txtAuthorizationNumber.Text,
                    isReplacement = isreplacement,
                    Document_Number = txtInvoiceNumber.Text,
                    Site = ddlSite.SelectedValue,
                    Reason_Code = ddlReasonCode.SelectedValue,
                    Date_Returned = DateTime.Parse(txtReturnDate.Text),
                    Remarks = txtSRERemarks.Text
                };
                int ReturnHeaderid = Return_Header.Save(oCon, rhm);

                DataSet ReturnDataSet = (DataSet)Session["ReturnDataSet"];

                foreach (GridViewRow row in grvItemDetails.Rows)
                {
                    string tablename = row.RowIndex.ToString() + "-" + row.Cells[2].Text;
                    DataTable dtUpload = ReturnDataSet.Tables[tablename];

                    if (ReturnHeaderid != 0)
                    {
                        Return_Detail_Model rdm = new Return_Detail_Model
                        {
                            idReturnHeader = ReturnHeaderid,
                            idItem = Convert.ToInt32(row.Cells[0].Text),
                            Returned_Qty = Convert.ToInt32(dtUpload.Rows.Count),
                            Returned_Cost = Convert.ToDecimal(row.Cells[4].Text)
                        };
                        int returnDetailId = Return_Detail.Save(oCon, rdm);

                        if (returnDetailId != 0)
                        {
                            for (var y = 0; y <= dtUpload.Rows.Count - 1; y++)
                            {
                                Item_Serial.ReturnStock(oCon, Convert.ToInt32(row.Cells[0].Text), dtUpload.Rows[y][0].ToString());

                                Return_Serial_Model rsm = new Return_Serial_Model
                                {
                                    idReturnDetail = returnDetailId,
                                    Serial_No = dtUpload.Rows[y][0].ToString(),
                                    Replacement_Serial = ""
                                };
                                bool saveValue = Return_Serial.Save(oCon, rsm);

                                if (saveValue == true)
                                {
                                    Trans_History_Model trans_History_Model = new Trans_History_Model
                                    {
                                        Trans_Code = "SRE",
                                        Item_Number = row.Cells[2].Text,
                                        Site = ddlSite.SelectedValue.ToString(),
                                        UM = "UT",
                                        Doc_No = SLR_Number,
                                        Serial_No = dtUpload.Rows[y][0].ToString(),
                                        Reason_Code = ddlReasonCode.SelectedValue.ToString(),
                                        Trans_Date = DateTime.Now,
                                        Order_No = txtSONumber.Text,
                                        Invoice_No = txtInvoiceNumber.Text,
                                        Reference_No = txtAuthorizationNumber.Text,
                                        Trans_Qty = 1,
                                        Trans_Amt = Convert.ToDecimal(row.Cells[4].Text),
                                        Remarks = txtSRERemarks.Text,
                                        user_domain = Session["User_Domain"].ToString()
                                    };
                                    Trans_History.Save(oCon, trans_History_Model);
                                }
                            }
                        }
                    }
                }

                Item_Master.InventoryCheckForError(oCon);

                clearReturns();

                Response.Write("<script>window.open('/Forms/DownloadReports/DownloadSalesReturn.aspx?SLR_Number=" + SLR_Number + "','_blank');</script>");
            }
        }

        protected void btnUploadReplacementSerial_Click(object sender, EventArgs e)
        {
            StreamReader SR = new StreamReader(uploadReplacementSerial.PostedFile.InputStream);
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

            int duplicateCount = 0;
            int notInDatabaseCount = 0;
            int TransHistDuplicate = 0;

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

                if (Item_Serial.CheckDuplicate(oCon, dtCheck.Rows[x][0].ToString(), Convert.ToInt32(Session["idItem"].ToString())) == 1)
                {
                    notInDatabaseCount = notInDatabaseCount + 1;
                }

                if (Trans_History.CheckDuplicate(oCon, dtCheck.Rows[x][0].ToString(), Convert.ToInt32(Session["idItem"].ToString()), txtSONumber.Text) == 0)
                {
                    TransHistDuplicate = TransHistDuplicate + 1;
                }
            }

            if (duplicateCount == 0)
            {
                if (notInDatabaseCount == 0)
                {
                    if (TransHistDuplicate == 0)
                    {
                        string dtName = "Rep" + Session["rowIndex"].ToString() + "-" + Session["Item_Number"].ToString();
                        Session[dtName] = dtUpload;
                        lblVerify.Text = "Serial numbers verified!";

                        btnSaveReturn.Visible = true;
                        //HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Serial numbers verified!" + ControlChars.Quote + ");</script>");                    
                    }
                    else
                    {
                        lblVerify.Text = "There are " + TransHistDuplicate.ToString() + " serial numbers that is already returned.";
                        //HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "There are " + notInDatabaseCount.ToString() + " serial numbers that is not in the database." + ControlChars.Quote + ");</script>");
                    }
                }
                else
                {
                    lblVerify.Text = "There are " + notInDatabaseCount.ToString() + " serial numbers that are not included in the specified item number.";
                    //HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "There are " + notInDatabaseCount.ToString() + " serial numbers that is not in the database." + ControlChars.Quote + ");</script>");
                }
            }
            else
            {
                lblVerify.Text = "Duplicate serial numbers detected in file : " + duplicateCount.ToString();
                //HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Duplicate serial numbers detected in file : " + duplicateCount.ToString() + ControlChars.Quote + ");</script>");
            }
        }

        private void RetrieveSerialList(int idItem, string soNumber)
        {
            var dt = Pick_Serial.RetrieveData(oCon, idItem, soNumber);
            gvSerialList.DataSource = dt;
            gvSerialList.DataBind();
        }

        protected void gvSerialList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvSerialList_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        protected void btnSavePickedFromGV_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Serial_No");
            foreach (GridViewRow item in gvSerialList.Rows)
            {
                if ((item.Cells[0].FindControl("cbSelect") as CheckBox).Checked)
                {
                    DataRow dr = dt.NewRow();
                    dr["Serial_No"] = item.Cells[2].Text;
                    dt.Rows.Add(dr);
                }
            }

            string dtName = Session["rowIndex"].ToString() + "-" + Session["Item_Number"].ToString();
            DataTable returnTable = dt;
            returnTable.TableName = dtName;

            DataSet ReturnDataSet = (DataSet)Session["ReturnDataSet"];

            if (ddlReasonCode.SelectedValue.Trim().ToString() == "210" || ddlReasonCode.SelectedValue.Trim().ToString() == "165")
            {
                string dtRepName = "Rep" + Session["rowIndex"].ToString() + "-" + Session["Item_Number"].ToString();
                DataTable returnTableRep = (DataTable)Session[dtRepName];
                returnTable.TableName = dtRepName;

                if (!ReturnDataSet.Tables.Contains(dtRepName))
                {
                    ReturnDataSet.Tables.Add(returnTableRep);
                }
                else
                {
                    ReturnDataSet.Tables.Remove(dtRepName);
                    ReturnDataSet.Tables.Add(returnTableRep);
                }
            }

            if (!ReturnDataSet.Tables.Contains(dtName))
            {
                ReturnDataSet.Tables.Add(returnTable);
            }
            else
            {
                ReturnDataSet.Tables.Remove(dtName);
                ReturnDataSet.Tables.Add(returnTable);
            }

            Session["ReturnDataSet"] = ReturnDataSet;
            dvUploadReturn.Visible = false;
            btnSaveReturn.Visible = false;
        }

        //protected void gvSerialList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    gvSerialList.PageIndex = e.NewPageIndex;
        //    RetrieveSerialList(Convert.ToInt32(Session["idItem"].ToString()),txtSONumber.Text);
        //}
        #endregion

        //#region "Cost Adjustment"
        //public void setItemProducts(string itemNumber)
        //{
        //    var idItem = Item_Master.RetrieveData(oCon, "", "", itemNumber, 0);

        //    Session["idItem"] = idItem.Rows[0][0].ToString();

        //    DataTable ldata = Item_Serial.GetDetailsForCostAdjustment(oCon, itemNumber);


        //    if (ldata.Rows[0][0].ToString() == "")
        //    {
        //        txtOnHandQty.Text = "0";
        //    }
        //    else
        //    {
        //        txtOnHandQty.Text = ldata.Rows[0][0].ToString();
        //    }

        //    if (ldata.Rows[0][1].ToString() == "")
        //    {
        //        txtCostPerUnit.Text = "0";
        //    }
        //    else
        //    {
        //        txtCostPerUnit.Text = ldata.Rows[0][1].ToString();
        //    }

        //    if (ldata.Rows[0][0].ToString() == "")
        //    {
        //        txtCostAdjQty.Text = "0";
        //    }
        //    else
        //    {
        //        txtCostAdjQty.Text = ldata.Rows[0][0].ToString();
        //    }
        //    txtOnHandCost.Text = (Convert.ToDecimal(txtOnHandQty.Text) * Convert.ToDecimal(txtCostPerUnit.Text)).ToString();
        //}

        //protected void btnSearchItem_Click(object sender, EventArgs e)
        //{
        //    lblTransactionDate.Text = DateTime.Now.ToString();
        //    setItemProducts(txtItemSearch.Text.Trim());
        //}

        //protected void btnSearchPO_Click(object sender, EventArgs e)
        //{
        //    lblTransactionDate.Text = DateTime.Now.ToString();
        //    setItemProducts(txtItemSearch.Text.Trim());
        //}

        //protected void btnSaveCostAdj_Click(object sender, EventArgs e)
        //{
        //    string CostAdjNum = Cost_Adjustment.GetLastCostAdjNum(oCon);
        //    if (CostAdjNum == null)
        //    {
        //        CostAdjNum = "CF000000";
        //    }
        //    CostAdjNum = CostAdjNum.Substring(2);
        //    int CostAdjNum_ = int.Parse(CostAdjNum) + 1;

        //    string CostAdjNumber = "CF" + CostAdjNum_.ToString("000000");

        //    Cost_Adjustment_Model Cost_Adjustment_Model = new Cost_Adjustment_Model
        //    {
        //        CostAdjustNumber = CostAdjNumber,
        //        idItem = Convert.ToInt32(Session["idItem"].ToString()),
        //        InitialCost = Convert.ToDecimal(txtCostPerUnit.Text),
        //        InitialQuantity = Convert.ToInt32(txtOnHandQty.Text),
        //        AdjustedCostPerUnit = Convert.ToDecimal(txtCostAdjPerUnit.Text),
        //        AdjustedQuantity = Convert.ToInt32(txtCostAdjQty.Text),
        //        AdjustedAmount = Convert.ToDecimal(txtCostAdjAmt.Text),
        //        DocumentNumber = txtDocNumber.Text,
        //        Transaction_Date = DateTime.Now,
        //        Remarks = txtCARemarks.Text,
        //        user_id_chg_by = Session["User_Domain"].ToString()
        //    };
        //    bool save = Cost_Adjustment.Save(oCon, Cost_Adjustment_Model);

        //    if (save == true)
        //    {
        //        bool updateCost = Item_Serial.UpdateCost(oCon, Convert.ToInt32(Session["idItem"].ToString()), Convert.ToDecimal(txtCostAdjPerUnit.Text));
        //        if (updateCost == true)
        //        {
        //            Item_Master.RecomputeItemCost(oCon, Convert.ToInt32(Session["idItem"].ToString()));

        //            Trans_History_Model trans_History_Model = new Trans_History_Model
        //            {
        //                Trans_Code = "COF",
        //                Item_Number = txtItemSearch.Text,
        //                Site = "WH-JMS",
        //                UM = "UT",
        //                Doc_No = txtDocNumber.Text,
        //                Serial_No = "",
        //                Reason_Code = "",
        //                Trans_Date = DateTime.Now,
        //                Order_No = "",
        //                Invoice_No = "",
        //                Reference_No = "",
        //                Trans_Qty = Convert.ToInt32(txtCostAdjQty.Text),
        //                Trans_Amt = Convert.ToDecimal(txtCostAdjAmt.Text),
        //                Remarks = txtCARemarks.Text,
        //                user_domain = Session["User_Domain"].ToString()
        //            };
        //            Trans_History.Save(oCon, trans_History_Model);

        //            HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Saving Complete! Cost Adjustment # : " + CostAdjNumber + ControlChars.Quote + ");</script>");

        //            ClearAdj();
        //        }
        //        else
        //        {
        //            HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Error in updating cost" + ControlChars.Quote + ");</script>");
        //        }
        //    }
        //    else
        //    {
        //        HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Error in saving cost adjustment" + ControlChars.Quote + ");</script>");
        //    }
        //}

        //protected void btnComputeAdj_Click(object sender, EventArgs e)
        //{
        //    if (txtOnHandQty.Text != "0" || txtOnHandQty.Text != "")
        //    {
        //        txtCostAdjPerUnit.Text = ((Convert.ToDecimal(txtCostAdjAmt.Text) + Convert.ToDecimal(txtOnHandCost.Text)) / Convert.ToDecimal(txtCostAdjQty.Text)).ToString();

        //        btnSaveCostAdj.Visible = true;
        //        btnComputeAdj.Visible = false;
        //    }
        //    else
        //    {
        //        HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Quantity can't be zero" + ControlChars.Quote + ");</script>");
        //    }
        //}

        //private void ClearAdj()
        //{
        //    lblCostAdj.Visible = false;
        //    lblCostAdj_.Visible = false;
        //    lblTransactionDate.Visible = false;
        //    lblTransactionDate_.Visible = false;
        //    btnSaveCostAdj.Visible = false;
        //    btnComputeAdj.Visible = true;
        //    txtCostAdjAmt.Text = "";
        //    txtCostAdjPerUnit.Text = "";
        //    txtCostAdjQty.Text = "";
        //    txtCostPerUnit.Text = "";
        //    txtDocNumber.Text = "";
        //    txtItemSearch.Text = "";
        //    txtOnHandCost.Text = "";
        //    txtOnHandQty.Text = "";
        //    txtCARemarks.Text = "";
        //    txtSearchCA.Text = "";
        //}

        //protected void btnSearchCA_Click(object sender, EventArgs e)
        //{
        //    var ldata = Cost_Adjustment.RetreiveData(oCon, txtSearchCA.Text);

        //    txtCostAdjAmt.Text = ldata[0].AdjustedAmount.ToString();
        //    txtCostAdjPerUnit.Text = ldata[0].AdjustedCostPerUnit.ToString();
        //    txtCostAdjQty.Text = ldata[0].AdjustedQuantity.ToString();
        //    txtCostPerUnit.Text = ldata[0].InitialCost.ToString();
        //    txtDocNumber.Text = ldata[0].DocumentNumber.ToString();
        //    txtOnHandCost.Text = ldata[0].InitialCost.ToString();
        //    txtOnHandQty.Text = ldata[0].InitialQuantity.ToString();
        //    txtCARemarks.Text = ldata[0].Remarks.ToString();

        //    var idItem = Item_Master.RetrieveData(oCon, "", "", "", ldata[0].idItem);
        //    txtItemSearch.Text = idItem.Rows[0][3].ToString();
        //    lblTransactionDate.Visible = true;
        //    lblTransactionDate_.Visible = true;
        //    lblTransactionDate.Text = ldata[0].Transaction_Date.ToString();
        //    lblCostAdj.Visible = true;
        //    lblCostAdj_.Visible = true;
        //    lblCostAdj.Text = ldata[0].CostAdjustNumber.ToString();
        //}

        //protected void btnClear_Click(object sender, EventArgs e)
        //{
        //    ClearAdj();
        //}
        //#endregion

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