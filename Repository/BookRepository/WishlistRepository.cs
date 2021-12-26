using Microsoft.Extensions.Configuration;
using Model;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Repository.BookRepository
{
    public class WishlistRepository : IWishlistRepository
    {
        public IConfiguration Configuration { get; }
        public WishlistRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        SqlConnection sqlConnection;
        public bool AddToWishList(WishlistModel wishListmodel)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("BookStoreDB"));
            using (sqlConnection)
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("spAddWishlist", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@BookId", wishListmodel.BookId);
                    sqlCommand.Parameters.AddWithValue("@UserId", wishListmodel.UserId);

                    sqlCommand.Parameters.Add("@wishlist", SqlDbType.Int).Direction = ParameterDirection.Output;
                    sqlCommand.ExecuteNonQuery();
                    var result = sqlCommand.Parameters["@wishlist"].Value;
                    if (result.Equals(2))
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
                finally
                {
                    sqlConnection.Close();
                }
        }

        public List<WishlistModel> GetWishList(int userId)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("BookStoreDB"));
            using (sqlConnection)
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("spGetWishlist", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<WishlistModel> wishList = new List<WishlistModel>();
                        while (reader.Read())
                        {
                            BookModel booksModel = new BookModel();
                            WishlistModel wishListModel = new WishlistModel();

                            wishListModel.WishlistId = Convert.ToInt32(reader["WishlistId"]);
                            wishListModel.UserId = Convert.ToInt32(reader["UserId"]);
                            wishListModel.BookId = Convert.ToInt32(reader["BookId"]);
                            booksModel.BookName = reader["BookName"].ToString();
                            booksModel.AuthorName = reader["AuthorName"].ToString();
                            booksModel.BookDescription = reader["BookDescription"].ToString();
                            booksModel.BookImage = reader["BookImage"].ToString();
                            booksModel.Quantity = reader["Quantity"].ToString();
                            booksModel.Price = Convert.ToInt32(reader["Price"]);                            
                            booksModel.DiscountPrice = Convert.ToInt32(reader["DiscountPrice"]);
                            
                            wishList.Add(wishListModel);
                        }
                        return wishList;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
        }

    }
}
