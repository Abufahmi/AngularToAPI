using AngularToAPI.Models;
using AngularToAPI.ModelViews;
using AngularToAPI.Repository.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularToAPI.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeRepository homeRepository;

        public HomeController(IHomeRepository HomeRepository)
        {
            homeRepository = HomeRepository;
        }

        [Route("GetSubCategories")]
        [HttpGet]
        public async Task<IEnumerable<SubCategory>> GetSubCategories()
        {
            return await homeRepository.GetSubCategoriesAsync();
        }

        [Route("GetMovies/{search}")]
        [HttpGet]
        public async Task<IEnumerable<Movie>> GetMovies(string search)
        {
            return await homeRepository.GetMoviesAsync(search);
        }

        [Route("GetMovie/{id}")]
        [HttpGet]
        public async Task<ActionResult<MovieModel>> GetMovie(long id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            return await homeRepository.GetMovieAsync(id);
        }

        [Route("GetMoviesByActor/{id}")]
        [HttpGet]
        public async Task<IEnumerable<MovieActor>> GetMoviesByActor(int id)
        {
            if (id < 1)
                return null;
            return await homeRepository.GetMoviesByActorAsync(id);
        }
    }
}
