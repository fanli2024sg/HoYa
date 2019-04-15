using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using HoYa.Models;
using HoYa.Repository;
using System.Security.Claims;

namespace HoYa.Controllers
{
    [Authorize]
    public class EnquiriesController : ApiController
    {
        private HoYaContext db = new HoYaContext();


        public IQueryable<Enquiry> GetEnquiries(
       Guid? ownerId = null)
        {
            return db.Enquiries.Where(x => x.OwnerId == ownerId || ownerId == null);
        }

        [Route("api/Enquiries/General")]
        public async Task<IHttpActionResult> GetEnquiryGenerals(
            string anyLike = "",
            string sortBy = "",
            string orderBy = "",
            int? pageIndex = 1,
            int? pageSize = 20)
        {
            IQueryable<EnquiryGeneral> enquiryGenerals = db.EnquiryGenerals.Where(x => (x.CustomerName.Contains(anyLike) || anyLike == null) ||
                           (x.ContactPerson.Contains(anyLike) || anyLike == null) ||
                           (x.ContactValue.Contains(anyLike) || anyLike == null) ||
                           (x.Content.Contains(anyLike) || anyLike == ""));
            switch (sortBy)
            {
                case "contactPerson":
                    if (orderBy == "asc")
                        return Ok(new Query<EnquiryGeneral>
                        {
                            PaginatorLength = enquiryGenerals.Count(),
                            Data = enquiryGenerals.OrderBy(x => x.ContactPerson).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<EnquiryGeneral>()
                        });
                    else return Ok(new Query<EnquiryGeneral>
                    {
                        PaginatorLength = enquiryGenerals.Count(),
                        Data = enquiryGenerals.OrderByDescending(x => x.ContactPerson).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<EnquiryGeneral>()
                    });
                case "contactValue":
                    if (orderBy == "asc") return Ok(new Query<EnquiryGeneral>
                    {
                        PaginatorLength = enquiryGenerals.Count(),
                        Data = enquiryGenerals.OrderBy(x => x.ContactValue).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<EnquiryGeneral>()
                    });
                    else return Ok(new Query<EnquiryGeneral>
                    {
                        PaginatorLength = enquiryGenerals.Count(),
                        Data = enquiryGenerals.OrderByDescending(x => x.ContactValue).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<EnquiryGeneral>()
                    });
                case "processNo":
                    if (orderBy == "asc") return Ok(new Query<EnquiryGeneral>
                    {
                        PaginatorLength = enquiryGenerals.Count(),
                        Data = enquiryGenerals.OrderBy(x => x.Process.No).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<EnquiryGeneral>()
                    });
                    else return Ok(new Query<EnquiryGeneral>
                    {
                        PaginatorLength = enquiryGenerals.Count(),
                        Data = enquiryGenerals.OrderByDescending(x => x.Process.No).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<EnquiryGeneral>()
                    });
                case "createdDate":
                    if (orderBy == "asc") return Ok(new Query<EnquiryGeneral>
                    {
                        PaginatorLength = enquiryGenerals.Count(),
                        Data = enquiryGenerals.OrderBy(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<EnquiryGeneral>()
                    });
                    else return Ok(new Query<EnquiryGeneral>
                    {
                        PaginatorLength = enquiryGenerals.Count(),
                        Data = enquiryGenerals.OrderByDescending(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<EnquiryGeneral>()
                    });
                case "customerName":
                    if (orderBy == "asc") return Ok(new Query<EnquiryGeneral>
                    {
                        PaginatorLength = enquiryGenerals.Count(),
                        Data = enquiryGenerals.OrderBy(x => x.CustomerName).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<EnquiryGeneral>()
                    });
                    else return Ok(new Query<EnquiryGeneral>
                    {
                        PaginatorLength = enquiryGenerals.Count(),
                        Data = enquiryGenerals.OrderByDescending(x => x.CustomerName).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<EnquiryGeneral>()
                    });
                default:
                    if (orderBy == "asc") return Ok(new Query<EnquiryGeneral>
                    {
                        PaginatorLength = enquiryGenerals.Count(),
                        Data = enquiryGenerals.OrderBy(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<EnquiryGeneral>()
                    });
                    else return Ok(new Query<EnquiryGeneral>
                    {
                        PaginatorLength = enquiryGenerals.Count(),
                        Data = enquiryGenerals.OrderByDescending(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<EnquiryGeneral>()
                    });
            }
        }

        [Route("api/Enquiries/General")]
        public async Task<IHttpActionResult> PostEnquiryGeneral(EnquiryGeneral enquiryGeneral)
        {
            WorkFlow workFlow = await db.WorkFlows.FindAsync(enquiryGeneral.Process.DefinitionId);
            string codeYearMonth = workFlow.Code + DateTime.Now.ToString("yyyyMM");
            enquiryGeneral.Process.No = codeYearMonth + (db.Processes.Where(x => x.DefinitionId == enquiryGeneral.Process.DefinitionId && x.No.Substring(0, 10) == codeYearMonth).Count() + 1).ToString("0000");
            string userId = (User as ClaimsPrincipal).Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            Guid profileId = (await db.Profiles.FirstOrDefaultAsync(x => x.UserId == userId)).Id;
            enquiryGeneral.Process.CreatedById = profileId;
            enquiryGeneral.CreatedById = profileId;
            db.EnquiryGenerals.Add(enquiryGeneral);
            await db.SaveChangesAsync();
            return Ok(enquiryGeneral);
        }
        [Route("api/Enquiries/General/{id}")]
        public async Task<IHttpActionResult> GetEnquiryGeneral(Guid id)
        {
            EnquiryGeneral existedEnquiryGeneral = await db.EnquiryGenerals.FindAsync(id);
            return Ok(existedEnquiryGeneral);
        }
        [Route("api/Enquiries/General/{id}")]
        public async Task<IHttpActionResult> PutEnquiryGeneral(Guid? id, EnquiryGeneral enquiryGeneral)
        {
            EnquiryGeneral existedEnquiryGeneral = await db.EnquiryGenerals.FindAsync(id);
            db.Entry(existedEnquiryGeneral).CurrentValues.SetValues(enquiryGeneral);
            await db.SaveChangesAsync();
            return Ok();
        }

        public async Task<IHttpActionResult> PostEnquiry(Enquiry enquiry)
        {
            enquiry.Material = await db.Materials.FindAsync(enquiry.MaterialId);
            db.Enquiries.Add(enquiry);
            await db.SaveChangesAsync();
            return Ok(enquiry);
        }



        public async Task<IHttpActionResult> PutEnquiry(Guid id, Enquiry enquiry)
        {
            Enquiry existedEnquiry = await db.Enquiries.FindAsync(id);
            db.Entry(existedEnquiry).CurrentValues.SetValues(enquiry);
            await db.SaveChangesAsync();
            return Ok(existedEnquiry);
        }


        public async Task<IHttpActionResult> DeleteEnquiry(Guid id)
        {
            Enquiry existedEnquiry = await db.Enquiries.FindAsync(id);
            if(existedEnquiry!=null) db.Enquiries.Remove(existedEnquiry); 
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}