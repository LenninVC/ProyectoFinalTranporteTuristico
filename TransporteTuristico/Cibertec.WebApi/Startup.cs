using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Cibertec.WebApi.App_Start;
using Cibertec.WebApi.Handlers;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(Cibertec.WebApi.Startup))]

namespace Cibertec.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            /*Configuracion del log4net:*/
            log4net.Config.XmlConfigurator.Configure();
            var log = log4net.LogManager.GetLogger(typeof(Startup));
            log.Debug("Loggin habilitado");

            var config = new HttpConfiguration();
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());

            DIConfig.ConfigureInjector(config);
            //Permite el Cross Origin
            app.UseCors(CorsOptions.AllowAll);

            TokenConfig.ConfigureOAuth(app, config);
            RouteConfig.Register(config);
            WebApiConfig.Configure(config);
            app.UseWebApi(config);
        }
    }
}
