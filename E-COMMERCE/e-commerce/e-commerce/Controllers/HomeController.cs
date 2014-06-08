using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using e_commerce.Areas.Admin.Controllers;
using e_commerce.Helpers;
using e_commerce.Models;
using e_commerce.Models.Classes;
using e_commerce.Models.Repositorios;
using e_commerce.Properties;
using VipWebUtils.Helpers;
using VipWebUtils.Helpers.Exceptions;
using VipWebUtils.Helpers.Extensoes;
using VipWebUtils.Helpers.Security;
using System.Linq;

namespace e_commerce.Controllers
{
    public class HomeController : Controller
    {
        private DefineCaminho caminho = new DefineCaminho();
        private ProdutosDao produtos = new ProdutosDao();
        private List<Home> lista = new List<Home>();
        private List<Menu> listaMenuLateral = new List<Menu>();
        private List<Home> listaProdutosBusca = new List<Home>();
        private PedidosDAO pedi = new PedidosDAO();
        private ControleCarrinho controleCarrinho = new ControleCarrinho();
        private FilialDao _filial = new FilialDao();
        /// <summary>
        /// Chamada inicial da tela principal do site
        /// </summary>
        /// <Author>Claudinei Nascimento / Vip-Systems Informática & Consultoria Ltda.</Author>
        /// <returns>lista de produtos</returns>
        public ActionResult Index()
        {

            ViewBag.Tema = Settings.Default.Tema;

            Session.RemoveAll();

            if (Request.Cookies["Admin"] != null) { RemoverUsuario(); }
            try
            {
                HttpCookie cookie;

                if (!Diretorio.Existe(AppDomain.CurrentDomain.BaseDirectory + "/Logs"))
                    Diretorio.Criar(AppDomain.CurrentDomain.BaseDirectory + "/Logs");

                // Se o cookie não existe, efetuamos sua criação
                if (Request.Cookies["usuario"] == null)
                {
                    cookie = new HttpCookie("usuario");

                    // Configura a expiração do Cookie para 1 horas
                    cookie.Expires = DateTime.Now.AddMinutes(Settings.Default.IntervaloLimpezaCookies);

                    //cookie.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(cookie);
                }
                //ObjectResult<buscaprodutos_result> result = null;
                //result = produtos.GetProdutosRelacionados("ww");
                


                if (string.IsNullOrEmpty(Convert.ToString(Session["qtdeCart"])))
                    Session["qtdeCart"] = controleCarrinho.ContarCarrinho(RecuperarIDProdutos()).ToString();

            }
            catch (Exception ex)
            {
                StackTrace exe = new StackTrace(ex, true);
                CustomException ep = new CustomException(ex, exe, "");
                ep.Save(AppDomain.CurrentDomain.BaseDirectory + "Log.log");
            }

            ViewData["Lancamento"] = "";
            List<Home> model = produtosHome();
            return View(model);
        }

        /// <summary>
        /// Preenceh
        /// </summary>
        /// <returns></returns>
        public ActionResult Banner()
        {
            try
            {
                String[] ids = Settings.Default.IdsProdutosBanner.Split(',');
                ObjectResult<buscaprodutos_result> result = null;
                //ObjectResult<Parcelamentos_Result> resultParcelamento = null;

                foreach (var itemIds in ids)
                {
                    result = produtos.getProdutosBanner(itemIds.Trim());

                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            Home _home = new Home();
                            _home.CodFamilia = item.CodFamilia.Trim();
                            _home.codigo = item.codigo.ToString().Trim();
                            _home.descricao = item.descricao.Trim();
                            _home.fotoitem = caminho.getCaminho(item.fotoitem, item.codigo.ToString());
                            _home.nomeresumido = item.nomeresumido.Trim();
                            _home.ec5cod = item.ec5cod.Trim();
                            _home.ec5nom = item.ec5nom.Trim();
                            _home.ec6nom = item.ec6nom.Trim();
                            _home.preco = item.preco;

                            //foreach (var itemParcela in resultParcelamento = produtos.getParcelamento(item.preco))
                            //{
                            //    _home.parcela = itemParcela.parcela;
                            //    _home.VlrParcela = itemParcela.VlrParcela;
                            //}

                            lista.Add(_home);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StackTrace exe = new StackTrace(ex, true);
                CustomException ep = new CustomException(ex, exe, "");
                ep.Save(AppDomain.CurrentDomain.BaseDirectory + "LogBanner.log");
            }
            return PartialView("Banner", lista);
        }

        public ActionResult Busca()
        {
            ViewData["formBusca"] = "true";

            return PartialView("Busca");
        }

        public ActionResult Rodape(String search)
        {
            filial f = _filial.getFilialByIdAndRede(Settings.Default.Filial, Settings.Default.Rede);

            return PartialView("Rodape", f);
        }

        [HttpPost]
        public ActionResult Search(String search)
        {

            sbyte parametroPrimeiroSearch = 3;//Indica que a busca será feita pela descrição do produto.
            string conteudo = search.Trim();
            int contador = 0;

            foreach (var item in conteudo)
            {
                for (int n = 0; n < 9; ++n)
                {
                    if (item.CompareTo(Convert.ToChar(n.ToString())) == 0)
                    {
                        contador++;
                    }
                }
            }
            if (contador >= conteudo.Length)
            {
                parametroPrimeiroSearch = 1;//Indica que a busca será feita pela código do produto.
                conteudo = String.Format("{0:0000000000000}", conteudo);
                contador = 0;
            }


            ViewData["formBusca"] = "false";

            try
            {
                ObjectResult<buscaprodutos_result> result = null;
                result = produtos.getProdutosSearch(parametroPrimeiroSearch, conteudo);
                //  ObjectResult<Parcelamentos_Result> resultParcelamento = null;

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        Home _home = new Home();

                        _home.CodFamilia = item.CodFamilia.Trim();
                        _home.codigo = item.codigo.ToString().Trim();
                        _home.descricao = item.descricao;
                        _home.fotoitem = caminho.getCaminho(item.fotoitem, item.codigo.ToString());
                        _home.nomeresumido = item.nomeresumido.Trim();
                        _home.ec5cod = item.ec5cod.Trim();
                        _home.ec5nom = item.ec5nom.Trim();
                        _home.ec6nom = item.ec6nom.Trim();
                        _home.preco = item.preco;
                        //foreach (var itemParcela in resultParcelamento = produtos.getParcelamento(item.preco))
                        //{
                        //    _home.parcela = itemParcela.parcela;
                        //    _home.VlrParcela = itemParcela.VlrParcela;
                        //}

                        listaProdutosBusca.Add(_home);
                        contador++;
                    }
                }

                ViewData["filtroTela"] = "<strong> RESULTADO DA PESQUISA POR > <span style='color:red;'>" + search.ToUpper().Trim() + "</span> > ITENS ENCONTRADOS: " + contador + "</strong>";

            }
            catch (Exception ex)
            {
                StackTrace exe = new StackTrace(ex, true);
                CustomException ep = new CustomException(ex, exe, "");
                ep.Save(AppDomain.CurrentDomain.BaseDirectory + "Log.log");
            }

            return PartialView("Busca", listaProdutosBusca);
        }

        /// <summary>
        /// preenche a tela inicial com os produdos retornados
        /// </summary>
        /// <Author>Claudinei Nascimento / Vip-Systems Informática & Consultoria Ltda.</Author>
        /// <returns>lista de produtos</returns>
        public List<Home> produtosHome()
        {
            //System.Globalization.CultureInfo cultureinfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            //cultureinfo.TextInfo.ToTitleCase(string.ToLower());// deixa a primeira letra de cada palavra em minusculo
            List<Home> listaMost = new List<Home>();
            
            try
            {
                List<buscaprodutos_result> resultListagem = produtos.getProdutos().Where(p => p.preco > 0).ToList();
                //ObjectResult<buscaprodutos_result> result = produtos.getProdutos();
                //IQueryable<buscaprodutos_result> result = produtos.getProdutosCond();
                //result = produtos.getProdutos();
                //ObjectResult<Parcelamentos_Result> resultParcelamento = null;

                if (resultListagem != null)
                {
                    foreach (var item in resultListagem)
                    {
                        Home _home = new Home();

                        _home.fotoitem = caminho.getCaminho(item.fotoitem, item.codigo.ToString());
                        _home.CodFamilia = item.CodFamilia.Trim();
                        _home.codigo = item.codigo.ToString();
                        _home.descricao = item.descricao.Trim();

                        _home.nomeresumido = item.nomeresumido;
                        _home.ec5cod = item.ec5cod;
                        _home.ec5nom = item.ec5nom.Trim();
                        _home.ec6nom = item.ec6nom.Trim();
                        _home.preco = item.preco;
                        //foreach (var itemParcela in resultParcelamento = produtos.getParcelamento(Convert.ToDecimal(item.preco)))
                        //{
                        //    _home.parcela = itemParcela.parcela;
                        //    _home.VlrParcela = itemParcela.VlrParcela;
                        //}
                        listaMost.Add(_home);
                        if (lista.Count >= Settings.Default.QtdeElementosPaginaInicial)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StackTrace exe = new StackTrace(ex, true);
                CustomException ep = new CustomException(ex, exe, "");
                ep.Save(AppDomain.CurrentDomain.BaseDirectory + "LogBuscaProduto.log");
            }
            //return PartialView(lista);
            return listaMost;
        }

        /// <summary>
        /// Procedure:
        /// preenche o menu superior com o resultado das categorias
        /// encontradas no banco
        /// </summary>
        /// <Author>Claudinei Nascimento / Vip-Systems Informática & Consultoria Ltda.</Author>
        /// <returns>lista de menu</returns>
        public ObjectResult<buscaelementoscontrole_Result> MenuPrincipal()
        {
            ObjectResult<buscaelementoscontrole_Result> result = null;
            try
            {
                result = produtos.getElementoControleMenu(Convert.ToSByte(Resources.IdRetorno), (Convert.ToSByte(Resources.TipoLista)));
            }
            catch (Exception ex)
            {
                StackTrace exe = new StackTrace(ex, true);
                CustomException ep = new CustomException(ex, exe, "");
                ep.Save(AppDomain.CurrentDomain.BaseDirectory + "Log.log");
            }
            return result;
        }

        /// <summary>
        /// Preenche o submenu contido no menu superior
        /// </summary>
        /// <Author>Claudinei Nascimento / Vip-Systems Informática & Consultoria Ltda.</Author>
        /// <returns>lista de menu</returns>
        public ObjectResult<buscaelementoscontroleCategoria_Result> SubMenuPrincipal(String categoria)
        {
            ObjectResult<buscaelementoscontroleCategoria_Result> result = null;
            try
            {
                result = produtos.getElementoControleCategoria(idRetono: 2, filtroec1: categoria);
            }
            catch (Exception ex)
            {
                StackTrace exe = new StackTrace(ex, true);
                CustomException ep = new CustomException(ex, exe, "");
                ep.Save(AppDomain.CurrentDomain.BaseDirectory + "Log.log");
            }
            return result;
        }

        /// <summary>
        /// Preenche o menu lateral
        /// </summary>
        /// <Author>Claudinei Nascimento / Vip-Systems Informática & Consultoria Ltda.</Author>
        /// <returns>lista de menu</returns>
        //[HttpPost]
        public ActionResult getMenuLateral()
        {
            try
            {
                ObjectResult<buscaelementoscontrolegetAllCategorias_Result> result = null;
                result = produtos.getElementoControleAllCategoria(idRetono: 2);

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        Menu menuLateral = new Menu();
                        menuLateral.descricao = item.Nome.Trim();
                        menuLateral.codigo = item.Codigo.ToString().Trim();

                        listaMenuLateral.Add(menuLateral);
                    }
                }
            }
            catch (Exception ex)
            {
                StackTrace exe = new StackTrace(ex, true);
                CustomException ep = new CustomException(ex, exe, "");
                ep.Save(AppDomain.CurrentDomain.BaseDirectory + "Log.log");
            }

            return PartialView("getMenuLateral", listaMenuLateral);
        }

        public string RecuperarIDProdutos()
        {
            string ids = null;
            int index = 1;

            // Resgata o cookie
            HttpCookie cookie = (HttpCookie)Request.Cookies["Carrinho"];
            if (cookie != null)
            {
                // Verifica se o ID do produto já foi inserido ao cookie
                foreach (string item in cookie.Values.AllKeys)
                {
                    ids += item;

                    if (index < cookie.Values.Count)
                        ids += ",";

                    index += 1;
                }
            }
            return ids;
        }

        /// <summary>
        /// Se o usuário logado for um administrador ele é removido
        /// o login do mesmo é removido da pagina de compra
        /// </summary>
        /// <returns></returns>
        public ActionResult RemoverUsuario()
        {
            ViewBag.Tema = Settings.Default.Tema;

            FormsAuthentication.SignOut();

            HttpCookie aCookie;
            string cookieName;
            int limit = Request.Cookies.Count;

            cookieName = Request.Cookies["Admin"].Name;
            aCookie = new HttpCookie(cookieName);
            aCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(aCookie);

            return RedirectToAction("Index", "Home");
        }
    }
}