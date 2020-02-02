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
using Cibertec.Mvc.Models;
//using SelectListItem = Cibertec.Models.SelectListItem;

namespace Cibertec.Mvc.Controllers
{
    [RoutePrefix("Reserva")]
    public class ReservaController : BaseController
    {
        public ReservaController(ILog log, IUnitOfWork unit) : base(log, unit)
        {
            //_unit = unit;
        }

        // GET: Customer
        public ActionResult Index()
        {
            _log.Info("Ejecución de Customer Controller Ok");
            return View(_unit.Reservas.GetList());
        }

        [Route("List/{page:int}/{rows:int}")]
        public PartialViewResult List(int page, int rows)
        {
            if (page <= 0 || rows <= 0) return PartialView(new List<Reserva>());
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
            var result = _unit.Reservas.PagedList(startRecord, endRecord);
            return PartialView("_List", result);
        }
        [Route("Count/{rows:int}")]
        public int Count(int rows)
        {
            var totalRecords = _unit.Reservas.Count();
            return totalRecords % rows != 0 ? (totalRecords / rows) + 1 : totalRecords / rows;
        }

        public PartialViewResult Create()
        {
            var vm = new ReservaModel();

            vm.Fecha_Fin = DateTime.Now;
            vm.Fecha_Inicio = DateTime.Now;
            //vm.Itinerario = new List<System.Web.Mvc.SelectListItem>
            //{
            //    new System.Web.Mvc.SelectListItem { Value = "1", Text = "LIMA-ICA" },
            //    new System.Web.Mvc.SelectListItem { Value = "2", Text = "LIMA-TACNA" }
            //};

            var res = _unit.Itinerarios.GetListItinerarios().Select(item => new SelectListItem()
            {
                Text = item.Descripcion,
                Value = item.IdItinerario.ToString()
            });

            ViewData["SomeList"] = _unit.Itinerarios.GetListItinerarios();

            vm.Itinerario = new List<System.Web.Mvc.SelectListItem>(res);
            return PartialView("_Create", vm);
        }

        [HttpPost]
        public ActionResult Create(ReservaModel EmpDet)    
        {
            var reserva = new Reserva()
            {
                Fecha_Inicio = EmpDet.Fecha_Fin,
                Fecha_Fin = EmpDet.Fecha_Inicio,
                IdItinerario = EmpDet.IdItinerario,
                IdCliente=EmpDet.IdCliente,
                Costo = EmpDet.Costo
            };
            var val = _unit.Reservas.GuardarReserva(reserva);

            if (val)
            {
                return RedirectToAction("Index");
                //return Json("Records added Successfully.");
            }

           return PartialView("_Reserva", EmpDet);
            //return Json("Records not added,");
        }
        [Route("ListItinerario/{rows:int}")]
        public string ListItinerario(int rows)    
        {
            var val = _unit.Itinerarios.GetById(rows);
            //var lista = new List<Itinerario>()
            //{
            //  new Itinerario()
            //  {
            //      Costo=250,
            //      Descripcion ="Lima-Ica" +"_" + "250",
            //      IdItinerario =1
            //  },
            //  new Itinerario()
            //  {
            //      Costo=280,
            //      Descripcion ="Lima-Tacna" +"_" + "280",
            //      IdItinerario =2
            //  }
            //};
            return Newtonsoft.Json.JsonConvert.SerializeObject(val);

        }
        

        //public ActionResult Update(string id)
        public PartialViewResult Update(int id)
        {
            //return View(_unit.Customers.GetById(id));
            var result = _unit.Reservas.GetByIdReserva(id);

            var vm = new ReservaModel();    
            vm.NombreCliente = result.Nombres;
            vm.ApellidoCliente = result.Apellidos;
            vm.Fecha_Inicio = result.Fecha_Inicio;
            vm.IdCliente = result.IdCliente;
            vm.Fecha_Fin = result.Fecha_Fin;
           
            vm.IdReserva = result.IdReserva;
            //vm.Itinerario = new List<System.Web.Mvc.SelectListItem>
            //{
            //    new System.Web.Mvc.SelectListItem { Value = "1", Text = "LIMA-ICA" },
            //    new System.Web.Mvc.SelectListItem { Value = "2", Text = "LIMA-TACNA" }
            //};

            var res = _unit.Itinerarios.GetListItinerarios().Select(item => new SelectListItem()
            {
                Text = item.Descripcion,
                Value = item.IdItinerario.ToString()

            });
            vm.Itinerario = new List<System.Web.Mvc.SelectListItem>(res);
            return PartialView("_Update", vm);
        }

        [HttpPost]
        public ActionResult Update(ReservaModel reservaModal) 
        {


            var reserva = new Reserva()
            {
                Fecha_Inicio = reservaModal.Fecha_Inicio,
                Fecha_Fin = reservaModal.Fecha_Fin,
                IdItinerario = reservaModal.IdItinerario,
                IdCliente = reservaModal.IdCliente,
                Costo = reservaModal.Costo,
                Nombres = reservaModal.NombreCliente,
                Apellidos= reservaModal.ApellidoCliente,
                IdReserva=reservaModal.IdReserva
            };
            var val = _unit.Reservas.ActualizarReserva(reserva);
            if (val)
            {
                return RedirectToAction("Index");
            }
            //return View(customer);
            return PartialView("_Update", reserva);
        }

        //public ActionResult Delete(String id)
        public PartialViewResult Delete(int id)
        {
            //return View(_unit.Customers.GetById(id));
            var result = _unit.Reservas.GetByIdReserva(id);
            return PartialView("_Delete", result);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            var val = _unit.Reservas.DeleteReserva(id);

            if (val) return RedirectToAction("Index");
            //return View();
            return PartialView("_Delete", _unit.Reservas.GetByIdReserva(id));
        }
    }
}