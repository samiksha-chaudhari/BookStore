using Microsoft.Extensions.Configuration;
using Model;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface ICartRepository
    {
        bool AddToCart(CartModel cartModel);
        bool UpdateCart(int cartId, int Quantity);
        bool DeleteCart(int cartId);
        List<CartModel> GetCart(int userId);
    }
}