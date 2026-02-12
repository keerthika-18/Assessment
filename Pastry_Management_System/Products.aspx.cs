using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pastry_Management_System
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
       
    }

    public partial class Products : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        private void LoadProducts()
        {
            string cs = ConfigurationManager
                .ConnectionStrings["PastryDBConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
            SELECT 
                p.ProductId,
                p.ProductName,
                p.Price,
                i.Quantity AS AvailableQty
            FROM Products p
            JOIN Inventory i ON p.ProductId = i.ProductId";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvProducts.DataSource = dt;
                gvProducts.DataBind();
            }
        }

        protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                decimal price = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Price")); 
                int availableQty = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "AvailableQty"));

                TextBox txtQty = (TextBox)e.Row.FindControl("txtQty");
                Label lblTotal = (Label)e.Row.FindControl("lblTotal");
                Button btnOrder = (Button)e.Row.FindControl("btnOrder");

                int qty = Convert.ToInt32(txtQty.Text);
                lblTotal.Text = (price * qty).ToString("0.00");

                if (availableQty <= 0)
                {
                    btnOrder.Enabled = false;
                    btnOrder.Text = "Out of Stock";
                    lblTotal.Text = "0.00";
                }
            }
        }


       
        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddToCart")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvProducts.Rows[rowIndex];

                int productId = Convert.ToInt32(gvProducts.DataKeys[rowIndex].Value);
                string productName = row.Cells[0].Text;
                decimal price = Convert.ToDecimal(row.Cells[1].Text);
                int availableQty = Convert.ToInt32(row.Cells[2].Text);

                TextBox txtQty = (TextBox)row.FindControl("txtQty");
                int quantity = Convert.ToInt32(txtQty.Text);

                if (quantity > availableQty)
                {
                    lblMessage.Text = " Quantity not available";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }
               
                List<CartItem> cart = Session["CART"] as List<CartItem>;
                if (cart == null)
                    cart = new List<CartItem>();

            
                CartItem existing = cart.FirstOrDefault(p => p.ProductId == productId);
                if (existing != null)
                {
                    existing.Quantity += quantity;
                }
                else
                {
                    cart.Add(new CartItem
                    {
                        ProductId = productId,
                        ProductName = productName,
                        Price = price,
                        Quantity = quantity
                    });
                }

                Session["CART"] = cart;

                UpdateCartTotal(cart);

                lblMessage.Text = " Item added to cart";
                lblMessage.ForeColor = System.Drawing.Color.Green;
            }
        }
        private void UpdateCartTotal(List<CartItem> cart)
        {
            decimal grandTotal = cart.Sum(i => i.Price * i.Quantity);
            lblOrderTotal.Text = "Cart Total: ₹" + grandTotal.ToString("0.00");
        }


       
        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            List<CartItem> cart = Session["CART"] as List<CartItem>;

            if (cart == null || cart.Count == 0)
            {
                lblMessage.Text = "Cart is empty";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            decimal total = cart.Sum(i => i.Price * i.Quantity);

            string cs = ConfigurationManager
                .ConnectionStrings["PastryDBConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    SqlCommand orderCmd = new SqlCommand(
                        "INSERT INTO Orders (OrderDate, TotalPrice) OUTPUT INSERTED.OrderId VALUES (GETDATE(), @total)",
                        con, tran);

                    orderCmd.Parameters.AddWithValue("@total", total);
                    int orderId = (int)orderCmd.ExecuteScalar();

                    foreach (var item in cart)
                    {
                        SqlCommand itemCmd = new SqlCommand(
                            "INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price) VALUES (@oid,@pid,@qty,@price)",
                            con, tran);

                        itemCmd.Parameters.AddWithValue("@oid", orderId);
                        itemCmd.Parameters.AddWithValue("@pid", item.ProductId);
                        itemCmd.Parameters.AddWithValue("@qty", item.Quantity);
                        itemCmd.Parameters.AddWithValue("@price", item.Price * item.Quantity);
                        itemCmd.ExecuteNonQuery();

                        SqlCommand invCmd = new SqlCommand(
                            "UPDATE Inventory SET Quantity = Quantity - @qty WHERE ProductId=@pid",
                            con, tran);

                        invCmd.Parameters.AddWithValue("@qty", item.Quantity);
                        invCmd.Parameters.AddWithValue("@pid", item.ProductId);
                        invCmd.ExecuteNonQuery();
                    }

                    tran.Commit();

                    Session["CART"] = null;
                    lblOrderTotal.Text = "";
                    lblMessage.Text = " Order placed successfully!";
                    lblMessage.ForeColor = System.Drawing.Color.Green;

                    LoadProducts();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }//exception handling can be improved by logging the error and showing a user-friendly message instead of rethrowing it.
        }



    }
}
