<%@ Page Title="Update Thresholds" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UpdateThreshold.aspx.cs" Inherits="WebApplication1.UpdateThreshold" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
    <asp:Label ID="Label1" runat="server" Text="Current Minimum Threshold Amount:"></asp:Label>
    <br />
    <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
</p>
<asp:Label ID="Label3" runat="server" Text="New Minimum Threshold Amount:"></asp:Label>
<br />
<asp:TextBox ID="TextBox1" runat="server" ontextchanged="TextBox1_TextChanged"></asp:TextBox>
<br />
<br />
<asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Set" 
    Width="91px" />
<br />
<br />
<br />
<br />
<asp:Label ID="Label4" runat="server" Text="Current Maximum Threshold Amount"></asp:Label>
<br />
<asp:Label ID="Label5" runat="server" Text="Label"></asp:Label>
<br />
<br />
<asp:Label ID="Label6" runat="server" Text="New Maximum Threshold Amount:"></asp:Label>
<br />
<asp:TextBox ID="TextBox2" runat="server" ontextchanged="TextBox1_TextChanged"></asp:TextBox>
<br />
<br />
<asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Set" 
    Width="91px" style="height: 26px" />
</asp:Content>
