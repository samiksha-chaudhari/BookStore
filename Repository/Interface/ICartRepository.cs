using Microsoft.Extensions.Configuration;
using Model;

namespace Repository.BookRepository
{
    public interface ICartRepository
    {
        IConfiguration Configuration { get; }

        bool AddToCart(CartModel cartModel);
    }
}