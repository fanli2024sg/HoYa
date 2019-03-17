using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System.Data.Entity;
using System;
using HoYa.Models;

namespace HoYa.Controllers
{
    //[Authorize]

    public class GroupsController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public IQueryable<Group> GetGroups()
        {
            return db.Groups;
        }
        [Route("api/Groups/Options")]
        public IQueryable<GroupOption> GetGroupOptions(
            Guid? typeId = null,
            string anyLike = "",
            int? pageIndex = 1,
            int? pageSize = 200)
        {
            return db.Groups.Where(x =>
             (x.TypeId == typeId || typeId == null) &&
             ((x.Value + "(" + x.Code + ")").Contains(anyLike) || anyLike == null || anyLike == "")
             ).OrderBy(x => (x.Value + "(" + x.Code + ")")).Take(pageSize.GetValueOrDefault()).Select(x => new GroupOption
             {
                 Id = x.Id,
                 Value = x.Value + "(" + x.Code + ")"
             });
        }

        [Route("api/Groups/Option/{id}")]
        public async Task<IHttpActionResult> GetGroupOption(
           Guid? id = null)
        {
            GroupOption groupOption = await db.Groups.Where(x => x.Id == id).Select(x => new GroupOption
            {
                Id = x.Id,
                Value = x.Value + "(" + x.Code + ")"
            }).FirstOrDefaultAsync();
            return Ok(groupOption);
        }

        [ResponseType(typeof(Group))]
        public async Task<IHttpActionResult> GetGroup(Guid id)
        {
            Group group = await db.Groups.FindAsync(id);

            if (group == null) return NotFound();
            return Ok(group);
        }
        [Route("api/Groups/ChangeGeneral/{id}")]
        [ResponseType(typeof(GroupChangeGeneral))]
        public async Task<IHttpActionResult> GetGroupChangeGeneral(Guid id)
        {
            GroupChangeGeneral groupChangeGeneral = await db.GroupChangeGenerals.FindAsync(id);
            return Ok(groupChangeGeneral);
        }
        
        [Route("api/Groups/Changes")]
        [ResponseType(typeof(GroupChange))]
        public IQueryable<GroupChange> GetGroupChanges(Guid? processId = null)
        {

            return db.GroupChanges.Where(x=>x.ProcessingId== processId);
        }
        [Route("api/Groups/ChangeGeneral")]
        [ResponseType(typeof(GroupChangeGeneral))]
        public async Task<IHttpActionResult> PostGroupChangeGeneral(GroupChangeGeneral groupChangeGeneral)
        {
            db.GroupChangeGenerals.Add(groupChangeGeneral);
            await db.SaveChangesAsync();
            await db.Entry(groupChangeGeneral).GetDatabaseValuesAsync();
          /*  switch (groupChangeGeneral.Process.Mission)
            {
                case "managers":
                    //string[] personIds = groupChange.PersonIds.Split(',');
                    //string[] gateIds = groupChange.GateIds.Split(',');
                    //foreach (string personId in personIds)
                    //{
                    //    Person person = await db.People.FirstOrDefaultAsync(x => x.Id.ToString() == personId);
                    //    foreach (string gateId in gateIds)
                    //    {
                    //        Gate gate = await db.Gates.FirstOrDefaultAsync(x => x.Id.ToString() == gateId);
                    //        GateCardChange gateCardChange = new GateCardChange
                    //        {
                    //            ProfileId = person.ProfileId,
                    //            ProcessId = groupChange.Process.Id,
                    //            OwnerId = gate.ManagerId,
                    //            PersonValue = (person.Document.SurName + person.Document.GivenName).Length > 0 ?
                    //                              person.Document.SurName + person.Document.GivenName + "(" + person.Profile.No + ")" :
                    //                              person.Document.EnglishGivenName + person.Document.EnglishSurName + "(" + person.Profile.No + ")",
                    //            GateValue = gate.Value + "(" + gate.No + ")",
                    //            GateId = gate.Id
                    //        };
                    //        db.GateCardChanges.Add(gateCardChange);
                    //    }
                    //}
                    //await db.SaveChangesAsync();
                    break;
                default:
                    break;
            }*/
            return Ok(groupChangeGeneral);
        }
        [Route("api/Groups/Change")]
        [ResponseType(typeof(GroupChangeGeneral))]
        public async Task<IHttpActionResult> PostGroupChange(GroupChange groupChange)
        {
            groupChange.Processing = await db.Processings.FindAsync(groupChange.ProcessingId);
            db.GroupChanges.Add(groupChange);
            await db.SaveChangesAsync();
            await db.Entry(groupChange).GetDatabaseValuesAsync();
          /*  switch (groupChange.Processing.StepId)
            {
                case "managers":
                    //string[] personIds = groupChange.PersonIds.Split(',');
                    //string[] gateIds = groupChange.GateIds.Split(',');
                    //foreach (string personId in personIds)
                    //{
                    //    Person person = await db.People.FirstOrDefaultAsync(x => x.Id.ToString() == personId);
                    //    foreach (string gateId in gateIds)
                    //    {
                    //        Gate gate = await db.Gates.FirstOrDefaultAsync(x => x.Id.ToString() == gateId);
                    //        GateCardChange gateCardChange = new GateCardChange
                    //        {
                    //            ProfileId = person.ProfileId,
                    //            ProcessId = groupChange.Process.Id,
                    //            OwnerId = gate.ManagerId,
                    //            PersonValue = (person.Document.SurName + person.Document.GivenName).Length > 0 ?
                    //                              person.Document.SurName + person.Document.GivenName + "(" + person.Profile.No + ")" :
                    //                              person.Document.EnglishGivenName + person.Document.EnglishSurName + "(" + person.Profile.No + ")",
                    //            GateValue = gate.Value + "(" + gate.No + ")",
                    //            GateId = gate.Id
                    //        };
                    //        db.GateCardChanges.Add(gateCardChange);
                    //    }
                    //}
                    //await db.SaveChangesAsync();
                    break;
                default:
                    break;
            }*/
            return Ok(groupChange);
        }
        [Route("api/Groups/ChangeGeneral/{id}")]
        [ResponseType(typeof(GroupChange))]
        public async Task<IHttpActionResult> PutGroupChangeGeneral(Guid id, GroupChangeGeneral groupChangeGeneral)
        {


            GroupChangeGeneral existedGroupChangeGeneral = await db.GroupChangeGenerals.FindAsync(id);
            db.Entry(existedGroupChangeGeneral).CurrentValues.SetValues(groupChangeGeneral);
            Process existedProcess = await db.Processings.FindAsync(existedGroupChangeGeneral.ProcessingId);
            db.Entry(existedProcess).CurrentValues.SetValues(groupChangeGeneral.Processing);
            foreach (GroupChange groupChange in db.GroupChanges.Where(x => x.ProcessingId == groupChangeGeneral.ProcessingId).ToArray())
            {
                db.GroupChanges.Remove(groupChange);
            }

            await db.SaveChangesAsync();


            /*switch (groupChangeGeneral.Process.Mission)
            {
                case "managers":
                    //foreach (GateCardChange gateCardChange in db.GateCardChanges.Where(x => x.ProcessId == groupChange.ProcessId).ToArray())
                    //{
                    //    db.GateCardChanges.Remove(gateCardChange);
                    //}
                    //await db.SaveChangesAsync();
                    //string[] profileIds = groupChange.PersonIds.Split(',');
                    //string[] gateIds = groupChange.GateIds.Split(',');
                    //foreach (string profileId in profileIds)
                    //{
                    //    Person person = await db.People.FirstOrDefaultAsync(x => x.ProfileId.ToString() == profileId);
                    //    foreach (string gateId in gateIds)
                    //    {
                    //        Gate gate = await db.Gates.FirstOrDefaultAsync(x => x.Id.ToString() == gateId);
                    //        GateCardChange gateCardChange = new GateCardChange
                    //        {
                    //            ProfileId = person.ProfileId,
                    //            ProcessId = groupChange.Process.Id,
                    //            OwnerId = gate.ManagerId,
                    //            PersonValue = (person.Document.SurName + person.Document.GivenName).Length > 0 ?
                    //                              person.Document.SurName + person.Document.GivenName + "(" + person.Profile.No + ")" :
                    //                              person.Document.EnglishGivenName + person.Document.EnglishSurName + "(" + person.Profile.No + ")",
                    //            GateValue = gate.Value + "(" + gate.No + ")",
                    //            GateId = gate.Id
                    //        };
                    //        db.GateCardChanges.Add(gateCardChange);
                    //    }
                    //}
                    //await db.SaveChangesAsync();
                    break;
                default:
                    break;
            }*/
            return Ok(groupChangeGeneral);
        }
        public async Task<IHttpActionResult> PutGroup(Guid id, Group group)
        {
            Group existedGroup = await db.Groups.FindAsync(id);
            db.Entry(existedGroup).CurrentValues.SetValues(group);
            await db.SaveChangesAsync();
            await db.Entry(existedGroup).GetDatabaseValuesAsync();
            return Ok(existedGroup);
        }

        [ResponseType(typeof(int))]
        public async Task<IHttpActionResult> PostGroup(Group group)
        {
            group.Id = Guid.NewGuid();
            db.Groups.Add(group);
            await db.SaveChangesAsync();
            await db.Entry(group).GetDatabaseValuesAsync();
            return Ok(group);
        }

        public async Task<IHttpActionResult> DeleteGroup(Guid id)
        {
            Group group = await db.Groups.FindAsync(id);
            if (group == null) return NotFound();
            db.Groups.Remove(group);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}