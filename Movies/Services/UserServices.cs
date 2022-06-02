using MongoDB.Driver;
using Movies.Interfaces;
using Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Services
{
    public class UsersServices : IUsersServices
    {
        private readonly IMongoCollection<User> users;

        //Connection
        public UsersServices(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            this.users = database.GetCollection<User>("Users");
        }
        // select all users
        public List<User> Read()
        {
            var u = users.Find(u => true);
            return u.ToList();
        }
        public User Insert(User u)
        {
            // add new user
            users.InsertOne(u);
            return u;
        }
        public User Find(string username)
        {
            // find user(username)
            var u = users.Find(k => k.username == username).SingleOrDefault();
            return u;
        }
        public User FindUserById(string id)
        {
            // find user(ID)
            var k = users.Find(k => k.userID == id).SingleOrDefault();
            return k;
        }
        //Delete User
        public void DeleteUser(string id)
        {
            users.DeleteOne(sub => sub.userID == id);
        }
    }
}
