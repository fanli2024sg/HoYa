using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Security.Claims;
using System;
using HoYa.Models;
using System.Collections.Generic;
using HoYa.Repository;

namespace HoYa.Controllers
{
    //[Authorize]
    public class PeopleController : ApiController
    {
        private HoYaContext db = new HoYaContext();
        public IQueryable<Person> GetPeople()
        {
            return db.People;
        }
        [Route("api/People/Options")]
        public IQueryable<PersonOption> GetPersonOptions(
            Guid? typeId = null,
            Guid? profileTypeId = null,
            Guid? experienceEmployerId = null,
            string anyLike = "",
            int? pageIndex = 1,
            int? pageSize = 200)
        {
            if (anyLike == null) anyLike = "";
            switch (typeId.ToString())
            {
                case "4052aa16-2b11-4729-9ca1-636a364f3841":
                    return db.People.Where(x =>
                    (x.TypeId == typeId || typeId == null) &&
                    (x.Experience.EmployerId == experienceEmployerId || experienceEmployerId == null) &&
                    (x.Profile.TypeId == profileTypeId || profileTypeId == null) &&
                    ((x.Document.SurName + x.Document.GivenName + "(" + x.Profile.No + ")").Contains(anyLike) ||
                    (x.Document.EnglishGivenName + x.Document.EnglishSurName + "(" + x.Profile.No + ")").Contains(anyLike))
                    ).OrderBy(x => x.Profile.No).Take(pageSize.GetValueOrDefault()).Select(x => new PersonOption
                    {
                        Id = x.Id,
                        Value = (x.Document.SurName + x.Document.GivenName + "(" + x.Profile.No + ")").Contains(anyLike) ?
                        x.Document.SurName + x.Document.GivenName + "(" + x.Profile.No + ")" :
                        x.Document.EnglishGivenName + x.Document.EnglishSurName + "(" + x.Profile.No + ")",
                        Name = (x.Document.SurName + x.Document.GivenName).Contains(anyLike) ?
                        x.Document.SurName + x.Document.GivenName :
                        x.Document.EnglishGivenName + x.Document.EnglishSurName,
                        PhoneValue = x.Phone.Value
                    });
                case "E1985957-093D-41BF-83F8-49315A853058":
                    return db.People.Where(x =>
                    (x.TypeId == typeId || typeId == null) &&
                    (x.Profile.TypeId == profileTypeId || profileTypeId == null) &&
                    ((x.Document.SurName + x.Document.GivenName + "(" + x.Document.No + ")").Contains(anyLike) ||
                    (x.Document.EnglishGivenName + x.Document.EnglishSurName + "(" + x.Document.No + ")").Contains(anyLike))
                    ).OrderBy(x => x.Document.SurName + x.Document.GivenName + "(" + x.Document.No + ")").Take(pageSize.GetValueOrDefault()).Select(x => new PersonOption
                    {
                        Id = x.Id,
                        Value = (x.Document.SurName + x.Document.GivenName + "(" + x.Document.No + ")").Contains(anyLike) ?
                        x.Document.SurName + x.Document.GivenName + "(" + x.Document.No + ")" :
                        x.Document.EnglishGivenName + x.Document.EnglishSurName + "(" + x.Document.No + ")"
                    });
                case "87C3549B-B2B0-4326-9223-625E6673517E":
                    return db.People.Where(x =>
                    (x.TypeId == typeId || typeId == null) &&
                    (x.Profile.TypeId == profileTypeId || profileTypeId == null) &&
                    ((x.Document.SurName + x.Document.GivenName + "(" + x.Phone.Value + ")").Contains(anyLike) ||
                    (x.Document.EnglishGivenName + x.Document.EnglishSurName + "(" + x.Phone.Value + ")").Contains(anyLike))
                    ).OrderBy(x => x.Document.SurName + x.Document.GivenName + "(" + x.Phone.Value + ")").Take(pageSize.GetValueOrDefault()).Select(x => new PersonOption
                    {
                        Id = x.Id,
                        Value = (x.Document.SurName + x.Document.GivenName + "(" + x.Phone.Value + ")").Contains(anyLike) ?
                        x.Document.SurName + x.Document.GivenName + "(" + x.Phone.Value + ")" :
                        x.Document.EnglishGivenName + x.Document.EnglishSurName + "(" + x.Phone.Value + ")"
                    });
                default:
                    return db.People.Where(x =>
                    (x.TypeId == typeId || typeId == null) &&
                    (x.Profile.TypeId == profileTypeId || profileTypeId == null) &&
                    ((x.Document.SurName + x.Document.GivenName + "(" + x.Profile.No + ")").Contains(anyLike) ||
                    (x.Document.EnglishGivenName + x.Document.EnglishSurName + "(" + x.Profile.No + ")").Contains(anyLike) ||
                    anyLike == null || anyLike == "")
                    ).OrderBy(x => x.Document.SurName + x.Document.GivenName + "(" + x.Profile.No + ")").Take(pageSize.GetValueOrDefault()).Select(x => new PersonOption
                    {
                        Id = x.Id,
                        Value = (x.Document.SurName + x.Document.GivenName + "(" + x.Profile.No + ")").Contains(anyLike) ?
                        x.Document.SurName + x.Document.GivenName + "(" + x.Profile.No + ")" :
                        x.Document.EnglishGivenName + x.Document.EnglishSurName + "(" + x.Profile.No + ")"
                    });
            }
        }


        [Route("api/People/Option")]
        public async Task<IHttpActionResult> GetPersonOption(
            Guid? profileId = null, Guid? id = null)
        {


            PersonOption personOption = await db.People.Where(x =>
                     (x.ProfileId == profileId || profileId == null) && (x.Id == id || id == null)).Select(x => new PersonOption
                     {
                         Id = x.Id,
                         Value = (x.Document.SurName + x.Document.GivenName).Length > 0 ?
                        x.Document.SurName + x.Document.GivenName + "(" + x.Profile.No + ")" :
                        x.Document.EnglishGivenName + x.Document.EnglishSurName + "(" + x.Profile.No + ")",
                         ProfileId = x.ProfileId
                     }).FirstOrDefaultAsync();
            return Ok(personOption);
        }

        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> GetPerson(Guid id)
        {
            Person person = await db.People.FindAsync(id);
            if (person == null) return NotFound();
            return Ok(person);
        }




        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> PutPerson(Guid id, Person person)
        {
            Person existedPerson = await db.People.FindAsync(id);
            db.Entry(existedPerson).CurrentValues.SetValues(person);
            await db.SaveChangesAsync();
            await db.Entry(existedPerson).GetDatabaseValuesAsync();
            return Ok(existedPerson);
        }


        [Route("api/People/Process")]
        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> PostPeopleProcess(Person person)
        {
            db.People.Add(person);
            await db.SaveChangesAsync();
            await db.Entry(person).GetDatabaseValuesAsync();
            return Ok(person);
        }

        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> PostPerson(Person person)
        {
            db.People.Add(person);
            await db.SaveChangesAsync();
            await db.Entry(person).GetDatabaseValuesAsync();
            return Ok(person);
        }

        public async Task<IHttpActionResult> DeletePerson(Guid id)
        {
            Person person = await db.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            db.People.Remove(person);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}