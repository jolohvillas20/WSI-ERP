<%@ Page Title="ERP - PO Creation"
    Language="C#"
    Async="true"
    EnableSessionState="True"
    MasterPageFile="~/Site Master/SiteMaster.Master"
    AutoEventWireup="true"
    CodeBehind="POCreation.aspx.cs"
    Inherits="SOPOINV.Forms.POCreation"
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
        <div class="w3-col s8">
            <asp:Button ID="btnBack" CssClass="w3-btn w3-green ButtonStyle" runat="server" Text="Back" OnClick="btnBack_Click" Visible="false" />
            <asp:Button ID="btnEdit" CssClass="w3-btn w3-green ButtonStyle" runat="server" Text="Edit" OnClick="btnEdit_Click" Visible="false" />
            <asp:Button ID="btnAddNew" CssClass="w3-btn w3-green ButtonStyle" runat="server" Text="Add New" OnClick="btnAddNew_Click" Visible="false" />
            <asp:Button ID="btnSave" CssClass="w3-btn w3-green ButtonStyle" runat="server" Text="Save" OnClick="btnSave_Click" Visible="false" />
            <asp:Button ID="btnPrintPO" CssClass="w3-btn w3-blue ButtonStyle" runat="server" Text="Download P.O." OnClick="btnPrintPO_Click" Visible="False" />
        </div>
        <div class="w3-animate-opacity w3-margin-0">
            <div class="w3-col s3 w3-padding-0 w3-right" style="vertical-align: central;">
                <asp:Table ID="tblSearch" CellPadding="0" CellSpacing="0" runat="server" Width="100%" Style="border: 1px solid #2196F3">
                    <asp:TableRow>
                        <asp:TableCell Width="90%">
                            <asp:TextBox ID="txtSearch" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="Enter P.O. Number"></asp:TextBox>
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
                            <asp:Label ID="lblPONumber_" runat="server" type="text" Font-Bold="true" Font-Size="Medium" ForeColor="gray" Text="PO # :" Visible="false"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell Width="80%">
                            <asp:Label ID="lblPONumber" runat="server" type="text" Font-Bold="true" Font-Size="Medium" ForeColor="lightseagreen" Visible="false"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel runat="server" ID="POPanel" CssClass="w3-panel" Style="margin-top: 0px">
        <div class="w3-white w3-card-4" runat="server">
            <asp:GridView ID="grvMainPO" DataKeyNames="idPOHeader" OnSelectedIndexChanged="grvMainPO_SelectedIndexChanged" OnRowDataBound="grvMainPO_RowDataBound" OnPageIndexChanging="grvMainPO_PageIndexChanging" runat="server" CssClass="w3-table w3-hoverable w3-round-small" AutoGenerateColumns="true" ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="10" Font-Size="14px" PagerStyle-BorderStyle="None" PagerStyle-Height="20px">
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
        <asp:Panel runat="server" ID="pnlPOHeader" CssClass="w3-panel w3-animate-opacity">
            <div class="w3-row w3-card-4 w3-white w3-padding-bottom w3-round-small">
                <div class="w3-col s12 w3-padding-left">
                    <h5 class="w3-text-green w3-leftbar">&nbsp;PO Details</h5>
                </div>
                <div class="w3-half w3-padding-right w3-padding-left w3-padding-bottom">                    
                    <div>
                        <asp:Label runat="server" Style="font-size: small; font-weight: bold;" Text="PO Amount"></asp:Label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtPOAmount" runat="server" CssClass="w3-input w3-border TextBoxStyle" type="text" Text="0" Enabled="false"></asp:TextBox>
                    </div>
                    <div>
                        <asp:Label runat="server" Style="font-size: small; font-weight: bold;" Text="Terms"></asp:Label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtTerms" runat="server" CssClass="w3-input w3-border TextBoxStyle" type="text"></asp:TextBox>
                    </div>
                    <%--<div>
                        <label style="font-size: small">Credit Term</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:DropDownList ID="ddCreditTerm" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small">
                            <asp:ListItem>--Select Credit Term-</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div>
                        <label style="font-size: small">Currency</label>&nbsp;<label class="requiredColor">*</label>                       
                        <asp:TextBox ID="txtCurrency" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small" Visible="false"></asp:TextBox>
                    </div>--%>
                </div>
                <div class="w3-half w3-padding-right">
                    <%--<div>
                        <label style="font-size: small">Total Charges</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtTotalCharges" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small" Text="0"></asp:TextBox>                       
                    </div>--%>
                    <div id="pnlOrderReqlbl" runat="server">
                        <asp:Label runat="server" ID="lblOrderDate" Style="font-size: small; font-weight: bold;" Text="PO Quantity"></asp:Label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtPOQuantity" runat="server" CssClass="w3-input w3-border TextBoxStyle" type="text" Text="0" Enabled="false"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Forex Rate</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtForexRate" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                    <%--<div>
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
                    </div>--%>
                </div>

                <div class="w3-col s12 w3-padding-left w3-padding-right w3-padding-bottom">                 
                    <div>
                        <label style="font-size: small">Remarks</label>
                        <asp:TextBox ID="txtRemarks" runat="server" class="w3-input w3-border TextBoxStyle" style="resize:none;" type="text" TextMode="MultiLine" Height="60px" Font-Size="Small"></asp:TextBox>
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
                    <div class="w3-col s12 w3-padding-left">
                        <label class="requiredColor">*</label>
                        <asp:Table runat="server" Width="100%" CellPadding="0" CellSpacing="0">

                            <asp:TableRow>
                                <asp:TableCell CssClass="paddingRow" Width="100%">
                                    <asp:GridView ID="gvItems" runat="server" BorderColor="White" OnRowDataBound="gvItems_RowDataBound" OnSelectedIndexChanged="gvItems_SelectedIndexChanged"
                                        GridLines="Horizontal" Width="100%" class="table table-hover" AutoGenerateColumns="true" ShowHeaderWhenEmpty="True">
                                        <HeaderStyle CssClass="w3-blue" />
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
                                    <asp:TableCell Width="5%">
                                        <asp:LinkButton runat="server" Style="width: 100%;" ID="btnSearchItem" Text="Search" CssClass="w3-btn w3-green ButtonStyle w3-right" OnClick="btnSearchItem_Click"><i class="glyphicon glyphicon-search"></i></asp:LinkButton>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                    </div>

                    <asp:GridView ID="grvItemClass" DataKeyNames="idItem" AutoGenerateSelectButton="true" 
                        OnPageIndexChanging="grvItemClass_PageIndexChanging" OnSelectedIndexChanged="grvItemClass_SelectedIndexChanged" OnRowDataBound="grvItemClass_RowDataBound" runat="server" CssClass="w3-table w3-hoverable w3-round-small" AutoGenerateColumns="False" AllowPaging="True" PageSize="5" Font-Size="14px" PagerStyle-BorderStyle="None" PagerStyle-Height="20px">
                        <HeaderStyle CssClass="w3-blue" />
                        <Columns>
                            <asp:BoundField DataField="idItem" HeaderText="idItem" HtmlEncode="False" />
                            <asp:BoundField DataField="idClass" HeaderText="idClass" HtmlEncode="False" />
                            <asp:BoundField DataField="Product_Name" HeaderText="Product Name" HtmlEncode="False" />
                            <asp:BoundField DataField="Item_Number" HeaderText="Item No." HtmlEncode="False" />
                            <asp:BoundField DataField="Description" HeaderText="Description" HtmlEncode="False" />
                            <asp:BoundField DataField="UM" HeaderText="Description" HtmlEncode="False" />
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
                    <%--<asp:Label ID="lblidItem" runat="server" type="text" Font-Size="Small" Visible="false"></asp:Label>--%>
                    <div>
                        <label style="font-size: small">Item</label>
                        <asp:TextBox ID="txtItemName" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Quantity</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtQuantity" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Cost</label>&nbsp;<label class="requiredColor">*</label>
                        <asp:TextBox ID="txtPrice" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small"></asp:TextBox>
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
                            <asp:TableCell>
                                <asp:Button ID="btnRemove" runat="server" AutoPostBack="true" OnClick="btnRemove_Click" class="btn btn-primary navbar-left" data-toggle="modal" data-target="#myModal" Text="Remove" />
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
                          <%--  <asp:TableCell>
                                <asp:Button ID="btnConfirmDelete" runat="server" OnClick="btnConfirmDelete_Click" class="btn btn-primary navbar-right" data-toggle="modal" data-target="#myModal" Text="Confirm" />
                            </asp:TableCell>--%>
                            <asp:TableCell>
                                <asp:Button ID="btnCancelDelete" runat="server" OnClick="btnCancelDelete_Click" class="btn btn-default navbar-left" Style="margin-left: 20px;" data-toggle="modal" data-target="#myModal" Text="Cancel" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
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
