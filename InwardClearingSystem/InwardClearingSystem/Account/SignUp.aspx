<%@ Page Title="Sign Up" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="InwardClearingSystem.Account.SignUp" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .table
        {
            background-color:Silver;
            border-radius:5px;
            border:1px solid #666666;
            padding:1px 5px 0px 1px;
        }
        .title
        {
            font-family:Franklin Gothic Medium;
            color:#990000;
            font-size:x-large;
            padding-left:10px; 
        }
        .label
        {
            font-family:Franklin Gothic Medium;
            color:Black;
        }
        .textbox
        {
            height: 20px;
        }
        .button
        {
            font-size:1.0em; 
            font-family:Franklin Gothic Medium;
            background-color:#990000;
            color:#FFFFFF;  
            height:35px; 
            width:90px;
        }
        .button:hover
        {
            background:ButtonHighlight;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<table class="table">
    <tr>
        <td class="title" colspan="5">Register New User</td>
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
        <td class="label">Username:</td>
        <td><asp:TextBox ID="unTxtBx" runat="server" ToolTip="This field is REQUIRED." CssClass="textbox"></asp:TextBox>
            <cc1:RoundedCornersExtender ID="unTxtBx_RoundedCornersExtender" runat="server" 
                BehaviorID="unTxtBx_RoundedCornersExtender" TargetControlID="unTxtBx" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="unTxtBx"></asp:RequiredFieldValidator></td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="label">First Name:</td>
        <td><asp:TextBox ID="fnTxtBx" runat="server" ToolTip="This field is REQUIRED." 
                CssClass="textbox"></asp:TextBox>
            <cc1:RoundedCornersExtender ID="fnTxtBx_RoundedCornersExtender" runat="server" 
                BehaviorID="fnTxtBx_RoundedCornersExtender" TargetControlID="fnTxtBx" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="unTxtBx"></asp:RequiredFieldValidator></td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="label">Middle Name:</td>
        <td><asp:TextBox ID="mnTxtBx" runat="server" ToolTip="This field is REQUIRED." CssClass="textbox"></asp:TextBox>
            <cc1:RoundedCornersExtender ID="RoundedCornersExtender1" runat="server" 
                BehaviorID="mnTxtBx_RoundedCornersExtender" TargetControlID="mnTxtBx" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="mnTxtBx"></asp:RequiredFieldValidator></td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="label">Last Name:</td>
        <td><asp:TextBox ID="lnTxtBx" runat="server" ToolTip="This field is REQUIRED." CssClass="textbox"></asp:TextBox>
            <cc1:RoundedCornersExtender ID="lnTxtBx_RoundedCornersExtender" runat="server" 
                BehaviorID="lnTxtBx_RoundedCornersExtender" TargetControlID="lnTxtBx" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="lnTxtBx"></asp:RequiredFieldValidator></td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="label">E-mail:</td>
        <td><asp:TextBox ID="emTxtBx" runat="server" TextMode="Email" 
                ToolTip="This field is REQUIRED." CssClass="textbox"></asp:TextBox>
            <cc1:RoundedCornersExtender ID="emTxtBx_RoundedCornersExtender" runat="server" 
                BehaviorID="emTxtBx_RoundedCornersExtender" TargetControlID="emTxtBx" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="emTxtBx"></asp:RequiredFieldValidator></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="label">Password:</td>
        <td>
            <asp:TextBox ID="passTxtBx" runat="server" TextMode="Password" 
                ToolTip="Requires at least 8 characters." CssClass="textbox"></asp:TextBox>
            <cc1:RoundedCornersExtender ID="passTxtBx_RoundedCornersExtender" 
                runat="server" BehaviorID="passTxtBx_RoundedCornersExtender" 
                TargetControlID="passTxtBx" />
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
        <td class="label">Confirm Password:</td>
        <td><asp:TextBox ID="cpassTxtBx" runat="server" TextMode="Password" CssClass="textbox"></asp:TextBox>
            <cc1:RoundedCornersExtender ID="cpassTxtBx_RoundedCornersExtender" 
                runat="server" BehaviorID="cpassTxtBx_RoundedCornersExtender" 
                TargetControlID="cpassTxtBx"/>
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
                runat="server" Text="Register" onclick="regBtn_Click" CssClass="button"/>
            <cc1:RoundedCornersExtender ID="regBtn_RoundedCornersExtender" runat="server" 
                BehaviorID="regBtn_RoundedCornersExtender" TargetControlID="regBtn" />
        </td>
    </tr>
</table>
    <br />
    </asp:Content>
