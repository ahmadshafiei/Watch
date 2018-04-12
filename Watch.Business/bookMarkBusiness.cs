using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.Business.Exceptions;
using Watch.DataAccess.Repositories;
using Watch.DataAccess.UnitOfWork;
using Watch.Models;

namespace Watch.Business
{
    public class BookMarkBusiness
    {
        private readonly WatchBookmarkRepository watchBookmarkRepository;
        private readonly WatchRepository watchRepository;
        private readonly SellerRepository sellerRepository;
        private readonly StoreBookmarkRepository storeBookmarkRepository;
        private readonly UnitOfWork unitOfWork;

        public BookMarkBusiness(WatchBookmarkRepository watchBookmarkRepository, StoreBookmarkRepository storeBookmarkRepository, WatchRepository watchRepository,SellerRepository sellerRepository, UnitOfWork unitOfWork)
        {
            this.watchRepository = watchRepository;
            this.sellerRepository = sellerRepository;
            this.watchBookmarkRepository = watchBookmarkRepository;
            this.storeBookmarkRepository = storeBookmarkRepository;
            this.unitOfWork = unitOfWork;
        }
        public void BookmarkWatch(int userId, int watchId)
        {
            if (!watchRepository.Get().Any(w => w.Id == watchId))
                throw new NotFoundException("ساعت");
            watchBookmarkRepository.Insert(new WatchBookmark { User_Id = userId, Watch_Id = watchId });
            unitOfWork.Commit();
        }

        public List<int> GetAllWatchBookmarks(int userId)
        {
            return watchBookmarkRepository.Get().Where(wb => wb.User_Id == userId).Select(wb => wb.Watch_Id).ToList();
        }

        public void BookmarkStore(int userId, int storeId)
        {
            if (!sellerRepository.Get().Any(s => s.Id == storeId))
                throw new NotFoundException("فروشگاه");
            storeBookmarkRepository.Insert(new StoreBookmark { User_Id = userId, Seller_Id = storeId });
            unitOfWork.Commit();
        }

        public List<int> GetAllStoreBookmarks(int userId)
        {
            return storeBookmarkRepository.Get().Where(sb => sb.User_Id == userId).Select(sb => sb.Seller_Id).ToList();
        }
    }
}
