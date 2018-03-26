using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.Business.Exceptions;
using Watch.DataAccess.Repositories;
using Watch.Models;

namespace Watch.Business
{
    public class RequestBusiness
    {
        private readonly UserRepository userRepository;

        public void OrderWatch(int userId , int addressId , int watchId , int count, bool buyerProtection)
        {
            User user = userRepository.GetById(userId);

            if (user == null)
                throw new NotFoundException("کاربر");

            user.Requests.Add(new Request
            {
                Address_Id = addressId,
                Watch_Id = watchId,
                Count = count,
                IsBuyerProtected = buyerProtection,
            });
        }
    }
}
