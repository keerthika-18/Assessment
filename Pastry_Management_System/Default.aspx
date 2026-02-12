<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="Pastery_Management_System.Default" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Pastry List</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Pastry Products</h2>

        <asp:GridView ID="gvProducts" runat="server"
            AutoGenerateColumns="true"
            BorderWidth="1">
        </asp:GridView>
    </form>
</body>
</html>

