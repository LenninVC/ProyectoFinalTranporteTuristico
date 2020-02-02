using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cibertec.Repositories.Dapper.NorthWind;
using Cibertec.UnitOfWork;
using System.Configuration;
using Cibertec.Models;
using log4net;
using Cibertec.Mvc.ActionFilters;

namespace Cibertec.Mvc.Controllers
{
    [RoutePrefix("Itinerario")]
    public class ItinerarioController : BaseController
    {
        // GET: Itinerario
        public ItinerarioController(ILog log, IUnitOfWork unit) : base(log, unit)
        {
            //_unit = unit;
        }
        public ActionResult Error()
        {
            throw new System.Exception("Prueba de Validación de Error - Action Filter");
        }
        public ActionResult Index()
        {
            _log.Info("Ejecución de Itinerario Controller Ok");
            //return View(_unit.Itinerarios.GetList());
            return View(_unit.Itinerarios.GetListItinerarios());
            
        }

        [Route("List/{page:int}/{rows:int}")]
        public PartialViewResult List(int page, int rows)
        {
            if (page <= 0 || rows <= 0) return PartialView(new List<Itinerario>());
            var startRecord = ((page - 1) * rows) + 1;
            var endRecord = page * rows;

            /*
             * Llamando a un WEB API
             solicitar token:
             var token = llamada al servicio(userName,password,grant_type);
             consultar servicio:
             List<Customers> lstCustomers = llamada al servicio(page,rows,token);
              return PartialView("_List", lstCustomers)
             */
            return PartialView("_List", _unit.Itinerarios.PagedList(startRecord, endRecord));
        }

        public PartialViewResult Create()
        {
            //return View();
            return PartialView("_Create", new Itinerario());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Itinerario itinerario)
        {
            if (ModelState.IsValid)
            {
                _unit.Itinerarios.Insert(itinerario);
                return RedirectToAction("Index");
            }
            //return View(customer);
            return PartialView("_Create", itinerario);
        }

        public PartialViewResult Update(int id)
        {
            //return View(_unit.Customers.GetById(id));
            return PartialView("_Update", _unit.Itinerarios.GetById(id));
        }

        [HttpPost]
        public ActionResult Update(Itinerario itinerario)
        {
            var val = _unit.Itinerarios.Update(itinerario);

            if (val)
            {
                return RedirectToAction("Index");
            }
            //return View(customer);
            return PartialView("_Update", itinerario);
        }

        public PartialViewResult Delete(int id)
        {
            //return View(_unit.Customers.GetById(id));
            return PartialView("_Delete", _unit.Itinerarios.GetById(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            var val = _unit.Itinerarios.Delete(id);

            if (val) return RedirectToAction("Index");
            //return View();
            return PartialView("_Delete", _unit.Itinerarios.GetById(id));
        }                   
        
        [Route("Count/{rows:int}")]
        public int Count(int rows)
        {
            var totalRecords = _unit.Itinerarios.Count();
            return totalRecords % rows != 0 ? (totalRecords / rows) + 1 : totalRecords / rows;
        }
     
    }
}