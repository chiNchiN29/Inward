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
        .grid_scroll
        {
            overflow: scroll;
            height: 300px;
            border: solid 1px orange;
          
            width: 900px;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true"/>
    <asp:Button ID="uploadDoc" runat="server" Text="Upload Document" OnClick="uploadDoc_Click"/>
   
 
   
    <asp:Button ID="insertSig" runat="server" Text="Insert Signature" OnClick="insertSig_Click" />
    <br /><br/>
    <div class ="grid_scroll">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="true" 
            SelectedRowStyle-BackColor="Aqua" 
            onselectedindexchanged="GridView1_SelectedIndexChanged">
        </asp:GridView>
</div>

    <asp:FileUpload ID="FileUpload2" runat="server" />
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
        Text="Load Check Data" />

    <asp:Button ID="loadDoc" runat="server" CssClass="style1" Text="Load Document" OnClick="loadDoc_Click"
        Width="151px" />
    <asp:Label ID="Label1" runat="server" Text="1_01"></asp:Label>
<asp:Label ID="Label2" runat="server" Text="1_02"></asp:Label>
    <br/><br/>
    <br/>
    <asp:Image ID="Image1" runat="server" CssClass="style2" Height="181px" 
        Width="449px" Visible="False" /> &nbsp&nbsp&nbsp
    <asp:Image ID="Image2" runat="server" CssClass="style3" Height="180px" 
        Width="444px" Visible="False" />
    <br/>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>" >
    </asp:SqlDataSource>
    <br/><br/>

    <asp:Button ID="acceptButton" runat="server" CssClass="style4" Height="36px" 
        Text="Accept" Width="86px" onclick="acceptButton_Click" />
   
    
    &nbsp&nbsp&nbsp&nbsp
    <asp:Button ID="rejectButton" runat="server" Text="Reject" Height="36px" 
        Width="86px" onclick="rejectButton_Click" />
</asp:Content>
