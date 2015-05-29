<%@ Page Title="Signature Verification" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Verification.aspx.cs" Inherits="InwardClearingSystem.Verification" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.elevateZoom-3.0.8.min.js" type="text/javascript"></script>   
    <style type="text/css">
        .grid_scroll
        {  
            overflow: scroll;
            height: 60px;
            border: solid 2px black;
            width: 100%;        
            margin: 0px  
        }
   
        .YesVer
        {
            color: #009900;
             border-bottom-color: Black;
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
            width:80%;
            margin:0 auto;
        }
        #imageLeft
        {
            width:50%;
            float:left;
            text-align:center;
        }
        #imageRight
        {
            width:50%;
            float:right;
            text-align:center;
        }
        #buttonWield
        {
            text-align:center;
        }
        .amount
        {
            text-align: right;
        }
        .NoVer
        {
            color: Red;
            border-bottom-color: Black;
        }
        #verifyOptions
        {
            width: 70%;
            height:110px;
            display:inline-block;
            margin:0 auto;
        }
        #acceptBox
        {
            width:25%;
            float:left;
            text-align:center;
            height:110px;
            vertical-align:middle;
        }
        .acceptBtn
        {
            vertical-align:middle; 
            color:White;
            height:36px; 
            width:155px;
        }
        .acceptBtn:hover
        {
            background-color:ButtonShadow;
        }
        #remarksBox
        {
            text-align: center;
            float:left;
            width:48%;
        }
        #rejectBox
        {
            width:25%;
            float:left;   
            text-align:center;
            height:110px;
            vertical-align:middle;
        }
        .rejectBtn
        {
            vertical-align:middle; 
            color:White;
            height:36px; 
            width:155px;
        }
        .rejectBtn:hover
        {
            background-color:ButtonShadow;
        }
        .whiteSpace
        {
            margin-top:10px;
            margin-bottom:10px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="images">
        <div id="imageLeft">
            <asp:Label ID="Label1" runat="server" AssociatedControlID="checkImage" 
            BorderStyle="None" Text="Image"></asp:Label>
            <br />
            <asp:Image ID="checkImage" runat="server" CssClass="image_box" Height="180px" 
            Width="450px"  ImageAlign="Left" onmouseover="zoomin(this)" onmouseout="zoomout(this)"
            ImageUrl="~/Resources/H2DefaultImage.jpg" />
   
        </div>
        <div id="imageRight">
            <asp:Label ID="Label2" runat="server" AssociatedControlID="sigImage" 
            Text="Signature"></asp:Label>
            <br />
            <asp:Image ID="sigImage" runat="server" CssClass="image_box" Height="180px" 
            Width="450px" ImageAlign="Right" onmouseover="zoomin(this)" onmouseout="zoomout(this)" 
            ImageUrl="~/Resources/H2DefaultImage.jpg"/>
        </div>

    </div>
    <div class="whiteSpace">
    </div>
    <div id="verifyOptions">
        <div id="acceptBox">
            <asp:Button ID="acceptButton" runat="server" CssClass="acceptBtn"
            Text="Accept" onclick="acceptButton_Click" OnClientClick="needToConfirm = false;" 
             BackColor="#00CC00" />
            
            <cc1:RoundedCornersExtender ID="acceptButton_RoundedCornersExtender" 
                runat="server" BehaviorID="acceptButton_RoundedCornersExtender" 
                TargetControlID="acceptButton">
            </cc1:RoundedCornersExtender>
            
        </div>
        <div id="remarksBox">
            <asp:Label ID="verifyRemarkLabel" runat="server" Text="Remarks"></asp:Label>
            <br />
            <asp:DropDownList ID="verifyChoice" runat="server" EnableTheming="True">
            <asp:ListItem Value="0">&lt;SELECT ITEM&gt;</asp:ListItem>
            <asp:ListItem>SIGNATURE DIFFERS</asp:ListItem>
            <asp:ListItem>AMOUNT DOES NOT MATCH</asp:ListItem>
            <asp:ListItem>POST-DATED ISSUE</asp:ListItem>
            <asp:ListItem>FUTURE DATED ISSUE</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:TextBox ID="verifyRemarks" runat="server" TextMode="MultiLine" 
            Width="200px"></asp:TextBox>
        </div>
        <div id="rejectBox">
            <asp:Button ID="rejectButton" runat="server" CssClass="rejectBtn" Text="Reject" 
            onclick="rejectButton_Click" OnClientClick="needToConfirm = false;" BackColor="#CC0000" />
            
            <cc1:RoundedCornersExtender ID="rejectButton_RoundedCornersExtender" 
                runat="server" BehaviorID="rejectButton_RoundedCornersExtender" 
                TargetControlID="rejectButton">
            </cc1:RoundedCornersExtender>
            
        </div>
    </div>
    <br />
    
    <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true"/>
   
    <asp:Button ID="insertSig" runat="server" Text="Insert Signature" OnClick="insertSig_Click" />
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <br /><br/>
    <div class ="grid_scroll">
        <asp:GridView ID="VerifyView" runat="server" AutoGenerateColumns="false" 
            BorderColor="Black"
              AllowSorting="true" OnSorting="VerifyView_Sorting" HeaderStyle-CssClass="GridHeader" 
             OnRowDataBound="VerifyView_RowDataBound" ShowFooter="True" 
            FooterStyle-CssClass="gridViewFooterStyle" Width="100%">
                 <Columns>
                    <asp:TemplateField>
                    <ItemTemplate>
                        <asp:RadioButton ID="RowSelect" runat="server" OnClick="javascript:CheckOtherIsCheckedByGVID(this); needToConfirm = false;"  AutoPostBack="true" OnCheckedChanged="RowSelect_CheckedChanged" />
                    </ItemTemplate>
                    </asp:TemplateField>
                     <asp:BoundField DataField="check_number" SortExpression="check_number" HeaderText="Check Number" />
                     <asp:BoundField DataField="name" SortExpression="name" HeaderText="Name" />
                     <asp:BoundField DataField="account_number" SortExpression="account_number" HeaderText="Account Number" />
                     <asp:BoundField DataField="check_date" SortExpression="check_date" DataFormatString="{0:d}" HeaderText="Date" />
                     <asp:BoundField DataField="amount" SortExpression="amount" HeaderText="Amount" DataFormatString="{0:N}" ItemStyle-CssClass="amount" />
                     <asp:BoundField DataField="balance" SortExpression="balance" HeaderText="Balance" DataFormatString="{0:N}" ItemStyle-CssClass="amount" />
                     <asp:BoundField DataField="branch_name" SortExpression="branch_name" HeaderText="Branch Name" />
                     <asp:BoundField DataField="drawee_bank" SortExpression="drawee_bank" HeaderText="Drawee Bank" />
                     <asp:BoundField DataField="drawee_bank_branch" SortExpression="drawee_bank_branch" HeaderText="Drawee Bank Branch" />
                     <asp:BoundField DataField="verification" SortExpression="verification" HeaderText="Verified?" />
                 </Columns>
                  </asp:GridView>
</div>
       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
    Verified:
    <asp:Label ID="totalVer" runat="server" Text="0"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Total:
    <asp:Label ID="totalCount" runat="server" Text="0"></asp:Label>
    <br />
    

        <script type="text/javascript">

            function CheckOtherIsCheckedByGVID(spanChk) {

                var IsChecked = spanChk.checked;
                if (IsChecked) {
                }
                var CurrentRdbID = spanChk.id;
               
                var Chk = spanChk;
                Parent = document.getElementById("<%=VerifyView.ClientID%>");
                var items = Parent.getElementsByTagName('input');
                for (i = 0; i < items.length; i++) {
                    if (items[i].id != CurrentRdbID && items[i].type == "radio") {
                        if (items[i].checked) {

                            items[i].checked = false;
                        }
                    }
                }
            }

            

            var needToConfirm = true;
          window.onbeforeunload = confirmExit;
          function confirmExit() {
              var totalVer = document.getElementById("<%=totalVerHide.ClientID%>").value;
              var total = document.getElementById("<%=totalCountHide.ClientID%>").value;
              if (/Firefox[\/\s](\d+)/.test(navigator.userAgent) && new Number(RegExp.$1) >= 4) {
                  if (totalVer < total) {
                      if (needToConfirm)
                          return totalVer + "/" + total + " have been verified."
                  }
              }
              else {
                  if (totalVer < total) {
                      if (needToConfirm)
                          return totalVer + "/" + total + " have been verified."
                  }
              }
          }
</script>

    <asp:HiddenField ID="totalCountHide" runat="server" />
    <asp:HiddenField ID="totalVerHide" runat="server" />

</asp:Content>
