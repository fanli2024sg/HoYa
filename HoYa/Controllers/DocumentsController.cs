using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System.Data.Entity;
using System;

namespace HoYa.Controllers
{
    public class DocumentsController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public IQueryable<Document> GetDocuments()
        {
            return db.Documents;
        }

        [ResponseType(typeof(Document))]
        public async Task<IHttpActionResult> GetDocument(Guid id)
        {
            Document document = await db.Documents.FindAsync(id);
            if (document == null) return NotFound();
            return Ok(document);
        }

        [Route("api/Documents/ByLikeValue")]
        [ResponseType(typeof(Document))]
        public IQueryable<Document> GetDocumentByLikeValue(string likeValue)
        {
            if (likeValue == null) likeValue = "";

            return db.Documents.Where(x => x.SurName.Contains(likeValue) || x.GivenName.Contains(likeValue)).OrderBy(x => x.No);
        }

        internal class Document_h3
        {
            public IQueryable<Document> Document { get; set; }
        }
        [Route("api/Documents/By")]
        public IQueryable<Document> GetDocumentsBy(string nameLike = "", string englishNameLike = "",
             int? pageIndex = 1,
            int? pageSize = 10)
        {
            return db.Documents.Where(x =>
            (nameLike == null || nameLike == "" || (x.SurName + x.GivenName).Contains(nameLike)) &&
            (englishNameLike == null || englishNameLike == "" || (x.EnglishGivenName + " " + x.EnglishSurName).Contains(englishNameLike))
            ).OrderBy(x => x.No).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault());
        }

        public async Task<IHttpActionResult> PutDocument(Guid id, Document document)
        {
            Document existedDocument = await db.Documents.Where(i => i.Id == id).AsQueryable().FirstOrDefaultAsync();
            db.Entry(existedDocument).CurrentValues.SetValues(document);
            await db.SaveChangesAsync();
            await db.Entry(existedDocument).GetDatabaseValuesAsync();
            return Ok(existedDocument);
        }

        [ResponseType(typeof(int))]
        public async Task<IHttpActionResult> PostDocument(Document document)
        {
            document.Id = Guid.NewGuid();
            db.Documents.Add(document);
            await db.SaveChangesAsync();
            await db.Entry(document).GetDatabaseValuesAsync();
            return Ok(document);
        }

        public async Task<IHttpActionResult> DeleteDocument(Guid id)
        {
            Document document = await db.Documents.FindAsync(id);
            if (document == null) return NotFound();
            db.Documents.Remove(document);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}