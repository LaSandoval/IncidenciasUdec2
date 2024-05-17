using System;
using System.Linq;
using System.Web.Mvc;
using IncidenciasUdec.Models;
using System.Data.Entity;

namespace IncidenciasUdec.Controllers
{
    public class EmpleadoController : Controller
    {
        private REPORTE_UDECEntities db = new REPORTE_UDECEntities();

        public ActionResult Asignacion(string nombre = "")
        {
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
        public ActionResult VerReporte(int idReporte, string nuevoEstado)
        {
            var reporte = db.REPORTE.Find(idReporte);
            if (reporte == null)
            {
                return HttpNotFound();
            }

            // Buscar el estado correspondiente en la base de datos
            var estadoFinalizado = db.ESTADO.FirstOrDefault(e => e.ID == 3);
            if (estadoFinalizado == null)
            {
                // Manejar el caso en que el estado no exista
                return HttpNotFound();
            }

            // Actualizar el estado del reporte al estado "FINALIZADO"
            reporte.ID_ESTADO = estadoFinalizado.ID;

            // Guardar los cambios en la base de datos
            db.SaveChanges();

            // Redirigir a la vista del reporte actualizado
            return RedirectToAction("VerReporte", new { idReporte = reporte.ID });
        }


    }
}




