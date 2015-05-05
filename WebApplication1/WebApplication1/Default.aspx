 <%@ Register Assembly="FlashUpload" Namespace="FlashUpload" 
	TagPrefix="FlashUpload" %>

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
        .fileUpload
        {
            width:255px;    
            font-size:11px;
            color:#000000;
            border:solid;
            border-width:1px;
            border-color:#7f9db9;    
            height:17px;
           }
        </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="grid_scroll">
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"></asp:SqlDataSource>
    <asp:Label ID="Label1" runat="server" Font-Size="Larger" ForeColor="Black" 
        Text="Load Image"></asp:Label>
    <br />
    <asp:FileUpload ID="FileUpload3" runat="server" AllowMultiple="true"/>

    <br />

    <asp:Button ID="uploadDoc" runat="server" Text="Upload Image" 
        OnClick="uploadDoc_Click" Width="155px"/>   
    <br />
    <br />
    <asp:Label ID="Label2" runat="server" Font-Size="Larger" ForeColor="Black" 
        Text="Load Check Data"></asp:Label>
    <br />
    <br />
    <asp:FileUpload ID="FileUpload2" runat="server"/>
    <asp:RegularExpressionValidator ID="regexValidator" runat="server" 
     ControlToValidate="FileUpload2" 
     ErrorMessage="Only csv files are allowed"  
     ValidationExpression="(.*\.([cC][sS][vV])$)"> 
</asp:RegularExpressionValidator> 
    <br />
    <br />
   
 
   
    <asp:Button ID="uploadDoc0" runat="server" Text="Load" 
        OnClick="uploadDoc0_Click" Width="156px"/>
   
   <script type="text/javascript">
       function DeleteItem() {
           if (confirm("Are you sure you want to delete ...?")) {
               return true;
           }
           return false;
       }
 </script>

   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

   <asp:Button ID="clearCheck" runat="server" onclientclick="return DeleteItem()" 
        Text="Clear Check Data" onclick="clearCheck_Click1" />
 
   
    <br />
       
 
   
    </asp:Content>