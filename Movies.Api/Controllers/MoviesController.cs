using Microsoft.AspNetCore.Mvc;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MoviesController : ControllerBase
    {
        IMovieRepository _movieRepository;
        public MoviesController(IMovieRepository movieRepository) 
        {
            _movieRepository = movieRepository;
        }

        [HttpPost("movies")]
        public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
        {
            Movie movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                YearOfRelease = request.YearOfRelease,
                Genres = request.Genres.ToList()
            };
            await _movieRepository.CreateAsync(movie);
            return Created($"/api/movies/{movie.Id}", movie);
        }

    }
}
