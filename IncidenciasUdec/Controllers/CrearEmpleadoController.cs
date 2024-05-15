using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IncidenciasUdec.Models;
using IncidenciasUdec.Permisos;

namespace IncidenciasUdec.Controllers
{
    [ValidarSession]
    public class CrearEmpleadoController : Controller
    {
        private REPORTE_UDECEntities db = new REPORTE_UDECEntities();

        // GET: CrearEmpleado
        public ActionResult Index()
        {
            var uSUARIO = db.USUARIO.Include(u => u.PROGRAMA).Where(u=>u.PROGRAMA.ID.Equals(null));
            return View(uSUARIO.ToList());
        }

        // GET: CrearEmpleado/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USUARIO uSUARIO = db.USUARIO.Find(id);
            if (uSUARIO == null)
            {
                return HttpNotFound();
            }
            return View(uSUARIO);
        }

        // GET: CrearEmpleado/Create
        public ActionResult Create()
        {
            ViewBag.ID_PROGRAMA = new SelectList(db.PROGRAMA, "ID", "NOMBRE");
            return View();
        }

        // POST: CrearEmpleado/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NOMBRE,APELLIDO,EMAIL,PASSWORD,TELEFONO,ID_PROGRAMA,SEMESTRE,RESTABLECER_PASS,USER_CONFIRMADO,TOKEN")] USUARIO uSUARIO)
        {
            if (ModelState.IsValid)
            {
                db.USUARIO.Add(uSUARIO);
                int save = db.SaveChanges();
                if(save > 1)
                {
                    TempData["Mensaje"] = $"Se ha creado el usuario {uSUARIO.NOMBRE}";
                }
                return RedirectToAction("Index");
            }

            ViewBag.ID_PROGRAMA = new SelectList(db.PROGRAMA, "ID", "NOMBRE", uSUARIO.ID_PROGRAMA);
            return View(uSUARIO);
        }

        // GET: CrearEmpleado/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USUARIO uSUARIO = db.USUARIO.Find(id);
            if (uSUARIO == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_PROGRAMA = new SelectList(db.PROGRAMA, "ID", "NOMBRE", uSUARIO.ID_PROGRAMA);
            return View(uSUARIO);
        }

        // POST: CrearEmpleado/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NOMBRE,APELLIDO,EMAIL,PASSWORD,TELEFONO,ID_PROGRAMA,SEMESTRE,RESTABLECER_PASS,USER_CONFIRMADO,TOKEN")] USUARIO uSUARIO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uSUARIO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_PROGRAMA = new SelectList(db.PROGRAMA, "ID", "NOMBRE", uSUARIO.ID_PROGRAMA);
            return View(uSUARIO);
        }

        // GET: CrearEmpleado/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USUARIO uSUARIO = db.USUARIO.Find(id);
            if (uSUARIO == null)
            {
                return HttpNotFound();
            }
            return View(uSUARIO);
        }

        // POST: CrearEmpleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            USUARIO uSUARIO = db.USUARIO.Find(id);
            db.USUARIO.Remove(uSUARIO);
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
