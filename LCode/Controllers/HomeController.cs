﻿using LCode.Models;
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

        public ActionResult Index()
        {
            return View();
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
            return View();
        }

        public ActionResult Contato()
        {
            return View();
        }

        public ActionResult Sobre()
        {
            return View();
        }

        public ActionResult Cursos()
        {
           var retorno =  bd.ListarCursos();

            return View(retorno);
        }
    }
}