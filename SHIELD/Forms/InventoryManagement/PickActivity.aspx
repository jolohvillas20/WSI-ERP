<%@ Page
    Title="ERP - Pick Activity"
    Language="C#"
    Async="true"
    EnableSessionState="True"
    AutoEventWireup="true"
    MasterPageFile="~/Site Master/SiteMaster.Master"
    CodeBehind="PickActivity.aspx.cs"
    Inherits="SHIELD.Forms.PickActivity"
    EnableEventValidation="false"
    ValidateRequest="false"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link rel="stylesheet" href="../../css/bootstrap.min.css" />
    <link rel="stylesheet" href="../../css/w3.css" />
    <link rel="stylesheet" href="../../Content/bootstrap-datetimepicker.css" />
    <script src="../../js/jquery.min.js"></script>

    <link href="../../css/DatePicker/bootstrap-datepicker3.min.css" rel="stylesheet" />

    <script src="../../css/DatePicker/bootstrap-datepicker.js"></script>
    <script src="../../css/DatePicker/bootstrap-datepicker.min.js"></script>

    <script type="text/javascript" src="../../scripts/moment.min.js"></script>
    <script type="text/javascript" src="../../scripts/bootstrap-datetimepicker.js"></script>


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

        .searchButtonStyle {
            border: none;
            background-color: transparent;
            background-image: url('../../Images/search_30.png');
            background-repeat: no-repeat;
            height: 30px;
            width: 30px;
        }
    </style>
    <%--<script type="text/javascript">
        function SelectAll(CheckBoxControl) {
            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {

                    if ((document.forms[0].elements[i].type == 'checkbox') &&

                        (document.forms[0].elements[i].name.indexOf('gvItems') > -1)) {
                        if (document.forms[0].elements[i].disabled) {
                            document.forms[0].elements[i].checked = false;
                        }
                        else {
                            document.forms[0].elements[i].checked = true;
                        }
                    }
                }
            }
            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {

                    if ((document.forms[0].elements[i].type == 'checkbox') &&

                        (document.forms[0].elements[i].name.indexOf('gvItems') > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
        }
    </script>--%>
    <%--    <script type="text/javascript">
<!--
    function Check_Click(objRef) {
        //Get the Row based on checkbox
        var row = objRef.parentNode.parentNode;

        //Get the reference of GridView
        var GridView = row.parentNode;

        //Get all input elements in Gridview
        var inputList = GridView.getElementsByTagName("input");

        for (var i = 0; i < inputList.length; i++) {
            //The First element is the Header Checkbox
            var headerCheckBox = inputList[0];

            //Based on all or none checkboxes
            //are checked check/uncheck Header Checkbox
            var checked = true;
            if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                if (!inputList[i].checked) {
                    checked = false;
                    break;
                }
            }
        }
        headerCheckBox.checked = checked;

    }
    function checkAll(objRef) {
        var GridView = objRef.parentNode.parentNode.parentNode;
        var inputList = GridView.getElementsByTagName("input");
        for (var i = 0; i < inputList.length; i++) {
            var row = inputList[i].parentNode.parentNode;
            if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                if (objRef.checked) {
                    inputList[i].checked = true;
                }
                else {
                    if (row.rowIndex % 2 == 0) {
                        row.style.backgroundColor = "#C2D69B";
                    }
                    else {
                        row.style.backgroundColor = "white";
                    }
                    inputList[i].checked = false;
                }
            }
        }
    }
//-->
    </script>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

    <asp:Panel ID="pnlButtons" runat="server" Visible="true" CssClass="w3-panel w3-margin-0">
        <div class="w3-col s8">
            <asp:Button ID="btnBack" CssClass="w3-btn w3-green ButtonStyle" runat="server" Text="Back" OnClick="btnBack_Click" Visible="false" />
        </div>
        <div class="w3-animate-opacity w3-margin-0">
            <div class="w3-col w3-padding-0" style="vertical-align: central;">
                <asp:Table ID="tblSearch" CellPadding="0" CellSpacing="0" runat="server">
                    <asp:TableRow>
                        <asp:TableCell >
                             <label runat="server" style="font-size: small">SO Status : </label>
                        </asp:TableCell>
                        <asp:TableCell >
                            <asp:DropDownList runat="server" ID="ddlSOStatus" OnSelectedIndexChanged="ddlSOStatus_SelectedIndexChanged" AutoPostBack="true" class="w3-input w3-border TextBoxStyle" >
                                <asp:ListItem Value="" Text="All"></asp:ListItem>
                                <asp:ListItem Value="Open" Text="Open"></asp:ListItem>
                                <asp:ListItem Value="Closed" Text="Closed"></asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell >
                            <asp:TextBox ID="txtSearchSO" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="Enter S.O. Number"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell Width="10%">
                            <asp:LinkButton runat="server" Style="width: 100%;" ID="btnSearch" Text="Search" CssClass="w3-btn w3-green ButtonStyle w3-right" OnClick="btnSearch_Click"><i class="glyphicon glyphicon-search"></i></asp:LinkButton>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel runat="server" ID="SOPanel" CssClass="w3-panel" Style="margin-top: 0px" Visible="true">
        <div class="w3-white w3-card-4" runat="server">
            <asp:GridView ID="grvMainSO" runat="server" CssClass="w3-table w3-hoverable w3-round-small" AutoGenerateColumns="True"
                OnSelectedIndexChanged="grvMainSO_SelectedIndexChanged" OnRowDataBound="grvMainSO_RowDataBound"
                OnPageIndexChanging="grvMainSO_PageIndexChanging" ShowHeaderWhenEmpty="True" AllowPaging="True"
                PageSize="10" Font-Size="14px" PagerStyle-BorderStyle="None" PagerStyle-Height="20px">
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
                            </font> 
                        </center>
                </EmptyDataTemplate>
                <PagerSettings Mode="NumericFirstLast" NextPageText="Next" />
                <PagerStyle CssClass="Pager" />
            </asp:GridView>
        </div>
    </asp:Panel>

    <div runat="server" id="pnlAllDetails" visible="false">
        <asp:Panel runat="server" ID="pnlSOHeader" CssClass="w3-panel ">
            <div class="w3-row w3-card-4 w3-white w3-padding-bottom w3-round-small">
                <%--<div class="w3-col s12 w3-padding-left">
                    <h5 class="w3-text-black w3-leftbar">&nbsp;Search SO Details</h5>
                </div>--%>
                <div class="w3-quarter w3-padding-right w3-padding-left w3-padding-bottom" style="width: 100%">
                    <div style="width: 50%; display: inline-block; float: left">
                        <div>
                            <%--<label runat="server" style="font-size: small">SO Number : </label>--%>
                        </div>
                        <div>
                            <%--<asp:TextBox ID="txtSearchSO" runat="server" CssClass="w3-input w3-border TextBoxStyle" Width="200px" Style="display: inline-block; float: left"></asp:TextBox>
                            &nbsp&nbsp&nbsp
                            <asp:Button ID="btnSearch" CssClass="searchButtonStyle" runat="server" OnClick="btnSearch_Click" Style="float: left; display: inline-block" />--%>
                        </div>
                    </div>
                    <div style="width: 50%; display: inline-block; float: right; display: flex; justify-content: flex-end">
                        <label runat="server" style="font-size: small">Pick Number : </label>
                        <asp:Label runat="server" Style="font-size: small" ID="lblPickNumber"></asp:Label>
                    </div>
                </div>

                <div class="w3-col s12 w3-padding-left">
                    <h5 class="w3-text-black w3-leftbar">&nbsp;SO Details</h5>
                </div>
                <div class="w3-half w3-padding-right w3-padding-left w3-padding-bottom">
                     <div>
                        <label style="font-size: small">SO Number</label>
                        <asp:TextBox ID="txtSONumber" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
                    </div>
                    <div>
                        <label runat="server" style="font-size: small">Order Date</label>
                        <asp:TextBox ID="txtOrderDate" runat="server" CssClass="w3-input w3-border TextBoxStyle" type="text"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Due Date</label>
                        <asp:TextBox ID="txtDueDate" runat="server" CssClass="w3-input w3-border TextBoxStyle" type="text"></asp:TextBox>
                    </div>

                   

                </div>
                <div class="w3-half w3-padding-right">
                <%--<div>
                        <label style="font-size: small">Salesman</label>
                        <asp:TextBox ID="txtSalesman" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small"></asp:TextBox>
                    </div>--%>
                    <div>
                        <label style="font-size: small">Customer Code</label>
                        <asp:TextBox ID="txtCustomerCode" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small"></asp:TextBox>
                    </div>
                     <div>
                        <label style="font-size: small">Customer PO No.</label>
                        <asp:TextBox ID="txtCustPONum" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small"></asp:TextBox>
                    </div>
                </div>

                <div class="w3-col s12 w3-padding-left">
                    <h5 class="w3-text-black w3-leftbar">&nbsp;Item Details</h5>
                </div>

                <div>
                    <div class="w3-col s12 w3-padding-left">
                        <asp:Table runat="server" Width="100%" CellPadding="0" CellSpacing="0">
                            <asp:TableRow>
                                <asp:TableCell CssClass="paddingRow" Width="100%">
                                    <asp:GridView ID="gvItems" runat="server" BorderColor="White" GridLines="Horizontal" Width="100%" class="table table-hover"
                                        OnRowDataBound="gvItems_RowDataBound" OnSelectedIndexChanged="gvItems_SelectedIndexChanged"
                                        AutoGenerateColumns="true" ShowHeaderWhenEmpty="True" EmptyDataText="No records has been added.">
                                        <HeaderStyle CssClass="w3-blue" />
                                        <PagerStyle CssClass="pagination-ys" />
                                    </asp:GridView>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>
                </div>
                <div class="w3-col s12 w3-padding-left">
                    <h5 class="w3-text-black w3-leftbar">&nbsp;Operations Comments</h5>
                </div>

                <div>
                    <div class="w3-col s12 w3-padding-left">
                        <asp:TextBox runat="server" ID="txtComments" TextMode="MultiLine" Width="100%" Height="50px"></asp:TextBox>
                        <br />
                        <asp:Button ID="btnSaveAllPick" CssClass="w3-btn w3-dark-gray ButtonStyle" runat="server" Text="Save" OnClick="btnSaveAllPick_Click" />
                        <asp:Button ID="btnDownload" CssClass="w3-btn w3-dark-gray ButtonStyle" runat="server" Text="Download Pick Activity" OnClick="btnDownload_Click" Visible="false" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>

    <div id="dvPickItems" class="w3-modal w3-round" runat="server" visible="false" style="z-index: 1400; background-color: transparent black; display: block; padding-top: 20px">
        <div class="w3-modal-content w3-card-4 w3-round" style="width: 90%;">
            <div class="w3-text-white w3-padding" style="background-color: #0A90CF;">
                <table runat="server" style="width: 100%">
                    <tr>
                        <td>
                            <h3 class="labelStyle2">Item Picking</h3>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="w3-padding" style="overflow-y: auto; height: 600px;">
                <div style="position: absolute; top: 10%;" class="w3-padding">
                    <div style="display: inline-block; float: left;">
                        <asp:TextBox Style="display: inline-block" runat="server" ID="txtSearchPO" placeholder="Search PO Number" class="w3-input w3-border TextBoxStyle" Font-Size="Small"></asp:TextBox>
                    </div>
                    <div style="display: inline-block; float: left; padding-right:10px">
                        <asp:Button Style="display: inline-block" runat="server" class="searchButtonStyle " ID="btnSearchPO" OnClick="btnSearchPO_Click" />
                    </div>
                    &nbsp&nbsp&nbsp&nbsp
                    <div style="display: inline-block; float: left;">
                        <asp:TextBox Style="display: inline-block" runat="server" ID="txtSearchSerial" placeholder="Search Serial Number" class="w3-input w3-border TextBoxStyle" Font-Size="Small"></asp:TextBox>
                    </div>
                    <div style="display: inline-block; float: left; padding-right:10px">
                        <asp:Button Style="display: inline-block" runat="server" class="searchButtonStyle " ID="btnSearchSerial" OnClick="btnSearchSerial_Click" />
                    </div>
                    &nbsp&nbsp&nbsp&nbsp
                    <div style="display: inline-block; float: left;">
                        <asp:TextBox Style="display: inline-block" runat="server" ID="txtLotSerial" placeholder="Lot Serial Number" class="w3-input w3-border TextBoxStyle" Font-Size="Small"></asp:TextBox>
                    </div>
                    <div style="display: inline-block; float: left; padding-right:10px">
                        <asp:Button Style="display: inline-block" runat="server" class="searchButtonStyle " ID="btnUploadLotSerial" OnClick="btnUploadLotSerial_Click" />
                    </div>
                    &nbsp&nbsp&nbsp&nbsp
                    <div style="display: inline-block; float: left;">
                        <asp:FileUpload Style="display: inline-block" runat="server" class="textBoxBorderRadius" ID="uploadExcel" />
                    </div>
                    <div style="display: inline-block; float: left; padding-right:10px">
                        <asp:Button Style="display: inline-block" ID="btnUploadSerial" class="buttonStyle buttonStyle-default" runat="server" Text="Save" Font-Size="Smaller" Height="30px" OnClick="btnUploadSerial_Click" />
                    </div>
                </div>
                <div style="position: absolute; width: 100%; top: 100px; height: 500px;">
                    <div class="w3-padding" style="width: 50%; display: inline-block; float: left">
                        <div class="w3-col">
                            <h5 class="w3-text-black w3-leftbar">SKU List</h5>
                        </div>
                        <br />
                        <div style="display: inline-block; float: left; overflow: auto; max-height: 450px">
                            <asp:GridView ID="gvSKUList" runat="server" BorderColor="White" GridLines="Horizontal" Width="95%" class="table table-hover"
                                OnRowDataBound="gvSKUList_RowDataBound" OnPageIndexChanging="gvSKUList_PageIndexChanging"
                                AutoGenerateColumns="true" ShowHeaderWhenEmpty="True" EmptyDataText="No records has been added." AllowPaging="true" PageIndex="10">
                                <HeaderStyle CssClass="w3-blue" />
                                <PagerStyle CssClass="pagination-ys" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAll" runat="server" OnCheckedChanged="chkAll_CheckedChanged"
                                                AutoPostBack="true" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkOne" runat="server" OnCheckedChanged="chkOne_CheckedChanged"
                                                AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="w3-padding" style="width: 50%; display: inline-block; float: left">
                        <div class="w3-col">
                            <h5 class="w3-text-black w3-leftbar">Picked Items</h5>
                        </div>
                        <br />
                        <%-- OnRowDataBound="gvItems_RowDataBound" OnPageIndexChanging="gvItems_PageIndexChanging"--%>
                        <div style="display: inline-block; float: left; max-height: 450px; overflow: auto">
                            <asp:GridView ID="gvSelectedSKU" runat="server" BorderColor="White" GridLines="Horizontal" Width="95%" class="table table-hover"
                                AutoGenerateColumns="true" ShowHeaderWhenEmpty="True" EmptyDataText="No records has been added."
                                OnRowDataBound="gvSelectedSKU_RowDataBound">
                                <HeaderStyle CssClass="w3-blue" />
                            </asp:GridView>
                        </div>
                    </div>
                    <br />
                    <br />
                </div>
                <div style="position: absolute; bottom: 0;" class="w3-padding">
                    <asp:Button ID="btnSavePick" class="btn btn-default" runat="server" Text="Save" Font-Size="Smaller" Height="30px" OnClick="btnSavePick_Click" />
                    &nbsp;
                    <asp:Button ID="btnCancelPick" class="btn btn-default" runat="server" Text="Cancel" Font-Size="Smaller" Height="30px" OnClick="btnCancelPick_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
