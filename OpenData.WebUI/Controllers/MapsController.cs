using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpenData.Domain.Entities;
using OpenData.Domain.Abstract;
using OpenData.WebUI.Models;
using System.Data;
using CsvHelper;
using System.Text;


namespace OpenData.WebUI.Controllers
{
    public class MapsController : Controller
    {
        //
        // GET: /Maps/
        private IODRepository repository;
        private ICRepository c_repository;

        public MapsController(IODRepository ODRepository, ICRepository c_repository)
        {
            this.repository = ODRepository;
            this.c_repository = c_repository;
        }

        private int CountGeoInCategory(int cat)
        {
            return repository.OpenData.Count(od => od.HasGeo && od.Category.ID == cat); 
        }

        private int CountGeoInAuthority(long inn)
        {
            return repository.OpenData.Count(od => od.HasGeo && od.Authority.INN == inn);
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

        private DataTable GetTableFromBase(string ODID)
        {
            byte[] data = repository.Versions.FirstOrDefault(v => v.ODID == ODID && v.IsCurrent).File;
           
            string fileName = Server.MapPath("~/tmp/tmp.csv");
            try
	        {
	            // Open file for reading
	            System.IO.FileStream Stream = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
	 
	            // Writes a block of bytes to this stream using data from a byte array.
	            Stream.Write(data, 0, data.Length);
	 
	            // close file stream
	            Stream.Close();
	        }
	        catch (Exception _Exception)
	        {
	            // Error
	            Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
	        }



            return csvToDataTable(fileName,true);
        }
        
        
        private int CountRowInGeoData(string ODID)
        {
            DataTable dataTable = GetTableFromBase(ODID);
            return dataTable.Rows.Count;
        }
        
        public ActionResult Index()
        {
            ////IEnumerable<MapsDataset> datasets = repository.OpenData.Where(ods => ods.IsPublished).Select(ods => new MapsDataset { Dataset = ods, Count = CountRowInGeoData(ods.ODID) }).AsEnumerable();
            IEnumerable<MapsDataset> datasets = (from ds in repository.OpenData
                                                 join v in repository.Versions on ds.ODID equals v.ODID
                                                 where ds.IsPublished && ds.HasGeo && v.IsCurrent
                                                 select new MapsDataset
                                                 {
                                                     ODID = ds.ODID,
                                                     Description=ds.Description,
                                                     Count = v.RowsCount
                                                 }).AsEnumerable();


            //IEnumerable<MapsModel> model = (from c in c_repository.Category
                                             
            //                                select new MapsModel
            //                                {
            //                                    Category = c,
            //                                    Datasets = from ds in datasets
            //                                               where ds.Dataset.IsPublished && ds.Dataset.CategoryID == c.ID
            //                                               select new MapsDataset { 
            //                                               Dataset = ds.Dataset,
            //                                               Count = ds.Count
            //                                               },
            //                                    Count = CountGeoInCategory(c.ID)
            //                                }).AsEnumerable();
            //IEnumerable<MapsModel> model = from c in repository.OpenData
            //                               where c.HasGeo
            //                               join ds in repository.OpenData on c.Category.ID equals ds.CategoryID
            //                               join v in repository.Versions on ds.ODID equals v.ODID
            //                               select new MapsModel
            //                               {
            //                                   Category = c.Category,
            //                                   Datasets = new MapsDataset
            //                                   {
            //                                       Dataset = ds,
            //                                       Count = v.RowsCount
            //                                   },
            //                                   Count = repository.OpenData.Count(od => od.HasGeo && od.Category.ID == c.Category.ID)
            //                               };

            IEnumerable<MapsModel> model = from c in repository.OpenData
                                           //join ds in datasets on c.ODID equals ds.ODID
                                           where c.HasGeo
                                           select new MapsModel
                                           {
                                               CategoryId = c.Category.ID,
                                               CategoryName =c.Category.Name,
                                               Count = repository.OpenData.Count(od => od.HasGeo && od.Category.ID == c.Category.ID),
                                               Datasets = from ds in repository.OpenData
                                                          join v in repository.Versions on ds.ODID equals v.ODID
                                                          where ds.IsPublished && ds.CategoryID == c.Category.ID && v.IsCurrent && ds.HasGeo
                                                          select new MapsDataset
                                                          {
                                                              ODID = ds.ODID,
                                                              Description=ds.Description,
                                                              Count =   v.RowsCount
                                                          }
                                              
                                           };


            //foreach (MapsModel mm in model)
            //{
            //    mm.Datasets = from ds in repository.OpenData
            //                  join v in repository.Versions on ds.ODID equals v.ODID
            //                  where ds.IsPublished && ds.CategoryID == mm.CategoryId && v.IsCurrent && ds.HasGeo
            //                  select new MapsDataset
            //                  {
            //                      ODID = ds.ODID,
            //                      Description=ds.Description,
            //                      Count = v.RowsCount
            //                  };
            //    foreach (MapsDataset md in mm.Datasets)
            //    {
            //        bool q = 1 == 1;
            //    }
            //}


            //IEnumerable<MapsModel> model1 = from ds in repository.OpenData
            //                                where ds.HasGeo
            //                                let c = ds.Category
            //                                let CountInDS =1
            //                                select new MapsModel
            //                                {
            //                                    Category = ds.Category,
            //                                    Datasets = from cds in repository.OpenData
            //                                               where cds.IsPublished && cds.CategoryID == c.ID
            //                                               select new MapsDataset
            //                                               {
            //                                                   Dataset = ds,
            //                                                   Count = CountInDS
            //                                               },
            //                                    Count = repository.OpenData.Count(od => od.HasGeo && od.Category.ID == c.ID)

            //                                };
           
            //IEnumerable<MapsModel> model;

            //foreach (cat_Category cat in c_repository.Category)
            //{ 
            //    model.
            //}


            //Dictionary<string, int> categories= new Dictionary<string,int>();
            //Dictionary<string, int> datasets = new Dictionary<string, int>();
            //foreach (cat_Category cat in c_repository.Category)
            //{
            //    categories.Add(cat.Name, CountGeoInCategory(cat.ID));
            //}

            //foreach (OpenDataSet ods in repository.OpenData.Where(ods=>ods.IsPublished))
            //{
            //    datasets.Add(ods.Description, CountRowInGeoData(ods.ODID));
            //}

            //TempData["Categories"]=categories;
            //TempData["Datasets"] = datasets;
            //ViewBag.InCat = CountGeoInCategory(7);


            //ViewBag.InRow = CountRowInGeoData("3525093804-Libraries");
            return View(model);
        }

        public void AddCoordinates(string Odid)
        { 
        
        }

        [HttpGet]
        public PartialViewResult GetMap(string[] ds)
        {
            
            foreach (string odid in ds)
            {
                AddCoordinates(odid);
            }
            
            ViewBag.ODID = "/YMapsML/test.xml";
            return PartialView();
        }

        public JsonResult GetDataMap()
        {
            //string VerNum = repository.Versions.FirstOrDefault(v => (v.IsCurrent && v.ODID == ODID)).VerNum;
            //string csvpath = Server.MapPath("downloads/" + ODID + "/data-" + VerNum + ".csv");
            //csvpath = csvpath.Replace("Table", "datasets");
            ////var query = repository.OpenData.Select(name=>name.Name);
            //var DataAsTable = csvToDataTable(csvpath, true); //GetSetAsTable(ODID);
            List<object[]> Data = new List<object[]>();

            
            return Json(new
            {
                //sEcho = param.sEcho,
                //iTotalRecords = DataAsTable.Rows.Count,
                //iTotalDisplayRecords = param.iDisplayLength,
                data = Data
            }, "text/x-json",
                            JsonRequestBehavior.AllowGet); 
        }

    }
}
