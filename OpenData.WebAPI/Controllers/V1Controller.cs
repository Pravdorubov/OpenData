using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OpenData.Domain.Abstract;
using OpenData.Domain.Entities;
using OpenData.WebAPI.Models;
using System.Web.Http.Description;
using Microsoft.Data.OData;
using System.Web.Http.OData.Query;


namespace OpenData.WebAPI.Controllers
{
    public class V1Controller : ApiController
    {
        private IODRepository repository;

        public V1Controller(IODRepository repo)
        {
            repository = repo;
        }

        [AcceptVerbs("GET")]
        [ActionName("List")]
        [Queryable]
        public IQueryable<DatasetBrief> List(/*ODataQueryOptions<DatasetBrief> options,*/ string format = "json")
        {

            IQueryable<DatasetBrief> ds = repository.OpenData.Select(ods =>

                        new DatasetBrief
                    {
                        ID = ods.ODID,
                        Description = ods.FullDescription,
                        Caption = ods.Description,
                        Owner = new _Authority
                        {
                            INN = ods.Authority.INN,
                            Name = ods.Authority.Name
                        },
                        Category = new Category
                        {
                            ID = ods.Category.ID,
                            Name = ods.Category.Name
                        },


                    });
            //DatasetsList query =
            //    new DatasetsList
            //    {
            //        Count = repository.OpenData.Count(),
            //        Datasets = options.ApplyTo(ds).Cast<DatasetBrief>()
            //    };
            return ds;

        }

        [AcceptVerbs("GET")]
        [ActionName("Passport")]
        public IQueryable<Dataset> Passport(string Id, string format = "json")
        {

            var query = repository.OpenData.Where(ods=>ods.ODID==Id).Select(ods =>
                new Dataset
                {
                    ID = ods.ODID,
                    Caption=ods.Description,
                    Description = ods.FullDescription,
                    Name = ods.Name,
                    Owner = new _Authority
                    {
                        INN = ods.Authority.INN,
                        Name = ods.Authority.Name
                    },
                    Category = new Category
                    {
                        ID = ods.Category.ID,
                        Name = ods.Category.Name
                    },
                    Responsible = new Employer
                    {
                        SNP = ods.Users.UserProfile.FNS,
                        Duty = ods.Users.UserProfile.Duty,
                        Phone = ods.Users.UserProfile.Phone,
                        Email = ods.Users.UserProfile.Email
                    }


                });
            return query;
        }



        [ApiExplorerSettings(IgnoreApi = true)]
        public dynamic Get()
        {
            return repository.OpenData.Select(ods => new { ODID = ods.ODID, Name = ods.Name });
        }

        //GET api/values/5
        [ApiExplorerSettings(IgnoreApi = true)]
        public string Get(int id)
        {
            return "value";
        }
    }
}
