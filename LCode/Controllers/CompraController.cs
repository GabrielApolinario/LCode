using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LCode.Dados;
using LCode.Models;
using LCode.ViewModels;

namespace LCode.Controllers
{
    public class CompraController : Controller
    {
        // GET: Compra
        CompraAcoes ca = new CompraAcoes();
        public ActionResult Carrinho()
        {

            if(Session["carrinho"] == null)
            {
                ViewBag.carrinhoVazio = "Você não adicionou nenhum curso ao carrinho";
                return View();
            }

            List<Curso> cursosCarrinho = new List<Curso>();
            Curso retorno = new Curso();
            
            foreach(var item in (List<Curso>)Session["carrinho"]) 
            {
                retorno = ca.CursoCarrinho(item.Curso_id);
                
                var curso = retorno;

                cursosCarrinho.Add(new Curso()
                {
                    Curso_id = Convert.ToInt32(curso.Curso_id),
                    Curso_nome = curso.Curso_nome,
                    //Curso_descricao = curso.Curso_descricao,
                    Curso_valor = curso.Curso_valor

                });

                ViewBag.total = cursosCarrinho.Sum(c => c.Curso_valor);
                ViewBag.qtde = cursosCarrinho.Count();
            }
            return View(cursosCarrinho);
        }

        public ActionResult AddCarrinho(int curso_id)
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

            return RedirectToAction("Carrinho");
        }

        public ActionResult RemoveCarrinho(int curso_id)
        {
            List<Curso> carrinho = (List<Curso>)Session["carrinho"];
            var curso = curso_id;
            carrinho.Remove(new Curso()
            {
                Curso_id = Convert.ToInt32(curso)
            });
            Session["carrinho"] = carrinho;

            return RedirectToAction("Carrinho");
        }


        public ActionResult Compra()
        {
            if (Session["Adm"] != null || Session["Professor"] != null || Session["Estudante"] != null)
                return View();
            else 
                return RedirectToAction("Login", "Autenticacao");

        }
    }
}