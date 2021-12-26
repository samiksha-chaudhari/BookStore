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
    public class OrderRepository : IOrderRepository
    {
        public OrderRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        SqlConnection sqlConnection;

        public bool AddOrder(List<CartModel> orderdetails)
        {
            bool res = false;
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("BookStoreDB"));
            using (sqlConnection)
                try
                {
                    foreach (var order in orderdetails)
                    {
                        SqlCommand sqlCommand = new SqlCommand("spAddOrder", sqlConnection);
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlConnection.Open();
                        sqlCommand.Parameters.AddWithValue("@BookId", order.BookId);
                        //sqlCommand.Parameters.AddWithValue("@CartId", order.CartID);
                        sqlCommand.Parameters.AddWithValue("@UserId", order.UserId);
                        sqlCommand.Parameters.AddWithValue("@Quantity", order.Quantity);
                        sqlCommand.Parameters.AddWithValue("@AddressId", order.AddressId);

                        sqlCommand.Parameters.Add("@order", SqlDbType.Int).Direction = ParameterDirection.Output;
                        sqlCommand.ExecuteNonQuery();
                        var result = sqlCommand.Parameters["@order"].Value;
                        if (result.Equals(2))
                        {
                            res = true;
                            sqlConnection.Close();
                        }
                        else
                        {
                            res = false;
                            break;
                        }
                    }
                    return res;
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

        public List<OrderModel> GetOrderList(int userId)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("BookStoreDB"));
            using (sqlConnection)
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("spGetOrder", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<OrderModel> orderList = new List<OrderModel>();
                        while (reader.Read())
                        {
                            OrderModel orderModel = new OrderModel();
                            orderModel.OrderId = Convert.ToInt32(reader["OrderId"]);
                            orderModel.UserId = Convert.ToInt32(reader["UserId"]);
                            orderModel.AddressId = Convert.ToInt32(reader["AddressId"]);
                            orderModel.BookId = Convert.ToInt32(reader["BookId"]);
                            orderModel.Price = Convert.ToInt32(reader["Price"]);
                            orderModel.Quantity = Convert.ToInt32(reader["Quantity"]);
                            orderList.Add(orderModel);
                        }
                        return orderList;
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
