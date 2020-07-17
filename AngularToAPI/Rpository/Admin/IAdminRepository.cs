using AngularToAPI.Models;
using AngularToAPI.ModelViews.users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularToAPI.Rpository.Admin
{
    public interface IAdminRepository
    {
        Task<IEnumerable<ApplicationUser>> GetUsers();
        Task<ApplicationUser> AddUser(AddUserModel model);
        Task<ApplicationUser> GetUserAsync(string id);
        Task<ApplicationUser> EditUserAsync(EditUserModel model);
        Task<bool> DeleteUserAsync(List<string> ids);
    }
}
