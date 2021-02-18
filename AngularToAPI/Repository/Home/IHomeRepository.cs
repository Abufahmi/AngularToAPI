using AngularToAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularToAPI.Repository.Home
{
    public interface IHomeRepository
    {
        Task<IEnumerable<SubCategory>> GetSubCategoriesAsync();
        Task<IEnumerable<Movie>> GetMoviesAsync(string search);
    }
}
