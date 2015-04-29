<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .grid_scroll
        {
            overflow: scroll;
            height: 300px;
            border: solid 2px black;
            width: 900px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<div class="grid_scroll">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="true" 
            SelectedRowStyle-BackColor="Aqua">
    </asp:GridView>
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"></asp:SqlDataSource>
    <asp:Label ID="Label1" runat="server" Font-Size="Larger" ForeColor="Black" 
        Text="Load Image"></asp:Label>
    <br />
    <br />
    <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true"/>
    <br />
    <br />
    <asp:Button ID="uploadDoc" runat="server" Text="Upload Image" 
        OnClick="uploadDoc_Click" Width="155px"/>
   
 
   
    <br />
    <br />
    <br />
    <asp:Label ID="Label2" runat="server" Font-Size="Larger" ForeColor="Black" 
        Text="Load Check Data"></asp:Label>
    <br />
    <br />
    <asp:FileUpload ID="FileUpload2" runat="server"/>
    <br />
    <br />
   
 
   
    <asp:Button ID="uploadDoc0" runat="server" Text="Load" 
        OnClick="uploadDoc0_Click" Width="156px"/>
   
 
   
    </asp:Content>