using System.Collections.Generic;
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
        private readonly SellerRepository sellerRepository;
        private readonly SuggestPriceRepository suggestPriceRepository;
        private readonly UserRepository userRepository;

        public WatchBusiness(BrandRepository brandRepository, WatchRepository watchRepository, WatchBookmarkRepository bookmarkRepository, SellerRepository sellerRepository, SuggestPriceRepository suggestPriceRepository, UserRepository userRepository, UnitOfWork unitOfWork)
        {
            this.watchRepository = watchRepository;
            this.bookmarkRepository = bookmarkRepository;
            this.brandRepository = brandRepository;
            this.unitOfWork = unitOfWork;

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

        #region [Watch , Brand]
        public List<Models.Watch> GetBestProducts(int? pageNumber, int? pageSize, int[] brands, out int count)
        {
            IQueryable<Models.Watch> result = watchRepository.Get();

            if (brands != null)
                result = result.Where(w => w.Brand_Id.HasValue ? brands.ToList().Contains(w.Brand_Id.Value) : false);

            result = result.OrderByDescending(w => w.WatchBookmarks.Count);

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.Take(10); //To API 4 , 10 tae aval o khaste

            return result.ToList();

        }

        public List<Brand> GetTopBrands(int? pageNumber, int? pageSize, out int count)
        {
            IQueryable<Brand> result = brandRepository.Get().OrderByDescending(b => b.Watches.Sum(w => w.WatchBookmarks.Count));

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.OrderBy(w => Guid.NewGuid()).Take(10); //Transpose Records

            return result.ToList();
        }

        public List<Brand> GetAllBrands(out int count)
        {
            IQueryable<Brand> result = brandRepository.Get();

            count = result.Count();

            return result.ToList();
        }

        public List<Models.Watch> GetStoreWatches(int storeId, int? pageNumber, int? pageSize, out int count)
        {
            Seller store = sellerRepository.GetById(storeId);

            if (store == null)
                throw new NotFoundException("فروشگاه");

            int userId = store.User_Id;

            IQueryable<Models.Watch> result = watchRepository.Get().Where(w => w.User_Id == userId).OrderByDescending(w => w.DateCreated);

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.OrderBy(w => Guid.NewGuid()).Take(10); //Transpose records

            return result.ToList();

        }

        public List<Models.Watch> GetBrandButique(int? pageNumber, int? pageSize, int brandId, out int count)
        {
            Brand brand = brandRepository.GetById(brandId);

            if (brand == null)
                throw new NotFoundException("برند");

            IQueryable<Models.Watch> result = watchRepository.Get().Where(w => w.Brand_Id == brandId);

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.OrderBy(w => Guid.NewGuid()).Take(10); //Transpose records

            return result.ToList();
        }

        public List<Models.Watch> GetLatestProducts(int? pageNumber, int? pageSize, int[] brands, out int count)
        {
            IQueryable<Models.Watch> result = watchRepository.Get();

            if (brands != null)
                result = result.Where(w => w.Brand_Id.HasValue ? brands.Contains(w.Brand_Id.Value) : false);

            result = result.OrderByDescending(w => w.DateCreated);

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.Take(10);

            return result.ToList();
        }

        public List<Models.Watch> GetTopSellWatches(int? pageNumber, int? pageSize, int[] brands, out int count)
        {
            IQueryable<Models.Watch> result = watchRepository.Get();

            if (brands != null)
                result = result.Where(w => w.Brand_Id.HasValue ? brands.Contains(w.Brand_Id.Value) : false);

            result = result.OrderByDescending(w => w.Requests.Count);

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

        public Models.Watch GetWatchDetail(int watchId, string username)
        {
            Models.Watch result = watchRepository.Get().Where(w => w.Id == watchId)
                .Include(w => w.WatchBookmarks)
                .Include(w => w.Brand)
                .Include(w => w.Images)
                .Include(w => w.OwnerUser)
                .FirstOrDefault();

            if (result == null)
                throw new NotFoundException("ساعت");

            if (!string.IsNullOrEmpty(username))
            {
                int userId = userRepository.Get().First(u => u.UserName == username).Id;
                result.IsBookmarked = result.WatchBookmarks.Any(w => w.User_Id == userId);
            }

            result.SimilarWatches = new List<Models.Watch>();
            result.SimilarWatches.AddRange(watchRepository.Get().OrderBy(w => Math.Abs(w.Price - result.Price)).Take(3).ToList());
            result.SimilarWatches.AddRange(watchRepository.Get().Where(w => w.Brand_Id == result.Brand_Id).Take(3).ToList());

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

        public void SuggestPrice(int watchId, int userId, decimal suggestedPrice, string description = null)
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
                Suggested_Price = suggestedPrice,
                Description = description
            };

            suggestPriceRepository.Insert(suggestPrice);

            watch.SuggestedPrices.Add(suggestPrice);

            watchRepository.Update(watch);

            unitOfWork.Commit();
        }

        public Seller GetSellerByUserId(int userId)
        {
            Seller seller = sellerRepository.Get().FirstOrDefault(s => s.User_Id == userId);

            if (seller == null)
                throw new NotFoundException("فروشنده");

            return seller;
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
                result = result.OrderBy(w => Guid.NewGuid()).Take(10);

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
            Seller store = sellerRepository.Get().Where(s => s.Id == storeId).Include(s => s.StoreBookmarks).FirstOrDefault();

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

        #endregion []

        #region [Address]
        public void AddAddress(int userId, string city, string fullAddress, string phoneNumber, string name, string family, string mainPhoneNumner, Models.Gender? gender)
        {
            User user = userRepository.GetById(userId);

            if (user == null)
                throw new NotFoundException("کاربر");

            if (!string.IsNullOrEmpty(name))
                user.Name = name;

            if (!string.IsNullOrEmpty(family))
                user.Family = family;

            if (!string.IsNullOrEmpty(phoneNumber))
                user.PhoneNumber = phoneNumber;

            if (gender != null)
                user.Gender = gender;

            user.Addresses.Add(
                new Address
                {
                    City = city,
                    FullAddress = fullAddress,
                    PhoneNumber = phoneNumber,
                });

            userRepository.Update(user);

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
        #endregion
    }
}
