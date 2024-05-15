using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using IncidenciasUdec.Models;
using IncidenciasUdec.Permisos;

namespace IncidenciasUdec.Controllers
{
    [ValidarSession]
    public class ReportesController : Controller
    {
        private REPORTE_UDECEntities db = new REPORTE_UDECEntities();

        public ActionResult Create()
        {
            ViewBag.ID_USUARIO = new SelectList(db.USUARIO, "ID", "NOMBRE");
            ViewBag.ID_CLASIFICACION = new SelectList(db.CLASIFICACION, "ID", "NOMBRE");
            ViewBag.ID_TIPO_DAÑO = new SelectList(db.TIPO_DAÑO, "ID", "NOMBRE");
            ViewBag.ID_ESTADO = new SelectList(db.ESTADO, "ID", "NOMBRE");
            ViewBag.ID_UBICACION = new SelectList(db.UBICACION, "ID", "NOMBRE");
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,NOMBRE,DESCRIPCION,IMAGEN,ID_USUARIO,ID_TIPO_DAÑO,ID_CLASIFICACION,ID_ESTADO,ID_UBICACION")] REPORTE rEPORTE, HttpPostedFileBase imagen)
        {
            if (ModelState.IsValid)
            {
                if (imagen != null && imagen.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(imagen.FileName);
                    var path = Path.Combine(Server.MapPath("~/ImagenesReportes"), fileName);
                    imagen.SaveAs(path);
                    rEPORTE.IMAGEN = "/ImagenesReportes/" + fileName;
                }
                var usuarioSession = (USUARIO)Session["usuario"];
                rEPORTE.ID_USUARIO = usuarioSession.ID;
                rEPORTE.ID_ESTADO = 1;
                db.REPORTE.Add(rEPORTE);
                db.SaveChanges();
                ViewBag.Mensaje = "El reporte ha sido creado";
                return RedirectToAction("Create","Reportes");
            }

            ViewBag.ID_USUARIO = new SelectList(db.USUARIO, "ID", "NOMBRE", rEPORTE.ID_USUARIO);
            ViewBag.ID_CLASIFICACION = new SelectList(db.CLASIFICACION, "ID", "NOMBRE", rEPORTE.ID_CLASIFICACION);
            ViewBag.ID_TIPO_DAÑO = new SelectList(db.TIPO_DAÑO, "ID", "NOMBRE", rEPORTE.ID_TIPO_DAÑO);
            ViewBag.ID_ESTADO = new SelectList(db.ESTADO, "ID", "NOMBRE", rEPORTE.ID_ESTADO);
            ViewBag.ID_UBICACION = new SelectList(db.UBICACION, "ID", "NOMBRE", rEPORTE.ID_UBICACION);
            return View(rEPORTE);
        }
    }
}
