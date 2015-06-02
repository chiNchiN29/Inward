﻿<%@ Page Title="Update Thresholds" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UpdateThreshold.aspx.cs" Inherits="InwardClearingSystem.UpdateThreshold" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
    .money
    {
        color:#C19132;
    }
    div.clear
    {
        clear:both;
        text-align:center;
    }
    div.container
    {
        width: 50%;
        height:250px;
        margin: auto;
        background-color: #f1f1f1;
        border:2px solid #990000;
    }
    div.max
    {
        
        text-align:center;
        font-family:Segoe UI;
        font-size:larger;
        padding-top:20px;
        color:#990000;
        width: 300px;
        float: right;
        height: 130px;
    }
    div.min
    {
        text-align:center;
        font-family:Segoe UI;
        font-size:larger;
        padding-top:20px;
        color:#990000;
        width: 300px;
        float: left;
        height: 130px;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="whiteSpace">&nbsp;</div>
    <div class="container">
        <div class="gridTitleBar" style="background-color:#990000; border:1px solid #990000; width:98.9%">Set New Thresholds</div>
        <div class="min">
            <asp:Label ID="Label1" runat="server" Text="Current <strong>Minimum</strong> Threshold Amount:"></asp:Label>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Label" CssClass="money"></asp:Label>
            <br />
            <br />
            <asp:Label ID="Label3" runat="server" Text="New Minimum Threshold Amount:"></asp:Label>
            <br />
            <asp:TextBox ID="TextBox1" runat="server" TextMode="Number" CssClass="textbox"></asp:TextBox>
            <br />
        </div>
    

        <div class="max">
            <asp:Label ID="Label4" runat="server" Text="Current <strong>Maximum</strong> Threshold Amount:"></asp:Label>
            <br />
            <asp:Label ID="Label5" runat="server" Text="Label" CssClass="money"></asp:Label>
            <br />
            <br />
            <asp:Label ID="Label6" runat="server" Text="New Maximum Threshold Amount:"></asp:Label>
            <br />
            <asp:TextBox ID="TextBox2" runat="server" TextMode="Number" CssClass="textbox"></asp:TextBox>
            <br />
        </div>

        <div class="clear">
        <asp:Button ID="Button1" runat="server" onclick="SetThresholds" Text="Set" OnClientClick="return ConfirmSettings();"
        Width="91px" CssClass="defaultButton"/>
            <cc1:RoundedCornersExtender ID="Button1_RoundedCornersExtender" runat="server" 
                BehaviorID="Button1_RoundedCornersExtender" TargetControlID="Button1">
            </cc1:RoundedCornersExtender>
        </div>
</div>
    <br />

    <script type="text/javascript">

        function ConfirmSettings() {
            if (confirm("Are you sure you want to change the settings?")) {
                return true;
            }
            return false;
        }

</script>
</asp:Content>
