﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using EMC.SPaaS.Repository;
using EMC.SPaaS.Entities;
using Microsoft.AspNet.Authorization;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EMC.SPaaS.Manager.Controllers
{
    [Route("api/[controller]")]
    public class InstanceController : Controller
    {
        private RepositoryManager Repositories { get; set; }

        public InstanceController(SPaaSDbContext dbContext)
        {
            Repositories = new RepositoryManager(dbContext);
        }

        // GET: api/values
        [HttpGet]
        [Authorize]
        public JsonResult Get()
        {
            var sUserId = User.FindAll(Constants.AuthenticationSession.Properties.UserId).FirstOrDefault().Value;
            int userId = int.Parse(sUserId);

            var instances = Repositories.Instances.GetInstancesForUser(userId);

            return new JsonResult(instances);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Authorize]
        public JsonResult Get(int id)
        {
            var sUserId = User.FindAll(Constants.AuthenticationSession.Properties.UserId).FirstOrDefault().Value;
            int userId = int.Parse(sUserId);

            var instance = Repositories.Instances.GetInstance(id, userId);

            return new JsonResult(instance);
        }

        // POST api/values
        [HttpPost("{id}/{jobType}")]
        [Authorize]
        public void Post(int id, int jobType)
        {
            JobType type = (JobType)jobType;

            var sUserId = User.FindAll(Constants.AuthenticationSession.Properties.UserId).FirstOrDefault().Value;
            int userId = int.Parse(sUserId);

            var instance = Repositories.Instances.GetInstance(id, userId);
            Repositories.Instances.CreateJob(instance, type, userId);

            Repositories.Save();
        }

        [HttpPost]
        [Authorize]
        public void Post([FromBody]JObject itemDesign)
        {
            var sUserId = User.FindAll(Constants.AuthenticationSession.Properties.UserId).FirstOrDefault().Value;
            int userId = int.Parse(sUserId);
            int designId = int.Parse(itemDesign["designId"].ToString());
            string name = itemDesign["name"].ToString();

            //var design = Repositories.Designs.Find(designId);

            var instance = new InstanceEntity {
                DesignId = designId,
                Name = name,
                StatusId = (int)InstanceStatus.NotProvisioned,
                UserId = userId
            };

            Repositories.Instances.Create(instance);
            Repositories.Save();

            Repositories.Instances.Provision(instance, userId);
            Repositories.Save();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var sUserId = User.FindAll(Constants.AuthenticationSession.Properties.UserId).FirstOrDefault().Value;
            int userId = int.Parse(sUserId);

            var instance = Repositories.Instances.GetInstance(id, userId);
            Repositories.Instances.Delete(instance);
            Repositories.Save();
        }
    }
}
