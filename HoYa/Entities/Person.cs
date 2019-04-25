using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    public class PersonDefinition : Definition
    {
    }
    public class Person : Branch<PersonDefinition>
    {
    }
    public class Contact : TypeSimpleDetail<Person>
    {
    }
    public class PersonChange : Change<Person>
    {
    }
    public class Profile : RealTypeSimpleInstance<Person, PersonChange>
    {
        public virtual string UserId { get; set; }
        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual AspNetUser User { get; set; }
    }
    public class ProfileGroup : Relation<Profile, Group>
    {
    }
    public class ProfileActivity : Relation<Profile, Activity>
    {
    }
}