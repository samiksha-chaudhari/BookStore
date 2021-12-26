﻿using Manager.Interface;
using Microsoft.AspNetCore.Mvc;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controller
{
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistManager manager;

        public WishlistController(IWishlistManager manager)
        {
            this.manager = manager;

        }
        [HttpPost]
        [Route("api/AddToWishList")]
        public IActionResult AddToWishList([FromBody] WishlistModel wishListmodel)
        {
            try
            {
                var result = this.manager.AddToWishList(wishListmodel);
                if (result)
                {
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Added To wish list Successfully !" });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Failed to add to wish list, Try again" });
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/getwishlist")]
        public IActionResult GetWishList(int userId)
        {
            var result = this.manager.GetWishList(userId);
            try
            {
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = "Wish List successfully retrived", Data = result });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "No WishList available" });
                }
            }
            catch (Exception e)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = e.Message });
            }
        }
    }
}
