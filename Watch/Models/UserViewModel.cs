﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Watch.Models
{
    public class UserViewModel
    {
        public string UserName;
        public string Password;
        public string Name;
        public string Family;
        public string PhoneNumber;

        public static explicit operator User(UserViewModel model)
        {
            return model == null ? null : new User
            {
                UserName = model.UserName,
                Password = model.Password,
                Name = model.Name,
                Family = model.Family,
                PhoneNumber = model.PhoneNumber,
            };
        }
    }
}