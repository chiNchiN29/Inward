<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="WebApplication1.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">
// <![CDATA[

// ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    
    <p>
        <asp:Label ID="Label1" runat="server" Font-Size="X-Large" Text="Register User" 
            ForeColor="Black"></asp:Label>
        <br />
    </p>
    <asp:Label ID="Label2" runat="server" Text="User Name: " ForeColor="Black"></asp:Label>
    <br />
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <br />
    <asp:Label ID="Label4" runat="server" Text="Email: " ForeColor="Black"></asp:Label>
    <br />
    <asp:TextBox ID="TextBox2" runat="server" TextMode="Email"></asp:TextBox>
    <br />
    <asp:Label ID="Label3" runat="server" Text="Password: " ForeColor="Black"></asp:Label>
    <br />
    <asp:TextBox ID="TextBox3" runat="server" TextMode="Password"></asp:TextBox>
    <br />
    <asp:Label ID="Label5" runat="server" Text="Confirm Password: " 
        ForeColor="Black"></asp:Label>
    <br />
    <asp:TextBox ID="TextBox4" runat="server" TextMode="Password"></asp:TextBox>
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" Text="Sign Up" 
        onclick="Button1_Click" />
    <br />
    <br />
    <br />

    
</asp:Content>
