using HoYa.Entities;
using HoYa.Providers;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
namespace HoYa.Controllers
{
    //[Authorize]
    public class FilesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        [ResponseType(typeof(Entities.File))]
        public IQueryable<Entities.File> GetFiles()
        {
            return db.Files;
        }

        /// <summary>  
        /// 取得【檔案】
        /// </summary>  
        /// <param name="id">檔案序號</param> 
        [ResponseType(typeof(Entities.File))]
        public async Task<IHttpActionResult> GetFile(Guid id)
        {
            Entities.File file = await db.Files.FindAsync(id);
            if (file == null) return NotFound();
            return Ok(file);
        }

        /// <summary>  
        /// 更新【檔案】
        /// </summary>  
        /// <param name="id">檔案</param> 
        /// <param name="file">檔案序號</param> 
        [ResponseType(typeof(Entities.File))]
        public async Task<IHttpActionResult> PutFile(Guid id, Entities.File file)
        {
            Entities.File existedFile = await db.Files.FindAsync(id);
            if (existedFile == null) return NotFound();
            db.Entry(existedFile).CurrentValues.SetValues(file);
            await db.SaveChangesAsync();
            await db.Entry(existedFile).GetDatabaseValuesAsync();
            return Ok(existedFile);
        }

        /// <summary>  
        /// 新增【圖片】
        /// </summary>
        [MimeMultipart]
        [ResponseType(typeof(Entities.File))]
        public async Task<IHttpActionResult> PostFile()
        {
            UploadMultipartFormProvider uploadMultipartFormProvider = new UploadMultipartFormProvider(HttpContext.Current.Server.MapPath("~/Temps"));
            await Request.Content.ReadAsMultipartAsync(uploadMultipartFormProvider);
            string value = Path.GetFileName(uploadMultipartFormProvider.FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault());
            string path = uploadMultipartFormProvider.Contents.FirstOrDefault().Headers.ContentDisposition.Name.Replace("\"","");
            path = (HttpContext.Current.Server.MapPath("~/"+path)+ "\\" + value).Replace(HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath, "");
            System.IO.File.Delete(path);
            System.IO.File.Move(HttpContext.Current.Server.MapPath("~/Temps/") + value, path);
            Entities.File file = await db.Files.FirstOrDefaultAsync(x => x.Value == value);


            // Profile profile = await db.Profiles.FirstOrDefaultAsync(x => x.User.UserName == (User as ClaimsPrincipal).Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value);

            if (file == null)
            {
                Entities.File existedFile = new Entities.File
                {
                    Path = path,
                    Value = value
                };
                db.Files.Add(existedFile);
                await db.SaveChangesAsync();
                await db.Entry(existedFile).GetDatabaseValuesAsync();
                return Ok(existedFile);
            }
            else
            {
                Entities.File updatedFile = await db.Files.FirstOrDefaultAsync(x => x.Value == value);
                file.Path = path;
                file.Value = value;
                file.CreatedDate = DateTime.Now;
                db.Entry(updatedFile).CurrentValues.SetValues(file);
                await db.SaveChangesAsync();
                await db.Entry(updatedFile).GetDatabaseValuesAsync();
                return Ok(updatedFile);
            }
        }

        /// <summary>  
        /// 刪除【檔案】
        /// </summary>  
        /// <param name="id">檔案序號</param> 
        public async Task<IHttpActionResult> DeleteFile(Guid id)
        {
            Entities.File existedFile = await db.Files.FindAsync(id);
            if (existedFile == null) return NotFound();
            else
            {
                foreach (FolderFile existedFolderFile in db.FolderFiles.Where(x => x.TargetId == existedFile.Id).ToArray()) db.FolderFiles.Remove(existedFolderFile);
                db.Files.Remove(existedFile);
            }
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}