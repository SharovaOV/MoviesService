using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
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

        [HttpPost(ApiEndpoints.Movies.Create)]
        public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
        {
            Movie movie = request.MapToMove();
            await _movieRepository.CreateAsync(movie);
            return Created($"{ApiEndpoints.Movies.Create}/{movie.Id}", movie);
        }

    }
}
