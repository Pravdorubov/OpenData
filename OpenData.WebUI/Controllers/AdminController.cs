using OpenData.Domain.Abstract;
using OpenData.Domain.Entities;
using OpenData.Domain.CSVLoader;
using OpenData.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using OpenData.Domain.Concrete;

namespace OpenData.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IODRepository od_repository;
        private IARepository a_repository;
        private ICRepository c_repository;
        private IURepository u_repository;
       

        public AdminController(IODRepository od_repo,IARepository a_repo,ICRepository c_repo, IURepository u_repo)
        {
            od_repository = od_repo;
            a_repository = a_repo;
            c_repository = c_repo;
            u_repository = u_repo;
        }

     
        
        public ActionResult Index()
        {
            
            return View(od_repository.OpenData);
        }

        public ViewResult Edit(string ODId)
        {
            
            
            
            OpenDataSet opendataset = od_repository.OpenData.FirstOrDefault(p => p.ODID == ODId);
            

            
            //OpenDataSetAdminView model = new OpenDataSetAdminView
            //{
            //    OpenDataSet = opendataset,
            //    Authorities = repository.Authority,
            //    Categories = repository.Category
            //};
            //var AuthorityQuery = a_repository.Authority.ToList();
            //ViewBag.AutorityINN = new SelectList(AuthorityQuery, "INN", "Name");
            //ViewData["Authorities"] = from a in od_repository.OpenData select new SelectListItem { Text = a.Authority.Name, Value = a.Authority.INN.ToString() }; 
            //return View(opendataset);
            //AuthorityDropDownList(opendataset.AuthorityINN);
            //CategoryDropDownList(opendataset.CategoryID);
            return View(opendataset);
        }

        //public ViewResult LoadSet(string ODID)
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult LoadSet(string ODID, HttpPostedFileBase CsvFile, HttpPostedFileBase SchemeFile)
        //{

        //    //string fileName = "C:\\inetpub\\wwwroot\\OpenData\\OpenData.WebUI\\CSV" + ODID + ".csv";
        //    //string fileName = "C:\\Users\\Leshukov.VA\\Documents\\Visual Studio 2012\\Projects\\OpenData\\OpenData.WebUI\\CSV\\" + ODID + ".csv";
        //    string fileName = Server.MapPath("~/CSV") + "\\" + ODID + ".csv";

        //    CsvFile.SaveAs(fileName);
        //    CSVLoader.SaveToDatabase_withDataSet(fileName, ConfigurationManager.ConnectionStrings["SetsDataBase"].ConnectionString, ODID);
        //    return RedirectToAction("Index");
        //}

        public ViewResult LoadSet(string ODID)
        {
            return View();
        }


        [HttpPost]
        public ActionResult LoadSet(string ODID, HttpPostedFileBase VerCsv, HttpPostedFileBase StrCsv, int ReasonId = 0)
        { 
            if (StrCsv!=null && VerCsv==null) {
                TempData["message"]="Невозмлжно изменение структуры без изменения набора данных ";
                return View();
            }

            if (StrCsv!=null) {
                od_repository.OpenData.FirstOrDefault(ods => ods.ODID == ODID).IsPublished = true;
                od_repository.SaveOpenDataSet(od_repository.OpenData.FirstOrDefault(ods => ods.ODID == ODID));
                OpenData.Domain.Entities.Version CurVersion=od_repository.Versions.FirstOrDefault(v => v.IsCurrent);
                Structure CurStructure = od_repository.Structures.FirstOrDefault(s => s.IsCurrent);
                long CurVersionId = CurVersion != null ? CurVersion.ID : 0;
                int CurStructureId = CurStructure != null ? CurStructure.ID : 0;
                OpenData.Domain.Entities.Version NewVersion = new OpenData.Domain.Entities.Version();
                Structure NewStructure = new Structure();

                NewStructure.IsCurrent = true;
                NewStructure.Date = DateTime.Today.Date;
                NewStructure.VerNum = string.Format("{0:yyyyMMdd}", DateTime.Today.Date);
                NewStructure.File = new byte[StrCsv.ContentLength];
                StrCsv.InputStream.Read(NewStructure.File, 0, StrCsv.ContentLength);
                od_repository.AddStructure(NewStructure, CurStructureId);

                NewVersion.StructureID = NewStructure.ID;
                NewVersion.ODID = ODID; 
                NewVersion.IsCurrent = true;
                NewVersion.Date = DateTime.Today.Date;
                NewVersion.Reason = (Reasons)ReasonId;
                NewVersion.VerNum = string.Format("{0:yyyyMMdd}", DateTime.Today.Date);
                NewVersion.File = new byte[VerCsv.ContentLength];
                VerCsv.InputStream.Read(NewVersion.File, 0, VerCsv.ContentLength);
                od_repository.AddVersion(NewVersion, CurVersionId);

                return RedirectToAction("Index");
            }
            else {
                od_repository.OpenData.FirstOrDefault(ods => ods.ODID == ODID).IsPublished = true;
                od_repository.SaveOpenDataSet(od_repository.OpenData.FirstOrDefault(ods => ods.ODID == ODID));
                long CurVersionId = od_repository.Versions.FirstOrDefault(v => v.IsCurrent).ID;
                OpenData.Domain.Entities.Version NewVersion = new OpenData.Domain.Entities.Version();
                NewVersion.StructureID = od_repository.Versions.FirstOrDefault(v=>v.IsCurrent).StructureID;
                NewVersion.ODID = ODID;
                NewVersion.IsCurrent = true;
                NewVersion.Date = DateTime.Today.Date;
                NewVersion.Reason = (Reasons)ReasonId;
                NewVersion.VerNum = string.Format("{0:yyyyMMdd}", DateTime.Today.Date);
                NewVersion.File = new byte[VerCsv.ContentLength];
                VerCsv.InputStream.Read(NewVersion.File, 0, VerCsv.ContentLength);
                od_repository.AddVersion(NewVersion, CurVersionId);

                
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit( OpenDataSet opendataset, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    //opendataset.ImageMimeType = image.ContentType;
                    //opendataset.ImageData = new byte[image.ContentLength];
                    //image.InputStream.Read(opendataset.ImageData, 0, image.ContentLength);
                }

                od_repository.SaveOpenDataSet(opendataset);
                TempData["message"] = string.Format("{0} has been saved", opendataset.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(opendataset);
            }
        }

        public ViewResult Create() 
        {
            //OpenDataSetAdminView model = new OpenDataSetAdminView
            //{
            //    OpenDataSet = new OpenDataSet(),
            //    Authorities = repository.Authority,
            //    Categories = repository.Category
            //};
            //return View("Edit", model);
            OpenDataSet opendataset = new OpenDataSet();
            //AuthorityDropDownList(opendataset.AuthorityINN);
            //CategoryDropDownList(opendataset.CategoryID);
            return View("Edit", opendataset);
        }

        [HttpPost]
        public ActionResult Delete(string ODID)
        {
            OpenDataSet deleteOpenDataSet = od_repository.DeleteOpenDataSet(ODID);
            if (deleteOpenDataSet != null)
            { 
                TempData["message"]=string.Format("{0} was deleted", deleteOpenDataSet.Name);
            }
            return RedirectToAction("Index");
        }



        //public PartialViewResult CreateOwner()
        //{
        //    return PartialView();
        //}

        //[HttpPost]
        //public ViewResult CreateOwner(Owner owner)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        od_repository.SaveOwner(owner);
        //        return View("Index");
        //    }
        //    else
        //    {
        //        return View(owner);
        //    }
        //}

        private void AuthorityDropDownList(object selected = null)
        {
            var Query = a_repository.Authorities.OrderBy(a => a.Name);
            ViewBag.AuthorityID = new SelectList(Query, "ID", "Name", selected);
        }

        private void UserDropDownList(object selected = null)
        {
            //IQueryable<Owner> owners = od_repository.OpenData.Select(ods => ods.Owner);
            //var Query = from ods in od_repository.OpenData
            //            join o in owners on ods.OwnerID equals o.ID
            //            select new
            //            {
            //                ID=o.ID,
            //                SNP=o.SNP
            //            };
            var Query = u_repository.Users.OrderBy(u=>u.UserProfile.FNS);
            ViewBag.OwnerID = new SelectList(Query, "ID", "SNP", selected);
        }

        private void CategoryDropDownList(object selected = null)
        {
            var Query = c_repository.Category.OrderBy(a => a.Name);
            ViewBag.CategoryID = new SelectList(Query, "ID", "Name", selected);
        }

        public ViewResult CreatePass()
        {
            AuthorityDropDownList();
            CategoryDropDownList();
            UserDropDownList();
            return View();
        }

        [HttpPost]
        public ViewResult CreatePass(OpenDataSet opendataset)
        {

            if (ModelState.IsValid)
            {
                opendataset.IsPublished = false;
                od_repository.SaveOpenDataSet(opendataset);
                return View("Index", od_repository.OpenData);
            }
            else
            {
                AuthorityDropDownList(opendataset.AuthorityID);
                CategoryDropDownList(opendataset.CategoryID);
                UserDropDownList(opendataset.UserID);
                return View(opendataset);
            }
        }

        public ActionResult PubUnpub(string ODID)
        {
            bool pubunpub= od_repository.OpenData.FirstOrDefault(ods => ods.ODID == ODID).IsPublished;
            od_repository.OpenData.FirstOrDefault(ods => ods.ODID == ODID).IsPublished = !pubunpub;
            od_repository.SaveOpenDataSet(od_repository.OpenData.FirstOrDefault(ods => ods.ODID == ODID));
            return View("Index", od_repository.OpenData);
        }

    }
}
