<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserMaintenance.aspx.cs" Inherits="WebApplication1.UserMaintenance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: medium;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <span class="style1"><strong>User Maintenance</strong></span><br />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="true" DataKeyNames="ID"
            onselectedindexchanged="GridView1_SelectedIndexChanged">
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
    ConnectionString="<%$ ConnectionStrings:ConnectionString %>" >
    </asp:SqlDataSource>
    <br />
</asp:Content>
