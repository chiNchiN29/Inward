<%@ Page Title="User Maintenance" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserMaintenance.aspx.cs" Inherits="InwardClearingSystem.UserMaintenance" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: medium;
            color: #000000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
<div class="maintenancePowers">
    <asp:TextBox ID="searchBar" runat="server" placeholder="Enter Username"></asp:TextBox>
        
    <asp:Button ID="searchUser" runat="server" Text="Search User" CssClass="defaultButton" OnClick="searchUser_Click" />
    
    <asp:Button ID="addUser" runat="server" Text="Add User" CssClass="defaultButton" OnClick="addUser_Click" />

    <asp:Button ID="editUser" runat="server" Text="Edit User" CssClass="defaultButton" OnClick="editUser_Click" />

    <asp:Button ID="deleteUser" runat="server" onclick="deleteUser_Click" Text="Delete User" CssClass="defaultButton"/>

</div>
<div class="whiteSpace">
    &nbsp;
</div>
<div class="gridDiv" style="width:50%; border:2px solid #333333">
    <div class="gridTitleBar"><strong>User Maintenance</strong></div>
    <asp:GridView ID="UserView" runat="server" DataKeyNames="user_id" AutoGenerateColumns="false" HeaderStyle-CssClass="GridHeader" CssClass="gridView2">
        <Columns>
            <asp:TemplateField>
               <ItemTemplate>
                    <asp:RadioButton ID="RowSelect" runat="server" OnClick="javascript:CheckOtherIsCheckedByGVID(this);" AutoPostBack="true" OnCheckedChanged="RowSelect_CheckedChanged"/>
               </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="user_id" HeaderText="ID" />
            <asp:BoundField DataField="username" HeaderText="User Name" />
            <asp:BoundField DataField="f_name" HeaderText="First Name" />
            <asp:BoundField DataField="m_name" HeaderText="Middle Name" />
            <asp:BoundField DataField="l_name" HeaderText="Last Name" />
            <asp:BoundField DataField="email" HeaderText="Email Address" />
            <asp:BoundField DataField="role_desc" HeaderText="Role" />
        </Columns>
    </asp:GridView>
</div>
<br />
    Assign Role:<br />

     <script type="text/javascript">
            function CheckOtherIsCheckedByGVID(spanChk) {

                var IsChecked = spanChk.checked;
                if (IsChecked) {

                }
                var CurrentRdbID = spanChk.id;
                var Chk = spanChk;
                Parent = document.getElementById("<%=UserView.ClientID%>");
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
    <asp:DropDownList ID="RoleDrpDwn"  runat="server">
    </asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
    <asp:Button ID="branchBtn" runat="server" Text="Assign Branches" 
        onclick="branchBtn_Click" CssClass="defaultButton" />
    <br />
    <br />
    <asp:Button ID="assignBtn" runat="server" Text="Assign" 
        onclick="assignBtn_Click" CssClass="defaultButton" />
    </asp:Content>
