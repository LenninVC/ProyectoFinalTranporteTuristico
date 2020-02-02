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
    [RoutePrefix("Colaborador")]
    public class ColaboradorController : BaseController
    {
        // GET: Colaborador
        public ColaboradorController(ILog log, IUnitOfWork unit) : base(log, unit)
        {
            //_unit = unit;
        }

        public ActionResult Error()
        {
            throw new System.Exception("Prueba de Validación de Error - Action Filter");
        }

        public ActionResult Index()
        {
            _log.Info("Ejecución de Colaborador Controller Ok");
            return View(_unit.Colaboradores.GetList());
        }

        public PartialViewResult Create()
        {
            //return View();
            return PartialView("_Create", new Colaborador());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Colaborador colaborador)
        {
            if (ModelState.IsValid)
            {
                _unit.Colaboradores.Insert(colaborador);
                return RedirectToAction("Index");
            }
            //return View(customer);
            return PartialView("_Create", colaborador);
        }


        public PartialViewResult Update(int id)
        {
            //return View(_unit.Customers.GetById(id));
            return PartialView("_Update", _unit.Colaboradores.GetById(id));
        }

        [HttpPost]
        public ActionResult Update(Colaborador colaborador)
        {
            var val = _unit.Colaboradores.Update(colaborador);

            if (val)
            {
                return RedirectToAction("Index");
            }
            //return View(customer);
            return PartialView("_Update", colaborador);
        }

        public PartialViewResult Delete(int id)
        {
            //return View(_unit.Customers.GetById(id));
            return PartialView("_Delete", _unit.Colaboradores.GetById(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            var val = _unit.Colaboradores.Delete(id);

            if (val) return RedirectToAction("Index");
            //return View();
            return PartialView("_Delete", _unit.Colaboradores.GetById(id));
        }



        [Route("List/{page:int}/{rows:int}")]
        public PartialViewResult List(int page, int rows)
        {
            if (page <= 0 || rows <= 0) return PartialView(new List<Colaborador>());
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
            return PartialView("_List", _unit.Colaboradores.PagedList(startRecord, endRecord));
        }

        [Route("Count/{rows:int}")]
        public int Count(int rows)
        {
            var totalRecords = _unit.Colaboradores.Count();
            return totalRecords % rows != 0 ? (totalRecords / rows) + 1 : totalRecords / rows;
        }
    }

}
