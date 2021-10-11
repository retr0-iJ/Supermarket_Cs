using cs_IMMANUEL_JOSEPH_2301852215.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace cs_IMMANUEL_JOSEPH_2301852215.Repositories
{
    public class TransactionRepository
    {
        public List<TransactionModel> ViewTransactions()
        {
            DBConnection.Init();
            SqlDataReader reader;

            List<TransactionModel> transactionList = new List<TransactionModel>();

            string query = "View_All_Transactions";

            DBConnection.command.Connection = DBConnection.connection;
            DBConnection.command.CommandType = CommandType.StoredProcedure;
            DBConnection.command.CommandText = query;

            DBConnection.connection.Open();

            reader = DBConnection.command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TransactionModel transaction = new TransactionModel();
                    transaction.TransactionID = Convert.ToInt32(reader["TransactionID"].ToString());
                    transaction.PaymentMethod = reader["PaymentMethod"].ToString();

                    transaction.detailList = this.ViewTransactionDetails(transaction.TransactionID);

                    transactionList.Add(transaction);
                }
            }

            reader.Close();
            DBConnection.command.Dispose();
            DBConnection.connection.Close();

            return transactionList;
        }

        private List<ProductModel> ViewTransactionDetails(int TransactionID)
        {
            SqlConnection connection = new SqlConnection(@"Data Source=.;Initial Catalog=MarketDB;Integrated Security=True");
            SqlCommand command = new SqlCommand();
            SqlDataReader reader;

            List<ProductModel> detailList = new List<ProductModel>();

            string query = "View_All_Transaction_Details";

            command.Parameters.Add("@TransactionID", SqlDbType.Int).Value = TransactionID;

            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = query;

            connection.Open();

            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ProductModel transactionDetail = new ProductModel();
                    int ProductID = Convert.ToInt32(reader["ProductID"].ToString());
                    string ProductName = reader["ProductName"].ToString();
                    int ProductPrice = Convert.ToInt32(reader["ProductPrice"].ToString());
                    int ProductQty = Convert.ToInt32(reader["ProductQty"].ToString());
                    transactionDetail = new ProductModel(ProductID, ProductName, ProductPrice, ProductQty);

                    detailList.Add(transactionDetail);
                }
            }

            reader.Close();
            command.Dispose();
            connection.Close();

            return detailList;
        }

        public void InsertTransaction(TransactionModel transaction)
        {
            DBConnection.Init();

            string query = "Insert_Transaction";

            DBConnection.command.Parameters.Add("@PaymentMethod", SqlDbType.VarChar, 7).Value = transaction.PaymentMethod;

            DBConnection.command.Connection = DBConnection.connection;
            DBConnection.command.CommandType = CommandType.StoredProcedure;
            DBConnection.command.CommandText = query;

            DBConnection.connection.Open();

            transaction.TransactionID = (int)DBConnection.command.ExecuteScalar();

            DBConnection.command.Dispose();
            DBConnection.connection.Close();

            this.InsertTransactionDetails(transaction);
        }

        private void InsertTransactionDetails(TransactionModel transaction)
        {
            List<ProductModel> detailList = transaction.detailList;

            string query = "Insert_Transaction_Detail";

            foreach (ProductModel d in detailList)
            {
                DBConnection.Init();

                DBConnection.command.Parameters.Add("@TransactionID", SqlDbType.Int).Value = transaction.TransactionID;
                DBConnection.command.Parameters.Add("@ProductID", SqlDbType.Int).Value = d.ProductID;
                DBConnection.command.Parameters.Add("@ProductQty", SqlDbType.Int).Value = d.ProductQty;

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
}
