﻿<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="InwardClearingSystem.Account.Login" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .labelPositions
        {
            width: 100px;
        }
        .loginScreen
        {
            height:260px;
            width:380px;
            text-align:center;
            float: right;
            font-family:Segoe UI;
            background-image: -webkit-gradient(linear, left bottom, left top, color-stop(0, #FFFFFF), color-stop(1, #B5B5B5)) !important;
            border-radius:10px;
        }
        .textBoxPositions
        {
            width: 331px;
            text-align:center;
        }
        div.leftSide
        {
            width:426px;
            height:240px;
            float:left;
        }
        div.logoImage
        {
            float:left;
        }
        div.registerHyperLink
        {
            float:left;
            padding-left:22px;
            width: 250px;   
        }
        div.userScreen
        {
            height:320px;
            width:61%;  
            margin:0 auto; 
        }
        div.whiteSpace
        {
            height:50px;   
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="whiteSpace">
        
    </div>
    <div class="userScreen">
    <div class="leftSide">
        <br />
        <div class="logoImage">
            <img src="../Resources/H2ics.png">
        </div>
    </div>
    <asp:Login ID="Login1" runat="server" onauthenticate="authenticate_Click" 
        BackColor="White" Font-Size="1.2em" 
        ForeColor="#333333" CssClass="loginScreen">
        <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
        <LayoutTemplate>
            <table>
                <tr>
                    <td>
                        <table cellpadding="5">
                            <tr>
                                <td colspan="2" 
                                    style="color:#333333;font-size:1.3em">
                                    USER LOGIN</td>
                            </tr>
                            <tr>
                                <td colspan="2" style="color:#990000; font-style:italic;">Please Enter your Username and Password.</td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="color:Red;">
                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="labelPositions">
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" >Username:</asp:Label>
                                </td>
                                <td class="textBoxPositions">
                                    <asp:TextBox ID="UserName" runat="server" Font-Size="0.8em" Width="75%" CssClass="textbox"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                        ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                        ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="labelPositions">
                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                </td>
                                <td class="textBoxPositions">
                                    <asp:TextBox ID="Password" runat="server" Font-Size="0.8em" TextMode="Password" Width="75%" CssClass="textbox"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                        ControlToValidate="Password" ErrorMessage="Password is required." 
                                        ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right" style="padding-right:50px">
                                    <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." 
                                        Font-Italic="True" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td align="right" colspan="2" style="padding-right:50px">
                                    <asp:Button ID="LoginButton" runat="server"  
                                        CommandName="Login" 
                                        Font-Size="1.0em" Text="Login" 
                                        ValidationGroup="Login1" Height="35px" Width="90px" CssClass="defaultButton"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
        <LoginButtonStyle BackColor="#FFFBFF" Font-Size="0.8em" ForeColor="#284775" />
        <TextBoxStyle Font-Size="0.8em" />
        <TitleTextStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="0.9em" 
            ForeColor="White" />
    </asp:Login>
        <cc1:DropShadowExtender ID="Login1_DropShadowExtender" runat="server" 
            BehaviorID="Login1_DropShadowExtender" TargetControlID="Login1" />
    </div>
</asp:Content>
