using IncidenciasUdec.Models;
using IncidenciasUdec.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;


namespace IncidenciasUdec.Controllers
{
    public class LoginController : Controller
    {
        private REPORTE_UDECEntities db = new REPORTE_UDECEntities();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string pass)
        {

            var passEncriptada = Utilidades.EncriptarContraseña(pass);
            var usuario = db.USUARIO.FirstOrDefault(u => u.EMAIL == email && u.PASSWORD == passEncriptada);

            if (usuario != null)
            {
                if ((bool)!usuario.USER_CONFIRMADO)
                {
                    TempData["Mensaje"] = $"Debe confirmar su cuenta, se ha enviado un correo a {usuario.EMAIL}";
                }
                else if ((bool)usuario.RESTABLECER_PASS)
                {
                    TempData["Mensaje"] = $"Se ha solicitado restablecer su contraseña, se ha enviado un correo a {usuario.EMAIL}";
                }
                else
                {
                    Session["usuario"] = usuario;
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["Email"] = email;
                TempData["Mensaje"] = "Correo y/o contraseña no son validos";
            }

            return View();
        }
        public ActionResult RestablecerPass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RestablecerPass(string email)
        {
            USUARIO usuario = DBUsuario.ObtenerUsuario(email);
            ViewBag.CorreoRestablecer = email;
            if (usuario != null)
            {
                bool respuesta = DBUsuario.ActualizarContraseña(1, usuario.PASSWORD, usuario.TOKEN);
                if (respuesta)
                {
                    string ruta = HttpContext.Server.MapPath("~/OtrasPlantillas/Restablecer.html");
                    string contenido = System.IO.File.ReadAllText(ruta);
                    string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Headers["host"], "/Login/Actualizar?token=" + usuario.TOKEN);
                    string cuerpoHtml = string.Format(contenido, usuario.NOMBRE, url);
                    Correo correo = new Correo()
                    {
                        Para = email,
                        Asunto = "Restablecer contraseña",
                        Contenido = cuerpoHtml
                    };

                    bool enviado = CorreoServicio.Enviar(correo);
                    if (enviado)
                    {
                        ViewBag.Restablecido = true;
                    }
                }
            }
            else
            {
                ViewBag.Mensaje = "No hay coincidencias";
            }
            return View();
        }

        public ActionResult Actualizar(string token)
        {
            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public ActionResult Actualizar(string token, string pass, string confPass)
        {
            ViewBag.Token = token;
            if (pass != confPass)
            {
                ViewBag.Mensaje = "Las contraseñas no coinciden";
                return View();
            }
            bool respuesta = DBUsuario.ActualizarContraseña(0, Utilidades.EncriptarContraseña(pass), token);
            if (respuesta)
            {
                ViewBag.Restablecido = true;
            }
            else
            {
                ViewBag.Mensaje = "No se pudo actualizar la contraseña";
            }
            return View();
        }

    }
}