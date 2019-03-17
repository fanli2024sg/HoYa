using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Security.Claims;
using System;
using HoYa.Providers;

namespace HoYa.Controllers
{
    public class AppliesController : ApiController
    {
        private HoYaContext db = new HoYaContext();



        public IQueryable<Apply> GetApplies()
        {
            return db.Applies;
        }

        [ResponseType(typeof(Apply))]
        public async Task<IHttpActionResult> GetApply(Guid id)
        {
            Apply recruit = await db.Applies.FindAsync(id);
            if (recruit == null) return NotFound();
            return Ok(recruit);
        }


        public async Task<IHttpActionResult> PutApply(Guid id, Apply recruit)
        {
            Apply existedApply = await db.Applies.FindAsync(id);
            db.Entry(existedApply).CurrentValues.SetValues(recruit);
            await db.SaveChangesAsync();
            await db.Entry(existedApply).GetDatabaseValuesAsync();
            return Ok(existedApply);
        }

        [ResponseType(typeof(Apply))]
        public async Task<IHttpActionResult> PostApply(Apply apply)
        {/*
            apply.Id = Guid.NewGuid();
            GlobalProvider globalProvider = new GlobalProvider();
            string identityDocumentNo = apply.Owner.IdentityDocument.No;
            Document existedIdentification = await db.Documents.FirstOrDefaultAsync(x => x.No == identityDocumentNo);
            string phoneValue = apply.Owner.Phone.Value;
            Contact existedContact = await db.Contacts.FirstOrDefaultAsync(x => x.Value == phoneValue);
            if (existedIdentification != null)
            {
                if (existedContact != null)
                {
                    Apply existedApply = await db.Applies.FirstOrDefaultAsync(x => x.Owner.IdentityDocument.No == identityDocumentNo && x.Owner.Phone.Value == phoneValue);
                    return Ok(existedApply);
                }
                else return NotFound();
            }
            Profile profile = new Profile
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                Gallery = new Folder
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now
                }
            };
            db.Profiles.Add(profile);
            await db.SaveChangesAsync();
            await db.Entry(profile).GetDatabaseValuesAsync();
            if (identityDocumentNo.Length == 15) identityDocumentNo = identityDocumentNo.Substring(0, 6) + "19" + identityDocumentNo.Substring(6) + "0";
            Location birthPlace = db.Locations.FirstOrDefault(x => x.Code == identityDocumentNo.Substring(0, 6));
            string sexCode = (Convert.ToInt16(identityDocumentNo.Substring(14, 3)) % 2).ToString();

            apply = new Apply
            {
                Id = Guid.NewGuid(),
                Owner = new Person
                {
                    Id = Guid.NewGuid(),
                    Profile = profile,
                    ProfileId = profile.Id,
                    Phone = new Contact
                    {
                        Id = Guid.NewGuid(),
                        Value = apply.Owner.Phone.Value,
                        Name = "本人手機",
                        TypeId = globalProvider.ContactType.PhoneId,
                        OwnerId = profile.Id,
                        CreatedById = profile.Id,
                        CreatedDate = DateTime.Now
                    },
                    Address = new Contact
                    {
                        Id = Guid.NewGuid(),
                        Value = apply.Owner.Phone.Value,
                        Name = "本人現居地址",
                        TypeId = globalProvider.ContactType.AddressId,
                        OwnerId = profile.Id,
                        CreatedById = profile.Id,
                        CreatedDate = DateTime.Now
                    },
                    Email = new Contact
                    {
                        Id = Guid.NewGuid(),
                        Name = "本人個人電郵",
                        TypeId = globalProvider.ContactType.EmailId,
                        OwnerId = profile.Id,
                        CreatedById = profile.Id,
                        CreatedDate = DateTime.Now
                    },
                    Emergency = new Relationship
                    {
                        Id = Guid.NewGuid(),
                        Phone = new Contact
                        {
                            Id = Guid.NewGuid(),
                            Value = apply.Owner.Phone.Value,
                            Name = "緊急連絡人手機",
                            TypeId = globalProvider.ContactType.PhoneId,
                            OwnerId = profile.Id,
                            CreatedById = profile.Id,
                            CreatedDate = DateTime.Now
                        },
                        TypeId = globalProvider.RelationshipType.EmergencyId,
                        CreatedById = profile.Id,
                        CreatedDate = DateTime.Now
                    },
                    BirthDate = identityDocumentNo.Substring(6, 4) + "/" + identityDocumentNo.Substring(10, 2) + "/" + identityDocumentNo.Substring(12, 2),
                    IdentityDocument = new Document
                    {
                        Id = Guid.NewGuid(),
                        No = apply.Owner.IdentityDocument.No,
                        TypeId = globalProvider.RelationshipType.EmergencyId,
                        BirthPlace = birthPlace,
                        Address = new Contact
                        {
                            Id = Guid.NewGuid(),
                            Name = "本人證件地址",
                            Value = birthPlace.Parent.Parent.Value + birthPlace.Parent.Value + birthPlace.Value,
                            TypeId = globalProvider.ContactType.AddressId,
                            OwnerId = profile.Id,
                            CreatedById = profile.Id,
                            CreatedDate = DateTime.Now,
                        },
                        CreatedById = profile.Id,
                        CreatedDate = DateTime.Now

                    },
                    SexId = db.Options.Where(x => x.ParentId == globalProvider.SexParentId).FirstOrDefault(x => x.Code == sexCode).Id,
                    CreatedById = profile.Id,
                    CreatedDate = DateTime.Now
                },
                CreatedById = profile.Id,
                CreatedDate = DateTime.Now
            };
            db.Applies.Add(apply);
            await db.SaveChangesAsync();
            await db.Entry(apply).GetDatabaseValuesAsync();*/
            return Ok(apply);
        }

        public async Task<IHttpActionResult> DeleteApply(Guid id)
        {
            Apply recruit = await db.Applies.FindAsync(id);
            if (recruit == null)
            {
                return NotFound();
            }
            db.Applies.Remove(recruit);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}