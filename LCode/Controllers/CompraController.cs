using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LCode.Controllers
{
    public class CompraController : Controller
    {
        // GET: Compra
        public ActionResult Carrinho(Nullable <int> curso_id)
        {
            

            return View();
        }

        public ActionResult Compra()
        {
            return View();
        }
    }
}