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
using ClosedXML.Excel;
using Microsoft.Reporting.WebForms;
using POSOINV.Functions;
using POSOINV.Models;

namespace SHIELD.Inventory
{
    public partial class Inventory : System.Web.UI.Page
    {
        SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        //private int editRow = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnSaveNewSerial);
            //scriptManager.RegisterPostBackControl(this.gvSupplier);
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            //paneProd.Value = Request.Form[paneProd.UniqueID];
            //paneSub.Value = Request.Form[paneSub.UniqueID];
            //paneItem.Value = Request.Form[paneItem.UniqueID];
            //paneSerial.Value = Request.Form[paneSerial.UniqueID];

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
                    PopulateDept();
                    setUserAccess(access);
                }
                else
                {
                    Response.Redirect("~/Login.aspx");
                }
            }
        }

        private void setUserAccess(string user_access)
        {
            if (user_access == "AE")
            {
                dvSerial.Visible = false;
                btnShowAddNewProduct.Visible = false;
                btnAddNewSubProduct.Visible = false;
                btnAddNewProductItem.Visible = false;
                btnAddNewSerial.Visible = false;
                //btnDownloadInventorySerialReport.Visible = false;
                btnDownloadInventoryAging.Visible = false;
            }
            else if (user_access == "PM")
            {
                dvSerial.Visible = false;
                btnShowAddNewProduct.Visible = false;
                btnAddNewSubProduct.Visible = true;
                btnAddNewProductItem.Visible = true;
                btnAddNewSerial.Visible = false;
                //btnDownloadInventorySerialReport.Visible = false;
                btnDownloadInventoryAging.Visible = true;
            }
            else if (user_access == "AU")
            {
                dvSerial.Visible = true;
                btnShowAddNewProduct.Visible = true;
                btnAddNewSubProduct.Visible = true;
                btnAddNewProductItem.Visible = true;
                btnAddNewSerial.Visible = true;
                //btnDownloadInventorySerialReport.Visible = true;
                btnDownloadInventoryAging.Visible = true;
            }
            else if (user_access == "OP")
            {
                dvSerial.Visible = true;
                btnShowAddNewProduct.Visible = false;
                btnAddNewSubProduct.Visible = false;
                btnAddNewProductItem.Visible = false;
                btnAddNewSerial.Visible = true;
                //btnDownloadInventorySerialReport.Visible = true;
                btnDownloadInventoryAging.Visible = true;
            }
            else if (user_access == "IT")
            {
                dvSerial.Visible = true;
                btnShowAddNewProduct.Visible = true;
                btnAddNewSubProduct.Visible = true;
                btnAddNewProductItem.Visible = true;
                btnAddNewSerial.Visible = true;
                //btnDownloadInventorySerialReport.Visible = true;
                btnDownloadInventoryAging.Visible = true;
            }
            else if (user_access == "PR")
            {
                dvSerial.Visible = false;
                btnShowAddNewProduct.Visible = true;
                btnAddNewSubProduct.Visible = true;
                btnAddNewProductItem.Visible = true;
                btnAddNewSerial.Visible = false;
                //btnDownloadInventorySerialReport.Visible = false;
                btnDownloadInventoryAging.Visible = false;
            }
            else if (user_access == "BCC")
            {
                Response.Redirect("~/LandingPage.aspx");
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

        public void PopulateDept()
        {
            var ds = Site_Loc.RetrieveData(oCon, "");
            ddlSite.DataSource = ds;
            ddlSite.DataValueField = "idSite";
            ddlSite.DataTextField = "Site_Desc";
            ddlSite.DataBind();
            ddlSite.Items.Insert(0, new ListItem("---"));

            var dss = UM.RetrieveData(oCon);
            ddlUM.DataSource = dss;
            ddlUM.DataValueField = "UM";
            ddlUM.DataTextField = "UM_Description";
            ddlUM.DataBind();
            ddlUM.Items.Insert(0, new ListItem("---"));
        }

        #region "products"
        private void GetProducts()
        {
            DataTable ds = new DataTable();
            if (Session["User_Access"].ToString() == "AE" || Session["User_Access"].ToString() == "PM")
            {
                ds = ToDataTable(Item_Class.RetrieveData(oCon, txtProductSearch.Text.Trim(), Session["User_Domain"].ToString()));
            }
            else
            {
                ds = ToDataTable(Item_Class.RetrieveData(oCon, txtProductSearch.Text.Trim(), ""));
            }
            grvProducts.DataSource = ds;
            grvProducts.DataBind();
        }

        protected void grvProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow rowProduct in grvProducts.Rows)
            {
                if (rowProduct.RowIndex == grvProducts.SelectedIndex)
                {
                    rowProduct.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                }
                else
                {
                    rowProduct.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                }
            }

            GridViewRow row = grvProducts.Rows[grvProducts.SelectedIndex];
            Session["idClass"] = (row.FindControl("lblidClass") as Label).Text.Trim();
            try
            {
                Session["ProductName"] = (row.FindControl("lblProduct") as Label).Text.Trim();
            }
            catch
            {
                Session["ProductName"] = (row.FindControl("txtProduct") as TextBox).Text.Trim();
            }
            GetSubProducts();
            //tblSubProducts.Visible = true;
            //tblProductItems.Visible = false;
            //tblSerialNo.Visible = false;

            lblClick.Text = Session["ProductName"].ToString();

            Session["idSubclass"] = null;
            Session["idItem"] = null;
            Session["SubClassName"] = null;
            Session["Item_Name"] = null;
            Session["Item_Description"] = null;
            grvProductItem.DataSource = null;
            grvProductItem.DataBind();
            grvSerial.DataSource = null;
            grvSerial.DataBind();
        }

        protected void grvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;
                if (Session["User_Access"].ToString() == "AE")
                    ((LinkButton)e.Row.Cells[2].Controls[0]).Visible = false;
                else
                    ((LinkButton)e.Row.Cells[2].Controls[0]).Visible = true;
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grvProducts, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click To Select this row";
            }
        }

        protected void grvProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvProducts.PageIndex = e.NewPageIndex;
            GetProducts();
        }

        protected void btnSearchProduct_Click(object sender, EventArgs e)
        {
            GetProducts();
        }

        protected void btnShowAddNewProduct_Click(object sender, EventArgs e)
        {
            dvAddNewProduct.Visible = true;
        }

        protected void btnSaveNewProduct_Click(object sender, EventArgs e)
        {
            if (txtProductName.Text.Trim() != "")
            {
                Item_Class_Model products = new Item_Class_Model
                {
                    Product_Name = txtProductName.Text
                };
                Item_Class.Save(oCon, products);
                dvAddNewProduct.Visible = false;
                GetProducts();
                clearFields();
            }
            else
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Please enter a product name" + ControlChars.Quote + ");</script>");
            }
        }

        protected void btnCancelSaveProduct_Click(object sender, EventArgs e)
        {
            dvAddNewProduct.Visible = false;
        }

        protected void grvProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grvProducts.EditIndex = e.NewEditIndex;
            GetProducts();
        }

        protected void grvProducts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grvProducts.EditIndex = -1;
            GetProducts();
        }

        protected void grvProducts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = grvProducts.Rows[e.RowIndex];
            Item_Class_Model updtMdl = new Item_Class_Model
            {
                idClass = Convert.ToInt32((row.FindControl("lblidClass") as Label).Text),
                Product_Name = (row.FindControl("txtProduct") as TextBox).Text
            };
            Item_Class.Update(oCon, updtMdl);
            grvProducts.EditIndex = -1;
            GetProducts();
        }

        #endregion

        #region "sub_products"
        private void GetSubProducts()
        {
            try
            {
                DataTable ds = ToDataTable(Item_Subclass.RetrieveData(oCon, Session["idClass"].ToString(), txtSubProductSearch.Text.Trim(), ""));
                grvSubProducts.DataSource = ds;
                grvSubProducts.DataBind();
            }
            catch
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Select a product first!" + ControlChars.Quote + ");</script>");
            }
        }

        protected void grvSubProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow rowSub in grvSubProducts.Rows)
            {
                if (rowSub.RowIndex == grvSubProducts.SelectedIndex)
                {
                    rowSub.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                }
                else
                {
                    rowSub.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                }
            }

            GridViewRow row = grvSubProducts.Rows[grvSubProducts.SelectedIndex];
            Session["idSubclass"] = (row.FindControl("lblidSubClass") as Label).Text.Trim();
            try
            {
                Session["SubClassName"] = (row.FindControl("lblSubclassName") as Label).Text.Trim();
            }
            catch
            {
                Session["SubClassName"] = (row.FindControl("txtSubclassName") as TextBox).Text.Trim();
            }
            GetProductItem();
            //tblProductItems.Visible = true;
            //tblSerialNo.Visible = false;

            if (Session["SubClassName"].ToString() == null)
                lblClick.Text = Session["ProductName"].ToString();
            else
                lblClick.Text = Session["ProductName"].ToString() + " > " + Session["SubClassName"].ToString();

            Session["idItem"] = null;
            Session["Item_Name"] = null;
            Session["Item_Description"] = null;
            grvSerial.DataSource = null;
            grvSerial.DataBind();
        }

        protected void grvSubProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;
                if (Session["User_Access"].ToString() == "AE")
                    ((LinkButton)e.Row.Cells[2].Controls[0]).Visible = false;
                else
                    ((LinkButton)e.Row.Cells[2].Controls[0]).Visible = true;
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grvSubProducts, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click To Select this row";
            }
        }

        protected void grvSubProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvSubProducts.PageIndex = e.NewPageIndex;
            GetSubProducts();
        }

        protected void btnSearchSubProduct_Click(object sender, EventArgs e)
        {
            GetSubProducts();
        }

        protected void btnAddNewSubProduct_Click(object sender, EventArgs e)
        {
            try
            {
                lblProductLink.Text = Session["ProductName"].ToString();
                dvAddNewSubProduct.Visible = true;
            }
            catch
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Select a product first!" + ControlChars.Quote + ");</script>");
            }
        }

        protected void btnSaveNewSubProduct_Click(object sender, EventArgs e)
        {
            if (txtSubProductName.Text.Trim() != "")
            {
                Item_Subclass_Model subproducts = new Item_Subclass_Model
                {
                    idClass = Convert.ToInt32(Session["idClass"].ToString()),
                    Subclass_Name = txtSubProductName.Text
                };
                Item_Subclass.Save(oCon, subproducts);
                dvAddNewSubProduct.Visible = false;
                GetSubProducts();
                clearFields();
            }
            else
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Please enter a sub-product name" + ControlChars.Quote + ");</script>");
            }
        }

        protected void btnCancelSaveSubProduct_Click(object sender, EventArgs e)
        {
            dvAddNewSubProduct.Visible = false;
        }

        protected void grvSubProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grvSubProducts.EditIndex = e.NewEditIndex;
            GetSubProducts();
        }

        protected void grvSubProducts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grvSubProducts.EditIndex = -1;
            GetSubProducts();
        }

        protected void grvSubProducts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = grvSubProducts.Rows[e.RowIndex];
            Item_Subclass_Model updtMdl = new Item_Subclass_Model
            {
                idClass = Convert.ToInt32(Session["idClass"].ToString()),
                idSubclass = Convert.ToInt32(Session["idSubclass"].ToString()),
                Subclass_Name = (row.FindControl("txtSubclassName") as TextBox).Text
            };
            Item_Subclass.Update(oCon, updtMdl);
            grvSubProducts.EditIndex = -1;
            GetSubProducts();
        }
        #endregion

        #region "ProductItem"
        private void GetProductItem()
        {
            if (Session["idSubclass"] == null)
                Session["idSubclass"] = "";
            DataTable ds = Item_Master.RetrieveData(oCon, Session["idClass"].ToString(), Session["idSubclass"].ToString(), txtProductItemSearch.Text.Trim(), 0);
            grvProductItem.DataSource = ds;
            grvProductItem.DataBind();
        }

        protected void grvProductItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grvProductItem.Rows)
            {
                if (row.RowIndex == grvProductItem.SelectedIndex)
                {
                    row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                }
            }

            Session["ItemIndex"] = grvProductItem.SelectedIndex.ToString();
            var x = Session["ItemIndex"].ToString();
            Session["idItem"] = grvProductItem.Rows[grvProductItem.SelectedIndex].Cells[0].Text.ToString();
            Session["Item_Name"] = grvProductItem.Rows[grvProductItem.SelectedIndex].Cells[3].Text.ToString();
            Session["Item_Description"] = grvProductItem.Rows[grvProductItem.SelectedIndex].Cells[5].Text.ToString();
            Session["Site_Name"] = grvProductItem.Rows[grvProductItem.SelectedIndex].Cells[6].Text.ToString();
            GetSerial();
            PopulateDept();
            //tblSerialNo.Visible = true;
            if (Session["SubClassName"].ToString() == null)
                lblClick.Text = Session["ProductName"].ToString() + " > " + Session["Item_Name"].ToString();
            else
                lblClick.Text = Session["ProductName"].ToString() + " > " + Session["SubClassName"].ToString() + " > " + Session["Item_Name"].ToString();

        }

        protected void grvProductItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;//idItem
                e.Row.Cells[1].Visible = false; //idClass
                e.Row.Cells[2].Visible = false; //idSubClass
                //item_Number
                //Principal_SKU
                //Description
                //e.Row.Cells[6].Visible = false; //Site
                e.Row.Cells[7].Visible = false; //Unit_Weight
                e.Row.Cells[8].Visible = false; //Weight_UM
                                                //e.Row.Cells[9].Visible = false; //UM
                                                //e.Row.Cells[10].Visible = false; //OnHand_Qty
                                                //e.Row.Cells[11].Visible = false; //Alloc_Qty
                                                //e.Row.Cells[12].Visible = false; //Total_Qty
                                                //e.Row.Cells[13].Visible = false; //Ave_Cost
                                                //e.Row.Cells[14].Visible = false; //Total_Cost

                e.Row.Cells[13].Text = string.Format("{0:N2}", e.Row.Cells[13].Text);
                e.Row.Cells[14].Text = string.Format("{0:N2}", e.Row.Cells[14].Text);
                if (Session["User_Access"].ToString() == "AE")
                {
                    e.Row.Cells[13].Visible = false;
                    e.Row.Cells[14].Visible = false;
                    //e.Row.Cells[15].Visible = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Text = "Item Number";
                e.Row.Cells[4].Text = "Principal SKU";
                e.Row.Cells[5].Text = "Description";
                e.Row.Cells[6].Text = "Site";
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Text = "UM";
                e.Row.Cells[10].Text = "On Hand Qty";
                e.Row.Cells[11].Text = "Alloc Qty";
                e.Row.Cells[12].Text = "Total Qty";
                //e.Row.Cells[13].Text = "Ave Cost";
                //e.Row.Cells[14].Text = "Total Cost";
                if (Session["User_Access"].ToString() == "AE")
                {
                    e.Row.Cells[13].Visible = false;
                    e.Row.Cells[14].Visible = false;
                    //e.Row.Cells[15].Visible = false;
                }
                else
                {
                    e.Row.Cells[13].Text = "Ave Cost";
                    e.Row.Cells[14].Text = "Total Cost";
                    //e.Row.Cells[15].Text = "Age";
                }
                e.Row.Cells[15].Text = "Age";
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["User_Access"].ToString() == "AE")
                {

                }
                else
                {
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grvProductItem, "Select$" + e.Row.RowIndex);
                    e.Row.ToolTip = "Click To Select this row";
                }
            }
        }

        protected void grvProductItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvProductItem.PageIndex = e.NewPageIndex;
            GetProductItem();
        }

        protected void btnSearchItem_Click(object sender, EventArgs e)
        {
            try
            {
                GetProductItem();
            }
            catch
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Select Product / Sub - product first!" + ControlChars.Quote + ");</script>");
            }
        }

        protected void btnAddNewItem_Click(object sender, EventArgs e)
        {
            if (txtItemNumber.Text.Trim() != "")
            {
                if (txtPrincipal_SKU.Text.Trim() != "")
                {
                    if (txtDescription.Text.Trim() != "")
                    {
                        bool checkDuplicate = Item_Master.checkItemNumber(oCon, txtItemNumber.Text.Trim(), txtDescription.Text.Trim());
                        if (checkDuplicate == false)
                        {
                            int idsubclass = 0;
                            if (Session["idSubClass"].ToString() != "")
                            {
                                idsubclass = Convert.ToInt32(Session["idSubClass"]);
                            }
                            else
                            {
                                idsubclass = 0;
                            }
                            Item_Master_Model itemMaster = new Item_Master_Model
                            {
                                idClass = Convert.ToInt32(Session["idClass"]),
                                idSubClass = idsubclass,
                                ItemNumber = txtItemNumber.Text.Trim(),
                                Principal_SKU = txtPrincipal_SKU.Text.Trim(),
                                Description = txtDescription.Text.Trim(),
                                Site = ddlSite.SelectedValue.ToString(),
                                Unit_Weight = 0,
                                Weight_UM = "",
                                UM = ddlUM.SelectedValue.ToString(),
                                OnHand_Qty = 0,
                                Alloc_Qty = 0,
                                Total_Qty = 0,
                                Ave_Cost = 0,
                                Total_Cost = 0
                            };
                            Item_Master.Save(oCon, itemMaster);
                            dvAddNewItem.Visible = false;
                            GetProductItem();
                            clearFields();
                        }
                        else
                        {
                            HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Item exists in the database!" + ControlChars.Quote + ");</script>");
                        }
                    }
                    else
                    {
                        HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Please enter a description" + ControlChars.Quote + ");</script>");
                    }
                }
                else
                {
                    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Please enter a principal SKU" + ControlChars.Quote + ");</script>");
                }
            }
            else
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Please enter an item number" + ControlChars.Quote + ");</script>");
            }
        }

        protected void btnCancelItem_Click(object sender, EventArgs e)
        {
            dvAddNewItem.Visible = false;
        }

        protected void btnAddNewProductItem_Click(object sender, EventArgs e)
        {
            try
            {
                lblSubProductLink.Text = Session["ProductName"].ToString() + " > " + Session["SubClassName"].ToString();
                PopulateDept();
                //Session["saveMode"] = "New";
                txtItemNumber.Text = "";
                txtPrincipal_SKU.Text = "";
                txtDescription.Text = "";
                ddlSite.SelectedIndex = 0;
                dvAddNewItem.Visible = true;
            }
            catch
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Select a product / sub - product first!" + ControlChars.Quote + ");</script>");
            }
            //txtUnit_Weight.Text = "";
            //txtWeight_UM.Text = "";
            //txtUnit_Volume.Text = "";
            //txtVolume_UM.Text = "";
        }

        //protected void btnViewDetails_Click(object sender, EventArgs e)
        //{
        //    GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        //    int x = gvRow.RowIndex;
        //    dvAddNewItem.Visible = true;
        //    //int x = Convert.ToInt32(e.CommandArgument);
        //    Session["idItem"] = grvProductItem.Rows[x].Cells[1].Text.ToString();
        //    Session["Item_Name"] = grvProductItem.Rows[x].Cells[4].Text.ToString();
        //    txtItemNumber.Text = grvProductItem.Rows[x].Cells[4].Text.ToString();
        //    txtPrincipal_SKU.Text = grvProductItem.Rows[x].Cells[5].Text.ToString();
        //    txtDescription.Text = grvProductItem.Rows[x].Cells[6].Text.ToString();
        //    ddlSite.SelectedIndex = Convert.ToInt32(grvProductItem.Rows[x].Cells[7].Text.ToString());
        //    txtUnit_Weight.Text = grvProductItem.Rows[x].Cells[8].Text.ToString();
        //    txtWeight_UM.Text = grvProductItem.Rows[x].Cells[9].Text.ToString();
        //    txtUnit_Volume.Text = grvProductItem.Rows[x].Cells[10].Text.ToString();
        //    txtVolume_UM.Text = grvProductItem.Rows[x].Cells[11].Text.ToString();
        //    //txtOnHandQty.Text = grvProductItem.Rows[x].Cells[12].Text.ToString();
        //    //txtAllocQty.Text = grvProductItem.Rows[x].Cells[13].Text.ToString();
        //    //txtTotalQty.Text = grvProductItem.Rows[x].Cells[14].Text.ToString();
        //    Session["onHandQty"] = grvProductItem.Rows[x].Cells[12].Text.ToString();
        //    Session["allocQty"] = grvProductItem.Rows[x].Cells[13].Text.ToString();
        //    Session["totalQty"] = grvProductItem.Rows[x].Cells[14].Text.ToString();
        //    Session["saveMode"] = "Update";
        //}
        //protected void grvProductItem_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //dvAddNewItem.Visible = true;
        //int x = Convert.ToInt32(e.CommandArgument);
        //txtItemNumber.Text = grvProductItem.Rows[x].Cells[4].Text.ToString();
        //txtPrincipal_SKU.Text = grvProductItem.Rows[x].Cells[5].Text.ToString();
        //txtDescription.Text = grvProductItem.Rows[x].Cells[6].Text.ToString();
        //ddlSite.SelectedIndex = Convert.ToInt32(grvProductItem.Rows[x].Cells[7].Text.ToString());
        //txtUnit_Weight.Text = grvProductItem.Rows[x].Cells[8].Text.ToString();
        //txtWeight_UM.Text = grvProductItem.Rows[x].Cells[9].Text.ToString();
        //txtUnit_Volume.Text = grvProductItem.Rows[x].Cells[10].Text.ToString();
        //txtVolume_UM.Text = grvProductItem.Rows[x].Cells[11].Text.ToString();
        //Session["onHandQty"] = grvProductItem.Rows[x].Cells[12].Text.ToString();
        //Session["allocQty"] = grvProductItem.Rows[x].Cells[13].Text.ToString();
        //Session["totalQty"] = grvProductItem.Rows[x].Cells[14].Text.ToString();
        //Session["saveMode"] = "Update";
        //}
        #endregion

        #region "Serial"
        private void GetSerial()
        {
            DataTable ds = Item_Serial.RetrieveData(oCon, Session["idItem"].ToString(), txtPOSearch.Text.Trim(), txtSerialSearch.Text.Trim());
            grvSerial.DataSource = ds;
            grvSerial.DataBind();
        }
        protected void grvSerial_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow rowSerial in grvSerial.Rows)
            {
                if (rowSerial.RowIndex == grvSerial.SelectedIndex)
                {
                    rowSerial.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                }
                else
                {
                    rowSerial.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                }
            }

            GridViewRow row = grvSerial.Rows[grvSubProducts.SelectedIndex];
            Session["idSerial"] = (row.FindControl("lblidSerial") as Label).Text.Trim();
            string x = Session["idSerial"].ToString();
        }

        protected void grvSerial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[4].Text = string.Format("{0:#,##0}", e.Row.Cells[4].Text);
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
            }

            //if (editRow >= 0 & e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    if (((e.Row.RowState & DataControlRowState.Edit) > 0))
            //        e.Row.Cells[4].Visible = true;
            //    else
            //        e.Row.Cells[4].Visible = false;
            //}

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grvSerial, "Select$" + e.Row.RowIndex);
            //    e.Row.ToolTip = "Click To Select this row";
            //}
        }

        protected void grvSerial_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvSerial.PageIndex = e.NewPageIndex;
            GetSerial();
        }

        protected void btnPOSerial_Click(object sender, EventArgs e)
        {
            try
            {
                GetSerial();
            }
            catch
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Select an item number first!" + ControlChars.Quote + ");</script>");
            }
        }

        protected void btnSearchSerial_Click(object sender, EventArgs e)
        {
            try
            {
                GetSerial();
            }
            catch
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Select an item number first!" + ControlChars.Quote + ");</script>");
            }
        }

        protected void btnAddNewSerial_Click(object sender, EventArgs e)
        {
            try
            {
                lblItemLink.Text = Session["ProductName"].ToString() + " > " + Session["SubClassName"].ToString() + " > " + Session["Item_Name"].ToString() + " - " + Session["Item_Description"].ToString();
                //setInitalRow();
                dvAddNewSerial.Visible = true;
            }
            catch
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Select an item number first!" + ControlChars.Quote + ");</script>");
            }
        }

        protected void btnSaveNewSerial_Click(object sender, EventArgs e)
        {
            //bool hasPO = Convert.ToBoolean(Session["hasPO"].ToString());
            //if (hasPO)
            //{
            //    List<Item_Tax_Model> tax = Item_Tax.RetrieveData(oCon);
            //    string currentTax = tax[0].Tax.ToString();

            //    PO_Detail_Model detail_Model = new PO_Detail_Model
            //    {
            //        idPOHeader = Convert.ToInt32(Session["idPOHeader"].ToString()),
            //        idItem = Convert.ToInt32(Session["idItem"].ToString()),
            //        Quantity = Convert.ToDecimal(txtItemQuantity.Text),
            //        Price = Convert.ToDecimal(txtFinalItemCost.Text),                    
            //        Tax = (Convert.ToDecimal(txtFinalItemCost.Text) * Convert.ToDecimal(txtItemQuantity.Text)) * Convert.ToDecimal(currentTax),
            //        Amount = Convert.ToDecimal(txtFinalItemCost.Text) * Convert.ToDecimal(txtItemQuantity.Text),
            //        Final_Cost = Convert.ToDecimal(txtFinalItemCost.Text)
            //    };
            //    PO_Detail.Save(oCon, detail_Model);
            //}
            //else
            //{
            //    PO_Header_Model header_Model = new PO_Header_Model
            //    {
            //        //header_Model. idPOHeader = Convert.ToInt32(Session["idPOHeader"].ToString());
            //        PO_Number = txtPONumberUpload.Text.Trim(),
            //        PO_Date = DateTime.Now,
            //        Delivery_Date = DateTime.Now,
            //        Terms = "",
            //        FOB_Point = "",
            //        Shipping_Via = "",
            //        Currency = ddlCurrency.SelectedValue,
            //        idSupplier = 0,
            //        PO_Total = Convert.ToDecimal(txtPOAmount.Text),
            //        PO_Quantity = Convert.ToInt32(txtPOQuantity.Text),
            //        Total_Charges = Convert.ToDecimal(txtTotalAddCharges.Text),
            //        Forex_Rate = Convert.ToDecimal(txtForexRate.Text),
            //        PR_Number = "",
            //        Created_By = "",
            //        Remarks = ""
            //    };

            //    Session["idPOHeader"] = PO_Header.Save(oCon, header_Model);

            //    List<Item_Tax_Model> tax = Item_Tax.RetrieveData(oCon);
            //    string currentTax = tax[0].Tax.ToString();

            //    PO_Detail_Model detail_Model = new PO_Detail_Model
            //    {
            //        idPOHeader = Convert.ToInt32(Session["idPOHeader"].ToString()),
            //        idItem = Convert.ToInt32(Session["idItem"].ToString()),
            //        Quantity = Convert.ToDecimal(txtItemQuantity.Text),
            //        Price = Convert.ToDecimal(txtFinalItemCost.Text),                   
            //        Tax = Convert.ToDecimal(currentTax),
            //        Amount = Convert.ToDecimal(txtFinalItemCost.Text) * Convert.ToDecimal(txtItemQuantity.Text),
            //        Final_Cost = Convert.ToDecimal(txtFinalItemCost.Text)
            //    };
            //    PO_Detail.Save(oCon, detail_Model);
            //}

            string PRNumber = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("d2") + DateTime.Now.Day.ToString("d2") + Purchase_Reciept.getPRnumber(oCon);
            if (ddlSerialMode.SelectedValue == "1")
            {
                if (txtSerial.Text.Trim() != "")
                {
                    int qty = 0;
                    string isRecieved = "";
                    int partial = 0;
                    int actual = 0;
                    int remaining = 0;

                    if (cbxPartial.Checked)
                    {
                        qty = decimal.ToInt32(Convert.ToDecimal(txtPartialQuantity.Text));
                        partial = decimal.ToInt32(Convert.ToDecimal(txtPartialQuantity.Text));
                        actual = decimal.ToInt32(Convert.ToDecimal(txtItemQuantity.Text));
                        remaining = actual - partial;
                    }
                    else
                    {
                        qty = decimal.ToInt32(Convert.ToDecimal(txtItemQuantity.Text));
                    }


                    if (remaining == 0)
                        isRecieved = "Yes";
                    else
                        isRecieved = "Partial";

                    int lotserialcount = Item_Serial.LotSerialCount(oCon, txtSerial.Text);

                    for (int x = 1; x <= qty; x++)
                    {
                        Item_Serial_Model serialmdl = new Item_Serial_Model
                        {
                            idItem = Convert.ToInt32(Session["idItem"].ToString()),
                            PO_Number = txtPONumberUpload.Text.Trim(),
                            Serial_No = txtSerial.Text.Trim() + "-" + (lotserialcount + x).ToString(),
                            timestamp = DateTime.Now,
                            Unit_Cost = Convert.ToDecimal(txtFinalItemCost.Text),
                            Unit_Comp = Session["Unit_Comp"].ToString(),
                            user_change = Session["User_Domain"].ToString(),
                            InStock = "Y"
                        };
                        Item_Serial.Save(oCon, serialmdl);

                        Trans_History_Model trans_History_Model = new Trans_History_Model
                        {
                            Trans_Code = "PUR",
                            Item_Number = Session["Item_Name"].ToString(),
                            Site = "WH-JMS",
                            UM = "UT",
                            Doc_No = PRNumber,
                            Serial_No = txtSerial.Text.Trim() + "-" + x.ToString(),
                            Reason_Code = "",
                            Trans_Date = DateTime.Now,
                            Order_No = txtPONumberUpload.Text.Trim(),
                            Invoice_No = txtDocNo.Text.Trim(),
                            Reference_No = txtDocNo.Text.Trim(),
                            Trans_Qty = 1,
                            Trans_Amt = Convert.ToDecimal(txtFinalItemCost.Text),
                            Remarks = "",
                            user_domain = Session["User_Domain"].ToString()
                        };
                        Trans_History.Save(oCon, trans_History_Model);
                    }
                    Item_Master.RecomputeItemCost(oCon, Convert.ToInt32(Session["idItem"].ToString()));

                    var poheader = new PO_Header_Model
                    {
                        idPOHeader = Convert.ToInt32(Session["idPOHeader"].ToString()),
                        POStatus = "Recieved"
                    };
                    PO_Header.UpdatePOStatus(oCon, poheader);

                    var podetail = new PO_Detail_Model
                    {
                        idPODetail = Convert.ToInt32(Session["idPODetail"].ToString()),
                        isReceived = isRecieved,
                        Partial_Remaining = remaining
                    };
                    PO_Detail.UpdateReceivedStatus(oCon, podetail);

                    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Saving Complete!" + ControlChars.Quote + ");</script>");

                    GetSerial();
                    GetProductItem();
                    PrintReceipt(PRNumber);
                }
                else
                {
                    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Input a lot serial!" + ControlChars.Quote + ");</script>");
                }
            }
            else if (ddlSerialMode.SelectedValue == "2")
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
                            line = line.ToString().Replace(",", "");
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

                    if (Item_Serial.CheckDuplicate(oCon, dtCheck.Rows[x][0].ToString(), Convert.ToInt32(Session["idItem"].ToString())) >= 1)
                        duplicateCount = duplicateCount + 1;
                }

                int qty = 0;
                string isRecieved = "";
                int partial = 0;
                int actual = 0;
                int remaining = 0;
                if (cbxPartial.Checked)
                {
                    qty = decimal.ToInt32(Convert.ToDecimal(txtPartialQuantity.Text));
                    partial = decimal.ToInt32(Convert.ToDecimal(txtPartialQuantity.Text));
                    actual = decimal.ToInt32(Convert.ToDecimal(txtItemQuantity.Text));
                    remaining = actual - partial;
                }
                else
                {
                    qty = decimal.ToInt32(Convert.ToDecimal(txtItemQuantity.Text));
                }


                if (remaining == 0)
                    isRecieved = "Yes";
                else
                    isRecieved = "Partial";

                if (duplicateCount == 0)
                {
                    if (dtUpload.Rows.Count == qty)
                    {
                        for (var x = 0; x <= dtUpload.Rows.Count - 1; x++)
                        {
                            Item_Serial_Model serMdl = new Item_Serial_Model
                            {
                                idItem = Convert.ToInt32(Session["idItem"].ToString()),
                                Serial_No = dtUpload.Rows[x][0].ToString(),
                                PO_Number = txtPONumberUpload.Text.Trim(),
                                timestamp = DateTime.Now,
                                Unit_Cost = Convert.ToDecimal(txtFinalItemCost.Text),
                                Unit_Comp = Session["Unit_Comp"].ToString(),
                                user_change = Session["User_Domain"].ToString(),
                                InStock = "Y"
                            };
                            Item_Serial.Save(oCon, serMdl);

                            Trans_History_Model trans_History_Model = new Trans_History_Model
                            {
                                Trans_Code = "PUR",
                                Item_Number = Session["Item_Name"].ToString(),
                                Site = "WH-JMS",
                                UM = "UT",
                                Doc_No = PRNumber,
                                Serial_No = dtUpload.Rows[x][0].ToString(),
                                Reason_Code = "",
                                Trans_Date = DateTime.Now,
                                Order_No = txtPONumberUpload.Text.Trim(),
                                Invoice_No = txtDocNo.Text.Trim(),
                                Reference_No = txtDocNo.Text.Trim(),
                                Trans_Qty = 1,
                                Trans_Amt = Convert.ToDecimal(txtFinalItemCost.Text),
                                Remarks = "",
                                user_domain = Session["User_Domain"].ToString()
                            };
                            Trans_History.Save(oCon, trans_History_Model);
                        }

                        var poheader = new PO_Header_Model
                        {
                            idPOHeader = Convert.ToInt32(Session["idPOHeader"].ToString()),
                            POStatus = "Recieved"
                        };
                        PO_Header.UpdatePOStatus(oCon, poheader);

                        var podetail = new PO_Detail_Model
                        {
                            idPODetail = Convert.ToInt32(Session["idPODetail"].ToString()),
                            isReceived = isRecieved,
                            Partial_Remaining = remaining
                        };
                        PO_Detail.UpdateReceivedStatus(oCon, podetail);

                        HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "File Upload Complete!" + ControlChars.Quote + ");</script>");

                        GetSerial();
                        GetProductItem();
                        PrintReceipt(PRNumber);
                    }
                    else
                    {
                        HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Quantity doesn't match!" + ControlChars.Quote + ");</script>");
                    }
                }
                else
                {
                    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Duplicate Serial Numbers Detected" + ControlChars.Quote + ");</script>");
                }
            }
            cbxPartial.Checked = false;
            txtPartialQuantity.Text = "";
        }

        protected void btnCancelNewSerial_Click(object sender, EventArgs e)
        {
            dvAddNewSerial.Visible = false;
            cbxPartial.Checked = false;
            txtPartialQuantity.Text = "";
        }

        protected void grvSerial_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = grvSerial.Rows[e.RowIndex];
            Item_Serial_Model updtMdl = new Item_Serial_Model
            {
                idItem = Convert.ToInt32(Session["idItem"].ToString()),
                idSerial = Convert.ToInt32((row.FindControl("lblidSerial") as Label).Text),
                PO_Number = (row.FindControl("txtPONumber") as TextBox).Text.Trim(),
                Serial_No = (row.FindControl("txtSerial_No") as TextBox).Text
            };
            Item_Serial.Update(oCon, updtMdl);
            grvSerial.EditIndex = -1;
            GetSerial();
        }

        protected void ddlSerialMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSerialMode.SelectedValue == "1")
            {
                txtSerial.Visible = true; uploadExcel.Visible = false;
            }
            else if (ddlSerialMode.SelectedValue == "2")
            {
                txtSerial.Visible = false; uploadExcel.Visible = true;
            }
            else
            {
                txtSerial.Visible = false; uploadExcel.Visible = false;
            }
        }

        protected void cbxPartial_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxPartial.Checked)
            {
                txtPartialQuantity.ReadOnly = false;
            }
            else
            {
                txtPartialQuantity.ReadOnly = true;
                txtPartialQuantity.Text = "";
            }
        }

        protected void btnSearchPO_Click(object sender, EventArgs e)
        {
            DataTable dl = Currency_Code.RetrieveData(oCon);
            ddlCurrency.DataSource = dl;
            ddlCurrency.DataTextField = "Currency_Name";
            ddlCurrency.DataValueField = "Currency_Code";
            ddlCurrency.DataBind();

            string x = PO_Header.CheckDuplicate(oCon, txtPONumberUpload.Text);
            if (x != "")
            {
                try
                {
                    string idpoheader = x;
                    string iditem = Session["idItem"].ToString();
                    DataTable pomodel = PO_Detail.RetrieveData(oCon, idpoheader, iditem);
                    string isReceived = pomodel.Rows[0][11].ToString();
                    if (isReceived == "No" || isReceived == "Partial")
                    {
                        Session["idPODetail"] = pomodel.Rows[0][0].ToString();
                        txtItemCost.Text = pomodel.Rows[0][6].ToString();
                        if (isReceived == "No")
                        {
                            txtItemQuantity.Text = pomodel.Rows[0][5].ToString();
                        }
                        else if (isReceived == "Partial")
                        {
                            txtItemQuantity.Text = pomodel.Rows[0][12].ToString();
                        }

                        txtFinalItemCost.Text = pomodel.Rows[0][9].ToString();
                        Session["Unit_Comp"] = pomodel.Rows[0][10].ToString();

                        Session["hasPO"] = true;
                        List<PO_Header_Model> models = PO_Header.RetrieveData(oCon, txtPONumberUpload.Text);
                        Session["idPOHeader"] = models[0].idPOHeader.ToString();
                        ddlCurrency.SelectedValue = models[0].Currency.ToString();
                        txtForexRate.Text = models[0].Forex_Rate.ToString();
                        txtTotalAddCharges.Text = models[0].Total_Charges.ToString();
                        txtPOAmount.Text = models[0].PO_Total.ToString();
                        txtPOQuantity.Text = models[0].PO_Quantity.ToString();
                    }
                    else
                    {
                        HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Item is already received!" + ControlChars.Quote + ");</script>");
                    }

                }
                catch
                {
                    Session["hasPO"] = false;
                    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Item is not included in the PO!" + ControlChars.Quote + ");</script>");
                }
            }
            else
            {
                Session["hasPO"] = false;
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "PO doesn't exist!" + ControlChars.Quote + ");</script>");
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Session["hasPO"] = false;
            Session["idPOHeader"] = "";
            //setInitalRow();
            clearFields();
        }

        public bool PrintReceipt(string PRNumber)
        {
            bool resultValue = false;

            ClientScript.RegisterStartupScript(this.GetType(), "alert", @"window.location.href='/Forms/DownloadReports/DownloadPurchaseReceipt.aspx?ponumber=" + txtPONumberUpload.Text +
                "&idItem=" + Session["idItem"].ToString() +
                "&docno=" + txtDocNo.Text +
                "&sitename=" + "WH-JMS" +
                "&PRNumber=" + PRNumber +
                "';", true);

            dvAddNewSerial.Visible = false;
            clearFields();
            return resultValue;

            //var lData = Purchase_Reciept.GetPurchaseReciept(oCon, txtPONumberUpload.Text.Trim(), Convert.ToInt32(Session["idItem"].ToString()));

            //Microsoft.Reporting.WebForms.ReportViewer viewer = new ReportViewer();
            //viewer.ProcessingMode = ProcessingMode.Local;
            //viewer.LocalReport.ReportPath = Server.MapPath(@"~\Resources\PurchaseReceipt.rdlc");

            //ReportParameter p1 = new ReportParameter("PONumber", txtPONumberUpload.Text);
            //ReportParameter p2 = new ReportParameter("DocNumber", txtDocNo.Text);
            //ReportParameter p3 = new ReportParameter("Site", Session["Site_Name"].ToString());
            //ReportParameter p4 = new ReportParameter("TransDate", DateTime.Now.ToShortDateString());
            //ReportParameter p5 = new ReportParameter("SupplierName", "GN AUDIO SINGAPORE PTE LTD");
            //ReportParameter p6 = new ReportParameter("Address1", "150 Beach Road,");
            //ReportParameter p7 = new ReportParameter("Address2", "#15-05/06, Gateway West");
            //ReportParameter p8 = new ReportParameter("Address3", "Singapore");
            //ReportParameter p9 = new ReportParameter("Address4", "189720");
            ////ReportParameter p5 = new ReportParameter("SupplierName", Session["SupplierName"].ToString());
            ////ReportParameter p6 = new ReportParameter("Address1", Session["Address1"].ToString());
            ////ReportParameter p7 = new ReportParameter("Address2", Session["Address2"].ToString());
            ////ReportParameter p8 = new ReportParameter("Address3", Session["Address3"].ToString());
            ////ReportParameter p9 = new ReportParameter("Address4", Session["Address4"].ToString());
            //ReportParameter p10 = new ReportParameter("PRNumber", PRNumber);

            //ReportDataSource repDataSource = new ReportDataSource("DataSet2", lData);

            //viewer.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 });

            //viewer.LocalReport.DataSources.Clear();
            //viewer.LocalReport.DataSources.Add(repDataSource);

            //Warning[] warnings;
            //string[] streamIds;
            //string mimeType = string.Empty;
            //string encoding = string.Empty;
            //string extension = "pdf";

            //try
            //{
            //    byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            //    Response.Buffer = true;
            //    Response.Clear();
            //    Response.ContentType = mimeType;
            //    Response.AddHeader("content-disposition", "attachment; filename= " + "PR_" + PRNumber + "." + extension);
            //    Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
            //    Response.Flush(); // send it to the client to download  
            //    Response.End();
            //}
            //catch
            //{
            //    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Download not successful!" + ControlChars.Quote + ");</script>");
            //}
            //finally
            //{                
            //}
        }

        private void clearFields()
        {
            txtDescription.Text = "";
            txtDocNo.Text = "";
            txtFinalItemCost.Text = "";
            txtForexRate.Text = "0";
            txtItemCost.Text = "";
            txtItemNumber.Text = "";
            txtItemQuantity.Text = "";
            txtPOAmount.Text = "";
            txtPONumberUpload.Text = "";
            txtPOQuantity.Text = "";
            txtPrincipal_SKU.Text = "";
            txtProductItemSearch.Text = "";
            txtProductName.Text = "";
            txtProductSearch.Text = "";
            //txtSearchSupplier.Text = "";
            txtSerial.Text = "";
            txtPOSearch.Text = "";
            txtSubProductName.Text = "";
            txtSubProductSearch.Text = "";
            //txtSupplier.Text = "";
            txtTotalAddCharges.Text = "";
        }
        #endregion

        //protected void lbtnDelete_Click(object sender, EventArgs e)
        //{
        //    var lbtnDelete = (LinkButton)sender;
        //    var row = (GridViewRow)lbtnDelete.Parent.Parent.Parent.Parent.Parent;
        //    var rowIndex = row.RowIndex;
        //    List<clsLineItem> ldata = new List<clsLineItem>();

        //    foreach (GridViewRow gvr in grvAdditionalCharges.Rows)
        //    {
        //        clsLineItem obj = new clsLineItem();
        //        var Name = (DropDownList)gvr.FindControl("ddlAdditionalCharge");
        //        var Charges = (TextBox)gvr.FindControl("txtCharges");

        //        obj.Name = Name.Text.Trim();
        //        obj.Charges = Charges.Text.Trim();
        //        ldata.Add(obj);
        //    }

        //    ldata.RemoveAt(rowIndex);

        //    int i = 0;
        //    DataTable dt = new DataTable();
        //    while (i < ldata.Count())
        //    {
        //        DataRow dr = dt.NewRow();
        //        dt.Rows.Add(dr);
        //        i = i + 1;
        //    }

        //    grvAdditionalCharges.DataSource = dt;
        //    grvAdditionalCharges.DataBind();

        //    int j = 0;
        //    try
        //    {
        //        while (j <= ldata.Count())
        //        {
        //            var Name = (DropDownList)grvAdditionalCharges.Rows[j].FindControl("ddlAdditionalCharge");
        //            var Charges = (TextBox)grvAdditionalCharges.Rows[j].FindControl("txtCharges");

        //            Name.Text = ldata[j].Name;
        //            Charges.Text = ldata[j].Charges;
        //            j += 1;
        //        }
        //    }
        //    catch
        //    {

        //    }

        //    if (grvAdditionalCharges.Rows.Count == 0)
        //        setInitalRow();
        //}

        //protected void lbtnAdd_Click1(object sender, EventArgs e)
        //{
        //    var ldata = new List<clsLineItem>();

        //    foreach (GridViewRow gvr in grvAdditionalCharges.Rows)
        //    {
        //        clsLineItem obj = new clsLineItem();
        //        var Name = (DropDownList)gvr.FindControl("ddlAdditionalCharge");
        //        var Charges = (TextBox)gvr.FindControl("txtCharges");

        //        obj.Name = Name.Text.Trim();
        //        obj.Charges = Charges.Text.Trim();
        //        ldata.Add(obj);
        //    }

        //    int i = 0;
        //    DataTable dt = new DataTable();
        //    while (i <= ldata.Count())
        //    {
        //        DataRow dr = dt.NewRow();
        //        dt.Rows.Add(dr);
        //        i = i + 1;
        //    }

        //    grvAdditionalCharges.DataSource = dt;
        //    grvAdditionalCharges.DataBind();

        //    int j = 0;

        //    while (j < ldata.Count())
        //    {
        //        var Name = (DropDownList)grvAdditionalCharges.Rows[j].FindControl("ddlAdditionalCharge");
        //        var Charges = (TextBox)grvAdditionalCharges.Rows[j].FindControl("txtCharges");

        //        Name.Text = ldata[j].Name;
        //        Charges.Text = ldata[j].Charges;
        //        j += 1;
        //    }

        //}

        //private void setInitalRow()
        //{
        //    DataTable dt = new DataTable();
        //    DataRow dr;
        //    dr = dt.NewRow();
        //    dt.Rows.Add(dr);
        //    grvAdditionalCharges.DataSource = dt;
        //    grvAdditionalCharges.DataBind();

        //    DataTable dl = Currency_Code.RetrieveData(oCon);
        //    ddlCurrency.DataSource = dl;
        //    ddlCurrency.DataTextField = "Currency_Name";
        //    ddlCurrency.DataValueField = "Currency_Code";
        //    ddlCurrency.DataBind();

        //    Session["hasPO"] = false;
        //    //int j = 0;
        //    //while (j < dt.Rows.Count)
        //    //{
        //    //    var LineNumber = (TextBox)grvAdditionalCharges.Rows[j].FindControl("txtLineNumber");
        //    //    LineNumber.Text = "";
        //    //    j += 1;
        //    //}
        //}

        //protected void grvAdditionalCharges_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            DropDownList ddl = (DropDownList)e.Row.FindControl("ddlAdditionalCharge");
        //            var ds = Additional_Charge.RetrieveData(oCon);
        //            ddl.DataSource = ds;
        //            ddl.DataTextField = "Additional_Charge";
        //            ddl.DataValueField = "idAddCharge";
        //            ddl.DataBind();
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}

        //protected void btnCompute_Click(object sender, EventArgs e)
        //{
        //    decimal dCharges = 0;
        //    decimal dFinalPOAmount = 0;
        //    decimal dPercentage = 0;
        //    decimal dComputedCharge = 0;
        //    decimal dItemCost = 0;
        //    decimal dFinalItemCost = 0;
        //    decimal dItemCostQuantity = 0;
        //    decimal dItemQuantity = Convert.ToDecimal(txtItemQuantity.Text);
        //    decimal dPOQuantity = Convert.ToDecimal(txtPOQuantity.Text);
        //    //decimal dPOAmount = Convert.ToDecimal(txtPOAmount.Text);

        //    //if (txtTotalAddCharges.Text == "")
        //    //{
        //    //    foreach (GridViewRow gvr in grvAdditionalCharges.Rows)
        //    //    {
        //    //        var Charges = (TextBox)gvr.FindControl("txtCharges");
        //    //        dCharges = dCharges + Convert.ToDecimal(Charges.Text);
        //    //    }
        //    //    txtTotalAddCharges.Text = dCharges.ToString("N4");
        //    //}
        //    //else
        //    //{
        //        dCharges = Convert.ToDecimal(txtTotalAddCharges.Text);
        //    //}

        //    if (txtForexRate.Text.Trim() != "" && txtForexRate.Text.Trim() != "0")
        //    {
        //        dFinalPOAmount = Convert.ToDecimal(txtForexRate.Text) * Convert.ToDecimal(txtPOAmount.Text);
        //        dItemCost = Convert.ToDecimal(txtForexRate.Text) * Convert.ToDecimal(txtItemCost.Text);
        //    }
        //    else
        //    {
        //        dFinalPOAmount = Convert.ToDecimal(txtPOAmount.Text);
        //        dItemCost = Convert.ToDecimal(txtItemCost.Text);
        //    }

        //    if (ddlPurchase.SelectedIndex == 1)
        //    {
        //        dPercentage = (dItemCost * dItemQuantity) / dFinalPOAmount;
        //        dComputedCharge = dPercentage * dCharges;
        //        dItemCostQuantity = dItemCost * dItemQuantity;
        //        dFinalItemCost = (dComputedCharge + dItemCostQuantity) / dItemQuantity;
        //    }
        //    else if (ddlPurchase.SelectedIndex == 2)
        //    {
        //        dComputedCharge = dCharges / dPOQuantity;
        //        dFinalItemCost = dComputedCharge + dItemCost;
        //    }

        //    if (ddlUnitPrice.SelectedIndex == 1)
        //    {
        //        txtFinalItemCost.Text = dFinalItemCost.ToString("N4");
        //    }
        //    else if (ddlUnitPrice.SelectedIndex == 2)
        //    {
        //        DataTable averageCost = Item_Serial.AverageCosting(oCon, Session["idItem"].ToString());
        //        decimal dTotalInventoryCost = 0;
        //        //decimal dAverageCost = 0;
        //        int inventoryStock = 0;

        //        for (int x = 0; x <= averageCost.Rows.Count - 1; x++)
        //        {
        //            dTotalInventoryCost = dTotalInventoryCost + Convert.ToDecimal(averageCost.Rows[x][3].ToString());
        //            inventoryStock = inventoryStock + Convert.ToInt32(averageCost.Rows[x][1].ToString());
        //        }
        //        dFinalItemCost = ((dFinalItemCost * dItemQuantity) + dTotalInventoryCost) / (dItemQuantity + inventoryStock);
        //        txtFinalItemCost.Text = string.Format("{0:n}", dFinalItemCost.ToString("N4"));
        //    }
        //}



        //protected void btnShowSupplierModal_Click(object sender, EventArgs e)
        //{
        //    dvSupplier.Visible = true;
        //    setSupplierGridView(txtSearchSupplier.Text);
        //}

        //protected void btnCancelSupplier_Click(object sender, EventArgs e)
        //{
        //    dvSupplier.Visible = false;
        //}

        //protected void btnSearchSupplier_Click(object sender, EventArgs e)
        //{
        //    setSupplierGridView(txtSearchSupplier.Text);
        //}

        //protected void gvSupplier_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var x = gvSupplier.SelectedIndex;
        //    txtSupplier.Text = gvSupplier.Rows[x].Cells[1].Text.ToString();
        //    Session["SupplierName"] = gvSupplier.Rows[x].Cells[1].Text.ToString();
        //    Session["Address1"] = gvSupplier.Rows[x].Cells[2].Text.ToString();
        //    Session["Address2"] = gvSupplier.Rows[x].Cells[3].Text.ToString();
        //    Session["Address3"] = gvSupplier.Rows[x].Cells[4].Text.ToString();
        //    Session["Address4"] = gvSupplier.Rows[x].Cells[5].Text.ToString();
        //    dvSupplier.Visible = false;
        //}

        //protected void gvSupplier_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    gvSupplier.PageIndex = e.NewPageIndex;
        //    setSupplierGridView(txtSearchSupplier.Text);
        //}

        //protected void gvSupplier_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.Header)
        //    {
        //        e.Row.Cells[0].Text = "Abbreviation";
        //        e.Row.Cells[1].Text = "Supplier Name";
        //        e.Row.Cells[2].Text = "Address 1";
        //        e.Row.Cells[3].Text = "Address 2";
        //        e.Row.Cells[4].Text = "Address 3";
        //        e.Row.Cells[5].Text = "Address 4";
        //    }
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvSupplier, "Select$" + e.Row.RowIndex);
        //        e.Row.ToolTip = "Click To Select this row";
        //    }
        //}

        //private void setSupplierGridView(string searchSupplier)
        //{
        //    DataTable dt = Supplier.RetrieveData(oCon, searchSupplier);
        //    gvSupplier.DataSource = dt;
        //    gvSupplier.DataBind();
        //}



        protected void btnDownloadInventoryAging_Click(object sender, EventArgs e)
        {
            DataTable dtReport = new DataTable();
            dtReport.Columns.Add("Item Number");
            dtReport.Columns.Add("Principal SKU");
            dtReport.Columns.Add("Description");
            dtReport.Columns.Add("On Hand Qty");
            dtReport.Columns.Add("Ave Cost");
            dtReport.Columns.Add("Age");

            DataTable dtSO = Item_Serial.InventoryAgingReport(oCon);

            for (int x = 0; x <= dtSO.Rows.Count - 1; x++)
            {
                if (dtSO.Rows[x][3].ToString() != "0")
                {
                    DataRow dr = dtReport.NewRow();
                    dr[0] = dtSO.Rows[x][0];
                    dr[1] = dtSO.Rows[x][1];
                    dr[2] = dtSO.Rows[x][2];
                    dr[3] = dtSO.Rows[x][3];
                    dr[4] = dtSO.Rows[x][4];
                    dr[5] = dtSO.Rows[x][5];
                    dtReport.Rows.Add(dr);
                }
            }

            //var emailTo = new List<string>();
            //emailTo.Add("Sherwin.Santome@lsi.ph");
            //var emailCC = new List<string>();
            //emailCC.Add("JohnlordJoseph.Villas@wsiphil.com.ph");
            //var emailBCC = new List<string>();

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dtReport, "Aging Report");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Inventory Aging Report " + DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Year.ToString() + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    //var email = new WSIExchange.WSIEmailExchange();
                    //email.SendExchangeEmailNotification("JohnlordJoseph.Villas@wsiphil.com.ph", "Inventory Aging Report", "Please see attached file. This is an automatic generated e-mail from the ERP website.", emailTo, emailCC, emailBCC, "Inventory Aging Report " + DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Year.ToString() + ".xlsx", MyMemoryStream.ToArray());

                    Response.Flush();
                    Response.End();
                }
            }
        }


    }
}