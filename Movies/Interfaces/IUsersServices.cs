using Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Interfaces
{
    public interface IUsersServices
    {
        List<User> Read();
        User Insert(User u);
        User Find(string username);
        User FindUserById(string id);
        void DeleteUser(string id);

    }
}
