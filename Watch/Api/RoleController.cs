using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Watch.Business;
using Watch.Models;

namespace Watch.Api
{
    public class RoleController : ApiController
    {
        private readonly RoleBusiness roleBusiness;

        public RoleController(RoleBusiness roleBusiness)
        {
            this.roleBusiness = roleBusiness;
        }

        [HttpGet]
        public IResponse GetAllRoles()
        {
            Response<Role> response = new Response<Role>();

            response.Result = new PagedResult<Role>();

            response.Result.Data = roleBusiness.GetAllRoles(out response.Result.Count);

            return response;
        }
    }
}
