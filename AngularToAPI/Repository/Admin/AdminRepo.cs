using AngularToAPI.Models;
using AngularToAPI.ModelViews;
using AngularToAPI.ModelViews.users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace AngularToAPI.Repository.Admin
{
    public class AdminRepo : IAdminRepository
    {
        private readonly ApplicationDb _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        [Obsolete]
        private readonly IHostingEnvironment _host;

        [Obsolete]
        public AdminRepo(ApplicationDb db, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IHostingEnvironment host)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _host = host;
        }

        public async Task<ApplicationUser> AddUserAsync(AddUserModel model)
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
                if (await _roleManager.RoleExistsAsync("User"))
                {
                    if (!await _userManager.IsInRoleAsync(user, "User") && !await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }
                }
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

        public async Task<IEnumerable<UserRolesModel>> GetUserRoleAsync()
        {
            var query = await (
                from userRole in _db.UserRoles
                join users in _db.Users
                on userRole.UserId equals users.Id
                join roles in _db.Roles
                on userRole.RoleId equals roles.Id
                select new
                {
                    userRole.UserId,
                    users.UserName,
                    userRole.RoleId,
                    roles.Name
                }).ToListAsync();

            List<UserRolesModel> userRolesModels = new List<UserRolesModel>();
            if (query.Count > 0)
            {
                for (int i = 0; i < query.Count; i++)
                {
                    var model = new UserRolesModel
                    {
                        UserId = query[i].UserId,
                        UserName = query[i].UserName,
                        RoleId = query[i].RoleId,
                        RoleName = query[i].Name
                    };
                    userRolesModels.Add(model);
                }
            }
            return userRolesModels;
        }

        public async Task<IEnumerable<ApplicationRole>> GetRolesAsync()
        {
            return await _db.Roles.ToListAsync();
        }

        public async Task<bool> EditUserRoleAsync(EditUserRoleModel model)
        {
            if (model.UserId == null || model.RoleId == null)
            {
                return false;
            }

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            if (user == null)
            {
                return false;
            }

            var currentRoleId = await _db.UserRoles.Where(x => x.UserId == model.UserId).Select(x => x.RoleId).FirstOrDefaultAsync();
            var currentRoleName = await _db.Roles.Where(x => x.Id == currentRoleId).Select(x => x.Name).FirstOrDefaultAsync();
            var newRoleName = await _db.Roles.Where(x => x.Id == model.RoleId).Select(x => x.Name).FirstOrDefaultAsync();

            if (await _userManager.IsInRoleAsync(user, currentRoleName))
            {
                var x = await _userManager.RemoveFromRoleAsync(user, currentRoleName);
                if (x.Succeeded)
                {
                    var s = await _userManager.AddToRoleAsync(user, newRoleName);
                    if (s.Succeeded)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<Category> AddCategoryAsync(Category model)
        {
            var category = new Category
            {
                CategoryName = model.CategoryName
            };
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            return category;
        }

        public async Task<Category> EditCategoryAsync(Category model)
        {
            if (model == null || model.Id < 1)
            {
                return null;
            }

            var category = await _db.Categories.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (category == null)
            {
                return null;
            }
            _db.Categories.Attach(category);
            category.CategoryName = model.CategoryName;
            _db.Entry(category).Property(x => x.CategoryName).IsModified = true;
            await _db.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategoriesAsync(List<string> ids)
        {
            if (ids.Count < 1)
            {
                return false;
            }

            var i = 0;
            foreach (var id in ids)
            {
                try
                {
                    var catId = int.Parse(id);
                    var category = await _db.Categories.FirstOrDefaultAsync(x => x.Id == catId);
                    if (category != null)
                    {
                        _db.Categories.Remove(category);
                        i++;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            if (i > 0)
            {
                await _db.SaveChangesAsync();
            }
            return true;
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategoriesAsync()
        {
            return await _db.SubCategories.Include(x => x.Category).ToListAsync();
        }

        public async Task<SubCategory> AddSubCategoryAsync(SubCategory model)
        {
            var subCategory = new SubCategory
            {
                SubCategoryName = model.SubCategoryName,
                CategoryId = model.CategoryId
            };
            _db.SubCategories.Add(subCategory);
            await _db.SaveChangesAsync();
            return subCategory;
        }

        public async Task<SubCategory> EditSubCategoryAsync(SubCategory model)
        {
            if (model == null || model.Id < 1)
            {
                return null;
            }

            var subCategory = await _db.SubCategories.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (subCategory == null)
            {
                return null;
            }
            _db.SubCategories.Attach(subCategory);
            subCategory.SubCategoryName = model.SubCategoryName;
            subCategory.CategoryId = model.CategoryId;

            _db.Entry(subCategory).Property(x => x.SubCategoryName).IsModified = true;
            _db.Entry(subCategory).Property(x => x.CategoryId).IsModified = true;

            await _db.SaveChangesAsync();
            return subCategory;
        }

        public async Task<bool> DeleteSubCategoriesAsync(List<string> ids)
        {
            if (ids.Count < 1)
            {
                return false;
            }

            var i = 0;
            foreach (var id in ids)
            {
                try
                {
                    var subCatId = int.Parse(id);
                    var subCategory = await _db.SubCategories.FirstOrDefaultAsync(x => x.Id == subCatId);
                    if (subCategory != null)
                    {
                        _db.SubCategories.Remove(subCategory);
                        i++;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            if (i > 0)
            {
                await _db.SaveChangesAsync();
            }
            return true;
        }

        public async Task<IEnumerable<Actor>> GetActorsAsync()
        {
            return await _db.Actors.ToListAsync();
        }

        [Obsolete]
        public async Task<Actor> AddActorAsync(string actorName, IFormFile img)
        {
            // Real path
            // var filePath = Path.Combine(_host.WebRootPath + "/images/actors", img.FileName); 
            var filePath = Path.Combine(@"E:\Lab\AngularTutorial\CinamaMovies\src\assets\images\actors", img.FileName);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await img.CopyToAsync(fileStream);
            }

            var actor = new Actor
            {
                ActorName = actorName,
                ActorPicture = img.FileName
            };
            _db.Actors.Add(actor);
            await _db.SaveChangesAsync();
            return actor;
        }

        [Obsolete]
        public async Task<Actor> GetActorAsync(int id)
        {
            var actor = await _db.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if (actor == null)
            {
                return null;
            }

            //// real path
            //var newActor = new Actor
            //{
            //    Id = actor.Id,
            //    ActorName = actor.ActorName,
            //    ActorPicture = $"{_host.WebRootPath}/images/actors/{actor.ActorPicture}"
            //};
            return actor;
        }

        [Obsolete]
        public async Task<Actor> EditActorAsync(int id, string actorName, IFormFile img)
        {
            var actor = await _db.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if (actor == null)
            {
                return null;
            }

            _db.Attach(actor);
            actor.ActorName = actorName;
            if (img != null && img.FileName.ToLower() != actor.ActorPicture.ToLower())
            {
                // Real path
                // var filePath = Path.Combine(_host.WebRootPath + "/images/actors", img.FileName); 
                var filePath = Path.Combine(@"E:\Lab\AngularTutorial\CinamaMovies\src\assets\images\actors", img.FileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await img.CopyToAsync(fileStream);
                }
                actor.ActorPicture = img.FileName;
                _db.Entry(actor).Property(x => x.ActorPicture).IsModified = true;
            }
            _db.Entry(actor).Property(x => x.ActorName).IsModified = true;
            await _db.SaveChangesAsync();
            return actor;
        }

        public async Task<bool> DeleteActorsAsync(List<string> ids)
        {
            if (ids.Count < 1)
            {
                return false;
            }

            int i = 0;
            foreach (var id in ids)
            {
                try
                {
                    var actorId = int.Parse(id);
                    var actor = await _db.Actors.FirstOrDefaultAsync(x => x.Id == actorId);
                    if (actor != null)
                    {
                        _db.Actors.Remove(actor);
                        i++;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            if (i > 0)
            {
                await _db.SaveChangesAsync();
            }
            return true;
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            return await _db.Movies.OrderByDescending(x => x.Id).Include(x => x.SubCategory).ToListAsync();
        }

        public async Task<bool> AddMovieAsync(IFormFile img, IFormFile video, string story, string movieName, string trailer,
            string catId, List<int> ids, string[] links)
        {
            foreach (var mov in await _db.Movies.ToListAsync())
            {
                if (mov.MovieName.Trim().ToLower() == movieName.Trim().ToLower())
                {
                    return false;
                }
            }

            var movie = new Movie
            {
                MovieName = movieName,
                MovieStory = story,
                MovieTrailer = trailer,
                MoviePost = img.FileName,
                SubCategoryId = int.Parse(catId)
            };
            _db.Movies.Add(movie);
            await _db.SaveChangesAsync();

            foreach (var id in ids)
            {
                var movieActors = new MovieActor
                {
                    MovieId = movie.Id,
                    ActorId = id
                };
                _db.MovieActors.Add(movieActors);
                await _db.SaveChangesAsync();
            }

            if (links.Count() >= 0)
            {
                for (int i = 0; i < links.Count(); i++)
                {
                    var movieLink = new MovieLink
                    {
                        MovLink = links[i],
                        MovieId = movie.Id
                    };
                    _db.MovieLinks.Add(movieLink);
                    await _db.SaveChangesAsync();
                }
            }

            // Real path
            // var filePath = Path.Combine(_host.WebRootPath + "/images/posts", img.FileName); 
            var filePath = Path.Combine(@"E:\Lab\AngularTutorial\CinamaMovies\src\assets\images\posts", img.FileName);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await img.CopyToAsync(fileStream);
            }

            // Real path
            //filePath = Path.Combine(_host.WebRootPath + "/Videos", video.FileName);
            filePath = Path.Combine(@"E:\Lab\AngularTutorial\CinamaMovies\src\assets\videos", video.FileName);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await video.CopyToAsync(fileStream);
                var movieLink = new MovieLink
                {
                    MovLink = video.FileName,
                    MovieId = movie.Id
                };
                _db.MovieLinks.Add(movieLink);
                await _db.SaveChangesAsync();
            }

            return true;
        }

        public async Task<Movie> GetMovieAsync(long id)
        {
            return await _db.Movies.Include(x => x.SubCategory).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> EditMovieAsync(Movie movie, IFormFile img)
        {
            var mov = await _db.Movies.FirstOrDefaultAsync(x => x.Id == movie.Id);
            if (mov == null)
            {
                return false;
            }

            try
            {
                _db.Attach(mov);
                mov.MovieName = movie.MovieName;
                mov.MovieTrailer = movie.MovieTrailer;
                mov.MovieStory = movie.MovieStory;
                mov.SubCategoryId = movie.SubCategoryId;

                _db.Entry(mov).Property(x => x.MovieName).IsModified = true;
                _db.Entry(mov).Property(x => x.MovieTrailer).IsModified = true;
                _db.Entry(mov).Property(x => x.MovieStory).IsModified = true;
                _db.Entry(mov).Property(x => x.SubCategoryId).IsModified = true;

                if (mov.MoviePost.ToLower() != img.FileName.ToLower())
                {
                    mov.MoviePost = img.FileName;
                    _db.Entry(mov).Property(x => x.MoviePost).IsModified = true;
                }

                await _db.SaveChangesAsync();

                if (mov.MoviePost.ToLower() != img.FileName.ToLower())
                {
                    // Real path
                    // var filePath = Path.Combine(_host.WebRootPath + "/images/posts", img.FileName); 
                    var filePath = Path.Combine(@"E:\Lab\AngularTutorial\CinamaMovies\src\assets\images\posts", img.FileName);
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await img.CopyToAsync(fileStream);
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<Movie>> SearchMoviesAsync(string search)
        {
            return await _db.Movies.OrderByDescending(x => x.Id).Include(x => x.SubCategory)
                .Where(x => x.MovieName.ToLower().Contains(search.ToLower()) || x.SubCategory.SubCategoryName.ToLower().Contains(search.ToLower()))
                .ToListAsync();
        }

        public async Task<bool> DeleteMoviesAsync(List<string> ids)
        {
            if (ids.Count < 1)
            {
                return false;
            }

            int i = 0;
            foreach (var id in ids)
            {
                try
                {
                    var movieId = long.Parse(id);
                    var movie = await _db.Movies.FirstOrDefaultAsync(x => x.Id == movieId);
                    if (movie != null)
                    {
                        _db.Movies.Remove(movie);
                        i++;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            if (i > 0)
            {
                await _db.SaveChangesAsync();
            }
            return true;
        }

        public async Task<IEnumerable<MovieLink>> GetMovieLinksAsync(string search)
        {
            if (search == null || search == "null" || string.IsNullOrEmpty(search))
                return await _db.MovieLinks.Include(x => x.Movie).ThenInclude(x => x.SubCategory).ToListAsync();
            else
            {
                search = search?.ToLower();
                return await _db.MovieLinks.Include(x => x.Movie).ThenInclude(x => x.SubCategory)
                        .Where(x => x.Movie.MovieName.ToLower().Contains(search) || x.MovLink.ToLower().Contains(search))
                        .ToListAsync();
            }

        }

        public async Task<MovieLink> GetMovieLinkAsync(long id)
        {
            return await _db.MovieLinks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> AddMovieLinkAsync(MovieLink movieLink)
        {
            var movLink = new MovieLink
            {
                MovieId = movieLink.MovieId,
                MovLink = movieLink.MovLink,
                Quality = movieLink.Quality,
                Resolation = movieLink.Resolation
            };
            _db.MovieLinks.Add(movLink);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditMovieLinkAsync(MovieLink movieLink, IFormFile video)
        {
            var movLink = await _db.MovieLinks.FirstOrDefaultAsync(x => x.Id == movieLink.Id);
            if (movLink == null)
                return false;

            if (video != null && video.Length > 0)
            {
                if (movLink.MovLink.ToLower() != video.FileName.ToLower())
                {
                    // Real path
                    //filePath = Path.Combine(_host.WebRootPath + "/Videos", movieLink.Video.FileName);
                    var filePath = Path.Combine(@"E:\Lab\AngularTutorial\CinamaMovies\src\assets\videos", video.FileName);
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await video.CopyToAsync(fileStream);
                    }
                }

            }

            _db.Attach(movLink);
            movLink.MovieId = movieLink.MovieId;
            movLink.Resolation = movieLink.Resolation;
            movLink.Quality = movieLink.Quality;
            if (video != null && video.Length > 0)
            {
                if (movLink.MovLink.ToLower() != video.FileName.ToLower())
                {
                    movLink.MovLink = video.FileName;
                    _db.Entry(movLink).Property(x => x.MovLink).IsModified = true;
                }
            }
            else
            {
                movLink.MovLink = movieLink.MovLink;
                _db.Entry(movLink).Property(x => x.MovLink).IsModified = true;
            }

            _db.Entry(movLink).Property(x => x.MovieId).IsModified = true;
            _db.Entry(movLink).Property(x => x.Resolation).IsModified = true;
            _db.Entry(movLink).Property(x => x.Quality).IsModified = true;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllMovieLinksAsync(List<string> ids)
        {
            if (ids.Count < 1)
            {
                return false;
            }

            var i = 0;
            foreach (var id in ids)
            {
                try
                {
                    var moviLinkId = long.Parse(id);
                    var moviLink = await _db.MovieLinks.FirstOrDefaultAsync(x => x.Id == moviLinkId);
                    if (moviLink != null)
                    {
                        _db.MovieLinks.Remove(moviLink);
                        i++;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            if (i > 0)
            {
                await _db.SaveChangesAsync();
            }
            return true;
        }

        public async Task<IEnumerable<MovieActor>> GetMovieActorsAsync(string search)
        {
            if (search == null || search == "null" || string.IsNullOrEmpty(search))
                return await _db.MovieActors.Include(x => x.Actor).Include(x => x.Movie).ThenInclude(x => x.SubCategory).ToListAsync();
            else
            {
                search = search?.ToLower();
                return await _db.MovieActors.Include(x => x.Actor).Include(x => x.Movie).ThenInclude(x => x.SubCategory)
                        .Where(x => x.Movie.MovieName.ToLower().Contains(search) || x.Actor.ActorName.ToLower().Contains(search))
                        .ToListAsync();
            }
        }

        public async Task<MovieActor> GetMovieActorAsync(long id)
        {
            return await _db.MovieActors.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> AddMovieActorAsync(MovieActor movieActor)
        {
            var movActor = new MovieActor
            {
                MovieId = movieActor.MovieId,
                ActorId = movieActor.ActorId
            };
            _db.MovieActors.Add(movActor);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditMovieActorAsync(MovieActor movieActor)
        {
            var mov = await _db.MovieActors.FirstOrDefaultAsync(x => x.Id == movieActor.Id);
            if (mov == null)
                return false;

            _db.Attach(mov);
            mov.ActorId = movieActor.ActorId;
            mov.MovieId = movieActor.MovieId;
            _db.Entry(mov).Property(x => x.MovieId).IsModified = true; 
            _db.Entry(mov).Property(x => x.ActorId).IsModified = true;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllMovieActorsAsync(List<string> ids)
        {
            if (ids.Count < 1)
            {
                return false;
            }

            var i = 0;
            foreach (var id in ids)
            {
                try
                {
                    var moviActorId = long.Parse(id);
                    var movieActor = await _db.MovieActors.FirstOrDefaultAsync(x => x.Id == moviActorId);
                    if (movieActor != null)
                    {
                        _db.MovieActors.Remove(movieActor);
                        i++;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            if (i > 0)
            {
                await _db.SaveChangesAsync();
            }
            return true;
        }
    }
}
