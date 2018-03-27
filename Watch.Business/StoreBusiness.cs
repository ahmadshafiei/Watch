using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.DataAccess.Repositories;
using Watch.Models;

namespace Watch.Business
{
    public class StoreBusiness
    {
        private readonly SellerRepository sellerRepository;

        public StoreBusiness(SellerRepository sellerRepository)
        {
            this.sellerRepository = sellerRepository;
        }

        public List<Seller> GetAllStores(int? pageNumber , int? pageSize , out int count)
        {
            IQueryable<Seller> result = sellerRepository.Get().OrderByDescending(s => s.Id).Include(s => s.StoreBookmarks);

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.OrderBy(s => Guid.NewGuid()).Take(10);
            
            foreach (Seller seller in result)
                if (seller.StoreBookmarks.Any())
                    seller.IsBookmarked = true;
            
            return result.ToList();
        }
    }
}
