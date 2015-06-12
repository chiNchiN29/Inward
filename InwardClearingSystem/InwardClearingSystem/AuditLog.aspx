<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AuditLog.aspx.cs" Inherits="InwardClearingSystem.AuditLog" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    AUDIT LOG<br />
    <asp:Button ID="btnSearch" runat="server" onclick="btnSearch_Click" CssClass="defaultButton" 
        Text="Search" />
    <asp:TextBox ID="txtBxDateFrom" runat="server"></asp:TextBox>
    <asp:ImageButton ID="imgPopup1" ImageUrl="~/Resources/calendar.png" Width=20px Height=15px ImageAlign=Bottom
    runat="server" />
<cc1:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup1" runat="server" TargetControlID="txtBxDateFrom"
    Format="M/d/yyyy">
</cc1:CalendarExtender>
    to
    <asp:TextBox ID="txtBxDateTo" runat="server"></asp:TextBox>
    <asp:ImageButton ID="imgPopup2" ImageUrl="~/Resources/calendar.png" Width=20px Height=15px ImageAlign=Bottom
    runat="server" />
<cc1:CalendarExtender ID="CalendarExtender2" PopupButtonID="imgPopup2" runat="server" TargetControlID="txtBxDateTo"
    Format="M/d/yyyy">
</cc1:CalendarExtender>
<br />
    <asp:CheckBox ID="chkBxUser" runat="server" />
    <asp:DropDownList ID="drpDwnUserSearch" runat="server">
    </asp:DropDownList>
    <asp:Button ID="btnViewAll" runat="server" onclick="btnViewAll_Click" CssClass="defaultButton" 
        Text="View All" />
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
    AutoGenerateColumns="false" AllowSorting="true" OnSorting="LogView_Sorting" AllowPaging="true"
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
