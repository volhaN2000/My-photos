using Microsoft.AspNetCore.Mvc;
using MyPhotos.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
namespace MyPhotos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index() { 
             try {
                var directory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\", User.Identity.Name));
                FileInfo[] files = directory.GetFiles();
                int i = 0;
                foreach (FileInfo file in files)
                { i++; }
                ViewBag.cardnumber = i;


                ViewBag.Path = Path.Combine("\\Files\\", User.Identity.Name);
             }

            catch (Exception ex)
            {
                ViewBag.message = "Error "+ex.Message.ToString();
            }


return View();
        }

        public IActionResult Albums()
        {
            try
            {
                var directory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\", User.Identity.Name));
                FileInfo[] files = directory.GetFiles();
                int i = 0;
                foreach (FileInfo file in files)
                {
                    i++;
                }
                ViewBag.cardnumber = i;
                DirectoryInfo[] directories = directory.GetDirectories();
                new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\", User.Identity.Name));
                string directoriesname = "";
                List <string> people=new List<string>();
                List<int> numbOfCardswthPerson = new List<int>();
                int k = 0;
                foreach (DirectoryInfo directory_ in directories)
               {
                    k++;
                    directoriesname += directory_.Name;
                    people.Add(directory_.Name);
                    FileInfo[] files_ = directory_.GetFiles();
                    int j = 0;
                    foreach (FileInfo file in files_)
                    {
                        j++;
                    }
                    numbOfCardswthPerson.Add(j);
                }
                ViewBag.peoplenumber = k;
                ViewBag.people =people;
                ViewBag.numbOfCardswthPerson = numbOfCardswthPerson;

                ViewBag.Path = Path.Combine("\\Files\\", User.Identity.Name);
            }

            catch (Exception ex)
            {
                ViewBag.message = "Error " + ex.Message.ToString();
            }


            return View();
        }



        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UploadPhoto()
        {
            return View();

        }

        [HttpPost]
        public IActionResult UploadPhoto(IFormFile userfile)
        {
            try { 
            string filename = userfile.FileName;
            filename = Path.GetFileName(filename);
                bool directory_exists = Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\", User.Identity.Name));
                if (!directory_exists) { Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\", User.Identity.Name));
                    filename = "image0.jpg"; }
                else
                {
                    var directory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\", User.Identity.Name));
                    FileInfo[] files = directory.GetFiles();
                    int i = 0;
                    foreach (FileInfo file in files)
                    { i++; }
                    filename = "image" + i + ".jpg";
                }
               

                string uploadfilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\", User.Identity.Name, filename);
              
         var stream = new FileStream(uploadfilepath, FileMode.Create);
            userfile.CopyToAsync(stream);
            ViewBag.message = "File uploaded successfully";
                 ViewBag.Path = Path.Combine("\\Files\\", User.Identity.Name, filename); 
                //string directory_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\", User.Identity.Name);

                //foreach (System.IO.File f in directory_path)
                //    {
                //    ;

                //}

                //ViewBag.Path = String.Format("/Images/profile/{0}", fileName.Replace('+', '_'));
            }

            catch (Exception ex)
            {
                ViewBag.message = "Error "+ex.Message.ToString();
            }
       
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}