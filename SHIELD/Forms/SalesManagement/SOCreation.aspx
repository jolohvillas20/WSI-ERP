<%@ Page Title="ERP - SO Creation"
    Language="C#"
    Async="true"
    EnableSessionState="True"
    MasterPageFile="~/Site Master/SiteMaster.Master"
    AutoEventWireup="true"
    CodeBehind="SOCreation.aspx.cs"
    Inherits="SHIELD.ERP.SOCreation"
    EnableEventValidation="false"
    ValidateRequest="false"
    EnableViewState="True"
    MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/Site Master/SiteMaster.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <link rel="stylesheet" href="/Content/SOstyles/bootstrap.min.css" />
    <link rel="stylesheet" href="/Content/SOstyles/w3.css" />
    <link rel="stylesheet" href="/Content/SOstyles/bootstrap-datetimepicker.css" />
    <script src="/Content/SOstyles/jquery.min.js"></script>

    <link href="/Content/SOstyles/bootstrap-datepicker3.min.css" rel="stylesheet" />

    <script src="/Content/SOstyles/bootstrap-datepicker.js"></script>
    <script src="/Content/SOstyles/bootstrap-datepicker.min.js"></script>

    <script type="text/javascript" src="/Content/SOstyles/moment.min.js"></script>
    <script type="text/javascript" src="/Content/SOstyles/bootstrap-datetimepicker.js"></script>

    <style>
        .Pager a {
            color: #000000;
            text-decoration: none;
        }

        .Pager {
            color: #2196F3;
        }

        .smallwidth {
            width: 15px;
            min-width: 15px;
            max-width: 15px;
            padding-left: 0px !important;
        }

        .TextBoxStyle {
            border-radius: 0px;
            height: 35px;
        }

        body {
            padding-right: 0 !important
        }

        .ButtonStyle {
            border-radius: 0px;
            height: 35px;
        }

        .note-group-select-from-files {
            display: none;
        }

        .switch {
            position: relative;
            display: inline-block;
            width: 50px;
            height: 24px;
        }

            .switch input {
                display: none;
            }

        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 16px;
                width: 16px;
                left: 4px;
                bottom: 4px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #2196F3;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(26px);
            -ms-transform: translateX(26px);
            transform: translateX(26px);
        }

        /* Rounded sliders */
        .slider.round {
            border-radius: 24px;
        }

            .slider.round:before {
                border-radius: 50%;
            }

        .alert {
            padding: 20px;
            background-color: #f44336;
            color: white;
            opacity: 1;
            transition: opacity 0.6s;
            margin-bottom: 15px;
        }

            .alert.success {
                background-color: #4CAF50;
            }

            .alert.info {
                background-color: #2196F3;
            }

            .alert.warning {
                background-color: #ff9800;
            }


        .requiredColor {
            color: red;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlButtons" runat="server" Visible="true" CssClass="w3-panel w3-margin-0">
        <div class="w3-col s6">
            <asp:Button ID="btnBack" CssClass="w3-btn w3-green ButtonStyle" runat="server" Text="Back" OnClick="btnBack_Click" Visible="false" />
            <asp:Button ID="btnEdit" CssClass="w3-btn w3-green ButtonStyle" runat="server" Text="Edit" OnClick="btnEdit_Click" Visible="false" />
            <asp:Button ID="btnAddNew" CssClass="w3-btn w3-green ButtonStyle" runat="server" Text="Add New" OnClick="btnAddNew_Click" Visible="false" />
            <asp:Button ID="btnSave" CssClass="w3-btn w3-green ButtonStyle" runat="server" Text="Save" OnClick="btnSave_Click" Visible="false" />
            <asp:Button ID="btnDelete" CssClass="w3-btn w3-green ButtonStyle" runat="server" Text="Delete" OnClick="btnDelete_Click" Visible="false" />
            <asp:Button ID="btnPrintSO" CssClass="w3-btn w3-blue ButtonStyle" runat="server" Text="Download S.O." OnClick="btnPrintSO_Click" Visible="False" />
            <asp:Button ID="btnDownloadSalesReport" CssClass="w3-btn w3-blue ButtonStyle" runat="server" Text="Download Sales Report" OnClick="btnDownloadSalesReport_Click" Visible="False" />
            <asp:Button ID="btnShowItemSOModal" CssClass="w3-btn w3-blue ButtonStyle" runat="server" Text="Check SO By Item Number" OnClick="btnShowItemSOModal_Click" Visible="False" />
        </div>
        <div class="w3-animate-opacity w3-margin-0">
            <div class="w3-col s6 w3-padding-0 w3-right" style="vertical-align: central;">
                <asp:Table ID="tblSort" CellPadding="0" CellSpacing="0" runat="server" Width="49%" Style="display: inline-block">
                    <asp:TableRow>
                        <asp:TableCell Width="20%">
                            <label runat="server" style="font-size: small">SO Status : </label>
                        </asp:TableCell>
                        <asp:TableCell Width="80%">
                            <asp:DropDownList runat="server" ID="ddlSOStatus" OnSelectedIndexChanged="ddlSOStatus_SelectedIndexChanged" AutoPostBack="true" class="w3-input w3-border TextBoxStyle">
                                <asp:ListItem Value="" Text="All"></asp:ListItem>
                                <asp:ListItem Value="Open" Text="Open"></asp:ListItem>
                                <asp:ListItem Value="Closed" Text="Closed"></asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:Table ID="tblSearch" CellPadding="0" CellSpacing="0" runat="server" Width="49%" Style="border: 1px solid #2196F3; display: inline-block">
                    <asp:TableRow>
                        <asp:TableCell Width="90%">
                            <asp:TextBox ID="txtSearch" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="Enter S.O. Number"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell Width="10%">
                            <asp:LinkButton runat="server" Style="width: 100%;" ID="btnSearch" Text="Search" CssClass="w3-btn w3-green ButtonStyle w3-right" OnClick="btnSearch_Click"><i class="glyphicon glyphicon-search"></i></asp:LinkButton>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
        </div>
        <div class="w3-animate-opacity w3-margin-0">
            <div class="w3-col s3 w3-padding-left w3-padding-top w3-right" style="vertical-align: central;">
                <asp:Table ID="Table4" CellPadding="0" CellSpacing="0" runat="server" Width="100%">
                    <asp:TableRow>
                        <asp:TableCell Width="20%">
                            <asp:Label ID="lblSONumber_" runat="server" type="text" Font-Bold="true" Font-Size="Medium" ForeColor="gray" Text="SO # :" Visible="false"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell Width="80%">
                            <asp:Label ID="lblSONumber" runat="server" type="text" Font-Bold="true" Font-Size="Medium" ForeColor="lightseagreen" Visible="false"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel runat="server" ID="SOPanel" CssClass="w3-panel" Style="margin-top: 0px">
        <div class="w3-white w3-card-4" runat="server">
            <asp:GridView ID="grvMainSO" DataKeyNames="idSODetail" OnSelectedIndexChanged="grvMainSO_SelectedIndexChanged" OnRowDataBound="grvMainSO_RowDataBound" OnPageIndexChanging="grvMainSO_PageIndexChanging" runat="server" CssClass="w3-table w3-hoverable w3-round-small" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="10" Font-Size="14px" PagerStyle-BorderStyle="None" PagerStyle-Height="20px">
                <HeaderStyle CssClass="w3-blue" />
                <Columns>
                    <asp:BoundField DataField="idSOHeader" HeaderText="idSOHeader" HtmlEncode="False" />
                    <asp:BoundField DataField="idCustomer" HeaderText="idCustomer" HtmlEncode="False" />
                    <asp:BoundField DataField="SO_Number" HeaderText="S.O. Number" HtmlEncode="False" />
                    <asp:BoundField DataField="Order_Date" HeaderText="Order Date </br> (dd/mm/yyyy)" HtmlEncode="False" />
                    <asp:BoundField DataField="Due_Date" HeaderText="Due Date </br> (dd/mm/yyyy)" HtmlEncode="False" />
                    <asp:BoundField DataField="Salesman" HeaderText="Salesman" HtmlEncode="False" />
                    <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name" HtmlEncode="False" />
                    <asp:BoundField DataField="Customer_PO" HeaderText="Customer PO" HtmlEncode="False" />
                    <asp:BoundField DataField="Freight_Charges" HeaderText="Freight Charges" HtmlEncode="False" />
                    <asp:BoundField DataField="Other_Charges" HeaderText="Other Charges" HtmlEncode="False" />
                    <asp:BoundField DataField="idPOHeader" HeaderText="idPOHeader" HtmlEncode="False" />
                    <asp:BoundField DataField="Customer_Code" HeaderText="Customer_Code" HtmlEncode="False" />
                    <asp:BoundField DataField="credit_term" HeaderText="credit_term" HtmlEncode="False" />
                    <asp:BoundField DataField="Gross_Amount" HeaderText="Gross_Amount" HtmlEncode="False" />
                    <asp:BoundField DataField="Net_Amount" HeaderText="Net_Amount" HtmlEncode="False" />
                    <asp:BoundField DataField="Tax_Amount" HeaderText="Tax_Amount" HtmlEncode="False" />
                    <asp:BoundField DataField="idSite" HeaderText="idSite" HtmlEncode="False" />
                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" HtmlEncode="False" />
                    <asp:BoundField DataField="currency_code" HeaderText="currency_code" HtmlEncode="False" />
                    <asp:BoundField DataField="Final_Discount" HeaderText="Final_Discount" HtmlEncode="False" />
                    <asp:BoundField DataField="Address1" HeaderText="Address1" HtmlEncode="False" />
                    <asp:BoundField DataField="Address2" HeaderText="Address2" HtmlEncode="False" />
                    <asp:BoundField DataField="Address3" HeaderText="Address3" HtmlEncode="False" />
                    <asp:BoundField DataField="Address4" HeaderText="Address4" HtmlEncode="False" />
                    <asp:BoundField DataField="Special_Concession" HeaderText="Special_Concession" HtmlEncode="False" />
                    <asp:BoundField DataField="Company_Name" HeaderText="Company Name" HtmlEncode="False" />
                    <asp:BoundField DataField="SO_Status" HeaderText="Status" HtmlEncode="False" />
                    <asp:BoundField DataField="Stock_Status" HeaderText="Stock Status" HtmlEncode="False" />
                    <asp:BoundField DataField="AddressShipping1" HeaderText="AddressShipping1" HtmlEncode="False" />
                    <asp:BoundField DataField="AddressShipping2" HeaderText="AddressShipping2" HtmlEncode="False" />
                    <asp:BoundField DataField="AddressShipping3" HeaderText="AddressShipping3" HtmlEncode="False" />
                    <asp:BoundField DataField="AddressShipping4" HeaderText="AddressShipping4" HtmlEncode="False" />
                    <asp:BoundField DataField="DaysOpen" HeaderText="No. of <br> Days Open" HtmlEncode="False" />
                    <asp:BoundField DataField="CreatedBy" HeaderText="Created By" HtmlEncode="False" />
                    <asp:BoundField DataField="Pick_Status" HeaderText="Picked Status" HtmlEncode="False" />
                    <asp:BoundField DataField="End_User" HeaderText="End User" HtmlEncode="False" />
                    <asp:BoundField DataField="End_User_City" HeaderText="End User City" HtmlEncode="False" />
                </Columns>

                <EmptyDataTemplate>
                    <center>    
                            <table class="emptyTable"  >
                            <tr>
                                <td>
                                    ...no record available...
                                </td>
                            </tr>
                            </table>
                            </font> 
                        </center>
                </EmptyDataTemplate>

                <PagerSettings Mode="NumericFirstLast" NextPageText="Next" />
                <PagerStyle CssClass="Pager" />
            </asp:GridView>
        </div>
    </asp:Panel>

    <div runat="server" id="pnlAllDetails">
        <asp:Panel runat="server" ID="pnlSOHeader" CssClass="w3-panel w3-animate-opacity">
            <div class="w3-row w3-card-4 w3-white w3-padding-bottom w3-round-small">
                <div class="w3-col s12 w3-padding-left">
                    <h5 class="w3-text-green w3-leftbar">&nbsp;SO Details</h5>
                </div>
                <div class="w3-half w3-padding-right w3-padding-left w3-padding-bottom">
                    <div>
                        <asp:Label runat="server" ID="lblDueDate" Style="font-size: small; font-weight: bold;" Text="Due Date"></asp:Label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtDueDate" runat="server" CssClass="w3-input w3-border TextBoxStyle" TextMode="Date"></asp:TextBox>
                        <asp:TextBox ID="txtDueDate_" runat="server" CssClass="w3-input w3-border TextBoxStyle" type="text"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Customer PO No.</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtCustPONum" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Site</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:DropDownList ID="ddSite" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small">
                            <asp:ListItem>--Select Location-</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtSite" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small" Visible="false"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Credit Term</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:DropDownList ID="ddCreditTerm" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Visible="false">
                            <asp:ListItem>--Select Credit Term-</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtCreditTerm" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small" Visible="false"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Currency</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:DropDownList ID="ddCurrency" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small">
                            <asp:ListItem>--Select Currency-</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtCurrency" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small" Visible="false"></asp:TextBox>
                    </div>
                </div>
                <div class="w3-half w3-padding-right">
                    <div id="pnlOrderReqlbl" runat="server">
                        <asp:Label runat="server" ID="lblOrderDate" Style="font-size: small; font-weight: bold;" Text="Order Date"></asp:Label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtOrderDate" runat="server" CssClass="w3-input w3-border TextBoxStyle" TextMode="Date"></asp:TextBox>
                        <asp:TextBox ID="txtOrderDate_" runat="server" CssClass="w3-input w3-border TextBoxStyle" type="text"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Salesman</label>
                        <asp:TextBox ID="txtSalesman" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                        <asp:DropDownList ID="ddSalesman" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <label style="font-size: small">Customer Code</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:Table ID="Table3" CellPadding="0" CellSpacing="0" runat="server" Width="100%">
                            <asp:TableRow>
                                <asp:TableCell Width="20%">
                                    <asp:TextBox ID="txtCustomerCode" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder=""></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell Width=".1%">
                                    <asp:LinkButton runat="server" Style="width: 100%;" ID="lnkBtnSearchCust" Text="Search" CssClass="w3-btn w3-blue ButtonStyle w3-right" OnClick="lnkBtnSearchCust_Click"><i class="glyphicon glyphicon-search"></i></asp:LinkButton>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>
                    <div>
                        <label style="font-size: small">Company Name</label>
                        <asp:TextBox ID="txtCustomerName" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Stock Status</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:DropDownList ID="ddStockStatus" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small">
                            <asp:ListItem>On Stock</asp:ListItem>
                            <asp:ListItem>Order Basis</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtStockStatus" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small" Visible="false"></asp:TextBox>
                    </div>
                </div>

                <div class="w3-col s12 w3-padding-left w3-padding-right w3-padding-bottom">
                    <div>
                        <label style="font-size: small">Special Concession</label>&nbsp;<label class="requiredColor" style="font-size: 10px;">For BCC use only</label>
                        <asp:TextBox ID="txtSpecialConc" runat="server" class="w3-input w3-border TextBoxStyle" type="text" TextMode="MultiLine" Height="60px" Font-Size="Small"></asp:TextBox>
                    </div>
                </div>

                <div class="w3-col s12 w3-padding-left">
                    <h5 class="w3-text-green w3-leftbar">&nbsp;End User Details</h5>
                </div>

                <div class="w3-half w3-padding-right w3-padding-left w3-padding-bottom">
                    <div>
                        <label style="font-size: small">End User</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtEndUser" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
                    </div>
                </div>

                <div class="w3-half w3-padding-right">
                    <div>
                        <label style="font-size: small">City</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtEndUserCity" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
                    </div>
                </div>

                <div class="w3-col s12 w3-padding-left w3-padding-right w3-padding-bottom">
                    <div>
                        <label style="font-size: small">Remarks</label>
                        <asp:TextBox ID="txtRemarks" runat="server" class="w3-input w3-border TextBoxStyle" type="text" TextMode="MultiLine" Height="60px" Font-Size="Small"></asp:TextBox>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel runat="server" ID="pnlItemDetails" CssClass="w3-panel w3-animate-opacity">
            <div class="w3-row w3-card-4 w3-white w3-padding-bottom w3-round-small">

                <div class="w3-col s12 w3-padding-left">
                    <h5 class="w3-text-green w3-leftbar">&nbsp;Item Details</h5>
                </div>

                <div>
                    <div class="w3-half w3-padding-right w3-padding-left w3-padding-bottom">
                        <asp:Button ID="btnAdd" CssClass="w3-btn w3-green ButtonStyle" runat="server" Text="Add Item" OnClick="btnAdd_Click" />
                    </div>
                    <div id="taxexempt" runat="server" style="float: right; margin-right: 3%;">
                        <label style="font-size: small"></label>
                        <asp:CheckBox ID="chckTaxExempt" runat="server" Text="Tax Exempt" Font-Size="Small" AutoPostBack="true" OnCheckedChanged="chckTaxExempt_CheckedChanged" />
                    </div>
                    <div class="w3-col s12 w3-padding-left">
                        <label class="requiredColor">*</label>
                        <asp:Table runat="server" Width="100%" CellPadding="0" CellSpacing="0">

                            <asp:TableRow>
                                <asp:TableCell CssClass="paddingRow" Width="100%">
                                    <asp:GridView ID="gvItems" runat="server" BorderColor="White" OnRowDataBound="gvItems_RowDataBound" GridLines="Horizontal" Width="100%" class="table table-hover" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True">
                                        <HeaderStyle CssClass="w3-blue" />
                                        <Columns>
                                            <asp:BoundField DataField="idItem" HeaderText="idItem" HtmlEncode="False" />
                                            <asp:BoundField DataField="Item_Number" HeaderText="Item No." HtmlEncode="False" />
                                            <asp:BoundField DataField="Description" HeaderText="Description" HtmlEncode="False" />
                                            <asp:BoundField DataField="Qty" HeaderText="Qty" HtmlEncode="False" />
                                            <asp:BoundField DataField="Cost" HeaderText="Cost" HtmlEncode="False" />
                                            <asp:BoundField DataField="UM" HeaderText="UM" HtmlEncode="False" />
                                            <asp:BoundField DataField="Discount" HeaderText="Discount" HtmlEncode="False" />
                                            <asp:BoundField DataField="Tax_Amount" HeaderText="Tax_Amount" HtmlEncode="False" />
                                            <asp:BoundField DataField="Amount" HeaderText="Amount" HtmlEncode="False" />
                                            <asp:BoundField DataField="Item_Status" HeaderText="Status" HtmlEncode="False" />
                                            <asp:BoundField DataField="idSODetail" HeaderText="idSODetail" HtmlEncode="False" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <center>   
                                            <table class="emptyTable"  >
                                            <tr>
                                                <td>
                                                ...no items added...
                                                </td>
                                            </tr>
                                            </table>
              
                                            </center>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkRemove" name="lnkRemove" Text="Remove" runat="server" OnClick="lnkRemove_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" name="lnkDelete" Text="Delete" runat="server" OnClick="lnkDelete_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" name="lnkEdit" Text="Edit" runat="server" OnClick="lnkEdit_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:TableCell>
                                <asp:TableCell>
                                    &nbsp;
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>
                </div>
                <div class="w3-half w3-padding-right w3-padding-left w3-padding-bottom">

                    <div>
                        <label style="font-size: small" runat="server" visible="false">Freight Charges</label>
                        <asp:TextBox Visible="false" ID="txtFreightCharges" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
                    </div>
                    <div id="grossamt" runat="server">
                        <label style="font-size: small">Gross Amount</label>
                        <asp:TextBox ID="txtGrossAmt" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
                    </div>
                    <div id="netamt" runat="server">
                        <label style="font-size: small">Net Amount</label>
                        <asp:TextBox ID="txtNetAmt" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
                    </div>
                </div>
                <div class="w3-half w3-padding-right">
                    <div>
                        <label style="font-size: small" runat="server" visible="false">Other Charges</label>
                        <asp:TextBox Visible="false" ID="txtOtherCharges" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
                    </div>
                    <div id="outputvat" runat="server">
                        <label style="font-size: small">Output Vat</label>
                        <asp:TextBox ID="txtOutputVat" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>

    <div id="myModal" class="modal" runat="server" visible="false" style="display: block; z-index: 1000; background-color: rgba(0, 0, 0, 0.37); overflow-y: auto;">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" runat="server" id="btnMyModalClose" onserverclick="btnMyModalClose_Click">&times;</button>
                    <h4 class="modal-title">Item List : </h4>
                </div>

                <div class="modal-body">
                    <div class="w3-animate-opacity w3-margin-0">
                        <div class="w3-col s8 w3-padding-bottom w3-left" style="vertical-align: central;">
                            <asp:Table ID="Table1" CellPadding="0" CellSpacing="0" runat="server" Width="100%" Style="border: 1px solid #2196F3">
                                <asp:TableRow>
                                    <asp:TableCell Width="15%">
                                        <asp:DropDownList ID="ddItemSearchItem" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small">
                                            <asp:ListItem>--- Search by ---</asp:ListItem>
                                            <asp:ListItem>Product Name</asp:ListItem>
                                            <asp:ListItem>Item Number</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                    <asp:TableCell Width="20%">
                                        <asp:TextBox ID="txtSearchItem" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder=""></asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell Width="10%">
                                        <asp:DropDownList ID="ddlItemSite" runat="server" class="w3-input w3-border TextBoxStyle"></asp:DropDownList>
                                    </asp:TableCell>
                                    <asp:TableCell Width="5%">
                                        <asp:LinkButton runat="server" Style="width: 100%;" ID="btnSearchItem" Text="Search" CssClass="w3-btn w3-green ButtonStyle w3-right" OnClick="btnSearchItem_Click"><i class="glyphicon glyphicon-search"></i></asp:LinkButton>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                    </div>

                    <asp:GridView ID="grvItemClass" AutoGenerateSelectButton="true"
                        OnPageIndexChanging="grvItemClass_PageIndexChanging" OnSelectedIndexChanged="grvItemClass_SelectedIndexChanged"
                        OnRowDataBound="grvItemClass_RowDataBound" runat="server" CssClass="w3-table w3-hoverable w3-round-small"
                        AutoGenerateColumns="true" AllowPaging="True" PageSize="5" Font-Size="14px" PagerStyle-BorderStyle="None" PagerStyle-Height="20px">
                        <HeaderStyle CssClass="w3-blue" />
                        <%--<Columns>
                            <asp:BoundField DataField="idItem" HeaderText="idItem" HtmlEncode="False" />
                            <asp:BoundField DataField="idClass" HeaderText="idClass" HtmlEncode="False" />
                            <asp:BoundField DataField="Product_Name" HeaderText="Product Name" HtmlEncode="False" />
                            <asp:BoundField DataField="Item_Number" HeaderText="Item No." HtmlEncode="False" />
                            <asp:BoundField DataField="Description" HeaderText="Description" HtmlEncode="False" />
                            <asp:BoundField DataField="UM" HeaderText="Description" HtmlEncode="False" />
                        </Columns>--%>

                        <EmptyDataTemplate>
                            <center>    
                                    <table class="emptyTable"  >
                                    <tr>
                                        <td>
                                            ...no record available...
                                        </td>
                                    </tr>
                                    </table>
                                    </font> 
                                </center>
                        </EmptyDataTemplate>

                        <PagerSettings Mode="NumericFirstLast" NextPageText="Next" />
                        <PagerStyle CssClass="Pager" />
                    </asp:GridView>
                </div>
                <div class="modal-footer">
                    <asp:Table runat="server">
                        <asp:TableRow>
                            <asp:TableCell>
                                <button type="button" id="btnCancel1" class="btn btn-default navbar-left" runat="server" onserverclick="btnCancel1_Click">Cancel</button>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
        </div>
    </div>

    <div id="myModal2" class="modal" runat="server" visible="false" style="display: block; z-index: 1000; background-color: rgba(0, 0, 0, 0.37); overflow-y: auto;">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" runat="server" id="btnMyModalClose2" onserverclick="btnMyModalClose2_Click">&times;</button>
                    <h4 class="modal-title">Enter Item Details : </h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblidItem" runat="server" type="text" Font-Size="Small" Visible="false"></asp:Label>
                    <div id="productnamediv" runat="server">
                        <label style="font-size: small">Product Name</label>
                        <asp:TextBox ID="txtItemName" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Item No.</label>
                        <asp:TextBox ID="txtItemNo" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Description</label>
                        <asp:TextBox ID="txtDescription" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Quantity</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtQuantity" runat="server" onkeypress="return isNumberKey(event)" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Cost</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtCost" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small"></asp:TextBox>
                    </div>
                    <div>
                        <%--<label style="font-size: small">UM</label>--%>
                        <asp:TextBox ID="txtUM" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small" Visible="false"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Discount</label>
                        <asp:TextBox ID="txtDiscount" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Table runat="server">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Button ID="btnAddToTempGv" runat="server" AutoPostBack="true" OnClick="btnAddToTempGv_Click" class="btn btn-primary navbar-left" data-toggle="modal" data-target="#myModal" Text="Add" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <button type="button" id="btnCancel2" class="btn btn-default navbar-left" runat="server" onserverclick="btnCancel2_Click">Cancel</button>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
        </div>
    </div>

    <div id="myModalWarning" class="modal" runat="server" visible="false" style="display: block; z-index: 1000; background-color: rgba(0, 0, 0, 0.37); overflow-y: auto;">
        <div class="modal-dialog modal-sm">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header w3-red">
                </div>
                <div class="modal-body">

                    <asp:Label ID="lblItemWarning" runat="server" type="text" Font-Size="Small" Text="" Visible="true"></asp:Label>

                </div>
                <div class="modal-footer">
                    <asp:Table runat="server">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Button ID="btnCloseWarning" runat="server" AutoPostBack="true" OnClick="btnCloseWarning_Click" class="btn btn-primary navbar-right" data-toggle="modal" data-target="#myModal" Text="OK" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
        </div>
    </div>

    <div id="myModalCustomerDetails" class="modal" runat="server" visible="false" style="display: block; z-index: 1000; background-color: rgba(0, 0, 0, 0.37); overflow-y: auto;">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" runat="server" id="btnCloseCustModal" onserverclick="btnCloseCustModal_Click">&times;</button>
                    <h4 class="modal-title">Customer List : </h4>
                </div>

                <div class="modal-body">
                    <div class="w3-animate-opacity w3-margin-0">
                        <div class="w3-col s8 w3-padding-bottom w3-left" style="vertical-align: central;">
                            <asp:Table ID="Table2" CellPadding="0" CellSpacing="0" runat="server" Width="100%" Style="border: 1px solid #2196F3">
                                <asp:TableRow>
                                    <asp:TableCell Width="15%">
                                        <asp:DropDownList ID="ddSearchByCustomer" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small">
                                            <asp:ListItem>--- Search by ---</asp:ListItem>
                                            <asp:ListItem>Company Name</asp:ListItem>
                                            <asp:ListItem>Customer Code</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                    <asp:TableCell Width="20%">
                                        <asp:TextBox ID="txtSearchCustomer" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder=""></asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell Width="5%">
                                        <asp:LinkButton runat="server" Style="width: 100%;" ID="lnkBtnSearchCustomer" Text="Search" CssClass="w3-btn w3-green ButtonStyle w3-right" OnClick="lnkBtnSearchCustomer_Click"><i class="glyphicon glyphicon-search"></i></asp:LinkButton>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                    </div>

                    <asp:GridView ID="grvCustomerDetails" DataKeyNames="idCustomer" AutoGenerateSelectButton="true" OnPageIndexChanging="grvCustomerDetails_PageIndexChanging" OnSelectedIndexChanged="grvCustomerDetails_SelectedIndexChanged" OnRowDataBound="grvCustomerDetails_RowDataBound" runat="server" CssClass="w3-table w3-hoverable w3-round-small" AutoGenerateColumns="False" AllowPaging="True" PageSize="8" Font-Size="14px" PagerStyle-BorderStyle="None" PagerStyle-Height="20px">
                        <HeaderStyle CssClass="w3-blue" />
                        <Columns>
                            <asp:BoundField DataField="idCustomer" HeaderText="idCustomer" HtmlEncode="False" />
                            <asp:BoundField DataField="Customer_Code" HeaderText="Customer Code" HtmlEncode="False" />
                            <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name" HtmlEncode="False" />
                            <asp:BoundField DataField="Company_Name" HeaderText="Company Name" HtmlEncode="False" />
                            <asp:BoundField DataField="Address1" HeaderText="Address1" HtmlEncode="False" />
                            <asp:BoundField DataField="Address2" HeaderText="Address2" HtmlEncode="False" />
                            <asp:BoundField DataField="Address3" HeaderText="Address3" HtmlEncode="False" />
                            <asp:BoundField DataField="Address4" HeaderText="Address4" HtmlEncode="False" />
                            <asp:BoundField DataField="AddressShipping1" HeaderText="AddressShipping1" HtmlEncode="False" />
                            <asp:BoundField DataField="AddressShipping2" HeaderText="AddressShipping2" HtmlEncode="False" />
                            <asp:BoundField DataField="AddressShipping3" HeaderText="AddressShipping3" HtmlEncode="False" />
                            <asp:BoundField DataField="AddressShipping4" HeaderText="AddressShipping4" HtmlEncode="False" />
                            <asp:BoundField DataField="credit_term" HeaderText="credit_term" HtmlEncode="False" />
                        </Columns>

                        <EmptyDataTemplate>
                            <center>    
                                    <table class="emptyTable"  >
                                    <tr>
                                        <td>
                                            ...no record available...
                                        </td>
                                    </tr>
                                    </table>
                                    </font> 
                                </center>
                        </EmptyDataTemplate>

                        <PagerSettings Mode="NumericFirstLast" NextPageText="Next" />
                        <PagerStyle CssClass="Pager" />
                    </asp:GridView>
                </div>
                <div class="modal-footer">
                    <asp:Table runat="server">
                        <asp:TableRow>
                            <asp:TableCell>
                                <button type="button" id="btnCancelCustModal" class="btn btn-default navbar-left" runat="server" onserverclick="btnCancelCustModal_Click">Cancel</button>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
        </div>
    </div>

    <div id="myModalConfirmation" class="modal" runat="server" visible="false" style="display: block; z-index: 1000; background-color: rgba(0, 0, 0, 0.37); overflow-y: auto;">
        <div class="modal-dialog modal-sm">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header w3-red">
                </div>
                <div class="modal-body">

                    <asp:Label ID="lblConfirmMsg" runat="server" type="text" Font-Size="Small" Text="" Visible="true"></asp:Label>

                </div>
                <div class="modal-footer">
                    <asp:Table runat="server">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Button ID="btnConfirmDelete" runat="server" OnClick="btnConfirmDelete_Click" class="btn btn-primary navbar-right" data-toggle="modal" data-target="#myModal" Text="Confirm" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button ID="btnCancelDelete" runat="server" OnClick="btnCancelDelete_Click" class="btn btn-default navbar-left" Style="margin-left: 20px;" data-toggle="modal" data-target="#myModal" Text="Cancel" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
        </div>
    </div>

    <div id="dvDownloadSalesReport" class="modal" runat="server" visible="false" style="display: block; z-index: 1000; background-color: rgba(0, 0, 0, 0.37); overflow-y: auto;">
        <div class="modal-dialog modal-sm">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header w3-blue">
                    <h4 class="modal-title">Choose date range : </h4>
                </div>
                <div class="modal-body">
                    <div id="Div1" runat="server">
                        <asp:Label runat="server" ID="Label1" Style="font-size: small; font-weight: bold;" Text="From"></asp:Label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="w3-input w3-border TextBoxStyle" TextMode="Date"></asp:TextBox>
                    </div>
                    <div id="Div2" runat="server">
                        <asp:Label runat="server" ID="Label2" Style="font-size: small; font-weight: bold;" Text="To"></asp:Label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="w3-input w3-border TextBoxStyle" TextMode="Date"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Table runat="server">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Button ID="btnDownloadReport" runat="server" OnClick="btnDownloadReport_Click" class="btn btn-primary navbar-right" data-toggle="modal" data-target="#myModal" Text="Confirm" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button ID="btnCancelDownload" runat="server" OnClick="btnCancelDownload_Click" class="btn btn-default navbar-left" Style="margin-left: 20px;" data-toggle="modal" data-target="#myModal" Text="Cancel" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
        </div>
    </div>

    <div id="dvItemNumberSO" class="modal" runat="server" visible="false" style="display: block; z-index: 1000; background-color: rgba(0, 0, 0, 0.37); overflow-y: auto;">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" runat="server" id="btnCloseItemNumberSO" onserverclick="btnCloseItemNumberSO_ServerClick">&times;</button>
                    <h4 class="modal-title">Item List : </h4>
                </div>

                <div class="modal-body" style="height: 350px">
                    <div class="w3-animate-opacity w3-margin-0">
                        <div class="w3-col s8 w3-padding-bottom w3-left" style="vertical-align: central;">
                            <asp:Table ID="Table5" CellPadding="0" CellSpacing="0" runat="server" Width="100%" Style="border: 1px solid #2196F3">
                                <asp:TableRow>
                                    <asp:TableCell Width="95%">
                                        <asp:TextBox ID="txtSearchItemNumberSO" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder=""></asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell Width="5%">
                                        <asp:LinkButton runat="server" Style="width: 100%;" ID="lbtnSearchItemNumberSO" Text="Search" CssClass="w3-btn w3-green ButtonStyle w3-right" OnClick="lbtnSearchItemNumberSO_Click"><i class="glyphicon glyphicon-search"></i></asp:LinkButton>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                    </div>

                    <asp:GridView ID="gvItemNumberSO" OnPageIndexChanging="grvItemClass_PageIndexChanging" GridLines="Horizontal"
                        runat="server" CssClass="w3-table w3-hoverable w3-round-small" AutoGenerateColumns="true" AllowPaging="True"
                        PageSize="5" Font-Size="14px" PagerStyle-BorderStyle="None" PagerStyle-Height="20px">
                        <HeaderStyle CssClass="w3-blue" />
                        <EmptyDataTemplate>
                            <center>    
                                <table class="emptyTable"  >
                                    <tr>
                                        <td>
                                            ...no record available...
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </EmptyDataTemplate>
                        <PagerSettings Mode="NumericFirstLast" NextPageText="Next" />
                        <PagerStyle CssClass="Pager" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <script>
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
</asp:Content>

