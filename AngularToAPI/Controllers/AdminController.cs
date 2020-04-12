using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularToAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularToAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {

        //[HttpGet]
        //[Route("GetAllUsers")]
        //public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllUsers()
        //{
        //    return await _db.Users.ToListAsync();
        //}
    }
}