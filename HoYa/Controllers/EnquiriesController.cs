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

namespace HoYa.Controllers
{
    public class EnquiriesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        [Route("api/Enquiries/General")]
        [ResponseType(typeof(EnquiryGeneral))]
        public async Task<IHttpActionResult> GetEnquiryGeneral(Guid? processingId = null)
        {
            EnquiryGeneral enquiryGeneral = await db.EnquiryGenerals.FirstOrDefaultAsync(x => x.ProcessId == processingId);
            return Ok(enquiryGeneral);
        }

        [Route("api/Enquiries/General")]
        [ResponseType(typeof(Mission))]
        public async Task<IHttpActionResult> PostEnquiryGeneral(EnquiryGeneral enquiryGeneral)
        {
            WorkFlow workFlow = await db.WorkFlows.FindAsync(enquiryGeneral.Process.DefinitionId);
            string no = workFlow.Code + DateTime.Now.ToString("yyyyMM");
            enquiryGeneral.Process.No = "ENQY" + no + (db.Processes.Where(x => x.DefinitionId == enquiryGeneral.Process.DefinitionId && x.No.Substring(0, 10) == no).Count() + 1).ToString("0000");
            db.EnquiryGenerals.Add(enquiryGeneral);
            await db.SaveChangesAsync();
            return Ok(enquiryGeneral);
        }

        [Route("api/Enquiries/General/{id}")]
        [ResponseType(typeof(Mission))]
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
    }
}