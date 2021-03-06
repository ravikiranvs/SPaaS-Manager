﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EMC.SPaaS.ProvisioningEngine;
using Microsoft.AspNet.Authorization;
using EMC.SPaaS.Entities;

namespace EMC.SPaaS.Manager.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private ProvisionerFactory ProvisioningFactory
        {
            get; set;
        }

        private SPaaSDbContext DbContext { get; set; }

        public ValuesController(ProvisionerFactory provisioningFactory, SPaaSDbContext dbContext)
        {
            this.ProvisioningFactory = provisioningFactory;
            this.DbContext = dbContext;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Authorize]
        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
