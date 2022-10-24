using OpenData.Domain.Entities;
using OpenData.Domain.Abstract;
using OpenData.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Web.Routing;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using OpenData.Domain.Helpers;


namespace OpenData.WebUI.Controllers
{
    public class TableController : Controller
    {
        //
        // GET: /Table/
        public int PageSize = 10;
        private int totalItems;
        private IODRepository repository;
        private IURepository u_repository;
        private IOrderProcessor OrderProcessor;

        public TableController(IODRepository repo, IOrderProcessor proc,IURepository u_repo)
        {
            this.repository = repo;
            this.OrderProcessor = proc;
            this.u_repository = u_repo;
        }

        public static DataTable csvToDataTable(string file, bool isRowOneHeader)
        {

        DataTable csvDataTable = new DataTable();

        //no try/catch - add these in yourselfs or let exception happen
        String[] csvData = System.IO.File.ReadAllLines(file,Encoding.UTF8);

        //if no data in file ‘manually’ throw an exception
        if (csvData.Length == 0)
        {
            throw new Exception("CSV File Appears to be Empty");
        }

        String[] headings = csvData[0].Split(';');
        int index = 0; //will be zero or one depending on isRowOneHeader

        if(isRowOneHeader) //if first record lists headers
        {
            index = 1; //so we won’t take headings as data
       
            //for each heading
            for(int i = 0; i < headings.Length-1; i++)
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
                csvDataTable.Columns.Add("col"+(i+1).ToString(), typeof(string));
            }
        }

        //populate the DataTable
        for (int i = index; i < csvData.Length; i++)
        {
            //create new rows
            DataRow row = csvDataTable.NewRow();

            for (int j = 0; j < headings.Length-1; j++)
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

        //public DataTable GetSetAsTable(string TableName, int page=1)
        //{
        //    string ConnectionString = ConfigurationManager.ConnectionStrings["SetsDataBase"].ConnectionString;
        //    using (SqlConnection connection =
        //               new SqlConnection(ConnectionString))
        //    {

        //        SqlDataAdapter adapter = new SqlDataAdapter();
        //        adapter.TableMappings.Add("Table", "OpenDataSet");
        //        connection.Open();
        //        //Console.WriteLine("The SqlConnection is open.");
        //        SqlCommand command = new SqlCommand("SELECT * FROM [" + TableName + "]", connection);
        //        command.CommandType = CommandType.Text;
        //        adapter.SelectCommand = command;
        //        DataSet dataSet = new DataSet("OpenDataSet");
        //        adapter.Fill(dataSet);
        //        DataTable dataTable = dataSet.Tables["OpenDataSet"];
        //        connection.Close();
        //        //Console.WriteLine("The SqlConnection is closed.");

        //        int begin = (page-1)*PageSize;
        //        totalItems = dataTable.Rows.Count;
        //        int end = (page*PageSize<totalItems)?page*PageSize:totalItems;
                
        //        //DataTable result = new DataTable();
        //        //result = dataTable.Clone();
        //        //result.Clear();
                

        //        //for (int i = begin; i < end; i++)
        //        //{
        //        //    result.ImportRow(dataTable.Rows[i]);
        //        //}

                               
        //        //return result;
        //        return dataTable;
                
        //    }

        //}

        [HttpPost]
        public ViewResult GetError(ShippingDetails err)
        {
            int UserId = repository.OpenData.FirstOrDefault(ods => ods.ODID == err.ODID).Users.ID;
            string Email = u_repository.Users.FirstOrDefault(u => u.ID == UserId).UserProfile.Email;
            err.RowNum = err.RowNum + 1;
            OrderProcessor.ProcessOrder(err, Email);
            CreateJsFile(err.ODID);
            ViewBag.ODID = err.ODID;
            ViewBag.Description = repository.OpenData.FirstOrDefault(ods => ods.ODID == err.ODID).Description;
            string VerNum = repository.Versions.FirstOrDefault(v => (v.IsCurrent && v.ODID == err.ODID)).VerNum;
            string csvpath = Server.MapPath("downloads/" + err.ODID + "/data-" + VerNum + ".csv");
            return View("Index", new TableViewModel
            {
                Data = csvToDataTable(csvpath, true),
                ReturnUrl = "/",
                OpenDataSet = repository.OpenData.FirstOrDefault(ods => ods.ODID == err.ODID),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = 1,
                    ItemsPerPage = PageSize,
                    TotalItems = totalItems
                }
            });
        }

        //private string ReasonToString(Reasons reason)
        //{
        //    switch (reason)
        //    {
        //        case Reasons.StructureChange:
        //            return "Изменение структуры";
        //        case Reasons.ErrorClean:
        //            return "Исправление ошибки";
        //        case Reasons.RefreshDataSet:
        //            return "Обновление данных";
        //        default:
        //            return "";
        //    }
        //}

        //private string PeriodicityToString(Periodicity periodicity)
        //{
        //    switch (periodicity)
        //    {
        //        case Periodicity.MoreThanDaily:
        //            return "Больше одного раза в день";
        //        case Periodicity.Daily:
        //            return "Ежедневно";
        //        case Periodicity.Weekly:
        //            return "Еженедельно";
        //        case Periodicity.Monthly:
        //            return "Ежемесячно";
        //        case Periodicity.Quarterly:
        //            return "Ежеквартально";
        //        case Periodicity.SemiAnnually:
        //            return "Каждые полгода";
        //        case Periodicity.Annually:
        //            return "Ежегодно";
        //        case Periodicity.UponReceipt:
        //            return "По мере изменения данных";
        //        default:
        //            return "";
        //    }
        //}

        
        public PartialViewResult DataSetInfo(OpenDataSet opendataset)
        {
            OpenData.Domain.Entities.Version LastVersion = repository.Versions.FirstOrDefault(v=>(v.IsCurrent && v.ODID==opendataset.ODID));
            
            string dataSetLink= opendataset.ODID + "/data-" + LastVersion.VerNum + ".csv";
            string structureLink = (from v in repository.Versions 
                                    join s in repository.Structures on v.StructureID equals s.ID 
                                    where v.ODID==opendataset.ODID && s.IsCurrent
                                    select s.VerNum).FirstOrDefault();
            structureLink =  opendataset.ODID + "/structure-" + structureLink + ".csv";
            int UserId = repository.OpenData.FirstOrDefault(ods => ods.ODID == opendataset.ODID).Users.ID;
            string SNP=  u_repository.Users.FirstOrDefault(u => u.ID == UserId).UserProfile.FNS;
            string Email = u_repository.Users.FirstOrDefault(u => u.ID == UserId).UserProfile.Email;
            string Phone = u_repository.Users.FirstOrDefault(u => u.ID == UserId).UserProfile.Phone;


            PassViewModel model = new PassViewModel();
            model.ODID = opendataset.ODID;
            model.Name = opendataset.Name;
            model.Description = opendataset.Description;
            model.FullDescription = opendataset.FullDescription;
            model.Authority = opendataset.Authority.Name;
            model.UserSNP = SNP;
            model.UserPhone = Phone;
            model.UserEmail = Email;
            model.DataSetLink = dataSetLink;
            model.StructureLink = structureLink;
            model.FirstDate =string.Format("{0:dd.MM.yyyy}", opendataset.CreateDate);
            model.LastDate = string.Format("{0:dd.MM.yyyy}", LastVersion.Date);
            model.Reason = LastVersion.Reason.GetDescriptionString();//ReasonToString(LastVersion.Reason);
            model.Periodicity = opendataset.Periodicity.GetDescriptionString();//PeriodicityToString(opendataset.Periodicity);
            model.KeyWords = opendataset.KeyWords;
            return PartialView(model);
        }

        public PartialViewResult DataSetAttr(DataTable data)
        {
            return PartialView(data);
        }
       
        public ViewResult ViewCSV()
        {
            return View();
        }

        public ActionResult SearchInput (string RowName=null)
        {

            return View();
        }

        public ViewResult Index(string Odid,string returnUrl, int page=1)
        {
            CreateJsFile(Odid);
            ViewBag.ODID = Odid;
            ViewBag.Description = repository.OpenData.FirstOrDefault(ods => ods.ODID == Odid).Description;
            string VerNum = repository.Versions.FirstOrDefault(v => (v.IsCurrent && v.ODID == Odid)).VerNum;
            string csvpath = Server.MapPath("downloads/" + Odid + "/data-" + VerNum + ".csv");
            //DataTable data = csvToDataTable(csvpath, true);
            return View(new TableViewModel
            {
                Data = csvToDataTable(csvpath, true),//GetSetAsTable(Odid,page),
                ReturnUrl = returnUrl,
                OpenDataSet=repository.OpenData.FirstOrDefault(ods=>ods.ODID==Odid),
                PagingInfo = new PagingInfo 
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = totalItems
                }
            });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        //[HttpPost]
        //public JsonResult Test(jQueryDataTablesModel param, string ODID)
        public JsonResult GetTable(string ODID)
        {
            string VerNum = repository.Versions.FirstOrDefault(v => (v.IsCurrent && v.ODID == ODID)).VerNum;
            string csvpath = Server.MapPath("downloads/" + ODID + "/data-" + VerNum + ".csv");
            csvpath = csvpath.Replace("Table", "datasets");
            //var query = repository.OpenData.Select(name=>name.Name);
            var DataAsTable = csvToDataTable(csvpath, true); //GetSetAsTable(ODID);
            var DataAsQuery = DataAsTable.AsEnumerable().AsQueryable();

            //List<string[]> data = new List<string[]>();
            //string[] DataRowAsArray= new string[DataAsTable.Columns.Count];
            //foreach (DataRow dr in DataAsTable.Rows)
            //{
            //    object[] itemArray = dr.ItemArray;
            //    for (int i=0; i<itemArray.Length;i++)
            //    {
            //        DataRowAsArray[i] = itemArray[i].ToString();

            //    }
            //    data.Add(DataRowAsArray);

            //}

            List<object[]> data = new List<object[]>();
            foreach (DataRow dr in DataAsTable.Rows)
            {
                data.Add(dr.ItemArray);


            }

            return Json(new
            {
                //sEcho = param.sEcho,
                //iTotalRecords = DataAsTable.Rows.Count,
                //iTotalDisplayRecords = param.iDisplayLength,
                aaData = data
            }, "text/x-json",
                            JsonRequestBehavior.AllowGet);

            ////string json = JsonConvert.SerializeObject(query);
            ////return Json(json);
            //var Data = GetSetAsTable("3525000008-Libraries");
            //jQueryDataTablesModel result= new jQueryDataTablesModel();
            ////result.iColumns = Data.Columns.Count;
            //result.sEcho = param.sEcho;

            //var list = repository.OpenData;
            //var data = from s in list select new { s.Name, s.Description };
            //return Json(result, "text/x-json", JsonRequestBehavior.AllowGet);
            ////return json;
        }

        //[HttpPost]
        //public ActionResult GetDataTables(Datatables.Mvc.DataTable dataTable)
        //{
        //    List<List<string>> table = new List<List<string>>();
        //    var DataAsTable=GetSetAsTable("3525000008-Libraries");
        //    var DataAsQuery=DataAsTable.AsEnumerable().AsQueryable();
        //    dataTable.iColumns=DataAsTable.Columns.Count;
        //    DataColumnCollection cols=DataAsTable.Columns;
        //    List<string> column = new List<string>();

        //    for (int i=0; i < cols.Count; i++ )
        //    {
        //        column.Add(cols[i].ToString());
        //    }

           
        //    return new Datatables.Mvc.DataTableResult (dataTable, table.Count, table.Count, table);
        //}

        public void CreateJsFile(string ODID)
        {
            string VerNum = repository.Versions.FirstOrDefault(v => (v.IsCurrent && v.ODID == ODID)).VerNum;
            string csvpath = Server.MapPath("downloads/" + ODID + "/data-" + VerNum + ".csv");
            DataTable dataTable = csvToDataTable(csvpath, true);//GetSetAsTable(ODID);
            //StreamWriter src = System.IO.File.CreateText(@"D:\OpenData\OpenData.WebUI\Scripts\Yadcf" + @"\" + ODID + ".js");
            StreamWriter src = System.IO.File.CreateText(Server.MapPath("/Scripts/Yadcf") + @"\" + ODID + ".js");
            src.Write("$(document).ready(function(){var oTable=$('#datatable').dataTable({'iDisplayLength': 10,'bJQueryUI': true,'bProcessing': true,'bServerSide': false,");
            src.Write("'sPaginationType': 'two_button','oLanguage':{sUrl:'/Content/datatables_lang_rus.txt'},");
            //src.Write("'sAjaxSource': '/Table/Test?ODID=" + ODID + "',");
            src.Write("'sAjaxSource': '/Table/GetTable?ODID="+ ODID + "',");
            src.Write("'aoColumns':[");
            int i = 0;
            int end = dataTable.Columns.Count;
            foreach (DataColumn col in dataTable.Columns)
            {
                if (i != end-1)
                {
                    src.Write("{'sTitle':'" + col.Caption + "'},");
                }
                else
                {
                    src.Write("{'sTitle':'" + col.Caption + "'}],");
                }
                i++;
            }
            src.Write("'sDom':'C<\"clear\">lfrtip',");
            src.Write("'oColVis':{'buttonText':'Показать/скрыть'}");
            src.Write("}).columnFilter({'sPlaceHolder': 'thead:after', 'aoColumns':[");
            for (int j = 0; j < end; j++)
            {
                if (j != end - 1)
                {
                    src.Write("{type:'text'},");
                }
                else
                {
                    src.Write("{type:'text'}]}); ");
                }
            }
            src.Write("$('#datatable').on('dblclick','td',function (e) {var sData = oTable.fnGetPosition( this );$('#requestCreation').show();$('#requestCreation #row').text(sData[0]);  }); ");
            src.Write("$('#requestCreation .cancelButton').click(function() {$('#requestCreation').hide()});");
            //src.Write("$('#datatable').on('click','td', function () {var sData = oTable.fnGetPosition( this ); alert( 'The cell clicked on had the value of '+sData );}) ;");
            src.Write(" });");
            //src.Write("}).yadcf([");
            //for (int j = 0; j < end; j++)
            //{
            //    if (j != end - 1)
            //    {
            //        src.Write("{column_number:" + j +",filter_type:'autocomplete'},");
            //    }
            //    else
            //    {
            //        src.Write("{column_number:" + j + "}]); });");
            //    }
            //}
            src.Close();
        
        }

        public ActionResult Show(string ODID = "3525000008-Libraries")
        {
            CreateJsFile(ODID);
            ViewBag.ODID = ODID;
            //StreamWriter scr = System.IO.File.CreateText(Server.MapPath("~/Scripts") + "\\test.js");
            //scr.Write("$(document).ready(function(){$('#userTable').dataTable({'sDom':'rt','bJQueryUI':true,'sPaginationType':'full_numbers','bProcessing':true,'bServerSide':true,'sAjaxSource':'/Table/GetDataTables','fnServerData':function(url,data,callback){$.ajax({'url':url,'data':data,'success':callback,'dataType':'json','type':'POST','cache':false,'error':function(){alert('DataTableswarning:JSONdatafromserverfailedtoloadorbeparsed.'+'ThisismostlikelytobecausedbyaJSONformattingerror.');}});}});});");
            //scr.Close();
            return View("show2");
        }


    }
}
