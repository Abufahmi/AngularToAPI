using AngularToAPI.Models;
using AngularToAPI.ModelViews.users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularToAPI.Rpository.Admin
{
    public class AdminRepo : IAdminRepository
    {
        private readonly ApplicationDb _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminRepo(ApplicationDb db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> AddUser(AddUserModel model)
        {
            if (model == null)
            {
                return null;
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = model.EmailConfirm,
                Country = model.Country,
                PhoneNumber = model.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return user;
            }
            return null;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            return await _db.Users.ToListAsync();
        }
    }
}
