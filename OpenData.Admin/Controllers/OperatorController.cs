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
using System.IO;
using OpenData.Domain.Helpers;
using System.Data;
using System.Text;


namespace OpenData.Admin.Controllers
{
    [Authorize (Roles="Operator")]
    public class OperatorController : Controller
    {
        private IODRepository od_repository;
        private IARepository a_repository;
        private ICRepository c_repository;
        private IURepository u_repository;
        //private string ServerPath = @"C:\Users\Leshukov.VA\Documents\Visual Studio 2012\Projects\OpenData\OpenData.WebUI\";
        private string ServerPath = @"D:\OpenData\OpenData.WebUI\datasets\downloads";

       

        public OperatorController(IODRepository od_repo,IARepository a_repo,ICRepository c_repo, IURepository u_repo)
        {
            od_repository = od_repo;
            a_repository = a_repo;
            c_repository = c_repo;
            u_repository = u_repo;
        }

        private static DataTable csvToDataTable(string file, bool isRowOneHeader)
        {

            DataTable csvDataTable = new DataTable();

            //no try/catch - add these in yourselfs or let exception happen
            String[] csvData = System.IO.File.ReadAllLines(file, Encoding.UTF8);

            //if no data in file ‘manually’ throw an exception
            if (csvData.Length == 0)
            {
                throw new Exception("CSV File Appears to be Empty");
            }

            String[] headings = csvData[0].Split(';');
            int index = 0; //will be zero or one depending on isRowOneHeader

            if (isRowOneHeader) //if first record lists headers
            {
                index = 1; //so we won’t take headings as data

                //for each heading
                for (int i = 0; i < headings.Length - 1; i++)
                {
                    //replace spaces with underscores for column names
                    //headings[i] = headings[i].Replace(" ", "_");

                    //add a column for each heading
                    csvDataTable.Columns.Add(headings[i], typeof(string));
                }
            }
            else //if no headers just go for col1, col2 etc.
            {
                for (int i = 0; i < headings.Length; i++)
                {
                    //create arbitary column names
                    csvDataTable.Columns.Add("col" + (i + 1).ToString(), typeof(string));
                }
            }

            //populate the DataTable
            for (int i = index; i < csvData.Length; i++)
            {
                //create new rows
                DataRow row = csvDataTable.NewRow();

                for (int j = 0; j < headings.Length - 1; j++)
                {
                    //fill them
                    row[j] = csvData[i].Split(';')[j];
                }

                //add rows to over DataTable
                csvDataTable.Rows.Add(row);
            }

            //return the CSV DataTable
            return csvDataTable;

        } 

        public ActionResult DataSets()
        {
            
            int Id=u_repository.Users.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name).ID;
            return View(od_repository.OpenData.Where(ods=>ods.UserID==Id));
        }

        public ActionResult Index(int UserId = 0)
        {
            return View(u_repository.Users.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name));
        }
        
        public PartialViewResult Profile()
        {
            return PartialView(u_repository.Users.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name));
        }

        private string GetRusText(string val)
        {
            switch (val)
            {
                case "StructureChange":
                    return "Изменение структуры";
                case "ErrorClean":
                    return "Устранение ошибки";
                case "RefreshDataSet":
                    return "Обновление набора данных";
            }
            return "";
        }

        

        public void ReasonsDropDownList(object selected=null)
        {
            var Query=(from Reasons r in Enum.GetValues(typeof(Reasons))
                       where r!=Reasons.Init
                      select new {
                        reason=r,
                        text = r.GetDescriptionString()
                      }).ToList();
            ViewBag.ReasonID = new SelectList(Query, "reason", "text", selected);
        }

        private bool InitLoad(string Odid)
        {
            return od_repository.Versions.Where(v => v.ODID == Odid).Count() == 0;
        }

        public ViewResult LoadSet(string ODID)
        {
            if (InitLoad(ODID))
            {
                ViewBag.ReasonInit = Reasons.Init;
               
            }
            else
            {
                ReasonsDropDownList();
            }
            LoadSetModel model = new LoadSetModel
            {
                ODID = ODID,
                ReturnUrl = "Operator/Index"
            };
            return View(model);
        }


        private bool ErrorLoad(bool InitLoad, Reasons Reason, bool StrExist, bool VerExist)
        {
            bool error = (InitLoad && (Reason != Reasons.Init)) || (InitLoad && (!StrExist || !VerExist)) || ((Reason == Reasons.StructureChange) && !StrExist) || (!VerExist);
            return error;
        }

        [HttpPost]
        public ActionResult LoadSet(string ODID, HttpPostedFileBase VerCsv, HttpPostedFileBase StrCsv, Reasons ReasonID=Reasons.Init)
        {
            if (ErrorLoad(InitLoad(ODID),ReasonID,StrCsv!=null,VerCsv!=null))
            //if (StrCsv != null && VerCsv == null)
            {
                TempData["message"] = "Ошибка при загрузке набора данных или структуры ";
                return View();
            }
            else
            {
                
                if (StrCsv != null)
                {
                    od_repository.OpenData.FirstOrDefault(ods => ods.ODID == ODID).IsPublished = true;
                    if (InitLoad(ODID))
                    {
                        od_repository.OpenData.FirstOrDefault(ods => ods.ODID == ODID).CreateDate = DateTime.Today;
                    }
                    od_repository.SaveOpenDataSet(od_repository.OpenData.FirstOrDefault(ods => ods.ODID == ODID));
                    OpenData.Domain.Entities.Version CurVersion = od_repository.Versions.FirstOrDefault(v => v.IsCurrent && v.ODID==ODID);
                    long CurVersionId;
                    int CurStructureId;
                    Structure CurStructure;
                    if (CurVersion!=null)
                    {
                        CurVersionId=CurVersion.ID;
                        CurStructure = od_repository.Structures.FirstOrDefault(s => s.IsCurrent && s.ID==CurVersion.StructureID);
                        CurStructureId = CurStructure.ID;
                    }
                    else
                    {
                        CurVersionId=0;
                        CurStructureId=0;
                    }
                  
                    OpenData.Domain.Entities.Version NewVersion = new OpenData.Domain.Entities.Version();
                    Structure NewStructure = new Structure();

                    NewStructure.IsCurrent = true;
                    NewStructure.Date = DateTime.Today.Date;
                    //NewStructure.VerNum = string.Format("{0:yyyyMMdd}", DateTime.Today.Date);
                    NewStructure.VerNum = string.Format("{0:yyyyMMdd}", DateTime.Now) + "T" +string.Format("{0:HHmm}", DateTime.Now);
                    NewStructure.File = new byte[StrCsv.ContentLength];
                    StrCsv.InputStream.Read(NewStructure.File, 0, StrCsv.ContentLength);
                    

                    NewVersion.StructureID = NewStructure.ID;
                    NewVersion.ODID = ODID;
                    NewVersion.IsCurrent = true;
                    NewVersion.Date = DateTime.Today.Date;
                    NewVersion.Reason = ReasonID;
                    //NewVersion.VerNum = string.Format("{0:yyyyMMdd}", DateTime.Today.Date);
                    NewVersion.VerNum = string.Format("{0:yyyyMMdd}", DateTime.Now) + "T" + string.Format("{0:HHmm}", DateTime.Now);
                    NewVersion.File = new byte[VerCsv.ContentLength];
                    VerCsv.InputStream.Read(NewVersion.File, 0, VerCsv.ContentLength);
                    //ServerPath = ServerPath == "" ? Server.MapPath("~/") : ServerPath;
                    DirectoryInfo DSdir = new DirectoryInfo(ServerPath + @"\" + ODID);
                    
                    if (CurVersionId == 0) { DSdir.Create(); }
                    string VerFile = DSdir + @"\data-"+ NewVersion.VerNum+ ".csv";
                    VerCsv.SaveAs(VerFile);
                    NewVersion.RowsCount = csvToDataTable(VerFile, true).Rows.Count;
                    CSVLoader.SaveToDatabase_withDataSet(VerFile, ConfigurationManager.ConnectionStrings["SetsDataBase"].ConnectionString, ODID);

                    string StrFile = DSdir.FullName+ @"\structure-" + NewStructure.VerNum + ".csv";
                    StrCsv.SaveAs(StrFile);
                    CSVLoader.SaveToDatabase_withDataSet(StrFile, ConfigurationManager.ConnectionStrings["SetsDataBase"].ConnectionString, ODID);
                    NewVersion.StructureID = od_repository.AddStructure(NewStructure, CurStructureId); 
                    od_repository.AddVersion(NewVersion, CurVersionId);

                    return RedirectToAction("Index");
                }
                else
                {
                    od_repository.OpenData.FirstOrDefault(ods => ods.ODID == ODID).IsPublished = true;
                    if (InitLoad(ODID))
                    {
                        od_repository.OpenData.FirstOrDefault(ods => ods.ODID == ODID).CreateDate = DateTime.Today;
                    }
                    od_repository.SaveOpenDataSet(od_repository.OpenData.FirstOrDefault(ods => ods.ODID == ODID ));
                    long CurVersionId = od_repository.Versions.FirstOrDefault(v => v.IsCurrent && v.ODID==ODID).ID;
                    OpenData.Domain.Entities.Version NewVersion = new OpenData.Domain.Entities.Version();
                    NewVersion.StructureID = od_repository.Versions.FirstOrDefault(v => v.IsCurrent && v.ODID==ODID).StructureID;
                    NewVersion.ODID = ODID;
                    NewVersion.IsCurrent = true;
                    NewVersion.Date = DateTime.Today.Date;
                    NewVersion.Reason = ReasonID;
                    //NewVersion.VerNum = string.Format("{0:yyyyMMdd}", DateTime.Today.Date);
                    NewVersion.VerNum = string.Format("{0:yyyyMMdd}", DateTime.Now) + "T" + string.Format("{0:HHmm}", DateTime.Now);
                    NewVersion.File = new byte[VerCsv.ContentLength];
                    VerCsv.InputStream.Read(NewVersion.File, 0, VerCsv.ContentLength);
                    DirectoryInfo DSdir = new DirectoryInfo(ServerPath + @"\" + ODID);
                    string VerFile = DSdir + @"\data-" + NewVersion.VerNum + ".csv";
                    VerCsv.SaveAs(VerFile);
                    NewVersion.RowsCount = csvToDataTable(VerFile, true).Rows.Count;
                    CSVLoader.SaveToDatabase_withDataSet(VerFile, ConfigurationManager.ConnectionStrings["SetsDataBase"].ConnectionString, ODID);
                    od_repository.AddVersion(NewVersion, CurVersionId);


                    return RedirectToAction("Index");
                }
            }
        }

    }
}
