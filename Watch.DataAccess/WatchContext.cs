using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Watch.Models;

namespace Watch.DataAccess
{
    public class WatchContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Models.Watch> Watches { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<WatchBookmark> WatchBookmarks { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<SuggestPrice> SuggestedPrices { get; set; }
        public DbSet<StoreBookmark> StoreBookmarks { get; set; }

    }
}
