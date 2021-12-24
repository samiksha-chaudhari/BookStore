using Model;
using System.Collections.Generic;

namespace Manager.Interface
{
    public interface ICartManager
    {
        bool AddToCart(CartModel cartModel);
        bool UpdateCart(int cartId, int Quantity);
        bool DeleteCart(int cartId);
        List<CartModel> GetCart(int userId);
    }
}