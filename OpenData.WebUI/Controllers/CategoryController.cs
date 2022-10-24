using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpenData.Domain.Abstract;
using OpenData.Domain.Entities;

namespace OpenData.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private ICRepository repository;
        public int PageSize = 20;
       
        
        public CategoryController(ICRepository repo)
        {
            this.repository = repo;
        }

        public ActionResult Index()
        {
            return View(repository.Category);
        }

        public ActionResult Edit(int id)
        {
            return View(repository.Category.FirstOrDefault(c=>c.ID==id));
        }

        [HttpPost]
        public ActionResult Edit(cat_Category category, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    category.ImageMimeType = image.ContentType;
                    category.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(category.ImageData, 0, image.ContentLength);
                }

                repository.SaveCategory(category);
                TempData["message"] = string.Format("{0} has been saved", category.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(category);
            }
        }
    }
}
