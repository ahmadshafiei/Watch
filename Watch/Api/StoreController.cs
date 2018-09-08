using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;
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
        public IResponse GetAllBookmarkedStores(int? pageNumber = null, int? pageSize = null)
        {
            PagedResult<Seller> result = new PagedResult<Seller>();

            string userName = string.Empty;

            if (User.Identity.IsAuthenticated)
                userName = User.Identity.Name;

            result.Data = storeBusiness.GetAllBookmarkedStores(pageNumber, pageSize, userName, out result.Count);

            return new Response<Seller>
            {
                Result = result
            };
        }

        #region [RegisterSeller-AdminPanel]

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public string UploadStoreLogo()
        {
            return StoreImageOnDisk();
        }

        [Authorize(Roles = "Admin,Seller")]
        public IResponse RegisterSeller(Seller seller)
        {
            if (seller.User_Id == 0)
                return new Response<Seller>
                {
                    Success = false,
                    Message = "کاربر مورد نظر را انتخاب کنید"
                };
            storeBusiness.RegisterSeller(seller);
            return new Response<Seller>();
        }

        [Authorize(Roles = "Admin")]
        public IResponse GetAllUsers(int? pageNumber, string searchExp)
        {
            PagedResult<User> result = new PagedResult<User>();
            result.Data = storeBusiness.GetAllUsers(pageNumber, searchExp, out result.Count);
            return new Response<User>
            {
                Result = result
            };
        }

        public IResponse GetAllStores(int? pageNumber, int? pageSize, string searchExp)
        {
            PagedResult<Seller> result = new PagedResult<Seller>();
            result.Data = storeBusiness.GetAllStores(pageNumber, pageSize, searchExp, out result.Count);
            return new Response<Seller>
            {
                Result = result
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public void RemoveStore(int storeId)
        {
            storeBusiness.RemoveStore(storeId);
        }

        #endregion

        #region [Store Images CRUD]
        [Authorize(Roles = "Seller")]
        [HttpGet]
        public IResponse GetAllStoreImages()
        {
            PagedResult<Image> result = new PagedResult<Image>();
            result.Data = storeBusiness.GetAllStoreImages(User.Identity.Name);
            return new Response<Image>
            {
                Result = result
            };
        }

        [Authorize(Roles = "Seller")]
        [HttpPost]
        public IResponse AddImages(List<byte[]> images)
        {
            int count;
            if (!storeBusiness.AddImages(images, User.Identity.Name, out count))
                return new Response<Image>()
                {
                    Success = false,
                    Message = "بیشتر از 3 عکس نمی توانید آپلود کنید تعداد عکس باقی مانده " + count
                };
            return new Response<Image>();
        }

        [Authorize(Roles = "Seller")]
        [HttpGet]
        public IResponse RemoveImage(int imageId)
        {
            if (storeBusiness.RemoveImage(imageId, User.Identity.Name))
                return new Response<Image>();
            return new Response<Image>
            {
                Success = false,
                Message = "عکس مورد نظر مربوط به فروشگاه شما نمی باشد"
            };
        }

        #endregion

        #region [Seller-Panel]

        [Authorize(Roles = "Seller")]
        [HttpGet]
        public IResponse GetUserProfile()
        {
            return new Response<User>
            {
                Result = new PagedResult<User>
                {
                    Data = new List<User> { storeBusiness.GetUserProfile(User.Identity.Name) },
                    Count = 1
                }
            };
        }

        [Authorize(Roles = "Seller")]
        [HttpPut]
        public IResponse UpdateStoreProfile(User user)
        {

            return new Response<User>
            {
                Result = new PagedResult<User>
                {
                    Data = new List<User> { storeBusiness.UpdateStoreProfile(user) },
                    Count = 1
                }
            };
        }

        [HttpPost]
        [Authorize]
        public void UploadStoreImage(int storeId)
        {
            if (storeBusiness.CanSaveImageForStore(storeId))
            {
                string imagePath = StoreImageOnDisk();

                storeBusiness.AddImageToStore(storeId, imagePath);
            }
        }

        [HttpDelete]
        [Authorize]
        public void RemoveStoreImage(int imageId)
        {
            storeBusiness.RemoveStoreImage(imageId);
        }

        [HttpGet]
        [Authorize(Roles = "Seller")]
        public IResponse GetStoreWatches(int? pageNumber = null, int? pageSize = null, string searchExp = null)
        {
            PagedResult<Models.Watch> watches = new PagedResult<Models.Watch>();

            watches.Data = storeBusiness.GetStoreWatches(User.Identity.Name, pageNumber, pageSize, searchExp, out watches.Count);

            return new Response<Models.Watch> { Result = watches };
        }

        [HttpDelete]
        [Authorize]
        public void RemoveWatchImage(int imageId)
        {
            storeBusiness.RemoveWatchImage(imageId);
        }
        #endregion

        private string StoreImageOnDisk()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var files = HttpContext.Current.Request.Files;
            byte[] buffer = new byte[files[0].ContentLength];
            files[0].InputStream.Read(buffer, 0, buffer.Length);

            MemoryStream ms = new MemoryStream(buffer);
            var mainImg = System.Drawing.Image.FromStream(ms);

            string relPath = @"/Images/" + Guid.NewGuid().ToString() + ".jpg";
            string path = AppDomain.CurrentDomain.BaseDirectory + relPath;

            mainImg.Save(path, mainImg.RawFormat);

            return relPath;
        }
    }
}
