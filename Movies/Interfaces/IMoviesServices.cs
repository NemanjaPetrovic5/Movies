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

    }
}
