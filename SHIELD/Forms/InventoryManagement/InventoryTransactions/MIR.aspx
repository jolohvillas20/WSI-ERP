<%@ Page
    Title="Inventory Transactions - MIR"
    Language="C#"
    MasterPageFile="~/Site Master/SiteMaster.Master"
    AutoEventWireup="true"
    CodeBehind="MIR.aspx.cs"
    Inherits="SOPOINV.Forms.InventoryManagement.InventoryTransactions.MIR"
    EnableEventValidation="false"
    ValidateRequest="false"
    MaintainScrollPositionOnPostback="true"
    EnableSessionState="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../../Content/css/accordion.css" rel="stylesheet" />
    <script src="../../../Scripts/accordionajax.txt"></script>
    <script src="../../../Scripts/accordionbootstrap.txt"></script>
    <link href="../../../Content/css/button.css" rel="stylesheet" />
    <link href="../../../Content/css/Comments.css" rel="stylesheet" />
    <link href="../../../Content/css/bootstrap.css" rel="stylesheet" />
    <script src="../../../Scripts/moment.js"></script>
    <script src="../../../Scripts/bootstrap-datetimepicker.js"></script>
    <link href="../../../Content/css/bootstrap-datetimepicker.css" rel="stylesheet" />
    <script src="../../../Scripts/bootstrap-datetimepicker.min.js"></script>
    <script src="../../../Script/jquery_3.3.1_jquery.min.js"></script>
    <script src="../../../Script/bootstrap_3.3.7_js_bootstrap.min.js"></script>
    <link rel="stylesheet" href="../../../css/bootstrap.css" />
    <link rel="stylesheet" href="../../../css/w3.css" />

    <%--<link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css" />--%>

    <style>
        .pagination-ys {
            /*display: inline-block;*/
            padding-left: 0;
            margin: 20px 0;
            border-radius: 4px;
        }

            .pagination-ys table > tbody > tr > td {
                display: inline;
            }

                .pagination-ys table > tbody > tr > td > a,
                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    color: #000000;
                    background-color: #ffffff;
                    border: 1px solid #dddddd;
                    margin-left: -1px;
                }

                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    margin-left: -1px;
                    z-index: 2;
                    color: #aea79f;
                    background-color: #f5f5f5;
                    border-color: #dddddd;
                    cursor: default;
                }

                .pagination-ys table > tbody > tr > td:first-child > a,
                .pagination-ys table > tbody > tr > td:first-child > span {
                    margin-left: 0;
                    border-bottom-left-radius: 4px;
                    border-top-left-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td:last-child > a,
                .pagination-ys table > tbody > tr > td:last-child > span {
                    border-bottom-right-radius: 4px;
                    border-top-right-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td > a:hover,
                .pagination-ys table > tbody > tr > td > span:hover,
                .pagination-ys table > tbody > tr > td > a:focus,
                .pagination-ys table > tbody > tr > td > span:focus {
                    color: #000000;
                    background-color: #eeeeee;
                    border-color: #dddddd;
                }

        .labelProp {
            font-size: 11px;
            margin: 0px;
            /*font-family: 'Segoe UI';*/
        }

        .buttonStyle {
            color: #0A90CF;
            border: 1px solid #0A90CF;
            border-radius: 0px;
            font-weight: 600;
        }

        .labelStyle {
            font-size: 12px;
            font-weight: bold;
            margin: 0px;
        }

        .labelStyle2 {
            font-weight: bold;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        .warningStyle {
            background-color: #EC756F;
            padding-left: 10px;
            text-align: center;
        }

        .successStyle {
            background-color: #89dd7f;
            padding-left: 10px;
            text-align: center;
        }

        .centerLoader {
            z-index: 1000;
            margin: 300px auto;
            /*padding: 10px;*/
            align-content: center;
            width: 250px;
            border-radius: 10px;
            filter: alpha(opacity=100);
            background-color: white;
            border: 1px solid #CCCCCC;
            /*opacity: 1;*/
            /*-moz-opacity: 1;*/
        }

        .paddingRow {
            padding-left: 10px;
            padding-right: 10px;
            padding-top: 4px;
        }

        .textBoxBorderRadius {
            border-radius: 0px;
            padding: 0px;
            padding-left: 5px;
            padding-right: 5px;
            /*font-size:16px;*/
            height: 30px;
            margin: 0px;
        }

        .buttonStyle {
            display: inline-block;
            padding: 4px 10px;
            margin-bottom: 0;
            font-size: 14px;
            font-weight: 400;
            line-height: 1.42857143;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            -ms-touch-action: manipulation;
            touch-action: manipulation;
            cursor: pointer;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            background-image: none;
            border: 1px solid transparent;
            border-radius: 4px;
        }

        .buttonStyle-default {
            color: #333;
            background-color: #fff;
            border-color: #ccc;
        }

            .buttonStyle-default:hover {
                color: #333;
                background-color: #e6e6e6;
                border-color: #adadad;
            }

        .buttonStyle.focus, .buttonStyle:focus, .buttonStyle:hover {
            color: #333;
            text-decoration: none;
        }

        .buttonStyle-default:hover {
            color: #333;
            background-color: #e6e6e6;
            border-color: #adadad;
        }

        .buttonStyle:hover, .buttonStyle:focus, .buttonStyle.focus {
            color: #333;
            text-decoration: none;
        }

        .labelStyle {
            font-size: 15px;
            font-weight: bold;
            margin: 0px;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        .labelStyle2 {
            font-weight: bold;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        .container {
            width: 100%;
            overflow: auto;
            padding: 10px 10px 10px 10px;
            background: #FFF;
            margin: auto;
        }

        .separator1 {
            float: left;
            background: #FFF;
            max-height: 250px;
            width: 50%;
            overflow-y: auto;
            padding: 5px 5px 5px 5px;
        }

        .separator2 {
            float: left;
            background: #FFF;
            max-height: 500px;
            width: 100%;
            overflow-y: auto;
            padding: 5px 5px 5px 5px;
        }

        .wholeBody {
            /*position: relative;*/
        }

        .searchHeader {
            position: sticky;
            background-color: white;
            top: -5px;
            z-index: 10;
            height: 50px;
            padding: 5px 5px 5px 5px;
            border-bottom: 1px solid black;
        }

        .searchLeft {
            float: left;
            width: 50%;
        }

        .searchRight {
            float: right;
            width: 50%;
        }

        /*.contentBody {
            display: block;
            overflow-y: visible;
            max-height: 200px;
        }*/

        .addButtonStyle {
            border: none;
            background-color: transparent;
            background-image: url('../../../Images/plus_30.png');
            background-repeat: no-repeat;
            height: 30px;
            width: 30px;
        }

        .searchButtonStyle {
            border: none;
            background-color: transparent;
            background-image: url('../../../Images/search_30.png');
            background-repeat: no-repeat;
            height: 30px;
            width: 30px;
        }

        .deleteButtonStyle {
            border: none;
            background-color: transparent;
            background-image: url('../../../Images/delete-sign_30.png');
            background-repeat: no-repeat;
            height: 25px;
            width: 25px;
        }

        .saveButtonStyle {
            border: none;
            background-color: transparent;
            background-image: url('../../../Images/save_30.png');
            background-repeat: no-repeat;
            height: 25px;
            width: 25px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-default card" style="border-radius: 0px; background-color: white; margin-top: 15px;">
        <div class="w3-padding">

            <h3 class="labelStyle2">
                <asp:Button runat="server" ID="btnMenu" OnClick="btnMenu_Click" Text="Inventory Transactions Menu" Style="border: none; background-color: white" /></h3>
            <%--       <ul class="nav nav-tabs">
                <li><a href="SalesReturn.aspx">Sales Return</a></li>
                <li><a href="CostAdjustment.aspx">Cost Adjustment</a></li>
                <li><a href="StockTransfers.aspx">Stock Transfer</a></li>
                <li><a href="InventoryHistory.aspx">Inventory History - Inquiry</a></li>
                <li><a href="InventoryReports.aspx">Inventory Reports</a></li>
                <li class="active"><a href="#UF">Update Forex</a></li>
            </ul>--%>
            <hr />
            <asp:Button runat="server" ID="btnView" Text="View MIR List" CssClass="buttonStyle w3-green" OnClick="btnView_Click" />
            <asp:Button runat="server" ID="btnCreate" Text="Create New MIR" CssClass="buttonStyle w3-green" OnClick="btnCreate_Click" />
        </div>
        <div runat="server" id="dvView" class="w3-padding" visible="true">
            <div class="w3-row">
                <asp:TextBox runat="server" ID="txtSearchRequest" CssClass="w3-input w3-border textBoxBorderRadius" Placeholder="Request Number" Style="float: left"></asp:TextBox>
                <asp:Button runat="server" ID="btnSearchRequest" Text="Search" CssClass="buttonStyle w3-green" OnClick="btnSearchRequest_Click" Style="float: left" />
            </div>
            <div>
                <asp:GridView ID="gvView" runat="server" OnSelectedIndexChanged="gvView_SelectedIndexChanged"
                    OnRowDataBound="gvView_RowDataBound" OnPageIndexChanging="gvView_PageIndexChanging" PageSize="10"
                    AllowPaging="true" BorderStyle="None" GridLines="Horizontal" CellPadding="5" Font-Size="14px" Width="100%"
                    EmptyDataText="No records has been added." AutoGenerateColumns="true" class="table table-hover">
                    <HeaderStyle CssClass="w3-blue" />
                    <PagerStyle CssClass="pagination-ys" />
                </asp:GridView>
            </div>
        </div>
        <div runat="server" id="dvCreate" class="w3-padding w3-margin w3-border" visible="false">
            <div class="w3-row">
                <asp:Button Text="Save" runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="buttonStyle w3-blue" />
                <asp:Button Text="Cancel" runat="server" ID="btnCancel" OnClick="btnCancel_Click" CssClass="buttonStyle w3-red" />
            </div>
            <div class="w3-padding-top w3-row">
                <asp:Table runat="server" CssClass="w3-table">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label Text="Request No :" runat="server" CssClass="labelStyle" />
                            <asp:TextBox runat="server" ID="txtRequestNo" CssClass="w3-input w3-border textBoxBorderRadius"></asp:TextBox>
                        </asp:TableCell><asp:TableCell>
                            <asp:Label Text="Date :" runat="server" CssClass="labelStyle" />
                            <asp:TextBox runat="server" ID="txtDate" CssClass="w3-input w3-border textBoxBorderRadius" TextMode="Date"></asp:TextBox>
                        </asp:TableCell><asp:TableCell>
                            <asp:Label Text="Reference No :" runat="server" CssClass="labelStyle" />
                            <asp:TextBox runat="server" ID="txtRefNo" CssClass="w3-input w3-border textBoxBorderRadius"></asp:TextBox>
                        </asp:TableCell></asp:TableRow><asp:TableRow>
                        <asp:TableCell>
                            <asp:Label Text="Requestor :" runat="server" CssClass="labelStyle" />
                            <asp:TextBox runat="server" ID="txtRequestor" CssClass="w3-input w3-border textBoxBorderRadius"></asp:TextBox>
                        </asp:TableCell><asp:TableCell>
                            <asp:Label Text="Prepared By :" runat="server" CssClass="labelStyle" />
                            <asp:TextBox runat="server" ID="txtPreparedBy" CssClass="w3-input w3-border textBoxBorderRadius"></asp:TextBox>
                        </asp:TableCell><asp:TableCell>
                            <asp:Label Text="PO / CM Number :" runat="server" CssClass="labelStyle" />
                            <asp:TextBox runat="server" ID="txtPOCMNum" CssClass="w3-input w3-border textBoxBorderRadius"></asp:TextBox>
                        </asp:TableCell></asp:TableRow><asp:TableRow>
                        <asp:TableCell ColumnSpan="3">
                            <asp:Label Text="Remarks" runat="server" CssClass="labelStyle" />
                            <asp:TextBox runat="server" ID="txtRemarks" CssClass="w3-input w3-border textBoxBorderRadius" TextMode="MultiLine" Style="width: 100%; height: 75px; resize: none"></asp:TextBox>
                        </asp:TableCell></asp:TableRow></asp:Table></div><div class="w3-padding-top w3-row">
                <div>
                    <asp:Button runat="server" ID="btnAddItemModal" Text="Add New Item" CssClass="buttonStyle w3-green" OnClick="btnAddItemModal_Click" />
                </div>
                <div>
                    <asp:GridView ID="gvTempGv" runat="server" OnSelectedIndexChanged="gvTempGv_SelectedIndexChanged"
                        OnRowDataBound="gvTempGv_RowDataBound" BorderStyle="None" GridLines="Horizontal" CellPadding="5" Font-Size="14px" Width="100%"
                        EmptyDataText="No records has been added." AutoGenerateColumns="true" class="table table-hover">
                        <HeaderStyle CssClass="w3-blue" />
                        <PagerStyle CssClass="pagination-ys" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <div id="dvMenu" class="w3-modal w3-round" runat="server" visible="false" style="z-index: 1400; background-color: transparent black; display: block; padding-top: 5%">
        <div class="w3-modal-content w3-card-4 w3-round" style="width: 34%">
            <div class="modal-header w3-blue">
                <div>
                    <h4 class="modal-title"><b>Inventory Transactions Menu</b></h4></div><div>
                    <asp:Button runat="server" OnClick="btnCloseMenu_Click" ID="btnCloseMenu" class="w3-large w3-display-topright w3-blue" Style="border: none" Text="X" />
                </div>
            </div>
            <div class="modal-body">
                <table>
                    <tr>
                        <td>
                            <h5>
                                <asp:LinkButton runat="server" href="SalesReturn.aspx" CssClass="buttonStyle" Font-Underline="false">Sales Return</asp:LinkButton></h5></td><td>
                            <h5>
                                <asp:LinkButton runat="server" href="CostAdjustment.aspx" CssClass="buttonStyle" Font-Underline="false">Cost Adjustment</asp:LinkButton></h5></td><td>
                            <h5>
                                <asp:LinkButton runat="server" href="StockTransfers.aspx" CssClass="buttonStyle" Font-Underline="false">Stock Transfer</asp:LinkButton></h5></td></tr><tr>
                        <td>
                            <h5>
                                <asp:LinkButton runat="server" href="InventoryHistory.aspx" CssClass="buttonStyle" Font-Underline="false">Inventory History - Inquiry</asp:LinkButton></h5></td><td>
                            <h5>
                                <asp:LinkButton runat="server" href="InventoryReports.aspx" CssClass="buttonStyle" Font-Underline="false">Inventory Reports</asp:LinkButton></h5></td><td>
                            <h5>
                                <asp:LinkButton runat="server" href="UpdateForex.aspx" CssClass="buttonStyle" Font-Underline="false">Update Forex</asp:LinkButton></h5></td></tr><tr>
                        <td>
                            <h5>
                                <asp:LinkButton runat="server" href="MIR.aspx" CssClass="buttonStyle" Font-Underline="false">MIR</asp:LinkButton></h5></td><td>
                            <h5>
                                <asp:LinkButton runat="server" href="MIR.aspx" CssClass="buttonStyle" Font-Underline="false">MIR</asp:LinkButton></h5></td><td>
                            <h5>
                                <asp:LinkButton runat="server" href="RTS.aspx" CssClass="buttonStyle" Font-Underline="false">RTS</asp:LinkButton></h5></td></tr></table></div></div></div>
    <div id="dvAddNewProduct" class="modal" runat="server" visible="false" style="display: block; z-index: 1000; background-color: rgba(0, 0, 0, 0.37); overflow-y: auto;">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" runat="server" id="btnCloseDvAddNewProduct" onserverclick="btnCloseDvAddNewProduct_ServerClick">&times;</button><h4 class="modal-title">Item List : </h4></div><div class="modal-body">
                    <div class="w3-animate-opacity w3-margin-0">
                        <div class="w3-col s8 w3-padding-bottom w3-left" style="vertical-align: central;">
                            <asp:Table ID="Table1" CellPadding="0" CellSpacing="0" runat="server" Width="100%" Style="border: 1px solid #2196F3">
                                <asp:TableRow>
                                    <asp:TableCell Width="20%">
                                        <asp:TextBox ID="txtSearchItem" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="Search Item Number" Style="width: 100%"></asp:TextBox>
                                    </asp:TableCell><asp:TableCell Width="5%">
                                        <asp:LinkButton runat="server" Style="width: 100%;" ID="btnSearchItem" Text="Search" CssClass="w3-btn w3-green ButtonStyle w3-right" OnClick="btnSearchItem_Click"><i class="glyphicon glyphicon-search"></i></asp:LinkButton>
                                    </asp:TableCell></asp:TableRow></asp:Table></div></div><asp:GridView ID="grvItemMaster" OnSelectedIndexChanged="grvItemMaster_SelectedIndexChanged"
                        OnPageIndexChanging="grvItemMaster_PageIndexChanging" OnRowDataBound="grvItemMaster_RowDataBound"
                        runat="server" AllowPaging="true" BorderStyle="None" GridLines="Horizontal" CellPadding="5" Font-Size="14px" Width="100%"
                        EmptyDataText="No records has been added." AutoGenerateColumns="true" class="table table-hover">
                        <HeaderStyle CssClass="w3-blue" />
                        <PagerStyle CssClass="pagination-ys" />
                    </asp:GridView>
                </div>
                <div class="modal-footer">
                    <asp:Table runat="server">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Button ID="btnCancelAddNewProduct" class="btn btn-default navbar-left" runat="server" OnClick="btnCancelAddNewProduct_Click" Text="Cancel" />
                            </asp:TableCell></asp:TableRow></asp:Table></div></div></div></div>
    <div id="dvItemDetails" class="modal" runat="server" visible="false" style="display: block; z-index: 1000; background-color: rgba(0, 0, 0, 0.37); overflow-y: auto;">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" runat="server" id="btnCloseItemDetails" onserverclick="btnCloseItemDetails_ServerClick">&times;</button><h4 class="modal-title">Enter Item Details : </h4></div><div class="modal-body">
                    <div>
                        <label style="font-size: small">Item Number</label> <asp:TextBox ID="txtItemNumber" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox></div><div>
                        <label style="font-size: small">Quantity</label>&nbsp;<label class="requiredColor">*</label> <asp:TextBox ID="txtQuantity" runat="server" onkeypress="return isNumberKey(event)" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small"></asp:TextBox></div><div>
                        <label style="font-size: small">Cost</label>&nbsp;<label class="requiredColor">*</label> <asp:TextBox ID="txtCost" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small"></asp:TextBox></div><div>
                        <label style="font-size: small">Upload Serial</label> <asp:FileUpload runat="server" class="w3-input w3-border textBoxBorderRadius" ID="fuUploadSerial" />
                    </div>
                    <div>
                        <asp:GridView ID="gvSerialList" runat="server" BorderStyle="None"  Visible="false"
                            GridLines="Horizontal" CellPadding="5" Font-Size="14px" Width="100%"
                            EmptyDataText="No records has been added." AutoGenerateColumns="true" class="table table-hover">
                            <HeaderStyle CssClass="w3-blue" />
                        </asp:GridView>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Table runat="server">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Button ID="btnAddToTempGv" runat="server" AutoPostBack="true" OnClick="btnAddToTempGv_Click" class="btn btn-primary navbar-left" data-toggle="modal" data-target="#myModal" Text="Add" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Button ID="btnCancelItemDetails" class="btn btn-default navbar-left" runat="server" OnClick="btnCancelItemDetails_Click" Text="Cancel" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Button ID="btnRemoveItemDetail" class="btn btn-default navbar-left w3-red" runat="server" OnClick="btnRemoveItemDetail_Click" Text="Remove" Visible="false" />
                            </asp:TableCell></asp:TableRow></asp:Table></div></div></div></div>

</asp:Content>