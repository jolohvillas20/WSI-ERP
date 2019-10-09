<%@ Page Title="ERP - Inventory"
    Language="C#"
    Async="true"
    EnableSessionState="True"
    AutoEventWireup="true"
    MasterPageFile="~/Site Master/SiteMaster.Master"
    CodeBehind="Inventory.aspx.cs"
    Inherits="SHIELD.Inventory.Inventory"
    EnableEventValidation="false"
    ValidateRequest="false"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../css/accordion.css" rel="stylesheet" />
    <script src="../../Scripts/accordionajax.txt"></script>
    <script src="../../Scripts/accordionbootstrap.txt"></script>
    <link href="../../css/button.css" rel="stylesheet" />
    <link href="../../css/Comments.css" rel="stylesheet" />
    <link href="../../css/bootstrap.css" rel="stylesheet" />
    <script src="../../Scripts/moment.js"></script>
    <script src="../../Scripts/bootstrap-datetimepicker.js"></script>
    <link href="../../css/bootstrap-datetimepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap-datetimepicker.min.js"></script>
    <link rel="stylesheet" href="../../css/w3.css" />

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
            background-image: url('../../Images/plus_30.png');
            background-repeat: no-repeat;
            height: 30px;
            width: 30px;
        }

        .searchButtonStyle {
            border: none;
            background-color: transparent;
            background-image: url('../../Images/search_30.png');
            background-repeat: no-repeat;
            height: 30px;
            width: 30px;
        }

        .deleteButtonStyle {
            border: none;
            background-color: transparent;
            background-image: url('../../Images/delete-sign_25.png');
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
    <script src="../../Script/jquery_3.3.1_jquery.min.js"></script>
    <script src="../../Script/bootstrap_3.3.7_js_bootstrap.min.js"></script>
</asp:Content>
<%-- class="panel panel-default card"--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container" id="dvMain">
        <asp:Label runat="server" ID="lblClick" class="labelStyle w3-padding" Style="font-size: 25px"></asp:Label>
        <div>
            <div class="separator1" runat="server" id="dvProducts">
                <%--<asp:UpdatePanel runat="server">
                    <ContentTemplate>--%>
                <div class="panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading headerProd">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" id="lnkProd" href="#colProds"><b>Products</b></a>
                            </h4>
                        </div>
                        <div id="colProds" class="panel-collapse " data-parent="dvMain">
                            <div class="panel-body">
                                <div class="wholeBody">
                                    <div class="searchHeader">
                                        <div class="searchLeft">
                                            <asp:TextBox Style="float: left" class="w3-input w3-border textBoxBorderRadius" runat="server" ID="txtProductSearch" Placeholder="Search Product"></asp:TextBox>
                                            <asp:Button Style="float: left" runat="server" class="searchButtonStyle " ID="btnSearchProduct" OnClick="btnSearchProduct_Click" />
                                        </div>
                                        <div class="searchRight" align="right">
                                            <asp:Button runat="server" class="addButtonStyle" ID="btnShowAddNewProduct" OnClick="btnShowAddNewProduct_Click" Style="float: right;" />
                                        </div>
                                    </div>
                                    <div>
                                        <asp:GridView ID="grvProducts" runat="server" OnSelectedIndexChanged="grvProducts_SelectedIndexChanged"
                                            OnRowDataBound="grvProducts_RowDataBound" OnPageIndexChanging="grvProducts_PageIndexChanging" PageSize="10"
                                            AllowPaging="true" BorderStyle="None" GridLines="Horizontal" CellPadding="5" Font-Size="14px" Width="100%"
                                            OnRowUpdating="grvProducts_RowUpdating" OnRowEditing="grvProducts_RowEditing" OnRowCancelingEdit="grvProducts_RowCancelingEdit"
                                            EmptyDataText="No records has been added." AutoGenerateColumns="false" class="table table-hover">
                                            <HeaderStyle CssClass="w3-blue" />
                                            <PagerStyle CssClass="pagination-ys" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="idClass">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblidClass" runat="server" Text='<%# Eval("idClass") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblidClass" runat="server" Text='<%# Eval("idClass") %>'></asp:Label>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Product Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProduct" runat="server" Text='<%# Eval("Product_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox class="textBoxBorderRadius" ID="txtProduct" runat="server" Text='<%# Eval("Product_Name") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:CommandField ButtonType="Link" ShowEditButton="true" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--</ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="grvProducts" />
                        <asp:PostBackTrigger ControlID="btnSearchProduct" />
                        <asp:PostBackTrigger ControlID="btnShowAddNewProduct" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </div>
            <div class="separator1" runat="server" id="dvSubProducts">
                <%--<asp:UpdatePanel runat="server">
                    <ContentTemplate>--%>
                <div class="panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading headerSub">
                            <h4 class="panel-title" style="position: sticky">
                                <a data-toggle="collapse" href="#colSub" id="lnkSub"><b>Sub - Products</b></a>
                            </h4>
                        </div>
                        <div id="colSub" class="panel-collapse " data-parent="dvMain">
                            <div class="panel-body">
                                <div class="wholeBody">
                                    <div class="searchHeader">
                                        <div class="searchLeft">
                                            <asp:TextBox Style="float: left" class="w3-input w3-border textBoxBorderRadius" runat="server" ID="txtSubProductSearch" Placeholder="Search Sub-Product"></asp:TextBox>
                                            <asp:Button Style="float: left" runat="server" class="searchButtonStyle" ID="btnSearchSubProduct" OnClick="btnSearchSubProduct_Click" />
                                        </div>
                                        <div class="searchRight" align="right">
                                            <asp:Button Style="float: right" runat="server" class="addButtonStyle" ID="btnAddNewSubProduct" OnClick="btnAddNewSubProduct_Click" />
                                        </div>
                                    </div>
                                    <div>
                                        <asp:GridView ID="grvSubProducts" runat="server" OnSelectedIndexChanged="grvSubProducts_SelectedIndexChanged"
                                            OnRowDataBound="grvSubProducts_RowDataBound" OnPageIndexChanging="grvSubProducts_PageIndexChanging" PageSize="10"
                                            AllowPaging="true" BorderStyle="None" GridLines="Horizontal" CellPadding="5" Font-Size="14px" Width="100%"
                                            OnRowUpdating="grvSubProducts_RowUpdating" OnRowEditing="grvSubProducts_RowEditing" OnRowCancelingEdit="grvSubProducts_RowCancelingEdit"
                                            EmptyDataText="No records has been added." AutoGenerateColumns="false" class="table table-hover">
                                            <HeaderStyle CssClass="w3-blue" />
                                            <PagerStyle CssClass="pagination-ys" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="idSubClass">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblidSubClass" runat="server" Text='<%# Eval("idSubClass") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblidSubClass" runat="server" Text='<%# Eval("idSubClass") %>'></asp:Label>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Subclass Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubclassName" runat="server" Text='<%# Eval("Subclass_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox class="textBoxBorderRadius" ID="txtSubclassName" runat="server" Text='<%# Eval("Subclass_Name") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:CommandField ButtonType="Link" ShowEditButton="true" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--</ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="grvProducts" />
                        <asp:PostBackTrigger ControlID="grvSubProducts" />
                        <asp:PostBackTrigger ControlID="btnSearchSubProduct" />
                        <asp:PostBackTrigger ControlID="btnAddNewSubProduct" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </div>
        </div>
        <div class="separator2" runat="server" id="dvItems">
            <%--<asp:UpdatePanel runat="server">
                <ContentTemplate>--%>
            <div class="panel-group">
                <div class="panel panel-default">
                    <div class="panel-heading headerItem">
                        <h4 class="panel-title">
                            <a data-toggle="collapse" href="#colItem" id="lnkItem"><b>Product Items</b></a>
                        </h4>
                    </div>
                    <div id="colItem" class="panel-collapse " data-parent="dvMain">
                        <div class="panel-body">
                            <div class="wholeBody">
                                <div class="searchHeader">
                                    <div class="searchLeft">
                                        <asp:TextBox Style="float: left" class="w3-input w3-border textBoxBorderRadius" runat="server" ID="txtProductItemSearch" Placeholder="Search Item Number"></asp:TextBox>
                                        <asp:Button Style="float: left" runat="server" class="searchButtonStyle" ID="btnSearchItem" OnClick="btnSearchItem_Click" />
                                    </div>
                                    <div class="searchRight" align="right">
                                        <asp:Button Style="float: right" runat="server" class="addButtonStyle" ID="btnAddNewProductItem" OnClick="btnAddNewProductItem_Click" />
                                    </div>
                                </div>
                                <div>
                                    <asp:GridView ID="grvProductItem" runat="server" OnSelectedIndexChanged="grvProductItem_SelectedIndexChanged"
                                        OnRowDataBound="grvProductItem_RowDataBound" OnPageIndexChanging="grvProductItem_PageIndexChanging"
                                        PageSize="10" class="table table-hover"
                                        AllowPaging="true" BorderStyle="None" GridLines="Horizontal" CellPadding="5" Font-Size="14px" Width="100%"
                                        EmptyDataText="No records has been added.">
                                        <PagerStyle CssClass="pagination-ys" />
                                        <HeaderStyle CssClass="w3-blue" />
                                        <%--<Columns> OnRowCommand="grvProductItem_RowCommand" 
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnViewDetails" class="buttonStyle buttonStyle-default" runat="server" CausesValidation="false" CommandName="ViewDetails"
                                                        Text="View" OnClick="btnViewDetails_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>--%>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--</ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grvProductItem" />
                    <asp:PostBackTrigger ControlID="grvSubProducts" />
                    <asp:PostBackTrigger ControlID="btnSearchItem" />
                    <asp:PostBackTrigger ControlID="btnAddNewProductItem" />
                </Triggers>
            </asp:UpdatePanel>--%>
        </div>
        <div class="separator2" runat="server" id="dvSerial">
            <%--<asp:UpdatePanel runat="server">
                <ContentTemplate>--%>
            <div class="panel-group">
                <div class="panel panel-default">
                    <div class="panel-heading headerSerial">
                        <h4 class="panel-title">
                            <a data-toggle="collapse" href="#colSerial" id="lnkSerial"><b>Serial Numbers</b></a>
                        </h4>
                    </div>
                    <div id="colSerial" class="panel-collapse " data-parent="dvMain">
                        <div class="panel-body">
                            <div class="wholeBody">
                                <div class="searchHeader">
                                    <div class="searchLeft">
                                        <asp:TextBox Style="float: left" class="w3-input w3-border textBoxBorderRadius" runat="server" ID="txtPOSearch" Placeholder="Search PO Number"></asp:TextBox>
                                        <asp:Button Style="float: left" runat="server" class="searchButtonStyle" ID="btnPOSerial" OnClick="btnPOSerial_Click" />
                                        <asp:TextBox Style="float: left" class="w3-input w3-border textBoxBorderRadius" runat="server" ID="txtSerialSearch" Placeholder="Search Serial Number"></asp:TextBox>
                                        <asp:Button Style="float: left" runat="server" class="searchButtonStyle" ID="btnSearchSerial" OnClick="btnSearchSerial_Click" />
                                    </div>
                                    <div class="searchRight" align="right">
                                        <asp:Button Style="float: right" runat="server" class="addButtonStyle" ID="btnAddNewSerial" OnClick="btnAddNewSerial_Click" />
                                    </div>
                                </div>
                                <div>
                                    <asp:GridView ID="grvSerial" runat="server" OnSelectedIndexChanged="grvSerial_SelectedIndexChanged"
                                        OnRowDataBound="grvSerial_RowDataBound" OnPageIndexChanging="grvSerial_PageIndexChanging" PageSize="10"
                                        AllowPaging="true" BorderStyle="None" GridLines="Horizontal" CellPadding="5" Font-Size="14px" Width="100%"
                                        OnRowUpdating="grvSerial_RowUpdating"
                                        EmptyDataText="No records has been added." AutoGenerateColumns="true" class="table table-hover">
                                        <HeaderStyle CssClass="w3-blue" />
                                        <PagerStyle CssClass="pagination-ys" />
                                        <%--                                        <Columns>
                                            <asp:TemplateField HeaderText="idSerial">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblidSerial" runat="server" Text='<%# Eval("idSerial") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Serial No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSerial_No" runat="server" Text='<%# Eval("Serial_No") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PO Number">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPONumber" runat="server" Text='<%# Eval("PO_Number") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Time Stamp">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltimestamp" runat="server" Text='<%# Eval("timestamp") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>--%>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--</ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grvSerial" />
                    <asp:PostBackTrigger ControlID="grvProductItem" />
                    <asp:PostBackTrigger ControlID="btnSearchSerial" />
                    <asp:PostBackTrigger ControlID="btnAddNewSerial" />
                </Triggers>
            </asp:UpdatePanel>--%>
        </div>
        <%--<asp:HiddenField ID="paneProd" runat="server" />
        <asp:HiddenField ID="paneSub" runat="server" />
        <asp:HiddenField ID="paneItem" runat="server" />
        <asp:HiddenField ID="paneSerial" runat="server" />--%>

        <asp:Button ID="btnDownloadInventoryAging" CssClass="w3-btn w3-blue ButtonStyle" runat="server" Text="Inventory Report - Aging " OnClick="btnDownloadInventoryAging_Click" Visible="true" />

    </div>
    <%--  <script type="text/javascript">
        //$(document).ready(function () {
        //    $("[#lnkProd]").click(function () {
        //        var paneProd = $("[id*=paneProd]").val() != "" ? $("[id*=paneProd]").val() : "colProds";

        //        //Remove the previous selected Pane.
        //        $("#colProds.in").removeClass("in");

        //        //Set the selected Pane.
        //        $("#" + paneProd).collapse("show");

        //        //When Pane is clicked, save the ID to the Hidden Field.
        //        $(".panel-heading a").click(function () {
        //            $("[id*=paneProd]").val($(this).attr("href").replace("#", ""));
        //        });
        //    });
        //});
        /* document.getElementById("lnkProd").onclick = */
        $(document).ready(function () {
            $/*(".headerProd a").click*/(function () {
                var paneProd = $("[id*=paneProd]").val() != "" ? $("[id*=paneProd]").val() : "colProds";

                //Remove the previous selected Pane.
                //$("#colProds.in").removeClass("in");z

                //Set the selected Pane.
                $("#" + paneProd).collapse("show");

                //When Pane is clicked, save the ID to the Hidden Field.
                $(".headerProd a").click(function () {
                    $("[id*=paneProd]").val($(this).attr("href").replace("#", ""));
                });
            });
        });

        /* document.getElementById("lnkSub").onclick = */
        $(document).ready(function () {
            $/*(".headerSub a").click*/(function () {
                var paneSub = $("[id*=paneSub]").val() != "" ? $("[id*=paneSub]").val() : "colSub";

                //Remove the previous selected Pane.
                $("#colSub.in").removeClass("in");

                //Set the selected Pane.
                $("#" + paneSub).collapse("show");

                //When Pane is clicked, save the ID to the Hidden Field.
                $(".headerSub a").click(function () {
                    $("[id*=paneSub]").val($(this).attr("href").replace("#", ""));
                });
            });
        });

        /*document.getElementById("lnkItem").onclick = */
        $(document).ready(function () {
            $/*(".headerItem a").click*/(function () {
                var paneItem = $("[id*=paneItem]").val() != "" ? $("[id*=paneItem]").val() : "colItem";

                //Remove the previous selected Pane.
                $("#colItem.in").removeClass("in");

                //Set the selected Pane.
                $("#" + paneItem).collapse("show");

                //When Pane is clicked, save the ID to the Hidden Field.
                $(".headerItem a").click(function () {
                    $("[id*=paneItem]").val($(this).attr("href").replace("#", ""));
                });
            });
        });

        /*document.getElementById("lnkSerial").onclick = */
        $(document).ready(function () {
            $/*(".headerSerial a").click*/(function () {
                var paneSerial = $("[id*=paneSerial]").val() != "" ? $("[id*=paneSerial]").val() : "colSerial";

                //Remove the previous selected Pane.
                $("#colSerial.in").removeClass("in");

                //Set the selected Pane.
                $("#" + paneSerial)/*.collapse*/("show");

                //When Pane is clicked, save the ID to the Hidden Field.
                $(".headerSerial a").click(function () {
                    $("[id*=paneSerial]").val($(this).attr("href").replace("#", ""));
                });
            });
        });
    </script>--%>

    <div id="dvAddNewProduct" class="w3-modal w3-round" runat="server" visible="false" style="z-index: 1400; background-color: transparent black; display: block;">
        <div class="w3-modal-content w3-card-4 w3-round" style="width: 20%">
            <div class="w3-text-white w3-padding" style="background-color: #0A90CF;">

                <table runat="server" style="width: 100%">
                    <tr>
                        <td>
                            <h3 class="labelStyle2">Add New Product</h3>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="w3-padding">
                <asp:Table runat="server" CellPadding="2" CellSpacing="0" Style="width: 100%; text-align: left;">
                    <asp:TableRow>
                        <asp:TableCell>
                                <asp:Label runat="server" Text="Product Name" Font-Bold="true"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:TextBox class="w3-input w3-border textBoxBorderRadius" runat="server" ID="txtProductName"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>

                <br />
                <br />
                <asp:Button ID="btnSaveNewProduct" class="buttonStyle buttonStyle-default" runat="server" Text="Save" Font-Size="Smaller" Height="30px" OnClick="btnSaveNewProduct_Click" />
                &nbsp;
                <asp:Button ID="btnCancelSaveProduct" class="buttonStyle buttonStyle-default" runat="server" Text="Cancel" Font-Size="Smaller" Height="30px" OnClick="btnCancelSaveProduct_Click" />
            </div>
        </div>
    </div>

    <div id="dvAddNewSubProduct" class="w3-modal w3-round" runat="server" visible="false" style="z-index: 1400; background-color: transparent black; display: block;">
        <div class="w3-modal-content w3-card-4 w3-round" style="width: 20%">
            <div class="w3-text-white w3-padding" style="background-color: #0A90CF;">

                <table runat="server" style="width: 100%">
                    <tr>
                        <td>
                            <h3 class="labelStyle2">Add New Sub Product</h3>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="w3-padding">
                <asp:Table runat="server" CellPadding="2" CellSpacing="0" Style="width: 100%; text-align: left;">
                    <asp:TableRow>
                        <asp:TableCell Style="border-bottom: 1px solid black">
                            <asp:Label runat="server" ID="lblProductLink" Font-Bold="true"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label runat="server" Text="Sub Product"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:TextBox class="w3-input w3-border textBoxBorderRadius" runat="server" ID="txtSubProductName"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>

                <br />
                <br />
                <asp:Button ID="btnSaveNewSubProduct" class="buttonStyle buttonStyle-default" runat="server" Text="Save" Font-Size="Smaller" Height="30px" OnClick="btnSaveNewSubProduct_Click" />
                &nbsp;
                <asp:Button ID="btnCancelSaveSubProduct" class="buttonStyle buttonStyle-default" runat="server" Text="Cancel" Font-Size="Smaller" Height="30px" OnClick="btnCancelSaveSubProduct_Click" />
            </div>
        </div>
    </div>

    <div id="dvAddNewItem" class="w3-modal w3-round" runat="server" visible="false" style="z-index: 1400; background-color: transparent black; display: block;">
        <div class="w3-modal-content  w3-card-4 w3-round" style="width: 500px">
            <div class="w3-text-white w3-padding" style="background-color: #0A90CF;">
                <table runat="server" style="width: 100%">
                    <tr>
                        <td>
                            <h3 class="labelStyle2">Add New Item</h3>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="w3-padding">
                <asp:Table runat="server" CellPadding="2" CellSpacing="2" Style="width: 100%; text-align: left;">
                    <asp:TableHeaderRow>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblSubProductLink" Font-Bold="true" Font-Size="Large"></asp:Label>
                        </asp:TableCell>
                    </asp:TableHeaderRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label runat="server" Text="Item Number" Font-Bold="true"></asp:Label>                            
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" Text="Principal SKU" Font-Bold="true"></asp:Label>                            
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:TextBox class="w3-input w3-border textBoxBorderRadius" runat="server" ID="txtItemNumber"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox class="w3-input w3-border textBoxBorderRadius" runat="server" ID="txtPrincipal_SKU"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="2">
                            <asp:Label runat="server" Text="Description" Font-Bold="true"></asp:Label>                            
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="2">
                            <asp:TextBox class="w3-input w3-border textBoxBorderRadius" runat="server" ID="txtDescription" TextMode="MultiLine" Width="100%" Height="50px"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label runat="server" Text="Site" Font-Bold="true"></asp:Label>                            
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" Text="Product UM" Font-Bold="true"></asp:Label>                            
                        </asp:TableCell>
                        <asp:TableCell>
                            <%--<asp:Label runat="server" Text="Volume UM" Font-Bold="true"></asp:Label>--%>                            
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>

                        <asp:TableCell>
                            <asp:DropDownList runat="server" ID="ddlSite" class="w3-input w3-border textBoxBorderRadius"></asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList runat="server" ID="ddlUM" class="w3-input w3-border textBoxBorderRadius"></asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell>
                            <%--<asp:TextBox class="textBoxBorderRadius" runat="server" ID="txtVolume_UM"></asp:TextBox>--%>
                        </asp:TableCell>
                    </asp:TableRow>

                </asp:Table>

                <br />
                <br />
                <asp:Button ID="btnAddNewItem" class="buttonStyle buttonStyle-default" runat="server" Text="Save" Font-Size="Smaller" Height="30px" OnClick="btnAddNewItem_Click" />
                &nbsp;
                <asp:Button ID="btnCancelItem" class="buttonStyle buttonStyle-default" runat="server" Text="Cancel" Font-Size="Smaller" Height="30px" OnClick="btnCancelItem_Click" />
            </div>
        </div>
    </div>

    <div id="dvAddNewSerial" class="w3-modal w3-round" runat="server" visible="false" style="z-index: 1400; background-color: transparent black; display: block; padding-top: 5%">
        <div class="w3-modal-content w3-card-4 w3-round" style="width: 60%; height: 93%">
            <div class="w3-text-white w3-padding" style="background-color: #0A90CF;">
                <table runat="server" style="width: 100%">
                    <tr>
                        <td>
                            <h3 class="labelStyle2">Add New Serial of Product</h3>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="w3-padding">
                <asp:Label runat="server" ID="lblItemLink" Font-Bold="true" Font-Size="20px"></asp:Label>
                <br />
                <%--<div style="width: 40%; display: inline-block; float: left; height: 480px; overflow-y: auto; border-right: 1px solid black; padding-right: 10px">
                    <asp:GridView runat="server" ID="grvAdditionalCharges" AutoGenerateColumns="false" ShowFooter="true"
                        OnRowDataBound="grvAdditionalCharges_RowDataBound" class="table table-hover">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Table runat="server" Width="100%" CellPadding="0" CellSpacing="2">
                                        <asp:TableRow>
                                            <asp:TableCell Width="50%">
                                                <asp:Label runat="server" class="w3-input w3-border textBoxBorderRadius" Text="Additional Charge" CssClass="labelSytle" style="font:bold"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell Width="40%">
                                                <asp:Label runat="server" class="w3-input w3-border textBoxBorderRadius" Text="Cost"  CssClass="labelSytle" style="font:bold"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell Width="10%">
                                                <%--<asp:Label runat="server" class="w3-input w3-border textBoxBorderRadius" Text="Remove"  CssClass="labelSytle"></asp:Label>--%
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Table runat="server" Width="100%" CellPadding="0" CellSpacing="2">
                                        <asp:TableRow>
                                            <asp:TableCell Width="60%">
                                                <asp:DropDownList runat="server" ID="ddlAdditionalCharge" class="textBoxBorderRadius"></asp:DropDownList>
                                                <%--<asp:Label runat="server" ID="lblName"></asp:Label>--%
                                            </asp:TableCell>
                                            <asp:TableCell Width="30%">
                                                <asp:TextBox runat="server" class="w3-input w3-border textBoxBorderRadius" ID="txtCharges" Style="border-top: none; border-left: none; border-right: none; border-bottom: 2px solid black"></asp:TextBox>
                                            </asp:TableCell>
                                            <asp:TableCell Width="10%">
                                                <asp:LinkButton runat="server" class="textBoxBorderRadius " ID="lbtnDelete" CssClass="deleteButtonStyle" OnClick="lbtnDelete_Click" Height="25px" Width="25px"></asp:LinkButton><%--Height="25px" Width="25px" Style="background-image: url('https://img.icons8.com/material-outlined/25/000000/delete-sign.png'); background-repeat: no-repeat;"--%
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Table runat="server" Width="100%">
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:LinkButton runat="server" ID="lbtnAdd" CssClass="addButtonStyle" OnClick="lbtnAdd_Click1" Height="35px" Width="35px"></asp:LinkButton>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>--%>
                <div class="w3-padding" style="width: 60%; display: inline-block;">
                    <asp:Table runat="server" CellPadding="3" CellSpacing="1" Style="text-align: left; width: 100%">
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="3">
                            <asp:Label runat="server" Text="PO Number" Font-Bold="true"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="3">
                                <asp:TextBox runat="server" class="w3-input w3-border textBoxBorderRadius" ID="txtPONumberUpload" Style="display: inline-block; float: left"></asp:TextBox>
                                <asp:Button runat="server" ID="btnSearchPO" class="buttonStyle buttonStyle-default" Text="Search" Font-Size="Smaller" Height="30px" Style="display: inline-block; float: left" OnClick="btnSearchPO_Click" />
                                <asp:Button runat="server" ID="btnClear" class="buttonStyle buttonStyle-default" Text="Clear" Font-Size="Smaller" Height="30px" Style="display: inline-block; float: left" OnClick="btnClear_Click" />
                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableCell>
                                &nbsp
                            </asp:TableCell>
                            <asp:TableCell>
                               
                            </asp:TableCell>
                            <asp:TableCell>

                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label runat="server" Text="Total Additional Charges" Font-Bold="true"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label runat="server" Text="PO Amount (Total)" Font-Bold="true"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label runat="server" Text="PO Quantity (Total)" Font-Bold="true"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:TextBox runat="server" class="w3-input w3-border textBoxBorderRadius" ID="txtTotalAddCharges" ReadOnly="true"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox runat="server" class="w3-input w3-border textBoxBorderRadius" ID="txtPOAmount" ReadOnly="true"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox runat="server" class="w3-input w3-border textBoxBorderRadius" ID="txtPOQuantity" ReadOnly="true"></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label runat="server" Text="Currency" Font-Bold="true"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label runat="server" Text="Forex Rate" Font-Bold="true"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label runat="server" Text="Document No." Font-Bold="true"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:DropDownList runat="server" class="textBoxBorderRadius" ID="ddlCurrency">
                                    <asp:ListItem Value="0">---</asp:ListItem>
                                </asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox runat="server" class="w3-input w3-border textBoxBorderRadius" ID="txtForexRate" Text="0" ReadOnly="true"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox runat="server" class="w3-input w3-border textBoxBorderRadius" ID="txtDocNo"></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>

                        <%--<asp:TableRow Visible="false">
                            <asp:TableCell ColumnSpan="3">
                                <asp:Label runat="server" Text="Supplier" Font-Bold="true"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow Visible="false">
                            <asp:TableCell ColumnSpan="3">
                                <asp:TextBox runat="server" class="w3-input w3-border textBoxBorderRadius" ID="txtSupplier" Style="display: inline-block; float: left"></asp:TextBox>
                                <asp:Button ID="btnShowSupplierModal" class="buttonStyle buttonStyle-default" runat="server" Text="Search" Font-Size="Smaller" Height="30px" OnClick="btnShowSupplierModal_Click" />
                            </asp:TableCell>
                        </asp:TableRow>--%>

                        <asp:TableRow Style="border-bottom: 1px solid black">
                            <asp:TableCell>
                                &nbsp
                            </asp:TableCell>
                            <asp:TableCell>
                               
                            </asp:TableCell>
                            <asp:TableCell>

                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableCell>
                                &nbsp
                            </asp:TableCell>
                            <asp:TableCell>
                               
                            </asp:TableCell>
                            <asp:TableCell>

                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label runat="server" Text="Item Cost (Per Line Item)" Font-Bold="true"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label runat="server" Text="Item Quantity (Per Line Item)" Font-Bold="true"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                            <asp:Label runat="server" Text="Final Item Cost" Font-Bold="true"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:TextBox runat="server" class="w3-input w3-border textBoxBorderRadius" ID="txtItemCost" ReadOnly="true"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox runat="server" class="w3-input w3-border textBoxBorderRadius" ID="txtItemQuantity" ReadOnly="true"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox runat="server" class="w3-input w3-border textBoxBorderRadius" ID="txtFinalItemCost"></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label runat="server" Text="" Font-Bold="true"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label runat="server" Text="Partial Quantity" Font-Bold="true"></asp:Label>
                            </asp:TableCell>
                              <asp:TableCell>
                                <%--<asp:TextBox runat="server" class="w3-input w3-border textBoxBorderRadius" ID="TextBox3"></asp:TextBox>--%>
                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:CheckBox runat="server" class="w3-input w3-border textBoxBorderRadius" ID="cbxPartial" Text="Partial Recieving" OnCheckedChanged="cbxPartial_CheckedChanged" AutoPostBack="true" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox runat="server" class="w3-input w3-border textBoxBorderRadius" ID="txtPartialQuantity" ReadOnly="true"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell>
                                <%--<asp:TextBox runat="server" class="w3-input w3-border textBoxBorderRadius" ID="TextBox3"></asp:TextBox>--%>
                            </asp:TableCell>
                        </asp:TableRow>

                        <%--<asp:TableRow>
                            <asp:TableCell>
                            <asp:Label runat="server" Text="Compute By" Font-Bold="true"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                            <asp:Label runat="server" Text="Compute Unit Price by" Font-Bold="true"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:DropDownList runat="server" class="textBoxBorderRadius" ID="ddlPurchase">
                                    <asp:ListItem Value="0">---</asp:ListItem>
                                    <asp:ListItem Value="1">Purchase Amount</asp:ListItem>
                                    <asp:ListItem Value="2">Purchase Quantity</asp:ListItem>
                                </asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:DropDownList runat="server" class="textBoxBorderRadius" ID="ddlUnitPrice">
                                    <asp:ListItem Value="0">---</asp:ListItem>
                                    <asp:ListItem Value="1">FIFO</asp:ListItem>
                                    <asp:ListItem Value="2">AVE</asp:ListItem>
                                </asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button ID="btnCompute" class="buttonStyle buttonStyle-default" runat="server" Text="Compute" Font-Size="Smaller" Height="30px" OnClick="btnCompute_Click" />
                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableCell>
                                &nbsp
                            </asp:TableCell>
                            <asp:TableCell>
                               
                            </asp:TableCell>
                            <asp:TableCell>

                            </asp:TableCell>
                        </asp:TableRow>--%>

                        <asp:TableRow>
                            <asp:TableCell>
                            <asp:Label runat="server" Text="Input Mode" Font-Bold="true"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                            <asp:Label runat="server" Text="Serial No." Font-Bold="true"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:DropDownList runat="server" class="textBoxBorderRadius" ID="ddlSerialMode" OnSelectedIndexChanged="ddlSerialMode_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0">---</asp:ListItem>
                                    <asp:ListItem Value="1">Lot Serial</asp:ListItem>
                                    <asp:ListItem Value="2">File</asp:ListItem>
                                </asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                                <asp:TextBox runat="server" class="w3-input w3-border textBoxBorderRadius" ID="txtSerial" Visible="false"></asp:TextBox>
                                <asp:FileUpload runat="server" class="textBoxBorderRadius" ID="uploadExcel" Visible="false" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <br />
                <%--                <asp:Button runat="server" Text="Upload Excel File" ID="btnShowUploadModal" OnClick="btnShowUploadModal_Click" class="buttonStyle buttonStyle-default" />--%>
            </div>
            <div style="position: absolute; bottom: 0;" class="w3-padding">
                <asp:Button ID="btnSaveNewSerial" class="buttonStyle buttonStyle-default" runat="server" Text="Save" Font-Size="Smaller" Height="30px" OnClick="btnSaveNewSerial_Click" />
                &nbsp;
                <asp:Button ID="btnCancelNewSerial" class="buttonStyle buttonStyle-default" runat="server" Text="Cancel" Font-Size="Smaller" Height="30px" OnClick="btnCancelNewSerial_Click" />
            </div>
        </div>
    </div>

    <%--<div id="dvSupplier" class="w3-modal w3-round" runat="server" visible="false" style="z-index: 1500; background-color: transparent black; display: block;">
        <div class="w3-modal-content w3-card-4 w3-round" style="width: 50%">
            <div class="w3-text-white w3-padding" style="background-color: #0A90CF;">
                <table runat="server" style="width: 100%">
                    <tr>
                        <td>
                            <h3 class="labelStyle2">Add New Sub Product</h3>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="w3-padding">
                <div style="height: 30px">
                    <asp:TextBox runat="server" class="w3-input w3-border textBoxBorderRadius" ID="txtSearchSupplier" Style="display: inline-block; float: left"></asp:TextBox>
                    <asp:Button ID="btnSearchSupplier" class="buttonStyle buttonStyle-default" runat="server" Text="Search" Font-Size="Smaller" Height="30px" OnClick="btnSearchSupplier_Click" />
                </div>
                <div>
                    <asp:GridView ID="gvSupplier" runat="server" OnSelectedIndexChanged="gvSupplier_SelectedIndexChanged"
                        OnRowDataBound="gvSupplier_RowDataBound" OnPageIndexChanging="gvSupplier_PageIndexChanging" PageSize="10"
                        AllowPaging="true" BorderStyle="None" GridLines="Horizontal" CellPadding="5" Font-Size="14px" Width="100%"
                        EmptyDataText="No records has been added." AutoGenerateColumns="true" class="table table-hover">
                        <HeaderStyle CssClass="w3-blue" />
                        <PagerStyle CssClass="pagination-ys" />
                    </asp:GridView>
                </div>
                <div>
                    <br />
                    <br />
                    <asp:Button ID="btnCancelSupplier" class="buttonStyle buttonStyle-default" runat="server" Text="Cancel" Font-Size="Smaller" Height="30px" OnClick="btnCancelSupplier_Click" />
                </div>
            </div>
        </div>
    </div>--%>

    <%--    <div runat="server" id="dvUploadFile" visible="false" class="w3-modal w3-round" style="z-index: 1400; background-color: transparent black; display: block;">
        <div class="w3-modal-content  w3-card-4 w3-round" style="width: 80%">
            <div class="w3-text-white w3-padding" style="background-color: #0A90CF;">
                <table runat="server" style="width: 100%">
                    <tr>
                        <td>
                            <h3 class="labelStyle2">Add New Serial of Product</h3>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="w3-padding">
                <asp:Label runat="server" ID="lblItemLink2" Font-Bold="true"></asp:Label>
                <asp:Table runat="server" CellPadding="2" CellSpacing="0" Style="width: 100%; text-align: left;">
                    <asp:TableRow>
                        <asp:TableCell Style="border-bottom: 1px solid black" ColumnSpan="2">
                            
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label runat="server" Text="PO Number" Font-Bold="true"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:TextBox class="textBoxBorderRadius" runat="server" ID="txtPONumberUpload"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <br />
                <br />
                <asp:Button runat="server" ID="btnUploadExcel" Text="Upload" OnClick="btnUploadExcel_Click" class="buttonStyle buttonStyle-default" />
                &nbsp;
                <asp:Button runat="server" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" class="buttonStyle buttonStyle-default" />
            </div>
        </div>
    </div>--%>
</asp:Content>
