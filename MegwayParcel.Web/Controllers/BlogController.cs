
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using MegwayParcel.Common.Data;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MegwayParcel.Common.CommonServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MegwayParcel.Web.Controllers
{
    public class BlogController : Controller
    {

        private readonly LogisticERPContext db;

        public BlogController(LogisticERPContext context)
        {
            db = context;
        }
        

        public IActionResult List()
        {
            var customer = HttpContext.Session.GetObjectFromJson<Customer>("LoginObject");

           // var value = Session.GetString(key);
            if (customer == null || customer.Email != "admin@megwayparcels.co.uk")
            {
                return RedirectToAction("Index", "Home");
            }
            ContactCount();
            List<Blog> lst = db.Blogs.ToList();
            return View(lst);
        }
        public IActionResult Create()
        {
            ContactCount();
            return View();
        }

        public IActionResult Save([FromForm] Blog model)
        {

            if (model.Image != null && model.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Blog");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string fileName = Guid.NewGuid().ToString();

                model.ImageUrl = $"/Blog/{fileName}.jpg";
                var filePath = Path.Combine(uploadsFolder, $"{fileName}.jpg");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(stream);
                }
            }


            if (model.BlogId > 0)
            {
                db.Blogs.Update(model);
            }
            else
            {
                model.IsActive = true;
                model.CreatedDate = DateTime.Now;
                db.Blogs.Add(model);
            }

            db.SaveChanges();
            return Ok(model);
        }
        public IActionResult Delete(int id)
        {
            
            Blog del = db.Blogs.Find(id);
            db.Blogs.Remove(del);
            db.SaveChanges();

            return RedirectToAction("List");
        }
        public IActionResult Edit(int id)
        {
            ContactCount();
            Blog edt = db.Blogs.Find(id);
            return View("Create", edt);
        }
        private void ContactCount()
        {
            var contactCount = db.ContactUs.Where(x => x.IsRead == false)?.Count();
            ViewBag.contactCount = contactCount ?? 0;
        }

        public IActionResult Index()
        {
            List<Blog> lst = db.Blogs.Where(x => x.IsActive).ToList();
            return View(lst);
        }
        public IActionResult Details(string id)
        {
            var blog = db.Blogs.FirstOrDefault(x => x.Prefix == id);
            return View(blog);
        }

    }
}
