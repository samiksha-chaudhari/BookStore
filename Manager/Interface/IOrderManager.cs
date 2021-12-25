using Model;
using System.Collections.Generic;

namespace Manager.Interface
{
    public interface IOrderManager
    {
        bool AddOrder(List<CartModel> orderdetails);
    }
}