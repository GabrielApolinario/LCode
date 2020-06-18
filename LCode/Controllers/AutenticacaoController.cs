﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using LCode.Models;
using System.Security.Cryptography.X509Certificates;
using MySql.Data.MySqlClient;
using Renci.SshNet.Security.Cryptography;
using LCode.Dados;
using System.Web.Security;


namespace LCode.Controllers
{
    public class AutenticacaoController : Controller
    {
        Login log = new Login();
        BancoDeDados bd = new BancoDeDados();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Usuarios u)
        {
            log.ValidaLogin(u);

            if (u.Usu_email != null && u.Usu_senha != null)
            {
                FormsAuthentication.SetAuthCookie(Convert.ToString(u.Usu_id), false);
                Session["UsuEmail"] = u.Usu_email.ToString();
                Session["UsuId"] = u.Usu_id.ToString();

                if (u.Usu_hierarquia == "A")
                {
                    Session["Adm"] = u.Usu_hierarquia.ToString();
                }
                else if (u.Usu_hierarquia == "P")
                {
                    Session["Professor"] = u.Usu_hierarquia.ToString();
                }
                else
                {
                    Session["Estudante"] = u.Usu_hierarquia.ToString();
                }
                TempData["nome"] = u.Usu_nome;
                ViewBag.mensagem = "Seja bem-vindo, " + ViewBag.nome;
                return RedirectToAction("Dashboard","Home");
            }
            else
            {
                ViewBag.mensagem = "Usuário ou senha inválido!";
                return View(u);
            }

        }
       
        
        public ActionResult Cadastro()
        {
            Usuarios u = new Usuarios();
            try
            {
                u.Paises = BancoDeDados.PopulaPais();
                //u.Hierarquia = BancoDeDados.PopulaHierarquia();
                return View(u);
            }
            catch
            {
                return View();
            }

        }
        

        [HttpPost]
        public ActionResult Cadastro(Usuarios u)
        {
            if (ModelState.IsValid)
            {
                
                var retorno = log.ValidarCadastro(u);

                if (retorno == null)
                {

                    Usuarios usuario = new Usuarios
                    {

                        Usu_email = u.Usu_email,
                        Usu_nome = u.Usu_nome,
                        Usu_sobrenome = u.Usu_sobrenome,
                        Usu_senha = u.Usu_senha,
                        Usu_hierarquia = u.Usu_hierarquia,
                        Usu_empresa = u.Usu_empresa,
                        Usu_pais = u.Usu_pais,
                        Usu_data_nasc = u.Usu_data_nasc,
                        Usu_cpf_ou_cnpj = u.Usu_cpf_ou_cnpj,

                    };

                    bd.InsereUsuario(usuario);

                    return View("Login");
                }
                else
                {
                    if (u.Usu_email != null && u.Usu_cpf_ou_cnpj == null)
                    {
                        ModelState.AddModelError("Usu_email", "Já existe um usuário cadastrado com esse e-mail.");
                        return View(u);
                    }
                    else if (u.Usu_cpf_ou_cnpj != null && u.Usu_email == null)
                    {
                        ModelState.AddModelError("Usu_cpf_ou_cnpj", "Já existe um usuário cadastrado com esse cpf.");
                        return View(u);
                    }
                    else
                    {
                        ModelState.AddModelError("Usu_email", "Já existe um usuário cadastrado para esse e-mail.");
                        ModelState.AddModelError("Usu_cpf_ou_cnpj", "Já existe um usuário cadastrado para esse CPF.");
                        return View(u);
                    }
                }
            }
            else
            {
                u.Paises = BancoDeDados.PopulaPais();                
                return View(u);
            }
        }

        public ActionResult Logout()
        {
            Session["UsuEmail"] = null;
            Session["UsuId"] = null;
            Session["Adm"] = null;
            Session["Professor"] = null;
            Session["Estudante"] = null;

            return RedirectToAction("Index", "Home");
        }
    }
}