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
        private readonly IRatingRepository _ratingRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IValidator<Movie> _movieValidator;

        public MovieService(IMovieRepository movieRepository, IValidator<Movie> movieValidator, IRatingRepository ratingRepository)
        {
            _movieRepository = movieRepository;
            _movieValidator = movieValidator;
            _ratingRepository = ratingRepository;
        }

        public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
        {
            await _movieValidator.ValidateAndThrowAsync(movie, cancellationToken: token);
            return await _movieRepository.CreateAsync(movie, token);
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
            => _movieRepository.DeleteByIdAsync(id, token);

        public Task<IEnumerable<Movie>> GetAllAsync(Guid? userid = default, CancellationToken token = default)
            => _movieRepository.GetAllAsync(userid, token);

        public Task<Movie?> GetByIdAsync(Guid id, Guid? userid = default, CancellationToken token = default)
            => _movieRepository.GetByIdAsync(id, userid, token);

        public Task<Movie?> GetBySlugAsync(string slug, Guid? userid = default, CancellationToken token = default)
            => _movieRepository.GetBySlugAsync(slug, userid, token);

        public async Task<Movie?> UpdateAsync(Movie movie, Guid? userid = default, CancellationToken token = default)
        {
            await _movieValidator.ValidateAndThrowAsync(movie, cancellationToken: token);
            var moveExists = await _movieRepository.ExistsByIdAsync(movie.Id);
            if (!moveExists) return null;
            await _movieRepository.UpdateAsync(movie, token); 

            if(!userid.HasValue)
            {
                var rating = await _ratingRepository.GetRatingAsync(movie.Id, token);
                movie.Rating = rating;
                return movie;
            }

            var ratings = await _ratingRepository.GetRatingAsync(movie.Id, userid.Value, token);
            movie.Rating = ratings.Rating;
            movie.UserRating = ratings.UserRating;
            return movie;

        }
    }
}
