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
                EmailConfirmed = model.EmailConfirmed,
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

        public async Task<ApplicationUser> EditUserAsync(EditUserModel model)
        {
            if (model.Id == null)
            {
                return null;
            }

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (user == null)
            {
                return null;
            }

            if (model.Password != user.PasswordHash)
            {
                var result = await _userManager.RemovePasswordAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.AddPasswordAsync(user, model.Password);
                }
            }

            _db.Users.Attach(user);
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.EmailConfirmed = model.EmailConfirmed;
            user.PhoneNumber = model.PhoneNumber;
            user.Country = model.Country;

            _db.Entry(user).Property(x => x.Email).IsModified = true;
            _db.Entry(user).Property(x => x.UserName).IsModified = true;
            _db.Entry(user).Property(x => x.EmailConfirmed).IsModified = true;
            _db.Entry(user).Property(x => x.PhoneNumber).IsModified = true;
            _db.Entry(user).Property(x => x.Country).IsModified = true;
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<ApplicationUser> GetUserAsync(string id)
        {
            if (id == null)
            {
                return null;
            }

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<bool> DeleteUserAsync(List<string> ids)
        {
            if (ids.Count < 1)
            {
                return false;
            }

            var i = 0;
            foreach (string id in ids)
            {
                var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                {
                    return false;
                }
                _db.Users.Remove(user);
                i++;
            }
            if (i > 0)
            {
                await _db.SaveChangesAsync();
            }
            return true;
        }
    }
}
