<%@ Page
    Title=""
    Language="C#"
    MasterPageFile="~/Site Master/SiteMaster.Master"
    AutoEventWireup="true"
    CodeBehind="OtherMaintenance.aspx.cs"
    Inherits="SOPOINV.Forms.Maintenance"
    EnableEventValidation="false"
    ValidateRequest="false"
    MaintainScrollPositionOnPostback="true" %>

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

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="w3-row">
        <div class="panel panel-default card" style="border-radius: 0px; background-color: white; margin-top: 15px;">
            <%--        <div class="w3-padding">
            <h2>Dynamic Tabs</h2>
            <p>To make the tabs toggleable, add the data-toggle="tab" attribute to each link. Then add a .tab-pane class with a unique ID for every tab and wrap them inside a div element with class .tab-content.</p>

            <ul class="nav nav-tabs">
                <li class="active"><a data-toggle="tab" href="#home">Home</a></li>
                <li><a data-toggle="tab" href="#menu1">Menu 1</a></li>
                <li><a data-toggle="tab" href="#menu2">Menu 2</a></li>
                <li><a data-toggle="tab" href="#menu3">Menu 3</a></li>
            </ul>

            <div class="tab-content">
                <div id="home" class="tab-pane fade in active">
                    <h3>HOME</h3>
                    <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
                </div>
                <div id="menu1" class="tab-pane fade">
                    <h3>Menu 1</h3>
                    <p>Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
                </div>
                <div id="menu2" class="tab-pane fade">
                    <h3>Menu 2</h3>
                    <p>Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam.</p>
                </div>
                <div id="menu3" class="tab-pane fade">
                    <h3>Menu 3</h3>
                    <p>Eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.</p>
                </div>
            </div>
        </div>--%>
            <div class="w3-padding w3-row">
                <div class="w3-row">
                    <div class="borderBottom">
                        <label style="color: #0A90CF; font-size: 16px">Users</label>
                        <br />
                    </div>
                    <br />
                    <asp:GridView runat="server" ID="gvUsers" Width="100%" PageSize="10" AllowPaging="true"
                        OnPageIndexChanging="gvUsers_PageIndexChanging" OnRowDataBound="gvUsers_RowDataBound"
                        OnSelectedIndexChanged="gvUsers_SelectedIndexChanged"
                        BorderStyle="None" GridLines="Horizontal" CellPadding="5" Font-Size="14px"
                        EmptyDataText="No records has been added." class="table table-hover">
                        <HeaderStyle CssClass="w3-blue" />
                        <PagerStyle CssClass="pagination-ys"></PagerStyle>
                    </asp:GridView>
                </div>
                <div class="w3-row">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:Label runat="server" Text="Name" Font-Bold="true" CssClass="labelStyle"></asp:Label></td>
                            <td>
                                <asp:Label runat="server" Text="Email" Font-Bold="true" CssClass="labelStyle"></asp:Label></td>
                            <td>
                                <asp:Label runat="server" Text="Domain" Font-Bold="true" CssClass="labelStyle"></asp:Label></td>
                            <td>
                                <asp:Label runat="server" Text="Password" Font-Bold="true" CssClass="labelStyle"></asp:Label></td>
                            <td>
                                <asp:Label runat="server" Text="Repeat Password" Font-Bold="true" CssClass="labelStyle"></asp:Label></td>
                            <td>
                                <asp:Label runat="server" Text="Access / Department" Font-Bold="true" CssClass="labelStyle"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="txtUserName" CssClass="w3-input w3-border TextBoxStyle"></asp:TextBox></td>
                            <td>
                                <asp:TextBox runat="server" ID="txtUserEmail" CssClass="w3-input w3-border TextBoxStyle"></asp:TextBox></td>
                            <td>
                                <asp:TextBox runat="server" ID="txtUserDomain" CssClass="w3-input w3-border TextBoxStyle"></asp:TextBox></td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPassword1" CssClass="w3-input w3-border TextBoxStyle" TextMode="Password"></asp:TextBox></td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPassword2" CssClass="w3-input w3-border TextBoxStyle" TextMode="Password"></asp:TextBox></td>
                            <td>
                                <asp:DropDownList ID="ddUserAccess" CssClass="w3-input w3-border TextBoxStyle" runat="server" TabIndex="1">
                                    <asp:ListItem Value="AE">Account Executive</asp:ListItem>
                                    <asp:ListItem Value="AU">Audit</asp:ListItem>
                                    <asp:ListItem Value="BCC">BCC</asp:ListItem>
                                    <asp:ListItem Value="IT">ITSG</asp:ListItem>
                                    <asp:ListItem Value="OP">Operations</asp:ListItem>
                                    <asp:ListItem Value="PR">Purchasing</asp:ListItem>
                                    <asp:ListItem Value="PM">Product Manager</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="btnSaveUser" Text="Save" CssClass="btn btn-default buttonStyle" OnClick="btnSaveUser_Click" Style="margin: 2px 2px 2px 2px" />
                                <asp:Button runat="server" ID="btnDeleteUser" Text="Delete" CssClass="btn btn-default buttonStyle" OnClick="btnDeleteUser_Click" Style="margin: 2px 2px 2px 2px" />
                                <asp:Button runat="server" ID="btnCancelUser" Text="Clear" CssClass="btn btn-default buttonStyle" OnClick="btnCancelUser_Click" Style="margin: 2px 2px 2px 2px" /></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <div class="w3-row">
        <div class="w3-half">
            <div class="panel panel-default card" style="border-radius: 0px; background-color: white; margin-top: 5px; width: 100%">
                <div class="w3-padding">
                    <div class="w3-row">
                        <div class="borderBottom">
                            <label style="color: #0A90CF; font-size: 16px">Credit Terms</label>
                            <br />
                        </div>
                        <br />
                        <asp:GridView runat="server" ID="gvCreditLimit" Width="100%" PageSize="10" AllowPaging="true"
                            OnPageIndexChanging="gvCreditLimit_PageIndexChanging" OnRowDataBound="gvCreditLimit_RowDataBound"
                            OnSelectedIndexChanged="gvCreditLimit_SelectedIndexChanged"
                            BorderStyle="None" GridLines="Horizontal" CellPadding="5" Font-Size="14px"
                            EmptyDataText="No records has been added." class="table table-hover">
                            <HeaderStyle CssClass="w3-blue" />
                            <PagerStyle CssClass="pagination-ys"></PagerStyle>
                        </asp:GridView>
                    </div>
                    <div class="w3-row">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Credit Term" Font-Bold="true" CssClass="labelStyle"></asp:Label></td>
                                <td>
                                    <asp:Label runat="server" Text="Description" Font-Bold="true" CssClass="labelStyle"></asp:Label></td>
                                <td>
                                    <asp:Label runat="server" Text="Days" Font-Bold="true" CssClass="labelStyle"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="txtCreditTerm" CssClass="w3-input w3-border TextBoxStyle"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtDescription" CssClass="w3-input w3-border TextBoxStyle"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtDays" CssClass="w3-input w3-border TextBoxStyle"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button runat="server" ID="btnSaveCL" Text="Save" CssClass="btn btn-default buttonStyle" OnClick="btnSaveCL_Click" Style="margin: 2px 2px 2px 2px" />
                                    <asp:Button runat="server" ID="btnDeleteCL" Text="Delete" CssClass="btn btn-default buttonStyle" OnClick="btnDeleteCL_Click" Style="margin: 2px 2px 2px 2px" />
                                    <asp:Button runat="server" ID="btnCancelCL" Text="Clear" CssClass="btn btn-default buttonStyle" OnClick="btnCancelCL_Click" Style="margin: 2px 2px 2px 2px" /></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button runat="server" ID="Button1" Text="Correct Inventory" CssClass="btn btn-default buttonStyle" OnClick="Button1_Click" Style="margin: 2px 2px 2px 2px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div class="w3-half">
            <div class="panel panel-default card" style="border-radius: 0px; background-color: white; margin-top: 5px; width: 100%">
                <div class="w3-padding">
                    <div class="w3-row">
                        <div class="borderBottom">
                            <label style="color: #0A90CF; font-size: 16px">Site Maintenance</label>
                            <br />
                        </div>
                        <br />
                        <asp:GridView runat="server" ID="gvSite" Width="100%" PageSize="10" AllowPaging="true"
                            OnPageIndexChanging="gvSite_PageIndexChanging" OnRowDataBound="gvSite_RowDataBound"
                            OnSelectedIndexChanged="gvSite_SelectedIndexChanged"
                            BorderStyle="None" GridLines="Horizontal" CellPadding="5" Font-Size="14px"
                            EmptyDataText="No records has been added." class="table table-hover">
                            <HeaderStyle CssClass="w3-blue" />
                            <PagerStyle CssClass="pagination-ys"></PagerStyle>
                        </asp:GridView>
                    </div>
                    <div class="w3-row">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Site Name" Font-Bold="true" CssClass="labelStyle"></asp:Label></td>
                                <td>
                                    <asp:Label runat="server" Text="Site Desc" Font-Bold="true" CssClass="labelStyle"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSiteName" CssClass="w3-input w3-border TextBoxStyle"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSiteDesc" CssClass="w3-input w3-border TextBoxStyle"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button runat="server" ID="btnSaveSite" Text="Save" CssClass="btn btn-default buttonStyle" OnClick="btnSaveSite_Click" Style="margin: 2px 2px 2px 2px" />
                                    <asp:Button runat="server" ID="btnDeleteSite" Text="Delete" CssClass="btn btn-default buttonStyle" OnClick="btnDeleteSite_Click" Style="margin: 2px 2px 2px 2px" />
                                    <asp:Button runat="server" ID="btnCancelSite" Text="Clear" CssClass="btn btn-default buttonStyle" OnClick="btnCancelSite_Click" Style="margin: 2px 2px 2px 2px" /></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="w3-row">
        <div class="w3-half">
            <div class="panel panel-default card" style="border-radius: 0px; background-color: white; margin-top: 5px; width: 100%">
                <div class="w3-padding">
                    <div class="w3-row">
                        <div class="borderBottom">
                            <label style="color: #0A90CF; font-size: 16px">User - Product Assignment</label>
                            <br />
                        </div>
                        <br />
                        <asp:GridView runat="server" ID="gvUserProduct" Width="100%" PageSize="10" AllowPaging="true"
                            OnPageIndexChanging="gvUserProduct_PageIndexChanging" OnRowDataBound="gvUserProduct_RowDataBound"
                            OnSelectedIndexChanged="gvUserProduct_SelectedIndexChanged"
                            BorderStyle="None" GridLines="Horizontal" CellPadding="5" Font-Size="14px"
                            EmptyDataText="No records has been added." class="table table-hover">
                            <HeaderStyle CssClass="w3-blue" />
                            <PagerStyle CssClass="pagination-ys"></PagerStyle>
                        </asp:GridView>
                    </div>
                    <div class="w3-row">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Product" Font-Bold="true" CssClass="labelStyle"></asp:Label></td>
                                <td>
                                    <asp:Label runat="server" Text="User Name" Font-Bold="true" CssClass="labelStyle"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlProduct" CssClass="w3-input w3-border TextBoxStyle" AppendDataBoundItems="true">
                                        <asp:ListItem Value="0" Text="---">----</asp:ListItem>
                                    </asp:DropDownList></td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlUsers" CssClass="w3-input w3-border TextBoxStyle" AppendDataBoundItems="true">
                                        <asp:ListItem Value="0" Text="---">----</asp:ListItem>
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button runat="server" ID="btnSaveUserProduct" Text="Save" CssClass="btn btn-default buttonStyle" OnClick="btnSaveUserProduct_Click" Style="margin: 2px 2px 2px 2px" />
                                    <asp:Button runat="server" ID="btnDeleteUserProduct" Text="Delete" CssClass="btn btn-default buttonStyle" OnClick="btnDeleteUserProduct_Click" Style="margin: 2px 2px 2px 2px" />
                                    <asp:Button runat="server" ID="btnCancelUserProduct" Text="Clear" CssClass="btn btn-default buttonStyle" OnClick="btnCancelUserProduct_Click" Style="margin: 2px 2px 2px 2px" /></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button runat="server" ID="Button5" Text="Correct Inventory" CssClass="btn btn-default buttonStyle" OnClick="Button1_Click" Style="margin: 2px 2px 2px 2px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
