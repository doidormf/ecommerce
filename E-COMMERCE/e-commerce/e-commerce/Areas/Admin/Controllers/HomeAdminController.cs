using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using e_commerce.Areas.Admin.Models.Repository;
using e_commerce.Models;
using e_commerce.Properties;
using VipWebUtils.Helpers.Security;

namespace e_commerce.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        private HttpCookie cookie;
        private AdminDao admin = new AdminDao();
        //
        // GET: /Admin/HomeAdmin/

        public ActionResult Index()
        {

                      ViewBag.Tema = Settings.Default.Tema;
            return View();
        }
        [HttpPost]
        public ActionResult Index(cadusu model, string returnUrl)
        {
            cookie = new HttpCookie("Admin");

            if (ModelState.IsValid)
            {
                String senha = Crypt.Crypter(model.senuser.ToUpper());

                cadusu result = admin.getAdminByIdAndPassword(model.loguser, senha);

                if (result != null)
                {
                    FormsAuthentication.SetAuthCookie(result.nomuser,false);

                    String usuario = Crypt.Crypter(result.coduser);

                    if (Request.Cookies["Admin"] == null)
                    {
                        //cookie.Expires = DateTime.Now.AddSeconds(30);

                        Response.Cookies.Add(cookie);
                    }
                    cookie.Values.Add(usuario, null);

                    Response.Cookies.Add(cookie);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        ViewBag.Tema = Settings.Default.Tema;

                        return Redirect(returnUrl);
                    }
                    else
                    {
                        ViewBag.Tema = Settings.Default.Tema;

                        return RedirectToAction("Index","Configuracao");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nome de usuário e/ou senha inválidos");
                }
            }

            ViewBag.Tema = Settings.Default.Tema;
            // If we got this far, something failed, redisplay form
            return View(model);
        }

    }
}
