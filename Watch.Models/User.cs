using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Watch.Models
{
    public class User : IUser<int>
    {
        public User()
        {
            SecurityStamp = Guid.NewGuid().ToString();
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalCode { get; set; }
        public Gender? Gender { get; set; }
        public string SecurityStamp { get; set; }
        public List<Address> Addresses { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public List<Request> Requests { get; set; }
        public List<WatchBookmark> BookmarkedWatches { get; set; }
        public List<StoreBookmark> BookmarkedStores { get; set; }
        public List<Watch> Watches { get; set; }
        public List<SuggestPrice> SuggestedPrices { get; set; }
    }

    public enum Gender
    {
        Male = 0,
        Female = 1,
        Unknown = 2,
    }
}
