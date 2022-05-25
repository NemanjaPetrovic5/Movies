using MongoDB.Driver;
using Movies.Interfaces;
using Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Services
{
    public class MoviesServices : IMoviesServices
    {
        private readonly IMongoCollection<Movie> movies;

        // Connection
        public MoviesServices(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            this.movies = database.GetCollection<Movie>("Movies");
        }
        //INSERT
        public Movie Insert(Movie movie)
        {
            movies.InsertOne(movie);
            return movie;
        }
            //READ
        public List<Movie> Read()
        {
            // Select all movies
            var m = movies.Find(m => true);
            return m.ToList();
        }
    }
}
