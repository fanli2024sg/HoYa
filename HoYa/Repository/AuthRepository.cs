using HoYa.Entities;
using HoYa.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

#pragma warning disable 1591

namespace HoYa.Repository
{
    public class AuthRepository : IDisposable
    {
        private HoYaContext context;

        private UserManager<IdentityUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private UserStore<IdentityUser> userStore;
        public AuthRepository()
        {
            context = new HoYaContext();
            userStore = new UserStore<IdentityUser>(context);
            userManager = new UserManager<IdentityUser>(userStore);
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        }

        public async Task<AspNetUser> getUserByEmail(string email)
        {
            return await context.AspNetUsers.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async void setPassword(AspNetUser user, string hashedNewPassword)
        {
            await userStore.SetPasswordHashAsync(user, hashedNewPassword);
        }

        public string getHashedNewPassword(string newPassword)
        {
            return userManager.PasswordHasher.HashPassword(newPassword);
        }

        public async Task<AspNetUser> getUserByUserName(string userName)
        {
            return await context.AspNetUsers.FirstOrDefaultAsync(user => user.UserName == userName);
        }

        public async Task<AspNetRole> getRoleById(string id)
        {
            return await context.AspNetRoles.FirstOrDefaultAsync(role => role.Id == id);
        }

        public async Task<AspNetRole> FindRole(string id)
        {
            AspNetRole role = await roleManager.FindByIdAsync(id) as AspNetRole;
            return role;

        }


        public async Task<AspNetUser> FindUser(string userName, string password)
        {
            AspNetUser user = await userManager.FindAsync(userName, password) as AspNetUser;

            return user;
        }

      

     

        public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            IdentityUser user = await userManager.FindAsync(loginInfo);

            return user;
        }

        public async Task<IdentityResult> CreateUser(IdentityUser user)
        {
            return await userManager.CreateAsync(user);
        }

        public async Task<IdentityResult> CreateUser(IdentityUser user, string password)
        {

            return await userManager.CreateAsync(user, password);
        }
        public async Task<IdentityResult> DeleteAllUserRolesAsync(IdentityUser user)
        {

            var logins = user.Logins;
            var rolesForUser = await userManager.GetRolesAsync(user.Id);

            using (var transaction = context.Database.BeginTransaction())
            {
                foreach (var login in logins.ToList())
                {
                    await userManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }

                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        // item should be the name of the role
                        var result = await userManager.RemoveFromRoleAsync(user.Id, item);
                    }
                }
                transaction.Commit();
            }









            IdentityResult deleteRoleResult = new IdentityResult();

            return deleteRoleResult;
        }


        public async Task<IdentityResult> CreateUserRoleAsync(IdentityUser user, string roleName)
        {

            IdentityRole getRole = await roleManager.FindByNameAsync(roleName);
            if (getRole != null)
            {
                return await userManager.AddToRoleAsync(user.Id, getRole.Name);
            }
            else
            {
                IdentityRole newRole = new IdentityRole(roleName);
                IdentityResult newRoleResult = await CreateRole(new AspNetRole() { Name = roleName, Description = roleName });
                if (newRoleResult.Succeeded)
                {
                    getRole = await roleManager.FindByNameAsync(roleName);
                    return await userManager.AddToRoleAsync(user.Id, getRole.Name);
                }
                else
                {
                    return newRoleResult;
                }
            }
        }

        public async Task<IdentityResult> CreateUserRoleAsync(IdentityUser user, IdentityUserRole userRole)
        {
            IdentityResult createRoleResult = new IdentityResult();
            IdentityRole getRole = await roleManager.FindByIdAsync(userRole.RoleId);
            if (getRole != null)
            {
                createRoleResult = await userManager.AddToRoleAsync(user.Id, getRole.Name);
            }
            return createRoleResult;
        }

        public async Task<IdentityResult> CreateRole(IdentityRole role)
        {
            return await roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> UpdateRole(IdentityRole role)
        {
            return await roleManager.UpdateAsync(role);
        }
        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await userManager.AddLoginAsync(userId, login);

            return result;
        }

        public void Dispose()
        {
            context.Dispose();
            userManager.Dispose();

        }
    }
}