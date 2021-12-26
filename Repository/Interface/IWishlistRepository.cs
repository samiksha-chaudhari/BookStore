using Microsoft.Extensions.Configuration;
using Model;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IWishlistRepository
    {
        bool AddToWishList(WishlistModel wishListmodel);
        List<WishlistModel> GetWishList(int userId);
    }
}