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
using IncidenciasUdec.Servicios;

namespace IncidenciasUdec.Controllers
{
    [ValidarSession]
    public class ReportesAdminController : Controller
    {
        private REPORTE_UDECEntities db = new REPORTE_UDECEntities();

        public ActionResult VistaGeneralAdministrador()
        {
            return View();
        }
        // GET: ReportesAdmin
        public ActionResult Index(string filtro)
        {
            var rEPORTE = db.REPORTE.Include(r => r.CLASIFICACION).Include(r => r.ESTADO).Include(r => r.TIPO_DAÑO).Include(r => r.UBICACION).Include(r => r.USUARIO);
            if (!string.IsNullOrEmpty(filtro))
            {
                rEPORTE = db.REPORTE.Include(r => r.CLASIFICACION).Include(r => r.ESTADO).Include(r => r.TIPO_DAÑO).Include(r => r.UBICACION).Include(r => r.USUARIO).Where(r => r.ESTADO.NOMBRE.Contains(filtro));
            }

            ViewBag.Filtro = new SelectList(db.ESTADO, "ID", "NOMBRE");
            return View(rEPORTE.ToList());
        }

        public ActionResult VerReportesAsignados()
        {
            var listaReportesAsignados = from r in db.REPORTE
                                         join u in db.USUARIO on r.ID_USUARIOASIGNACION equals u.ID
                                         join e in db.ESTADO on r.ID_ESTADO equals e.ID
                                         where r.ID_ESTADO == 2
                                         select new ViewModelReportesAsignados
                                         {
                                             IdReporte = r.ID,
                                             NombreReporte = r.NOMBRE,
                                             NombreColaboradorAsignado = u.NOMBRE,
                                             Estado = e.NOMBRE
                                         };
            return View(listaReportesAsignados.ToList());
        }

        // GET: ReportesAdmin/Edit/5
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
            ViewBag.Imagen = rEPORTE.IMAGEN;
            ViewBag.ID_CLASIFICACION = new SelectList(db.CLASIFICACION, "ID", "NOMBRE", rEPORTE.ID_CLASIFICACION);
            ViewBag.ID_ESTADO = new SelectList(db.ESTADO, "ID", "NOMBRE", rEPORTE.ID_ESTADO);
            ViewBag.ID_TIPO_DAÑO = new SelectList(db.TIPO_DAÑO, "ID", "NOMBRE", rEPORTE.ID_TIPO_DAÑO);
            ViewBag.ID_UBICACION = new SelectList(db.UBICACION, "ID", "NOMBRE", rEPORTE.ID_UBICACION);
            ViewBag.ID_USUARIO = new SelectList(db.USUARIO, "ID", "NOMBRE", rEPORTE.ID_USUARIO);
            ViewBag.ID_USUARIOASIGNACION = new SelectList(db.USUARIO, "ID", "NOMBRE", rEPORTE.ID_USUARIOASIGNACION);
            var listaColaboradores = from C in db.USUARIO
                                     where (C.ID_PROGRAMA == null)
                                     orderby C.NOMBRE ascending
                                     select C;
            ViewBag.LISTA_COLABORADORES = new SelectList(listaColaboradores, "ID", "NOMBRE", rEPORTE.ID_USUARIOASIGNACION);

            return View(rEPORTE);
        }

        // POST: ReportesAdmin/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NOMBRE,ID_UBICACION,ID_TIPO_DAÑO,DESCRIPCION,ID_CLASIFICACION,ID_ESTADO,IMAGEN,ID_USUARIO,ID_USUARIOASIGNACION")] REPORTE rEPORTE)
        {
            if (ModelState.IsValid)
            {

                var idColaborador = Request.Form["LISTA_COLABORADORES"];
                if (!string.IsNullOrEmpty(idColaborador))
                {
                    int idColaboradorInt = int.Parse(idColaborador);
                    rEPORTE.ID_USUARIOASIGNACION = idColaboradorInt;
                    rEPORTE.ID_ESTADO = 2;
                }
                else
                {
                    rEPORTE.ID_USUARIOASIGNACION = 0;
                }

                
                db.Entry(rEPORTE).State = EntityState.Modified;
                int save = db.SaveChanges();
                var correoColaborador = db.USUARIO.Where(u => u.ID == rEPORTE.ID_USUARIOASIGNACION).Select(c => new
                {
                    c.EMAIL,
                    c.NOMBRE
                }).FirstOrDefault();


                if (save > 0 && idColaborador != "")
                {
                    string ruta = HttpContext.Server.MapPath("~/OtrasPlantillas/ReporteAsignado.html");
                    string contenido = System.IO.File.ReadAllText(ruta);
                    string cuerpoHtml = string.Format(contenido, correoColaborador.NOMBRE);
                    Correo correo = new Correo()
                    {
                        Para = correoColaborador.EMAIL,
                        Asunto = "Asignación de reporte",
                        Contenido = cuerpoHtml
                    };
                    bool enviado = CorreoServicio.Enviar(correo);
                    if (enviado)
                    {
                        TempData["Mensaje"] = $"Se ha asignado el reporte a: {correoColaborador.NOMBRE}";
                    }
                }
                return RedirectToAction("VerReportesAsignados", "ReportesAdmin");
            }
            ViewBag.ID_CLASIFICACION = new SelectList(db.CLASIFICACION, "ID", "NOMBRE", rEPORTE.ID_CLASIFICACION);
            ViewBag.ID_ESTADO = new SelectList(db.ESTADO, "ID", "NOMBRE", rEPORTE.ID_ESTADO);
            ViewBag.ID_TIPO_DAÑO = new SelectList(db.TIPO_DAÑO, "ID", "NOMBRE", rEPORTE.ID_TIPO_DAÑO);
            ViewBag.ID_UBICACION = new SelectList(db.UBICACION, "ID", "NOMBRE", rEPORTE.ID_UBICACION);
            ViewBag.ID_USUARIO = new SelectList(db.USUARIO, "ID", "NOMBRE", rEPORTE.ID_USUARIO);
            return View(rEPORTE);
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
