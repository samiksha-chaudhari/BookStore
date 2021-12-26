using Manager.Interface;
using Model;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.BookManager
{
    public class WishlistManager : IWishlistManager
    {
        private readonly IWishlistRepository repository;
        public WishlistManager(IWishlistRepository repository)
        {
            this.repository = repository;
        }

        public bool AddToWishList(WishlistModel wishListmodel)
        {
            try
            {
                return this.repository.AddToWishList(wishListmodel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<WishlistModel> GetWishList(int userId)
        {
            try
            {
                return this.repository.GetWishList(userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
