<%@ Page Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Products.aspx.cs"
    Inherits="Pastry_Management_System.Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Pastry Products</h2>

    <asp:GridView 
        ID="gvProducts"
        runat="server"
        AutoGenerateColumns="False"
        DataKeyNames="ProductId"
        OnRowCommand="gvProducts_RowCommand"
        OnRowDataBound="gvProducts_RowDataBound">

        <Columns>
            <asp:BoundField DataField="ProductName" HeaderText="Product" />
            <asp:BoundField DataField="Price" HeaderText="Price" />
            <asp:BoundField DataField="AvailableQty" HeaderText="Available" />

            <asp:TemplateField HeaderText="Quantity">
                <ItemTemplate>
                    <asp:TextBox ID="txtQty" runat="server" Text="1" Width="60" />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Total">
                <ItemTemplate>
                    ₹<asp:Label ID="lblTotal" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button 
    ID="btnOrder"
    runat="server"
    Text="Add to Cart"
    CommandName="AddToCart"
    CommandArgument="<%# Container.DataItemIndex %>" />

                </ItemTemplate>
            </asp:TemplateField>
        </Columns>

    </asp:GridView>

    <br />

    <!-- ✅ ORDER TOTAL MUST BE HERE -->
    <asp:Label 
        ID="lblOrderTotal"
        runat="server"
        Font-Bold="true"
        Font-Size="Large"
        ForeColor="DarkGreen">
    </asp:Label>
    <br />
<asp:Button 
    ID="btnPlaceOrder"
    runat="server"
    Text="Place Order"
    CssClass="btn btn-success"
    OnClick="btnPlaceOrder_Click" />

    <br />
    <asp:Label ID="lblMessage" runat="server"></asp:Label>

</asp:Content>
