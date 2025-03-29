using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IValidator<Movie> _movieValidator;

        public MovieService(IMovieRepository movieRepository, IValidator<Movie> movieValidator)
        {
            _movieRepository = movieRepository;
            _movieValidator = movieValidator;
        }

        public async Task<bool> CreateAsync(Movie movie)
        {
            await _movieValidator.ValidateAndThrowAsync(movie);
            return await _movieRepository.CreateAsync(movie);
        }

        public Task<bool> DeleteByIdAsync(Guid id)
            => _movieRepository.DeleteByIdAsync(id);

        public Task<IEnumerable<Movie>> GetAllAsync()
            => _movieRepository.GetAllAsync();

        public Task<Movie?> GetByIdAsync(Guid id)
            => _movieRepository.GetByIdAsync(id);

        public Task<Movie?> GetBySlugAsync(string slug)
            => _movieRepository.GetBySlugAsync(slug);

        public async Task<Movie?> UpdateAsync(Movie movie)
        {
            await _movieValidator.ValidateAndThrowAsync(movie);
            var moveExists = await _movieRepository.ExistsByIdAsync(movie.Id);
            if (!moveExists) return null;
            return  await _movieRepository.UpdateAsync(movie); 
        }
    }
}
