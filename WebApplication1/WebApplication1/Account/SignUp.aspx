﻿<%@ Page Title="Sign Up" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="WebApplication1.Account.SignUp" %>
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
    <asp:CompareValidator ID="CompareValidator1" runat="server" 
        ControlToCompare="TextBox3" ControlToValidate="TextBox4" 
        ErrorMessage="The passwords do not match." ForeColor="Red"></asp:CompareValidator>
<br />
<table>
    <tr>
        <td>User Name:</td>
        <td>&nbsp;</td>
        <td><asp:TextBox ID="unTxtBx" runat="server" ToolTip="This field is REQUIRED."></asp:TextBox></td>
        <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="TextBox1"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td>First Name:</td>
        <td>&nbsp;</td>
        <td><asp:TextBox ID="fnTxtBx" runat="server" ToolTip="This field is REQUIRED." 
                CssClass="style2"></asp:TextBox></td>
        <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="TextBox1"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td>Last Name:</td>
        <td>&nbsp;</td>
        <td><asp:TextBox ID="lnTxtBx" runat="server" ToolTip="This field is REQUIRED."></asp:TextBox></td>
        <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="TextBox1"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td>E-mail:</td>
        <td>&nbsp;</td>
        <td><asp:TextBox ID="emTxtBx" runat="server" TextMode="Email" 
                ToolTip="This field is REQUIRED."></asp:TextBox></td>
        <td><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="TextBox2"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td class="style1">Password:</td>
        <td class="style1"></td>
        <td class="style1">
            <asp:TextBox ID="passTxtBx" runat="server" TextMode="Password" 
                ToolTip="Requires at least 8 characters."></asp:TextBox>
            <cc1:PasswordStrength ID="passtxtBx_PasswordStrength" runat="server" 
                BehaviorID="TextBox3_PasswordStrength" TargetControlID="passtxtBx" 
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
        <td><asp:TextBox ID="cpassTxtBx" runat="server" TextMode="Password"></asp:TextBox></td>
        <td><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ErrorMessage="This field is required." ForeColor="Red" 
                ControlToValidate="TextBox3"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td id="buttonstuff" colspan='3' align="center">
            <asp:Button ID="regBtn" 
                runat="server" Text="Register" onclick="regBtn_Click" /></td>
    </tr>
</table>
    <br />
    </asp:Content>
