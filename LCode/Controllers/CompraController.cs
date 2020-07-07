using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LCode.Models;

namespace LCode.Controllers
{
    public class CompraController : Controller
    {
        // GET: Compra
        public ActionResult Carrinho(Nullable <int> curso_id)
        {

            return View();
        }

        public ActionResult AddCarrinho(Nullable <int> curso_id)
        {
            if (Session["carrinho"] == null)
            {
                List<Curso> carrinho = new List<Curso>();
                var curso = curso_id;
                carrinho.Add(new Curso()
                {
                    Curso_id = Convert.ToInt32(curso)

                });
                Session["carrinho"] = carrinho;

            }

            else
            {
                List<Curso> carrinho = (List<Curso>)Session["carrinho"];
                var curso = curso_id;
                carrinho.Add(new Curso()
                {
                    Curso_id = Convert.ToInt32(curso)
                });
                Session["carrinho"] = carrinho;
            }

            return View();
        }

        public ActionResult Compra()
        {
            return View();
        }
    }
}