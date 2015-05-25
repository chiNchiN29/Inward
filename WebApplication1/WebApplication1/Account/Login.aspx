<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="WebApplication1.Account.Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style2
        {
            width: 201px;
            font-family:Palatino Linotype;
        }
        .style3
        {
            width: 77px;
        }
        .style4
        {
            width: 77px;
            font-family: Palatino Linotype;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <p>
        <asp:HyperLink ID="RegisterHyperLink" runat="server" EnableViewState="False" 
            NavigateUrl="~/Account/SignUp.aspx">Register</asp:HyperLink> if you don't have an account.
    </p>
    <asp:Login ID="Login1" runat="server" onauthenticate="Button1_Click" 
        BackColor="White" BorderColor="#FFFFFFF" BorderPadding="4" BorderStyle="Solid" 
        BorderWidth="1px" Font-Names="Verdana" Font-Size="0.9em" 
        ForeColor="#333333" Height="159px" Width="400px">
        <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
        <LayoutTemplate>
            <table cellpadding="4" cellspacing="0" style="border-collapse:collapse;">
                <tr>
                    <td>
                        <table cellpadding="0" style="height:159px;width:456px;">
                            <tr>
                                <td align="left" colspan="2" 
                                    style="color:#333333;background-color:White;font-size:1.5em;font-weight:bold;font-family:Palatino Linotype">
                                    Log In</td>
                            </tr>
                            <tr>
                                <td colspan="2">Please enter your username and password.</td>
                            </tr>
                            <tr>
                                <td class="style3">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="left" class="style4">
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" >User Name:</asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="UserName" runat="server" Font-Size="0.8em"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                        ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                        ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="style4">
                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="Password" runat="server" Font-Size="0.8em" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                        ControlToValidate="Password" ErrorMessage="Password is required." 
                                        ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="color:Red;">
                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="LoginButton" runat="server" BackColor="#FFFBFF" 
                                        BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CommandName="Login" 
                                        Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284775" Text="Log In" 
                                        ValidationGroup="Login1" Height="30px" Width="60px"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
        <LoginButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" 
            BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284775" />
        <TextBoxStyle Font-Size="0.8em" />
        <TitleTextStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="0.9em" 
            ForeColor="White" />
    </asp:Login>
</asp:Content>
