using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    //有證 ProfileId!=null
    //訪客 No!=null
    //關係人 No==null
    public class Person : Entity
    {
           public virtual Guid? ProfileId { get; set; }
        [ForeignKey("ProfileId")]
        public virtual Profile Profile { get; set; }
    }
 
    public class Profile : Entity
    {
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual AspNetUser User { get; set; }
        public string Value { get; set; }
        public string No { get; set; }
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }

    public class ProfileGroup : Relation<Profile, Group>
    {
    }
}