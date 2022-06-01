using MongoDB.Bson;
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
        // GET MOVIES
        public (List<Movie>, int) GetMovies(string search, int page, int pageSize)
        {
            var builder = Builders<Movie>.Filter;

            var filter = builder.Empty;

            if (search != null)
                filter &= builder.Regex("name", new BsonRegularExpression($"/{search}/i"));

            int skip = pageSize * (page - 1);
            var count = movies.CountDocuments(filter);
            var k = movies.Find(filter).SortBy(k => k.movieID).Skip(skip).Limit(pageSize);


            return (k.ToList(), (int)count);
        }
        //FIND
        public Movie Find(string id) =>
          movies.Find(sub => sub.movieID == id).SingleOrDefault();

        //UPDATE
        public void UpdateMovie(Movie movie) =>
                    movies.ReplaceOne(sub => sub.movieID == movie.movieID, movie);
    }
}

