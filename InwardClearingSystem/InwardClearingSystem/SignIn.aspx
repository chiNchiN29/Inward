<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="InwardClearingSystem.SignIn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <asp:Label ID="Label1" runat="server" Font-Size="X-Large" 
            Text="Sign In User"></asp:Label>
        <br />
        <br />
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
    <br />
    <br />
</asp:Content>
