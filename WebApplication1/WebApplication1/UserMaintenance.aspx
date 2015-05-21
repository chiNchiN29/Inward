<%@ Page Title="User Maintenance" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserMaintenance.aspx.cs" Inherits="WebApplication1.UserMaintenance" %>
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
    <span class="style1"><strong>User Maintenance</strong></span><br />
    <asp:GridView ID="UserView" runat="server" DataKeyNames="user_id" AutoGenerateColumns="false" HeaderStyle-CssClass="GridHeader" CssClass="gridView2">
        <Columns>
            <asp:TemplateField>
               <ItemTemplate>
                    <asp:RadioButton ID="RowSelect" runat="server" OnClick="javascript:CheckOtherIsCheckedByGVID(this);" AutoPostBack="true" OnCheckedChanged="RowSelect_CheckedChanged"/>
               </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="user_id" HeaderText="ID" Visible="false" />
            <asp:BoundField DataField="username" HeaderText="User Name" ItemStyle-Width="150px" />
            <asp:BoundField DataField="email" HeaderText="Email Address" ItemStyle-Width="200px" />
            <asp:BoundField DataField="role_name" HeaderText="Role" ItemStyle-Width="100px" />
        </Columns>
    </asp:GridView>
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
    <asp:Button ID="delBtn" runat="server" onclick="delBtn_Click" Text="Delete" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
    <asp:Button ID="branchBtn" runat="server" Text="Assign Branches" 
        Visible="false" onclick="branchBtn_Click" />
    <br />
    <br />
    <asp:Button ID="assignBtn" runat="server" Text="Assign" 
        onclick="assignBtn_Click" />
    </asp:Content>
