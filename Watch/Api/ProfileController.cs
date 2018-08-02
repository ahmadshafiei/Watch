using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Watch.Business;
using Watch.DataAccess.Identity;
using Watch.Models;

namespace Watch.Api
{
    public class ProfileController : ApiController
    {
        private readonly ProfileBusiness profileBusiness;
        private readonly UserManager userManager;

        public ProfileController(ProfileBusiness profileBusiness, UserManager userManager)
        {
            this.profileBusiness = profileBusiness;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "User,Seller,Admin")]
        public async Task<IResponse> GetProfileInfo()
        {
            try
            {
                User user = await userManager.FindByNameAsync(User.Identity.Name);

                if (await userManager.IsInRoleAsync(user.Id, "Seller"))
                    return new Response<Seller>
                    {
                        Result = new PagedResult<Seller>
                        {
                            Count = 1,
                            Data = new List<Seller>
                            {
                                profileBusiness.GetProfileInfo(user.Id, true)
                            }
                        }
                    };

                else
                    return new Response<User>
                    {
                        Result = new PagedResult<User>
                        {
                            Count = 1,
                            Data = new List<User>
                            {
                                profileBusiness.GetProfileInfo(user.Id, false)
                            }
                        }
                    };
            }
            catch (Exception e)
            {
                return new Response<User>
                {
                    Success = false,
                    Message = e.Message
                };
            }

        }

        [HttpGet]
        [Authorize(Roles = "User,Seller,Admin")]
        public async Task<IResponse> GetSuggestedPrices(int? pageNumber = null, int? pageSize = null)
        {
            User user = await userManager.FindByNameAsync(User.Identity.Name);

            try
            {
                PagedResult<SuggestPrice> result = new PagedResult<SuggestPrice>();

                bool isSeller;

                if (await userManager.IsInRoleAsync(user.Id, "Seller"))
                    isSeller = true;
                else
                    isSeller = false;

                result.Data = profileBusiness.GetSuggestedPrices(pageNumber, pageSize, user.Id, isSeller, out result.Count);

                return new Response<SuggestPrice>
                {
                    Result = result
                };
            }
            catch (Exception e)
            {
                return new Response<SuggestPrice>
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IResponse> EditSellerProfile(Seller seller)
        {
            return new Response<Seller>
            {
                Result = new PagedResult<Seller>
                {
                    Data = new List<Seller> { await profileBusiness.EditSellerProfile(seller, User.Identity.Name) },
                    Count = 1,
                }
            };
        }

        [HttpPost]
        [Authorize]
        public async Task<IResponse> EditUserProfile(User user)
        {
            return new Response<User>
            {
                Result = new PagedResult<User>
                {
                    Data = new List<User> { await profileBusiness.EditUserProfile(user, User.Identity.Name) },
                    Count = 1,
                }
            };
        }
    }
}
