using AngularToAPI.Models;
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

        public async Task<IEnumerable<SubCategory>> GetSubCategoriesAsync()
        {
            return await _db.SubCategories.ToListAsync();
        }
    }
}
