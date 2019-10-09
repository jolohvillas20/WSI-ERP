<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SHIELD.Login" %>

<%--<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label runat="server" ID="test"></asp:Label>
            <asp:Label runat="server" Text="Username : "></asp:Label>
            <asp:TextBox runat="server" ID="txtUsername"></asp:TextBox>
            <asp:Label runat="server" Text="Password"></asp:Label>
            <asp:TextBox runat="server" ID="txtPassword" TextMode="Password"></asp:TextBox>
            <asp:Button runat="server" Text="Login" ID="btnLogin" OnClick="btnLogin_Click" />
        </div>
    </form>
</body>
</html>--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>

        <link rel="stylesheet" href="/css/bootstrap.min.css"/>
        <link rel="stylesheet" href="/css/w3.css"/>

    <style>
        html, body, h1, h2, h3, h4, h5 {
            font-family: "Segoe UI", sans-serif;
        }
        /* latin-ext */
        @font-face {
            font-family: 'Segoe UI';
            font-style: normal;
            font-weight: 400;
            src: local('Segoe UI'), local('Segoe UI-SemiBold'), url(https://fonts.gstatic.com/s/raleway/v11/yQiAaD56cjx1AooMTSghGfY6323mHUZFJMgTvxaG2iE.woff2) format('woff2');
            unicode-range: U+0100-024F, U+1E00-1EFF, U+20A0-20AB, U+20AD-20CF, U+2C60-2C7F, U+A720-A7FF;
        }
        /* latin */
        @font-face {
            font-family: 'Segoe UI';
            font-style: normal;
            font-weight: 400;
            src: local('Segoe UI'), local('Segoe UI-SemiBold'), url(https://fonts.gstatic.com/s/raleway/v11/0dTEPzkLWceF7z0koJaX1A.woff2) format('woff2');
            unicode-range: U+0000-00FF, U+0131, U+0152-0153, U+02C6, U+02DA, U+02DC, U+2000-206F, U+2074, U+20AC, U+2212, U+2215;
        }
    </style>

</head>
<body class="w3-light-gray w3-main">
    <form id="form2" runat="server">
    <div class="w3-row " style="margin-top:100px;">
            <div class="w3-col s4">
               &nbsp;
            </div>
            <div class="w3-col s4 w3-card w3-white text-center w3-round-small w3-animate-zoom" >
                <div class="w3-col s12 w3-deep-purple w3-border-0" style="height:50px;">
                     <h4>WSI ERP</h4>
                </div>
                <div class="w3-animate-zoom">
                    
                    <asp:Panel ID="pnlWarningMessage" runat="server" CssClass="w3-red" Visible="false">
                        <asp:Label ID="lblWarningMessage" runat="server">Account is invalid</asp:Label>
                    </asp:Panel>

                    <div class="text-left w3-padding-left w3-padding-right w3-animate-opacity">
                        <i class="glyphicon glyphicon-user w3-padding-top w3-medium w3-text-gray"></i>
                        <label>Username</label>
                        <asp:TextBox ID="txtUsername" runat="server" style="width:100%" CssClass="w3-input w3-border w3-round-small text-center"></asp:TextBox>
                    </div>
                    <div class="text-left w3-padding-left w3-padding-right w3-animate-opacity">
                        <i class="glyphicon glyphicon-lock w3-medium w3-text-gray"></i>
                        <label>Password</label>
                        <asp:TextBox ID="txtPassword" runat="server" style="width:100%" CssClass="w3-input w3-border w3-round-small text-center" TextMode="Password"></asp:TextBox>
                    </div>
                    <div class="text-right w3-padding-left w3-padding-right w3-padding-bottom w3-margin-top">
                        <asp:Button ID="btnLogin" runat="server" CssClass="w3-btn w3-blue" OnClick="btnLogin_Click" Text="Login" style="width:100%;" />
                    </div>
                    <hr style="margin-top:0px; margin-bottom:0px;" />
                    <div style="margin-top:0px;">
                        <label class="w3-text-gray" style="font-size:11px;margin-top:0px; margin-bottom:0px;">©Copyright 2019</label><br />
                        <label class="w3-text-gray" style="font-size:11px;margin-top:0px; margin-bottom:5px;">Wordtext Systems, Inc.</label>

                    </div>
                </div>
            </div>
            
            <div class="w3-col s4">
                &nbsp;
            </div>
    </div>
    </form>
</body>
</html>