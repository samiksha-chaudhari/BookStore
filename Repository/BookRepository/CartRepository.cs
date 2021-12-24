using Microsoft.Extensions.Configuration;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Repository.BookRepository
{
    public class CartRepository : ICartRepository
    {
        public CartRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        SqlConnection sqlConnection;

        public bool AddToCart(CartModel cartModel)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("BookStoreDB"));
            using (sqlConnection)
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("spAddCart", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@BookId", cartModel.BookID);
                    sqlCommand.Parameters.AddWithValue("@UserId", cartModel.UserId);
                    sqlCommand.Parameters.AddWithValue("@NoOfBook", cartModel.BookOrderCount);
                    var returnedSQLParameter = sqlCommand.Parameters.Add("@result", SqlDbType.Int);
                    returnedSQLParameter.Direction = ParameterDirection.Output;
                    sqlCommand.ExecuteNonQuery();
                    var result = (int)returnedSQLParameter.Value;
                    if (result.Equals(1))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
        }
    }
}
