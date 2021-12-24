using Model;

namespace Manager.BookManager
{
    public interface ICartManager
    {
        bool AddToCart(CartModel cartModel);
    }
}