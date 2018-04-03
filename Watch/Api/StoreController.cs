﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Watch.Business;
using Watch.DataAccess.Repositories;
using Watch.Models;

namespace Watch.Api
{
    public class StoreController : ApiController
    {
        private readonly StoreBusiness storeBusiness;

        public StoreController(StoreBusiness storeBusiness)
        {
            this.storeBusiness = storeBusiness;
        }

        [HttpGet]
        public IResponse GetAllStores(int? pageNumber = null, int? pageSize = null)
        {
            PagedResult<Seller> result = new PagedResult<Seller>();

            string userName = string.Empty;

            if (User.Identity.IsAuthenticated)
                userName = User.Identity.Name;

            result.Data = storeBusiness.GetAllStores(pageNumber, pageSize , userName, out result.Count);

            return new Response<Seller>
            {
                Result = result
            };
        }
    }
}