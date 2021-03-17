using ImageSharePassword.Data;
using ImageSharePassword.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;


namespace ImageSharePassword.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString =
       @"Data Source=.\sqlexpress;Initial Catalog=Images;Integrated Security=true;";

        private readonly IWebHostEnvironment _environment;

        public HomeController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile myfile, string password)
        {
            Guid guid = Guid.NewGuid();
            string actualFileName = $"{guid}-{myfile.FileName}";
            string finalFileName = Path.Combine(_environment.WebRootPath, "uploads", actualFileName);
            using var fs = new FileStream(finalFileName, FileMode.CreateNew);
            myfile.CopyTo(fs);

            var db = new ImageDb(_connectionString);
            int imageId = db.AddImage(actualFileName, password);

            UploadViewModel vm = new UploadViewModel();
            vm.Image = db.GetImages().FirstOrDefault(i => i.Id == imageId);
            vm.Link = $"http://localhost:53644/home/ViewImage?id={imageId}";
            return View(vm);

        }

        public IActionResult ViewImage(int id)
        {
            var db = new ImageDb(_connectionString);
            UploadViewModel vm = new UploadViewModel();
            vm.Image = db.GetImages().FirstOrDefault(i => i.Id == id);

            List<int> ids = HttpContext.Session.Get<List<int>>("ApprovedIds");
            if (ids != null && ids.Contains(id))
            {
                return Redirect($"/home/viewpic?id={id}");
            }

            return View(vm);
        }

        public IActionResult ViewPic(int id)
        {
            var db = new ImageDb(_connectionString);
            UploadViewModel vm = new UploadViewModel();
            Image image = db.GetImages().FirstOrDefault(i => i.Id == id);
            vm.Image = image;
            vm.Image.Views++;
            db.UpdateViewCount(vm.Image);

            List<int> ids = HttpContext.Session.Get<List<int>>("ApprovedIds");
            if (ids == null)
            {
                ids = new List<int>();
            }
            if (!ids.Contains(image.Id))
            {
                ids.Add(image.Id);
            }
            HttpContext.Session.Set("ApprovedIds", ids);

            return View(vm);

        }
    }

        public static class SessionExtensions
        {
            public static void Set<T>(this ISession session, string key, T value)
            {
                session.SetString(key, JsonConvert.SerializeObject(value));
            }

            public static T Get<T>(this ISession session, string key)
            {
                string value = session.GetString(key);
                return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
            }
        }

    }
