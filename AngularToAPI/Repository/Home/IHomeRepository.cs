using AngularToAPI.Models;
using AngularToAPI.ModelViews;
using Microsoft.AspNetCore.Mvc;
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
        Task<ActionResult<MovieModel>> GetMovieAsync(long id);
        Task<IEnumerable<MovieActor>> GetMoviesByActorAsync(int id);
    }
}
