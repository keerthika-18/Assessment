using Pastery_Management_System.DataAccess;
using Pastry_Management_System.DataAccess;
using System;
using System.Data;

namespace Pastery_Management_System
{
    public partial class Default : System.Web.UI.Page
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {
        //        LoadPastries();
        //    }
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (var con = DbHelper.GetConnection())
                {
                    con.Open();
                    Response.Write("✅ Connected to PasteryDB");
                }
            }
            catch (Exception ex)
            {
                Response.Write("❌ DB ERROR: " + ex.Message);
            }

        }

        private void LoadPastries()
        {
            PastryRepository repo = new PastryRepository();
            DataSet ds = repo.GetPastries();

            gvProducts.DataSource = ds.Tables["Products"];
            gvProducts.DataBind();
        }
    }
}
