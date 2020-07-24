using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LCode.Dados;
using LCode.Models;
using LCode.ViewModels;
using System.Configuration;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Net.Mime;

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
                TempData["carrinhoVazio"] = "Seu carrinho está vazio!";
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
                    Curso_valor = curso.Curso_valor,
                    Imagem_link = curso.Imagem_link,

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
                var retornaCurso = ca.CursoJaComprado(Convert.ToInt32(Session["usuId"]), curso_id);

                if (retornaCurso != null)
                {
                    TempData["CursoJaComprado"] = "Você já adquiriu esse curso anteriormente em sua conta!";
                    return RedirectToAction("Carrinho", "Compra");
                };

                List<Curso> carrinho = new List<Curso>();               
                carrinho.Add(new Curso()
                {
                    Curso_id = Convert.ToInt32(curso_id)

                });
                Session["carrinho"] = carrinho;
                
            }
            else
            {
                var retornaCurso = ca.CursoJaComprado(Convert.ToInt32(Session["usuId"]),curso_id);
                
                if (retornaCurso != null)
                {
                    TempData["CursoJaComprado"] = "Você já adquiriu esse curso anteriormente em sua conta!";
                    return RedirectToAction("Carrinho", "Compra");
                };

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
                    var retornaCurso = ca.CursoJaComprado(Convert.ToInt32(Session["usuId"]), Convert.ToInt32(curso_id));

                    if (retornaCurso != null)
                    {
                        TempData["CursoJaComprado"] = "Você já adquiriu esse curso anteriormente em sua conta!";
                        return RedirectToAction("Carrinho", "Compra");
                    };

                    retorno = ca.CursoCarrinho(Convert.ToInt32(curso_id));

                    var curso = retorno;

                    cursosCarrinho.Add(new Curso()
                    {
                        Curso_nome = curso.Curso_nome.ToString(),
                        Curso_descricao = curso.Curso_descricao.ToString(),
                        Curso_duracao = Convert.ToDouble(curso.Curso_duracao),
                        Curso_valor = Convert.ToDouble(curso.Curso_valor),
                        Curso_id = Convert.ToInt32(curso.Curso_id),
                        Imagem_link = curso.Imagem_link.ToString(),

                    });

                    TempData["total"] = cursosCarrinho.Sum(c => c.Curso_valor);

                    return View(cursosCarrinho);
                }
                if (Session["carrinho"] != null) {
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
                            Imagem_link = curso.Imagem_link.ToString(),
                        });

                        TempData["total"] = cursosCarrinho.Sum(c => c.Curso_valor);
                    }
                    return View(cursosCarrinho);
                }
                TempData["carrinhoVazio"] = "Seu carrinho está vazio!";
                return View("Carrinho");
            }
            else
            {
                TempData["loginCompra"] = "Realize o login para realizar a compra!";
                return RedirectToAction("Login", "Autenticacao");
            }
        }


        [HttpPost]
        public ActionResult Compra(Curso c, FormCollection frm, Nullable<int> curso_id)
        {
            string formaPagamento = frm["pagamento"].ToString();

            //string curso_id = Request.QueryString["curso_id"];

            if (Session["carrinho"] != null)
            {
                foreach (var item in (List<Curso>)Session["carrinho"])
                {
                    ca.CompraCurso(item.Curso_id, Convert.ToInt32(Session["UsuId"]), formaPagamento);
                }

                Session["carrinho"] = null;
            }
            if (curso_id != null)
            {
                ca.CompraCurso(Convert.ToInt32(curso_id), Convert.ToInt32(Session["UsuId"]), formaPagamento);
            }
            string destino = Session["UsuEmail"].ToString(); //"wellissonrodrigues12@gmail.com";//"gabitorres118@gmail.com";
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("developmentkey.info@gmail.com");
            mail.To.Add(destino);
            mail.Subject = "Obrigado por comprar o nosso curso!";

            AlternateView htmlview = AlternateView.CreateAlternateViewFromString("<br/><center><img src=cid:imgPath height=500 width=600></center>", null, "text/html");
            string linkimg = ConfigurationManager.AppSettings["imgEmail"].ToString();
            LinkedResource image = new LinkedResource(linkimg);
            image.ContentType = new ContentType(MediaTypeNames.Image.Jpeg);
            image.ContentId = "imgPath";

            htmlview.LinkedResources.Add(image);
            mail.AlternateViews.Add(htmlview);

            string user = ConfigurationManager.AppSettings["usersmtp"].ToString();
            string senha = ConfigurationManager.AppSettings["senhasmtp"].ToString();

            try
            {
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(user, senha);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}