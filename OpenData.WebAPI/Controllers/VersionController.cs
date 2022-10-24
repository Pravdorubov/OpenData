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


namespace OpenData.WebAPI.Controllers
{
    public class VersionController : ApiController
    {
        private IODRepository repository;
        public struct Versions
        {
            public int Version { get; set; }
        }

        public VersionController(IODRepository repo)
        {
            repository = repo;
        }
        
        [ApiExplorerSettings(IgnoreApi = true)]
        public Versions Get()
        {

            return new Versions { Version = 1 };
        }
    }
}
