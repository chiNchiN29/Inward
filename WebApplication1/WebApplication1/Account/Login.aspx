<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="WebApplication3.Account.Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Log In
    </h2>
    <p>
        Please enter your username and password.
        <asp:HyperLink ID="RegisterHyperLink" runat="server" EnableViewState="False" 
            NavigateUrl="~/Account/SignUp.aspx">Register</asp:HyperLink> &nbsp;if you don't have an account.
    </p>
    <table>
            <tr>
                <td>User Name:</td>
                <td>&nbsp;</td>
                <td><asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Password:</td>
                <td>&nbsp;</td>
                <td><asp:TextBox ID="TextBox2" runat="server" TextMode="Password"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Remember Me</td>
                <td>&nbsp;</td>
                <td><asp:CheckBox ID="CheckBox1" runat="server" /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td><asp:Button ID="Button1" runat="server" Text="Log In" onclick="Button1_Click" /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td><asp:Label ID="Label2" runat="server" Text="Login attempt failed." 
                        ForeColor="Red" Visible="False"></asp:Label></td>
            </tr>
        </table>
</asp:Content>
