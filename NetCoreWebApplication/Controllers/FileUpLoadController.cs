using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NetCoreWebApplication.Models;

namespace NetCoreWebApplication.Controllers
{
    public class FileUpLoadController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public FileUpLoadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UploadFiles()
        {
            //this.HttpContext.Session.Set("progress",)
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string shortTime = DateTime.Now.ToString("yyyy/MM/dd") + "/";
            //  string filePhysicalPath = MapPath("~/Content" + shortTime);  //文件路径  可以通过注入 IHostingEnvironment 服务对象来取得Web根目录和内容根目录的物理路径

            string filePhysicalPath = $"{_hostingEnvironment.WebRootPath}/upload/{DateTime.Now.ToString("yyyy-MM-dd")}";
            if (!Directory.Exists(filePhysicalPath)) //判断上传文件夹是否存在，若不存在，则创建
            {
                Directory.CreateDirectory(filePhysicalPath); //创建文件夹
            }
            foreach (var file in files)
            {
                if (file.Length > 0)
                {

                    using (var stream = new FileStream(Path.Combine(filePhysicalPath, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            return Ok(new { count = files.Count, size });
        }


        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile()
        {

            var files = Request.Form.Files;
            //long size = files.Sum(f => f.Length);

            //  string filePhysicalPath = MapPath("~/Content" + shortTime);  //文件路径  可以通过注入 IHostingEnvironment 服务对象来取得Web根目录和内容根目录的物理路径

            string filePhysicalPath = $"upload/{DateTime.Now.ToString("yyyy-MM-dd")}";
            if (!Directory.Exists(filePhysicalPath)) //判断上传文件夹是否存在，若不存在，则创建
            {
                Directory.CreateDirectory(filePhysicalPath); //创建文件夹
            }


            var file = files[0];
            //this.HttpContext.Session.Remove("progress");
            //byte[] buffer = new byte[1024];
            //Stream stream = file.OpenReadStream();
            //long totalReadLength = 0;
            //using (var fileStream = new FileStream(Path.Combine(filePhysicalPath, file.FileName), FileMode.Create))
            //{
            //    int readLength = 0;
            //    while ((readLength = stream.Read(buffer, 0, buffer.Length)) != 0)
            //    {
            //        totalReadLength += readLength;
            //        var progress = totalReadLength*100 / file.Length;
            //        this.HttpContext.Session.Set("progress", BitConverter.GetBytes(progress));
            //        fileStream.Write(buffer, 0, readLength);
            //    }
            //}

            //int progressSession = 0;
            //if (this.HttpContext.Session.TryGetValue("progress", out byte[] progressBytes))
            //{
            //    progressSession = BitConverter.ToInt32(progressBytes, 0);
            //}
            //stream.Close();
            if (file.Length > 0)//此处可添加对大小进行判断
            {

                using (var stream = new FileStream(Path.Combine(filePhysicalPath, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

            }
            return Json(new MessageResult<int>() { Success = true, Message = "上传成功" });
        }

        [HttpPost("UploadFileWithProgress")]
        public async Task<IActionResult> UploadFileWithProgress()
        {

            var files = Request.Form.Files;
            //long size = files.Sum(f => f.Length);

            //  string filePhysicalPath = MapPath("~/Content" + shortTime);  //文件路径  可以通过注入 IHostingEnvironment 服务对象来取得Web根目录和内容根目录的物理路径

            string filePhysicalPath = $"upload/{DateTime.Now.ToString("yyyy-MM-dd")}";
            if (!Directory.Exists(filePhysicalPath)) //判断上传文件夹是否存在，若不存在，则创建
            {
                Directory.CreateDirectory(filePhysicalPath); //创建文件夹
            }


            var file = files[0];
            this.HttpContext.Session.Remove("progress");
            byte[] buffer = new byte[1024];
            Stream stream = file.OpenReadStream();
            long totalReadLength = 0;
            using (var fileStream = new FileStream(Path.Combine(filePhysicalPath, file.FileName), FileMode.Create))
            {
                int readLength = 0;
                while ((readLength = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    totalReadLength += readLength;
                    var progress = totalReadLength * 100 / file.Length;
                    this.HttpContext.Session.Set("progress", BitConverter.GetBytes(progress));
                    fileStream.Write(buffer, 0, readLength);
                }
            }

            int progressSession = 0;
            if (this.HttpContext.Session.TryGetValue("progress", out byte[] progressBytes))
            {
                progressSession = BitConverter.ToInt32(progressBytes, 0);
            }
            stream.Close();
            //if (file.Length > 0)
            //{

            //    using (var stream = new FileStream(Path.Combine(filePhysicalPath, file.FileName), FileMode.Create))
            //    {
            //        await file.CopyToAsync(stream);
            //    }

            //}
            return Json(new MessageResult<int>() { Success = true, Message = "上传成功" });
        }


        [HttpGet("UploadStatus")]
        public IActionResult UploadStatus()
        {
            int progress = 0;
            if (this.HttpContext.Session.TryGetValue("progress", out byte[] progressBytes))
            {
                progress = BitConverter.ToInt32(progressBytes, 0);
            }
            return Json(progress);
        }
    }
}