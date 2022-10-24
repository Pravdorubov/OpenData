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
    [Authorize (Roles="SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private IURepository u_repository;
        private IODRepository od_repository;
        private User CurrentUser;
       

        public SuperAdminController(IURepository u_repo, IODRepository od_repo)
        {
            u_repository = u_repo;
            od_repository = od_repo;
            
        }

       

        public ViewResult Return()
        {
            CurrentUser = u_repository.Users.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name);
            return View ("Index",CurrentUser);
        }
        
        public ActionResult Index()
        {
            CurrentUser = u_repository.Users.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name);
            return View(CurrentUser);
        }

        public PartialViewResult Menu()
        {
            return PartialView();
        }



        //public PartialViewResult DataSetList()
        //{
           
        //    IQueryable<User> users = od_repository.OpenData.Select(ods => ods.User);
        //    IQueryable<DataSetListModel> model = from od in od_repository.OpenData join u in users on od.UserID equals u.ID
        //                             select
        //                                 new DataSetListModel
        //                                 {
        //                                    ODID = od.ODID,
        //                                    Name=od.Name,
        //                                    Description = od.Description,
        //                                    //Authority = u.Authority.Name
        //                                 };
            
        //    return PartialView(model);
        //}

        public PartialViewResult Profile()
        {
            CurrentUser = u_repository.Users.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name);
            return PartialView(CurrentUser);
        }


        public ViewResult CreateAuthorityAdmin()
        {
            //AuthorityDropDownList();
            return View();
        }

        [HttpPost]
        public ViewResult CreateAuthorityAdmin(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipUser membershipUser = ((CustomMembershipProvider)Membership.Provider).CreateUser(model.Login, model.Password,1,model);

                if (membershipUser == null)
                {
                    ModelState.AddModelError("", "Ошибка при регистрации");
                }
            }
            return View("Index");
        }

        public ViewResult CreateOperator()
        {
            //AuthorityDropDownList();
            return View();
        }

        [HttpPost]
        public ViewResult CreateOperator(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipUser membershipUser = ((CustomMembershipProvider)Membership.Provider).CreateUser(model.Login, model.Password, 2, model);

                if (membershipUser == null)
                {
                    ModelState.AddModelError("", "Ошибка при регистрации");
                }
            }
            return View("Index");
        }

        public PartialViewResult UserList(int RoleID)
        {
            IEnumerable<UserView> model;
            IEnumerable<User> users=u_repository.Users.Where(m=>m.RoleID==RoleID);
            model = from u in users select new UserView {ID = u.ID, FNS = u.UserProfile.FNS, Login = u.Login}; 
            
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
                    Email= user.UserProfile.Email,
                    Duty = user.UserProfile.Duty,
                    FNS = user.UserProfile.FNS,
                    ID = Id
                };
            return View(model);
        }

        [HttpPost]
        public ViewResult EditUserProfile(EditUserModel model)
        {

            
            return View("Index");
        }

        public ViewResult EditOperatorProfile()
        {
            return View();
        }



        public ViewResult CreateDataSet()
        {
            return View();
        }

        public ViewResult EditDataSetPass()
        {
            return View();
        }

        public ViewResult EditDataSetData()
        {
            return View();
        }

        public ViewResult EditDataSetStructure()
        {
            return View();
        }



        public ViewResult EditProfile(int Id)
        {
            User user = u_repository.Users.FirstOrDefault(u => u.ID == Id);
            return View(user);
        }


        // -----------------
        public ViewResult EditDataSet(string ODId)
        {



            OpenDataSet opendataset = od_repository.OpenData.FirstOrDefault(p => p.ODID == ODId);



            //OpenDataSetAdminView model = new OpenDataSetAdminView
            //{
            //    OpenDataSet = opendataset,
            //    Authorities = u_repository.Authority,
            //    Categories = u_repository.Category
            //};
            //var AuthorityQuery = a_repository.Authority.ToList();
            //ViewBag.AutorityINN = new SelectList(AuthorityQuery, "INN", "Name");
            //ViewData["Authorities"] = from a in od_repository.OpenData select new SelectListItem { Text = a.Authority.Name, Value = a.Authority.INN.ToString() }; 
            //return View(opendataset);
            //AuthorityDropDownList(opendataset.User.AuthorityINN);
            //CategoryDropDownList(opendataset.CategoryID);
            return View(opendataset);
        }

        public ViewResult LoadSet(string ODID)
        {
            //LoadSetModel model = new LoadSetModel { ODID = ODID, ReturnUrl = "" };
            return View();
        }

        [HttpPost]
        public ActionResult LoadSet(string ODID, HttpPostedFileBase CsvFile, HttpPostedFileBase SchemeFile)
        {

            //string fileName = "C:\\inetpub\\wwwroot\\OpenData\\OpenData.WebUI\\CSV" + ODID + ".csv";
            //string fileName = "C:\\Users\\Leshukov.VA\\Documents\\Visual Studio 2012\\Projects\\OpenData\\OpenData.WebUI\\CSV\\" + ODID + ".csv";
            string fileName = Server.MapPath("~/CSVLoader") + "\\" + ODID + ".csv";
            CsvFile.SaveAs(fileName);
            CSVLoader.SaveToDatabase_withDataSet(fileName, ConfigurationManager.ConnectionStrings["SetsDataBase"].ConnectionString, ODID);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult EditDataSet(OpenDataSet opendataset, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    //opendataset.ImageMimeType = image.ContentType;
                    //opendataset.ImageData = new byte[image.ContentLength];
                    //image.InputStream.Read(opendataset.ImageData, 0, image.ContentLength);
                }

                //long inn=(long) u_repository.Users.FirstOrDefault(u => u.ID == opendataset.UserID).AuthorityINN;
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
            //    Authorities = u_repository.Authority,
            //    Categories = u_repository.Category
            //};
            //return View("Edit", model);
            OpenDataSet opendataset = new OpenDataSet();
            //AuthorityDropDownList();
            //CategoryDropDownList();
            return View("EditDataSet", opendataset);
        }

        //[HttpPost]
        //public ActionResult Delete(string ODID)
        //{
        //    OpenDataSet deleteOpenDataSet = od_repository.DeleteOpenDataSet(ODID);
        //    if (deleteOpenDataSet != null)
        //    {
        //        TempData["message"] = string.Format("{0} was deleted", deleteOpenDataSet.Name);
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}