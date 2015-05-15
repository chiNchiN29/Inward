<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

    <link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.5/themes/base/jquery-ui.css"
        type="text/css" media="all" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.5/jquery-ui.min.js"></script>
    <script type="text/javascript" src="http://github.com/malsup/blockui/raw/master/jquery.blockUI.js?v2.34"></script>
    <script type="text/javascript">

        $(function () {
            $('#<%= uploadDoc.ClientID %>').click(function () {
                $.blockUI({ message: '<h1>Uploading Images</h1>', css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#000',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: 4,
                    color: '#fff'
                }
                });
            });
        });


    </script>


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
         .amount
         {
             text-align: right;
         }
        </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="grid_scroll">
    <asp:GridView ID="GridView1" runat="server" CssClass="gridView" AutoGenerateColumns="false"
    AllowSorting="true" OnSorting="GridView1_Sorting" HeaderStyle-CssClass="GridHeader">
        <Columns>
            <asp:BoundField DataField="check_number" SortExpression="check_number" HeaderText="Check Number" />
            <asp:BoundField DataField="customer_name" SortExpression="customer_name" HeaderText="Name" />
            <asp:BoundField DataField="Account Number" SortExpression="Account Number" HeaderText="Account Number" />
            <asp:BoundField DataField="Date" SortExpression="Date" HeaderText="Date" />
            <asp:BoundField DataField="amount" SortExpression="amount" HeaderText="Amount" ItemStyle-CssClass="amount" />
            <asp:BoundField DataField="balance" SortExpression="balance" HeaderText="Balance" ItemStyle-CssClass="amount" />
            <asp:BoundField DataField="branch_name" SortExpression="branch_name" HeaderText="Branch Name" />
            <asp:BoundField DataField="drawee_bank" SortExpression="drawee_bank" HeaderText="Drawee Bank" />
            <asp:BoundField DataField="drawee_bank_branch" SortExpression="drawee_bank_branch" HeaderText="Drawee Bank Branch" />
            <asp:BoundField DataField="verification" SortExpression="verification" HeaderText="Verified?" />
            <asp:BoundField DataField="funded" SortExpression="funded" HeaderText="Funded?" />
        </Columns>
    </asp:GridView>
    </div>
<div id="loader">
        <div id="imageLoad">
            <asp:Label ID="Label1" runat="server" Font-Size="Larger" ForeColor="Black" 
            Text="1. Load Image"></asp:Label>
            <br />
            <asp:FileUpload ID="FileUpload3" runat="server" AllowMultiple="true"/>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
            ControlToValidate="FileUpload3" 
            ErrorMessage="Image files only"
            ValidationExpression="(.*\.([Jj][Pp][Gg])|.*\.([Jj][Pp][Ee][Gg])|.*\.([Pp][Nn][Gg])|.*\.([Tt][Ii][Ff])$)" 
            ForeColor="Red"></asp:RegularExpressionValidator>
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

 
       
 
   
    </asp:Content>