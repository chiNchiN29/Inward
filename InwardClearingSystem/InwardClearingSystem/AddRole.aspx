<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddRole.aspx.cs" Inherits="InwardClearingSystem.AddRole" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblHeader" runat="server" Text="Label"></asp:Label>
    <br />
    <br />
    Role Name:
    <asp:TextBox ID="txtRoleName" runat="server"></asp:TextBox>
    <br />
    <br />
    Role Type:
    <asp:TextBox ID="txtRoleType" runat="server"></asp:TextBox>
    <br />
    <br />
    Functions:
    <br />
    <asp:CheckBoxList ID="CheckBoxList1" runat="server" DataSourceID="FunctionData" 
        DataTextField="function_name" DataValueField="function_name">
    </asp:CheckBoxList>
    <asp:SqlDataSource ID="FunctionData" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
        SelectCommand="SELECT [function_name] FROM [Functions]"></asp:SqlDataSource>
    <br />
    <asp:Button ID="addBtn" runat="server" Text="Add Role" CssClass="defaultButton" 
        onclick="addBtn_Click" />
    <asp:Button ID="saveBtn" runat="server" Text="Save" CssClass="defaultButton" 
        onclick="saveBtn_Click" />
    <asp:Button ID="cancelBtn" runat="server" Text="Cancel" 
        CssClass="defaultButton" onclick="cancelBtn_Click" />
    <asp:Button ID="delBtn" runat="server" Text="Delete Role" 
        CssClass="defaultButton" onclick="delBtn_Click" />
    <br />
</asp:Content>
