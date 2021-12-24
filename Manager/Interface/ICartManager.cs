using Model;

namespace Manager.Interface
{
    public interface ICartManager
    {
        bool AddToCart(CartModel cartModel);
        bool UpdateCart(int cartId, int Quantity);
        bool DeleteCart(int cartId);
    }
}