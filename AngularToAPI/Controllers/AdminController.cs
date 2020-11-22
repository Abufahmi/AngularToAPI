using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularToAPI.Models;
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
    }
}