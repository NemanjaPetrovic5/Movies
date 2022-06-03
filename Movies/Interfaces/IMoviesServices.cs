using Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Interfaces
{
    public interface IMoviesServices
    {
        Movie Insert(Movie movie);
        List<Movie> Read();
        (List<Movie>, int) GetMovies(string search, int page, int pageSize);

        Movie Find(string id);

        void UpdateMovie(Movie movie);

        void DeleteMovie(string id);
    }
}
