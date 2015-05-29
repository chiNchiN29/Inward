<%@ Page Title="Signature Verification" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Verification.aspx.cs" Inherits="InwardClearingSystem.Verification" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.elevateZoom-3.0.8.min.js" type="text/javascript"></script>   
    <style type="text/css">
        .grid_scroll
        {  
            overflow: scroll;
            height: 300px;
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
            width:70%;
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
        #remarksBox
        {
            text-align: center; 
        }
        #verifyOptions
        {
            width: 50%;
            margin:0 auto;
        }
        #acceptBox
        {
            width:50%;
            float:left;
            text-align:center;
        }
        #rejectBox
        {
            width:50%;
            float:right;   
            text-align:center;
        }
  
 </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true"/>
   
    <asp:Button ID="insertSig" runat="server" Text="Insert Signature" OnClick="insertSig_Click" />
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <br /><br/>
    <div class ="grid_scroll">
        &nbsp;&nbsp;&nbsp;
        <asp:GridView ID="VerifyView" runat="server" AutoGenerateColumns="false" 
            BorderColor="Black" ForeColor="Black" HeaderStyle-ForeColor="White" Width="100%"
              AllowSorting="true" OnSorting="VerifyView_Sorting" HeaderStyle-CssClass="GridHeader" 
             OnRowDataBound="VerifyView_RowDataBound" ShowFooter="True" 
            FooterStyle-CssClass="gridViewFooterStyle" >
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
                     <asp:BoundField DataField="verify_remarks" SortExpression="verify_remarks" HeaderText="Technicalities" />
                 </Columns>
                  </asp:GridView>
</div>
       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
    Verified:
    <asp:Label ID="totalVer" runat="server" Text="0"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Total:
    <asp:Label ID="totalCount" runat="server" Text="0"></asp:Label>
    <br />
&nbsp;&nbsp;
       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    
    <div id="verifyOptions">
        <div id="acceptBox">
            <asp:Button ID="acceptButton" runat="server" CssClass="style4" Height="36px" 
            Text="Accept" Width="86px" onclick="acceptButton_Click" OnClientClick="needToConfirm = false;"  />
        </div>
        <div id="rejectBox">
            <asp:Button ID="rejectButton" runat="server" Text="Reject" Height="36px" 
            Width="86px" onclick="rejectButton_Click" OnClientClick="needToConfirm = false;" />
        </div>
    </div>
    <br />
    
    <br />
&nbsp;
    <div id="remarksBox">
        <asp:Label ID="verifyRemarkLabel" runat="server" Text="Remarks"></asp:Label>
        <br />
        <asp:DropDownList ID="verifyChoice" runat="server" EnableTheming="True">
            <asp:ListItem Value="None">&lt;SELECT ITEM&gt;</asp:ListItem>
            <asp:ListItem>SIGNATURE DIFFERS</asp:ListItem>
            <asp:ListItem>AMOUNT DOES NOT MATCH</asp:ListItem>
            <asp:ListItem>POST-DATED ISSUE</asp:ListItem>
            <asp:ListItem>FUTURE DATED ISSUE</asp:ListItem>
        </asp:DropDownList>
        <br />
        <br />
        <asp:TextBox ID="verifyRemarks" runat="server" TextMode="MultiLine" 
            Width="200px"></asp:TextBox>
    </div>
    <br />
    <br />
    <div id="images">

        <div id="imageLeft">
            <asp:Label ID="Label1" runat="server" AssociatedControlID="checkImage" 
            BorderStyle="None" Text="Image"></asp:Label>
            <br />
        
                  <asp:Image ID="checkImage" runat="server" CssClass="image_box" Height="180px" 
            Width="450px"  ImageAlign="Left"
            ImageUrl="~/Resources/H2DefaultImage.jpg" />
            <script type="text/javascript">
                $(function () {
                    $("#<%=checkImage.ClientID %>").elevateZoom({ scrollZoom: true, zoomWindowWidth: 450, zoomWindowHeight: 180, zoomWindowPosition: 2 });
                                });

</script>
            </div>
      
          
    
        </div>
        <div id="imageRight">
            <asp:Label ID="Label2" runat="server" AssociatedControlID="sigImage" 
            Text="Signature"></asp:Label>
            <br />
            <asp:Image ID="sigImage" runat="server" CssClass="image_box" Height="180px" 
            Width="450px" ImageAlign="Right"
            ImageUrl="~/Resources/H2DefaultImage.jpg" />
             <script type="text/javascript">
                 $(function () {
                     $("#<%=sigImage.ClientID %>").elevateZoom({ scrollZoom: true, zoomWindowWidth: 450, zoomWindowHeight: 180, zoomWindowPosition: 2 });
                 });

</script>
        </div>
    <br/>
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
