<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="InwardClearingSystem._Default" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">


    <style type="text/css">
        
        .LockOff { 
         display: none; 
         visibility: hidden; 
      } 

      .LockOn { 
         display: block; 
         visibility: visible; 
         position: absolute; 
         z-index: 999; 
         top: 0px; 
         left: 0px; 
         width: 105%; 
         height: 105%; 
         background-color: #ccc; 
         text-align: center; 
         padding-top: 20%; 
         filter: alpha(opacity=100); 
         opacity: 0.8;
         font-size: large; 
         font-weight: bolder;
      } 
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
         #buttonWield
         {
             width: 400px;
         }
        </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="grid_scroll">
    <asp:GridView ID="ViewAllCheck" runat="server" CssClass="gridView" AutoGenerateColumns="false"
    AllowSorting="true" OnSorting="ViewAllCheck_Sorting" 
            HeaderStyle-CssClass="GridHeader">
        <Columns>
            <asp:BoundField DataField="check_number" SortExpression="check_number" HeaderText="Check Number" />
            <asp:BoundField DataField="customer_name" SortExpression="customer_name" HeaderText="Name" />
            <asp:BoundField DataField="account_number" SortExpression="account_number" HeaderText="Account Number" />
            <asp:BoundField DataField="check_date" SortExpression="check_date" DataFormatString="{0:d}" HeaderText="Date" />
            <asp:BoundField DataField="amount" SortExpression="amount" HeaderText="Amount" DataFormatString="{0:N}" ItemStyle-CssClass="amount" />
            <asp:BoundField DataField="balance" SortExpression="balance" HeaderText="Balance" DataFormatString="{0:N}" ItemStyle-CssClass="amount" />
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
            <asp:Label ID="lblLoadImg" runat="server" Font-Size="Larger" ForeColor="Black" 
            Text="1. Load Image"></asp:Label>
            <br />
            <asp:FileUpload ID="ImageUpload" runat="server" AllowMultiple="true"/>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
            ControlToValidate="ImageUpload" 
            ErrorMessage="Image files only"
            ValidationExpression="(.*\.([Jj][Pp][Gg])|.*\.([Jj][Pp][Ee][Gg])|.*\.([Pp][Nn][Gg])|.*\.([Tt][Ii][Ff])$)" 
            ForeColor="Red"></asp:RegularExpressionValidator>
            <asp:Button ID="uploadImgBtn" runat="server" Text="Upload Image" 

            OnClick="uploadImgBtn_Click" OnClientClick="skm_LockScreen('Uploading Images');" 
                Width="155px"/>   
            <div id="skm_LockPane" class="LockOff"></div> 

        </div>
        <div id="dataLoad">
            <asp:Label ID="lblCheckData" runat="server" Font-Size="Larger" ForeColor="Black" 
            Text="2. Load Check Data"></asp:Label>
            <br />
            <asp:FileUpload ID="DataUpload" runat="server"/>
            <asp:RegularExpressionValidator ID="regexValidator" runat="server" 
            ControlToValidate="DataUpload" 
            ErrorMessage="Only csv files are allowed"  
            ValidationExpression="(.*\.([cC][sS][vV])$)" ForeColor="Red"></asp:RegularExpressionValidator> 
            <br />   
            <asp:Button ID="uploadDoc0" runat="server" Text="Load" 
            OnClick="UploadCheckData" Width="156px"/>
        </div>
        <div id="buttonHolder">
            <asp:Button ID="clearCheck" runat="server" onclientclick="return DeleteItem()" 
            Text="Clear Check Data" onclick="clearCheck_Click1" />  
        </div>
        <div id="buttonWield">
            <asp:Label ID="genLbl" runat="server" Font-Size="Larger" ForeColor="Black" Text="Generate List of Verified Cheques"></asp:Label><br/>
            <asp:Button ID="genListBtn" runat="server" onclick="genListBtn_Click" OnClientClick="return GenerateList(); needToConfirm = false;" 
            Text="Generate List" />
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

       function skm_LockScreen(str) {
           var lock = document.getElementById('skm_LockPane');
           if (lock)
               lock.className = 'LockOn';

           lock.innerHTML = str;
       }

       function GenerateList() {
           if (confirm("Are you sure you want to generate list?")) {
               return true;
           }
           return false;
       }

 </script>

 
       
 
   
    </asp:Content>