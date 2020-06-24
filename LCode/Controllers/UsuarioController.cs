using LCode.Dados;
using LCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LCode.Controllers
{
    public class UsuarioController : Controller
    {
        UsuarioAcoes ua = new UsuarioAcoes();

        [HttpGet]
        public ActionResult Perfil()
        {
            if(Session["Adm"] != null || Session["Professor"] != null || Session["Estudante"] != null)
            {
               int usu_id = Convert.ToInt32(Session["UsuId"]);
               var retorno = ua.GetUsuarios(usu_id);
            
               return View(retorno);

            }

            else
            {
                return RedirectToAction("Login", "Autenticacao");
            }
        }

        [HttpPost]
        public ActionResult Editar(Usuarios u)
        {
            
            ua.EditaUsuario(u);
            
            return View();
        }

    }
}