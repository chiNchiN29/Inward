<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BranchMaintenance.aspx.cs" Inherits="InwardClearingSystem.BranchMaintenance" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <script src="Scripts/jquery-1.8.3.min.js" type="text/javascript"></script>
        <script type="text/javascript" language="javascript">

            $(document).ready(function () {
                $("#<%=addBranch.ClientID %>").click(function (e) {
                    ShowDialog(true);
                    $("#<%=btnpopAdd.ClientID %>").show();
                    $("#<%=lblpopHeader.ClientID %>").text("Add Role");
                    $("#<%=txtBranchName.ClientID %>").val("");
                    $("#<%=txtBranchCode.ClientID %>").val("");
                    $("#<%=txtAddress.ClientID %>").val("");
                    e.preventDefault();
                });

                $("#<%=editBranch.ClientID %>").click(function (e) {
                    Parent = document.getElementById("<%=BranchView.ClientID%>");
                    var items = Parent.getElementsByTagName('input');
                    var isChecked = false;
                    for (i = 0; i < items.length; i++) {
                        if (items[i].type == "radio") {
                            if (items[i].checked) {
                                ShowDialog(true);
                                $("#<%=btnpopEdit.ClientID %>").show();
                                $("#<%=btnpopDel.ClientID %>").show();
                                var name = Parent.rows[i + 1].cells[1].innerHTML;
                                var code = Parent.rows[i + 1].cells[2].innerHTML;
                                var address = Parent.rows[i + 1].cells[3].innerHTML;
                                $("#<%=lblpopHeader.ClientID %>").text("Edit Role");
                                $("#<%=txtBranchName.ClientID %>").val(name);
                                $("#<%=txtBranchCode.ClientID %>").val(code);
                                $("#<%=txtAddress.ClientID %>").val(address);
                                $("#<%=editRow.ClientID %>").val(i + 1);
                                e.preventDefault();
                                isChecked = true;
                            }
                        }
                    }
                    if (!isChecked) {
                        alert("Please select a branch");
                    }
                });

                $("#btnClose").click(function (e) {
                    HideDialog();
                    e.preventDefault();
                });
            });

       function ShowDialog(modal)
       {
          $("#overlay").show();
          $("#dialog").fadeIn(300);

          if (modal)
          {
             $("#overlay").unbind("click");
          }
          else
          {
             $("#overlay").click(function (e)
             {
                HideDialog();
             });
          }
       }

       function HideDialog()
       {
          $("#overlay").hide();
          $("#dialog").fadeOut(300);
       } 
        </script>
        <style type="text/css">
            .web_dialog_overlay
            {
               position: fixed;
               top: 0;
               right: 0;
               bottom: 0;
               left: 0;
               height: 100%;
               width: 100%;
               margin: 0;
               padding: 0;
               background: #000000;
               opacity: .15;
               filter: alpha(opacity=15);
               -moz-opacity: .15;
               z-index: 101;
               display: none;
            }
            .web_dialog
            {
               display: none;
               position: fixed;
               width: 380px;
               height: 300px;
               top: 50%;
               left: 50%;
               margin-left: -190px;
               margin-top: -100px;
               background-color: #ffffff;
               border: 2px solid IndianRed;
               padding: 0px;
               z-index: 102;
               font-family: Verdana;
               font-size: 10pt;
            }
            .web_dialog_title
            {
               border-bottom: solid 2px IndianRed;
               background-color: IndianRed;
               padding: 4px;
               color: White;
               font-weight:bold;
            }
            .web_dialog_title a
            {
               color: White;
               text-decoration: none;
            }
            .align_right
            {
               text-align: right;
            }
            div.viewFunctions
            {
                float:left;
            }
            div.showNumber
            {
                width:50%;
                float:right;
                text-align:right;
                padding-top:20px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <p>BRANCH MAINTENANCE</p>
    <div class="gridWindow">
    <div class="gridTitleBar">Branches</div>
    <div class="viewFunctions">
        <asp:Button ID="searchBtn" runat="server" CausesValidation="false"
        Text="Search" onclick="searchBtn_Click" CssClass="defaultButton" />
    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="viewAllBtn" runat="server" onclick="viewAllBtn_Click" CausesValidation="false"
        Text="View All" CssClass="defaultButton" />
        </div>
    <div class="showNumber">
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
    </div>
        <asp:GridView ID="BranchView" runat="server" AutoGenerateColumns="false" CssClass="gridView" DataKeyNames="branch_id" 
        HeaderStyle-CssClass="GridHeader" AllowPaging="true" OnPageIndexChanging="BranchView_PageIndex" 
        PagerStyle-CssClass="paging" AllowSorting="true" OnSorting="BranchView_Sorting" ShowHeaderWhenEmpty="true" 
        AlternatingRowStyle-BackColor="#FFEFEF">
            <Columns>
             <asp:TemplateField>
                <ItemTemplate>
                    <asp:RadioButton ID="RowSelect" runat="server" OnClick="javascript:CheckOtherIsCheckedByGVID(this);" />
                </ItemTemplate>
             </asp:TemplateField>
             <asp:BoundField DataField="branch_id" Visible="false" />
             <asp:BoundField DataField="branch_name" SortExpression="branch_name" HeaderText="Branch Name" />
             <asp:BoundField DataField="branch_code" SortExpression="branch_code" HeaderText="Branch Code" />
             <asp:BoundField DataField="address" HeaderText="Address" />
            </Columns>
        </asp:GridView>
        </div>

    <asp:Button ID="addBranch" runat="server" Text="Add" CssClass="defaultButton" CausesValidation="false" /> 
    <asp:Button ID="editBranch" runat="server" Text="Edit" CssClass="defaultButton" CausesValidation="false" />
    
    <div id="overlay" class="web_dialog_overlay"></div>
    <div id="dialog" class="web_dialog">
        <table style="width: 100%; border: 0px;" cellpadding="3" cellspacing="0">
            <tr>
                <td class="web_dialog_title"><asp:Label ID="lblpopHeader" runat="server" CssClass="web_dialog_title" /></td>
                <td class="web_dialog_title align_right">
                    <a href="#" id="btnClose">X</a>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="width: 45%; text-align: center;">
                    <asp:Label ID="LabelValidate" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 45%">
                    Branch Name:
                </td>
                <td>
                    <asp:TextBox ID="txtBranchName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="BranchNameValidator" runat="server" ControlToValidate="txtBranchName"
                    ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 45%">
                    Branch Code:
                </td>
                <td>
                    <asp:TextBox ID="txtBranchCode" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="BranchCodeValidator" runat="server" ControlToValidate="txtBranchCode"
                    ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 45%">
                    Address:
                </td>
                <td>
                    <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="AddressValidator" runat="server" ControlToValidate="txtAddress" 
                    ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr> 
                <td>
                </td>
                <td>
                    <asp:Button ID="btnpopAdd" runat="server" Text="Add" OnClick="btnpopAdd_Click" style="display: none" />
                    <asp:Button ID="btnpopEdit" runat="server" Text="Save" OnClick="btnpopEdit_Click" style="display: none" />
                    <asp:Button ID="btnpopCancel" runat="server" Text="Cancel" OnClick="btnpopCancel_Click" CausesValidation="false" />
                    <asp:Button ID="btnpopDel" runat="server" Text="Delete" CssClass="defaultButton" onclick="delBranch_Click" 
                    OnClientClick="return DeleteItem()" style="display: none" />
                </td>
            </tr>
        </table>
    </div>
 
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

    <asp:HiddenField ID="editRow" runat="server" />

</asp:Content>