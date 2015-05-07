<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateRole.aspx.cs" Inherits="WebApplication1.Roles.CreateRole" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GridView1" runat="server">
   
</asp:GridView>
<br />
Role Name:
<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
<br />
Role Functions:<asp:CheckBoxList ID="branchList" runat="server">
</asp:CheckBoxList>
<br />
<asp:Button ID="addRole" runat="server" onclick="addRole_Click" 
    Text="Add Role" />
<asp:SqlDataSource ID="SqlDataSource1" runat="server"
ConnectionString="<%$ ConnectionStrings:ConnectionString %>" >
</asp:SqlDataSource>
</asp:Content>
