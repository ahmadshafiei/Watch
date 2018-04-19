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

        //public User GetUserProfileInfo(int userId)
        //{
        //    User user = userRepository.Get()
        //        .Include(u => u.BookmarkedStores)
        //        .Include(u => u.BookmarkedWatches)
        //        .Include(u => u.Requests)
        //        .Include(u => u.SuggestedPrices)
        //        .FirstOrDefault(u => u.Id == userId);

        //    if (user == null)
        //        throw new NotFoundException("کاربر");

        //    return user;
        //}

        public dynamic GetProfileInfo(int userId, bool isSeller)
        {
            User user = userRepository.Get()
                .Include(u => u.BookmarkedStores)
                .Include(u => u.BookmarkedWatches)
                .Include(u => u.Requests)
                .Include(u => u.SuggestedPrices)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("کاربر");

            if (isSeller)
            {
                Seller seller = sellerRepository.Get()
                .FirstOrDefault(s => s.User_Id == user.Id);

                if (seller == null)
                    throw new NotFoundException("فروشنده");

                seller.User = user;
                seller.User.Password = null;
                seller.User.UserRoles = null;
                seller.User.BookmarkedWatches.ForEach(bw => bw.User = null);
                seller.User.BookmarkedStores.ForEach(bs => bs.User = null);
                seller.User.Requests.ForEach(r => r.User = null);
                seller.User.SuggestedPrices.ForEach(s => s.User = null);


                return seller;
            }

            user.UserRoles = null;
            user.Password = null;
            user.BookmarkedWatches.ForEach(bw => bw.User = null);
            user.BookmarkedStores.ForEach(bs => bs.User = null);
            user.Requests.ForEach(r => r.User = null);
            user.SuggestedPrices.ForEach(s => s.User = null);

            return user;
        }

        public List<SuggestPrice> GetSuggestedPrices(int? pageNumber, int? pageSize, int userId, bool isSeller, out int count)
        {
            IQueryable<SuggestPrice> result = suggestPriceRepository.Get();

            if (isSeller)
                result = result.Include(sp => sp.Watch).Where(sp => sp.Watch.OwnerUser_Id == userId).OrderByDescending(sp => sp.DateCreated);
            else
                result = result.Where(sp => sp.User_Id == userId).OrderByDescending(sp => sp.DateCreated);

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.OrderBy(sp => Guid.NewGuid()).Take(10);

            foreach (SuggestPrice sp in result)
            {
                sp.User.Password = null;
                sp.User.SecurityStamp = null;
            }

            return result.ToList();
        }
    }
}
