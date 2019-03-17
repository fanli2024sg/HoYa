using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoYa.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual ICollection<UserRoleModel> Roles { get; set; } = new HashSet<UserRoleModel>();
    }

    public class UserRoleModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string RoleValue { get; set; }
    }
}