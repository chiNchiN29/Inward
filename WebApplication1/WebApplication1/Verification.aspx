<%@ Page Title="Signature Verification" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Verification.aspx.cs" Inherits="WebApplication1.Verification" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style4
        {}
        .grid_scroll
        {
            overflow: scroll;
            height: 260px;
            border: solid 2px black;
            width: 910px;
        }
        .image_box
        {
             border: solid 2px black;
        }
        .acceptButton
        {
            float:left;
        }
        .rejectButton
        {
            float:right;
        }
        #images
        {
            width: 920px;
        }
    </style>

    <%--<script language="JavaScript">
        window.onbeforeunload = confirmExit;
        function confirmExit() {
            return "You have attempted to leave this page.  If you have made any changes to the fields without clicking the Save button, your changes will be lost.  Are you sure you want to exit this page?";
        }
    </script>--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true"/>
   
    <asp:Button ID="insertSig" runat="server" Text="Insert Signature" OnClick="insertSig_Click" />
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <br /><br/>
    <div class ="grid_scroll">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="true" 
            SelectedRowStyle-BackColor="Aqua" 
            onselectedindexchanged="GridView1_SelectedIndexChanged">
        </asp:GridView>
</div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>" >
    </asp:SqlDataSource>
    <br/>
    
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    
    <asp:Button ID="acceptButton" runat="server" CssClass="style4" Height="36px" 
        Text="Accept" Width="86px" onclick="acceptButton_Click" />
   
    
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
   
    
    <asp:Button ID="rejectButton" runat="server" Text="Reject" Height="36px" 
        Width="86px" onclick="rejectButton_Click" />
    <br />
    <br/>
    <asp:Label ID="Label1" runat="server" AssociatedControlID="Image1" 
        BorderStyle="None" Text="Image"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
    <asp:Label ID="Label2" runat="server" AssociatedControlID="Image2" 
        Text="Signature"></asp:Label>
    <br/>
    <div id="images">
    <asp:Image ID="Image1" runat="server" CssClass="image_box" Height="180px" 
        Width="450px" Visible="False" ImageAlign="Left" 
        ImageUrl="~/Resources/H2DefaultImage.jpg"/> &nbsp&nbsp&nbsp
    <asp:Image ID="Image2" runat="server" CssClass="image_box" Height="180px" 
        Width="450px" Visible="False" ImageAlign="Right" 
        ImageUrl="~/Resources/H2DefaultImage.jpg"/>
    </div>
    <br/>
    <br/><br/>

    
    &nbsp&nbsp&nbsp&nbsp
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
        Text="Generate List" />
</asp:Content>
