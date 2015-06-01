<%@ Page Title="User Branches" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditUserBranches.aspx.cs" Inherits="InwardClearingSystem.EditUserBranches" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        User Name:
        <asp:Label ID="userLbl" runat="server" Text="Label"></asp:Label>
    </p>
    <br />
    Branches:<br />
    
    <asp:GridView ID="branchView" runat="server" AutoGenerateColumns="false" 
        CssClass="gridView2" HeaderStyle-CssClass="GridHeader">
    <Columns>
        <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="chkBox" runat="server" OnCheckedChanged="chkBox_CheckedChanged" AutoPostBack="true" />
                </ItemTemplate>
            </asp:TemplateField>
    <asp:BoundField DataField="branch_name" HeaderText="Branch Name" ItemStyle-Width="100px" /> 
    <asp:BoundField DataField="username" HeaderText="Current Handler" ItemStyle-Width="100px" />
    </Columns>
    </asp:GridView>
    <br />
    <asp:Button ID="saveBtn" runat="server" Text="Save" onclick="saveBtn_Click" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="backBtn" runat="server" onclick="backBtn_Click" 
        Text="Back" />
    </asp:Content>
