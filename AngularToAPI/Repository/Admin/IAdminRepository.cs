using AngularToAPI.Models;
using AngularToAPI.ModelViews;
using AngularToAPI.ModelViews.users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularToAPI.Repository.Admin
{
    public interface IAdminRepository
    {
        Task<IEnumerable<ApplicationUser>> GetUsers();
        Task<ApplicationUser> AddUserAsync(AddUserModel model);
        Task<ApplicationUser> GetUserAsync(string id);
        Task<ApplicationUser> EditUserAsync(EditUserModel model);
        Task<bool> DeleteUserAsync(List<string> ids);
        Task<IEnumerable<UserRolesModel>> GetUserRoleAsync();
        Task<IEnumerable<ApplicationRole>> GetRolesAsync();
        Task<bool> EditUserRoleAsync(EditUserRoleModel model);
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category> AddCategoryAsync(Category model);
        Task<Category> EditCategoryAsync(Category model);
        Task<bool> DeleteCategoriesAsync(List<string> ids);
        Task<IEnumerable<SubCategory>> GetSubCategoriesAsync();
        Task<SubCategory> AddSubCategoryAsync(SubCategory subCategory);
        Task<SubCategory> EditSubCategoryAsync(SubCategory subCategory);
        Task<bool> DeleteSubCategoriesAsync(List<string> ids);
    }
}
