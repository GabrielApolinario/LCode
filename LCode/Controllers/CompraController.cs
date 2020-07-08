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
                ViewBag.qtdeProdutos = cursosCarrinho.Count();
                ViewBag.total = cursosCarrinho.Sum(c => c.Curso_valor);
                if (cursosCarrinho.Count() < 1)
                    ViewBag.qtde = null;
                else if (cursosCarrinho.Count() == 1)   
                    ViewBag.qtde = cursosCarrinho.Count() + " Curso";
                else ViewBag.qtde = cursosCarrinho.Count() + " Cursos";
            }
            return View(cursosCarrinho);
        }

        public ActionResult AddCarrinho(int curso_id)
        {
            if (Session["carrinho"] == null)
            {
                List<Curso> carrinho = new List<Curso>();               
                carrinho.Add(new Curso()
                {
                    Curso_id = Convert.ToInt32(curso_id)

                });
                Session["carrinho"] = carrinho;
                
            }
            else
            {
                List<Curso> carrinho = (List<Curso>)Session["carrinho"];              
                if (carrinho.Where(c => c.Curso_id == curso_id) != null)
                {
                    TempData["cursoJaEstaNoCarrinho"] = "O curso selecionado já foi adicionado anteriormente ao carrinho!";
                    return RedirectToAction("Carrinho");
                }
                else
                {
                    carrinho.Add(new Curso()
                    {
                    Curso_id = Convert.ToInt32(curso_id)
                    });
                    Session["carrinho"] = carrinho;
                }
            }

            return RedirectToAction("Carrinho");
        }

        public ActionResult RemoveCarrinho(int curso_id)
        {
            List<Curso> carrinho = (List<Curso>)Session["carrinho"];          

            carrinho.RemoveAll(c => c.Curso_id == curso_id);
          
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