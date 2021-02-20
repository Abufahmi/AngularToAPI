using AngularToAPI.Models;
using AngularToAPI.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularToAPI.Repository.Home
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDb _db;

        public HomeRepository(ApplicationDb db)
        {
            _db = db;
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategoriesAsync()
        {
            return await _db.SubCategories.ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync(string search)
        {
            if (search == null || string.IsNullOrEmpty(search) || string.IsNullOrWhiteSpace(search) || search == "null")
            {
                return await _db.Movies.OrderByDescending(x => x.Id).Include(x => x.SubCategory)
                    .ThenInclude(x => x.Category).ToListAsync();
            }

            search = search?.ToLower();
            return await _db.Movies.OrderByDescending(x => x.Id).Include(x => x.SubCategory)
              .ThenInclude(x => x.Category)
              .Where(x => x.MovieName.ToLower().Contains(search) || x.SubCategory.SubCategoryName.ToLower().Contains(search))
              .ToListAsync();
        }

        public async Task<ActionResult<MovieModel>> GetMovieAsync(long id)
        {
            var mov = await _db.Movies.Include(x => x.SubCategory).FirstOrDefaultAsync(x => x.Id == id);
            if (mov == null)
                return null;

            var actors = await _db.MovieActors.Include(x => x.Actor).Where(x => x.MovieId == mov.Id).ToListAsync();
            var links = await _db.MovieLinks.Where(x => x.MovieId == mov.Id).ToListAsync();
            var model = new MovieModel
            {
                Movie = mov,
                Actors = actors,
                Links = links
            };
            return model;
        }

        public async Task<IEnumerable<MovieActor>> GetMoviesByActorAsync(int id)
        {
            return await _db.MovieActors.OrderByDescending(x => x.Id).Include(x => x.Movie)
              .Include(x => x.Actor)
              .Where(x => x.ActorId == id)
              .ToListAsync();
        }
    }
}
