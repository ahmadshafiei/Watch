using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public BookMarkBusiness(WatchBookmarkRepository watchBookmarkRepository, StoreBookmarkRepository storeBookmarkRepository, WatchRepository watchRepository, SellerRepository sellerRepository, UnitOfWork unitOfWork)
        {
            this.watchRepository = watchRepository;
            this.sellerRepository = sellerRepository;
            this.watchBookmarkRepository = watchBookmarkRepository;
            this.storeBookmarkRepository = storeBookmarkRepository;
            this.unitOfWork = unitOfWork;
        }
        public void BookmarkWatch(int userId, int watchId, bool bookmark)
        {
            if (!watchRepository.Get().Any(w => w.Id == watchId))
                throw new NotFoundException("ساعت");
            if (watchBookmarkRepository.Get().Any(wb => wb.User_Id == userId && wb.Watch_Id == watchId))
                return;
            if (bookmark)
                watchBookmarkRepository.Insert(new WatchBookmark { User_Id = userId, Watch_Id = watchId });
            else
                watchBookmarkRepository.Delete(new WatchBookmark { User_Id = userId, Watch_Id = watchId });
            unitOfWork.Commit();
        }

        public List<Models.Watch> GetAllWatchBookmarks(int userId, int? pageNumber, int? pageSize, out int count)
        {
            IQueryable<Models.Watch> watches;

            if (pageNumber.HasValue && pageSize.HasValue)
                watches = watchBookmarkRepository.GetAll(out count, wb => wb.User_Id == userId, (pageNumber.Value - 1) * pageSize.Value, pageSize.Value, wb => wb.Watch.Id, wb => wb.Watch).Select(wb => wb.Watch);
            else
            {
                watches = watchBookmarkRepository.Get()
                    .Include(wb => wb.Watch)
                    .Where(wb => wb.User_Id == userId)
                    .OrderByDescending(wb => wb.Id).Select(wb => wb.Watch);

                count = watches.Count();
            }

            List<Models.Watch> result = watches.ToList();

            result.ForEach(w => { w.IsBookmarked = true; });

            return result;
        }

        public void BookmarkStore(int userId, int storeId, bool bookmark)
        {
            if (!sellerRepository.Get().Any(s => s.Id == storeId))
                throw new NotFoundException("فروشگاه");
            if (storeBookmarkRepository.Get().Any(sb => sb.User_Id == userId && sb.Seller_Id == storeId))
                return;
            if (bookmark)
                storeBookmarkRepository.Insert(new StoreBookmark { User_Id = userId, Seller_Id = storeId });
            else
                storeBookmarkRepository.Delete(new StoreBookmark { User_Id = userId, Seller_Id = storeId });
            unitOfWork.Commit();
        }

        public List<Seller> GetAllStoreBookmarks(int userId, int? pageNumber, int? pageSize, out int count)
        {
            IQueryable<Seller> sellers;

            if (pageNumber.HasValue && pageSize.HasValue)
                sellers = storeBookmarkRepository.GetAll(out count, wb => wb.User_Id == userId, (pageNumber.Value - 1) * pageSize.Value, pageSize.Value, wb => wb.Store.Id, wb => wb.Store).Select(wb => wb.Store);
            else
            {
                sellers = storeBookmarkRepository.Get()
                    .Include(wb => wb.Store)
                    .Where(wb => wb.User_Id == userId)
                    .OrderByDescending(wb => wb.Id).Select(wb => wb.Store);

                count = sellers.Count();
            }

            return sellers.ToList();
        }
    }
}
