using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using e_commerce.Areas.Admin.Models;
using e_commerce.Properties;

namespace e_commerce.Areas.Admin.Controllers
{
    public class ConfiguracaoController : Controller
    {
        private HttpCookie cookieConf;
        //
        // GET: /Admin/Configuracao/

        public ActionResult Index()
        {

          
            if (Request.Cookies["Admin"] == null)
            {
                //RemoverUsuario();
                return RedirectToAction("Index", "HomeAdmin");
            }
            ViewBag.Tema = Settings.Default.Tema;
            return View();
        }

        public ActionResult SetConfLocal()
        {

            if (Request.Cookies["rede"] == null)
            {
                cookieConf = new HttpCookie("rede");
                cookieConf.Values.Add("0000", null);
                Response.Cookies.Add(cookieConf);
                cookieConf.Expires = DateTime.Now.AddYears(50);
            }
            if (Request.Cookies["filial"] == null)
            {
                cookieConf = new HttpCookie("filial");
                cookieConf.Values.Add("000", null);
                Response.Cookies.Add(cookieConf);
                cookieConf.Expires = DateTime.Now.AddYears(50);
            }


            ConfLocal confLocal = new ConfLocal();
            confLocal.filial = Request.Cookies["filial"].Value.Replace("=", "");
            confLocal.rede = Request.Cookies["rede"].Value.Replace("=", "");

            ViewBag.Tema = Settings.Default.Tema;

            return View(confLocal);
        }

        [HttpPost]
        public ActionResult SetConfLocal(ConfLocal model)
        {
            if (ModelState.IsValid)
            {

                if (Request.Cookies["rede"] != null)
                {
                    cookieConf = new HttpCookie("rede");
                    cookieConf.Values.Add(model.rede, null);
                    Response.Cookies.Add(cookieConf);
                    cookieConf.Expires = DateTime.Now.AddYears(50);
                }
                if (Request.Cookies["filial"] != null)
                {
                    cookieConf = new HttpCookie("filial");
                    cookieConf.Values.Add(model.filial, null);
                    Response.Cookies.Add(cookieConf);
                    cookieConf.Expires = DateTime.Now.AddYears(50);
                }
            }
            ViewBag.Tema = Settings.Default.Tema;

            ViewBag.Mgs = "As configurações foram alteradas com sucesso!";
            ViewBag.Tema = Settings.Default.Tema;
            return PartialView("Confirmacao");
        }
        /// <summary>
        /// Desconecta o suário do sistema
        /// </summary>
        /// <author>Claudinei Nascimento</author>
        /// <business>Vip-Systems Tecnologia & Inovação LTDA></business>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            HttpCookie cookie = (HttpCookie)Request.Cookies["Admin"];

            if (cookie.Values.AllKeys[0] != null)
            {
                RemoverUsuario(cookie.Values.AllKeys[0]);
            }
            ViewBag.Tema = Settings.Default.Tema;

            return RedirectToAction("Index", "Home", new { area = string.Empty });
        }

        protected void RemoverUsuario(string idUsuario)
        {
            // Resgata o cookie
            HttpCookie cookie = (HttpCookie)Request.Cookies["Admin"];

            // Remove o id do produto do cookie
            cookie.Values.Remove(idUsuario);
            cookie.Expires = DateTime.Now.AddDays(-1);

            // Grava o cookie
            Response.Cookies.Add(cookie);
        }

        public void RemoverUsuario()
        {
            FormsAuthentication.SignOut();

            HttpCookie aCookie;
            string cookieName;

            cookieName = Request.Cookies["Admin"].Name;
            aCookie = new HttpCookie(cookieName);
            aCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(aCookie);
        }

        public ActionResult RemoverConfiguracao()
        {

            HttpCookie aCookie;
            string cookieName;
            if (Request.Cookies["rede"] != null)
            {
                cookieName = Request.Cookies["rede"].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Values.Add("0000", null);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(aCookie);
            }
            if (Request.Cookies["filial"] != null)
            {
                cookieName = Request.Cookies["filial"].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Values.Add("000", null);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(aCookie);
            }
            ViewBag.Mgs = "As configurações foram removidas com sucesso! Atualize a página para ver as alterações";
            ViewBag.Tema = Settings.Default.Tema;

            return PartialView("Confirmacao");
        }
    }
}
