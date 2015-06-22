<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="InwardClearingSystem.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p><strong>CHANGE PASSWORD</strong></p>
    <asp:Label ID="Label4" runat="server" Text="Label" Visible="False"></asp:Label>
    <asp:CompareValidator ID="CompareValidator1" runat="server" 
        ErrorMessage="New password not confirmed properly." 
        ControlToCompare="newPassword" 
        ControlToValidate="confirmNewPassword"></asp:CompareValidator>
    <br />
    <table style="width:30%;">
        <tr>
            <td colspan="2" style="text-align:center">Change Your Password</td>
        </tr>
        <tr>
            <td>Current Password:</td>
            <td><asp:TextBox ID="currentPassword" runat="server" TextMode="Password" CssClass="textbox"></asp:TextBox></td>
        </tr>
        <tr>
            <td>New Password:</td>
            <td><asp:TextBox ID="newPassword" runat="server" TextMode="Password" CssClass="textbox" ></asp:TextBox></td>
        </tr>
        <tr>
            <td>Confirm New Password:</td>
            <td><asp:TextBox ID="confirmNewPassword" runat="server" TextMode="Password" CssClass="textbox"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="text-align:center"><asp:Button ID="confirmChange" runat="server" 
                    Text="Change" CssClass="defaultButton" onclick="confirmChange_Click" /></td>
            <td style="text-align:center"><asp:Button ID="cancelChange" runat="server" 
                    Text="Cancel" CssClass="defaultButton" onclick="cancelChange_Click" /></td>
        </tr>
    </table>
    <br />
    <br />


</asp:Content>
