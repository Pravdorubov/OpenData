using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpenData.Domain.Entities;
using OpenData.Domain.Abstract;
using OpenData.WebUI.Models;
using Newtonsoft.Json;


namespace OpenData.WebUI.Controllers
{
    public class OpenDataSetController : Controller
    {
        private IODRepository repository;
        private ICRepository c_repository;
        private IARepository a_repository;
        public int PageSize = 20;
        

        public OpenDataSetController(IODRepository ODRepository, ICRepository CRepository, IARepository ARepository)
        {
            this.repository = ODRepository;
            this.c_repository = CRepository;
            this.a_repository = ARepository;
        }

        public PartialViewResult AjaxList(int authorityId = 0, int categoryId = 0, int page = 1, int au = 0, int ca = 0)
        {
            DataSetListView dataSet = new DataSetListView();

            
            List<int> categories;
            if (ca == 0)
            {
                categories = TempData["MenuCat"] as List<int>;
                if (page == 1)
                {
                    if (categories == null || categoryId == -1)
                    {
                        categories = (from cat in c_repository.Category.OrderBy(c => c.Name)
                                      select cat.ID).ToList();
                    }
                    if (categoryId > 0)
                    {
                        if (categories.Exists(c => c == categoryId))
                        {
                            categories.Remove(categoryId);
                        }
                        else
                        {
                            categories.Add(categoryId);
                        }
                    }
                    else if (categoryId == -2)
                    {
                        categories = new List<int>();
                    }
                }
            }
            else
            {
                categories = new List<int>();
                categories.Add(ca);
            }

            List<int> authorities;
            if (au == 0)
            {
                authorities = TempData["MenuAuth"] as List<int>;
                if (page == 1)
                {
                    if (authorities == null || authorityId == -1)
                    {
                        authorities = (from auth in a_repository.Authorities.OrderBy(c => c.Name)
                                       select auth.ID).ToList();
                    }
                    if (authorityId > 0)
                    {
                        if (authorities.Exists(a => a == authorityId))
                        {
                            authorities.Remove(authorityId);
                        }
                        else
                        {
                            authorities.Add(authorityId);
                        }
                    }
                    else if (authorityId == -2)
                    {
                        authorities = new List<int>();
                    }
                }
            }
            else
            {
                authorities = new List<int>();
                authorities.Add(au);
            }


            TempData["MenuCat"] = categories;
            TempData["MenuAuth"] = authorities;
         

            var Query = (from ods in repository.OpenData.Where
                (p => p.IsPublished)
               
                        join v in repository.Versions.Where(vr => vr.IsCurrent) on ods.ODID equals v.ODID
                        join c in categories on ods.CategoryID equals c
                        join a in authorities on ods.AuthorityID equals a
                        select new DataSetListView
                        {
                            DataSet = ods,
                            Path = "datasets/downloads/" + v.ODID + "/data-" + v.VerNum + ".csv"
                        }).OrderBy(p => p.DataSet.ODID).Skip((page - 1) * PageSize).Take(PageSize);

            var array = Query.ToList();

            //Query=Query.OrderBy(p => p.DataSet.ODID).Skip((page - 1) * PageSize).Take(PageSize);

            //array = Query.ToList();

            int totalItems = (from ods in repository.OpenData.Where(ods => ods.IsPublished)
                     join c in categories on ods.CategoryID equals c
                     join a in authorities on ods.AuthorityID equals a
                     select new { }).Count();
            
            
            OpenDataSetsListViewModel model = new OpenDataSetsListViewModel
            {
                OpenDataSets = Query,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    //TotalItems = repository.OpenData.Where(e => (categories.Exists(c=>c==e.CategoryID)) || (authorities.Exists(a=>a==e.AuthorityID))).Count()
                    TotalItems=totalItems
                },
                CurrentCategories = categories,
                CurrentAuthorities = authorities
            };



           

            return PartialView(model);
        }

        public ViewResult List2()
        {
            return View();
        }
        
        public ViewResult List(int authorityId = 0, int categoryId = 0, int page = 1, int au=0, int ca=0)
        {
            DataSetListView dataSet = new DataSetListView();
           
            //IEnumerable<MenuCatModel> categories = TempData["MenuCat"] as IEnumerable<MenuCatModel>;
            //categories.FirstOrDefault(c => c.Category.ID == categoryId).isSelected = !categories.FirstOrDefault(c => c.Category.ID == categoryId).isSelected;
            List<int> categories;
            if (ca == 0)
            {
                categories = TempData["MenuCat"] as List<int>;
                if (categories == null || categoryId == -1)
                {
                    categories = (from cat in c_repository.Category.OrderBy(c => c.Name)
                                  select cat.ID).ToList();
                }
                if (categoryId > 0)
                {
                    if (categories.Exists(c => c == categoryId))
                    {
                        categories.Remove(categoryId);
                    }
                    else
                    {
                        categories.Add(categoryId);
                    }
                }
                else if (categoryId == -2)
                {
                    categories = new List<int>();
                }

            }
            else
            { 
                categories=new List<int>();
                categories.Add(ca);
            }

            List<int> authorities;
            if (au == 0)
            {
                authorities = TempData["MenuAuth"] as List<int>;
                if (authorities == null || authorityId == -1)
                {
                    authorities = (from auth in a_repository.Authorities.OrderBy(c => c.Name)
                                   select auth.ID).ToList();
                }
                if (authorityId > 0)
                {
                    if (authorities.Exists(a => a == authorityId))
                    {
                        authorities.Remove(authorityId);
                    }
                    else
                    {
                        authorities.Add(authorityId);
                    }
                }
                else if (authorityId == -2)
                {
                    authorities = new List<int>();
                }
            }
            else
            {
                authorities = new List<int>();
                authorities.Add(au);
            }


            TempData["MenuCat"] = categories;
            TempData["MenuAuth"] = authorities;
            //var SelCats = (from sc in categories where sc.isSelected select sc.Category.ID).ToList();
            //((categoryId == 0 && authorityINN == 0) || p.Authority.INN == authorityINN || p.Category.ID==categoryId ) &&

            var Query = from ods in repository.OpenData.Where
                (p =>  p.IsPublished)
                .OrderBy(p => p.ODID).Skip((page - 1) * PageSize).Take(PageSize)
                        join v in repository.Versions.Where(vr => vr.IsCurrent) on ods.ODID equals v.ODID
                        join c in categories on ods.CategoryID equals c
                        join a in authorities on ods.AuthorityID equals a
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
                    TotalItems = repository.OpenData.Where(e => (e.Category.ID == categoryId) || (e.Authority.ID == authorityId)).Count()
                },
                CurrentCategories = categories,
                CurrentAuthorities = authorities
            };



            //OpenDataSetsListViewModel model =new OpenDataSetsListViewModel
            //{
            //    OpenDataSets = repository.OpenData.Where
            //        ( p=>((categoryId==0 && authorityINN==0)   ||  p.Category.ID==categoryId || p.Authority.INN==authorityINN) && p.IsPublished )
            //        .OrderBy(p => p.ODID).Skip((page - 1) * PageSize).Take(PageSize),
            //    PagingInfo = new PagingInfo
            //    {
            //        CurrentPage = page,
            //        ItemsPerPage = PageSize,
            //        TotalItems =repository.OpenData.Where(e=>(e.Category.ID==categoryId)||(e.Authority.INN==authorityINN)).Count()
            //    },
            //    CurrentCategoryID=categoryId,
            //    CurrentAuthorityINN=authorityINN
            //    };

            return View(model);
        }

        //public ViewResult List(long authorityINN = 0, int categoryId = 0, int page = 1)
        //{
        //    DataSetListView dataSet= new DataSetListView();

        //    var Query = from ods in repository.OpenData.Where
        //        (p => ((categoryId == 0 && authorityINN == 0) || p.Category.ID == categoryId || p.Authority.INN == authorityINN) && p.IsPublished)
        //        .OrderBy(p => p.ODID).Skip((page - 1) * PageSize).Take(PageSize)
        //                join v in repository.Versions.Where(vr=>vr.IsCurrent) on ods.ODID equals v.ODID
        //                select new DataSetListView { 
        //                DataSet=ods,
        //                Path="datasets/downloads/" + v.ODID + "/data-" +  v.VerNum + ".csv"
        //                };


        //    OpenDataSetsListViewModel model = new OpenDataSetsListViewModel
        //    {
        //        OpenDataSets = Query,
        //        PagingInfo = new PagingInfo
        //        {
        //            CurrentPage = page,
        //            ItemsPerPage = PageSize,
        //            TotalItems = repository.OpenData.Where(e => (e.Category.ID == categoryId) || (e.Authority.INN == authorityINN)).Count()
        //        },
        //        CurrentCategoryID = categoryId,
        //        CurrentAuthorityINN = authorityINN
        //    };
            
                
            
        //    //OpenDataSetsListViewModel model =new OpenDataSetsListViewModel
        //    //{
        //    //    OpenDataSets = repository.OpenData.Where
        //    //        ( p=>((categoryId==0 && authorityINN==0)   ||  p.Category.ID==categoryId || p.Authority.INN==authorityINN) && p.IsPublished )
        //    //        .OrderBy(p => p.ODID).Skip((page - 1) * PageSize).Take(PageSize),
        //    //    PagingInfo = new PagingInfo
        //    //    {
        //    //        CurrentPage = page,
        //    //        ItemsPerPage = PageSize,
        //    //        TotalItems =repository.OpenData.Where(e=>(e.Category.ID==categoryId)||(e.Authority.INN==authorityINN)).Count()
        //    //    },
        //    //    CurrentCategoryID=categoryId,
        //    //    CurrentAuthorityINN=authorityINN
        //    //    };

        //        return View(model);
        //}

        public FileContentResult GetImage(string ODId)
        {
            OpenDataSet odp = repository.OpenData.FirstOrDefault(p => p.ODID == ODId);
            if (odp != null)
            {
                return File(odp.Category.ImageData, odp.Category.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        public ActionResult Show()
        {
            return View("Show1");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult Test()
        {
            //var query = repository.OpenData.Select(name=>name.Name);
            

            //string json = JsonConvert.SerializeObject(query);
            //return Json(json);

            var list = repository.OpenData;
            var data = from s in list select new { s.Name, s.Description };
            return Json(data, "text/x-json", JsonRequestBehavior.AllowGet);
            //return json;
        }

        //[AcceptVerbs(HttpVerbs.Get)]
        //public JsonResult Test2(testtype data)
        //{
            
        //    var list = repository.OpenData;
        //    var cdata = from s in list select new { s.Name, s.Description };
        //    return Json(data, "text/x-json", JsonRequestBehavior.AllowGet);
        //}

       

    }
}
