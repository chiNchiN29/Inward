<%@ Page Title="Signature Verification" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Verification.aspx.cs" Inherits="WebApplication1.Verification" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style4
        {}
        .grid_scroll
        {
            overflow: scroll;
            height: 261px;
            border: solid 2px black;
            width: 900px;
        }
        .SelectedRowStyle
        {
           background: aqua;
        }
        .gridview
        {
            
        }
        .YesVer
        {
            color: Green;
            border: solid 1px #000000; 
        }
        .image_box
        {
             border: solid 2px black;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true"/>
   
    <asp:Button ID="insertSig" runat="server" Text="Insert Signature" OnClick="insertSig_Click" />
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <br /><br/>
    <div class ="grid_scroll">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false"
             CssClass="gridView" AllowSorting="true" OnSorting="GridView1_Sorting" HeaderStyle-CssClass="GridHeader" 
             OnRowDataBound="GridView1_RowDataBound" >
                 <Columns>
                    <asp:TemplateField>
                    <ItemTemplate>
                        <asp:RadioButton ID="RowSelect" runat="server" OnClick="javascript:CheckOtherIsCheckedByGVID(this);" AutoPostBack="true" OnCheckedChanged="RowSelect_CheckedChanged" />
                    </ItemTemplate>
                    </asp:TemplateField>
                     <asp:BoundField DataField="check_number" SortExpression="check_number" HeaderText="Check Number" />
                     <asp:BoundField DataField="customer_name" SortExpression="customer_name" HeaderText="Name" />
                     <asp:BoundField DataField="Account Number" SortExpression="Account Number" HeaderText="Account Number" />
                     <asp:BoundField DataField="Date" SortExpression="Date" HeaderText="Date" />
                     <asp:BoundField DataField="amount" SortExpression="amount" HeaderText="Amount" />
                     <asp:BoundField DataField="balance" SortExpression="balance" HeaderText="Balance" />
                     <asp:BoundField DataField="Branch Name" SortExpression="Branch Name" HeaderText="Branch Name" />
                     <asp:BoundField DataField="drawee_bank" SortExpression="drawee_bank" HeaderText="Drawee Bank" />
                     <asp:BoundField DataField="drawee_bank_branch" SortExpression="drawee_bank_branch" HeaderText="Drawee Bank Branch" />
                     <asp:BoundField DataField="verification" SortExpression="verification" HeaderText="Verified?" />
                 </Columns>
                  </asp:GridView>
</div>
    <br/>
    <br/>
    <br/>
    <asp:Image ID="Image1" runat="server" CssClass="image_box" Height="180px" 
        Width="450px" Visible="False" /> &nbsp&nbsp&nbsp
    <asp:Image ID="Image2" runat="server" CssClass="image_box" Height="180px" 
        Width="450px" Visible="False" />
    <br/>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>" >
        <SelectParameters>

      <asp:Parameter Name="UserName" />

   </SelectParameters>
    </asp:SqlDataSource>
    <br/><br/>

    <asp:Button ID="acceptButton" runat="server" CssClass="style4" Height="36px" 
        Text="Accept" Width="86px" onclick="acceptButton_Click" />
   
    
    &nbsp&nbsp&nbsp&nbsp
    <asp:Button ID="rejectButton" runat="server" Text="Reject" Height="36px" 
        Width="86px" onclick="rejectButton_Click" />
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

            function GenerateList() {
                if (confirm("Are you sure you want to generate list?")) {
                    return true;
                }
                return false;
            }
</script>

</asp:Content>
