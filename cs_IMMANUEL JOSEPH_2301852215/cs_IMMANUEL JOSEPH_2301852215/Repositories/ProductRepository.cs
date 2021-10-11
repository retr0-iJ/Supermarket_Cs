using cs_IMMANUEL_JOSEPH_2301852215.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace cs_IMMANUEL_JOSEPH_2301852215.Repositories
{
    public class ProductRepository
    {
        public List<ProductModel> ViewProducts()
        {
            DBConnection.Init();
            SqlDataReader reader;

            List<ProductModel> productList = new List<ProductModel>();

            string query = "View_All_Products";

            DBConnection.command.Connection = DBConnection.connection;
            DBConnection.command.CommandType = CommandType.StoredProcedure;
            DBConnection.command.CommandText = query;

            DBConnection.connection.Open();

            reader = DBConnection.command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ProductModel product = new ProductModel();
                    product.ProductID = Convert.ToInt32(reader["ProductID"].ToString());
                    product.ProductName = reader["ProductName"].ToString();
                    product.ProductPrice = Convert.ToInt32(reader["ProductPrice"]);
                    product.ProductQty = Convert.ToInt32(reader["ProductQty"]);

                    productList.Add(product);
                }
            }

            reader.Close();
            DBConnection.command.Dispose();
            DBConnection.connection.Close();

            return productList;
        }

        public void InsertProduct(ProductModel product)
        {
            DBConnection.Init();

            string query = "Insert_Product";

            DBConnection.command.Parameters.Add("@ProductName", SqlDbType.VarChar, 21).Value = product.ProductName;
            DBConnection.command.Parameters.Add("@ProductPrice", SqlDbType.Int).Value = product.ProductPrice;
            DBConnection.command.Parameters.Add("@ProductQty", SqlDbType.Int).Value = product.ProductQty;

            DBConnection.command.Connection = DBConnection.connection;
            DBConnection.command.CommandType = CommandType.StoredProcedure;
            DBConnection.command.CommandText = query;

            DBConnection.connection.Open();

            DBConnection.command.ExecuteNonQuery();

            DBConnection.command.Dispose();
            DBConnection.connection.Close();
        }

        public void UpdateProduct(ProductModel product)
        {
            DBConnection.Init();

            string query = "Update_Product";

            DBConnection.command.Parameters.Add("@ProductID", SqlDbType.Int).Value = product.ProductID;
            DBConnection.command.Parameters.Add("@ProductName", SqlDbType.VarChar, 21).Value = product.ProductName;
            DBConnection.command.Parameters.Add("@ProductPrice", SqlDbType.Int).Value = product.ProductPrice;
            DBConnection.command.Parameters.Add("@ProductQty", SqlDbType.Int).Value = product.ProductQty;

            DBConnection.command.Connection = DBConnection.connection;
            DBConnection.command.CommandType = CommandType.StoredProcedure;
            DBConnection.command.CommandText = query;

            DBConnection.connection.Open();

            DBConnection.command.ExecuteNonQuery();

            DBConnection.command.Dispose();
            DBConnection.connection.Close();
        }

        public void DeleteProduct(int ProductID)
        {
            DBConnection.Init();

            string query = "Delete_Product";

            DBConnection.command.Parameters.Add("@ProductID", SqlDbType.Int).Value = ProductID;

            DBConnection.command.Connection = DBConnection.connection;
            DBConnection.command.CommandType = CommandType.StoredProcedure;
            DBConnection.command.CommandText = query;

            DBConnection.connection.Open();

            DBConnection.command.ExecuteNonQuery();

            DBConnection.command.Dispose();
            DBConnection.connection.Close();
        }
    }
}
