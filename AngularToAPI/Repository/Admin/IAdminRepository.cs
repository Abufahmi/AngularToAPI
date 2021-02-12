using AngularToAPI.Models;
using AngularToAPI.ModelViews;
using AngularToAPI.ModelViews.users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
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
        Task<IEnumerable<Actor>> GetActorsAsync();
        Task<Actor> AddActorAsync(string actorName, IFormFile img);
        Task<Actor> GetActorAsync(int id);
        Task<Actor> EditActorAsync(int id, string actorName, IFormFile img);
        Task<bool> DeleteActorsAsync(List<string> ids);
        Task<IEnumerable<Movie>> GetMoviesAsync();
        Task<bool> AddMovieAsync(IFormFile img, IFormFile video, string story, string movieName, string trailer, 
            string catId, List<int> ids, string[] links);
        Task<Movie> GetMovieAsync(long id);
        Task<bool> EditMovieAsync(Movie movie, IFormFile img);
        Task<IEnumerable<Movie>> SearchMoviesAsync(string search);
        Task<bool> DeleteMoviesAsync(List<string> ids);
        Task<IEnumerable<MovieLink>> GetMovieLinksAsync(string search);
        Task<MovieLink> GetMovieLinkAsync(long id);
        Task<bool> AddMovieLinkAsync(MovieLink movieLink);
        Task<bool> EditMovieLinkAsync(MovieLink movieLink, IFormFile video);
        Task<bool> DeleteAllMovieLinksAsync(List<string> ids);
        Task<IEnumerable<MovieActor>> GetMovieActorsAsync(string search);
        Task<MovieActor> GetMovieActorAsync(long id);
        Task<bool> AddMovieActorAsync(MovieActor movieActor);
        Task<bool> EditMovieActorAsync(MovieActor movieActor);
        Task<bool> DeleteAllMovieActorsAsync(List<string> ids);
    }
}
