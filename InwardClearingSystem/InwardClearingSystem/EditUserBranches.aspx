<%@ Page Title="User Branches" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditUserBranches.aspx.cs" Inherits="InwardClearingSystem.EditUserBranches" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 
 
    
    Edit User Branches<br />
    <br />
    Choose User:<br />
    <asp:DropDownList ID="UserDrpDwn" runat="server">
    </asp:DropDownList>
    <br />
 
 
    
    <asp:GridView ID="branchView" runat="server" AutoGenerateColumns="false" CssClass="gridView2" HeaderStyle-CssClass="GridHeader" 
    OnPageIndexChanging="branchView_PageIndex" AllowPaging="true" PagerStyle-CssClass="paging" AllowSorting="true" OnSorting="BranchView_Sorting" >
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:CheckBox ID="chkBoxAll" runat="server" onclick="GridSelectAllColumn(this);" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="chkBox" runat="server" OnCheckedChanged="chkBox_CheckedChanged" AutoPostBack="true" />
            </ItemTemplate>
        </asp:TemplateField>
    <asp:BoundField DataField="branch_name" SortExpression="branch_name" HeaderText="Branch Name" ItemStyle-Width="100px" /> 
    <asp:BoundField DataField="username" SortExpression="username" HeaderText="Current Handler" ItemStyle-Width="100px" />
    </Columns>
    </asp:GridView>
    *Please save before going to next page<br />
    <asp:Button ID="saveBtn" runat="server" Text="Save" onclick="saveBtn_Click" CssClass="yesButton" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="backBtn" runat="server" onclick="backBtn_Click" 
        Text="Back" CssClass="noButton" />
        <script type="text/javascript" language="javascript">
            function GridSelectAllColumn(spanChk) {
                var oItem = spanChk.children;
                var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0]; xState = theBox.checked;
                elm = theBox.form.elements;

                for (i = 0; i < elm.length; i++) {
                    if (elm[i].type === 'checkbox' && elm[i].checked != xState)
                        elm[i].click();
                }
            }
    </script>
    </asp:Content>
