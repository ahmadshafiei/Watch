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
        public IResponse BookmarkWatch(int watchId)
        {
            try
            {
                int userId = userRepository.Get().Where(u => u.UserName == User.Identity.Name).Single().Id;
                bookMarkBusiness.BookmarkWatch(userId, watchId);
                return new Response<WatchBookmark>();
            }
            catch (Exception e)
            {
                return new Response<WatchBookmark>() { Success = false, Message = e.Message };
            }

        }

        [Authorize(Roles = "User")]
        public IResponse GetAllWatchBookmarks()
        {
            try
            {
                int userId = userRepository.Get().Where(u => u.UserName == User.Identity.Name).Single().Id;
                PagedResult<int> result = new PagedResult<int>();
                result.Data = bookMarkBusiness.GetAllBookmarks(userId);
                return new Response<int>() { Result = result };
            }
            catch (Exception e)
            {
                return new Response<int>() { Success = false, Message = e.Message };
            }

        }
    }
}
