using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using X.PagedList;
using X.PagedList.Extensions;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        DbmarketingContext db = new DbmarketingContext();
        public IActionResult Index(int? page)
        {
            int pagesize = 8, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.TblProducts
               .Include(p => p.TblUser)
               .OrderBy(p => p.ProName)
               .ToList();
            IPagedList<TblProduct> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(TblUser avm)
        {
            TblUser ad = db.TblUsers.Where(x => x.UEmail == avm.UEmail && x.UPassword == avm.UPassword).SingleOrDefault();

            if (ad != null)
            {
                HttpContext.Session.SetString("u_id", ad.UId.ToString());
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("error", "Invalid Username or Password");

            }
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(TblUser uvm, IFormFile imgfile)
        {
            var (path, error) = Uploadimg(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded...";
            }
            else
            {
                TblUser u = new TblUser();
                u.UName = uvm.UName;
                u.UEmail = uvm.UEmail;
                u.UPassword = uvm.UPassword;
                u.UImage = path;
                u.UContact = uvm.UContact;
                db.TblUsers.Add(u);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View();

        }

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

        public IActionResult ViewCategory(int? page)
        {
            int pagesize = 8, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.TblCategories.Where(x => x.CatStatus == 1).OrderBy(x => x.CatId).ToList();
            IPagedList<TblCategory> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
        }

        public IActionResult Ads(int? id, int? page)
        {
            int pagesize = 6, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.TblProducts.Where(x => x.ProFkCat == id).OrderBy(x => x.ProId).ToList();
            IPagedList<TblProduct> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
        }

        [HttpPost]
        public IActionResult Ads(int? id, int? page, string search)
        {
            int pagesize = 8, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.TblProducts.Where(x => x.ProName.Contains(search)).OrderByDescending(x => x.ProId).ToList();
            IPagedList<TblProduct> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
        }

        public IActionResult ViewAd(int? id)
        {
            ViewAdModel vam = new ViewAdModel();
            TblProduct p = db.TblProducts.Where(x => x.ProId == id).SingleOrDefault();
            vam.ProId = p.ProId;
            vam.ProName = p.ProName;
            vam.ProPrice = p.ProPrice;
            vam.ProImage = p.ProImage;
            vam.ProDes = p.ProDes;

            TblCategory cat = db.TblCategories.Where(x => x.CatId == p.ProFkCat).SingleOrDefault();
            vam.CatName = cat.CatName;

            TblUser u = db.TblUsers.Where(x => x.UId == p.ProFkUser).SingleOrDefault();
            vam.UName = u.UName;
            vam.UImage = u.UImage;
            vam.UContact = u.UContact;
            vam.UEmail = u.UEmail;
            vam.ProFkUser = u.UId;

            return View(vam);
        }

        public IActionResult DeleteAd(int? id)
        {
            TblProduct p = db.TblProducts.Where(x => x.ProId == id).SingleOrDefault();
            db.TblProducts.Remove(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult CreateAd()
        {
            List<TblCategory> li = db.TblCategories.ToList();
            ViewBag.categoryList = new SelectList(li, "CatId", "CatName");
            return View();
        }

        [HttpPost]
        public IActionResult CreateAd(TblProduct pvm, IFormFile imgfile)
        {
            var (path, error) = Uploadimg(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded...";
            }
            else
            {
                TblProduct p = new TblProduct();
                p.ProName = pvm.ProName;
                p.ProFkCat = pvm.ProFkCat;
                p.ProFkUser = Convert.ToInt32(HttpContext.Session.GetString("u_id"));
                p.ProPrice = pvm.ProPrice;
                p.ProDes = pvm.ProDes;
                p.ProImage = path;
                db.TblProducts.Add(p);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }


    }
}
