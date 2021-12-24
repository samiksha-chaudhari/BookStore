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
    public class BookRepository : IBookRepository
    {
        public BookRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        SqlConnection sqlConnection;

        public bool AddBook(BookModel bookmodel)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("BookStoreDB"));
            using (sqlConnection)
                try
                {

                    SqlCommand sqlCommand = new SqlCommand("spAddBook", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@BookName", bookmodel.BookName);
                    sqlCommand.Parameters.AddWithValue("@AuthorName", bookmodel.AuthorName);
                    sqlCommand.Parameters.AddWithValue("@BookDescription", bookmodel.BookDescription);
                    sqlCommand.Parameters.AddWithValue("@BookImage", bookmodel.BookImage);
                    sqlCommand.Parameters.AddWithValue("@Quantity", bookmodel.Quantity);
                    sqlCommand.Parameters.AddWithValue("@Price", bookmodel.Price);
                    sqlCommand.Parameters.AddWithValue("@DiscountPrice", bookmodel.DiscountPrice);
                    sqlCommand.Parameters.AddWithValue("@Rating", bookmodel.Rating);
                    sqlCommand.Parameters.AddWithValue("@RatingCount", bookmodel.RatingCount);
                    int result = sqlCommand.ExecuteNonQuery();
                    if (result > 0)
                        return true;
                    else
                        return false;
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

        public BookModel GetBook(int bookId)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("BookStoreDB"));
            using (sqlConnection)
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("spGetSpecificBook", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@BookId", bookId);
                    BookModel booksModel = new BookModel();
                    SqlDataReader read = sqlCommand.ExecuteReader();
                    if (read.Read())
                    {
                        booksModel.BookName = read["BookName"].ToString();
                        booksModel.AuthorName = read["AuthorName"].ToString();                       
                        booksModel.BookDescription = read["BookDescription"].ToString();
                        booksModel.BookImage = read["BookImage"].ToString();
                        booksModel.Quantity = read["Quantity"].ToString();
                        booksModel.Price = Convert.ToInt32(read["Price"]);
                        booksModel.DiscountPrice = Convert.ToInt32(read["DiscountPrice"]);
                        booksModel.Rating = Convert.ToInt32(read["Rating"]);
                    }
                    return booksModel;
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

        public bool UpdateBook(BookModel bookmodel)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("BookStoreDB"));

            using (sqlConnection)

                try
                {
                    SqlCommand sqlCommand = new SqlCommand("spUpdateBook", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@BookId", bookmodel.BookId);
                    sqlCommand.Parameters.AddWithValue("@BookName", bookmodel.BookName);
                    sqlCommand.Parameters.AddWithValue("@AuthorName", bookmodel.AuthorName);
                    sqlCommand.Parameters.AddWithValue("@BookDescription", bookmodel.BookDescription);
                    sqlCommand.Parameters.AddWithValue("@BookImage", bookmodel.BookImage);
                    sqlCommand.Parameters.AddWithValue("@Quantity", bookmodel.Quantity);
                    sqlCommand.Parameters.AddWithValue("@Price", bookmodel.Price);
                    sqlCommand.Parameters.AddWithValue("@DiscountPrice", bookmodel.DiscountPrice);

                    sqlCommand.Parameters.Add("@book", SqlDbType.Int);
                    sqlCommand.Parameters["@book"].Direction = ParameterDirection.Output;
                    sqlCommand.ExecuteNonQuery();
                    var result = sqlCommand.Parameters["@book"].Value;
                    if (result.Equals(bookmodel.BookId))
                        return true;
                    else
                        return false;

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


        public bool DeleteBook(int bookId)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("BookStoreDB"));
            using (sqlConnection)
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("spDeleteBook", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@BookId", bookId);
                    sqlCommand.Parameters.Add("@book", SqlDbType.Int);
                    sqlCommand.Parameters["@book"].Direction = ParameterDirection.Output;
                    sqlCommand.ExecuteNonQuery();

                    var result = sqlCommand.Parameters["@book"].Value;
                    if (result.Equals(1))
                        return true;
                    else
                        return false;

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
