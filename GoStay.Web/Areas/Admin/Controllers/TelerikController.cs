using GoStay.Common;
using GoStay.Common.Configuration;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.News;
using GoStay.DataDto.Telerik;
using GoStay.Service;
using GoStay.Service.Api.News;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using ServiceStack;
using System.Drawing;
using System.Net.Http.Headers;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace GoStay.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public partial class EditorController : Controller
    {
        private readonly INewsApiServices _newsApiServices;
        private readonly INewsServices _newsServices;

        public EditorController(INewsApiServices newsApiServices, INewsServices newsServices)
        {
            _newsApiServices = newsApiServices;
            _newsServices = newsServices;
        }
        public IActionResult Index(int Id, string Obj, string Content)
        {
            ModelContent model = new ModelContent() { Id = Id, Obj = Obj, Content = Content };
            return View(model);
        }
        [HttpPost]
        public string TransitData(int Id, string Obj, string Content)
        {
            try
            {
                var result = new ResponseBase<int>();
                if (Obj == "news")
                {
                    result = _newsApiServices.EditContentNews(new EditNewsContentParam { Content = Content, NewsId = Id });
                }

                return result.Message;
            }
            catch
            {
                return "Exception";
            }
        }
    }
    [Area("Admin")]
    public class ImageBrowserController : EditorImageBrowserController
    {
        private const string contentFolderRoot = "wwwroot/";
        private const string folderName = "news/";
        private static readonly string[] foldersToCopy = new[] { "shared/images/employees" };
        private readonly IMyTypedClientServices _client;
        private readonly IPicturesServices _pictureServices;

        /// <summary>
        /// Gets the base paths from which content will be served.
        /// </summary>
        public override string ContentPath
        {
            get
            {
                return CreateUserFolder();
            }
        }

        public ImageBrowserController(IHostingEnvironment hostingEnvironment, IMyTypedClientServices client, IPicturesServices pictureServices)
            : base(hostingEnvironment)
        {
            _client = client;
            _pictureServices = pictureServices;
        }

        private string CreateUserFolder()
        {
            var virtualPath = Path.Combine("uploads", folderName);
            //var orig = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = AppConfigs.NewsImagePath;
            //var path = "shared/userfile/images";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                foreach (var sourceFolder in foldersToCopy)
                {
                    CopyFolder(HostingEnvironment.WebRootFileProvider.GetFileInfo(sourceFolder).PhysicalPath, path);
                }
            }
            return virtualPath;
        }

        private void CopyFolder(string source, string destination)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            foreach (var file in Directory.EnumerateFiles(source))
            {
                var dest = Path.Combine(destination, Path.GetFileName(file));
                System.IO.File.Copy(file, dest);
            }

            foreach (var folder in Directory.EnumerateDirectories(source))
            {
                var dest = Path.Combine(destination, Path.GetFileName(folder));
                CopyFolder(folder, dest);
            }
        }
        [AcceptVerbs("POST")]
        public IActionResult UploadImg(IFormFile file, int Id, string Obj)
        {
            List<IFormFile> files = new List<IFormFile>() { file };
            var temp = _client.PostImgAndGetData(files, 1024, Id.ToString(), 3);

            string[] charsToRemove = new string[] { "@", "[", "]", "'" };
            foreach (var c in charsToRemove)
            {
                temp.data = temp.data.Replace(c, string.Empty);
                temp.size = temp.size.Replace(c, string.Empty);
            }

            var url = temp.data.Split(",");
            var size = temp.size.Split(",");
            ResponseBase resultul = new ResponseBase();

            for (int i = 0; i < url.Length; i++)
            {

                Picture picture = new Picture();

                picture.Type = 3;
                picture.Name = DateTime.UtcNow.ToString() + $"00{i}";
                var x = Path.GetFileNameWithoutExtension(url[i]) + Path.GetExtension(url[i]).Replace("\"", "");


                picture.Url = $"/uploads/1/" + Obj + $"/{DateTime.Now.Year}/" + Id.ToString() + "/" + x;

                picture.IdType = Id;
                picture.NewsId = Id;

                picture.Size = int.Parse(size[i]);
                resultul.Message += _pictureServices.AddNewPicture(picture);
            }
            var result = new FileBrowserEntry
            {
                Size = file.Length,
                Name = GetFileName(file)
            };

            return Json(result + resultul.Message);
        }
    }
    [Area("Admin")]
    public abstract class EditorImageBrowserController : BaseFileBrowserController
    {

        protected EditorImageBrowserController(IHostingEnvironment hostingEnvironment)
            : this(DI.Current.Resolve<IDirectoryBrowser>(),
                  DI.Current.Resolve<IDirectoryPermission>(),
                  hostingEnvironment)
        {
        }

        protected EditorImageBrowserController(
            IDirectoryBrowser directoryBrowser,
            IDirectoryPermission permission,
            IHostingEnvironment hostingEnvironment)
             : base(directoryBrowser, permission, hostingEnvironment)
        {
        }

        /// <summary>
        /// Gets the valid file extensions by which served files will be filtered.
        /// </summary>
        public override string Filter
        {
            get
            {
                return EditorImageBrowserSettings.DefaultFileTypes;
            }
        }

        public virtual bool AuthorizeThumbnail(string path)
        {
            return CanAccess(path);
        }

        /// <summary>
        /// You can use a third-party library to create thumbnails as System.Drawing is not curretnly part of ASP.NET Core https://blogs.msdn.microsoft.com/dotnet/2016/02/10/porting-to-net-core/
        /// </summary>
        public virtual IActionResult Thumbnail(string path, int Id, string Obj)
        {
            //// This will get the current WORKING directory (i.e. \bin\Debug)
            //string workingDirectory = Environment.CurrentDirectory;
            //// or: Directory.GetCurrentDirectory() gives the same result

            //// This will get the current PROJECT bin directory (ie ../bin/)
            //string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

            //string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            ////This will strip just the working path name:
            ////C:\Program Files\MyApplication
            //string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);

            //// This will get the current PROJECT directory
            //string projectDirectorys = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string fullpath = $@"{AppConfigs.FullPath}1\{Obj}\{DateTime.Now.Year}\{Id}\{path}";
            try
            {
                Image image = Image.FromFile(fullpath);
                var thumb = image.GetThumbnailImage(80, 80, () => false, IntPtr.Zero);
                ImageConverter imgCon = new ImageConverter();
                var arr = (byte[])imgCon.ConvertTo(thumb, typeof(byte[]));
                Stream stream = new MemoryStream(arr);
                var sbase64 = Convert.ToBase64String(arr);
                return File(arr, "image/png");
            }
            catch (Exception e)
            {
                return null;
            }

        }


    }
    public class Folder
    {
        public Folder()
        {
            this.ChildFolders = new HashSet<Folder>();
            this.Images = new HashSet<Image>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string Path { get; set; }

        public virtual ICollection<Folder> ChildFolders { get; set; }
        public virtual Folder Parent { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }

    [Area("Admin")]
    public abstract class BaseFileBrowserController : Controller
    {
        private readonly IDirectoryBrowser directoryBrowser;
        private readonly IDirectoryPermission permission;

        protected readonly IHostingEnvironment HostingEnvironment;

        protected BaseFileBrowserController(IHostingEnvironment hostingEnvironment)
            : this(DI.Current.Resolve<IDirectoryBrowser>(),
                  DI.Current.Resolve<IDirectoryPermission>(),
                  hostingEnvironment)
        {
        }

        protected BaseFileBrowserController(
            IDirectoryBrowser directoryBrowser,
            IDirectoryPermission permission,
            IHostingEnvironment hostingEnvironment)
        {
            this.directoryBrowser = directoryBrowser;
            this.directoryBrowser.HostingEnvironment = hostingEnvironment;
            this.permission = permission;
            this.HostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Gets the base path from which content will be served.
        /// </summary>
        public abstract string ContentPath
        {
            get;
        }

        /// <summary>
        /// Gets the valid file extensions by which served files will be filtered.
        /// </summary>
        public virtual string Filter
        {
            get
            {
                return "*.*";
            }
        }

        /// <summary>
        /// Creates a folder with a given entry.
        /// </summary>
        /// <param name="path">The path to the parent folder in which the folder should be created.</param>
        /// <param name="entry">The entry.</param>
        /// <returns>An empty <see cref="ContentResult"/>.</returns>
        /// <exception cref="Exception">Forbidden</exception>
        [AcceptVerbs("POST")]
        public virtual ActionResult Create(string path, FileBrowserEntry entry)
        {
            var fullPath = NormalizePath(path);
            var name = entry.Name;

            if (name.HasValue() && AuthorizeCreateDirectory(fullPath, name))
            {
                var physicalPath = Path.Combine(fullPath, name);

                if (!Directory.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }

                return Json(entry);
            }

            throw new Exception("Forbidden");
        }

        /// <summary>
        /// Determines if a folder can be created.
        /// </summary>
        /// <param name="path">The path to the parent folder in which the folder should be created.</param>
        /// <param name="name">Name of the folder.</param>
        /// <returns>true if folder can be created, otherwise false.</returns>
        public virtual bool AuthorizeCreateDirectory(string path, string name)
        {
            return CanAccess(path);
        }

        public virtual JsonResult Read(string path, int Id, string Obj)
        {
            //var fullPath = NormalizePath(path);
            try
            {
                var fullPath = AppConfigs.FullPath + $"1\\{Obj}\\{DateTime.Now.Year}\\{Id}\\";
                var files = directoryBrowser.GetFiles(fullPath, Filter);
                var directories = directoryBrowser.GetDirectories(fullPath);
                var result = files.Concat(directories);

                return Json(result.ToArray());
            }
            catch
            {
                return Json("Exception");
            }
            //if (AuthorizeRead(fullPath))
            //{
            //    try
            //    {
            //        var files = directoryBrowser.GetFiles(fullPath, Filter);
            //        var directories = directoryBrowser.GetDirectories(fullPath);
            //        var result = files.Concat(directories);

            //        return Json(result.ToArray());
            //    }
            //    catch (DirectoryNotFoundException)
            //    {
            //        throw new Exception("File Not Found");
            //    }
            //}

            //throw new Exception("Forbidden");

        }

        /// <summary>
        /// Determines if content of a given path can be browsed.
        /// </summary>
        /// <param name="path">The path which will be browsed.</param>
        /// <returns>true if browsing is allowed, otherwise false.</returns>
        public virtual bool AuthorizeRead(string path)
        {
            return CanAccess(path);
        }

        protected virtual bool CanAccess(string path)
        {
            var rootPath = Path.GetFullPath(Path.Combine(this.HostingEnvironment.WebRootPath, ContentPath));

            return permission.CanAccess(rootPath, path);
        }

        protected string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return Path.GetFullPath(Path.Combine(this.HostingEnvironment.WebRootPath, ContentPath));
            }
            else
            {
                return Path.GetFullPath(Path.Combine(this.HostingEnvironment.WebRootPath, ContentPath, path));
            }
        }

        /// <summary>
        /// Deletes a entry.
        /// </summary>
        /// <param name="path">The path to the entry.</param>
        /// <param name="entry">The entry.</param>
        /// <returns>An empty <see cref="ContentResult"/>.</returns>
        /// <exception cref="Exception">File Not Found</exception>
        [AcceptVerbs("POST")]
        public virtual ActionResult Destroy(string path, FileBrowserEntry entry)
        {
            var fullPath = NormalizePath(path);

            if (entry != null)
            {
                fullPath = Path.Combine(fullPath, entry.Name);

                if (entry.EntryType == FileBrowserEntryType.File)
                {
                    DeleteFile(fullPath);
                }
                else
                {
                    DeleteDirectory(fullPath);
                }

                return Json(new object[0]);
            }

            throw new Exception("File Not Found");
        }

        /// <summary>
        /// Determines if a file can be deleted.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>true if file can be deleted, otherwise false.</returns>
        public virtual bool AuthorizeDeleteFile(string path)
        {
            return CanAccess(path);
        }

        /// <summary>
        /// Determines if a folder can be deleted.
        /// </summary>
        /// <param name="path">The path to the folder.</param>
        /// <returns>true if folder can be deleted, otherwise false.</returns>
        public virtual bool AuthorizeDeleteDirectory(string path)
        {
            return CanAccess(path);
        }

        protected virtual void DeleteFile(string path)
        {
            if (!AuthorizeDeleteFile(path))
            {
                throw new Exception("Forbidden");
            }

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        protected virtual void DeleteDirectory(string path)
        {
            if (!AuthorizeDeleteDirectory(path))
            {
                throw new Exception("Forbidden");
            }

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }


        /// <summary>
        /// Uploads a file to a given path.
        /// </summary>
        /// <param name="path">The path to which the file should be uploaded.</param>
        /// <param name="file">The file which should be uploaded.</param>
        /// <returns>A <see cref="JsonResult"/> containing the uploaded file's size and name.</returns>
        /// <exception cref="Exception">Forbidden</exception>
        [AcceptVerbs("POST")]
        public virtual ActionResult Upload(string path, IFormFile file, int Id, int Obj)
        {
            var fullPath = NormalizePath(path);

            if (AuthorizeUpload(fullPath, file))
            {
                SaveFile(file, fullPath);

                var result = new FileBrowserEntry
                {
                    Size = file.Length,
                    Name = GetFileName(file)
                };

                return Json(result);
            }

            throw new Exception("Forbidden");
        }

        protected virtual void SaveFile(IFormFile file, string pathToSave)
        {
            try
            {
                var path = Path.Combine(pathToSave, GetFileName(file));
                using (var stream = System.IO.File.Create(path))
                {
                    file.CopyTo(stream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Determines if a file can be uploaded to a given path.
        /// </summary>
        /// <param name="path">The path to which the file should be uploaded.</param>
        /// <param name="file">The file which should be uploaded.</param>
        /// <returns>true if the upload is allowed, otherwise false.</returns>
        public virtual bool AuthorizeUpload(string path, IFormFile file)
        {
            return CanAccess(path) && IsValidFile(GetFileName(file));
        }

        private bool IsValidFile(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var allowedExtensions = Filter.Split(',');

            return allowedExtensions.Any(e => e.Equals("*.*") || e.EndsWith(extension, StringComparison.OrdinalIgnoreCase));
        }

        public virtual string GetFileName(IFormFile file)
        {
            var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            return Path.GetFileName(fileContent.FileName.ToString().Trim('"'));
        }
    }
    public class FileBrowserController : EditorFileBrowserController
    {
        private const string contentFolderRoot = "wwwroot/";
        private const string folderName = "news/";
        private static readonly string[] foldersToCopy = new[] { "shared/images/employees" };

        /// <summary>
        /// Gets the base paths from which content will be served.
        /// </summary>
        public override string ContentPath
        {
            get
            {
                return CreateUserFolder();
            }
        }

        /// <summary>
        /// Gets the valid file extensions by which served files will be filtered.
        /// </summary>
        public override string Filter
        {
            get
            {
                // return "*.txt, *.doc, *.docx, *.xls, *.xlsx, *.ppt, *.pptx, *.zip, *.rar, *.jpg, *.jpeg, *.gif, *.png";
                return "*.jpg, *.jpeg, *.gif, *.png,*.mp4,*.mov";
            }
        }

        public FileBrowserController(IHostingEnvironment hostingEnvironment)
            : base(hostingEnvironment)
        {
        }
        private string CreateUserFolder()
        {
            var virtualPath = Path.Combine("uploads", folderName);
            //var orig = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = AppConfigs.NewsImagePath;
            //var path = "shared/userfile/images";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                foreach (var sourceFolder in foldersToCopy)
                {
                    CopyFolder(HostingEnvironment.WebRootFileProvider.GetFileInfo(sourceFolder).PhysicalPath, path);
                }
            }
            return virtualPath;
        }

        private void CopyFolder(string source, string destination)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            foreach (var file in Directory.EnumerateFiles(source))
            {
                var dest = Path.Combine(destination, Path.GetFileName(file));
                System.IO.File.Copy(file, dest);
            }

            foreach (var folder in Directory.EnumerateDirectories(source))
            {
                var dest = Path.Combine(destination, Path.GetFileName(folder));
                CopyFolder(folder, dest);
            }
        }
    }
}

