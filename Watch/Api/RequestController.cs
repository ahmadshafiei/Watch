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
    public class RequestController : ApiController
    {
        private readonly RequestBusiness requestBusiness;
        private readonly UserManager userManager;

        public RequestController(UserManager userManager, RequestBusiness requestBusiness)
        {
            this.userManager = userManager;
            this.requestBusiness = requestBusiness;
        }

        public async Task<IResponse> OrderWatch(int addressId, int watchId, int count = 1, bool buyerProtection = false)
        {
            try
            {
                User user = await userManager.FindByEmailAsync(User.Identity.Name);
                requestBusiness.OrderWatch(user.Id, addressId, watchId, count, buyerProtection);
                return new Response<Request>();
            }
            catch (Exception e)
            {
                return new Response<Request>
                {
                    Success = false,
                    Message = e.Message
                };
            }

        }
    }
}
