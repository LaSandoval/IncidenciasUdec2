using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IncidenciasUdec.Models;
using IncidenciasUdec.Servicios;
using System.Data.Entity;
using System.IO;


namespace IncidenciasUdec.Controllers
{
    public class EmpleadoController : Controller
    {
        private REPORTE_UDECEntities db = new REPORTE_UDECEntities();
        public ActionResult Asignacion(string nombre = "") {
            var listaReportesAsignados = from r in db.REPORTE
                                         join u in db.USUARIO on r.ID_USUARIOASIGNACION equals u.ID
                                         join e in db.ESTADO on r.ID_ESTADO equals e.ID
                                         where r.ID_ESTADO == 2 && (string.IsNullOrEmpty(nombre) || u.NOMBRE.Contains(nombre))
                                         select new ViewModelReportesAsignados
                                         {
                                             IdReporte = r.ID,
                                             NombreReporte = r.NOMBRE,
                                             NombreColaboradorAsignado = u.NOMBRE,
                                             Estado = e.NOMBRE
                                         };
            return View(listaReportesAsignados.ToList());
        }

            public ActionResult VerReporte(int idReporte)
            {
                
                var reporte = db.REPORTE
                                .Include(r => r.UBICACION)
                                .Include(r => r.TIPO_DAÑO)
                                .Include(r => r.CLASIFICACION)
                                .SingleOrDefault(r => r.ID == idReporte);

                if (reporte == null)
                {
                    return HttpNotFound();
                }

                return View(reporte);
            }

        [HttpPost]
        public ActionResult VerReporte(int idReporte, string estado, string motivo = "")
        {
            var reporte = db.REPORTE.Find(idReporte);
            if (reporte == null)
            {
                return HttpNotFound();
            }

            reporte.ESTADO = estado;

            db.SaveChanges();

            return RedirectToAction("VerReporte", new { idReporte = reporte.ID });
        }

    }
}
       



