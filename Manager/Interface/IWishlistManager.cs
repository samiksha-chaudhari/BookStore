using Model;
using System.Collections.Generic;

namespace Manager.Interface
{
    public interface IWishlistManager
    {
        bool AddToWishList(WishlistModel wishListmodel);
        List<WishlistModel> GetWishList(int userId);
        bool DeleteWishlist(int WishlistId);
    }
}