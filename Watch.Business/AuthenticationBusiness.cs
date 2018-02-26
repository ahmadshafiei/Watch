using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.DataAccess.Identity;
using Watch.DataAccess.Repositories;

namespace Watch.Business
{
    public class AuthenticationBusiness
    {
        private readonly UserManager userManager;
        private readonly UserRepository userRepository;

        public AuthenticationBusiness(UserManager userManager , UserRepository userRepository)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
        }

    }
}
