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
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace Cibertec.Mvc.Controllers
{
    //[ErrorActionFilter]
    [RoutePrefix("Customer")]
    public class CustomerController : BaseController
    {
        /*Ya no es necesario porque se maneja en el padre BaseController*/
        //private readonly IUnitOfWork _unit;

        /*Ya no es necesario porque se utiliza Inyección de Dependencia*/
        /*
        public CustomerController()
        {
            _unit = new NorthwindUnitOfWork(
                ConfigurationManager.ConnectionStrings["NorthwindConnection"].ToString());
        }
        */
        public CustomerController(ILog log, IUnitOfWork unit) : base(log, unit)
        {
            //_unit = unit;
        }

        //Simulación de Error
        public ActionResult Error()
        {
            throw new System.Exception("Prueba de Validación de Error - Action Filter");
        }

        // GET: Customer
        public ActionResult Index()
        {
            _log.Info("Ejecución de Customer Controller Ok");
            return View(_unit.Clientes.GetList());
        }

        //CREATE: Customer
        //public ActionResult Create()
        public PartialViewResult Create()
        {
            //return View();
            return PartialView("_Create", new Cliente());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cliente customer)
        {
            if (ModelState.IsValid)
            {
                _unit.Clientes.Insert(customer);
                return RedirectToAction("Index");
            }
            //return View(customer);
            return PartialView("_Create", customer);
        }

        //public ActionResult Update(string id)
        public PartialViewResult Update(int id)
        {
            //return View(_unit.Customers.GetById(id));
            return PartialView("_Update", _unit.Clientes.GetById(id));
        }

        [HttpPost]
        public ActionResult Update(Cliente customer)
        {
            var val = _unit.Clientes.Update(customer);

            if (val)
            {
                return RedirectToAction("Index");
            }
            //return View(customer);
            return PartialView("_Update", customer);
        }

        //public ActionResult Delete(String id)
        public PartialViewResult Delete(int id)
        {
            //return View(_unit.Customers.GetById(id));
            return PartialView("_Delete", _unit.Clientes.GetById(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            var val = _unit.Clientes.Delete(id);

            if (val) return RedirectToAction("Index");
            //return View();
            return PartialView("_Delete", _unit.Clientes.GetById(id));
        }

        //[Route("List/{page:int}/{rows:int}")]
        //public PartialViewResult List(int page, int rows)
        //{
        //    if (page <= 0 || rows <= 0) return PartialView(new List<Cliente>());
        //    var startRecord = ((page - 1) * rows) + 1;
        //    var endRecord = page * rows;

        //    /*
        //     * Llamando a un WEB API
        //     solicitar token:
        //     var token = llamada al servicio(userName,password,grant_type);
        //     consultar servicio:
        //     List<Customers> lstCustomers = llamada al servicio(page,rows,token);
        //      return PartialView("_List", lstCustomers)
        //     */
        //    return PartialView("_List", _unit.Clientes.PagedList(startRecord, endRecord));
        //}

        [Route("List/{page:int}/{rows:int}")]
        public async Task<PartialViewResult> List(int page, int rows)
        {
            /*
             * Si no se consume el servicio:
             * 
            if (page <= 0 || rows <= 0) return PartialView(new List<Customers>());
            var startRecord = ((page - 1) * rows) + 1;
            var endRecord = page * rows;
            return PartialView("_List", _unit.Customers.PagedList(startRecord, endRecord));
            */

            /*Si se consume el servicio:*/
            //Paso 1: Solicitar token
            var httpClient = new HttpClient();
            var credential = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "username", "KATTY@gmail.com" },
                { "password", "123456" }
            };

            var token = await httpClient.PostAsync("http://localhost:55724/token",
                new FormUrlEncodedContent(credential));
            var tokenContent = token.Content.ReadAsStringAsync().Result;
            var tokenDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenContent);

            //Paso 2: Consumir Servicio
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                tokenDictionary["access_token"]);
            var json = await httpClient.GetStringAsync("http://localhost:55724/customer/list/" + page + "/" + rows);

            List<Cliente> lstCustomers = JsonConvert.DeserializeObject<List<Cliente>>(json);
            return PartialView("_List", lstCustomers);
        }
        [Route("Count/{rows:int}")]
        public int Count(int rows)
        {
            var totalRecords = _unit.Clientes.Count();
            return totalRecords % rows != 0 ? (totalRecords / rows) + 1 : totalRecords / rows;
        }

        //[Route("Count/{rows:int}")]
        //public int Count(int rows)
        //{
        //    var totalRecords = _unit.Clientes.Count();
        //    return totalRecords % rows != 0 ? (totalRecords / rows) + 1 : totalRecords / rows;
        //}

        [Route("BuscarCliente/{documento:int}")]
        public JsonResult BuscarCliente(int documento)
        {

            var val = _unit.Clientes.GetByDocumento(documento);
           
            return Json(val, JsonRequestBehavior.AllowGet);
            
        }
    }
}