using Manager.Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.BookManager
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository repository;
        public UserManager(IUserRepository repository)
        {
            this.repository = repository;
        }
        public string register(RegisterModel userData)
        {
            return "Register Successfull";
        }
    }
}
