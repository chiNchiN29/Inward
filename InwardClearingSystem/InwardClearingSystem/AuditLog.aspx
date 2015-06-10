<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AuditLog.aspx.cs" Inherits="InwardClearingSystem.AuditLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    AUDIT LOG<br />
    <asp:Button ID="btnSearch" runat="server" onclick="btnSearch_Click" 
        Text="Search" />
    <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="auditLogUser">
    </asp:DropDownList>
    <asp:SqlDataSource ID="auditLogUser" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
        SelectCommand="SELECT [username] FROM [User]"></asp:SqlDataSource>
    Show
    <asp:DropDownList ID="pgSizeDrpDwn" runat="server" AutoPostBack="true" OnSelectedIndexChanged="LogView_PageSize"
        Height="20px" Width="50px">
        <asp:ListItem Value="10"></asp:ListItem>
        <asp:ListItem Value="30"></asp:ListItem>
        <asp:ListItem>50</asp:ListItem>
        <asp:ListItem>70</asp:ListItem>
        <asp:ListItem>100</asp:ListItem>
        <asp:ListItem>130</asp:ListItem>
        <asp:ListItem>150</asp:ListItem>
        <asp:ListItem>170</asp:ListItem>
        <asp:ListItem Value="200"></asp:ListItem>
    </asp:DropDownList>
&nbsp;entries<br />

    <asp:GridView ID="LogView" runat="server" CssClass="gridView" HeaderStyle-CssClass="GridHeader" 
    AutoGenerateColumns="false" AllowSorting="true" OnSorting="LogView_Sorting" AllowPaging="true" ShowFooter="true"
    OnPageIndexChanging="LogView_PageIndex" PagerStyle-CssClass="paging" ShowHeaderWhenEmpty="true">
        <Columns>
            <asp:BoundField DataField="action" SortExpression="action" HeaderText="Action" />
            <asp:BoundField DataField="check_number" SortExpression="check_number" HeaderText="Check Number" />
            <asp:BoundField DataField="account_number" SortExpression="account_number" HeaderText="Account Number" />
            <asp:BoundField DataField="remarks" SortExpression="remarks" HeaderText="Message" />
            <asp:BoundField DataField="date_logged" SortExpression="date_logged" HeaderText="Date Logged" />
            <asp:BoundField DataField="username" SortExpression="username" HeaderText="Username" />
        </Columns>
    </asp:GridView>
</asp:Content>
