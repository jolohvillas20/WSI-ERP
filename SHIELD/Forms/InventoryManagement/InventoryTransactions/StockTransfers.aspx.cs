using POSOINV.Functions;
using POSOINV.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOPOINV.Forms
{
    public partial class StockTransfers : System.Web.UI.Page
    {
        SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            scriptManager.RegisterPostBackControl(this.btnTransfer);
            scriptManager.RegisterPostBackControl(this.btnOpenModal);
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
                        getHistory("", "");
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

        protected void btnSearchInput_Click(object sender, EventArgs e)
        {
            if (txtSearchItemNumber.Text != "" && txtSearchSerialNumber.Text != "")
            {
                getHistory(txtSearchItemNumber.Text, txtSearchSerialNumber.Text);
                btnOpenModal.Visible = true;
            }
            else
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Please enter a serial number" + ControlChars.Quote + ");</script>");
            }
        }

        protected void btnClearST_Click(object sender, EventArgs e)
        {
            btnOpenModal.Visible = false;
            txtSearchItemNumber.Text = "";
            txtSearchSerialNumber.Text = "";
            getHistory("", "");
        }

        protected void gvHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
            }
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvHistory, "Select$" + e.Row.RowIndex.ToString()));
            //    e.Row.ToolTip = "Click to select this row.";
            //}
        }

        protected void gvHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvHistory.PageIndex = e.NewPageIndex;
            getHistory(txtSearchItemNumber.Text, txtSearchSerialNumber.Text);
        }

        private void getHistory(string ItemNumber, string SerialNumber)
        {
            var dt = Trans_History.RetrieveDataForStockTransfer(oCon, ItemNumber, SerialNumber);
            gvHistory.DataSource = dt;
            gvHistory.DataBind();
        }

        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            string transcode = "";
            string sourcesite = "";
            string ordernumber = "";
            bool result = false;
            if (lblStockCode.Text == "PUR" || lblStockCode.Text == "STI")
            {
                transcode = "STO";
                ordernumber = "";
            }
            else if (lblStockCode.Text == "STO")
            {
                transcode = "STI";
                ordernumber = "";
            }

            if (ddlSourceSite.SelectedValue.ToString() == "WH-JMS")
                transcode = "STO";
            else
                transcode = "STI";

            string idSO = Stock_Transfer_Header.GetLastDocNumber(oCon);
            string doc_number = "";
            if (idSO == null)
            {
                idSO = DateTime.Now.Year.ToString() + "000";
            }
            idSO = idSO.Substring(4);
            int idSO_ = int.Parse(idSO) + 1;

            doc_number = DateTime.Now.Year.ToString() + idSO_.ToString("000");

            if (dvstockcode.Visible == true && dvuploadexcel.Visible == false)
            {
                sourcesite = "";

                Stock_Transfer_Header_Model sthm = new Stock_Transfer_Header_Model
                {
                    Doc_Number = doc_number,
                    STR_Number = txtSTRNumber.Text,
                    //Qty = qty,
                    Site_From = sourcesite,
                    Site_To = ddlDestinationSite.SelectedItem.Text,
                    timestamp = DateTime.Now
                };
                int idSTHeader = Stock_Transfer_Header.Save(oCon, sthm);

                if (idSTHeader != 0)
                {
                    int iditem = 0;
                    int destSite = Convert.ToInt32(ddlDestinationSite.SelectedValue.ToString());
                    bool check = Item_Master.CheckIfExist(oCon, txtSearchItemNumber.Text, destSite);
                    if (check == true)
                    {
                        iditem = Item_Master.RetrieveIdItem(oCon, txtSearchItemNumber.Text, destSite);
                    }
                    else if (check == false)
                    {
                        int oldIdItem = Item_Master.RetrieveIdItem(oCon, txtSearchItemNumber.Text, 2);
                        var imm = Item_Master.RetrieveData(oCon, "", "", "", oldIdItem);
                        var isc = Item_Subclass.RetrieveData(oCon, imm.Rows[0][1].ToString(), "", imm.Rows[0][2].ToString());

                        if (destSite == 9)
                        {
                            Item_Master_Model item_Master_Model = new Item_Master_Model
                            {
                                idClass = Convert.ToInt32(imm.Rows[0][1].ToString()),
                                idSubClass = 12,
                                ItemNumber = "JBW-" + imm.Rows[0][4].ToString(),
                                Principal_SKU = imm.Rows[0][4].ToString(),
                                Description = imm.Rows[0][5].ToString(),
                                Site = destSite.ToString(),
                                Unit_Weight = 0,
                                Weight_UM = "",
                                UM = "UT",
                                OnHand_Qty = 0,
                                Alloc_Qty = 0,
                                Total_Qty = 0,
                                Ave_Cost = 0,
                                Total_Cost = 0
                            };
                            iditem = Item_Master.Save(oCon, item_Master_Model);
                        }
                        else
                        {
                            Item_Master_Model item_Master_Model = new Item_Master_Model
                            {
                                idClass = Convert.ToInt32(imm.Rows[0][1].ToString()),
                                idSubClass = Convert.ToInt32(imm.Rows[0][2].ToString()),
                                ItemNumber = isc[0].Subclass_Name.Substring(0, 4) + "-" + imm.Rows[0][4].ToString(),
                                Principal_SKU = imm.Rows[0][4].ToString(),
                                Description = imm.Rows[0][5].ToString(),
                                Site = destSite.ToString(),
                                Unit_Weight = 0,
                                Weight_UM = "",
                                UM = "UT",
                                OnHand_Qty = 0,
                                Alloc_Qty = 0,
                                Total_Qty = 0,
                                Ave_Cost = 0,
                                Total_Cost = 0
                            };
                            iditem = Item_Master.Save(oCon, item_Master_Model);
                        }
                    }

                    Stock_Transfer_Detail_Model stdm = new Stock_Transfer_Detail_Model
                    {
                        idItem = iditem,
                        Qty = 1,
                        idSTHeader = idSTHeader
                    };
                    int idstdetail = Stock_Transfer_Detail.Save(oCon, stdm);

                    Stock_Transfer_Serial_Model stsm = new Stock_Transfer_Serial_Model
                    {
                        idSerial = Item_Serial.RetrieveIdSerial(oCon, txtSearchSerialNumber.Text),
                        idSTDetail = idstdetail
                    };
                    Stock_Transfer_Serial.Save(oCon, stsm);

                    Item_Serial.UpdateStockStatus(oCon, txtSearchSerialNumber.Text.Trim(), "N");

                    Trans_History_Model trans_History_Model = new Trans_History_Model
                    {
                        Trans_Code = transcode,
                        Item_Number = txtSearchItemNumber.Text,
                        Site = ddlDestinationSite.SelectedItem.Text,
                        UM = "UT",
                        Doc_No = doc_number,
                        Serial_No = txtSearchSerialNumber.Text.Trim(),
                        Reason_Code = "",
                        Trans_Date = DateTime.Now,
                        Order_No = ordernumber,
                        Invoice_No = txtSTRNumber.Text,
                        Reference_No = txtSTRNumber.Text,
                        Trans_Qty = -1,
                        Trans_Amt = Trans_History.GetSerialCost(oCon, txtSearchItemNumber.Text, txtSearchSerialNumber.Text),
                        Remarks = "",
                        user_domain = Session["User_Domain"].ToString()
                    };
                    Trans_History.Save(oCon, trans_History_Model);

                    result = true;
                }
            }
            else if (dvstockcode.Visible == false && dvuploadexcel.Visible == true)
            {
                sourcesite = ddlSourceSite.SelectedItem.Text;
                int sourceSite = Convert.ToInt32(ddlSourceSite.SelectedValue.ToString());
                int destSite = Convert.ToInt32(ddlDestinationSite.SelectedValue.ToString());

                DataTable dtUpload = new DataTable();
                DataTable dtCheck = new DataTable();
                StreamReader SR = new StreamReader(fuTransfer.PostedFile.InputStream);
                string line;
                DataRow rowUpload;
                DataRow rowCheck;
                dtUpload.Columns.Add(new DataColumn());
                dtCheck.Columns.Add(new DataColumn());
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
                            //line = line.ToString().Replace(",", "_");
                            //line = line.ToString().Replace("'", "");
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
                int invalidCode = 0;
                for (var x = 0; x <= dtCheck.Rows.Count - 1; x++)
                {
                    int counter = 0;
                    for (var y = 0; y <= dtUpload.Rows.Count - 1; y++)
                    {
                        if (dtCheck.Rows[x][1].ToString() == dtUpload.Rows[y][1].ToString())
                        {
                            counter = counter + 1;
                            if (counter > 1)
                            {
                                duplicateCount = duplicateCount + 1;
                            }
                        }
                    }

                    if (Item_Serial.CheckDuplicate(oCon, dtCheck.Rows[x][1].ToString(), 0, dtCheck.Rows[x][0].ToString(), sourceSite) == 0)
                    {
                        notInDatabaseCount = notInDatabaseCount + 1;
                    }

                    string serialLastCode = Trans_History.GetItemLastState(oCon, dtCheck.Rows[x][0].ToString(), dtCheck.Rows[x][1].ToString());
                    if (serialLastCode != ddlSourceSite.SelectedItem.Text)
                    {
                        invalidCode = invalidCode + 1;
                    }
                }

                if (invalidCode == 0)
                {
                    if (duplicateCount == 0)
                    {
                        if (notInDatabaseCount == 0)
                        {

                            Stock_Transfer_Header_Model sthm = new Stock_Transfer_Header_Model
                            {
                                Doc_Number = doc_number,
                                STR_Number = txtSTRNumber.Text,
                                //Qty = qty,
                                Site_From = sourcesite,
                                Site_To = ddlDestinationSite.SelectedItem.Text,
                                timestamp = DateTime.Now
                            };
                            int idSTHeader = Stock_Transfer_Header.Save(oCon, sthm);

                            if (idSTHeader != 0)
                            {
                                DataSet dsUpload = new DataSet();

                                for (var x = 0; x <= dtUpload.Rows.Count - 1; x++)
                                {
                                    if (x == 0 || dtUpload.Rows[x][0].ToString() != dtUpload.Rows[x - 1][0].ToString())
                                    {
                                        dsUpload.Tables.Add(dtUpload.Rows[x][0].ToString());
                                        dsUpload.Tables[dtUpload.Rows[x][0].ToString()].Columns.Add(new DataColumn());
                                        dsUpload.Tables[dtUpload.Rows[x][0].ToString()].Columns.Add(new DataColumn());
                                        DataRow dr = dsUpload.Tables[dtUpload.Rows[x][0].ToString()].NewRow();
                                        dr[0] = dtUpload.Rows[x][0];
                                        dr[1] = dtUpload.Rows[x][1];
                                        dsUpload.Tables[dtUpload.Rows[x][0].ToString()].Rows.Add(dr);
                                    }
                                    else if (dtUpload.Rows[x][0].ToString() == dtUpload.Rows[x - 1][0].ToString())
                                    {
                                        DataRow dr = dsUpload.Tables[dtUpload.Rows[x][0].ToString()].NewRow();
                                        dr[0] = dtUpload.Rows[x][0];
                                        dr[1] = dtUpload.Rows[x][1];
                                        dsUpload.Tables[dtUpload.Rows[x][0].ToString()].Rows.Add(dr);
                                    }
                                }

                                for (int x = 0; x <= dsUpload.Tables.Count - 1; x++)
                                {
                                    int iditem = 0;
                                    bool check = Item_Master.CheckIfExist(oCon, dsUpload.Tables[x].Rows[0][0].ToString(), destSite);
                                    if (check == true)
                                    {
                                        iditem = Item_Master.RetrieveIdItem(oCon, dsUpload.Tables[x].Rows[0][0].ToString(), destSite);
                                    }
                                    else if (check == false)
                                    {
                                        int oldIdItem = Item_Master.RetrieveIdItem(oCon, txtSearchItemNumber.Text, 2);
                                        var imm = Item_Master.RetrieveData(oCon, "", "", "", oldIdItem);
                                        var isc = Item_Subclass.RetrieveData(oCon, imm.Rows[0][1].ToString(), "", imm.Rows[0][2].ToString());
                                        if (destSite == 9)
                                        {
                                            Item_Master_Model item_Master_Model = new Item_Master_Model
                                            {
                                                idClass = Convert.ToInt32(imm.Rows[0][1].ToString()),
                                                idSubClass = 12,
                                                ItemNumber = "JBW-" + imm.Rows[0][4].ToString(),
                                                Principal_SKU = imm.Rows[0][4].ToString(),
                                                Description = imm.Rows[0][5].ToString(),
                                                Site = destSite.ToString(),
                                                Unit_Weight = 0,
                                                Weight_UM = "",
                                                UM = "UT",
                                                OnHand_Qty = 0,
                                                Alloc_Qty = 0,
                                                Total_Qty = 0,
                                                Ave_Cost = 0,
                                                Total_Cost = 0
                                            };
                                            iditem = Item_Master.Save(oCon, item_Master_Model);
                                        }
                                        else
                                        {
                                            Item_Master_Model item_Master_Model = new Item_Master_Model
                                            {
                                                idClass = Convert.ToInt32(imm.Rows[0][1].ToString()),
                                                idSubClass = Convert.ToInt32(imm.Rows[0][2].ToString()),
                                                ItemNumber = isc[0].Subclass_Name.Substring(0, 4) + "-" + imm.Rows[0][4].ToString(),
                                                Principal_SKU = imm.Rows[0][4].ToString(),
                                                Description = imm.Rows[0][5].ToString(),
                                                Site = destSite.ToString(),
                                                Unit_Weight = 0,
                                                Weight_UM = "",
                                                UM = "UT",
                                                OnHand_Qty = 0,
                                                Alloc_Qty = 0,
                                                Total_Qty = 0,
                                                Ave_Cost = 0,
                                                Total_Cost = 0
                                            };
                                            iditem = Item_Master.Save(oCon, item_Master_Model);
                                        }
                                    }

                                    Stock_Transfer_Detail_Model stdm = new Stock_Transfer_Detail_Model
                                    {
                                        idItem = iditem,
                                        Qty = dsUpload.Tables[x].Rows.Count,
                                        // = Item_Serial.RetrieveidItem(oCon, dtUpload.Rows[x][1].ToString()),
                                        idSTHeader = idSTHeader
                                    };
                                    int idstdetail = Stock_Transfer_Detail.Save(oCon, stdm);

                                    for (int y = 0; y <= dsUpload.Tables[x].Rows.Count - 1; y++)
                                    {
                                        Stock_Transfer_Serial_Model stsm = new Stock_Transfer_Serial_Model
                                        {
                                            idSerial = Item_Serial.RetrieveIdSerial(oCon, dsUpload.Tables[x].Rows[y][1].ToString()),
                                            idSTDetail = idstdetail
                                        };
                                        Stock_Transfer_Serial.Save(oCon, stsm);

                                        int transqty = 0;

                                        if (transcode == "STO")
                                        {
                                            Item_Serial.UpdateStockStatus(oCon, dsUpload.Tables[x].Rows[y][1].ToString(), "N");
                                            transqty = -1;
                                        }
                                        else
                                        {
                                            Item_Serial.UpdateStockStatus(oCon, dsUpload.Tables[x].Rows[y][1].ToString(), "Y");
                                            transqty = 1;
                                        }

                                        Item_Serial.UpdateIdItem(oCon, dsUpload.Tables[x].Rows[y][1].ToString(), iditem);

                                        Trans_History_Model trans_History_Model = new Trans_History_Model
                                        {
                                            Trans_Code = transcode,
                                            Item_Number = dsUpload.Tables[x].Rows[y][0].ToString(),
                                            Site = ddlDestinationSite.SelectedItem.Text,
                                            UM = "PC",
                                            Doc_No = doc_number,
                                            Serial_No = dsUpload.Tables[x].Rows[y][1].ToString(),
                                            Reason_Code = "",
                                            Trans_Date = DateTime.Now,
                                            Order_No = ordernumber,
                                            Invoice_No = txtSTRNumber.Text,
                                            Reference_No = txtSTRNumber.Text,
                                            Trans_Qty = transqty,
                                            Trans_Amt = Trans_History.GetSerialCost(oCon, txtSearchItemNumber.Text, txtSearchSerialNumber.Text),
                                            Remarks = "",
                                            user_domain = Session["User_Domain"].ToString()
                                        };
                                        Trans_History.Save(oCon, trans_History_Model);

                                    }
                                }

                                result = true;
                            }
                        }
                        else
                        {
                            result = false;

                            //lblVerify.Text = "There are " + notInDatabaseCount.ToString() + " serial numbers that are not included in the specified item number.";
                            HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "There are " + notInDatabaseCount.ToString() + " serial numbers that is not in the database." + ControlChars.Quote + ");</script>");
                        }
                    }
                    else
                    {
                        result = false;
                        //lblVerify.Text = "Duplicate serial numbers detected in file : " + duplicateCount.ToString();
                        HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Duplicate serial numbers detected in file : " + duplicateCount.ToString() + ControlChars.Quote + ");</script>");
                    }
                }
                else
                {
                    result = false;
                    //lblVerify.Text = "Duplicate serial numbers detected in file : " + duplicateCount.ToString();
                    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Incorrect serial numbers' last location in file : " + duplicateCount.ToString() + ControlChars.Quote + ");</script>");
                }

            }

            if (result == true)
            {
                Response.Write(@"<script>window.open ('/Forms/DownloadReports/DownloadStockTransfer.aspx?DocumentNumber=" + doc_number +
                                "&STRNumber=" + txtSTRNumber.Text +
                                "&SourceSite=" + sourcesite +
                                "&DestinationSite=" + ddlDestinationSite.SelectedItem.Text +
                                "','_blank');</script>");

                dvStockTransfer.Visible = false;
                btnOpenModal.Visible = false;
                Item_Master.InventoryCheckForError(oCon);
                getHistory(txtSearchItemNumber.Text, txtSearchSerialNumber.Text);
            }
        }

        protected void btnCloseModal_Click(object sender, EventArgs e)
        {
            dvStockTransfer.Visible = false;
        }

        protected void btnOpenModal_Click(object sender, EventArgs e)
        {
            string serialLastCode = Trans_History.GetItemLastState(oCon, txtSearchItemNumber.Text, txtSearchSerialNumber.Text);

            if (serialLastCode == "SLE")
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "The item is already sold!" + ControlChars.Quote + ");</script>");
            }
            else if (serialLastCode == "PUR" || serialLastCode == "STI")
            {
                PopulateSite();
                lblStockCode.Text = serialLastCode;
                dvStockTransfer.Visible = true;
                dvstockcode.Visible = true;
                dvuploadexcel.Visible = false;
            }
            else if (serialLastCode == "STO")
            {
                PopulateSite();
                lblStockCode.Text = serialLastCode;
                dvStockTransfer.Visible = true;
                dvstockcode.Visible = true;
                dvuploadexcel.Visible = false;
            }
        }

        public void PopulateSite()
        {
            var ds = Site_Loc.RetrieveData(oCon, "");
            ddlDestinationSite.DataSource = ds;
            ddlDestinationSite.DataValueField = "idSite";
            ddlDestinationSite.DataTextField = "Site_Name";
            ddlDestinationSite.DataBind();
            ddlDestinationSite.Items.Insert(0, new ListItem("---"));

            ddlSourceSite.DataSource = ds;
            ddlSourceSite.DataValueField = "idSite";
            ddlSourceSite.DataTextField = "Site_Name";
            ddlSourceSite.DataBind();
            ddlSourceSite.Items.Insert(0, new ListItem("---"));
        }

        protected void btnOpenModal2_Click(object sender, EventArgs e)
        {
            dvstockcode.Visible = false;
            dvuploadexcel.Visible = true;
            PopulateSite();
            dvStockTransfer.Visible = true;
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