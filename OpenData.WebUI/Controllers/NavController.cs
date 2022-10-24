using OpenData.Domain.Abstract;
using OpenData.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpenData.WebUI.Models;

namespace OpenData.WebUI.Controllers
{
    public class NavController : Controller
    {
        //
        // GET: /Nav/

        private IODRepository od_repository;
        private IARepository a_repository;
        private ICRepository c_repository;

        public NavController(IODRepository od_repo, ICRepository c_repo, IARepository a_repo)
        {
            od_repository = od_repo;
            c_repository = c_repo;
            a_repository = a_repo;
        }

        //public PartialViewResult MenuCat(int[] categoryIds)
        //{
        //    ViewBag.SelectedCategory = categoryIds;
        //    IEnumerable<cat_Category> categories = c_repository.Category.OrderBy(x => x.Name);
        //    return PartialView(categories);
        //}
                
        //public PartialViewResult MenuCat(int categoryId=0)
        //{
        //    ViewBag.SelectedCategory = categoryId;
        //    IEnumerable<cat_Category> categories = c_repository.Category.OrderBy(x => x.Name);

        //    //IEnumerable<string> categories = c_repository.Category.Select(x => x.Name).Distinct().OrderBy(x => x);
        //    //IEnumerable<string> categories = od_repository.OpenData.Select(x => x.Category.Name).Distinct().OrderBy(x => x);
        //    return PartialView(categories) ;
        //}

        public PartialViewResult MenuCat(/*List<int> categories = null*/)
        {
            
            //ViewBag.SelectedCategory = categoryId;
            IEnumerable<cat_Category> model = c_repository.Category.OrderBy(x => x.Name);
            //
            List<int> categories=TempData["MenuCat"] as List<int>;
            if (categories == null)
            {
                categories = (from cat in c_repository.Category.OrderBy(c=>c.Name)
                             select cat.ID).ToList();

            }

        
            TempData["MenuCat"] = categories;




            //IEnumerable<string> categories = c_repository.Category.Select(x => x.Name).Distinct().OrderBy(x => x);
            //IEnumerable<string> categories = od_repository.OpenData.Select(x => x.Category.Name).Distinct().OrderBy(x => x);
            return PartialView(model);
        }

        public PartialViewResult MenuAuth()
        {

            IEnumerable<Authority> model = a_repository.Authorities.OrderBy(x => x.Name);
            List<int> authorities = TempData["MenuAuth"] as List<int>;
            if (authorities == null)
            {
                authorities = (from auth in a_repository.Authorities.OrderBy(c => c.Name)
                              select auth.ID).ToList();

            }


            TempData["MenuAuth"] = authorities;




            return PartialView(model);
        }

        //public PartialViewResult MenuAuth(long authorityINN = 0)
        //{
            
        //    ViewBag.SelectedAuthority = authorityINN;
        //    IEnumerable<cat_Authority> authorities = a_repository.Authority.OrderBy(x => x.Name);
        //    //IEnumerable<string> authorities = a_repository.Authority.Select(x => x.Name).Distinct().OrderBy(x => x);
        //    //IEnumerable<string> authorities = od_repository.OpenData.Select(x => x.Authority.Name).Distinct().OrderBy(x => x);
        //    return PartialView(authorities);
        //}

        public PartialViewResult MenuHorizontal(int link = 0)
        {
            ViewBag.SelectedLink = link;
            
            string [] links= new string [] {"Данные","Карта","Разработчикам","О портале"};
            return PartialView(links);
        }

    }
}
