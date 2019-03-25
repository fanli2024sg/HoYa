using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using HoYa.Entities;
using System.Security.Claims;
using System;

namespace HoYa.Controllers
{

    public class FoldersController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        /// <summary>  
        /// 取得所有【畫廊】
        /// </summary>
        [ResponseType(typeof(Folder))]
        public IQueryable<Folder> GetGalleries()
        {
            return db.Folders;
        }

        /// <summary>  
        /// 透過【畫廊序號】取得【畫廊】
        /// </summary>
        /// <param name="id">畫廊序號</param>  
        [ResponseType(typeof(Folder))]
        public async Task<IHttpActionResult> GetGallery(Guid id)
        {
            Folder gallery = await db.Folders.FindAsync(id);
            if (gallery == null) return NotFound();
            return Ok(gallery);
        }

        /// <summary>  
        /// 更新【畫廊】
        /// </summary>  
        /// <param name="id">畫廊序號</param>  
        /// <param name="gallery">畫廊</param>
        public async Task<IHttpActionResult> PutGallery(Guid id, Folder gallery)
        {
         //   gallery.UpdatedById = (User as ClaimsPrincipal).Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value;
         //   gallery.UpdatedDate = DateTime.Now.AddYears(-1911).ToString("yyyy/MM/dd HH:mm");
            Folder existedGallery = await db.Folders.FindAsync(id);
            db.Entry(existedGallery).CurrentValues.SetValues(gallery);
            await db.SaveChangesAsync();
            await db.Entry(existedGallery).GetDatabaseValuesAsync();
            return Ok(existedGallery);
        }

        /// <summary>  
        /// 新增【畫廊】
        /// </summary>  
        /// <param name="gallery"></param>
        public async Task<IHttpActionResult> PostGallery(Folder gallery)
        {
            //   gallery.CreatedById = (User as ClaimsPrincipal).Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value;
            //   gallery.CreatedDate = DateTime.Now.AddYears(-1911).ToString("yyyy/MM/dd HH:mm");
            gallery.Id = Guid.NewGuid();
            db.Folders.Add(gallery);
            await db.SaveChangesAsync();
            await db.Entry(gallery).GetDatabaseValuesAsync();
            return Ok(gallery);
        }

        /// <summary>  
        /// 刪除【畫廊】
        /// </summary>  
        /// <param name="id">畫廊序號</param>
        public async Task<IHttpActionResult> DeleteGallery(Guid id)
        {
            Folder existedGallery = await db.Folders.FindAsync(id);
            if (existedGallery == null) return NotFound();
            foreach (FolderFile existedGalleryImage in db.FolderFiles.Where(x=>x.OwnerId== id).ToArray())
            {
                db.FolderFiles.Remove(existedGalleryImage);
            }
            db.Folders.Remove(existedGallery);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}