using Microsoft.Extensions.Configuration;
using Model;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IOrderRepository
    {
        bool AddOrder(List<CartModel> orderdetails);
        List<OrderModel> GetOrderList(int userId);
    }
}