<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="WebApplication1.Account.SignUp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Register New User</h2>
    <br />
    <asp:CompareValidator ID="CompareValidator1" runat="server" 
        ControlToCompare="TextBox3" ControlToValidate="TextBox4" 
        ErrorMessage="The passwords do not match." ForeColor="Red"></asp:CompareValidator>
<br />
<table>
    <tr>
        <td>User Name:</td>
        <td>&nbsp;</td>
        <td><asp:TextBox ID="TextBox1" runat="server" ToolTip="This field is REQUIRED."></asp:TextBox></td>
        <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="TextBox1"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td>E-mail:</td>
        <td>&nbsp;</td>
        <td><asp:TextBox ID="TextBox2" runat="server" TextMode="Email" 
                ToolTip="This field is REQUIRED."></asp:TextBox></td>
        <td><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="TextBox2"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td>Password:</td>
        <td>&nbsp;</td>
        <td><asp:TextBox ID="TextBox3" runat="server" TextMode="Password"></asp:TextBox></td>
        <td><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="TextBox3"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td>Confirm Password:</td>
        <td>&nbsp;</td>
        <td><asp:TextBox ID="TextBox4" runat="server" TextMode="Password"></asp:TextBox></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td id="buttonstuff" colspan='3' align="center"><asp:Button ID="Button1" 
                runat="server" Text="Register" onclick="Button1_Click" /></td>
    </tr>
</table>
</asp:Content>
