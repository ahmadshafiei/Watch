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
                                profileBusiness.GetSellerProfileInfo(user.Id)
                            }
                        }
                    };

                return new Response<User>
                {
                    Result = new PagedResult<User>
                    {
                        Count = 1,
                        Data = new List<User>
                        {
                            profileBusiness.GetUserProfileInfo(user.Id)
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
        [Authorize(Roles="User,Seller,Admin")]
        public async Task<IResponse> GetSuggestedPrices(int? pageNumber = null , int? pageSize = null)
        {
            User user = await userManager.FindByEmailAsync(User.Identity.Name);

            try
            {
                PagedResult<SuggestPrice> result = new PagedResult<SuggestPrice>
                {
                    Data = profileBusiness.GetSuggestedPrices(pageNumber, pageSize, user.Id , out result.Count);
                };

                return new Response<SuggestPrice>
                {
                    
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


            return null;
        }
    }
}
