using IncidenciasUdec.Permisos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncidenciasUdec.Controllers
{
    [ValidarSession]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Reporte()
        {
            return View();
        }

        public ActionResult RegistroUsuario()
        {
       

            return View();

            //Toda la logica va aqui

        }

        public ActionResult Login()
        {
            return View();

            //El dato -> user 


        }

        public ActionResult CerrarSesion()
        {
            Session["usuario"] = null;
            return RedirectToAction("Login", "Login");
        }
    }
}