using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Security.Claims;
using HoYa.Models;
using System.Web;
using HoYa.Entities;
using HoYa.Repository;
using System.Data.Entity;
using System;
using System.Collections.Generic;

namespace HoYa.Controllers
{
    [Authorize]
    public class SettingsController : ApiController
    {
        private HoYaContext db = new HoYaContext();
        private AuthRepository repository = null;

        public SettingsController()
        {
            repository = new AuthRepository();
        }

        public async Task<SettingModel> GetSetting()
        {
            string userName = (User as ClaimsPrincipal).Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            AspNetUser aspNetUser = db.AspNetUsers.FirstOrDefault(x => x.Id == userName || x.UserName == userName);
            if (aspNetUser == null) aspNetUser = await CreateUser(userName, repository.getHashedNewPassword(userName));
            Inventory profile = null;
            profile = await db.Inventories.FirstOrDefaultAsync(x => x.UserId == aspNetUser.Id);
            if (profile == null) profile = await CreateInventory(aspNetUser);

            Inventory p = new Inventory("d5b63e24-1d67-4983-8803-1cb2bd80a2be");
            p.Id = profile.Id;
            p.No = profile.No;
            return new SettingModel
            {
                AD = HttpContext.Current.User.Identity.Name,
                Profile = p,
                IP = GetIpAddress()
            };
        }

        private async Task<Inventory> CreateInventory(AspNetUser aspNetUser)
        {
            db.Inventories.Add(new Inventory("d5b63e24-1d67-4983-8803-1cb2bd80a2be")
            {
                UserId = aspNetUser.Id,
                No = aspNetUser.UserName
            });
            await db.SaveChangesAsync();
            return await db.Inventories.FirstOrDefaultAsync(x => x.UserId == aspNetUser.Id && x.No == aspNetUser.UserName);
        }


        private async Task<AspNetUser> CreateUser(string userName, string password)
        {
            password = repository.getHashedNewPassword(userName).Substring(0, 8);
            await repository.CreateUser(new AspNetUser()
            {
                UserName = userName
            }, password);
            string roleId = db.AspNetRoles.FirstOrDefault(x => x.Name == "網域使用者").Id;
            AspNetUser aspNetUser = db.AspNetUsers.FirstOrDefault(x => x.UserName == userName);
            db.AspNetUserRoles.Add(new AspNetUserRole { RoleId = roleId, UserId = aspNetUser.Id });
            await db.SaveChangesAsync();
            return aspNetUser;
        }

        private string GetIpAddress()
        {
            string userIP = "未获取用户IP";
            try
            {
                if (HttpContext.Current == null || HttpContext.Current.Request == null || HttpContext.Current.Request.ServerVariables == null) return "";

                string CustomerIP = "";

                //CDN加速后取到的IP 
                CustomerIP = HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                CustomerIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];


                if (!String.IsNullOrEmpty(CustomerIP))
                    return CustomerIP;

                if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    CustomerIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (CustomerIP == null)
                        CustomerIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    CustomerIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                }

                if (string.Compare(CustomerIP, "unknown", true) == 0)
                    return HttpContext.Current.Request.UserHostAddress;
                return CustomerIP;
            }
            catch { }

            return userIP;
        }

    }
}