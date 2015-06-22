<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RoleMaintenance.aspx.cs" Inherits="InwardClearingSystem.RoleMaintenance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    ROLE MAINTENANCE<br />
    <div class="gridWindow">
    <div class="gridTitleBar">Roles</div>
    <asp:GridView ID="RoleView" runat="server" AutoGenerateColumns="false" CssClass="gridView" 
    HeaderStyle-CssClass="GridHeader" DataKeyNames="role_id" AlternatingRowStyle-BackColor="#FFEFEF">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:RadioButton ID="RowSelect" runat="server" OnClick="javascript:RBSelectOne(this);" 
                AutoPostBack="true" OnCheckedChanged="RowSelect_CheckedChanged" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="role_id" HeaderText="ID" Visible="false" />
        <asp:BoundField DataField="role_desc" HeaderText="Role"/>
        <asp:BoundField DataField="role_type" HeaderText="Role Type" />
    </Columns>
    </asp:GridView>
    </div>
    <asp:Button ID="addRole" runat="server" Text="Add Role" 
        CssClass="defaultButton" onclick="addRole_Click" />
     <script type="text/javascript">
         function RBSelectOne(spanChk) {

             var IsChecked = spanChk.checked;
             if (IsChecked) {

             }
             var CurrentRdbID = spanChk.id;
             var Chk = spanChk;
             Parent = document.getElementById("<%=RoleView.ClientID%>");
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
