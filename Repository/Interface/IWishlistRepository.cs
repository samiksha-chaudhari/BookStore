using Microsoft.Extensions.Configuration;
using Model;

namespace Repository.Interface
{
    public interface IWishlistRepository
    {
        IConfiguration Configuration { get; }

        bool AddToWishList(WishlistModel wishListmodel);
    }
}