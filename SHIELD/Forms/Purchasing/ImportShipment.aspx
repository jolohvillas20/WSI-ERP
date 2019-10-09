<%@ Page
    Title="ERP - Import Shipment"
    Language="C#"
    Async="true"
    EnableSessionState="True"
    MasterPageFile="~/Site Master/SiteMaster.Master"
    AutoEventWireup="true"
    CodeBehind="ImportShipment.aspx.cs"
    Inherits="SOPOINV.Forms.Purchasing.ImportShipment"
    EnableEventValidation="false"
    ValidateRequest="false"
    EnableViewState="True"
    MaintainScrollPositionOnPostback="true" %>

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

        .textBoxBorderRadius {
            border-radius: 0px;
            padding: 0px;
            padding-left: 5px;
            padding-right: 5px;
            /*font-size:16px;*/
            height: 30px;
            margin: 0px;
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
            <%--<asp:Button ID="btnPrintPO" CssClass="w3-btn w3-blue ButtonStyle" runat="server" Text="Download S.O." OnClick="btnPrintPO_Click" Visible="False" />--%>
        </div>
        <div class="w3-animate-opacity w3-margin-0">
            <div class="w3-col s3 w3-padding-0 w3-right" style="vertical-align: central;">
                <asp:Table ID="tblSearch" CellPadding="0" CellSpacing="0" runat="server" Width="100%" Style="border: 1px solid #2196F3">
                    <asp:TableRow>
                        <asp:TableCell Width="90%">
                            <asp:TextBox ID="txtSearch" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="Enter Import Shipment Number"></asp:TextBox>
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
            <asp:GridView ID="grvImpShp" OnSelectedIndexChanged="grvImpShp_SelectedIndexChanged" OnRowDataBound="grvImpShp_RowDataBound"
                OnPageIndexChanging="grvImpShp_PageIndexChanging" runat="server" CssClass="w3-table w3-hoverable w3-round-small"
                AutoGenerateColumns="true" ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="10" Font-Size="14px"
                PagerStyle-BorderStyle="None" PagerStyle-Height="20px">
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
                    <h5 class="w3-text-green w3-leftbar">&nbsp;Import Shipment Details</h5>
                </div>
                <div class="w3-half w3-padding-right w3-padding-left w3-padding-bottom">
                    <div>
                        <label style="font-size: small">Import Shipment Number</label>
                        <asp:TextBox ID="txtImpShipNum" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text=""></asp:TextBox>
                    </div>
                </div>
                <div class="w3-half w3-padding-right">
                    <div>
                        <label style="font-size: small">Total Charges</label>
                        <asp:TextBox ID="txtTotalCharges" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                </div>

            </div>
        </asp:Panel>

        <asp:Panel runat="server" ID="pnlCharges" CssClass="w3-panel w3-animate-opacity">
            <div class="w3-row w3-card-4 w3-white w3-padding-bottom w3-round-small">
                <div class="w3-col s12 w3-padding-left">
                    <h5 class="w3-text-green w3-leftbar">&nbsp;Charges</h5>
                </div>
                <div class="w3-half w3-padding-right w3-padding-left w3-padding-bottom">
                    <div>
                        <label style="font-size: small">Brokerage</label>
                        <asp:TextBox ID="txtBrokerage" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">CEDEC</label>
                        <asp:TextBox ID="txtCEDEC" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Customs Stamps</label>
                        <asp:TextBox ID="txtCustomsStamps" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Delivery Charges</label>
                        <asp:TextBox ID="txtDeliveryCharges" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Documentary Stamps</label>
                        <asp:TextBox ID="txtDocumentaryStamps" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Documentation charges</label>
                        <asp:TextBox ID="txtDocumentationCharges" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Forklift cost</label>
                        <asp:TextBox ID="txtForkliftCost" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Freight Charges</label>
                        <asp:TextBox ID="txtFreightCharges" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Handling Fee</label>
                        <asp:TextBox ID="txtHandlingFee" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                    <div>
                         <asp:Button ID="btnCompute" CssClass="w3-btn w3-green ButtonStyle" runat="server" Text="Compute" OnClick="btnCompute_Click"/>
                    </div>
                </div>

                <div class="w3-half w3-padding-right">
                    <div>
                        <label style="font-size: small">Import Duties</label>
                        <asp:TextBox ID="txtImportDuties" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Import Processing Fee</label>
                        <asp:TextBox ID="txtImportProcessingFee" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Importation Insurance</label>
                        <asp:TextBox ID="txtImportationInsurance" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Miscellaneous</label>
                        <asp:TextBox ID="txtMiscellaneous" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Notarial fee</label>
                        <asp:TextBox ID="txtNotarialFee" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Other Charges</label>
                        <asp:TextBox ID="txtOtherCharges" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Processing Fee</label>
                        <asp:TextBox ID="txtProcessingFee" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Warehouse / Storage Charges</label>
                        <asp:TextBox ID="txtWarehouseStorageCharges" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                    <div>
                        <label style="font-size: small">Xerox</label>
                        <asp:TextBox ID="txtXerox" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small" Text="0"></asp:TextBox>
                    </div>
                </div>

            </div>
        </asp:Panel>

        <asp:Panel runat="server" CssClass="w3-panel w3-animate-opacity">
            <div class="w3-row w3-card-4 w3-white w3-padding-bottom w3-round-small">

                <div class="w3-col s12 w3-padding-left">
                    <h5 class="w3-text-green w3-leftbar">&nbsp;PO Numbers</h5>
                </div>
                <div class="w3-half w3-padding-right w3-padding-left w3-padding-bottom">
                    <asp:Button ID="btnAdd" CssClass="w3-btn w3-green ButtonStyle" runat="server" Text="Add Item" OnClick="btnAdd_Click" Visible="false"/>
                </div>
                <div>
                    <div class="w3-col s12 w3-padding-left">
                        <asp:Table runat="server" Width="100%" CellPadding="0" CellSpacing="0">
                            <asp:TableRow>
                                <asp:TableCell CssClass="paddingRow" Width="100%">
                                    <asp:GridView ID="gvPO" runat="server" BorderColor="White" OnRowDataBound="gvPO_RowDataBound" OnSelectedIndexChanged="gvPO_SelectedIndexChanged"
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

        <div id="myModal" class="modal" runat="server" visible="false" style="display: block; z-index: 1000; background-color: rgba(0, 0, 0, 0.37); overflow-y: auto;">
            <div class="modal-dialog modal-lg">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" runat="server" id="btnMyModalClose" onserverclick="btnMyModalClose_ServerClick">&times;</button>
                        <h4 class="modal-title">Item List : </h4>
                    </div>

                    <div class="modal-body">
                        <div class="w3-animate-opacity w3-margin-0">
                            <div class="w3-col s8 w3-padding-bottom w3-left" style="vertical-align: central;">
                                <asp:Table ID="Table1" CellPadding="0" CellSpacing="0" runat="server" Width="100%" Style="border: 1px solid #2196F3">
                                    <asp:TableRow>
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

                        <asp:GridView ID="grvPOList" 
                            OnPageIndexChanging="grvPOList_PageIndexChanging" OnSelectedIndexChanged="grvPOList_SelectedIndexChanged" 
                            OnRowDataBound="grvPOList_RowDataBound" runat="server" CssClass="w3-table w3-hoverable w3-round-small"
                            AutoGenerateColumns="true" AllowPaging="True" PageSize="5" Font-Size="14px" PagerStyle-BorderStyle="None" 
                            PagerStyle-Height="20px">
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
                    <div class="modal-footer">
                        <asp:Table runat="server">
                            <asp:TableRow>
                                <asp:TableCell>
                                    <button type="button" id="btnCancel1" class="btn btn-default navbar-left" runat="server" onserverclick="btnCancel1_ServerClick">Cancel</button>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>
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

