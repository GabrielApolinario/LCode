using LCode.Dados;
using LCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LCode.Controllers
{
    public class HomeController : Controller
    {
        BancoDeDados bd = new BancoDeDados();
        UsuarioAcoes ua = new UsuarioAcoes();
        public ActionResult Index()
        {
            var retorno = bd.BuscaCategorias();
            

            return View(retorno);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Dashboard()
        {
            if (Session["Adm"] != null || Session["Professor"] != null || Session["Estudante"] != null)
            {
               var retorno = ua.GetUsuarios(Convert.ToInt32(Session["UsuId"]));
                return View(retorno);
            }

            return RedirectToAction("Login", "Autenticacao");
        }

        public ActionResult Contato()
        {
            return View();
        }

        public ActionResult Sobre()
        {
            return View();
        }

    }
}