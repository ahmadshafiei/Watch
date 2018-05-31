using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Watch.Models
{
    [JsonObject]
    public class User : IUser<int>
    {
        public User()
        {
            SecurityStamp = Guid.NewGuid().ToString();
            Addresses = new List<Address>();
            UserRoles = new List<UserRole>();
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalCode { get; set; }
        public Gender? Gender { get; set; }
        [JsonIgnore]
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
