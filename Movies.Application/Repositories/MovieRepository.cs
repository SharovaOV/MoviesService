using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Application.Models;

namespace Movies.Application.Repositories
{

    public class MovieRepository : IMovieRepository
    {
        List<Movie> _movies = new();
        public Task<bool> CreateAsync(Movie movie)
        {
            _movies.Add(movie);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            int removeCount = _movies.RemoveAll(x => x.Id == id);
            return Task.FromResult(removeCount > 0);
        }

        public Task<IEnumerable<Movie>> GetAllAsync()
        {
            return Task.FromResult(_movies.AsEnumerable());
        }

        public Task<Movie?> GetByIdAsync(Guid id)
        {
            Movie? movie = _movies.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(movie);
        }

        public Task<Movie?> GetBySlugAsync(string slug)
        {
            Movie? movie = _movies.FirstOrDefault(x => x.Slug == slug);
            return Task.FromResult(movie);
        }

        public Task<bool> UpdateAsync(Movie movie)
        {
            int movieIndex = _movies.FindIndex(x => x.Id == movie.Id);
            if (movieIndex == -1)
                return Task.FromResult(false);

            _movies[movieIndex] = movie;
            return Task.FromResult(true);
        }
    }
}
