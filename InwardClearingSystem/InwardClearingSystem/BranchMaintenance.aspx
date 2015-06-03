<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BranchMaintenance.aspx.cs" Inherits="InwardClearingSystem.BranchMaintenance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>BRANCH MAINTENANCE</p>
        <asp:GridView ID="BranchView" runat="server" AutoGenerateColumns="false" ShowFooter="true" CssClass="gridView2" DataKeyNames="branch_id" 
        HeaderStyle-CssClass="GridHeader" OnRowEditing="EditBranch" OnRowCancelingEdit="CancelEdit" OnRowUpdating="UpdateBranch"
        AllowPaging="true" OnPageIndexChanging="BranchView_PageIndex" PagerStyle-CssClass="paging" AllowSorting="true" OnSorting="BranchView_Sorting" >
            <Columns>
             <asp:TemplateField>
                <ItemTemplate>
                    <asp:RadioButton ID="RowSelect" runat="server" OnClick="javascript:CheckOtherIsCheckedByGVID(this);" AutoPostBack="true" OnCheckedChanged="RowSelect_CheckedChanged" />
                </ItemTemplate>
             </asp:TemplateField>
             <asp:BoundField DataField="branch_id" Visible="false" />
             <asp:TemplateField HeaderText="Branch Name" SortExpression="branch_name">
                <ItemTemplate>
                    <asp:Label ID="lblBranchName" runat="server" Text='<%# Eval("branch_name") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtedBranchName" runat="server" CausesValidation="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                         ErrorMessage="Required" ForeColor="Red" 
                         ControlToValidate="txtedBranchName"></asp:RequiredFieldValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="txtBranchName" runat="server" BorderWidth=1 BorderStyle="Solid" BorderColor="Black" ValidationGroup="group1" CausesValidation="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                         ErrorMessage="Required" ForeColor="Red" ValidationGroup="group1"
                         ControlToValidate="txtBranchName"></asp:RequiredFieldValidator>
                </FooterTemplate> 
             </asp:TemplateField>
             <asp:CommandField ShowEditButton="true" />
            </Columns>
        </asp:GridView>

    <asp:Button ID="addBranch" runat="server" Text="Add" CssClass="defaultButton" ValidationGroup="group1" onclick="addBranch_Click" />
    <asp:Button ID="delBranch" runat="server" Text="Delete" CssClass="defaultButton" onclick="delBranch_Click" OnClientClick="return DeleteItem()" />

     <script type="text/javascript">

            function CheckOtherIsCheckedByGVID(spanChk) {

                var IsChecked = spanChk.checked;
                if (IsChecked) {
                }
                var CurrentRdbID = spanChk.id;
               
                var Chk = spanChk;
                Parent = document.getElementById("<%=BranchView.ClientID%>");
                var items = Parent.getElementsByTagName('input');
                for (i = 0; i < items.length; i++) {
                    if (items[i].id != CurrentRdbID && items[i].type == "radio") {
                        if (items[i].checked) {

                            items[i].checked = false;
                        }
                    }
                }
            }

            function DeleteItem() {
                if (confirm("Are you sure you want to delete?")) {
                    return true;
                }
                return false;
            }

            </script>

</asp:Content>
