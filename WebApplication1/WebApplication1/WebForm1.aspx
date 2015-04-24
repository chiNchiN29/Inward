<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication1.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <p style="height: 30px">
            <asp:Button ID="uploadDoc" runat="server" Text="Upload Document" OnClick="uploadDoc_Click" />
               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               </p>
            <asp:Button ID="loadDoc" runat="server" Text="Load Document" OnClick="loadDoc_Click" />
            <asp:Label ID="Label1" runat="server" Text="1_01"></asp:Label>
        <asp:Label ID="Label2" runat="server" Text="1_02"></asp:Label>
        <asp:Label ID="Label3" runat="server" Text="1_03"></asp:Label>
        <asp:Label ID="Label4" runat="server" Text="2_01"></asp:Label>
        <asp:Label ID="Label5" runat="server" Text="2_02"></asp:Label>
        <asp:Label ID="Label6" runat="server" Text="2_03"></asp:Label>
        <asp:Label ID="Label7" runat="server" Text="3_01"></asp:Label>
        <asp:Label ID="Label8" runat="server" Text="3_02"></asp:Label>
        <asp:Label ID="Label9" runat="server" Text="3_03"></asp:Label>
        <asp:Label ID="Label10" runat="server" Text="3_04"></asp:Label>
            <p>
            <asp:Image ID="Image1" runat="server" Height="211px" Width="556px" 
                    Visible="False" />
                <asp:Image ID="Image2" runat="server" Height="185px" Width="562px" 
                    Visible="False" />
        </p>
    </div>
    </form>
</body>
</html>
