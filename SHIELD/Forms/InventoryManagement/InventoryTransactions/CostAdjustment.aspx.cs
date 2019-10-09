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
    public partial class CostAdjustment : System.Web.UI.Page
    {
        SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);          
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

        #region "Cost Adjustment"
        public void setItemProducts(string itemNumber)
        {
            var idItem = Item_Master.RetrieveData(oCon, "", "", itemNumber, 0);

            Session["idItem"] = idItem.Rows[0][0].ToString();

            DataTable ldata = Item_Serial.GetDetailsForCostAdjustment(oCon, itemNumber);


            if (ldata.Rows[0][0].ToString() == "")
            {
                txtOnHandQty.Text = "0";
            }
            else
            {
                txtOnHandQty.Text = ldata.Rows[0][0].ToString();
            }

            if (ldata.Rows[0][1].ToString() == "")
            {
                txtCostPerUnit.Text = "0";
            }
            else
            {
                txtCostPerUnit.Text = ldata.Rows[0][1].ToString();
            }

            if (ldata.Rows[0][0].ToString() == "")
            {
                txtCostAdjQty.Text = "0";
            }
            else
            {
                txtCostAdjQty.Text = ldata.Rows[0][0].ToString();
            }
            txtOnHandCost.Text = (Convert.ToDecimal(txtOnHandQty.Text) * Convert.ToDecimal(txtCostPerUnit.Text)).ToString();
        }

        protected void btnSearchItem_Click(object sender, EventArgs e)
        {
            lblTransactionDate.Text = DateTime.Now.ToString();
            setItemProducts(txtItemSearch.Text.Trim());
        }

        protected void btnSearchPO_Click(object sender, EventArgs e)
        {
            lblTransactionDate.Text = DateTime.Now.ToString();
            setItemProducts(txtItemSearch.Text.Trim());
        }

        protected void btnSaveCostAdj_Click(object sender, EventArgs e)
        {
            string CostAdjNum = Cost_Adjustment.GetLastCostAdjNum(oCon);
            if (CostAdjNum == null)
            {
                CostAdjNum = "CF000000";
            }
            CostAdjNum = CostAdjNum.Substring(2);
            int CostAdjNum_ = int.Parse(CostAdjNum) + 1;

            string CostAdjNumber = "CF" + CostAdjNum_.ToString("000000");

            Cost_Adjustment_Model Cost_Adjustment_Model = new Cost_Adjustment_Model
            {
                CostAdjustNumber = CostAdjNumber,
                idItem = Convert.ToInt32(Session["idItem"].ToString()),
                InitialCost = Convert.ToDecimal(txtCostPerUnit.Text),
                InitialQuantity = Convert.ToInt32(txtOnHandQty.Text),
                AdjustedCostPerUnit = Convert.ToDecimal(txtCostAdjPerUnit.Text),
                AdjustedQuantity = Convert.ToInt32(txtCostAdjQty.Text),
                AdjustedAmount = Convert.ToDecimal(txtCostAdjAmt.Text),
                DocumentNumber = txtDocNumber.Text,
                Transaction_Date = DateTime.Now,
                Remarks = txtCARemarks.Text,
                user_id_chg_by = Session["User_Domain"].ToString()
            };
            bool save = Cost_Adjustment.Save(oCon, Cost_Adjustment_Model);

            if (save == true)
            {
                bool updateCost = Item_Serial.UpdateCost(oCon, Convert.ToInt32(Session["idItem"].ToString()), Convert.ToDecimal(txtCostAdjPerUnit.Text));
                if (updateCost == true)
                {
                    Item_Master.RecomputeItemCost(oCon, Convert.ToInt32(Session["idItem"].ToString()));

                    Trans_History_Model trans_History_Model = new Trans_History_Model
                    {
                        Trans_Code = "COF",
                        Item_Number = txtItemSearch.Text,
                        Site = "WH-JMS",
                        UM = "UT",
                        Doc_No = txtDocNumber.Text,
                        Serial_No = "",
                        Reason_Code = "",
                        Trans_Date = DateTime.Now,
                        Order_No = "",
                        Invoice_No = "",
                        Reference_No = "",
                        Trans_Qty = Convert.ToInt32(txtCostAdjQty.Text),
                        Trans_Amt = Convert.ToDecimal(txtCostAdjAmt.Text),
                        Remarks = txtCARemarks.Text,
                        user_domain = Session["User_Domain"].ToString()
                    };
                    Trans_History.Save(oCon, trans_History_Model);

                    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Saving Complete! Cost Adjustment # : " + CostAdjNumber + ControlChars.Quote + ");</script>");

                    ClearAdj();
                }
                else
                {
                    HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Error in updating cost" + ControlChars.Quote + ");</script>");
                }
            }
            else
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Error in saving cost adjustment" + ControlChars.Quote + ");</script>");
            }
        }

        protected void btnComputeAdj_Click(object sender, EventArgs e)
        {
            if (txtOnHandQty.Text != "0" || txtOnHandQty.Text != "")
            {
                txtCostAdjPerUnit.Text = ((Convert.ToDecimal(txtCostAdjAmt.Text) + Convert.ToDecimal(txtOnHandCost.Text)) / Convert.ToDecimal(txtCostAdjQty.Text)).ToString();

                btnSaveCostAdj.Visible = true;
                btnComputeAdj.Visible = false;
            }
            else
            {
                HttpContext.Current.Response.Write("<script>alert(" + ControlChars.Quote + "Quantity can't be zero" + ControlChars.Quote + ");</script>");
            }
        }

        private void ClearAdj()
        {
            lblCostAdj.Visible = false;
            lblCostAdj_.Visible = false;
            lblTransactionDate.Visible = false;
            lblTransactionDate_.Visible = false;
            btnSaveCostAdj.Visible = false;
            btnComputeAdj.Visible = true;
            txtCostAdjAmt.Text = "";
            txtCostAdjPerUnit.Text = "";
            txtCostAdjQty.Text = "";
            txtCostPerUnit.Text = "";
            txtDocNumber.Text = "";
            txtItemSearch.Text = "";
            txtOnHandCost.Text = "";
            txtOnHandQty.Text = "";
            txtCARemarks.Text = "";
            txtSearchCA.Text = "";
        }

        protected void btnSearchCA_Click(object sender, EventArgs e)
        {
            var ldata = Cost_Adjustment.RetreiveData(oCon, txtSearchCA.Text);

            txtCostAdjAmt.Text = ldata[0].AdjustedAmount.ToString();
            txtCostAdjPerUnit.Text = ldata[0].AdjustedCostPerUnit.ToString();
            txtCostAdjQty.Text = ldata[0].AdjustedQuantity.ToString();
            txtCostPerUnit.Text = ldata[0].InitialCost.ToString();
            txtDocNumber.Text = ldata[0].DocumentNumber.ToString();
            txtOnHandCost.Text = ldata[0].InitialCost.ToString();
            txtOnHandQty.Text = ldata[0].InitialQuantity.ToString();
            txtCARemarks.Text = ldata[0].Remarks.ToString();

            var idItem = Item_Master.RetrieveData(oCon, "", "", "", ldata[0].idItem);
            txtItemSearch.Text = idItem.Rows[0][3].ToString();
            lblTransactionDate.Visible = true;
            lblTransactionDate_.Visible = true;
            lblTransactionDate.Text = ldata[0].Transaction_Date.ToString();
            lblCostAdj.Visible = true;
            lblCostAdj_.Visible = true;
            lblCostAdj.Text = ldata[0].CostAdjustNumber.ToString();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearAdj();
        }
        protected void btnCloseMenu_Click(object sender, EventArgs e)
        {
            dvMenu.Visible = false;
        }

        protected void btnMenu_Click(object sender, EventArgs e)
        {
            dvMenu.Visible = true;
        }
        #endregion
    }
}