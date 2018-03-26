using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.DataAccess.Repositories;
using Watch.DataAccess.UnitOfWork;
using Watch.Models;

namespace Watch.Business
{
    public class BookMarkBusiness
    {
        private readonly WatchBookmarkRepository watchBookmarkRepository;
        private readonly UnitOfWork unitOfWork;

        public BookMarkBusiness(WatchBookmarkRepository watchBookmarkRepository , UnitOfWork unitOfWork)
        {
            this.watchBookmarkRepository = watchBookmarkRepository;
            this.unitOfWork = unitOfWork;
        }
        public void BookmarkWatch(int userId, int watchId)
        {
            watchBookmarkRepository.Insert(new WatchBookmark() { User_Id = userId, Watch_Id = watchId });
            unitOfWork.Commit();
        }

        public List<int> GetAllBookmarks(int userId)
        {
            return watchBookmarkRepository.Get().Where(b => b.User_Id == userId).Select(b => b.Watch_Id).ToList();
        }
    }
}
