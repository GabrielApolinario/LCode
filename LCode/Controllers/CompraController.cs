using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                TempData["total"] = cursosCarrinho.Sum(c => c.Curso_valor);

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
                bool confereCurso = carrinho.Any(c => c.Curso_id.Equals(curso_id));

                if (confereCurso == true)
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


        public ActionResult Compra(Nullable<int> curso_id)
        {

            List<Curso> cursosCarrinho = new List<Curso>();
            Curso retorno = new Curso();

            if (Session["Adm"] != null || Session["Professor"] != null || Session["Estudante"] != null)
            {
                if (curso_id != null)
                {
                    retorno = ca.CursoCarrinho(Convert.ToInt32(curso_id));

                    var curso = retorno;

                    cursosCarrinho.Add(new Curso()
                    {
                        Curso_nome = curso.Curso_nome.ToString(),
                        Curso_descricao = curso.Curso_descricao.ToString(),
                        Curso_duracao = Convert.ToDouble(curso.Curso_duracao),
                        Curso_valor = Convert.ToDouble(curso.Curso_valor),
                        Curso_id = Convert.ToInt32(curso.Curso_id),

                    });

                    TempData["total"] = cursosCarrinho.Sum(c => c.Curso_valor);
                    
                    return View(cursosCarrinho);
                }

                foreach (var item in (List<Curso>)Session["carrinho"])
                {
                    retorno = ca.CursoCarrinho(item.Curso_id);

                    var curso = retorno;

                    cursosCarrinho.Add(new Curso()
                    {                       
                        Curso_nome = curso.Curso_nome.ToString(),
                        Curso_descricao = curso.Curso_descricao.ToString(),
                        Curso_duracao = Convert.ToDouble(curso.Curso_duracao),
                        Curso_valor = Convert.ToDouble(curso.Curso_valor),
                        Curso_id = Convert.ToInt32(curso.Curso_id),
                    });

                    TempData["total"] = cursosCarrinho.Sum(c => c.Curso_valor);
                }
                return View(cursosCarrinho);
            }
            else
                return RedirectToAction("Login", "Autenticacao");
        }


        [HttpPost]
        public ActionResult Compra(Curso c, FormCollection frm)
        {
            string formaPagamento = frm["Pagamento"].ToString();

            foreach (var item in (List<Curso>)Session["carrinho"])
            {
                ca.CompraCurso(item.Curso_id, Convert.ToInt32(Session["UsuId"]), formaPagamento);           
            }



            return RedirectToAction("Index", "Home");
        }
    }
}