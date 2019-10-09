<%@ Page
    Title="Inventory Transactions - Stock Transfers"
    Language="C#"
    MasterPageFile="~/Site Master/SiteMaster.Master"
    AutoEventWireup="true"
    CodeBehind="StockTransfers.aspx.cs"
    Inherits="SOPOINV.Forms.StockTransfers"
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
           <%-- <h2 class="labelStyle2">Inventory Management</h2>
            <ul class="nav nav-tabs">
                <li><a href="SalesReturn.aspx">Sales Return</a></li>
                <li><a href="CostAdjustment.aspx">Cost Adjustment</a></li>
                <li class="active"><a href="#ST">Stock Transfer</a></li>
                <li><a href="InventoryHistory.aspx">Inventory History - Inquiry</a></li>
                <li><a href="InventoryReports.aspx">Inventory Reports</a></li>
                <li><a href="UpdateForex.aspx">Update Forex</a></li>
            </ul>--%>

            <div class="tab-content">
                <div id="ST" class="tab-pane fade in active" style="height: 530px;">
                    <div class="w3-padding">
                        <div class="searchHeader">
                            <div class="searchLeft">
                                <asp:TextBox Style="float: left" class="w3-input w3-border textBoxBorderRadius" runat="server" ID="txtSearchItemNumber" Placeholder="Item Number"></asp:TextBox>
                                <asp:TextBox Style="float: left" class="w3-input w3-border textBoxBorderRadius" runat="server" ID="txtSearchSerialNumber" Placeholder="Serial Number"></asp:TextBox>
                                <asp:Button Style="float: left" runat="server" class="searchButtonStyle " ID="btnSearchInput" OnClick="btnSearchInput_Click" />
                                <asp:Button Style="float: left" runat="server" class="textBoxBorderRadius " ID="btnClearST" CssClass="deleteButtonStyle" OnClick="btnClearST_Click" Height="30px" Width="30px"></asp:Button>
                                <asp:Button Style="float: left" ID="btnOpenModal" class="buttonStyle buttonStyle-default w3-green" runat="server" Text="Transfer" OnClick="btnOpenModal_Click" Visible="false" />
                                <asp:Button Style="float: left" ID="btnOpenModal2" class="buttonStyle buttonStyle-default w3-green" runat="server" Text="Transfer Bulk" OnClick="btnOpenModal2_Click" />
                            </div>
                        </div>

                        <div>
                            <asp:GridView ID="gvHistory" runat="server"
                                OnRowDataBound="gvHistory_RowDataBound" OnPageIndexChanging="gvHistory_PageIndexChanging" PageSize="10"
                                AllowPaging="true" BorderStyle="None" GridLines="Horizontal" CellPadding="5" Font-Size="14px" Width="100%"
                                EmptyDataText="No records has been added." AutoGenerateColumns="true" class="table table-hover">
                                <HeaderStyle CssClass="w3-blue" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <div id="dvStockTransfer" class="w3-modal w3-round" runat="server" visible="false" style="z-index: 1400; background-color: transparent black; display: block; padding-top: 5%">
                    <div class="w3-modal-content w3-card-4 w3-round" style="width: 30%">
                        <div class="modal-header w3-blue">
                            <h4 class="modal-title"><b>Stock Transfer > Select Site</b></h4>
                        </div>
                        <div class="modal-body">
                            <div runat="server" id="dvstockcode" class="w3-padding">
                                <asp:Label runat="server" Text="Last Stock Code :" Font-Bold="true"></asp:Label>
                                <asp:Label runat="server" ID="lblStockCode" Font-Bold="true"></asp:Label>
                            </div>
                            <div runat="server" id="dvuploadexcel" class="w3-padding">
                                <asp:Label runat="server" Text="Upload Excel :" Font-Bold="true"></asp:Label>
                                <asp:FileUpload runat="server" class="w3-input w3-border textBoxBorderRadius" ID="fuTransfer" />
                                <asp:Label runat="server" Text="Choose Source Site :" Font-Bold="true"></asp:Label>
                                <asp:DropDownList runat="server" ID="ddlSourceSite" class="w3-input w3-border TextBoxStyle"></asp:DropDownList>
                            </div>
                            <div class="w3-padding">
                                <asp:Label runat="server" Text="Choose Destination Site :" Font-Bold="true"></asp:Label>
                                <asp:DropDownList runat="server" ID="ddlDestinationSite" class="w3-input w3-border TextBoxStyle"></asp:DropDownList>
                                <asp:Label runat="server" Text="STR Number :" Font-Bold="true"></asp:Label>
                                <asp:TextBox runat="server" ID="txtSTRNumber" class="w3-input w3-border textBoxBorderRadius"></asp:TextBox>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnTransfer" class="buttonStyle buttonStyle-default w3-green" runat="server" Text="Transfer" Font-Size="Smaller" Height="30px" OnClick="btnTransfer_Click" />
                            &nbsp;
                            <asp:Button ID="btnCloseModal" class="buttonStyle buttonStyle-default" runat="server" Text="Cancel" Font-Size="Smaller" Font-Bold="true" Height="30px" OnClick="btnCloseModal_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div id="dvMenu" class="w3-modal w3-round" runat="server" visible="false" style="z-index: 1400; background-color: transparent black; display: block; padding-top: 5%">
        <div class="w3-modal-content w3-card-4 w3-round" style="width:34%">
            <div class="modal-header w3-blue">
                <div>
                    <h4 class="modal-title"><b>Inventory Transactions Menu</b></h4>
                </div>
                <div>
                    <asp:Button runat="server" OnClick="btnCloseMenu_Click" ID="btnCloseMenu" class="w3-large w3-display-topright w3-blue" Style="border: none" Text="X" />
                </div>
            </div>
            <div class="modal-body">
                <table>
                    <tr>
                        <td>
                            <h5>
                                <asp:LinkButton runat="server" href="SalesReturn.aspx" CssClass="buttonStyle" Font-Underline="false">Sales Return</asp:LinkButton></h5>
                        </td>
                        <td>
                            <h5>
                                <asp:LinkButton runat="server" href="CostAdjustment.aspx" CssClass="buttonStyle" Font-Underline="false">Cost Adjustment</asp:LinkButton></h5>
                        </td>
                        <td>
                            <h5>
                                <asp:LinkButton runat="server" href="StockTransfers.aspx" CssClass="buttonStyle" Font-Underline="false">Stock Transfer</asp:LinkButton></h5>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h5>
                                <asp:LinkButton runat="server" href="InventoryHistory.aspx" CssClass="buttonStyle" Font-Underline="false">Inventory History - Inquiry</asp:LinkButton></h5>
                        </td>
                        <td>
                            <h5>
                                <asp:LinkButton runat="server" href="InventoryReports.aspx" CssClass="buttonStyle" Font-Underline="false">Inventory Reports</asp:LinkButton></h5>
                        </td>
                        <td>
                            <h5>
                                <asp:LinkButton runat="server" href="UpdateForex.aspx" CssClass="buttonStyle" Font-Underline="false">Update Forex</asp:LinkButton></h5>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h5>
                                <asp:LinkButton runat="server" href="MIS.aspx" CssClass="buttonStyle" Font-Underline="false">MIS</asp:LinkButton></h5>
                        </td>
                        <td>
                            <h5>
                                <asp:LinkButton runat="server" href="MIR.aspx" CssClass="buttonStyle" Font-Underline="false">MIR</asp:LinkButton></h5>
                        </td>
                        <td>
                            <h5>
                                <asp:LinkButton runat="server" href="RTS.aspx" CssClass="buttonStyle" Font-Underline="false">RTS</asp:LinkButton></h5>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
