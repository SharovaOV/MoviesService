using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Constants;
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
        IMovieService _movieService;
        public MoviesController(IMovieService movieService) 
        {
            _movieService = movieService;
        }

        [Authorize(AuthConstants.AdminUserPolicyName)]
        [HttpPost(ApiEndpoints.Movies.Create)]
        public async Task<IActionResult> Create([FromBody] CreateMovieRequest request, CancellationToken token)
        {
            Movie movie = request.MapToMove();
            await _movieService.CreateAsync(movie, token);
            return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie);
        }


        [Authorize(AuthConstants.TrustedMemberPolicyName)]
        [HttpGet(ApiEndpoints.Movies.Get)]
        public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            Movie? movie = Guid.TryParse(idOrSlug, out Guid id) ?
                await _movieService.GetByIdAsync(id, userId, token) :
                await _movieService.GetBySlugAsync(idOrSlug, userId, token);

            if (movie is null)
                return NotFound();

            return Ok(movie.MapToResponse());
        }

        [Authorize(AuthConstants.TrustedMemberPolicyName)]
        [HttpGet(ApiEndpoints.Movies.GetAll)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var movies = await _movieService.GetAllAsync(userId, token);
            return Ok(movies.MapToResponse());
        }

        [Authorize(AuthConstants.TrustedMemberPolicyName)]
        [HttpPut(ApiEndpoints.Movies.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request , CancellationToken token = default) 
        {
            var userId = HttpContext.GetUserId();
            Movie? movie = request.MapToMovie(id);
            Movie? updateMovie = await _movieService.UpdateAsync(movie, userId, token);
            if(updateMovie is null)
            {
                return NotFound();
            }
            return Ok(movie.MapToResponse());
        }

        [Authorize(AuthConstants.AdminUserPolicyName)]
        [HttpDelete(ApiEndpoints.Movies.Delete)]
        public async Task<IActionResult> Delete([FromRoute]Guid id, CancellationToken token)
        {
            bool delete = await _movieService.DeleteByIdAsync(id, token);
            if (!delete)
                return NotFound();
            return Ok();
        }

    }
}
