<%@ Page Title="Sign Up" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="WebApplication1.Account.SignUp" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            height: 20px;
        }
        .style2
        {
            height: 22px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Register New User</h2>
    <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="Label" 
        Visible="False"></asp:Label>
    <br />
<br />
<table>
    <tr>
        <td>User Name:</td>
        <td>&nbsp;</td>
        <td><asp:TextBox ID="unTxtBx" runat="server" ToolTip="This field is REQUIRED."></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="unTxtBx"></asp:RequiredFieldValidator></td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>First Name:</td>
        <td>&nbsp;</td>
        <td><asp:TextBox ID="fnTxtBx" runat="server" ToolTip="This field is REQUIRED." 
                CssClass="style2"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="unTxtBx"></asp:RequiredFieldValidator></td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>Last Name:</td>
        <td>&nbsp;</td>
        <td><asp:TextBox ID="lnTxtBx" runat="server" ToolTip="This field is REQUIRED."></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="unTxtBx"></asp:RequiredFieldValidator></td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>E-mail:</td>
        <td>&nbsp;</td>
        <td><asp:TextBox ID="emTxtBx" runat="server" TextMode="Email" 
                ToolTip="This field is REQUIRED."></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="emTxtBx"></asp:RequiredFieldValidator></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="style1">Password:</td>
        <td class="style1"></td>
        <td class="style1">
            <asp:TextBox ID="passTxtBx" runat="server" TextMode="Password" 
                ToolTip="Requires at least 8 characters."></asp:TextBox>
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
        <td class="style1">
            <asp:Label ID="Label5" runat="server" Text="Label" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>Confirm Password:</td>
        <td>&nbsp;</td>
        <td><asp:TextBox ID="cpassTxtBx" runat="server" TextMode="Password"></asp:TextBox>
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
        <td id="buttonstuff" colspan='3' align="center">
            <asp:Button ID="regBtn" 
                runat="server" Text="Register" onclick="regBtn_Click" /></td>
    </tr>
</table>
    <br />
    </asp:Content>
