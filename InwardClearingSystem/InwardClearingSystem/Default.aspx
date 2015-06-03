<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="InwardClearingSystem._Default" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .amount
        {
            text-align: right;
        }
        .LockOff
        { 
            display: none; 
            visibility: hidden; 
        }
        .LockOn 
        { 
             display: block; 
             visibility: visible; 
             position: absolute; 
             top: 0px; 
             left: 0px;
             width: 100%; 
             height: 100%;
             background-color: #ccc; 
             text-align: center;
             vertical-align: middle; 
             opacity: 0.8;
             font-size: large; 
             font-weight: bolder;
             background-image: url('../resources/loading.gif');
             background-size: 150px;
             background-repeat: no-repeat;
             background-position: center;
             
          
        } 
        div.buttonHolder
        {
            padding-top:15px;
            text-align:right;
        }
        div.dataLoad
        {
            width: 50%;
            float:left;
            display:inline-block;
        }
        div.generateConfirmed
        {
            width: 400px;
            float: right;
            padding-top:20px;
        }
        div.generateVerified
        {
            width: 400px;
            float: left;
            padding-top:20px;
        }
        div.generator
        {
            width:80%;
            margin: 0 auto;   
        }
        div.grid_scroll
        {  
            overflow: scroll;
            height: 300px;
            border: solid 2px black;
            width: auto;        
        }
        div.gridWindow
        {
            width:100%;
            float:left;   
        }
        div.imageLoad
        {
            width: 50%;
            float:left;
            display:inline-block;
        }
        div.loader
        {
            padding-top:25px;
            float:left;   
        }
        div.number
        {
            float:left;
            height:auto;
            font-family:Segoe UI;
            font-size:7em;
            width:10%;
            color:#990000;
        }
        div.uploaderDiv
        {
            width: 100%;
            height: 100px;
        }
        </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <div class="uploaderDiv">
        <div class="imageLoad">
            <div class="number">
                1
            </div>
            <div class="loader">
            <asp:Label ID="lblLoadImg" runat="server" Font-Size="Larger" ForeColor="Black" 
            Text="Load Image"></asp:Label>
            <br />
            <asp:FileUpload ID="ImageUpload" runat="server" AllowMultiple="true"/>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
            ControlToValidate="ImageUpload" 
            ErrorMessage="Image files only"
            ValidationExpression="(.*\.([Jj][Pp][Gg])|.*\.([Jj][Pp][Ee][Gg])|.*\.([Pp][Nn][Gg])|.*\.([Tt][Ii][Ff])$)" 
            ForeColor="Red"></asp:RegularExpressionValidator>
            <br />
            <asp:Button ID="uploadImgBtn" runat="server" Text="Upload Image" 
            OnClick="uploadImgBtn_Click" OnClientClick="skm_LockScreen('Uploading Images');" 
                CssClass="defaultButton" />   
            <cc1:RoundedCornersExtender ID="uploadImgBtn_RoundedCornersExtender" 
                runat="server" BehaviorID="uploadImgBtn_RoundedCornersExtender" 
                TargetControlID="uploadImgBtn" />
            <div id="skm_LockPane" class="LockOff"></div> 
            </div>

        </div>
        <div class="dataLoad">
            <div class="number">
                2
            </div>
            <div class="loader">
                <asp:Label ID="lblCheckData" runat="server" Font-Size="Larger" ForeColor="Black" 
                Text="Load Check Data"></asp:Label>
                <br />
                <asp:FileUpload ID="DataUpload" runat="server"/>
                <asp:RegularExpressionValidator ID="regexValidator" runat="server" 
                ControlToValidate="DataUpload" 
                ErrorMessage="Only csv files are allowed"  
                ValidationExpression="(.*\.([cC][sS][vV])$)" ForeColor="Red"></asp:RegularExpressionValidator> 
                <br />
                <asp:Button ID="uploadDoc0" runat="server" Text="Load" 
                OnClick="UploadCheckData" CssClass="defaultButton"/>
                <cc1:RoundedCornersExtender ID="uploadDoc0_RoundedCornersExtender" 
                    runat="server" BehaviorID="uploadDoc0_RoundedCornersExtender" 
                    TargetControlID="uploadDoc0" />
            </div>
        </div>
</div>

<div class="whiteSpace">
    &nbsp;
</div>

    <div class="gridWindow">
    <div class="gridTitleBar" style="width:99.4%">
        All Cheques
    </div>
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
            <asp:BoundField DataField="bank_remarks" SortExpression="bank_remarks" HeaderText="Bank Remarks" />
            <asp:BoundField DataField="modified_by" SortExpression="modified_by" HeaderText="Modified By" />
            <asp:BoundField DataField="modified_date" SortExpression="modified_date" HeaderText="Modified Date" />
        </Columns>
    </asp:GridView>
    </div>
        <div class="buttonHolder">
            <asp:Button ID="genListBtn" runat="server" onclick="genListBtn_Click" OnClientClick="return GenerateList(); needToConfirm = false;"
                Text="Generate List" CssClass="defaultButton"/>
            <cc1:RoundedCornersExtender ID="genListBtn_RoundedCornersExtender"
                runat="server" BehaviorID="genListBtn_RoundedCornersExtender" 
                TargetControlID="genListBtn" />
            <asp:Button ID="produceReport" runat="server" Text="Produce Report" OnClick="ProduceFinalReport" CssClass="defaultButton" />
            <cc1:RoundedCornersExtender ID="produceReport_RoundedCornersExtender"
                runat="server" BehaviorID="produceReport_RoundedCornersExtender" 
                TargetControlID="produceReport" />
            <asp:Button ID="clearCheck" runat="server" onclientclick="return DeleteItem()" 
                Text="Clear Check Data" onclick="clearCheck_Click1" CssClass="defaultButton" />  
            <cc1:RoundedCornersExtender ID="clearCheck_RoundedCornersExtender" 
                runat="server" BehaviorID="clearCheck_RoundedCornersExtender" 
                TargetControlID="clearCheck" />
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

     
       function CompareData() {
           var im = document.getElementById("<%=ImgCount.ClientID%>").value;
           var data = document.getElementById("<%=DataCount.ClientID%>").value;
           alert(im + " images were uploaded.\n" + data + " check data were uploaded");
       }
       </script>
 
       
 <asp:HiddenField ID="ImgCount" runat="server" />
    <asp:HiddenField ID="DataCount" runat="server" />
   
    </asp:Content>
