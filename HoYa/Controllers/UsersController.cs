using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using HoYa.Entities;
using HoYa.Repository;
using Microsoft.Owin.Security;
using System.Net.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using HoYa.Models;

namespace HoYa.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {
        private HoYaContext db = new HoYaContext();
        private AuthRepository repository = null;
        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public UsersController()
        {
            repository = new AuthRepository();
        }

        /// <summary>  
        /// 取得所有【使用者】
        /// </summary>
        [ResponseType(typeof(UserModel))]
        public IQueryable<UserModel> GetUsers()
        {
            var users = db.AspNetUsers.Select(x => new UserModel
            {

                Id = x.Id,
                Email = x.Email,
                Password = "",
                Value = x.UserName,
                Roles = x.Roles.Select(y => new UserRoleModel
                {
                    RoleId = y.RoleId,
                    UserId = y.UserId,
                    RoleValue = ""
                }).ToList()
            }).ToList();

            foreach (var user in users)
            {
                foreach (var role in user.Roles) role.RoleValue = db.AspNetRoles.Find(role.RoleId).Name;
            }
            return users.AsQueryable();
        }

        /// <summary>  
        /// 取得【使用者】
        /// </summary>
        /// <param name="id">使用者序號</param> 
        [ResponseType(typeof(AspNetUser))]
        public async Task<IHttpActionResult> GetUser(string id)
        {
            AspNetUser user = await db.AspNetUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>  
        /// 更新【使用者】
        /// </summary>  
        /// <param name="id">使用者序號</param>  
        /// <param name="userModel">使用者</param>
        public async Task<IHttpActionResult> PutUser(string id, UserModel userModel)
        {
            AspNetUser user = await db.AspNetUsers.FindAsync(id);
            AspNetUser existedUser = await db.AspNetUsers.FindAsync(id);
            if (existedUser == null) return NotFound();
            user.UserName = userModel.Value;
            user.Id = userModel.Id;
            user.Email = userModel.Email;
            if (userModel.Password != "")
                user.PasswordHash = repository.getHashedNewPassword(userModel.Password);
            db.Entry(existedUser).CurrentValues.SetValues(user);

            foreach (UserRoleModel userRoleModel in userModel.Roles)
            {
                if (existedUser.Roles.FirstOrDefault(role => role.RoleId == userRoleModel.RoleId) == null)
                    db.AspNetUserRoles.Add(new AspNetUserRole { RoleId = userRoleModel.RoleId, UserId = userRoleModel.UserId });
            }
            foreach (AspNetUserRole existedUserRole in existedUser.Roles.ToArray())
            {
                if (userModel.Roles.FirstOrDefault(role => role.RoleId == existedUserRole.RoleId) == null)
                    db.AspNetUserRoles.Remove(existedUserRole);
            }
            await db.SaveChangesAsync();
            await db.Entry(existedUser).GetDatabaseValuesAsync();

            return Ok(new UserModel
            {

                Id = existedUser.Id,
                Email = existedUser.Email,
                Password = "",
                Value = existedUser.UserName,
                Roles = existedUser.Roles.Select(x => new UserRoleModel
                {
                    RoleId = x.RoleId,
                    UserId = x.UserId,
                    RoleValue = db.Roles.Find(x.RoleId).Name
                }).ToList()
            });
        }

        public async Task<IHttpActionResult> PostUser(UserModel userModel)
        {
            userModel.Value = userModel.Value.Replace("\\", "_");
            if (userModel.Password == "") userModel.Password = repository.getHashedNewPassword(userModel.Value).Substring(0, 12);
            await repository.CreateUser(new AspNetUser()
            {
                Email = userModel.Email,
                UserName = userModel.Value
            }, userModel.Password);
            AspNetUser existedUser = db.AspNetUsers.FirstOrDefault(appUser => appUser.UserName == userModel.Value);
            foreach (UserRoleModel userRoleModel in userModel.Roles)
            {
                db.AspNetUserRoles.Add(new AspNetUserRole { RoleId = userRoleModel.RoleId, UserId = existedUser.Id });
            }
            await db.SaveChangesAsync();
            await db.Entry(existedUser).GetDatabaseValuesAsync();
            return Ok(new UserModel
            {

                Id = existedUser.Id,
                Email = existedUser.Email,
                Password = "",
                Value = existedUser.UserName,
                Roles = existedUser.Roles.Select(x => new UserRoleModel
                {
                    RoleId = x.RoleId,
                    UserId = x.UserId,
                    RoleValue = db.Roles.Find(x.RoleId).Name
                }).ToList()
            });
        }

        /// <summary>  
        /// 刪除【使用者】
        /// </summary>  
        /// <param name="id">使用者序號</param>
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            AspNetUser existedUser = await db.AspNetUsers.FindAsync(id);
            if (existedUser == null) return NotFound();
            foreach (AspNetUserRole existedUserRole in existedUser.Roles.ToArray())
            {
                db.AspNetUserRoles.Remove(existedUserRole);
            }
            db.AspNetUsers.Remove(existedUser);
            await db.SaveChangesAsync();
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}