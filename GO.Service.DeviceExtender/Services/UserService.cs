using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GO.Service.DeviceExtender.Services
{
    public interface IUserService
    {
        bool ValidateCredentials(string username, string password);
    }

    public class UserService : IUserService
    {
        public bool ValidateCredentials(string username, string password)
        {
            return username.Equals("admin") && password.Equals("Pa$$WoRd");
        }
    }
}
