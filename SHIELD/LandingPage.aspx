<%@ Page Title="ERP - Landing Page"
    Language="C#"
    MasterPageFile="~/Site Master/SiteMaster.Master"
    AutoEventWireup="true" CodeBehind="LandingPage.aspx.cs"
    Inherits="SOPOINV.Forms.LandingPage" EnableEventValidation="false"
    ValidateRequest="false"
    MaintainScrollPositionOnPostback="true"
    Async="true"
    EnableSessionState="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/accordion.css" rel="stylesheet" />
    <script src="../Scripts/accordionajax.txt"></script>
    <script src="../Scripts/accordionbootstrap.txt"></script>
    <link href="../css/button.css" rel="stylesheet" />
    <link href="../css/Comments.css" rel="stylesheet" />
    <link href="../css/bootstrap.css" rel="stylesheet" />
    <script src="../Scripts/moment.js"></script>
    <script src="../Scripts/bootstrap-datetimepicker.js"></script>
    <link href="../css/bootstrap-datetimepicker.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap-datetimepicker.min.js"></script>
    <link rel="stylesheet" href="../css/w3.css" />

    <style>
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

        .right {
            text-align: right;
            margin-right: 1em;
        }

        .labelProp {
            font-size: 11px;
            margin: 0px;
            /*font-family: 'Segoe UI';*/
        }

        .borderRightOfTable {
            border-right: 1px solid #CCCCCC;
            /*padding-right:10px;*/
        }

        .borderBottom {
            border-bottom: 1px solid rgba(0, 0, 0, 0.19);
        }

        .card {
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
            transition: 0.3s;
        }

            .card:hover {
                box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
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

        .mainButtonSelectedStyle {
            border: 0px;
            background-color: #FEDC56;
            color: #000000;
            font-size: 14px;
            height: 40px;
            width: 150px;
            font-weight: 600;
            text-align: center;
            vertical-align: middle;
            /*font-weight:700;*/
        }

        .mainButtonNotSelectedStyle {
            border: 0px;
            background-color: white;
            color: #363637;
            font-size: 14px;
            height: 40px;
            width: 150px;
            font-weight: 600;
            text-align: center;
            vertical-align: middle;
            /*font-weight:700;*/
        }

            .mainButtonNotSelectedStyle:hover {
                background-color: rgba(0, 0, 0, 0.19);
            }

        .requiredColor {
            color: red;
            font-size: 12px;
        }

        .rowBorder {
            border-bottom: solid 1px #EEEEEE;
            padding-left: 10px;
            padding-right: 10px;
            font-size: 14px;
        }

        .rowHeaderBorder {
            border-bottom: solid 1px #0A91CF;
            padding-left: 10px;
            padding-right: 10px;
            font-size: 14px;
        }

        .modalLoader {
            position: fixed;
            z-index: 3000;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: rgba(0, 0, 0, 0.37);
            /*filter: alpha(opacity=60);*/
            /*opacity: 0.6;*/
            /*-moz-opacity: 0.8;*/
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

        .ulStyle {
            list-style-type: none;
            margin: 0;
            padding: 0;
            overflow: hidden;
        }

        .liStyle {
            float: left;
        }

        .divTransition {
            position: static;
            float: left;
        }

        .container {
            width: 100%;
            overflow: auto;
            padding: 10px 10px 10px 10px;
            background: #FFF;
            margin: auto;
        }
        /*
        .cube {
            float: left;
            background: #FFF;
            max-height: 500px;
            height: 500px;
            overflow-y: scroll;
            padding: 10px 10px 10px 10px;
            border: 5px solid #FFF;
        }*/
        .separator {
            float: left;
            background: #FFF;
            max-height: 600px;
            width: 25%;
            overflow-y: auto;
            padding: 5px 5px 5px 5px;
        }

        /*.separator2 {
            float: left;
            background: #FFF;
            max-height: 500px;
            width: 100%;
            overflow-y: auto;
            padding: 5px 5px 5px 5px;
        }*/

        .wholeBody {
            /*position: relative;*/
        }

        .searchHeader {
            position: sticky;
            background-color: white;
            top: -5px;
            z-index: 10;
            height: 30px;
        }

        .searchLeft {
            float: left;
            width: 90%;
        }

        .searchRight {
            float: right;
            width: 10%;
        }

        /*.contentBody {
            display: block;
            overflow-y: visible;
            max-height: 200px;
        }*/

        .addButtonStyle {
            border: none;
            background-color: transparent;
            background-image: url('../Images/plus_30.png');
            background-repeat: no-repeat;
            height: 30px;
            width: 30px;
        }

        .searchButtonStyle {
            border: none;
            background-color: transparent;
            background-image: url('../Images/search_30.png');
            background-repeat: no-repeat;
            height: 30px;
            width: 30px;
        }

        .deleteButtonStyle {
            border: none;
            background-color: transparent;
            background-image: url('../Images/delete-sign_25.png');
            background-repeat: no-repeat;
            height: 25px;
            width: 25px;
        }

        .headerItem {
        }

        .headerSerial {
        }
    </style>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <%--<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />--%>
    <script src="../Script/jquery_3.3.1_jquery.min.js"></script>
    <script src="../Script/bootstrap_3.3.7_js_bootstrap.min.js"></script>
</asp:Content>
<%-- class="panel panel-default card"--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container" id="dvMain" style="width: 100%; margin-top: 20px">
        <div class="separator" runat="server" id="dvItems">
            <div class="panel-group">
                <div class="panel panel-default">
                    <div class="panel-heading headerItem">
                        <div class="panel-title">
                            <center>
                                <button data-toggle="collapse" href="#colItem" style="border: none; background-color: transparent; background-image: url('../Images/warehouse_200.png'); background-repeat: no-repeat; height: 200px; width: 200px;"></button>
                                <br />
                                <H5>Inventory Management</H5>
                            </center>
                        </div>
                    </div>
                    <div id="colItem" class="panel-collapse collapse" data-parent="dvMain" style="text-align: center">
                        <div class="panel-body">
                            <div>
                                <a href="../Forms/InventoryManagement/Inventory.aspx">Inventory</a>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div>
                                <a href="../Forms/InventoryManagement/PickActivity.aspx">Pick Activity</a>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div>
                                <a href="../Forms/InventoryManagement/InventoryTransactions/LandingPage.aspx">Transactions</a>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div>
                                <%--../Forms/InventoryTransactions.aspx--%>
                                <a href="../Forms/OtherMaintenance.aspx">Other Maintenance</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="separator" runat="server" id="dvSerial">
            <%--<asp:UpdatePanel runat="server">
                <ContentTemplate>--%>
            <div class="panel-group">
                <div class="panel panel-default">
                    <div class="panel-heading headerSerial">
                        <div class="panel-title">
                            <center>
                                <button data-toggle="collapse" href="#colSerial" style="border: none; background-color: transparent; background-image: url('../Images/create_document_200.png'); background-repeat: no-repeat; height: 200px; width: 200px;"></button>
                                <br />
                                <H5>Sales Management</H5>
                            </center>
                        </div>
                    </div>
                    <div id="colSerial" class="panel-collapse collapse" data-parent="dvMain" style="text-align: center">
                        <div class="panel-body">
                            <div>
                                <a href="../Forms/SalesManagement/SOCreation.aspx">Sales Order</a>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div>
                                <a href="../Forms/SalesManagement/CustomerMaintenance.aspx">Customer Maintenance</a>
                            </div>
                        </div>
                          <div class="panel-body">
                            <div>
                                <a href="../Forms/SalesManagement/InvoiceManagement.aspx">Invoice Management</a>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div>
                                <a href="https://lsoms.wordtext.ph:94/Login.aspx">SOMS</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="separator" runat="server" id="Div1">
            <%--<asp:UpdatePanel runat="server">
                <ContentTemplate>--%>
            <div class="panel-group">
                <div class="panel panel-default">
                    <div class="panel-heading headerSerial">
                        <div class="panel-title">
                            <center>
                                <button data-toggle="collapse" href="#colPurchasing" style="border: none; background-color: transparent; background-image: url('../Images/bill_200.png'); background-repeat: no-repeat; height: 200px; width: 200px;"></button>
                                <br />
                                <H5>Purchasing</H5>
                            </center>
                        </div>
                    </div>
                    <div id="colPurchasing" class="panel-collapse collapse" data-parent="dvMain" style="text-align: center">
                        <div class="panel-body">
                            <div>
                                <a href="../Forms/Purchasing/POCreation.aspx">Purchase Order</a>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div>
                                <a href="../Forms/Purchasing/ImportShipment.aspx">Import Shipment</a>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div>
                                <a href="../Forms/Purchasing/Announcement.aspx">Announcement</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="separator" runat="server" id="dvAccounting">
            <%--<asp:UpdatePanel runat="server">
                <ContentTemplate>--%>
            <div class="panel-group">
                <div class="panel panel-default">
                    <div class="panel-heading headerSerial">
                        <div class="panel-title">
                            <center>
                                <button data-toggle="collapse" href="#colAccounting" style="border: none; background-color: transparent; background-image: url('../Images/accounting-200.png'); background-repeat: no-repeat; height: 200px; width: 200px;"></button>
                                <br />
                                <H5>Accounting</H5>
                            </center>
                        </div>
                    </div>
                    <div id="colAccounting" class="panel-collapse collapse" data-parent="dvMain" style="text-align: center">
                        <div class="panel-body">
                            <div>
                                <a href="../Forms/Accounting/Payments.aspx">Payments</a>
                            </div>
                        </div>                       
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

