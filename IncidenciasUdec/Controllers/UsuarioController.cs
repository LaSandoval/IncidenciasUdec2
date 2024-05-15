using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IncidenciasUdec.Models;

namespace IncidenciasUdec.Controllers
{
    public class UsuarioController : Controller
    {
        private REPORTE_UDECEntities db = new REPORTE_UDECEntities();
     
        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View();
        }
    }
}
