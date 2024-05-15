using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using IncidenciasUdec.Models;
using IncidenciasUdec.Permisos;
using IncidenciasUdec.Servicios;

namespace IncidenciasUdec.Controllers
{
    public class CrearEstudianteController : Controller
    {
        private REPORTE_UDECEntities db = new REPORTE_UDECEntities();


        // GET: CrearEstudiante/Create
        public ActionResult Create()
        {
            ViewBag.ID_PROGRAMA = new SelectList(db.PROGRAMA, "ID", "NOMBRE");
            return View();
        }

        // POST: CrearEstudiante/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NOMBRE,APELLIDO,EMAIL,PASSWORD,ConfirmarPassword,TELEFONO,SEMESTRE,USER_CONFIRMADO,TOKEN,RESTABLECER_PASS,ID_PROGRAMA")] USUARIO uSUARIO)
        {
            try
            {
                var existeUsuario = db.USUARIO.Any(e => e.EMAIL == uSUARIO.EMAIL);
                if (!existeUsuario)
                {
                    if (ValidarCorreo(uSUARIO.EMAIL) == true)
                    {
                        if (uSUARIO.PASSWORD == uSUARIO.ConfirmarPassword)
                        {
                            if (ModelState.IsValid)
                            {
                                uSUARIO.PASSWORD = Utilidades.EncriptarContraseña(uSUARIO.PASSWORD);
                                uSUARIO.TOKEN = Utilidades.GenerarToken();
                                uSUARIO.RESTABLECER_PASS = false;
                                uSUARIO.USER_CONFIRMADO = false;
                                db.USUARIO.Add(uSUARIO);
                                int filasAfectadas = db.SaveChanges();
                                if (filasAfectadas > 0)
                                {
                                    string ruta = HttpContext.Server.MapPath("~/OtrasPlantillas/ConfirmarCorreo.html");
                                    string contenido = System.IO.File.ReadAllText(ruta);
                                    string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Headers["host"], "/CrearEstudiante/Confirmar?token=" + uSUARIO.TOKEN);
                                    string cuerpoHtml = string.Format(contenido, uSUARIO.NOMBRE, url);
                                    Correo correo = new Correo()
                                    {
                                        Para = uSUARIO.EMAIL,
                                        Asunto = "Confirmación de correo",
                                        Contenido = cuerpoHtml
                                    };

                                    bool enviado = CorreoServicio.Enviar(correo);
                                    if (enviado)
                                    {
                                        ViewBag.Mensaje = $"Su cuenta ha sido creada, hemos enviado un mensaje al correo {uSUARIO.EMAIL} para su confirmación";
                                    }
                                }
                                else
                                {
                                    ViewBag.Mensaje = "No ha sido posible crear el usuario";
                                }
                            }
                        }
                        else
                        {
                            ViewBag.Mensaje = "Las contraseñas no coinciden";
                        }
                    }
                    else
                    {
                        ViewBag.Mensaje = "Dominio de correo no valido";
                    }
                }
                else
                {
                    ViewBag.Mensaje = "El correo digitado ya esxiste";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            ViewBag.ID_PROGRAMA = new SelectList(db.PROGRAMA, "ID", "NOMBRE", uSUARIO.ID_PROGRAMA);
            return View(uSUARIO);
        }

        public ActionResult Confirmar(string token)
        {
            ViewBag.Respuesta = DBUsuario.Confirmar(token);
            return View();
        }
        public static bool ValidarCorreo(string correo)
        {
            string patron = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$";
            if (Regex.IsMatch(correo, patron))
            {
                if (correo.EndsWith("@ucundinamarca.edu.co"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
