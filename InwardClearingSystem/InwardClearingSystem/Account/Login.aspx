<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="InwardClearingSystem.Account.Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        div.userScreen
        {
            width:67.5%;  
            margin:0 auto; 
        }
        div.logoImage
        {
            float:left;   
        }
        .loginScreen
        {
            text-align:center;
            -moz-border-radius: 15px;
            border-radius: 15px;
            border:1px solid #999999 !important;
            float: right;
            height:210px !important;
            width:290px !important;
            font-family:Franklin Gothic Medium;
            background-image: -webkit-gradient(linear, left bottom, left top, color-stop(0, #FFFFFF), color-stop(1, #B5B5B5)) !important;
        }
        #LoginButton
        {
            border-radius: 15px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <p>
        <asp:HyperLink ID="RegisterHyperLink" runat="server" EnableViewState="False" 
            NavigateUrl="~/Account/SignUp.aspx">Register</asp:HyperLink> if you don't have an account.
    </p>
    <div class="userScreen">
    <div class="logoImage">
        <img src="../Resources/H2ics.png">
    </div>
    <asp:Login ID="Login1" runat="server" onauthenticate="Button1_Click" 
        BackColor="White" BorderColor="#FFFFFFF" BorderPadding="4" BorderStyle="Solid" 
        BorderWidth="1px" Font-Names="Verdana" Font-Size="0.9em" 
        ForeColor="#333333" CssClass="loginScreen">
        <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
        <LayoutTemplate>
            <table cellpadding="4" cellspacing="0">
                <tr>
                    <td>
                        <table cellpadding="0" style="height:159px;width:456px;">
                            <tr>
                                <td colspan="2" 
                                    style="color:#333333;font-size:1.5em">
                                    USER LOGIN</td>
                            </tr>
                            <tr>
                                <td colspan="2" style="color:#990000; font-style:italic;">Please Enter your Username and Password.</td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="color:Red;">
                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" >Username:</asp:Label>
                                </td>
                                <td align="left" style="padding-left:10px">
                                    <asp:TextBox ID="UserName" runat="server" Font-Size="0.8em"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                        ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                        ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                </td>
                                <td align="left" style="padding-left:10px">
                                    <asp:TextBox ID="Password" runat="server" Font-Size="0.8em" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                        ControlToValidate="Password" ErrorMessage="Password is required." 
                                        ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right" style="padding-right:100px">
                                    <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." 
                                        Font-Italic="True" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td align="right" colspan="2" style="padding-right:100px">
                                    <asp:Button ID="LoginButton" runat="server" BackColor="#990000" 
                                        BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CommandName="Login" 
                                        Font-Size="1.0em" ForeColor="#FFFFFF" Text="Login" 
                                        ValidationGroup="Login1" Height="35px" Width="90px"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
        <LoginButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" 
            BorderWidth="1px" Font-Size="0.8em" ForeColor="#284775" />
        <TextBoxStyle Font-Size="0.8em" />
        <TitleTextStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="0.9em" 
            ForeColor="White" />
    </asp:Login>
    </div>
</asp:Content>
