using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IncidenciasUdec.Models;
using IncidenciasUdec.Permisos;

namespace IncidenciasUdec.Controllers
{
    [ValidarSession]
    public class HistorialController : Controller
    {
        private REPORTE_UDECEntities db = new REPORTE_UDECEntities();

        // GET: REPORTEs1
        public ActionResult Index(string searchString)
        {
            var reportes = db.REPORTE.Include(r => r.UBICACION).Include(r => r.TIPO_DAÑO).Include(r => r.CLASIFICACION);

            if (!String.IsNullOrEmpty(searchString))
            {
                reportes = reportes.Where(s => s.UBICACION.NOMBRE.Contains(searchString));
            }

            return View(reportes.ToList());
        }

        // GET: REPORTEs1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REPORTE rEPORTE = db.REPORTE.Find(id);
            if (rEPORTE == null)
            {
                return HttpNotFound();
            }
            return View(rEPORTE);
        }

     
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NOMBRE,UBICACION,TIPO_DAÑO,DESCRIPCION,IMAGEN,CLASIFICACION")] REPORTE rEPORTE)
        {
            if (ModelState.IsValid)
            {
                db.REPORTE.Add(rEPORTE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rEPORTE);
        }

        // GET: REPORTEs1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REPORTE rEPORTE = db.REPORTE.Find(id);
            if (rEPORTE == null)
            {
                return HttpNotFound();
            }
            return View(rEPORTE);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NOMBRE,UBICACION,TIPO_DAÑO,DESCRIPCION,IMAGEN,CLASIFICACION")] REPORTE rEPORTE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rEPORTE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rEPORTE);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REPORTE rEPORTE = db.REPORTE.Find(id);
            if (rEPORTE == null)
            {
                return HttpNotFound();
            }
            return View(rEPORTE);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            REPORTE rEPORTE = db.REPORTE.Find(id);
            db.REPORTE.Remove(rEPORTE);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
