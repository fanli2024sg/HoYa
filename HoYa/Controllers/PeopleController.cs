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

            return db.People.Where(x => anyLike == null || anyLike == ""
            ).Select(x => new PersonOption
            {
                Id = x.Id,
                Value = ""
            });

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