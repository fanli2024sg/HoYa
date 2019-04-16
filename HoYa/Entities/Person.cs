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
    public class Person : TypeDefinition
    {
    }

    public class Profile : RealSimpleGeneral<Person>
    {
        public virtual string UserId { get; set; }
        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual AspNetUser User { get; set; }
    }

    public class ProfileGroup : Relation<Profile, Group>
    {
    }
}