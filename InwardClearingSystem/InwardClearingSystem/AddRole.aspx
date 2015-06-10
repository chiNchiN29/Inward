<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddRole.aspx.cs" Inherits="InwardClearingSystem.AddRole" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblHeader" runat="server" Text="Label"></asp:Label>
    <br />
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
    <br />
    Role Name:
    <asp:TextBox ID="txtRoleName" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="roleNameValidator" runat="server" ErrorMessage="This field is required." ForeColor="Red"
    ControlToValidate="txtRoleName"></asp:RequiredFieldValidator>
    <br />
    <br />
    Role Type:
    <asp:TextBox ID="txtRoleType" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="roleTypeValidator" runat="server" ErrorMessage="This field is required." ForeColor="Red"
    ControlToValidate="txtRoleType"></asp:RequiredFieldValidator>
    <br />
    <br />
    Functions:
    <br />
    <asp:CheckBoxList ID="chkBoxFunctions" runat="server">
    </asp:CheckBoxList>
    <br />
    <asp:Button ID="addBtn" runat="server" Text="Add Role" CssClass="defaultButton" 
        onclick="addBtn_Click" />
    <asp:Button ID="saveBtn" runat="server" Text="Save" CssClass="defaultButton" 
        onclick="saveBtn_Click" />
    <asp:Button ID="backBtn" runat="server" Text="Back" CausesValidation="false" 
        CssClass="defaultButton" onclick="backBtn_Click" />
    <asp:Button ID="delBtn" runat="server" Text="Delete Role" 
        CssClass="defaultButton" onclick="delBtn_Click" />
    <br />
</asp:Content>
