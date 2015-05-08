<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditUserBranches.aspx.cs" Inherits="WebApplication1.EditUserBranches" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        User Name:
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    </p>
    Branches:<br />
    <asp:CheckBoxList ID="branchList" runat="server">
    </asp:CheckBoxList>
    <asp:Button ID="saveBtn" runat="server" Text="Save" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="cancelBtn" runat="server" onclick="cancelBtn_Click" 
        Text="Cancel" />
</asp:Content>
