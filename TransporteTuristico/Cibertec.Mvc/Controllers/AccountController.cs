using Cibertec.Models;
using Cibertec.Mvc.Models;
using Cibertec.UnitOfWork;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Cibertec.Mvc.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(ILog log, IUnitOfWork unit): base(log, unit)
        {
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return View(new UserViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(UserViewModel user)
        {
            if (!ModelState.IsValid) return View(user);
            var validUser = _unit.Users.ValidateUser(user.Email, user.Password);
            if(validUser == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(user);
            }
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, validUser.Email),
                new Claim(ClaimTypes.Name, $"{validUser.Nombres} {validUser.Apellidos}"),
                new Claim(ClaimTypes.NameIdentifier, validUser.Email)
            },
            "ApplicationCookie");

            var context = Request.GetOwinContext();
            var authManager = context.Authentication;
            authManager.SignIn(identity);
            return RedirectToLocal(user.ReturnUrl);
        }

        public ActionResult Logout()
        {
            var context = Request.GetOwinContext();
            var authManager = context.Authentication;
            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(RegisterUserViewModel userView)
        {
            if (!ModelState.IsValid) return View(userView);

            Usuario user = new Usuario
            {
                Email = userView.Email,
                IdColaborador = userView.IdColaborador,
                Password = userView.Password
            };
            var validUser = _unit.Users.CreateUser(user);
            if(validUser == null)
            {
                ModelState.AddModelError("Error", "No se pudo crear el usuario");
                return View(user);
            }
            /*sign in*/
            /*
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, validUser.Email),
                new Claim(ClaimTypes.Name, $"{validUser.FirstName} {validUser.LastName}"),
                new Claim(ClaimTypes.NameIdentifier, validUser.Email)
            },
            "ApplicationCookie");

            var context = Request.GetOwinContext();
            var authManager = context.Authentication;
            authManager.SignIn(identity);
            */

            /*redireccionar al login*/
            return RedirectToAction("Login", "Account");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}