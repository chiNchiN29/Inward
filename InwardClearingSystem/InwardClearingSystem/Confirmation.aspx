<%@ Page Title="Confirmation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Confirmation.aspx.cs" Inherits="InwardClearingSystem.Confirmation" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <script src="Scripts/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.elevateZoom-3.0.8.min.js" type="text/javascript"></script>
     <style type="text/css">
        .amount
        {
            text-align: right;
        }
        .grid_scroll
        {  
            overflow: scroll;
            height: 300px;
            border: solid 2px black;
            width: 100%;        
            margin: 0px  
        }
        .imageBox
        {
             border: solid 2px black;
        }
        div.confirmOptions
        {
            width:65%;
            margin:0 auto;
            font-family:Segoe UI;
        }
        div.fundBtnDiv
        {
            width:50%;
            float:left;
            text-align:center;
        }
        div.genBtnHolder
        {
            text-align:center;
        }
        div.gridDetails
        {
            float:right;  
            color:#990000;
            font-size:1.3em;
            padding-right:20px; 
        }
        div.imageLeft
        {
            width:50%;
            float:left;
            text-align:center;
        }
        div.imageRight
        {
            width:50%;
            float:right;
            text-align:center;
        }
		div.images
        {
            width:80%;
            height:auto;
            margin:0 auto;
        }
        div.remarksBox
        {
             text-align: center;   
        }
        div.unfundBtnDiv
        {
            width:50%;
            float:right;
            text-align:center;
        }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="grid_scroll">
        <br />
        <asp:Button ID="searchBtn" runat="server" Text="Search Check#" 
            CssClass="defaultButton" onclick="searchBtn_Click" />
        <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
        <asp:Button ID="viewAllBtn" runat="server" Text="View All" 
            CssClass="defaultButton" onclick="viewAllBtn_Click" />
    <asp:GridView ID="ConfirmView" runat="server" AutoGenerateColumns="false" 
            BorderColor="Black"
    AllowSorting="true" OnSorting="ConfirmView_Sorting" 
    HeaderStyle-CssClass="GridHeader"
            FooterStyle-CssClass="gridViewFooterStyle" 
            OnRowDataBound="ConfirmView_RowDataBound" Width="100%">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:RadioButton ID="RowSelect" runat="server" OnClick="javascript:CheckOtherIsCheckedByGVID(this); needToConfirm = false;" AutoPostBack="true" OnCheckedChanged="RowSelect_CheckedChanged" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="check_number" SortExpression="check_number" HeaderText="Check Number" />
        <asp:BoundField DataField="customer_name" SortExpression="customer_name" HeaderText="Name" />
        <asp:BoundField DataField="account_number" SortExpression="account_number" HeaderText="Account Number" />
        <asp:BoundField DataField="address" SortExpression="address" HeaderText="Address" />
        <asp:BoundField DataField="contact_number" SortExpression="contact_number" HeaderText="Contact Number" />
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
    <div class="gridDetails">
    Confirmed:
    <asp:Label ID="totalCon" runat="server" Text="0" ForeColor="Black"></asp:Label>
    /
    <asp:Label ID="totalCount" runat="server" Text="0" ForeColor="Black"></asp:Label>
    </div>
    <br />
    <div class="confirmOptions">
        <div class="fundBtnDiv">
            <asp:Button ID="fundButton" runat="server" Text="Validate" 
            onclick="fundButton_Click" OnClientClick="needToConfirm = false;"  
            CssClass="yesButton"/>
        </div>
        <div class="unfundBtnDiv">
            <asp:Button ID="unfundButton" runat="server" Text="Revoke" 
            onclick="unfundButton_Click" OnClientClick="needToConfirm = false;"  
            CssClass="noButton"/>
        </div>
    </div>
    <br />
    &nbsp;<div class="remarksBox">
        <asp:Label ID="confirmRemarkLabel" runat="server" Text="Remarks"></asp:Label>
        <br />
        <asp:TextBox ID="confirmRemarks" runat="server" TextMode="MultiLine" 
            Height="34px" Width="206px"></asp:TextBox>
    </div>
    <br />
    <div class="images">
        <div class="imageLeft">
            <asp:Label ID="Label1" runat="server" AssociatedControlID="checkImage" 
            BorderStyle="None" Text="Image"></asp:Label>
            <br />
            <asp:Image ID="checkImage" runat="server" CssClass="imageBox" Height="180px" 
            Width="450px" ImageAlign="Left" 
            ImageUrl="~/Resources/No_image_available.jpg"/>
            <script type="text/javascript">
                $(function () {
                    $("#<%=checkImage.ClientID %>").elevateZoom({ scrollZoom: true, zoomWindowWidth: 450, zoomWindowHeight: 180, zoomWindowPosition: 2 });
                });
            </script>
        </div>
        <div class="imageRight">
            <asp:Label ID="Label2" runat="server" AssociatedControlID="sigImage" 
            Text="Signature"></asp:Label>
            <br />
            <asp:Image ID="sigImage" runat="server" CssClass="imageBox" Height="180px" 
            Width="450px" ImageAlign="Right" 
            ImageUrl="~/Resources/No_image_available.jpg"/>
            <script type="text/javascript">
                $(function () {
                    $("#<%=sigImage.ClientID %>").elevateZoom({ scrollZoom: true, zoomWindowWidth: 450, zoomWindowHeight: 180, zoomWindowPosition: 2 });
                });
                 </script>
        </div>
        <div class="whiteSpace">
            
        </div>
        <div class="genBtnHolder">
            <asp:Button ID="genListBtn" runat="server" onclick="genListBtn_Click" OnClientClick="return GenerateList(); needToConfirm = false;" 
            Text="Generate List" CssClass="defaultButton"/>
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
