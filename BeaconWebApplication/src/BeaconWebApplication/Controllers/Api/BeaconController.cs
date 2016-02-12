using BeaconWebApplication.Models;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace BeaconWebApplication.Controllers.Api
{
    public class BeaconController : ApiController
    {
        private IBeaconsRepository _repository;

        public BeaconController(IBeaconsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("api/message")]
        public JsonResult Get(string uuid, string major, string minor, string proximity)
        {
            return Json(new { message = _repository.GetMessageByUuidAndMajorAndMinorAndProximity(uuid, major,minor, proximity) });
        }
    }
}
