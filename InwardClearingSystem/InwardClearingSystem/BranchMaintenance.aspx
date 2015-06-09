﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BranchMaintenance.aspx.cs" Inherits="InwardClearingSystem.BranchMaintenance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>BRANCH MAINTENANCE</p>
        <asp:Button ID="searchBtn" runat="server" onclick="searchBtn_Click" 
        Text="Search" />
    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="viewAllBtn" runat="server" onclick="viewAllBtn_Click" 
        Text="View All" />
    <br />
    Show&nbsp;
    <asp:DropDownList ID="pgSizeDrpDwn" runat="server" CssClass="style1" AutoPostBack="true" OnSelectedIndexChanged="BranchView_PageSize" 
        Height="20px" Width="50px">
        <asp:ListItem>10</asp:ListItem>
        <asp:ListItem>30</asp:ListItem>
        <asp:ListItem>50</asp:ListItem>
        <asp:ListItem>70</asp:ListItem>
        <asp:ListItem>100</asp:ListItem>
        <asp:ListItem>130</asp:ListItem>
        <asp:ListItem>150</asp:ListItem>
        <asp:ListItem>170</asp:ListItem>
        <asp:ListItem>200</asp:ListItem>
    </asp:DropDownList>
    <br />
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