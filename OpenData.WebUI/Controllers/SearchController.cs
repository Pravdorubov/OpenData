using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpenData.Domain.Abstract;
using OpenData.WebUI.Models;

namespace OpenData.WebUI.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        private IODRepository repository;
        private ICRepository c_repository;
        private IARepository a_repository;
        public int PageSize = 20;
        

        public SearchController(IODRepository ODRepository, ICRepository CRepository, IARepository ARepository)
        {
            this.repository = ODRepository;
            this.c_repository = CRepository;
            this.a_repository = ARepository;
        }

        [HttpGet]
        public ActionResult Index(string text, int page=1)
        {
            //var query = from ods in repository.OpenData
            //            where ods.Name.Contains(text);
            string lowerText = text.ToLower();

            var query = repository.OpenData.Where(ods => ods.Name.ToLower().Contains(lowerText) || ods.ODID.ToLower().Contains(lowerText) || ods.Description.ToLower().Contains(lowerText)
             || ods.FullDescription.ToLower().Contains(lowerText) || ods.Authority.Name.ToLower().Contains(lowerText) || ods.Category.Name.ToLower().Contains(lowerText)
             || ods.KeyWords.ToLower().Contains(lowerText));

            var Query = from ods in repository.OpenData.Where(ods => (ods.Name.ToLower().Contains(lowerText) || ods.ODID.ToLower().Contains(lowerText) || ods.Description.ToLower().Contains(lowerText)
             || ods.FullDescription.ToLower().Contains(lowerText) || ods.Authority.Name.ToLower().Contains(lowerText) || ods.Category.Name.ToLower().Contains(lowerText)
             || ods.KeyWords.ToLower().Contains(lowerText)) && ods.IsPublished)
                .OrderBy(p => p.ODID).Skip((page - 1) * PageSize).Take(PageSize)
                        join v in repository.Versions.Where(vr => vr.IsCurrent) on ods.ODID equals v.ODID
                        select new DataSetListView
                        {
                            DataSet = ods,
                            Path = "datasets/downloads/" + v.ODID + "/data-" + v.VerNum + ".csv"
                        };

            var array = Query.Count();

            OpenDataSetsListViewModel model = new OpenDataSetsListViewModel
            {
                OpenDataSets = Query,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = Query.Count()
                },
                CurrentCategories = null,
                CurrentAuthorities = null
            };
                        
            return View(model);
        }

    }
}
