using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using HoYa.Entities;
using System.Security.Claims;
using System;
using System.Data.Entity;

namespace HoYa.Controllers
{

    public class FolderFilesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        /// <summary>  
        /// 取得所有【畫廊】
        /// </summary>
        [ResponseType(typeof(FolderFile))]
        public IQueryable<FolderFile> GetFolderFiles()
        {
            return db.FolderFiles;
        }

        /// <summary>  
        /// 透過【畫廊序號】取得【畫廊】
        /// </summary>
        /// <param name="id">畫廊序號</param>  
        [ResponseType(typeof(FolderFile))]
        public async Task<IHttpActionResult> GetFolderFile(Guid id)
        {
            FolderFile folderFile = await db.FolderFiles.FindAsync(id);
            if (folderFile == null) return NotFound();
            return Ok(folderFile);
        }

        /// <summary>  
        /// 更新【畫廊】
        /// </summary>  
        /// <param name="id">畫廊序號</param>  
        /// <param name="folderFile">畫廊</param>
        public async Task<IHttpActionResult> PutFolderFile(Guid id, FolderFile folderFile)
        {
            FolderFile existedFolderFile = await db.FolderFiles.FindAsync(id);
            db.Entry(existedFolderFile).CurrentValues.SetValues(folderFile);
            await db.SaveChangesAsync();
            await db.Entry(existedFolderFile).GetDatabaseValuesAsync();
            return Ok(existedFolderFile);
        }

        /// <summary>  
        /// 新增【畫廊】
        /// </summary>  
        /// <param name="folderFile"></param>
        public async Task<IHttpActionResult> PostFolderFile(FolderFile folderFile)
        {
            folderFile.Id = Guid.NewGuid();
            FolderFile existedFolderFile = await db.FolderFiles.FirstOrDefaultAsync(x => x.OwnerId == folderFile.OwnerId && x.TargetId == folderFile.TargetId);
            if (existedFolderFile != null) db.FolderFiles.Remove(existedFolderFile);
            folderFile.Id = Guid.NewGuid();
            folderFile.Owner = await db.Folders.FindAsync(folderFile.OwnerId);
            folderFile.Target = await db.Files.FindAsync(folderFile.TargetId);
            db.FolderFiles.Add(folderFile); await db.SaveChangesAsync();
            await db.Entry(folderFile).GetDatabaseValuesAsync();
            return Ok(folderFile);
        }

        /// <summary>  
        /// 刪除【畫廊】
        /// </summary>  
        /// <param name="id">畫廊序號</param>
        public async Task<IHttpActionResult> DeleteFolderFile(Guid id)
        {
            FolderFile existedFolderFile = await db.FolderFiles.FindAsync(id);
            if (existedFolderFile == null) return NotFound();
            db.FolderFiles.Remove(existedFolderFile);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}