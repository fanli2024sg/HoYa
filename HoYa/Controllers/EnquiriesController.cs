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
            EnquiryGeneral enquiryGeneral = await db.EnquiryGenerals.FirstOrDefaultAsync(x => x.ProcessingId == processingId);
            return Ok(enquiryGeneral);
        }

        [Route("api/Enquiries/General")]
        [ResponseType(typeof(Mission))]
        public async Task<IHttpActionResult> PostEnquiryGeneral(EnquiryGeneral enquiryGeneral)
        {
            Option type = await db.Options.FindAsync(enquiryGeneral.Processing.Definition.TypeId);
            string no = type.Code + DateTime.Now.ToString("yyyyMM");
            enquiryGeneral.Processing.No = "ENQY" + no + (db.Processings.Where(x => x.TypeId == enquiryGeneral.Processing.TypeId && x.No.Substring(0, 10) == no).Count() + 1).ToString("0000");
            db.EnquiryGenerals.Add(enquiryGeneral);
            await db.SaveChangesAsync();
            return Ok(enquiryGeneral);
        }

        [Route("api/Enquiries/General/{id}")]
        [ResponseType(typeof(Mission))]
        public async Task<IHttpActionResult> PutEnquiryGeneral(Guid? id, EnquiryGeneral enquiryGeneral)
        {
            Process existedProcess = await db.Processings.FindAsync(enquiryGeneral.ProcessingId);
            enquiryGeneral.Processing.UpdatedDate = DateTime.Now;
            db.Entry(existedProcess).CurrentValues.SetValues(enquiryGeneral.Processing);
            await db.SaveChangesAsync();
            Guid? ownerId = null;
            await db.SaveChangesAsync();
            return Ok();
        }

        public async Task<IHttpActionResult> PostEnquiry(Enquiry enquiry)
        {
            enquiry.MaterialProcedure = await db.MaterialProcedures.FindAsync(enquiry.MaterialProcedureId);
            db.Enquiries.Add(enquiry);
            await db.SaveChangesAsync();
            return Ok(enquiry);
        }

        public async Task<IHttpActionResult> PutDocument(Guid id, Document document)
        {
            Document existedDocument = await db.Documents.Where(i => i.Id == id).AsQueryable().FirstOrDefaultAsync();
            db.Entry(existedDocument).CurrentValues.SetValues(document);
            await db.SaveChangesAsync();
            await db.Entry(existedDocument).GetDatabaseValuesAsync();
            return Ok(existedDocument);
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