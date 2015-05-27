<%@ Page Title="Confirmation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Confirmation.aspx.cs" Inherits="InwardClearingSystem.Confirmation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .grid_scroll
        {  
            overflow: scroll;
            height: 300px;
            border: solid 2px black;
            width: 100%;        
            margin: 0px  
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
        .YesVer
        {
            color: #009900;
             border-bottom-color: Black;
        }
        #dataLoad
        {
            width: 400px;
            float: left;
            margin-left: 50px;
        }
		#images
        {
            width:80%;
            height:auto;
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
        .image_box
        {
             border: solid 2px black;
        }
        #remarksBox
        {
             text-align: center;   
        }
        #confirmOptions
        {
            width:50%;
            margin:0 auto;
        }
        #fundBtn
        {
            width:50%;
            float:left;
            text-align:center;
        }
        #unfundBtn
        {
            width:50%;
            float:right;
            text-align:center;
        }
        #genBtnHolder
        {
            text-align:center;
        }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="grid_scroll">
    <asp:GridView ID="ConfirmView" runat="server" AutoGenerateColumns="false" 
            BorderColor="Black" ForeColor="Black" HeaderStyle-ForeColor="White" 
   AllowSorting="true" OnSorting="ConfirmView_Sorting" Width="100%"
    HeaderStyle-CssClass="GridHeader" ShowFooter="True" 
            FooterStyle-CssClass="gridViewFooterStyle" 
            OnRowDataBound="ConfirmView_RowDataBound">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:RadioButton ID="RowSelect" runat="server" OnClick="javascript:CheckOtherIsCheckedByGVID(this); needToConfirm = false;" AutoPostBack="true" OnCheckedChanged="RowSelect_CheckedChanged" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="check_number" SortExpression="check_number" HeaderText="Check Number" />
        <asp:BoundField DataField="customer_name" SortExpression="customer_name" HeaderText="Name" />
        <asp:BoundField DataField="address" SortExpression="address" HeaderText="Address" />
        <asp:BoundField DataField="contact_number" SortExpression="contact_number" HeaderText="Contact Number" />
        <asp:BoundField DataField="account_number" SortExpression="account_number" HeaderText="Account Number" />
        <asp:BoundField DataField="check_date" SortExpression="check_date" DataFormatString="{0:d}" HeaderText="Date" />
        <asp:BoundField DataField="amount" SortExpression="amount" HeaderText="Amount" DataFormatString="{0:N}" ItemStyle-CssClass="amount" />
        <asp:BoundField DataField="branch_name" SortExpression="branch_name" HeaderText="Branch Name" />
        <asp:BoundField DataField="funded" SortExpression="funded" HeaderText="Funded?" />
        <asp:BoundField DataField="verification" SortExpression="verification" HeaderText="Verified?" />
        <asp:BoundField DataField="confirmed" SortExpression="confirmed" HeaderText="Confirmed?" />
        <asp:BoundField DataField="confirm_remarks" SortExpression="confirm_remarks" HeaderText="Remarks" />
    </Columns>
    </asp:GridView>
      </div>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
    Confirmed:
    <asp:Label ID="totalCon" runat="server" Text="0"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Total:
    <asp:Label ID="totalCount" runat="server" Text="0"></asp:Label>
    <br />
    <div id="confirmOptions">
        <div id="fundBtn">
            <asp:Button ID="fundButton" runat="server" Text="Validate" 
            onclick="fundButton_Click" OnClientClick="needToConfirm = false;" />
        </div>
        <div id="unfundBtn">
            <asp:Button ID="unfundButton" runat="server" Text="Revoke" 
            onclick="unfundButton_Click" OnClientClick="needToConfirm = false;" />
        </div>
    </div>
    <br />
    &nbsp;<div id="remarksBox">
        <asp:Label ID="confirmRemarkLabel" runat="server" Text="Remarks"></asp:Label>
        <br />
        <asp:TextBox ID="confirmRemarks" runat="server" TextMode="MultiLine" 
            Height="34px" Width="206px"></asp:TextBox>
    </div>
    <br />
    <div id="images">
        <div id="imageLeft">
            <asp:Label ID="Label1" runat="server" AssociatedControlID="checkImage" 
            BorderStyle="None" Text="Image"></asp:Label>
            <br />
            <asp:Image ID="checkImage" runat="server" CssClass="image_box" Height="180px" 
            Width="450px" ImageAlign="Left" 
            ImageUrl="~/Resources/H2DefaultImage.jpg"/>
        </div>
        <div id="imageRight">
            <asp:Label ID="Label2" runat="server" AssociatedControlID="sigImage" 
            Text="Signature"></asp:Label>
            <br />
            <asp:Image ID="sigImage" runat="server" CssClass="image_box" Height="180px" 
            Width="450px" ImageAlign="Right" 
            ImageUrl="~/Resources/H2DefaultImage.jpg"/>
        </div>
        <div id="emptySpace">
            
        </div>
        <div id="genBtnHolder">
            <asp:Button ID="genListBtn" runat="server" onclick="genListBtn_Click" OnClientClick="return GenerateList(); needToConfirm = false;" 
            Text="Generate List" />
        </div>
    </div>
    <br />

    <script type="text/javascript">
        function CheckOtherIsCheckedByGVID(spanChk) {
            var IsChecked = spanChk.checked;
            if (IsChecked) {

            }
            var CurrentRdbID = spanChk.id;
            var Chk = spanChk;
            Parent = document.getElementById("<%=ConfirmView.ClientID%>");
            var items = Parent.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].id != CurrentRdbID && items[i].type == "radio") {
                    if (items[i].checked) {

                        items[i].checked = false;
                    }
                }
            }
        }

        function GenerateList() {
            if (confirm("Are you sure you want to generate list?")) {
                return true;
            }
            return false;
        }

        var needToConfirm = true;
        window.onbeforeunload = confirmExit;
        function confirmExit() {
            var totalCon = document.getElementById("<%=totalConHide.ClientID%>").value;
            var total = document.getElementById("<%=totalCountHide.ClientID%>").value;
            if (/Firefox[\/\s](\d+)/.test(navigator.userAgent) && new Number(RegExp.$1) >= 4) {
                if (totalCon < total) {
                    if (needToConfirm)
                        return totalCon + "/" + total + " have been verified."
                }
            }
            else {
                if (totalCon < total) {
                    if (needToConfirm)
                        return totalCon + "/" + total + " have been verified."
                }
            }
        }


</script>
    <asp:HiddenField ID="totalConHide" runat="server" />
    <asp:HiddenField ID="totalCountHide" runat="server" />
</asp:Content>
