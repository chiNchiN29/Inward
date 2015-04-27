<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Verification.aspx.cs" Inherits="WebApplication1.Verification" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {}
        .style2
        {}
        .style3
        {}
        .style4
        {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Button ID="uploadDoc" runat="server" Text="Upload Document" OnClick="uploadDoc_Click"/>
    <br/>
    <asp:Label ID="acctName" runat="server" Text="Account Name"></asp:Label>
    <br /> 
    <asp:Label ID="acctNo" runat="server" Text="Account Number"></asp:Label>
    <br />
    <asp:Label ID="chkNo" runat="server" Text="Check Number"></asp:Label>
    
    <br/>
    <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
    <br/>

    <asp:Button ID="loadDoc" runat="server" CssClass="style1" Text="Load Document" OnClick="loadDoc_Click"
        Width="151px" />
    <asp:Label ID="Label1" runat="server" Text="1_01"></asp:Label>
<asp:Label ID="Label2" runat="server" Text="1_02"></asp:Label>
    <br/><br/><br/>
    <asp:Image ID="Image1" runat="server" CssClass="style2" Height="181px" 
        Width="449px" Visible="False" /> &nbsp&nbsp&nbsp
    <asp:Image ID="Image2" runat="server" CssClass="style3" Height="180px" 
        Width="444px" Visible="False" />
    <br/><br/><br/>

    <asp:Button ID="acceptButton" runat="server" CssClass="style4" Height="36px" 
        Text="Accept" Width="86px" />
   
    
    &nbsp&nbsp&nbsp&nbsp
    <asp:Button ID="rejectButton" runat="server" Text="Reject" Height="36px" Width="86px" />
   
    

</asp:Content>
