using HoYa.Entities;
using HoYa.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Security.Claims;
using System.Web.Http;

#pragma warning disable 1591

namespace HoYa.Repository
{
    [Authorize]
    public class HoYaRepository : IDisposable
    {
        private HoYaContext db;
        public HoYaRepository(HoYaContext db)
        {
            this.db = db;
        }

        public IEnumerable<Function> Functions(string userName)
        {
            return db.Functions.Where(function =>
                       db.FunctionGroups.Where(x =>
                           db.Groups.Where(group =>
                               db.ProfileGroups.Where(y =>
                                   y.TargetId == db.Profiles.FirstOrDefault(profile =>
                                       profile.UserId == db.AspNetUsers.FirstOrDefault(z => z.UserName == userName)
                                       .Id)
                                   .Id)
                               .Any(d => d.TargetId == group.Id))
                           .Any(group => group.Id == x.OwnerId))
                       .Any(x => x.TargetId == function.Id))
                   .AsEnumerable();
        }
        
        public void Dispose()
        {
            db.Dispose();
        }
    }
}