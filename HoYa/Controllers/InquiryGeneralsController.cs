using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Data.Entity;
using System;
using HoYa.Models;
using System.Security.Claims;

namespace HoYa.Controllers
{
    [Authorize]
    public class InquiryGeneralsController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public async Task<IHttpActionResult> GetInquiryGenerals(
            string anyLike = "",
            string sortBy = "",
            string orderBy = "",
            int? pageIndex = 1,
            int? pageSize = 20)
        {
            IQueryable<InquiryGeneral> inquiryGenerals = db.InquiryGenerals.Where(x => (x.Owner.No.Contains(anyLike) || anyLike == null) ||
                           (x.Contact.Value.Contains(anyLike) || anyLike == null) ||
                           (x.Contact.Owner.Code.Contains(anyLike) || anyLike == null) ||
                           (x.Contact.Owner.Value.Contains(anyLike) || anyLike == null) ||
                           (x.Customer.Code.Contains(anyLike) || anyLike == null) ||
                           (x.Customer.Value.Contains(anyLike) || anyLike == null) ||
                           (x.Value.Contains(anyLike) || anyLike == ""));
            switch (sortBy)
            {
                case "ownerNo":
                    if (orderBy == "asc") return Ok(new Query<InquiryGeneral>
                    {
                        PaginatorLength = inquiryGenerals.Count(),
                        Data = inquiryGenerals.OrderBy(x => x.Owner.No).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<InquiryGeneral>()
                    });
                    else return Ok(new Query<InquiryGeneral>
                    {
                        PaginatorLength = inquiryGenerals.Count(),
                        Data = inquiryGenerals.OrderByDescending(x => x.Owner.No).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<InquiryGeneral>()
                    });
                case "contactValue":
                    if (orderBy == "asc")
                        return Ok(new Query<InquiryGeneral>
                        {
                            PaginatorLength = inquiryGenerals.Count(),
                            Data = inquiryGenerals.OrderBy(x => x.Contact.Value).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<InquiryGeneral>()
                        });
                    else return Ok(new Query<InquiryGeneral>
                    {
                        PaginatorLength = inquiryGenerals.Count(),
                        Data = inquiryGenerals.OrderByDescending(x => x.Contact.Value).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<InquiryGeneral>()
                    });
                case "contactOwnerCode":
                    if (orderBy == "asc") return Ok(new Query<InquiryGeneral>
                    {
                        PaginatorLength = inquiryGenerals.Count(),
                        Data = inquiryGenerals.OrderBy(x => x.Contact.Owner.Code).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<InquiryGeneral>()
                    });
                    else return Ok(new Query<InquiryGeneral>
                    {
                        PaginatorLength = inquiryGenerals.Count(),
                        Data = inquiryGenerals.OrderByDescending(x => x.Contact.Owner.Code).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<InquiryGeneral>()
                    });
                case "contactOwnerValue":
                    if (orderBy == "asc") return Ok(new Query<InquiryGeneral>
                    {
                        PaginatorLength = inquiryGenerals.Count(),
                        Data = inquiryGenerals.OrderBy(x => x.Contact.Owner.Value).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<InquiryGeneral>()
                    });
                    else return Ok(new Query<InquiryGeneral>
                    {
                        PaginatorLength = inquiryGenerals.Count(),
                        Data = inquiryGenerals.OrderByDescending(x => x.Contact.Owner.Value).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<InquiryGeneral>()
                    });
                case "customerCode":
                    if (orderBy == "asc") return Ok(new Query<InquiryGeneral>
                    {
                        PaginatorLength = inquiryGenerals.Count(),
                        Data = inquiryGenerals.OrderBy(x => x.Customer.Code).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<InquiryGeneral>()
                    });
                    else return Ok(new Query<InquiryGeneral>
                    {
                        PaginatorLength = inquiryGenerals.Count(),
                        Data = inquiryGenerals.OrderByDescending(x => x.Customer.Code).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<InquiryGeneral>()
                    });
                case "customerValue":
                    if (orderBy == "asc") return Ok(new Query<InquiryGeneral>
                    {
                        PaginatorLength = inquiryGenerals.Count(),
                        Data = inquiryGenerals.OrderBy(x => x.Customer.Value).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<InquiryGeneral>()
                    });
                    else return Ok(new Query<InquiryGeneral>
                    {
                        PaginatorLength = inquiryGenerals.Count(),
                        Data = inquiryGenerals.OrderByDescending(x => x.Customer.Value).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<InquiryGeneral>()
                    });
                case "value":
                    if (orderBy == "asc") return Ok(new Query<InquiryGeneral>
                    {
                        PaginatorLength = inquiryGenerals.Count(),
                        Data = inquiryGenerals.OrderBy(x => x.Value).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<InquiryGeneral>()
                    });
                    else return Ok(new Query<InquiryGeneral>
                    {
                        PaginatorLength = inquiryGenerals.Count(),
                        Data = inquiryGenerals.OrderByDescending(x => x.Value).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<InquiryGeneral>()
                    });
                case "createdDate":
                    if (orderBy == "asc") return Ok(new Query<InquiryGeneral>
                    {
                        PaginatorLength = inquiryGenerals.Count(),
                        Data = inquiryGenerals.OrderBy(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<InquiryGeneral>()
                    });
                    else return Ok(new Query<InquiryGeneral>
                    {
                        PaginatorLength = inquiryGenerals.Count(),
                        Data = inquiryGenerals.OrderByDescending(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<InquiryGeneral>()
                    });                
                default:
                    if (orderBy == "asc") return Ok(new Query<InquiryGeneral>
                    {
                        PaginatorLength = inquiryGenerals.Count(),
                        Data = inquiryGenerals.OrderBy(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<InquiryGeneral>()
                    });
                    else return Ok(new Query<InquiryGeneral>
                    {
                        PaginatorLength = inquiryGenerals.Count(),
                        Data = inquiryGenerals.OrderByDescending(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<InquiryGeneral>()
                    });
            }
        }

        public async Task<IHttpActionResult> PostInquiryGeneral(InquiryGeneral inquiryGeneral)
        {
            WorkFlow workFlow = await db.WorkFlows.FindAsync(inquiryGeneral.Owner.DefinitionBranchId);
            string codeYearMonth = workFlow.Code + DateTime.Now.ToString("yyyyMM");
            inquiryGeneral.Owner.No = codeYearMonth + (db.Processes.Where(x => x.DefinitionBranchId == inquiryGeneral.Owner.DefinitionBranchId && x.No.Substring(0, 10) == codeYearMonth).Count() + 1).ToString("0000");

            db.InquiryGenerals.Add(inquiryGeneral);
            await db.SaveChangesAsync();
            return Ok(inquiryGeneral);
        }

        public async Task<IHttpActionResult> GetInquiryGeneral(Guid id)
        {
            InquiryGeneral existedInquiryGeneral = await db.InquiryGenerals.FindAsync(id);
            return Ok(existedInquiryGeneral);
        }

        public async Task<IHttpActionResult> PutInquiryGeneral(Guid? id, InquiryGeneral inquiryGeneral)
        {
            InquiryGeneral existedInquiryGeneral = await db.InquiryGenerals.FindAsync(id);
            db.Entry(existedInquiryGeneral).CurrentValues.SetValues(inquiryGeneral);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}