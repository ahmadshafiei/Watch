using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.Business.Exceptions;
using Watch.DataAccess.Identity;
using Watch.DataAccess.Repositories;
using Watch.DataAccess.UnitOfWork;
using Watch.Models;

namespace Watch.Business
{
    public class StoreBusiness
    {
        private readonly SellerRepository sellerRepository;
        private readonly UserRepository userRepository;
        private readonly ImageRepository imageRepository;
        private readonly UnitOfWork unitOfWork;

        public StoreBusiness(SellerRepository sellerRepository, UserRepository userRepository, UnitOfWork unitOfWork, ImageRepository imageRepository)
        {
            this.sellerRepository = sellerRepository;
            this.userRepository = userRepository;
            this.imageRepository = imageRepository;
            this.unitOfWork = unitOfWork;
        }

        public void RegisterSeller(Seller seller)
        {
            sellerRepository.Insert(seller);
            unitOfWork.Commit();
        }

        public List<Seller> GetAllBookmarkedStores(int? pageNumber, int? pageSize, string userName, out int count)
        {
            IQueryable<Seller> result = sellerRepository.Get().OrderByDescending(s => s.Id).Include(s => s.StoreBookmarks);

            count = result.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
                result = result.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            else
                result = result.OrderBy(s => Guid.NewGuid()).Take(10);

            if (!string.IsNullOrEmpty(userName))
            {
                int userId = userRepository.Get().Where(u => u.UserName == userName).Single().Id;

                foreach (Seller seller in result)
                    if (seller.StoreBookmarks.Any(sb => sb.User_Id == userId))
                        seller.IsBookmarked = true;
            }

            return result.ToList();
        }

        public List<User> GetAllUsers(int? pageNumber, string searchExp, out int count)
        {
            searchExp = searchExp ?? "";

            IQueryable<User> result = userRepository.Get().Where(u => u.UserName.Contains(searchExp) && u.UserRoles.Any(ur => ur.Role.Name == "User") && u.UserRoles.Count == 1).OrderBy(u => u.UserName);

            count = result.Count();

            if (pageNumber.HasValue)
                result = result.Skip((pageNumber.Value - 1) * 10).Take(10);

            return result.ToList().Select(u => new User { Id = u.Id, UserName = u.UserName }).ToList();
        }

        public List<Seller> GetAllStores(int? pageNumber, int? pageSize, string searchExp, out int count)
        {
            searchExp = searchExp ?? "";

            return sellerRepository.GetAll(out count, s => s.StoreName.Contains(searchExp) || s.PhoneNumber.Contains(searchExp) || s.Tell.Contains(searchExp), (pageNumber - 1) * pageSize, pageSize, s => s.Id, s => s.User).Include(s => s.Images).ToList();
        }

        public void RemoveStore(int storeId)
        {
            sellerRepository.DeleteById(storeId);
            unitOfWork.Commit();
        }

        public List<Image> GetAllStoreImages(string username)
        {
            User user = userRepository.Get().Where(u => u.UserName == username).SingleOrDefault();

            if (user == null)
                throw new NotFoundException("کاربر");

            Seller seller = sellerRepository.Get().Where(s => s.User_Id == user.Id).Include(s => s.Images).SingleOrDefault();

            if (seller == null)
                throw new NotFoundException("فروشنده");

            return seller.Images;
        }

        public bool AddImages(List<byte[]> images, string username, out int count)
        {
            User user = userRepository.Get().Where(u => u.UserName == username).SingleOrDefault();

            if (user == null)
                throw new NotFoundException("کاربر");

            Seller seller = sellerRepository.Get().Where(s => s.User_Id == user.Id).Include(s => s.Images).SingleOrDefault();

            if (seller == null)
                throw new NotFoundException("فروشنده");

            count = 3 - seller.Images.Count;

            if (images.Count + seller.Images.Count > 3)
                return false;

            seller.Images = new List<Image>();

            foreach (byte[] image in images)
                seller.Images.Add(new Image
                {
                    SellerId = seller.Id,
                    Path = Utility.Image.Save(image)

                });

            unitOfWork.Commit();

            return true;
        }

        public bool RemoveImage(int imageId, string username)
        {
            User user = userRepository.Get().Where(u => u.UserName == username).SingleOrDefault();

            if (user == null)
                throw new NotFoundException("کاربر");

            Seller seller = sellerRepository.Get().Where(s => s.User_Id == user.Id).Include(s => s.Images).SingleOrDefault();

            if (seller == null)
                throw new NotFoundException("فروشنده");

            if (seller.Images.Any(i => i.Id == imageId))
                imageRepository.DeleteById(imageId);
            else
                return false;

            return true;

        }

        public User UpdateStoreProfile(User user)
        {
            userRepository.Update(user);
            unitOfWork.Commit();
            return user;
        }

        public User GetUserProfile(string username)
        {
            User user = userRepository.Get().Where(u => u.UserName == username).SingleOrDefault();

            if (user == null)
                throw new NotFoundException("کاربر");

            Seller seller = sellerRepository.Get().Where(s => s.User_Id == user.Id).Include(s => s.Images).SingleOrDefault();

            if (seller == null)
                throw new NotFoundException("فروشنده");

            user.Store = seller;

            return user;
        }
    }
}
