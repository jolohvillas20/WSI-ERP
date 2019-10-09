<%@ Page 
    Title="ERP - Customer Maintenance"
    Language="C#" 
    MasterPageFile="~/Site Master/SiteMaster.Master"    
    AutoEventWireup="true"
    CodeBehind="CustomerMaintenance.aspx.cs"     
    Inherits="SHIELD.ERP.CustomerMaintenance" 
    EnableViewState="True" 
    EnableEventValidation="false"
    MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/Site Master/SiteMaster.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <link rel="stylesheet" href="/Content/SOstyles/bootstrap.min.css" />
    <link rel="stylesheet" href="/Content/SOstyles/w3.css" />
    <script src="/Content/SOstyles/jquery.min.js"></script>

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
        </div>
        <div class="w3-animate-opacity w3-margin-0">
            <div class="w3-col s4 w3-padding-0 w3-right" style="vertical-align: central;">
                <asp:Table ID="tblSearch" CellPadding="0" CellSpacing="0" runat="server" Width="100%" Style="border: 1px solid #2196F3">
                    <asp:TableRow>
                        <asp:TableCell Width="15%">
                            <asp:DropDownList ID="ddSearchBy" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small">
                                <asp:ListItem>Company Name</asp:ListItem>
                                <asp:ListItem>Customer Code</asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell Width="20%">
                            <asp:TextBox ID="txtSearch" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder=""></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell Width="5%">
                            <asp:LinkButton runat="server" Style="width: 100%;" ID="btnSearch" Text="Search" CssClass="w3-btn w3-green ButtonStyle w3-right" OnClick="btnSearch_Click"><i class="glyphicon glyphicon-search"></i></asp:LinkButton>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel runat="server" ID="CustomerPanel" CssClass="w3-panel" Style="margin-top: 0px">
        <div class="w3-white w3-card-4" runat="server">
            <asp:GridView ID="grvCustomerDetails" DataKeyNames="idCustomer" OnSelectedIndexChanged="grvCustomerDetails_SelectedIndexChanged" OnRowDataBound="grvCustomerDetails_RowDataBound" OnPageIndexChanging="grvCustomerDetails_PageIndexChanging" runat="server" CssClass="w3-table w3-hoverable w3-round-small" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="10" Font-Size="14px" PagerStyle-BorderStyle="None" PagerStyle-Height="20px">
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
                    <asp:BoundField DataField="FullAddress" HeaderText="Address" HtmlEncode="False" />
                    <asp:BoundField DataField="credit_term" HeaderText="Credit Term" HtmlEncode="False" />
                    <asp:BoundField DataField="Position" HeaderText="Position" HtmlEncode="False" />
                    <asp:BoundField DataField="Status" HeaderText="Status" HtmlEncode="False" />
                    <asp:BoundField DataField="AddressShipping1" HeaderText="AddressShipping1" HtmlEncode="False" />
                    <asp:BoundField DataField="AddressShipping2" HeaderText="AddressShipping2" HtmlEncode="False" />
                    <asp:BoundField DataField="AddressShipping3" HeaderText="AddressShipping3" HtmlEncode="False" />
                    <asp:BoundField DataField="AddressShipping4" HeaderText="AddressShipping4" HtmlEncode="False" />
                    <asp:BoundField DataField="Customer_Type" HeaderText="Customer Type" HtmlEncode="False" />
                    <asp:BoundField DataField="Credit_Limit" HeaderText="Credit Limit" HtmlEncode="False" />
                    <asp:BoundField DataField="Contact_Number" HeaderText="Contact Number" HtmlEncode="False" />
                    <asp:BoundField DataField="TIN_Number" HeaderText="TIN_Number" HtmlEncode="False" />
                    <asp:BoundField DataField="Salesman" HeaderText="Salesman" HtmlEncode="False" />
                    <asp:BoundField DataField="Email_Address" HeaderText="Email Address" HtmlEncode="False" />
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
        <asp:Panel runat="server" ID="pnlCustomerDetails" CssClass="w3-panel w3-animate-opacity">
            <div class="w3-padding-bottom">
                <div class="w3-row w3-card-4 w3-white w3-padding-bottom w3-round-small">
                    <div class="w3-col s12 w3-padding-left">
                        <h5 class="w3-text-green w3-leftbar">&nbsp;Reseller Details</h5>
                    </div>
                    <div id="statusdiv" runat="server" class="w3-padding">
                        <label style="font-size: small">Status</label>
                        <asp:DropDownList ID="ddStatus" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small">
                            <asp:ListItem>Active</asp:ListItem>
                            <asp:ListItem>Inactive</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtStatus" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small" Visible="false"></asp:TextBox>
                    </div>

                    <div class="w3-half w3-padding-right w3-padding-left w3-padding-bottom">

                        <div>
                            <asp:Label runat="server" ID="lblCompanyName" Style="font-size: small; font-weight: bold;" Text="Company Name"></asp:Label>&nbsp;<label class="requiredColor">*</label>
                            <asp:TextBox ID="txtCompanyName" runat="server" CssClass="w3-input w3-border TextBoxStyle"></asp:TextBox>
                        </div>

                        <div>
                            <asp:Label runat="server" ID="lblCustomerName" Style="font-size: small; font-weight: bold;" Text="Contact Person"></asp:Label>
                            <asp:TextBox ID="txtCustomerName" runat="server" CssClass="w3-input w3-border TextBoxStyle"></asp:TextBox>
                        </div>


                        <div>
                            <asp:Label runat="server" ID="lblPosition" Style="font-size: small; font-weight: bold;" Text="Position"></asp:Label>
                            <asp:TextBox ID="txtPosition" runat="server" CssClass="w3-input w3-border TextBoxStyle"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Label runat="server" ID="lblContactNo" Style="font-size: small; font-weight: bold;" Text="Contact Number"></asp:Label>
                            <asp:TextBox ID="txtContactNo" runat="server" CssClass="w3-input w3-border TextBoxStyle"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Label runat="server" ID="lblEmail" Style="font-size: small; font-weight: bold;" Text="Email Address"></asp:Label>
                            <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="w3-input w3-border TextBoxStyle"></asp:TextBox>
                        </div>

                    </div>

                    <div class="w3-half w3-padding-right">

                        <div>
                            <asp:Label runat="server" ID="lblSalesman" Style="font-size: small; font-weight: bold;" Text="Salesman"></asp:Label>&nbsp;<label style="color: white;">*</label>
                            <asp:TextBox ID="txtSalesman" runat="server" CssClass="w3-input w3-border TextBoxStyle"></asp:TextBox>
                            <asp:DropDownList ID="ddSalesman" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small">
                            </asp:DropDownList>
                        </div>

                        <div id="credittermdiv" runat="server">
                            <asp:Label runat="server" Style="font-size: small">Credit Term</asp:Label>
                            <asp:DropDownList ID="ddCreditTerm" runat="server" class="w3-input w3-border TextBoxStyle" type="text">
                                <asp:ListItem>--Select Credit Term-</asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="txtCreditTerm" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small" Visible="false"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Label runat="server" ID="lblCreditLimit" Style="font-size: small; font-weight: bold;" Text="Credit Limit"></asp:Label>
                            <asp:TextBox ID="txtCreditLimit" runat="server" CssClass="w3-input w3-border TextBoxStyle"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Label runat="server" Style="font-size: small">Customer Type</asp:Label>
                            <asp:DropDownList ID="ddCustomerType" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="" Font-Size="Small">
                                <asp:ListItem>Reseller</asp:ListItem>
                                <asp:ListItem>Special Account</asp:ListItem>
                                <asp:ListItem>End User</asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="txtCustomerType" runat="server" class="w3-input w3-border TextBoxStyle" type="text" Font-Size="Small" Visible="false"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Label runat="server" ID="lblTinNo" Style="font-size: small; font-weight: bold;" Text="Tin Number"></asp:Label>
                            <asp:TextBox ID="txtTINNo" runat="server" CssClass="w3-input w3-border TextBoxStyle"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="w3-padding-bottom">
                <div class="w3-row w3-card-4 w3-white w3-padding-bottom">
                    <div class="w3-col s12 w3-padding-left">
                        <h6 class="w3-text-blue">&nbsp;Billing Address</h6>
                    </div>
                    <div class="w3-half w3-padding-right w3-padding-left w3-padding-bottom">
                        <div>
                            <asp:Label runat="server" ID="lblAddress1" Style="font-size: small; font-weight: bold;" Text="Address 1" />
                            <asp:TextBox Font-Size="Small" ID="txtAddress1" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="No./Unit/Floor/Street/Building/Village"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Label runat="server" ID="lblProvince" Style="font-size: small; font-weight: bold;" Text="Address 3" />
                            <asp:TextBox Font-Size="Small" ID="txtAddress3" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="City"></asp:TextBox>
                        </div>

                        <br />

                        <asp:CheckBox ID="chckCopy" OnCheckedChanged="chckCopy_CheckChanged" runat="server" Text="Copy Billing Address to Shipping Address" Font-Size="Small" AutoPostBack="true" />

                    </div>
                    <div class="w3-half w3-padding-right">
                        <div>
                            <asp:Label runat="server" ID="lblAddress2" Style="font-size: small; font-weight: bold;" Text="Address 2" />
                            <asp:TextBox ID="txtAddress2" Font-Size="Small" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="Baranggay/Municipality/Town"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Label runat="server" ID="lblCity" Style="font-size: small; font-weight: bold;" Text="Address 4" />
                            <asp:TextBox Font-Size="Small" ID="txtAddress4" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="Province"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="w3-padding-bottom">
                <div class="w3-row w3-card-4 w3-white w3-padding-bottom">
                    <div class="w3-col s12 w3-padding-left">
                        <h6 class="w3-text-blue">&nbsp;Shipping Address</h6>
                    </div>
                    <div class="w3-half w3-padding-right w3-padding-left w3-padding-bottom">
                        <div>
                            <asp:Label runat="server" ID="lblShipAddress1" Style="font-size: small; font-weight: bold;" Text="Address 1" />
                            <asp:TextBox Font-Size="Small" ID="txtShipAddress1" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="No./Unit/Floor/Street/Building/Village"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Label runat="server" ID="lblShipAddress3" Style="font-size: small; font-weight: bold;" Text="Address 3" />
                            <asp:TextBox Font-Size="Small" ID="txtShipAddress3" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="City"></asp:TextBox>
                        </div>
                    </div>
                    <div class="w3-half w3-padding-right">

                        <div>
                            <asp:Label runat="server" ID="lblShipAddress2" Style="font-size: small; font-weight: bold;" Text="Address 2" />
                            <asp:TextBox ID="txtShipAddress2" Font-Size="Small" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="Baranggay/Municipality/Town"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Label runat="server" ID="lblShipAddress4" Style="font-size: small; font-weight: bold;" Text="Address 4" />
                            <asp:TextBox Font-Size="Small" ID="txtShipAddress4" runat="server" class="w3-input w3-border TextBoxStyle" type="text" placeholder="Province"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>

</asp:Content>
