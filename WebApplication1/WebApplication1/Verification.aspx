﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Verification.aspx.cs" Inherits="WebApplication1.Verification" %>
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
    <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true"/>
    <asp:Button ID="uploadDoc" runat="server" Text="Upload Document" OnClick="uploadDoc_Click"/>
    <br />
    <br/>
 
    <asp:GridView ID="GridView1" runat="server" ViewStateMode="Enabled" AutoGenerateSelectButton="true" SelectedRowStyle-BackColor="Blue">
</asp:GridView>

    <asp:FileUpload ID="FileUpload2" runat="server" />
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
        Text="Load Check Data" />

    <asp:Button ID="loadDoc" runat="server" CssClass="style1" Text="Load Document" OnClick="loadDoc_Click"
        Width="151px" />
    <asp:Label ID="Label1" runat="server" Text="1_01"></asp:Label>
<asp:Label ID="Label2" runat="server" Text="1_02"></asp:Label>
    <br/>
    <asp:Button ID="Button2" runat="server" onclick="Button2_Click" 
        style="height: 26px" Text="Save Data" />
    <br/>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:ConnectionString %>" >
</asp:SqlDataSource>
    <br/>
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