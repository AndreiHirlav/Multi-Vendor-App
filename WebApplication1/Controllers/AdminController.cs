using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList.Extensions;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        DbmarketingContext db = new DbmarketingContext();

        //Login
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(TblAdmin avm)
        {
            TblAdmin ad = db.TblAdmins.Where(x => x.AdUsername == avm.AdUsername && x.AdPassword == avm.AdPassword).SingleOrDefault();

            if (ad != null)
            {
                HttpContext.Session.SetString("ad_id", ad.AdId.ToString());
                return RedirectToAction("ViewCategory");
            }
            else
            {
                ModelState.AddModelError("error", "Invalid Username or Password");

            }
            return View();
        }

        //Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(TblAdmin avm)
        {
            TblAdmin admin = db.TblAdmins.Where(x => x.AdUsername == avm.AdUsername).SingleOrDefault();

            if (admin != null)
            {
                ModelState.AddModelError("error", "Username already registered.");
            }
            else
            {
                TblAdmin ad = new TblAdmin();
                ad.AdUsername = avm.AdUsername;
                ad.AdPassword = avm.AdPassword;
                db.TblAdmins.Add(ad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult ViewCategory(int? page)
        {
            int pagesize = 8, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.TblCategories.Where(x => x.CatStatus == 1).OrderBy(x => x.CatId).ToList();
            IPagedList<TblCategory> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
        }

        // GET: Create Category
        public IActionResult CreateCategory()
        {
            if (HttpContext.Session.GetString("ad_id") == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(TblCategory cvm, IFormFile imgfile)
        {
            var (path, error) = Uploadimg(imgfile);

            if (path == "-1")
            {
                ViewBag.error = error ?? "Image could not be uploaded...";
                return View();
            }

            TblCategory cat = new TblCategory();
            cat.CatName = cvm.CatName;
            cat.CatImage = path;
            cat.CatStatus = 1;
            cat.CatFkAd = Convert.ToInt32(HttpContext.Session.GetInt32("ad_id"));
            db.TblCategories.Add(cat);
            db.SaveChanges();
            return RedirectToAction("ViewCategory");
        }


        //upload image
        public (string path, string error) Uploadimg(IFormFile file)
        {
            Random r = new Random();
            string path = "-1";
            string error = null;
            int random = r.Next();

            if (file != null && file.Length > 0)
            {
                string extension = Path.GetExtension(file.FileName).ToLower();
                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                {
                    try
                    {
                        var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");

                        if (!Directory.Exists(uploadFolder))
                            Directory.CreateDirectory(uploadFolder);

                        var fileName = random + Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(uploadFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        path = "/Uploads/" + fileName;
                    }
                    catch (Exception)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    error = "Only jpg, jpeg, and png formats are accepted...";
                }
            }
            else
            {
                error = "Please select a file to upload.";
            }

            return (path, error);
        }

        [HttpGet]
        public IActionResult SignOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }


    }
}
