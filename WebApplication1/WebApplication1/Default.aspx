<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

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
         #imageLoad
         {
            width: 400px;
            float: left;
         }
         #dataLoad
         {
             width: 400px;
             float: right;
         }
         #loader
         {
             width: 850px;
             margin: auto;
         }
         #buttonHolder
         {
             text-align:right;
         }
        </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="grid_scroll">
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"></asp:SqlDataSource>
    <br />
    <div id="loader">
        <div id="imageLoad">
            <asp:Label ID="Label1" runat="server" Font-Size="Larger" ForeColor="Black" 
            Text="1. Load Image"></asp:Label>
            <br />
            <asp:FileUpload ID="FileUpload3" runat="server" AllowMultiple="true"/>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
            ControlToValidate="FileUpload3" 
            ErrorMessage="Only image files are allowed"
            
                ValidationExpression="(.*\.([Jj][Pp][Gg])|.*\.([Jj][Pp][Ee][Gg])|.*\.([Pp][Nn][Gg])|.*\.([Tt][Ii][Ff])$)" 
                ForeColor="Red"></asp:RegularExpressionValidator>
            <br />
            <asp:Button ID="uploadDoc" runat="server" Text="Upload Image" 
            OnClick="uploadDoc_Click" Width="155px"/>   
        </div>
        <div id="dataLoad">
            <asp:Label ID="Label2" runat="server" Font-Size="Larger" ForeColor="Black" 
            Text="2. Load Check Data"></asp:Label>
            <br />
            <asp:FileUpload ID="FileUpload2" runat="server"/>
            <asp:RegularExpressionValidator ID="regexValidator" runat="server" 
            ControlToValidate="FileUpload2" 
            ErrorMessage="Only csv files are allowed"  
            ValidationExpression="(.*\.([cC][sS][vV])$)" ForeColor="Red"></asp:RegularExpressionValidator> 
            <br />   
            <asp:Button ID="uploadDoc0" runat="server" Text="Load" 
            OnClick="uploadDoc0_Click" Width="156px"/>
        </div>
        <div id="buttonHolder">
            <asp:Button ID="clearCheck" runat="server" onclientclick="return DeleteItem()" 
            Text="Clear Check Data" onclick="clearCheck_Click1" />  
        </div>
    </div> 
    <br />
    <br />
   
 
   
   <script type="text/javascript">
       function DeleteItem() {
           if (confirm("Are you sure you want to delete ...?")) {
               return true;
           }
           return false;
       }
 </script>

   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

   
 
   
    <br />
       
 
   
    </asp:Content>