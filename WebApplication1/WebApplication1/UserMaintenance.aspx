<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserMaintenance.aspx.cs" Inherits="WebApplication1.UserMaintenance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GridView1" runat="server" datakeynames="user_id"
        onselectedindexchanged="GridView1_SelectedIndexChanged">
    </asp:GridView>
    <br />
</asp:Content>
