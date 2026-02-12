using System.Configuration;
using System.Data.SqlClient;

namespace Pastry_Management_System.DataAccess
{
    public static class DbHelper
    {
        public static SqlConnection GetConnection()
        {
            //string cs = ConfigurationManager.ConnectionStrings["PastryDBConnection"].ConnectionString;
            string cs = ConfigurationManager
                       .ConnectionStrings["PastryDBConnection"]
                       .ConnectionString;

            //using (SqlConnection con = new SqlConnection(cs))
            //{
            //    con.Open();
            //    // DB is now connected 🎉
            //}


            return new SqlConnection(cs);
        }
    }
}
