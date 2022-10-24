using OpenData.Admin.Infrastructure.Abstract;
using OpenData.Admin.Models;
using OpenData.Admin.Providers;
using OpenData.Domain.Abstract;
using OpenData.Domain.Concrete;
using OpenData.Domain.CSVLoader;
using OpenData.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OpenData.Admin.Controllers
{
    [Authorize (Roles="Administrator")]
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

     
        
        //public ActionResult Index()
        //{
            
        //    return View(od_repository.OpenData);
        //}

        public ActionResult DataSets()
        {

            return View(od_repository.OpenData);
        }
        
        public ActionResult Index()
        {
            return View(u_repository.Users.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name));
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


       
        
        //public ViewResult LoadSet(string ODID)
        //{
        //    return View();
        //}


        //[HttpPost]
        //public ActionResult LoadSet(string ODID, HttpPostedFileBase VerCsv, HttpPostedFileBase StrCsv, int ReasonId = 0)
        //{ 
        //    if (StrCsv!=null && VerCsv==null) {
        //        TempData["message"]="Невозмлжно изменение структуры без изменения набора данных ";
        //        return View();
        //    }

        //    if (StrCsv!=null) {
        //        od_repository.OpenData.FirstOrDefault(ods => ods.ODID == ODID).IsPublished = true;
        //        od_repository.SaveOpenDataSet(od_repository.OpenData.FirstOrDefault(ods => ods.ODID == ODID));
        //        OpenData.Domain.Entities.Version CurVersion=od_repository.Versions.FirstOrDefault(v => v.IsCurrent);
        //        Structure CurStructure = od_repository.Structures.FirstOrDefault(s => s.IsCurrent);
        //        long CurVersionId = CurVersion != null ? CurVersion.ID : 0;
        //        int CurStructureId = CurStructure != null ? CurStructure.ID : 0;
        //        OpenData.Domain.Entities.Version NewVersion = new OpenData.Domain.Entities.Version();
        //        Structure NewStructure = new Structure();

        //        NewStructure.IsCurrent = true;
        //        NewStructure.Date = DateTime.Today.Date;
        //        NewStructure.VerNum = string.Format("{0:yyyyMMdd}", DateTime.Today.Date);
        //        NewStructure.File = new byte[StrCsv.ContentLength];
        //        StrCsv.InputStream.Read(NewStructure.File, 0, StrCsv.ContentLength);
        //        od_repository.AddStructure(NewStructure, CurStructureId);

        //        NewVersion.StructureID = NewStructure.ID;
        //        NewVersion.ODID = ODID; 
        //        NewVersion.IsCurrent = true;
        //        NewVersion.Date = DateTime.Today.Date;
        //        NewVersion.Reason = (Reasons)ReasonId;
        //        NewVersion.VerNum = string.Format("{0:yyyyMMdd}", DateTime.Today.Date);
        //        NewVersion.File = new byte[VerCsv.ContentLength];
        //        VerCsv.InputStream.Read(NewVersion.File, 0, VerCsv.ContentLength);
        //        od_repository.AddVersion(NewVersion, CurVersionId);

        //        return RedirectToAction("Index");
        //    }
        //    else {
        //        od_repository.OpenData.FirstOrDefault(ods => ods.ODID == ODID).IsPublished = true;
        //        od_repository.SaveOpenDataSet(od_repository.OpenData.FirstOrDefault(ods => ods.ODID == ODID));
        //        long CurVersionId = od_repository.Versions.FirstOrDefault(v => v.IsCurrent).ID;
        //        OpenData.Domain.Entities.Version NewVersion = new OpenData.Domain.Entities.Version();
        //        NewVersion.StructureID = od_repository.Versions.FirstOrDefault(v=>v.IsCurrent).StructureID;
        //        NewVersion.ODID = ODID;
        //        NewVersion.IsCurrent = true;
        //        NewVersion.Date = DateTime.Today.Date;
        //        NewVersion.Reason = (Reasons)ReasonId;
        //        NewVersion.VerNum = string.Format("{0:yyyyMMdd}", DateTime.Today.Date);
        //        NewVersion.File = new byte[VerCsv.ContentLength];
        //        VerCsv.InputStream.Read(NewVersion.File, 0, VerCsv.ContentLength);
        //        od_repository.AddVersion(NewVersion, CurVersionId);

                
        //        return RedirectToAction("Index");
        //    }
        //}

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
            var Query = from u in u_repository.Users
                        select new
                        {
                            ID=u.ID,
                            FNS=u.UserProfile.FNS
                        };
            ViewBag.UserID = new SelectList(Query, "ID", "FNS", selected);
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
                opendataset.AuthorityINN = a_repository.Authorities.FirstOrDefault(a => a.ID == opendataset.AuthorityID).INN;
                opendataset.CreateDate = DateTime.Today;
                od_repository.SaveOpenDataSet(opendataset);
                return View("Index", u_repository.Users.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name));
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
            return View("Index", u_repository.Users.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name));
        }

        public ViewResult CreateOperator()
        {
            AuthorityDropDownList();
            return View();
        }

        [HttpPost]
        public ActionResult CreateOperator(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipUser membershipUser = ((CustomMembershipProvider)Membership.Provider).CreateUser(model.Login, model.Password, 2, model);

                if (membershipUser != null)
                {

                    return RedirectToAction("Index", "Admin");

                }
                else
                {
                    AuthorityDropDownList(model.AuthorityID);
                    ModelState.AddModelError("", "Ошибка при регистрации");
                }
            }
            return View("Index", u_repository.Users.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name));
        }

        public PartialViewResult UserList(int RoleID)
        {
            IEnumerable<UserView> model;
            IEnumerable<User> users = u_repository.Users.Where(m => m.RoleID == RoleID);
            model = from u in users select new UserView { ID = u.ID, FNS = u.UserProfile.FNS, Login = u.Login, Authority=u.UserProfile.Authority.Name };

            return PartialView(model);
        }

        public ViewResult EditUserProfile(int Id)
        {
            User user = u_repository.Users.FirstOrDefault(u => u.ID == Id);
            EditUserModel model = new EditUserModel
            {
                Login = user.Login,
                //Authority = user.Authority.Name,
                Phone = user.UserProfile.Phone,
                Email = user.UserProfile.Email,
                Duty = user.UserProfile.Duty,
                FNS = user.UserProfile.FNS,
                ID = Id
            };
            return View(model);
        }

        [HttpPost]
        public ViewResult EditUserProfile(EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.NewPassword != null)
                {
                    MembershipUser membershipUser =((CustomMembershipProvider)Membership.Provider).GetUser(model.Login, false);
                    membershipUser.ChangePassword("oldPassword", model.NewPassword);
                }
            }

            return View("Index", u_repository.Users.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name));
        }

        public ViewResult EditOperatorProfile()
        {
            return View();
        }

        public PartialViewResult Profile()
        {
            return PartialView(u_repository.Users.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name));
        }

    }
}
