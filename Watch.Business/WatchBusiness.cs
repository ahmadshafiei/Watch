﻿using System.Collections.Generic;
using System.Linq;
using Watch.DataAccess.Repositories;
using System.Data.Entity;
using Watch.DataAccess.UnitOfWork;
using Watch.Models;
using System;
using Watch.Business.Exceptions;
using Watch.Models.Enum;
using System.IO;

namespace Watch.Business
{
    public class WatchBusiness
    {
        private readonly WatchRepository watchRepository;
        private readonly WatchBookmarkRepository bookmarkRepository;
        private readonly BrandRepository brandRepository;
        private readonly UnitOfWork unitOfWork;
        private readonly StoreRepository storeRepository;
        private readonly SellerRepository sellerRepository;
        private readonly SuggestPriceRepository suggestPriceRepository;
        private readonly UserRepository userRepository;

        public WatchBusiness(BrandRepository brandRepository, WatchRepository watchRepository, WatchBookmarkRepository bookmarkRepository, StoreRepository storeRepository, SellerRepository sellerRepository, SuggestPriceRepository suggestPriceRepository, UserRepository userRepository, UnitOfWork unitOfWork)
        {
            this.watchRepository = watchRepository;
            this.bookmarkRepository = bookmarkRepository;
            this.brandRepository = brandRepository;
            this.unitOfWork = unitOfWork;
            this.storeRepository = storeRepository;
            this.sellerRepository = sellerRepository;
            this.suggestPriceRepository = suggestPriceRepository;
            this.userRepository = userRepository;
        }

        #region [WATHC-CRUD]

        public void InsertWatch(Models.Watch watch, byte[] mainImage, List<byte[]> images)
        {
            watch.MainImagePath = Utility.Image.Save(mainImage);

            foreach (byte[] img in images)
            {
                watch.Images.Add(new Image
                {
                    Path = Utility.Image.Save(mainImage)
                });
            }

            watchRepository.Insert(watch);
            unitOfWork.Commit();
        }

        public void DeleteWatch(int id)
        {
            watchRepository.Delete(watchRepository.GetById(id));
            unitOfWork.Commit();
        }

        public void UpdateWatch(Models.Watch watch)
        {
            watchRepository.Update(watch);
            unitOfWork.Commit();
        }

        public List<Models.Watch> GetAllWatches(string searchExp, int? skip, int? take, out int count)
        {
            return watchRepository.GetAll(out count, null, skip, take, w => w.Id, w => w.Images);
        }

        public List<Models.Watch> GetAllHeaderWatches()
        {
            int count = 0;
            return watchRepository.GetAll(out count, w => w.IsHeader, null, null, w => w.Id, w => w.Images);
        }

        #endregion


        #region [BOOKMARK]

        public void BookmarkWatch(int userId, int watchId)
        {
            bookmarkRepository.Insert(new WatchBookmark() { User_Id = userId, Watch_Id = watchId });
            unitOfWork.Commit();
        }

        public List<int> GetAllBookmarks(int userId)
        {
            return bookmarkRepository.Get().Where(b => b.User_Id == userId).Select(b => b.Watch_Id).ToList();
        }

        #endregion

        #region

        public List<Models.Watch> GetBestProducts(int? pageNumber, int? pageSize, out int count)
        {
            IQueryable<Models.Watch> result = watchRepository.Get().OrderByDescending(w => w.WatchBookmarks.Count);

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.Take(10); //To API 4 10 tae aval o khaste

            return result.ToList();

        }

        public List<Brand> GetTopBrands(int? pageNumber, int? pageSize, out int count)
        {
            IQueryable<Brand> result = brandRepository.Get().OrderByDescending(b => b.Watches.Sum(w => w.WatchBookmarks.Count));

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.Take(10);

            return result.ToList();
        }

        public List<Models.Watch> GetStoreWatches(int storeId, int? pageNumber, int? pageSize, out int count)
        {
            Store store = storeRepository.GetById(storeId);

            if (store == null)
                throw new NotFoundException("فروشگاه");

            int userId = store.User_Id;

            IQueryable<Models.Watch> result = watchRepository.Get().Where(w => w.User_Id == userId).OrderByDescending(w => w.DateCreated);

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
            {
                result = result.OrderBy(w => Guid.NewGuid()); //Transpose records
                result = result.Take(10);
            }

            return result.ToList();

        }

        public List<Models.Watch> GetLatestProducts(int? pageNumber, int? pageSize, out int count)
        {
            IQueryable<Models.Watch> result = watchRepository.Get().OrderByDescending(w => w.DateCreated);

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.Take(10);

            return result.ToList();
        }

        public List<Models.Watch> GetTopSellWatches(int? pageNumber, int? pageSize, out int count)
        {
            IQueryable<Models.Watch> result = watchRepository.Get().OrderByDescending(w => w.Requests.Count);

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.Take(10);

            return result.ToList();
        }

        public List<Models.Watch> GetLatestMenWatches(int? pageNumber, int? pageSize, out int count)
        {
            IQueryable<Models.Watch> result = watchRepository.Get().Where(w => w.Gender == Models.Gender.Male).OrderByDescending(w => w.DateCreated);

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.Take(10);

            return result.ToList();
        }

        public List<Models.Watch> GetLatestWomenWatches(int? pageNumber, int? pageSize, out int count)
        {
            IQueryable<Models.Watch> result = watchRepository.Get().Where(w => w.Gender == Models.Gender.Female).OrderByDescending(w => w.DateCreated);

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.Take(10);

            return result.ToList();
        }

        public Models.Watch GetWatchDetail(int watchId)
        {
            Models.Watch result = watchRepository.Get().Where(w => w.Id == watchId).Include(w => w.WatchBookmarks)
                .Include(w => w.Brand)
                .Include(w => w.Images)
                .Include(w => w.OwnerUser)
                .FirstOrDefault();

            if (result == null)
                throw new NotFoundException("ساعت");

            //Prevent self looping
            result.Brand.Watches = null;
            result.OwnerUser.Watches = null;

            return result;
        }

        public List<Models.Watch> GetSellerWatches(int sellerId, int? pageNumber, int? pageSize, out int count)
        {
            Seller seller = sellerRepository.Get().Where(s => s.Id == sellerId).Include(s => s.User.Watches).FirstOrDefault();

            if (seller == null)
                throw new NotFoundException("فروشنده");

            IQueryable<Models.Watch> result = seller.User.Watches.OrderByDescending(w => w.DateCreated).AsQueryable();

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.Take(10);

            return result.ToList();
        }

        public List<Models.Watch> GetRecommendedWatches(int brandId)
        {
            Brand brand = brandRepository.Get().Where(b => b.Id == brandId).Include(b => b.Watches).FirstOrDefault();

            if (brand == null)
                throw new NotFoundException("برند");

            IQueryable<Models.Watch> result = brand.Watches.OrderBy(w => Guid.NewGuid()).Take(4).AsQueryable();

            List<Models.Watch> listedResult = result.ToList();

            listedResult.ForEach(w =>
            {
                w.Brand.Watches = null;
            });

            return listedResult;
        }


        public void SuggestPrice(int watchId, int userId, decimal suggestedPrice)
        {
            Models.Watch watch = watchRepository.Get().Where(w => w.Id == watchId).Include(w => w.SuggestedPrices).FirstOrDefault();

            if (watch == null)
                throw new NotFoundException("ساعت");
            if (!watch.SuggestPrice)
                throw new NotAllowedException("پیشنهاد قیمت");

            SuggestPrice suggestPrice = new SuggestPrice
            {
                User_Id = userId,
                Watch_Id = watchId,
                Suggested_Price = suggestedPrice
            };

            suggestPriceRepository.Insert(suggestPrice);

            watch.SuggestedPrices.Add(suggestPrice);

            watchRepository.Update(watch);

            unitOfWork.Commit();
        }

        public List<Address> GetAddressList(int userId)
        {
            User user = userRepository.Get().Where(u => u.Id == userId).Include(u => u.Addresses).FirstOrDefault();

            if (user == null)
                throw new NotFoundException("کاربر");

            if (user.Addresses == null)
                throw new NotFoundException("آدرس کاربر");

            return user.Addresses;
        }

        public List<Models.Watch> SearchWatch(string searchExp, int? pageNumber, int? pageSize, int? brandId, Movement? movement, decimal? minPrice, decimal? maxPrice, Condition? condition, out int count)
        {
            searchExp = searchExp ?? "";

            IQueryable<Models.Watch> result = watchRepository.Get().Where(w =>
                w.Name.Contains(searchExp) ||
                w.Price.ToString().Contains(searchExp) ||
                w.CaseMaterial.Contains(searchExp) ||
                w.BraceletMaterial.Contains(searchExp) ||
                w.Quantity.ToString().Equals(searchExp) ||
                w.Brand.Name.Contains(searchExp)
                ).OrderByDescending(w => w.DateCreated);

            if (brandId.HasValue)
                result = result.Where(r => r.Brand_Id == brandId.Value);
            if (movement.HasValue)
                result = result.Where(r => r.CounterMovement == movement);
            if (minPrice.HasValue)
                result = result.Where(r => r.Price >= minPrice);
            if (maxPrice.HasValue)
                result = result.Where(r => r.Price <= maxPrice);
            if (condition.HasValue)
                result = result.Where(r => r.Condition == condition);

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.Take(10);

            return result.ToList();
        }

        public List<Brand> SearchBrand(string searchExp, int? pageNumber, int? pageSize, out int count)
        {
            searchExp = searchExp ?? "";

            IQueryable<Brand> result = brandRepository.Get().Where(b => b.Name.Contains(searchExp)).OrderByDescending(b => b.Id);

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.Take(10);

            return result.ToList();
        }


        public List<Models.Watch> GetStoreWatches(int storeId, SortBy? sortBy, int? pageNumber, int? pageSize, out int count)
        {
            Store store = storeRepository.Get().Where(s => s.Id == storeId).Include(s => s.StoreBookmarks).FirstOrDefault();

            if (store == null)
                throw new NotFoundException("فروشگاه");

            IQueryable<Models.Watch> result = watchRepository.Get().Where(w => w.User_Id == store.User_Id).Include(w => w.Brand);

            if (sortBy == SortBy.Popularity)
                result = result.OrderByDescending(w => w.WatchBookmarks.Count);

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.Take(10);

            return result.ToList();
        }

        #endregion

    }
}
