using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using e_commerce.Helpers;
using e_commerce.Models.Classes;
using e_commerce.Properties;

namespace e_commerce.Controllers
{
    public class ContatoController : Controller
    {
        //
        // GET: /Contato/

        public ActionResult Index()
        {
             ViewBag.Tema = Settings.Default.Tema;

            return View();
        }

       [HttpPost]
        public ActionResult Create(Contato entidade)
        {
            ViewBag.Tema = Settings.Default.Tema;
            if (!ModelState.IsValid) return RedirectToAction("Index");

            string retorno = EnvioEmailToEcommerce(entidade);

            if (retorno.Equals("E-mail enviado com sucesso!"))
            {
                ViewBag.Menssagem = retorno + " Em breve retornaremos seu contato";

                EnvioEmailToUser(entidade);

            }
            else
            {

                ViewBag.Menssagem = retorno;
            }

            return PartialView("ConfEmail");
        }

        private string EnvioEmailToUser(Contato entidade)
        {
            string retorno = string.Empty;

            SetEmail email = new SetEmail();


            StringBuilder sb = new StringBuilder();
            sb.Append("<div style='font-size:14px; border: 1px solid #ccc; margin:5% 10% 0 10%;'>");
            sb.Append("<div style='background-color: #ccc;padding: 10px 10px 10px 10px; color:#fff; height: 60px; margin-bottom:15px;'><img src='http://" + Settings.Default.LinkSite + "/e-commerce/Imagens/Template/logo_small.png' /></div>");
            sb.Append("<div style='padding: 5px 5px 5px 5px;'>");
            sb.Append("<table>");
            sb.Append("<tr><td> ");
            sb.Append("</td></tr>");
            sb.Append("<tr><td>");
            sb.Append("</td></tr>");
            sb.Append("</table>");
            sb.Append("<div style='font-size:12px; color:#000; margin-bottom:15px;'>");
            sb.Append("<p>Recebemos sua menssagem, em breve entraremos em contato para maiores esclarecimentos</p>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<div style='background-color: #ccc;text-align:center; vertical-align:central;font-size:10px;padding: 10px; color:#000; height:30px;'>");
            sb.Append("<p>Esta é uma mensagem automática. Por favor, não a responda.</p>");
            sb.Append("</div>");
            sb.Append("</div>");

            retorno = email.SendEmail(entidade.email, entidade.assunto, sb.ToString(), Settings.Default.EmailContatoSite, Settings.Default.senhaEmailContato);


            return retorno;
        }
        private string EnvioEmailToEcommerce(Contato entidade)
        {
            string retorno = string.Empty;

            SetEmail email = new SetEmail();


            StringBuilder sb = new StringBuilder();
            sb.Append("<div style='font-size:14px; border: 1px solid #ccc; margin:5% 10% 0 10%;'>");
            sb.Append("<div style='background-color: #ccc;padding: 10px 10px 10px 10px; color:#fff; height: 60px; margin-bottom:15px;'><img src='http://" + Settings.Default.LinkSite + "/e-commerce/Imagens/Template/logo_small.png' /></div>");
            sb.Append("<div style='padding: 5px 5px 5px 5px;'>");
            sb.Append("<table>");
            sb.Append("<tr><td> ");
            sb.Append("<strong>Nome: </strong>" + entidade.nome);
            sb.Append("</td></tr>");
            sb.Append("<tr><td> ");
            sb.Append("<strong>E-mail: </strong>" + entidade.email);
            sb.Append("</td></tr>");
            sb.Append("<tr><td>");
            sb.Append("<strong>Fone: </strong>" + entidade.telefone);
            sb.Append("</td></tr>");
            sb.Append("<tr><td>");
            sb.Append("<strong>CPF: </strong>" + entidade.cpf);
            sb.Append("</td></tr>");
            sb.Append("</table>");
            sb.Append("<div style='font-size:12px; color:#000; margin-bottom:15px;'>");
            sb.Append("<p><strong>Menssagem: </strong>" + entidade.conteudo + "</p>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<div style='background-color: #ccc;text-align:center; vertical-align:central;font-size:10px;padding: 10px; color:#000; height:30px;'>");
            //sb.Append("<p>Esta é uma mensagem automática. Por favor, não a responda.</p>");
            sb.Append("</div>");
            sb.Append("</div>");

            retorno = email.SendEmail(entidade.email, entidade.assunto, sb.ToString(), Settings.Default.EmailContatoSite, Settings.Default.senhaEmailContato, false);

            return retorno;
        }
    }
}
