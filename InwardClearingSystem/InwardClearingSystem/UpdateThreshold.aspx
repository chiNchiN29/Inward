<%@ Page Title="Update Thresholds" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UpdateThreshold.aspx.cs" Inherits="InwardClearingSystem.UpdateThreshold" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
    #container
    {
        width: 700px;
        margin: auto;
    }
    #min
    {
        text-align:center;
        width: 300px;
        float: left;
        height: 130px;
    }
    #max
    {
        text-align:center;
        width: 300px;
        float: right;
        height: 130px;
    }
    #clear
    {
        clear:both;
    }
    #clear
    {
        text-align:center;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="container">
    <br />
    <br />
    <div id="min">
        <asp:Label ID="Label1" runat="server" Text="Current Minimum Threshold Amount:"></asp:Label>
        <br />
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
        <br />
        <br />
        <asp:Label ID="Label3" runat="server" Text="New Minimum Threshold Amount:"></asp:Label>
        <br />
        <asp:TextBox ID="TextBox1" runat="server" TextMode="Number"></asp:TextBox>
        <br />
    </div>
    

    <div id="max">
        <asp:Label ID="Label4" runat="server" Text="Current Maximum Threshold Amount:"></asp:Label>
        <br />
        <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label>
        <br />
        <br />
        <asp:Label ID="Label6" runat="server" Text="New Maximum Threshold Amount:"></asp:Label>
        <br />
        <asp:TextBox ID="TextBox2" runat="server" TextMode="Number"></asp:TextBox>
        <br />
    </div>

    <div id="clear">
    <asp:Button ID="Button1" runat="server" onclick="SetThresholds" Text="Set" OnClientClick="return ConfirmSettings();"
    Width="91px" />
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
