using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.DataAccess.Identity;
using Watch.DataAccess.Repositories;
using Watch.Models;

namespace Watch.Business
{
    public class StoreBusiness
    {
        private readonly SellerRepository sellerRepository;
        private readonly UserRepository userRepository;

        public StoreBusiness(SellerRepository sellerRepository, UserRepository userRepository)
        {
            this.sellerRepository = sellerRepository;
            this.userRepository = userRepository;
        }

        public List<Seller> GetAllStores(int? pageNumber, int? pageSize, string userName, out int count)
        {
            IQueryable<Seller> result = sellerRepository.Get().OrderByDescending(s => s.Id).Include(s => s.StoreBookmarks);

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.OrderBy(s => Guid.NewGuid()).Take(10);

            if (!string.IsNullOrEmpty(userName))
            {
                int userId = userRepository.Get().Where(u => u.UserName == userName).Single().Id;

                foreach (Seller seller in result)
                    if (seller.StoreBookmarks.Any(sb => sb.User_Id == userId))
                        seller.IsBookmarked = true;
            }

            return result.ToList();
        }
    }
}
