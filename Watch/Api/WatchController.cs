using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Watch.Business;
using Watch.DataAccess.Identity;
using Watch.DataAccess.Repositories;
using Watch.Models;
using Watch.Models.Enum;

namespace Watch.Api
{
    public class WatchController : ApiController
    {
        private readonly WatchBusiness watchBusiness;
        private readonly UserRepository userRepository;
        private readonly UserManager userManager;

        public WatchController(WatchBusiness watchBusiness, UserRepository userRepository, UserManager userManager)
        {
            this.watchBusiness = watchBusiness;
            this.userRepository = userRepository;
            this.userManager = userManager;
        }

        #region [WATCH-CRUD]

        [Authorize]
        [HttpPost]
        public async Task<IResponse> InsertWatch(Models.Watch watch)
        {
            try
            {
                User user = await userManager.FindByNameAsync(User.Identity.Name);
                watch.OwnerUser_Id = user.Id;
                watchBusiness.InsertWatch(watch, watch.MainImage, watch.SubImages);
                return new Response<Models.Watch>();
            }
            catch (Exception e)
            {
                return new Response<Models.Watch>() { Success = false, Message = e.Message, Result = null };
            }
        }

        [Authorize]
        [HttpGet]
        public IResponse DeleteWatch(int id)
        {
            try
            {
                watchBusiness.DeleteWatch(id);
                return new Response<Models.Watch>();
            }
            catch (Exception e)
            {
                return new Response<Models.Watch>() { Success = false, Message = e.Message, Result = null };
            }
        }

        [Authorize]
        [HttpPost]
        public IResponse UpdateWatch(Models.Watch watch)
        {
            try
            {
                watchBusiness.UpdateWatch(watch);
                return new Response<Models.Watch>();
            }
            catch (Exception e)
            {
                return new Response<Models.Watch>() { Success = false, Message = e.Message, Result = null };
            }
        }

        [HttpGet]
        [Authorize]
        public IResponse GetUserWatches(int? pageNumber = null, int? pageSize = null, string searchExp = null)
        {
            Response<Models.Watch> response = new Response<Models.Watch>();

            response.Result.Data = watchBusiness.GetUserWatches(pageNumber, pageSize, searchExp, User.Identity.Name, out response.Result.Count);

            return response;
        }

        [HttpGet]
        public IResponse GetAllWatches(int? skip = null, int? take = null, string searchExp = null)
        {
            Response<Models.Watch> response = new Response<Models.Watch>();
            response.Result.Data = watchBusiness.GetAllWatches(searchExp, skip, take, out response.Result.Count);
            return response;
        }

        [HttpGet]
        public IResponse GetHeaderWatches()
        {
            Response<Models.Watch> response = new Response<Models.Watch>();
            response.Result.Data = watchBusiness.GetAllHeaderWatches();
            return response;
        }
        #endregion

        #region [Watch , Brand]

        [HttpGet]
        public IResponse GetBestProducts(int[] brands, int? pageNumber = null, int? pageSize = null)
        {
            PagedResult<Models.Watch> result = new PagedResult<Models.Watch>();

            string userName = User.Identity.IsAuthenticated ? User.Identity.Name : null;

            result.Data = watchBusiness.GetBestProducts(pageNumber, pageSize, brands, userName, out result.Count);
            return new Response<Models.Watch>
            {
                Result = result
            };
        }

        [HttpGet]
        public IResponse GetTopBrands(int? pageNumber = null, int? pageSize = null)
        {
            PagedResult<Brand> result = new PagedResult<Brand>();
            result.Data = watchBusiness.GetTopBrands(pageNumber, pageSize, out result.Count);
            return new Response<Brand>
            {
                Result = result
            };
        }

        [HttpGet]
        public IResponse GetAllBrands()
        {
            PagedResult<Brand> result = new PagedResult<Brand>();
            result.Data = watchBusiness.GetAllBrands(out result.Count);
            return new Response<Brand>
            {
                Result = result
            };
        }

        [HttpGet]
        public IResponse GetBrandBotique(int? pageNumber, int? pageSize, int brandId)
        {
            PagedResult<Models.Watch> result = new PagedResult<Models.Watch>();
            result.Data = watchBusiness.GetBrandButique(pageNumber, pageSize, brandId, out result.Count);
            return new Response<Models.Watch>
            {
                Result = result
            };

        }

        [HttpGet]
        public IResponse GetStoreWatches(int storeId, int? pageNumber = null, int? pageSize = null)
        {
            try
            {
                PagedResult<Models.Watch> result = new PagedResult<Models.Watch>();

                string userName = User.Identity.IsAuthenticated ? User.Identity.Name : null;

                result.Data = watchBusiness.GetStoreWatches(storeId, pageNumber, pageSize, userName, out result.Count);
                return new Response<Models.Watch>
                {
                    Result = result
                };
            }
            catch (Exception e)
            {
                return new Response<Models.Watch>
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }

        [HttpGet]
        public IResponse GetStoreBestWatches(int storeId, int? pageNumber = null, int? pageSize = null)
        {
            try
            {
                PagedResult<Models.Watch> result = new PagedResult<Models.Watch>();

                string userName = User.Identity.IsAuthenticated ? User.Identity.Name : null;

                result.Data = watchBusiness.GetStoreBestWatches(storeId, pageNumber, pageSize, userName, out result.Count);
                return new Response<Models.Watch>
                {
                    Result = result
                };
            }
            catch (Exception e)
            {
                return new Response<Models.Watch>
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }

        [HttpGet]
        public IResponse GetLatestWatches(int[] brands, int? pageNumber = null, int? pageSize = null)
        {
            PagedResult<Models.Watch> result = new PagedResult<Models.Watch>();

            string userName = User.Identity.IsAuthenticated ? User.Identity.Name : null;

            result.Data = watchBusiness.GetLatestProducts(pageNumber, pageSize, brands, userName, out result.Count);
            return new Response<Models.Watch>
            {
                Result = result
            };
        }

        [HttpGet]
        public IResponse GetTopSellWatches(int[] brands, int? pageNumber = null, int? pageSize = null)
        {
            PagedResult<Models.Watch> result = new PagedResult<Models.Watch>();

            string userName = User.Identity.IsAuthenticated ? User.Identity.Name : null;

            result.Data = watchBusiness.GetTopSellWatches(pageNumber, pageSize, brands, userName, out result.Count);
            return new Response<Models.Watch>
            {
                Result = result
            };
        }

        [HttpGet]
        public IResponse GetLatestMenWatches(int? pageNumber = null, int? pageSize = null)
        {
            PagedResult<Models.Watch> result = new PagedResult<Models.Watch>();

            string userName = User.Identity.IsAuthenticated ? User.Identity.Name : null;

            result.Data = watchBusiness.GetLatestMenWatches(pageNumber, pageSize, userName, out result.Count);

            return new Response<Models.Watch>
            {
                Result = result
            };
        }

        [HttpGet]
        public IResponse GetLatestWomenWatches(int? pageNumber = null, int? pageSize = null)
        {
            PagedResult<Models.Watch> result = new PagedResult<Models.Watch>();

            string userName = User.Identity.IsAuthenticated ? User.Identity.Name : null;

            result.Data = watchBusiness.GetLatestWomenWatches(pageNumber, pageSize, userName, out result.Count);

            return new Response<Models.Watch>
            {
                Result = result
            };
        }

        [HttpGet]
        public IResponse GetWatchDetail(int watchId)
        {
            try
            {
                string username = String.Empty;
                if (User.Identity.IsAuthenticated)
                    username = User.Identity.Name;

                Models.Watch watch = watchBusiness.GetWatchDetail(watchId, username);
                return new Response<Models.Watch>
                {
                    Result = new PagedResult<Models.Watch>
                    {
                        Count = 1,
                        Data = new List<Models.Watch>
                        {
                            watch,
                        }
                    }
                };
            }
            catch (Exception e)
            {
                return new Response<Models.Watch>
                {
                    Success = false,
                    Message = e.Message,
                };
            }
        }

        [HttpGet]
        public IResponse GetWatchSummary(int watchId)
        {
            try
            {
                string username = String.Empty;
                if (User.Identity.IsAuthenticated)
                    username = User.Identity.Name;

                Models.Watch watch = watchBusiness.GetWatchDetail(watchId, username);
                return new Response<WatchSummaryViewModel>
                {
                    Result = new PagedResult<WatchSummaryViewModel>
                    {
                        Count = 1,
                        Data = new List<WatchSummaryViewModel>
                        {
                            (WatchSummaryViewModel)watch,
                        }
                    }
                };
            }
            catch (Exception e)
            {
                return new Response<WatchSummaryViewModel>
                {
                    Success = false,
                    Message = e.Message,
                };
            }
        }

        [HttpGet]
        public IResponse GetSellerWatches(int sellerId, int? pageNumber = null, int? pageSize = null)
        {
            try
            {
                PagedResult<Models.Watch> result = new PagedResult<Models.Watch>();

                string userName = User.Identity.IsAuthenticated ? User.Identity.Name : null;

                result.Data = watchBusiness.GetSellerWatches(sellerId, pageNumber, pageNumber, userName, out result.Count);

                return new Response<Models.Watch>
                {
                    Result = result
                };
            }
            catch (Exception e)
            {
                return new Response<Models.Watch>
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }

        [HttpGet]
        public IResponse GetRecommendedWatches(int brandId)
        {
            try
            {
                PagedResult<Models.Watch> result = new PagedResult<Models.Watch>();

                string userName = User.Identity.IsAuthenticated ? User.Identity.Name : null;

                result.Data = watchBusiness.GetRecommendedWatches(brandId, userName);

                return new Response<Models.Watch>
                {
                    Result = result
                };
            }
            catch (Exception e)
            {
                return new Response<Models.Watch>
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }

        [Authorize(Roles = "Admin,Seller,User")]
        [HttpGet]
        public async Task<IResponse> SuggestPrice(int watchId, decimal suggestedPrice)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                watchBusiness.SuggestPrice(watchId, user.Id, suggestedPrice);
                return new Response<Models.Watch>();
            }
            catch (Exception e)
            {
                return new Response<Models.Watch>
                {
                    Success = false,
                    Message = e.Message,
                };
            }
        }

        [HttpGet]
        public IResponse WatchSearch(string searchExp, int? pageNumber = null, int? pageSize = null, int? brandId = null, Movement? movement = null, decimal? minPrice = null, decimal? maxPrice = null, Condition? condition = null, Models.Gender? gender = null)
        {
            PagedResult<Models.Watch> result = new PagedResult<Models.Watch>();

            result.Data = watchBusiness.SearchWatch(searchExp, pageNumber, pageSize, brandId, movement, minPrice, maxPrice, condition, gender, out result.Count);

            return new Response<Models.Watch>
            {
                Result = result
            };
        }

        [HttpGet]
        public IResponse BrandSearch(string searchExp, int? pageNumber = null, int? pageSize = null)
        {
            PagedResult<Brand> result = new PagedResult<Brand>();

            result.Data = watchBusiness.SearchBrand(searchExp, pageNumber, pageNumber, out result.Count);

            return new Response<Brand>
            {
                Result = result
            };
        }

        [HttpGet]

        public IResponse GetSellerByUserId(int userId)
        {
            try
            {
                Seller seller = watchBusiness.GetSellerByUserId(userId);
                return new Response<Seller>
                {
                    Result = new PagedResult<Seller>
                    {
                        Count = 1,
                        Data = new List<Seller>
                        {
                            seller,
                        }
                    }
                };
            }
            catch (Exception e)
            {
                return new Response<Seller>
                {
                    Success = false,
                    Message = e.Message,
                };
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public IResponse GetPurchaseHistory(int? pageNumber = null, int? pageSize = null)
        {
            PagedResult<Models.Watch> result = new PagedResult<Models.Watch>();
            
            result.Data = watchBusiness.GetPurchaseHistory(pageNumber, pageSize, User.Identity.Name, out result.Count);

            return new Response<Models.Watch>
            {
                Result = result
            };
        }

        [HttpGet]
        [Authorize(Roles = "User,Seller")]
        public IResponse GetSoldItemHistory(int? pageNumber = null, int? pageSize = null)
        {
            PagedResult<Models.Watch> result = new PagedResult<Models.Watch>();
            result.Data = watchBusiness.GetSoldItemHistory(pageNumber, pageSize, User.Identity.Name, out result.Count);
            return new Response<Models.Watch>
            {
                Result = result
            };
        }

        #endregion

        #region [Address]
        [HttpPost]
        [Authorize(Roles = "Admin,Seller,User")]
        public async Task<IResponse> AddAddress(Address address, string name = null, string family = null, string mainPhoneNumber = null, Models.Gender? gender = null)
        {
            try
            {
                User user = await userManager.FindByNameAsync(User.Identity.Name);
                watchBusiness.AddAddress(user.Id, address.City, address.FullAddress, address.PhoneNumber, name, family, mainPhoneNumber, gender);
                return new Response<Address>();
            }
            catch (Exception e)
            {
                return new Response<Address>
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }

        [Authorize(Roles = "Admin,Seller,User")]
        [HttpGet]
        public async Task<IResponse> GetAddressList()
        {
            try
            {
                User user = await userManager.FindByNameAsync(User.Identity.Name);

                PagedResult<Address> result = new PagedResult<Address>
                {
                    Data = watchBusiness.GetAddressList(user.Id)
                };

                return new Response<Address>
                {
                    Result = result
                };
            }
            catch (Exception e)
            {
                return new Response<Address>
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }
        #endregion
    }
}
