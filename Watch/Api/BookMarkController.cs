using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Watch.Business;
using Watch.DataAccess.Repositories;
using Watch.Models;

namespace Watch.Api
{
    public class BookMarkController : ApiController
    {
        private readonly UserRepository userRepository;
        private readonly BookMarkBusiness bookMarkBusiness;

        public BookMarkController(BookMarkBusiness bookMarkBusiness, UserRepository userRepository)
        {
            this.bookMarkBusiness = bookMarkBusiness;
            this.userRepository = userRepository;
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IResponse BookmarkWatch(int watchId, bool bookmark)
        {
            try
            {
                int userId = userRepository.Get().Where(u => u.UserName == User.Identity.Name).Single().Id;

                bookMarkBusiness.BookmarkWatch(userId, watchId, bookmark);

                return new Response<WatchBookmark>
                {
                    Success = true,
                    Message = "Watch Bookmarked"
                };
            }
            catch (Exception e)
            {
                return new Response<WatchBookmark>() { Success = false, Message = e.Message };
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IResponse GetAllWatchBookmarks(int? pageNumber = null, int? pageSize = null)
        {
            try
            {
                int userId = userRepository.Get().Where(u => u.UserName == User.Identity.Name).Single().Id;

                PagedResult<Models.Watch> result = new PagedResult<Models.Watch>();

                result.Data = bookMarkBusiness.GetAllWatchBookmarks(userId, pageNumber, pageSize, out result.Count);

                return new Response<Models.Watch>() { Result = result };
            }
            catch (Exception e)
            {
                return new Response<Models.Watch>() { Success = false, Message = e.Message };
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IResponse BookmarkStore(int storeId, bool bookmark)
        {
            try
            {
                int userId = userRepository.Get().Where(u => u.UserName == User.Identity.Name).Single().Id;

                bookMarkBusiness.BookmarkStore(userId, storeId, bookmark);

                return new Response<StoreBookmark>()
                {
                    Success = true,
                    Message = "Store Bookmarked"
                };
            }
            catch (Exception e)
            {
                return new Response<StoreBookmark>() { Success = false, Message = e.Message };
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IResponse GetAllStoreBookmarks(int? pageNumber = null, int? pageSize = null)
        {
            try
            {
                int userId = userRepository.Get().Where(u => u.UserName == User.Identity.Name).Single().Id;

                PagedResult<Seller> result = new PagedResult<Seller>();

                result.Data = bookMarkBusiness.GetAllStoreBookmarks(userId, pageNumber, pageSize, out result.Count);

                return new Response<Seller>() { Result = result };
            }
            catch (Exception e)
            {
                return new Response<Seller>() { Success = false, Message = e.Message };
            }
        }
    }
}
