using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularToAPI.Models;
using AngularToAPI.ModelViews;
using AngularToAPI.ModelViews.users;
using AngularToAPI.Repository.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularToAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _repo;

        public AdminController(IAdminRepository repo)
        {
            _repo = repo;
        }

        [Route("GetAllUsers")]
        [HttpGet]
        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            var users = await _repo.GetUsers();
            if (users == null)
            {
                return null;
            }
            return users;
        }

        [Route("AddUser")]
        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _repo.AddUserAsync(model);
                if (user != null)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        [Route("GetUser/{id}")]
        [HttpGet]
        public async Task<ActionResult<ApplicationUser>> GetUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _repo.GetUserAsync(id);
            if (user != null)
            {
                return user;
            }
            return BadRequest();
        }

        [Route("EditUser")]
        [HttpPut]
        public async Task<ActionResult<ApplicationUser>> EditUser(EditUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await _repo.EditUserAsync(model);
            if (user != null)
            {
                return user;
            }
            return BadRequest();
        }

        [Route("DeleteUsers")]
        [HttpPost]
        public async Task<IActionResult> DeleteUsers(List<string> ids)
        {
            if (ids.Count < 1)
            {
                return BadRequest();
            }

            var result = await _repo.DeleteUserAsync(ids);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("GetUserRole")]
        [HttpGet]
        public async Task<IEnumerable<UserRolesModel>> GetUserRole()
        {
            var userRoles = await _repo.GetUserRoleAsync();
            if (userRoles == null)
            {
                return null;
            }
            return userRoles;
        }

        [Route("GetAllRoles")]
        [HttpGet]
        public async Task<IEnumerable<ApplicationRole>> GetAllRoles()
        {
            var roles = await _repo.GetRolesAsync();
            if (roles == null)
            {
                return null;
            }
            return roles;
        }

        [Route("EditUserRole")]
        [HttpPut]
        public async Task<IActionResult> EditUserRole(EditUserRoleModel model)
        {
            if (ModelState.IsValid)
            {
                var x = await _repo.EditUserRoleAsync(model);
                if (x)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        [Route("GetCategories")]
        [HttpGet]
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _repo.GetCategoriesAsync();
        }

        [Route("AddCategory")]
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var cat = await _repo.AddCategoryAsync(model);
            if (cat != null)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("EditCategory")]
        [HttpPut]
        public async Task<IActionResult> EditCategory(Category model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var cat = await _repo.EditCategoryAsync(model);
            if (cat != null)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("DeleteCategory")]
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(List<string> ids)
        {
            if (ids.Count < 1)
            {
                return BadRequest();
            }

            var result = await _repo.DeleteCategoriesAsync(ids);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("GetSubCategories")]
        [HttpGet]
        public async Task<IEnumerable<SubCategory>> GetSubCategories()
        {
            return await _repo.GetSubCategoriesAsync();
        }

        [Route("AddSubCategory")]
        [HttpPost]
        public async Task<IActionResult> AddSubCategory(SubCategory model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var cat = await _repo.AddSubCategoryAsync(model);
            if (cat != null)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("EditSubCategory")]
        [HttpPut]
        public async Task<IActionResult> EditSubCategory(SubCategory model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var cat = await _repo.EditSubCategoryAsync(model);
            if (cat != null)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("DeleteSubCategory")]
        [HttpPost]
        public async Task<IActionResult> DeleteSubCategory(List<string> ids)
        {
            if (ids.Count < 1)
            {
                return BadRequest();
            }

            var result = await _repo.DeleteSubCategoriesAsync(ids);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("GetAllActors")]
        [HttpGet]
        public async Task<IEnumerable<Actor>> GetAllActors()
        {
            return await _repo.GetActorsAsync();
        }

        [Route("AddActor")]
        [HttpPost]
        public async Task<IActionResult> AddActor()
        {
            var actorName = HttpContext.Request.Form["actorName"];
            var img = HttpContext.Request.Form.Files["image"];
            if (!string.IsNullOrEmpty(actorName) && img != null && img.Length > 0)
            {
                var actor = await _repo.AddActorAsync(actorName, img);
                if (actor != null)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        [Route("GetActor/{id}")]
        [HttpGet]
        public async Task<ActionResult<Actor>> GetActor(int id)
        {
            if (id < 1)
            {
                return NotFound();
            }
            var actor = await _repo.GetActorAsync(id);
            if (actor != null)
            {
                return actor;
            }
            return BadRequest();
        }

        [Route("EditActor")]
        [HttpPut]
        public async Task<ActionResult<Actor>> EditActor()
        {
            try
            {
                var actorName = HttpContext.Request.Form["actorName"].ToString();
                int id = int.Parse(HttpContext.Request.Form["id"].ToString());
                var img = HttpContext.Request.Form.Files["image"];
                if (!string.IsNullOrEmpty(actorName) && img != null && img.Length > 0)
                {
                    var actor = await _repo.EditActorAsync(id, actorName, img);
                    if (actor != null)
                    {
                        return actor;
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest();
        }

        [Route("DeleteAllActors")]
        [HttpPost]
        public async Task<IActionResult> DeleteAllActors(List<string> ids)
        {
            if (ids.Count < 1)
            {
                return BadRequest();
            }

            var result = await _repo.DeleteActorsAsync(ids);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("GetMovies")]
        [HttpGet]
        public async Task<IEnumerable<Movie>> GetMovies()
        {
            var movies = await _repo.GetMoviesAsync();
            if (movies != null)
            {
                return movies;
            }
            return null;
        }

        [Route("AddMovie")]
        [HttpPost]
        public async Task<IActionResult> AddMovie()
        {
            var img = HttpContext.Request.Form.Files["image"];
            var video = HttpContext.Request.Form.Files["video"];
            var story = HttpContext.Request.Form["story"].ToString();
            var movieName = HttpContext.Request.Form["movieName"].ToString();
            var trailer = HttpContext.Request.Form["trailer"].ToString();
            var catId = HttpContext.Request.Form["catId"].ToString();
            var actorsId = HttpContext.Request.Form["actorsId[]"].ToArray();
            var links = HttpContext.Request.Form["links[]"].ToArray();
            List<int> ids = new List<int>();
            for (int i = 0; i < actorsId.Length; i++)
            {
                var result = int.TryParse(actorsId[i], out int id);
                if (result)
                    ids.Add(id);
            }

            if (ids.Count < 1)
            {
                return NoContent();
            }

            if (img != null && video != null && !string.IsNullOrEmpty(story) && !string.IsNullOrEmpty(movieName) && !
                string.IsNullOrEmpty(trailer) && !string.IsNullOrEmpty(catId) && ids.Count > 0)
            {
                var result = await _repo.AddMovieAsync(img, video, story, movieName, trailer, catId, ids, links);
                if (result)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        [Route("GetMovie/{id}")]
        [HttpGet]
        public async Task<ActionResult<Movie>> GetMovie(long id)
        {
            if (id > 0)
            {
                var movie = await _repo.GetMovieAsync(id);
                if (movie != null)
                {
                    return movie;
                }
            }
            return BadRequest();
        }

        [Route("EditMovie")]
        [HttpPut]
        public async Task<IActionResult> EditMovie()
        {
            var img = HttpContext.Request.Form.Files["image"];
            var bodyId = HttpContext.Request.Form["id"].ToString();
            var story = HttpContext.Request.Form["story"].ToString();
            var movieName = HttpContext.Request.Form["movieName"].ToString();
            var trailer = HttpContext.Request.Form["trailer"].ToString();
            var subCatId = HttpContext.Request.Form["subCatId"].ToString();

            var isId = long.TryParse(bodyId, out long id);
            var isSubCatId = int.TryParse(subCatId, out int subId);

            if (!isId || !isSubCatId)
                return BadRequest();

            if (img != null && !string.IsNullOrEmpty(story) && !string.IsNullOrEmpty(movieName)
                && !string.IsNullOrEmpty(trailer))
            {
                var movie = new Movie
                {
                    Id = id,
                    MovieName = movieName,
                    MovieStory = story,
                    MoviePost = img.FileName,
                    SubCategoryId = subId,
                    MovieTrailer = trailer
                };
                var result = await _repo.EditMovieAsync(movie, img);
                if (result)
                {
                    return Ok();
                }
            }

            return BadRequest();
        }

        [Route("SearchMovies/{search}")]
        [HttpGet]
        public async Task<IEnumerable<Movie>> GetMovie(string search)
        {
            if (search == null || string.IsNullOrEmpty(search))
            {
                return null;
            }

            return await _repo.SearchMoviesAsync(search);
        }


        [Route("DeleteAllMovies")]
        [HttpPost]
        public async Task<IActionResult> DeleteAllMovies(List<string> ids)
        {
            if (ids.Count < 1)
            {
                return BadRequest();
            }

            var result = await _repo.DeleteMoviesAsync(ids);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("GetAllMovieLinks/{search}")]
        [HttpGet]
        public async Task<IEnumerable<MovieLink>> GetAllMovieLinks(string search)
        {
            var movieLink = await _repo.GetMovieLinksAsync(search);
            if (movieLink != null)
            {
                return movieLink;
            }
            return null;
        }

        [Route("GetMovieLink/{id}")]
        [HttpGet]
        public async Task<MovieLink> GetMovieLink(long id)
        {
            if (id < 1)
            {
                return null;
            }

            var movieLink = await _repo.GetMovieLinkAsync(id);
            if (movieLink != null)
            {
                return movieLink;
            }
            return null;
        }

        [Route("AddMovieLink")]
        [HttpPost]
        public async Task<IActionResult> AddMovieLink(MovieLink movieLink)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _repo.AddMovieLinkAsync(movieLink);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("EditMovieLink")]
        [HttpPut]
        public async Task<IActionResult> EditMovieLink()
        {
            var video = HttpContext.Request.Form.Files["video"];
            var ilinkId = HttpContext.Request.Form["id"].ToString();
            var quality = HttpContext.Request.Form["quality"].ToString();
            var res = HttpContext.Request.Form["resolation"].ToString();
            var link = HttpContext.Request.Form["movLink"].ToString();
            var movId = HttpContext.Request.Form["movieId"].ToString();

            var isId = long.TryParse(ilinkId, out long id);
            var isMovId = long.TryParse(movId, out long movieId);
            int.TryParse(res, out int resolation);

            if (!isId || !isMovId)
                return BadRequest();

            var movLink = new MovieLink
            {
                Id = id,
                MovLink = link,
                Quality = quality,
                Resolation = resolation,
                MovieId = movieId
            };

            var result = await _repo.EditMovieLinkAsync(movLink, video);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("DeleteAllMovieLinks")]
        [HttpPost]
        public async Task<IActionResult> DeleteAllMovieLinks(List<string> ids)
        {
            if (ids.Count < 1)
            {
                return BadRequest();
            }

            var result = await _repo.DeleteAllMovieLinksAsync(ids);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("GetAllMovieActors/{search}")]
        [HttpGet]
        public async Task<IEnumerable<MovieActor>> GetAllMovieActors(string search)
        {
            return await _repo.GetMovieActorsAsync(search);
        }

        [Route("GetMovieActor/{id}")]
        [HttpGet]
        public async Task<MovieActor> GetMovieActor(long id)
        {
            if (id < 1)
            {
                return null;
            }
            return await _repo.GetMovieActorAsync(id);
        }

        [Route("AddMovieActor")]
        [HttpPost]
        public async Task<IActionResult> AddMovieActor(MovieActor movieActor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _repo.AddMovieActorAsync(movieActor);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("EditMovieActor")]
        [HttpPut]
        public async Task<IActionResult> EditMovieActor(MovieActor movieActor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _repo.EditMovieActorAsync(movieActor);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("DeleteAllMovieActors")]
        [HttpPost]
        public async Task<IActionResult> DeleteAllMovieActors(List<string> ids)
        {
            if (ids.Count < 1)
            {
                return BadRequest();
            }

            var result = await _repo.DeleteAllMovieActorsAsync(ids);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}