<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditUserBranches.aspx.cs" Inherits="WebApplication1.EditUserBranches" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        User Name:
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    </p>
    Branches:<br />
    <br />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="chkBox" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
    <asp:BoundField DataField="branch_name" HeaderText="Branch Name" />
    <asp:BoundField DataField="number_checks" HeaderText="Number of Checks" />
    <asp:BoundField DataField="username" HeaderText="Handler" />
    </Columns>
    </asp:GridView>
    <br />
    <asp:Button ID="saveBtn" runat="server" Text="Save" onclick="saveBtn_Click" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="cancelBtn" runat="server" onclick="cancelBtn_Click" 
        Text="Cancel" />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
    ConnectionString="<%$ ConnectionStrings:ConnectionString %>" >
    </asp:SqlDataSource>
</asp:Content>
