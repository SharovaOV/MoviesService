using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers
{
    [ApiController]
    public class MoviesController : ControllerBase
    {
        IMovieService _movieRepository;
        public MoviesController(IMovieService movieRepository) 
        {
            _movieRepository = movieRepository;
        }

        [HttpPost(ApiEndpoints.Movies.Create)]
        public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
        {
            Movie movie = request.MapToMove();
            await _movieRepository.CreateAsync(movie);
            return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie);
        }

        [HttpGet(ApiEndpoints.Movies.Get)]
        public async Task<IActionResult> Get([FromRoute] string idOrSlug)
        {
            Movie? movie = Guid.TryParse(idOrSlug, out Guid id) ?
                await _movieRepository.GetByIdAsync(id) :
                await _movieRepository.GetBySlugAsync(idOrSlug);

            if (movie is null)
                return NotFound();

            return Ok(movie.MapToResponse());
        }

        [HttpGet(ApiEndpoints.Movies.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _movieRepository.GetAllAsync();
            return Ok(movies.MapToResponse());
        }

        [HttpPut(ApiEndpoints.Movies.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request ) 
        {
            Movie? movie = request.MapToMovie(id);
            Movie? updateMovie = await _movieRepository.UpdateAsync(movie);
            if(updateMovie is null)
            {
                return NotFound();
            }

            return Ok(movie.MapToResponse());
        }

        [HttpDelete(ApiEndpoints.Movies.Delete)]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            bool delete = await _movieRepository.DeleteByIdAsync(id);
            if (!delete)
                return NotFound();
            return Ok();
        }

    }
}
