using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using BeaconWebApplication.Models;
using BeaconWebApplication.ViewModels;
using AutoMapper;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Sakura.AspNet;

namespace BeaconWebApplication.Controllers.Web.AppController
{
    public class AppController : Controller
    {
        private IBeaconsRepository _repository;

        public AppController(IBeaconsRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index(int page=1)
        {
            var beaons = _repository.GetAllBeacons();
            var pageSize = 10;
            var pagedBeacons = beaons.ToPagedList(pageSize, page);

            return View(pagedBeacons);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BeaconViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newBeacon = Mapper.Map<Beacon>(vm);

                // save to database
                _repository.AddBeacon(newBeacon);
            }
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            Beacon beacon = _repository.GetBeaconById(id);
            var vm = Mapper.Map<BeaconViewModel>(beacon);
         
            if (beacon == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(BeaconViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var beaconToUpdate = Mapper.Map<Beacon>(vm);

                // save to database
                _repository.Edit(beaconToUpdate);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            Beacon beacon = _repository.GetBeaconById(id);
            if (beacon == null)
            {
                return HttpNotFound();
            }

            _repository.Delete(beacon);

            return RedirectToAction("Index");
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            Beacon beacon = _repository.GetBeaconById(id);
            if (beacon == null)
            {
                return HttpNotFound();
            }
            var vm = Mapper.Map<BeaconViewModel>(beacon);
            return View(vm);
        }


        public IActionResult Logs(int page=1)
        {
            var logs = _repository.GetAllLogs();
            var logsVm = Mapper.Map<IEnumerable<LogViewModel>>(logs);

            //ar pageNumber = 1; // Note that page number starts from 1 (not zero!)
            var pageSize = 20;

            var pagedLogs = logsVm.ToPagedList(pageSize, page);

            return View(pagedLogs);
        }
    }
}
