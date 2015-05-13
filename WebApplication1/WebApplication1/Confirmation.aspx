<%@ Page Title="Confirmation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Confirmation.aspx.cs" Inherits="WebApplication1.Confirmation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
    .grid_scroll
        {
            overflow: scroll;
            height: 300px;
            border: solid 2px black;
            width: 900px;
        }
    
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="grid_scroll">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" 
    CssClass="gridView" AllowSorting="true" OnSorting="GridView1_Sorting" 
    HeaderStyle-CssClass="GridHeader">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:RadioButton ID="RowSelect" runat="server" OnClick="javascript:CheckOtherIsCheckedByGVID(this);" AutoPostBack="true" OnCheckedChanged="RowSelect_CheckedChanged" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="check_number" SortExpression="check_number" HeaderText="Check Number" />
        <asp:BoundField DataField="customer_name" SortExpression="customer_name" HeaderText="Name" />
        <asp:BoundField DataField="customer_address" SortExpression="customer_address" HeaderText="Address" />
        <asp:BoundField DataField="contact_number" SortExpression="contact_number" HeaderText="Contact Number" />
        <asp:BoundField DataField="Account Number" SortExpression="Account Number" HeaderText="Account Number" />
        <asp:BoundField DataField="Date" SortExpression="Date" HeaderText="Date" />
        <asp:BoundField DataField="amount" SortExpression="amount" HeaderText="Amount" />
        <asp:BoundField DataField="branch_name" SortExpression="branch_name" HeaderText="Branch Name" />
        <asp:BoundField DataField="drawee_bank" SortExpression="drawee_bank" HeaderText="Drawee Bank" />
        <asp:BoundField DataField="drawee_bank_branch" SortExpression="drawee_bank_branch" HeaderText="Drawee Bank Branch" />
        <asp:BoundField DataField="funded" SortExpression="funded" HeaderText="Funded?" />
        <asp:BoundField DataField="verification" SortExpression="verification" HeaderText="Verified?" />
        <asp:BoundField DataField="confirmed" SortExpression="confirmed" HeaderText="Confirmed?" />
    </Columns>
    </asp:GridView>
      </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>" >
        </asp:SqlDataSource>
      
    <br />
    <asp:Button ID="fundButton" runat="server" Text="Validate" 
        onclick="fundButton_Click" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="unfundButton" runat="server" Text="Revoke" 
        onclick="unfundButton_Click" />
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
        Text="Generate List" />

    <script type="text/javascript">
        function CheckOtherIsCheckedByGVID(spanChk) {
            var IsChecked = spanChk.checked;
            if (IsChecked) {

            }
            var CurrentRdbID = spanChk.id;
            var Chk = spanChk;
            Parent = document.getElementById("<%=GridView1.ClientID%>");
            var items = Parent.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].id != CurrentRdbID && items[i].type == "radio") {
                    if (items[i].checked) {

                        items[i].checked = false;
                    }
                }
            }
        }
</script>
</asp:Content>
