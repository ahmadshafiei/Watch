using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.Business.Exceptions;
using Watch.DataAccess.Identity;
using Watch.DataAccess.Repositories;
using Watch.Models;

namespace Watch.Business
{
    public class ProfileBusiness
    {
        private readonly UserRepository userRepository;
        private readonly SellerRepository sellerRepository;
        private readonly SuggestPriceRepository suggestPriceRepository;
        private readonly UserManager userManager;

        public ProfileBusiness(UserManager userManager, UserRepository userRepository, SellerRepository sellerRepository, SuggestPriceRepository suggestPriceRepository)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
            this.sellerRepository = sellerRepository;
            this.suggestPriceRepository = suggestPriceRepository;
        }

        public User GetUserProfileInfo(int userId)
        {
            User user = userRepository.Get()
                .Include(u => u.BookmarkedStores)
                .Include(u => u.BookmarkedWatches)
                .Include(u => u.Requests)
                .Include(u => u.SuggestedPrices)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("کاربر");

            return user;
        }

        public Seller GetSellerProfileInfo(int userId)
        {
            User user = userRepository.Get()
                .Include(u => u.BookmarkedStores)
                .Include(u => u.BookmarkedWatches)
                .Include(u => u.Requests)
                .Include(u => u.SuggestedPrices)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("کاربر");

            Seller seller = sellerRepository.Get()
                .FirstOrDefault(s => s.User_Id == user.Id);

            if (seller == null)
                throw new NotFoundException("فروشنده");

            seller.User = user;

            return seller;
        }

        public List<SuggestPrice> GetSuggestedPrices(int? pageNumber, int? pageSize, int userId, out int count)
        {
            IQueryable<SuggestPrice> result = suggestPriceRepository.Get();

            result = result.Where(sp => sp.User_Id == userId).OrderByDescending(sp => sp.DateCreated);

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.OrderBy(sp => Guid.NewGuid()).Take(10);

            return result.ToList();
        }
    }
}
