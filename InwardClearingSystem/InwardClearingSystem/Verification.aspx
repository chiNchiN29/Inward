<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Verification.aspx.cs" Inherits="InwardClearingSystem.Verification" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
    <script src="Scripts/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.elevateZoom-3.0.8.min.js" type="text/javascript"></script>   
    <style type="text/css">
        .amount
        {
            text-align: right;
        }
        .imageBox
        {
             border: solid 2px #990000;
        }
        div.acceptBox
        {
            width:50%;
            float:left;
            text-align:center;
        }
        div.grid_scroll
        {  
            overflow: scroll;
            height: 200px;
            border: solid 2px black;
            width: auto;        
            margin: 0px  
        }
        div.gridDetails
        {
            float:right;  
            color:#990000;
            font-size:1.3em;
            padding-right:5px; 
        }
        div.imageLeft
        {
            width:40%;
            float:left;
            text-align:center;
        }
        div.remarksBox
        {
            float: left;
        }
        div.imageRight
        {
            width:40%;
            float:right;
            text-align:center;
        }
        div.rejectBox
        {
            width:50%;
            float:right;   
            text-align:center;
        }
        
        div.verifyOptions
        {
            padding-bottom:10px;
        }
        div.verifyOptionsPositioning
        {
            width:20%;
            text-align:center;
            padding-top:200px;
            margin:0 auto;
        }
        
        .YesVer
        {
            color: #009900;
        }
        
        .NoVer
        {
            color: Red;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">  
<p>SIGNATURE VERIFICATION</p> 
    <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true"/>
   
    <asp:Button ID="insertSig" runat="server" Text="Insert Signature" OnClick="insertSig_Click" />
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <br />
    <div class="gridWindow">
    <div class="gridTitleBar">Checks for Verification</div>
    <asp:Button ID="searchBtn" runat="server" onclick="searchBtn_Click"  CssClass="defaultButton"
        Text="Search Check#" />
    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
    <asp:Button ID="ViewAll" runat="server" onclick="ViewAll_Click" CssClass="defaultButton"
        Text="View All" />
    <br/>
    <div class ="grid_scroll">
        <asp:GridView ID="VerifyView" runat="server" AutoGenerateColumns="false" 
            BorderColor="Black"
              AllowSorting="true" OnSorting="VerifyView_Sorting" HeaderStyle-CssClass="GridHeader" 
             OnRowDataBound="VerifyView_RowDataBound"
            FooterStyle-CssClass="gridViewFooterStyle" Width="100%"
            AlternatingRowStyle-BackColor="#FFEFEF">
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
</div>
    <div class="gridDetails">
    Verified:
    <asp:Label ID="totalVer" runat="server" Text="0" ForeColor="Black"></asp:Label>
    /
    <asp:Label ID="totalCount" runat="server" Text="0" ForeColor="Black"></asp:Label>
    </div>
    <br />
    <div class="verifyOptionsPositioning">
    
    <br />
    <br />
        
    </div>
    <br />
        <div class="imageLeft">
            <asp:Label ID="Label1" runat="server" AssociatedControlID="checkImage" 
            BorderStyle="None" Text="Image"></asp:Label>
            <br />
            <asp:Image ID="checkImage" runat="server" CssClass="imageBox" Height="180px" 
            Width="450px"  ImageAlign="Left" ImageUrl="~/Resources/No_image_available.jpg" />
            <script type="text/javascript">
                $(function () {
                    $("#<%=checkImage.ClientID %>").elevateZoom({ scrollZoom: true, zoomWindowWidth: 450, zoomWindowHeight: 180, zoomWindowPosition: 2 });
                });
            </script>
   
        </div>

        <div class="remarksBox">
            <div class="verifyOptions">
                <div class="acceptBox">
                    <asp:Button ID="acceptButton" runat="server" CssClass="yesButton"
                    Text="Accept" onclick="acceptButton_Click" OnClientClick="needToConfirm = false;" />
                </div>
                <div class="rejectBox">
                    <asp:Button ID="rejectButton" runat="server" CssClass="noButton" Text="Reject" 
                    onclick="rejectButton_Click" OnClientClick="needToConfirm = false;" />
                </div>
            </div>
                <asp:Label ID="verifyRemarkLabel" runat="server" Text="Remarks"></asp:Label>
                <br />
                <asp:TextBox ID="verifyRemarks" runat="server" TextMode="MultiLine" Width="242px" Height="32px" style="resize: none" ></asp:TextBox>
                <div class="chkBoxScroll" style="overflow-y: scroll; width: 250px; height: 100px; text-align:left; margin: 0 auto;">
                    <asp:CheckBoxList ID="verifyChoice" runat="server" >
                        <asp:ListItem>SIGNATURE DIFFERS</asp:ListItem>
                        <asp:ListItem>AMOUNT DOES NOT MATCH</asp:ListItem>
                        <asp:ListItem>STALE CHECK</asp:ListItem>
                        <asp:ListItem>POST-DATED CHECK</asp:ListItem>
                    </asp:CheckBoxList>
                </div>
            </div>

        <div class="imageRight">
            <asp:Label ID="Label2" runat="server" AssociatedControlID="sigImage" 
            Text="Signature"></asp:Label>
            <br />
            <asp:Image ID="sigImage" runat="server" CssClass="imageBox" Height="180px" 
            Width="450px" ImageAlign="Right" ImageUrl="~/Resources/No_image_available.jpg"/>
            <script type="text/javascript">
                $(function () {
                    $("#<%=sigImage.ClientID %>").elevateZoom({ scrollZoom: true, zoomWindowWidth: 450, zoomWindowHeight: 180, zoomWindowPosition: 2, zoomout: false });
                });
                 </script>
        </div>
    
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
              var totalVer = document.getElementById("<%=totalVer.ClientID%>").text;
              var total = document.getElementById("<%=totalCount.ClientID%>").text;
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
</asp:Content>
