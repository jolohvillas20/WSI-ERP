<%@ Page
    Title=""
    Language="C#"
    MasterPageFile="~/Site Master/SiteMaster.Master"
    AutoEventWireup="true"
    CodeBehind="InvoiceManagement.aspx.cs"
    Inherits="SOPOINV.Forms.InvoiceManagement"
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
            <asp:Button ID="btnSave" CssClass="w3-btn w3-green ButtonStyle" runat="server" Text="Save" OnClick="btnSave_Click" Visible="false" />
        </div>
        <div class="w3-animate-opacity w3-margin-0">
            <div class="w3-col s3 w3-padding-0 w3-right" style="vertical-align: central;">
                <asp:Table ID="tblSearch" CellPadding="0" CellSpacing="0" runat="server" Width="100%" Style="border: 1px solid #2196F3; display: inline-block">
                    <asp:TableRow>
                        <asp:TableCell Width="95%">
                            <asp:TextBox ID="txtSearch" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="Enter S.O. Number"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell Width="5%">
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


    <asp:Panel runat="server" ID="InvoicePanel" CssClass="w3-panel" Style="margin-top: 0px" Visible="true">
        <div class="w3-white w3-card-4" runat="server">
            <asp:GridView ID="grvSOInvoiceList" runat="server" CssClass="w3-table w3-hoverable w3-round-small" AutoGenerateColumns="True"
                OnSelectedIndexChanged="grvSOInvoiceList_SelectedIndexChanged" OnRowDataBound="grvSOInvoiceList_RowDataBound"
                OnPageIndexChanging="grvSOInvoiceList_PageIndexChanging" ShowHeaderWhenEmpty="True" AllowPaging="True"
                PageSize="10" Font-Size="14px" PagerStyle-BorderStyle="None" PagerStyle-Height="20px" GridLines="Horizontal">
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
                        <asp:TextBox ID="txtCreditTerm" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small" Visible="false"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Currency</label>&nbsp;<label class="requiredColor">*</label>
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
                            </asp:TableRow>
                        </asp:Table>
                    </div>
                    <div>
                        <label style="font-size: small">Company Name</label>
                        <asp:TextBox ID="txtCustomerName" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Stock Status</label>&nbsp;<label class="requiredColor">*</label>
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

        <asp:Panel runat="server" ID="pnlInvoice" CssClass="w3-panel w3-animate-opacity">
            <div class="w3-row w3-card-4 w3-white w3-padding-bottom w3-round-small">
                <div class="w3-col s12 w3-padding-left">
                    <h5 class="w3-text-green w3-leftbar">&nbsp;Invoice Details</h5>
                </div>
                <div class="w3-half w3-padding-right w3-padding-left w3-padding-bottom">
                    <div>
                        <label style="font-size: small">Invoice Number</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtInvoiceNumber" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">DR Number</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtDRNumber" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Delivery Date</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtDelDate" runat="server" CssClass="w3-input w3-border TextBoxStyle" TextMode="Date"></asp:TextBox>
                    </div>
                </div>
                <div class="w3-half w3-padding-right">
                    <div>
                        <label style="font-size: small">Invoice Date</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="w3-input w3-border TextBoxStyle" TextMode="Date"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Invoice Amount</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtInvoiceAmount" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
                    </div>
                      <div>
                        <label style="font-size: small">OR Number</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtORNumber" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
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
                    <div id="taxexempt" runat="server" style="float: right; margin-right: 3%;">
                        <label style="font-size: small"></label>
                        <asp:CheckBox ID="chckTaxExempt" runat="server" Text="Tax Exempt" Font-Size="Small" />
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
                        <label style="font-size: small">Freight Charges</label>
                        <asp:TextBox ID="txtFreightCharges" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
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
                        <label style="font-size: small">Other Charges</label>
                        <asp:TextBox ID="txtOtherCharges" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
                    </div>
                    <div id="outputvat" runat="server">
                        <label style="font-size: small">Output Vat</label>
                        <asp:TextBox ID="txtOutputVat" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
                    </div>
                </div>
            </div>
        </asp:Panel>
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

</asp:Content>
