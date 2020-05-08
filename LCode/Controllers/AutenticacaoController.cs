using System;
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
        Login usu = new Login();

 
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Usuarios u)
        {
            usu.ValidaLogin(u);
            ViewBag.mensagem = "Digite o usuário e senha";

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

                return View("Index", "Home");
            }
            else
            {
                return View();
            }

        }


        public ActionResult Cadastro()
        {
            return View();
        }
    }
}