<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" Inherits="InwardClearingSystem.EditUser" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"><style type="text/css">
        .label
        {
            font-family:Segoe UI;
            color:Black;
        }
        .table
        {
            background-color:Silver;
            border-radius:5px;
            border:1px solid #666666;
            padding:1px 5px 0px 1px;
        }
        .title
        {
            font-family:Segoe UI;
            color:#990000;
            font-size:x-large;
            padding-left:10px; 
        }
        .style1
        {
            font-family: Segoe UI;
            color: Black;
            width: 223px;
        }
        .style2
        {
            width: 223px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table class="table">
    <tr>
        <td class="title" colspan="5">Edit User Information</td>
    </tr>
    <tr>
        <td colspan="5">
            <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="Label" 
            Visible="False"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="5">&nbsp;</td>
    </tr>
    <tr>
        <td class="style1">Username:</td>
        <td><asp:TextBox ID="unTxtBx" runat="server" ToolTip="This field is REQUIRED." CssClass="textbox"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="unTxtBx"></asp:RequiredFieldValidator></td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style1">First Name:</td>
        <td><asp:TextBox ID="fnTxtBx" runat="server" ToolTip="This field is REQUIRED." 
                CssClass="textbox"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="unTxtBx"></asp:RequiredFieldValidator></td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style1">Middle Name:</td>
        <td><asp:TextBox ID="mnTxtBx" runat="server" ToolTip="This field is REQUIRED." CssClass="textbox"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="mnTxtBx"></asp:RequiredFieldValidator></td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style1">Last Name:</td>
        <td><asp:TextBox ID="lnTxtBx" runat="server" ToolTip="This field is REQUIRED." CssClass="textbox"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="lnTxtBx"></asp:RequiredFieldValidator></td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style1">E-mail:</td>
        <td><asp:TextBox ID="emTxtBx" runat="server" TextMode="Email" 
                ToolTip="This field is REQUIRED." CssClass="textbox"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="emTxtBx"></asp:RequiredFieldValidator></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="style1">Password:</td>
        <td>
            <asp:TextBox ID="passTxtBx" runat="server" TextMode="Password" 
                ToolTip="Requires at least 8 characters." CssClass="textbox"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="passTxtBx"></asp:RequiredFieldValidator>
            <cc1:PasswordStrength ID="passtxtBx_PasswordStrength" runat="server" 
                BehaviorID="passTxtBx_PasswordStrength" TargetControlID="passtxtBx" 
                DisplayPosition="RightSide"
                MinimumSymbolCharacters="0"
                MinimumUpperCaseCharacters="1"
                PreferredPasswordLength="8"
                CalculationWeightings="25;25;15;35"
                RequiresUpperAndLowerCaseCharacters="true"
                TextStrengthDescriptions="Poor; Weak; Good; Strong; Excellent"
                HelpStatusLabelID="Label5"
                StrengthIndicatorType="BarIndicator"
                HelpHandlePosition="AboveLeft"
                BarBorderCssClass="barIndicatorBorder"
                
                StrengthStyles="barIndicator_poor; barIndicator_weak; barIndicator_good; barIndicator_strong; barIndicator_excellent"/>
        </td>
        <td class="label">
            <asp:Label ID="Label5" runat="server" Text="Label" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="style1">Confirm Password:</td>
        <td><asp:TextBox ID="cpassTxtBx" runat="server" TextMode="Password" CssClass="textbox"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="cpassTxtBx"></asp:RequiredFieldValidator></td>          
    </tr>
    <tr>
    <asp:CompareValidator ID="CompareValidator1" runat="server" 
        ControlToCompare="passTxtBx" ControlToValidate="cpassTxtBx" 
        ErrorMessage="The passwords do not match." ForeColor="Red"></asp:CompareValidator>
        </tr>
    <tr>
        <td id="buttonstuff" align="center">
            <asp:Button ID="editBtn" 
                runat="server" Text="Update" onclick="editBtn_Click" CssClass="defaultButton"/>
        </td>
        <td id="Td1" align="center">
            <asp:Button ID="cancelBtn" CausesValidation="false" 
                runat="server" Text="Cancel" onclick="canceBtn_Click" CssClass="defaultButton"/>
        </td>
        <td>&nbsp</td>
    </tr>
    <tr>
        <td colspan='3'>
            &nbsp;
        </td>
    </tr>
</table>
    <br />
</asp:Content>
