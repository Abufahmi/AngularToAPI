using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularToAPI.Models;
using AngularToAPI.ModelViews.users;
using AngularToAPI.Rpository.Admin;
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
                var user = await _repo.AddUser(model);
                if(user != null)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }
    }
}