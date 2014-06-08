using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using e_commerce.Areas.Admin.Models.Classes;
using e_commerce.Properties;
using VipWebUtils.Helpers.Security;

namespace e_commerce.Areas.Admin.Controllers
{
    public class WebConfigController : Controller
    {
        //
        // GET: /Admin/WebConfig/

        public ActionResult Index()
        {
          
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "Web.config"); //Carregando o arquivo

            //Pegando elemento pelo nome da TAG
            XmlNodeList xnList = xmlDoc.GetElementsByTagName("setting");

            ConfWebConfig _configuracoes = new ConfWebConfig();
            string senha = Crypt.Decrypter(xnList[6]["value"].InnerText.Trim());

            string senhaEmailConatoSite = Crypt.Decrypter(xnList[30]["value"].InnerText.Trim());

            _configuracoes.caminhoImagens = xnList[26]["value"].InnerText.Trim();
            _configuracoes.rede = xnList[0]["value"].InnerText.Trim();
            _configuracoes.filial = xnList[1]["value"].InnerText.Trim();
            _configuracoes.filtraEstoque = xnList[4]["value"].InnerText.Trim();//define se irá ser feito  filtro pelo estoque do produto
            _configuracoes.qtdeElemntosPaginaInicial = xnList[3]["value"].InnerText.Trim();// total de produtos que apareceram na tela principal do site

            _configuracoes.msgCarrinho = xnList[27]["value"].InnerText.Trim();//menssagem do carrinho
            _configuracoes.idsProdutosBanner = xnList[4]["value"].InnerText.Trim();//ids dos produtos do banner
            _configuracoes.emailCredential = xnList[18]["value"].InnerText.Trim();//email cadastrado no pagseguro
            _configuracoes.tokenCredential = xnList[6]["value"].InnerText.Trim();//token do pagaseguro
            _configuracoes.tema = xnList[10]["value"].InnerText.Trim();//tema do site
            _configuracoes.respFrete = xnList[14]["value"].InnerText.Trim();// responsável por pagar o frete
            _configuracoes.emailContato = xnList[21]["value"].InnerText.Trim();//email utilizado para enviar a recuperação de senha
            _configuracoes.senhaEmail = senha;//senha do email de recuperação de senha
            _configuracoes.ServidorSmtp = xnList[19]["value"].InnerText.Trim();//sevidor de envio smtp
            _configuracoes.pesoMinCorreio = xnList[20]["value"].InnerText.Trim();//pesso minimo utilizado para calculo de frete dos correios
            _configuracoes.linkSite = xnList[23]["value"].InnerText.Trim();// link do site local
            _configuracoes.cepEnvio = xnList[25]["value"].InnerText.Trim();//cep de envio dos produtos
            _configuracoes.reservaBtComprar = xnList[27]["value"].InnerText.Trim();//define se irá ser feita a reserva no botão comprar
            _configuracoes.emailContatoSite = xnList[21]["value"].InnerText.Trim();//email do formulario de contato do site
            _configuracoes.senhaEmailContatoSite = senhaEmailConatoSite;//senha do email do formulário de contato do site

            ViewBag.Tema = Settings.Default.Tema;

            return View(_configuracoes);
        }

        [HttpPost]
        public ActionResult Index(ConfWebConfig entidade)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "Web.config"); //Carregando o arquivo

            //Pegando elemento pelo nome da TAG
            XmlNodeList xnList = xmlDoc.GetElementsByTagName("setting");

            string senha = Crypt.Crypter(entidade.senhaEmail.Trim());

            string senhaEmailContatoSite = Crypt.Crypter(entidade.senhaEmailContatoSite.Trim());

            entidade.caminhoImagens = xnList[0]["value"].InnerText;

            xnList[0]["value"].InnerText = entidade.caminhoImagens.Trim();
            xnList[1]["value"].InnerText = entidade.rede.Trim();
            xnList[2]["value"].InnerText = entidade.filial.Trim();
            xnList[4]["value"].InnerText = entidade.filtraEstoque.Trim();
            xnList[5]["value"].InnerText = entidade.qtdeElemntosPaginaInicial.Trim();
            xnList[6]["value"].InnerText = entidade.msgCarrinho.Trim();
            xnList[7]["value"].InnerText = entidade.idsProdutosBanner.Trim();
            xnList[8]["value"].InnerText = entidade.emailCredential.Trim();
            xnList[9]["value"].InnerText = entidade.tokenCredential.Trim();
            xnList[10]["value"].InnerText = entidade.tema.Trim();
            xnList[14]["value"].InnerText = entidade.respFrete.Trim();
            xnList[22]["value"].InnerText = entidade.emailContato.Trim();
            xnList[28]["value"].InnerText = senha;
            xnList[23]["value"].InnerText = entidade.ServidorSmtp.Trim();
            xnList[24]["value"].InnerText = entidade.pesoMinCorreio.Trim();
            xnList[25]["value"].InnerText = entidade.linkSite.Trim();
            xnList[26]["value"].InnerText = entidade.cepEnvio.Trim();
            xnList[27]["value"].InnerText = entidade.reservaBtComprar.Trim();
            xnList[29]["value"].InnerText = entidade.emailContatoSite.Trim();
            xnList[30]["value"].InnerText = senhaEmailContatoSite;

            xmlDoc.Save(AppDomain.CurrentDomain.BaseDirectory + "Web.config");
            ViewBag.Tema = Settings.Default.Tema;

            return View(entidade);
        }
    }
}
