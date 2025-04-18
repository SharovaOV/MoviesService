using System.Threading.Tasks;
using Asp.Versioning;
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
            return CreatedAtAction(nameof(GetV1), new { idOrSlug = movie.Id }, movie);
        }


        [Authorize(AuthConstants.TrustedMemberPolicyName)]
        [ApiVersion(1.0, Deprecated = true)]
        [HttpGet(ApiEndpoints.Movies.Get)]
        public async Task<IActionResult> GetV1(
            [FromRoute] string idOrSlug,
            [FromServices] LinkGenerator linkGenerator,
            CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            Movie? movie = Guid.TryParse(idOrSlug, out Guid id) ?
                await _movieService.GetByIdAsync(id, userId, token) :
                await _movieService.GetBySlugAsync(idOrSlug, userId, token);

            if (movie is null)
                return NotFound();

            var response = movie.MapToResponse();
            var movieObject = new { id = movie.Id };

            response.Links.Add(new Link
            {
                Href = linkGenerator.GetPathByAction(HttpContext, nameof(GetV1), values: new { isOrSlug = movie.UserRating }),
                Rel = "self",
                Type = "GET"
            });

            response.Links.Add(new Link
            {
                Href = linkGenerator.GetPathByAction(HttpContext, nameof(Update)),
                Rel = "self",
                Type = "PUT"
            });

            response.Links.Add(new Link
            {
                Href = linkGenerator.GetPathByAction(HttpContext, nameof(Delete)),
                Rel = "self",
                Type = "DELETE"
            });
            return Ok(response);
        }

        [Authorize(AuthConstants.TrustedMemberPolicyName)]
        [ApiVersion(2.0)]
        [HttpGet(ApiEndpoints.Movies.Get)]
        public async Task<IActionResult> GetV2(
            [FromRoute] string idOrSlug,
            [FromServices] LinkGenerator linkGenerator,
            CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            Movie? movie = Guid.TryParse(idOrSlug, out Guid id) ?
                await _movieService.GetByIdAsync(id, userId, token) :
                await _movieService.GetBySlugAsync(idOrSlug, userId, token);

            if (movie is null)
                return NotFound();

            var response = movie.MapToResponse();
            var movieObject = new { id = movie.Id };

            response.Links.Add(new Link
            {
                Href = linkGenerator.GetPathByAction(HttpContext, nameof(GetV2), values: new { isOrSlug = movie.UserRating }),
                Rel = "self",
                Type = "GET"
            });

            response.Links.Add(new Link
            {
                Href = linkGenerator.GetPathByAction(HttpContext, nameof(Update)),
                Rel = "self",
                Type = "PUT"
            });

            response.Links.Add(new Link
            {
                Href = linkGenerator.GetPathByAction(HttpContext, nameof(Delete)),
                Rel = "self",
                Type = "DELETE"
            });
            return Ok(response);
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

            var response = movie.MapToResponse();
            //var movieObject = new { id = movie.Id };
            return Ok(response);
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

        [HttpGet(ApiEndpoints.Movies.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllMoviesRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var options = request.MapToOptions()
                .WithUser(userId);
            var movies = await _movieService.GetAllAsync(options, token);
            var movieCount = await _movieService.GetCountAsync(options.Title, options.YearOfRelease, token);
            var moviesResponse = movies.MapToResponse(request.Page, request.PageSize, movieCount);
            return Ok(moviesResponse);
        }
    }
}
