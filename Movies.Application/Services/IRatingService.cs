using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services
{
    public interface IRatingService
    {
        Task<bool> RateNovieAsync(Guid id, int rating, Guid userId, CancellationToken token);
        Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken token);
        Task<IEnumerable<MovieRating>> GetRatingForUserAsync(Guid userId, CancellationToken token = default);
    }
}
