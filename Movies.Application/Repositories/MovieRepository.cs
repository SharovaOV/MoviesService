using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Movies.Application.Database;
using Movies.Application.Models;

namespace Movies.Application.Repositories
{

    public class MovieRepository : IMovieRepository
    {
        private IDbConnectionFactory _dbConnectionFactory;

        public async Task<bool> CreateAsync(Movie movie)
        {
            using IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();
            var result = await connection.ExecuteAsync(new CommandDefinition("""
                insert into movies ( id, slug, title, yearofreliase)
                values (@Id, @Slug, @Title, @YearOfRelease
                """, movie));

            if(result >0)
            {
                foreach(var genre in movie.Genres)
                {
                    await connection.ExecuteAsync(new CommandDefinition("""
                    insert into genres (movieId, name)
                    values (@MovieId, @Name)
                    """, new { MovieId = movie.Id, Name = genre }));
                }
            }
            transaction.Commit();

            return result > 0;
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            return Task.Run(()=>true);
        }

        public Task<bool> ExistsByIdAsync(Guid id)
        {
            return Task.Run(() => true);
        }

        public Task<IEnumerable<Movie>> GetAllAsync()
        {
            return null;
        }

        public Task<Movie?> GetByIdAsync(Guid id)
        {
            return null;
        }

        public Task<Movie?> GetBySlugAsync(string slug)
        {
            return null;
        }

        public Task<bool> UpdateAsync(Movie movie)
        {
            return Task.Run(() => true);

        }
    }
}
