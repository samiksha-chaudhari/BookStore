using Microsoft.Extensions.Configuration;
using Model;

namespace Repository.Interface
{
    public interface ICartRepository
    {
        bool AddToCart(CartModel cartModel);
        bool UpdateCart(int cartId, int Quantity);
        bool DeleteCart(int cartId);
    }
}