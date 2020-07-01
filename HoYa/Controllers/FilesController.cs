using HoYa.Entities;
using HoYa.Models;
using HoYa.Providers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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

        [MimeMultipart]
        [ResponseType(typeof(Entities.File))]
        public async Task<IHttpActionResult> PostFile()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var accountName = ConfigurationManager.AppSettings["storage:account:name"];
            var accountKey = ConfigurationManager.AppSettings["storage:account:key"];
            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer temps = blobClient.GetContainerReference("temps");
            CloudBlobContainer files = blobClient.GetContainerReference("files");

            var provider = new AzureStorageMultipartFormDataStreamProvider(temps);
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                string path = provider.Contents.FirstOrDefault().Headers.ContentDisposition.Name.Replace("\"", "");            

                ICollection<FolderFile> folderFiles = new List<FolderFile>();
                foreach (var fileData in provider.FileData)
                {                   
                    CloudBlockBlob srcBlob = temps.GetBlockBlobReference(fileData.LocalFileName);       
                    CloudBlockBlob destBlob = files.GetBlockBlobReference(path + "/" + fileData.LocalFileName);
                    await destBlob.StartCopyAsync(srcBlob);
                    srcBlob.Delete();             
                    FolderFile newFolderFile = new FolderFile
                    {
                        StartDate = DateTime.Now,
                        FolderId = null,
                        Target = new Entities.File
                        {
                            Id = new Guid(fileData.LocalFileName),
                            Path = path+"/"+ fileData.LocalFileName,
                            Value = fileData.Headers.ContentDisposition.FileName.Replace("\"", "")
                        }
                    };
                    db.FolderFiles.Add(newFolderFile);
                    await db.SaveChangesAsync();
                    folderFiles.Add(newFolderFile);
                }
                return Ok(new FileSave
                { 
                    Photos = folderFiles
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"An error has occured. Details: {ex.Message}");
            }
        }
        /*
        /// <summary>  
        /// 新增【圖片】
        /// </summary>
        [MimeMultipart]
        [ResponseType(typeof(Entities.File))]
        public async Task<IHttpActionResult> PostFile2()
        {
            DirectoryInfo directoryInfo;
            var tempsAzurePath = "D:/home/site/wwwroot/Temps";
            if (!Directory.Exists(tempsAzurePath))
            {
                directoryInfo = Directory.CreateDirectory(tempsAzurePath);

            }
            UploadMultipartFormProvider uploadMultipartFormProvider = new UploadMultipartFormProvider(tempsAzurePath);//HttpContext.Current.Server.MapPath("~/Temps")
            await Request.Content.ReadAsMultipartAsync(uploadMultipartFormProvider);
            string path = uploadMultipartFormProvider.Contents.FirstOrDefault().Headers.ContentDisposition.Name.Replace("\"", "");

            var filesAzurePath = "D:/home/site/wwwroot/Files";
            DirectoryInfo directoryInfo2;
            if (!Directory.Exists(filesAzurePath))
            {
                directoryInfo2 = Directory.CreateDirectory(filesAzurePath);

            }
            string folderPath = "D:/home/site/wwwroot/Files/" + path; //HttpContext.Current.Server.MapPath("~/Files/" + path)
            Guid folderId = new Guid(path.Substring(path.Length - 36, 36));
            Folder folder = await db.Folders.FindAsync(folderId);
            DirectoryInfo directoryInfo3;
            if (!Directory.Exists(folderPath))
            {
                directoryInfo3 = Directory.CreateDirectory(folderPath);

            }
            if (folder == null)
            {
                folder = new Folder
                {
                    Id = folderId
                };
                db.Folders.Add(folder);
                await db.SaveChangesAsync();
            }

            ICollection<FolderFile> folderFiles = new List<FolderFile>();
            foreach (var fileData in uploadMultipartFormProvider.FileData)
            {

                string fileName = Path.GetFileName(fileData.LocalFileName);
                Guid fileId = new Guid(fileName.Substring(0, 36));
                string targetPath = folderPath + "/" + fileName;

                // (HttpContext.Current.Server.MapPath("~/Files/" + path) + "\\" + fileName).Replace(HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath, "");


                if (System.IO.File.Exists(targetPath))
                {
                    string duplicateId = fileId.ToString();
                    fileId = Guid.NewGuid();
                    targetPath = targetPath.Replace(duplicateId, fileId.ToString());
                    fileName = fileName.Replace(duplicateId, fileId.ToString());
                }

                //System.IO.File.Move(HttpContext.Current.Server.MapPath("~/Temps/") + fileName, targetPath);
                System.IO.File.Move("D:/home/site/wwwroot/Temps/" + fileName, targetPath);
                Entities.File file = await db.Files.FirstOrDefaultAsync(x => x.Value == fileName);
                if (file == null)
                {
                    FolderFile newFolderFile = new FolderFile
                    {
                        StartDate = DateTime.Now,
                        OwnerId = folder.Id,
                        Target = new Entities.File
                        {
                            Id = fileId,
                            Path = path,
                            Value = fileName
                        }
                    };
                    db.FolderFiles.Add(newFolderFile);
                    await db.SaveChangesAsync();

                    folderFiles.Add(newFolderFile);
                }
            }
            return Ok(new FileSave
            {
                Folder = folder,
                Photos = folderFiles
            });
        }
        */
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