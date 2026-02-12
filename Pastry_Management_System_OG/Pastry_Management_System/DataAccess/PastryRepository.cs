using Pastry_Management_System.DataAccess;
using System.Data;
using System.Data.SqlClient;

namespace Pastery_Management_System.DataAccess
{
    public class PastryRepository //REpository class hides the sql,can able to fetch the data from database,can take up the data to the UI.
    {
        public DataSet GetPastries()//to take up the data from database, we need to use dataset, dataset is a collection of datatable, datatable is a collection of datarow, datarow is a collection of datacolumn.
        {
            DataSet ds = new DataSet();

            using (SqlConnection con = DbHelper.GetConnection())// help to make connection with database, using statement is used to dispose the connection after use, it will automatically close the connection after use.
            {
                string query = "SELECT ProductId, ProductName, ProductDescription, Price FROM Products";

                SqlDataAdapter da = new SqlDataAdapter(query, con);// to execute the query and fill the dataset, sqlDataAdapter is used to fill the dataset, it is a bridge between the dataset and the database, it is used to fill the dataset with the data from the database.It works automatically, it will open the connection, execute the query and fill the dataset, it will also close the connection after use.

                da.Fill(ds, "Products");
            }

            return ds;
        }
    }
}
