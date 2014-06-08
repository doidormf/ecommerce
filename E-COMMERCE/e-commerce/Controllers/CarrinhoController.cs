using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;
using e_commerce.Helpers;
using e_commerce.Models;
using e_commerce.Models.Classes;
using e_commerce.Models.Repositorios;
using e_commerce.Properties;
using Uol.PagSeguro;
using VipWebUtils.Helpers.Exceptions;
using VipWebUtils.Helpers.Extensoes;

namespace e_commerce.Controllers
{
    public class CarrinhoController : Controller
    {
        private List<Carrinho> lista;
        private DefineCaminho caminho = new DefineCaminho();
        private List<string> listaRemove = new List<string>();
        private List<string> listaAlterQuant = new List<string>();
        private ProdutosDao produtos = new ProdutosDao();
        private ClientesDao cliente = new ClientesDao();
        private PedidosDAO _pedido = new PedidosDAO();
        private SetPedido pedido = new SetPedido();
        private SetUpdadePedidoSaida updatePedido = new SetUpdadePedidoSaida();
        private ControleCarrinho controleCarrinho = new ControleCarrinho();
        private AccountController conta = new AccountController();
        private int desvio = 0;
        AccountController contaControle = new AccountController();
        
        /// <summary>
        /// Mosta a apagina do carrinho
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tamanho"></param>
        /// <param name="quantidade"></param>
        /// <returns></returns>
        public ActionResult IndexCarrinho(String id, String tamanho, String quantidade)
        {
            ViewBag.Tema = Settings.Default.Tema;


            ViewBag.MsgFaltaEstoque = string.Empty;
            ViewBag.MsgAlterQuant = string.Empty;
            ViewBag.MenssagemCart = "Após concluir a compra, dirija-se ao caixa para efetuar o pagamento.";

            ViewData["checkout"] = "true";

            if (Settings.Default.ResponsavelPeloFrete == 2)
                ViewBag.MenssagemCart = "O valor do frete será calculado na página do PagSeguro.";

            ViewData["controller"] = "FinalizarPagSeguro";
            ViewData["imgBtn"] = "~/Imagens/Template/Azul/pay.gif";
            if (Settings.Default.Tema.Equals("Cinza")) ViewData["imgBtn"] = "~/Imagens/Template/Cinza/pay.gif";

            //Caso a opção para venda local esteja acionado, esta condição troca o botão de pagamneto
            if (Request.Cookies["rede"] != null && Request.Cookies["filial"] != null)
            {
                ViewData["controller"] = "FinalizarNoCaixa";
                ViewData["imgBtn"] = "~/Imagens/Template/Azul/finalizar.png";
                ViewBag.MenssagemCart = "Após concluir a compra, dirija-se ao caixa para efetuar o pagamento.";
            }

            if (!string.IsNullOrEmpty(id))
                AdicionarAoCarrinhoQuant(id, quantidade, 0);

            if (Settings.Default.FiltraEstoque > 0)// condição que verifica se está sendo feito filtro pelo estoque
            {
                if (getEstoque().Count > 0)//Verifica se foi removido ou alterado a quantidade de algum produto do estoque
                {
                    if (listaRemove.Count > 0)// verifica se foi removido algum produto do carrinho
                    {
                        ViewBag.MsgFaltaEstoque = listaRemove;//lista de produtos removidos
                    }
                    if (listaAlterQuant.Count > 0)//Verifica se houve alteração na quantidade de algum produto
                    {
                        ViewBag.MsgAlterQuant = listaAlterQuant;//Lista dos produtos que tiverem a quntidade alterada
                    }
                }
            }

            IList<Carrinho> result = getCarrinho();

            if (string.IsNullOrEmpty(Convert.ToString(Session["qtdeCart"])))
                Session["qtdeCart"] = controleCarrinho.ContarCarrinho(RecuperarIDProdutos()).ToString();

            return View("IndexCarrinho", result);
        }

        /// <summary>
        /// Atualiza a pagina do carrinho
        /// </summary>
        /// <returns></returns>
        public ActionResult Refresh()
        {
            ViewBag.Tema = Settings.Default.Tema;

            ViewBag.MsgFaltaEstoque = String.Empty;
            ViewBag.MsgAlterQuant = string.Empty;
            ViewBag.MenssagemCart = "";


            if (Settings.Default.ResponsavelPeloFrete == 2)
                ViewBag.MenssagemCart = "O valor do frete será calculado na página do PagSeguro.";

            ViewData["controller"] = "FinalizarPagSeguro";
            ViewData["imgBtn"] = "~/Imagens/Template/pay.gif";

            if (Request.Cookies["rede"] != null && Request.Cookies["filial"] != null)
            {
                ViewData["controller"] = "FinalizarNoCaixa";
                ViewData["imgBtn"] = "~/Imagens/Template/Azul/finalizar.png";
                ViewBag.MenssagemCart = "";
            }

            if (Settings.Default.FiltraEstoque > 0)
            {
                if (getEstoque().Count > 0)
                {
                    if (listaRemove.Count > 0)
                    {
                        ViewBag.MsgFaltaEstoque = listaRemove;
                    }
                    if (listaAlterQuant.Count > 0)
                    {
                        ViewBag.MsgAlterQuant = listaAlterQuant;
                    }
                }
            }

            IList<Carrinho> result2 = null;
            result2 = getCarrinho();

            return View("IndexCarrinho", result2);
        }

        /// <summary>
        /// Exclui o produto do carrinho
        /// </summary>
        /// <param name="idProduto"></param>
        /// <param name="quantidade"></param>
        public ActionResult Confirmacao(String id)
        {
            ViewBag.Tema = Settings.Default.Tema;

            try
            {
                this.RemoverDoCarrinho(id);
            }
            catch (Exception ex)
            {
                StackTrace exe = new StackTrace(ex, true);
                CustomException ep = new CustomException(ex, exe, "");
                ep.Save(AppDomain.CurrentDomain.BaseDirectory + "/Logs/LogRemoverCarrinho.log");
            }
            if (desvio != 0) return View("IndexCarrinho");

            IList<Carrinho> result3 = null;
            result3 = getCarrinho();

            ViewBag.MenssagemCart = "";

            if (Settings.Default.ResponsavelPeloFrete == 2)
                ViewBag.MenssagemCart = "O valor do frete será calculado na página do PagSeguro.";

            ViewData["controller"] = "FinalizarPagSeguro";
            ViewData["imgBtn"] = "~/Imagens/Template/pay.gif";

            //Caso a opção para venda local esteja acionado, esta condição troca o botão de pagamneto
            if (Request.Cookies["rede"] != null && Request.Cookies["filial"] != null)
            {
                ViewData["controller"] = "FinalizarNoCaixa";
                ViewData["imgBtn"] = "~/Imagens/Template/Azul/finalizar.png";
                ViewBag.MenssagemCart = "Após concluir a compra, dirija-se ao caixa para efetuar o pagamento.";
            }
          
            return View("IndexCarrinho", result3);
        }

        /// <summary>
        /// Adciona o produto ao carrinho
        /// </summary>
        /// <param name="idProduto"></param>
        /// <param name="quantidade"></param>
        [HttpPost]
        public void AdicionarAoCarrinhoQuant(string idProduto, string quantidade, int options)
        {
            HttpCookie cookie;

            // Se o cookie não existe, efetuamos sua criação
            if (Request.Cookies["Carrinho"] == null)
            {
                cookie = new HttpCookie("Carrinho");

                // Configura a expiração do Cookie para 1 horas
                cookie.Expires = DateTime.Now.AddMinutes(Settings.Default.IntervaloLimpezaCookies);
                // cookie.Expires = DateTime.Now.AddYears(1);
                // Adiciona item ao cookie
                cookie.Values.Add(idProduto + "-" + quantidade, null);
            }

            // Caso o cookie já exista
            else
            {
                bool existe = false;

                // Resgata o cookie
                cookie = (HttpCookie)Request.Cookies["Carrinho"];

                // Configura a expiração do Cookie para 2 horas
                //cookie.Expires = DateTime.Now.AddHours(2);
                cookie.Expires = DateTime.Now.AddMinutes(Settings.Default.IntervaloLimpezaCookies);
                //cookie.Expires = DateTime.Now.AddYears(1);
                // Verifica se o ID do produto já foi inserido ao cookie
                foreach (string item in cookie.Values.AllKeys)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        String[] ids2 = item.Split('-');

                        int comparar = Convert.ToInt32(quantidade);

                        if (ids2[0].Equals(idProduto))
                        {
                            if (options == 1)
                            {
                                if (comparar >= 1)
                                {
                                    RemoverDoCarrinho(idProduto + "-" + ids2[1]);

                                    cookie.Values.Add(idProduto + "-" + quantidade, null);

                                    existe = true;

                                    break;
                                }
                            }
                            else
                            {
                                existe = true;
                            }
                        }
                    }
                }

                // Se o produto não existir no carrinho ser adicionado
                if (!existe)
                {
                    cookie.Values.Add(idProduto + "-" + quantidade, null);
                }
            }

            // Grava o cookie
            Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// faz a busca dos elementos que estão gravados nos cookies
        /// </summary>
        /// <returns>lista do produtos no arrinho</returns>
        public IList<Carrinho> getCarrinho()
        {
            int i = 0;
            int totalItensCarrinho = 0;
            lista = new List<Carrinho>();

            String produtosCarrinho = this.RecuperarIDProdutos();

            if (!string.IsNullOrEmpty(produtosCarrinho))
            {
                ViewData["informacao"] = "true";

                String[] ids = produtosCarrinho.Split(',');

                foreach (var itemId in ids)
                {
                    if (!string.IsNullOrEmpty(itemId.Trim()))
                    {
                        String[] ids2 = itemId.Split('-');

                        ObjectResult<buscaprodutos_result> result = null;

                        result = produtos.getProdutosById(ids2[0]);
                        try
                        {
                            if (result != null)
                            {
                                foreach (var item in result)
                                {
                                    Carrinho _produtos = new Carrinho();

                                    _produtos.codigo = item.codigo.ToString().Trim();
                                    _produtos.CodFamilia = item.CodFamilia.Trim();
                                    _produtos.descricao = item.descricao.Trim();
                                    _produtos.fotoitem = caminho.getCaminho(item.fotoitem, item.codigo.ToString());
                                    _produtos.nomeresumido = item.nomeresumido.Trim();
                                    _produtos.preco = item.preco;
                                    _produtos.soma = somaProdutos(Convert.ToInt32(ids2[1]), item.preco);
                                    _produtos.quantidade = Convert.ToInt32(ids2[1]);
                                    _produtos.contador = i;
                                    _produtos.ec6nom = item.ec6nom.Trim();
                                    totalItensCarrinho += Convert.ToInt32(ids2[1]);
                                    lista.Add(_produtos);
                                    i++;
                                }

                                Session["qtdeCart"] = totalItensCarrinho.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            StackTrace exe = new StackTrace(ex, true);
                            CustomException ep = new CustomException(ex, exe, "");
                            ep.Save(AppDomain.CurrentDomain.BaseDirectory + "/Logs/Log.log");
                        }
                    }
                }
            }
            else
            {

                ViewData["informacao"] = "false";
                Session["qtdeCart"] = 0;
                ViewData["MenssagemCarrinho"] = Settings.Default.MenssagemCarrinho;

            }
            return lista;
        }

        /// <summary>
        /// Recupera os produtros do carrinho
        /// </summary>
        /// <returns>ids do produtos</returns>
        public string RecuperarIDProdutos()
        {
            string ids = null;
            int index = 1;
            HttpCookie cookies;

            // Se o cookie não existe, efetuamos sua criação
            if (Request.Cookies["Carrinho"] == null)
            {
                cookies = new HttpCookie("Carrinho");
            }
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
            else
            {
                ids = string.Empty;
            }

            return ids;
        }

        /// <summary>
        /// remove os produtos do carrinho
        /// </summary>
        /// <param name="idProduto"></param>
        protected void RemoverDoCarrinho(string idProduto)
        {
            // Resgata o cookie
            HttpCookie cookie = (HttpCookie)Request.Cookies["Carrinho"];

            // Configura a expiração do Cookie para 1 horas
            //cookie.Expires = DateTime.Now.AddMinutes(Settings.Default.IntervaloLimpezaCookies);
            cookie.Expires = DateTime.Now.AddYears(1);
            // Remove o id do produto do cookie
            cookie.Values.Remove(idProduto);

            // Grava o cookie

            Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Metodo utilizado para gerar um pedido, em uma loja local
        /// quando o sistema é acessado por um quiosque 
        /// </summary>
        /// <returns></returns>
        public ActionResult FinalizarNoCaixa()
        {
            ViewBag.Tema = Settings.Default.Tema;

             //Recupera o id do usuário que esta gravado nos cookies
            HttpCookie cookie = (HttpCookie)Request.Cookies["usuario"];

            //se não ouver id, a pagina é redirecionada para a tela de login
            if (cookie.Values.AllKeys[0] == null)
            {
                //linkModel model = new linkModel();
                //model.link = "/Carrinho/IndexCarrinho";
                //return RedirectToAction("LogOn", "Account", model);
                return RedirectToAction("LogOn", "Account");
            }

            ecomm_clientes usuario = cliente.getUsuarioById(cookie.Values.AllKeys[0]);
            String produtosCarrinho = this.RecuperarIDProdutos();

            PaymentRequest payment = new PaymentRequest();

            if (usuario != null)
            {
                string retorno = string.Empty;

                if (Settings.Default.FiltraEstoque > 0)
                {
                    if (getEstoque().Count > 0) return RedirectToAction("IndexCarrinho");
                }
                 String[] ids = produtosCarrinho.Split(',');
                 decimal vlrTotalCompra = 0;
                 foreach (var itemId in ids)
                 {
                     String[] ids2 = itemId.Split('-');

                     ObjectResult<buscaprodutos_result> result = produtos.getProdutosById(ids2[0]);

                     if (result != null)
                     {
                         foreach (var item in result)
                         {

                             payment.Items.Add(new Item(item.codigo.ToString(), item.nomeresumido, Convert.ToInt32(ids2[1]), item.preco,0, 0));
                             vlrTotalCompra += Convert.ToInt32(ids2[1]) * item.preco;
                         }
                     }
                 }

                SetPedidoLocal setFinalizarLocal = new SetPedidoLocal();
                setFinalizarLocal.usuario = usuario;
                setFinalizarLocal.lstItem = payment.Items;
                setFinalizarLocal.vlrTotalCompra = vlrTotalCompra;
                setFinalizarLocal.rede = Request.Cookies["rede"].Value.Replace("=", "");
                setFinalizarLocal.filial = Request.Cookies["filial"].Value.Replace("=", "");
                retorno =  setFinalizarLocal.gerarPedido();

                if (retorno != null)
                {
                    ViewBag.Status = GetManutencaoPedido.statusRetornoPedido(1);
                    ViewBag.Valor = "R$ " + vlrTotalCompra;
                    ViewBag.Numero = retorno;

                    if (!string.IsNullOrEmpty(produtosCarrinho))
                    {
                        LimparCarrinho(produtosCarrinho);
                    }

                    LogOff();
                    return View("FinalizarNoCaixaMsg");
                }
            }

         
            return View("Index");
        }

        /// <summary>
        /// desloga o usuário no retorno da compra
        /// </summary>
        public void LogOff()
        {
            FormsAuthentication.SignOut();

            HttpCookie cookie = (HttpCookie)Request.Cookies["usuario"];

            if (cookie.Values.AllKeys[0] != null)
            {
                cookie.Values.Remove(cookie.Values.AllKeys[0]);

                Response.Cookies.Add(cookie);
            }
        }
        
        #region Pagamento com PagSeguro
        /// <summary>
        /// Função responsável por abrir uma requisição
        /// com a operadora de pagamentos PagSeguro,
        /// onde é passado para a mesma, as informações
        /// referente a os itens que estão sendo comprados
        /// </summary>
        /// <author>Claudinei Nascimento</author>
        /// <business>Vip-Systems Tecnologia & Inovação LTDA></business>
        /// <returns>Redireciona para a pagina do PagSeguro</returns>
        public ActionResult FinalizarPagSeguro()
        {
            //Recupera o id do usuário que esta gravado nos cookies
            HttpCookie cookie = (HttpCookie)Request.Cookies["usuario"];

            //se não ouver id, a pagina é redirecionada para a tela de login
            if (cookie.Values.AllKeys[0] == null)
            {
                //linkModel model = new linkModel();
                //model.link = "/Carrinho/IndexCarrinho";
                //return RedirectToAction("LogOn", "Account", model);
                return RedirectToAction("LogOn", "Account");
            }

            ecomm_clientes usuario = cliente.getUsuarioById(cookie.Values.AllKeys[0]);
            String produtosCarrinho = this.RecuperarIDProdutos();
            Uri paymentRedirectUri = null;

            if (usuario != null)
            {
                if (Settings.Default.FiltraEstoque > 0)
                {
                    if (getEstoque().Count > 0) return RedirectToAction("IndexCarrinho");
                }
                decimal peso = Settings.Default.PesoMinimoCorreios;

                if (controleCarrinho.getTotalPeso(produtosCarrinho) >= (int)Settings.Default.PesoMinimoCorreios) peso = 0;

                try
                {
                    decimal vlrTotalCompra = 0;

                    PaymentRequest payment = new PaymentRequest();

                    payment.Currency = Currency.Brl;

                    payment.Reference = cookie.Values.AllKeys[0];

                    payment.Sender = new Sender(usuario.nome, usuario.EMAIL1);

                    payment.Shipping = new Shipping();

                    //Forma de envio não especificada, o usuário podera escolher entra PAC e SEDEX dos Correios
                    payment.Shipping.ShippingType = ShippingType.NotSpecified;

                    Address address = new Address("BRASIL", usuario.UF_RESIDENCIAL, usuario.CIDADE_RESIDENCIAL, usuario.BAIRRO_RESIDENCIAL, usuario.CEP_RESIDENCIAL.Replace("-", ""), usuario.ENDERECO_RESIDENCIAL, usuario.NRO_RESIDENCIAL, usuario.COMPLEMENTO_RESIDENCIAL);

                    payment.Shipping.Address = address;

                    //payment.RedirectUri = new Uri("http://" + Settings.Default.LinkSite + "/e-commerce/Carrinho/Retorno");


                    AccountCredentials credentials = new AccountCredentials(
                    Settings.Default.EmailCredential,
                         Settings.Default.TokenCredential
                      );

                    String[] ids = produtosCarrinho.Split(',');

                    foreach (var itemId in ids)
                    {
                        String[] ids2 = itemId.Split('-');

                        ObjectResult<buscaprodutos_result> result = produtos.getProdutosById(ids2[0]);

                        if (result != null)
                        {

                            ObjectResult<SP_GetPeso_Result> resultPeso = null;

                            foreach (var item in result)
                            {
                                resultPeso = produtos.getPeso(item.codigo.ToString());

                                foreach (var itemPesso in resultPeso)
                                {

                                    if (peso == 0)
                                    {
                                        string pesoTotalFormatado = String.Format("{0:N3}", itemPesso.peso);

                                        peso = Convert.ToDecimal(pesoTotalFormatado.Replace(",", "."));

                                        payment.Items.Add(new Item(item.codigo.ToString(), item.nomeresumido, Convert.ToInt32(ids2[1]), item.preco, (int)peso, 0));

                                        peso = 0;
                                    }
                                    else
                                    {
                                        decimal totalItens = Convert.ToDecimal(controleCarrinho.ContarCarrinho(produtosCarrinho));
                                        decimal pesoT = Convert.ToDecimal(peso);
                                        int lPeso = 1;
                                        if (pesoT / totalItens > 0.54m) lPeso = Convert.ToInt32(pesoT / totalItens);

                                        payment.Items.Add(new Item(item.codigo.ToString(), item.nomeresumido, Convert.ToInt32(ids2[1]), item.preco, lPeso, 0));
                                    }
                                  vlrTotalCompra += Convert.ToInt32(ids2[1]) * item.preco;
                                }
                            }
                        }
                    }
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    if (Settings.Default.ReservaPedidoBotaoComprar)
                    {
                        SetPedidoFinalizar setFinalizar = new SetPedidoFinalizar();
                        setFinalizar.usuario = usuario;
                        setFinalizar.lstItem = payment.Items;
                        setFinalizar.vlrTotalCompra = vlrTotalCompra;

                        payment.Reference = setFinalizar.gerarPedido();
                    }
                        paymentRedirectUri = PaymentService.Register(credentials, payment);

                        if (!string.IsNullOrEmpty(produtosCarrinho))
                        {
                            LimparCarrinho(produtosCarrinho);
                        }

                        return Redirect(paymentRedirectUri.AbsoluteUri.ToString());
                }
                catch (PagSeguroServiceException ex)
                {
                    StackTrace exe = new StackTrace(ex, true);
                    CustomException ep = new CustomException(ex, exe, "");
                    ep.Save(AppDomain.CurrentDomain.BaseDirectory + "/Logs/LogRedirect.log");

                    foreach (PagSeguroServiceError error in ex.Errors)
                    {
                        GravarLog.gravarLogError(error.ToString(), "Erro PagSeguro");
                    }
                }

                ViewBag.Tema = Settings.Default.Tema;
            }
            return View("Shared/Error");
        }

        /// <summary>
        /// Retorno do PagSeguro.
        /// Método executado quando o usuário espera ou clica no botão
        /// para que seja redirecionado novamente ao site, recebe uma
        /// chave que contem o numero da transação no pagSeguro
        /// </summary>
        /// <author>Claudinei Nascimento</author>
        /// <business>Vip-Systems Tecnologia & Inovação LTDA></business>
        /// <param name="id_pagseguro">exemplo de id:766B9C-AD4B044B04DA-77742F5FA653-E1AB24</param>
        /// <returns></returns>
        public ActionResult Retorno(String id_pagseguro)
        {
            ViewBag.Tema = Settings.Default.Tema;

            AccountCredentials credentials = new AccountCredentials(
            Settings.Default.EmailCredential,
                 Settings.Default.TokenCredential
              );

            try
            {
                // obtendo o objeto transaction a partir do código de notificação
                Transaction transaction =
                    TransactionSearchService.SearchByCode(credentials, id_pagseguro);

                ViewBag.Stattus = GetManutencaoPedido.statusRetornoPedido(transaction.TransactionStatus);
                ViewBag.Valor = "R$ " + transaction.GrossAmount;
                ViewBag.Codigo = id_pagseguro;
                ViewBag.Pagamento = GetManutencaoPedido.formaPagamentoPorNome(transaction.PaymentMethod.PaymentMethodType);
            }
            catch (PagSeguroServiceException ex)
            {
                StackTrace exe = new StackTrace(ex, true);
                CustomException ep = new CustomException(ex, exe, "");
                ep.Save(AppDomain.CurrentDomain.BaseDirectory + "/Logs/Log.log");

                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    GravarLog.gravarLogError("Unauthorized: lease verify if the credentials used in the web service call  re correct./n", "Erro Transaction");
                }

                foreach (PagSeguroServiceError error in ex.Errors)
                {
                    GravarLog.gravarLogError(error.ToString(), "Erro PagSeguro");
                }
            }

            return View("Finalizar");
        }

        /// <summary>
        /// Responsável por receber as notificações enviadas pelo pagseguro,
        /// tais notificações são enviadas a cada alteração no status da compra
        /// mais informações vide link:https://pagseguro.uol.com.br/v2/guia-de-integracao/api-de-notificacoes.html
        /// ultimo acesso: 11/04/2013
        /// </summary>
        /// <author>Claudinei Nascimento</author>
        /// <business>Vip-Systems Tecnologia & Inovação LTDA></business>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Notificacao()
        {
            ViewBag.Tema = Settings.Default.Tema;

            try
            {
                //Stopwatch tempo = new Stopwatch();
                //tempo.Start();

                AccountCredentials credentials = new AccountCredentials(
                         Settings.Default.EmailCredential,
                              Settings.Default.TokenCredential
                           );

                string notificationType = Request.Form["notificationType"];
                string notificationCode = Request.Form["notificationCode"];//Recupera o código da transação que foi aberta pelo pagseguro

                if (notificationType == "transaction")
                {
                    // obtendo o objeto transaction a partir do código de notificação
                    Transaction transaction =
                        NotificationService.CheckTransaction(credentials, notificationCode);

                    IList<Item> lstItem = transaction.Items;//Lista com os itens que foram comprados

                    if (transaction != null)
                    {
                        if (Settings.Default.ReservaPedidoBotaoComprar)
                        {
                            updatePedido.gerarPedido(transaction, lstItem);
                            
                        }
                        else
                        {
                            pedido.gerarPedido(transaction, lstItem);
                            
                        }
                    }
                    //tempo.Stop();
                    //GravarLog.gravarLogError("Tempo de inserção: " + tempo.Elapsed, "Tempo de Inserção");
                }
                else
                {
                    GravarLog.gravarLogError("Tipo de transação: " + notificationType, "Não retornou transaction");
                }
            }
            catch (PagSeguroServiceException ex)
            {
                StackTrace exe = new StackTrace(ex, true);
                CustomException ep = new CustomException(ex, exe, "");
                ep.Save(AppDomain.CurrentDomain.BaseDirectory + "/Logs/Log.log");

                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    GravarLog.gravarLogError("Unauthorized: lease verify if the credentials used in the web service call  re correct./n", "Erro Transaction");
                }

                foreach (PagSeguroServiceError error in ex.Errors)
                {
                    GravarLog.gravarLogError(error.ToString(), "Erro PagSeguro");
                }
            }

          

            return View();
        }
        #endregion Pagamento com PagSeguro

        /// <summary>
        /// Verifica se os produtos que estão no carrinho possuem estoque
        /// </summary>
        /// <returns></returns>
        public List<string> getEstoque()
        {
            String[] ids = this.RecuperarIDProdutos().Split(',');

            foreach (var itemId in ids)
            {
                if (!string.IsNullOrEmpty(itemId.Trim()))
                {
                    String[] ids2 = itemId.Split('-');

                    ObjectResult<buscaprodutos_result> result = produtos.getProdutosById(ids2[0]);

                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            decimal qtde = _pedido.getProdSald(item.codigo);

                            if (qtde <= 0)
                            {
                                if (!listaRemove.Contains(item.descricao))
                                {
                                    desvio = 1;
                                    listaRemove.Add(item.nomeresumido);
                                    Confirmacao(itemId);
                                }
                            }
                            else if (qtde < Convert.ToInt32(ids2[1]))
                            {
                                int quant = (int)qtde;

                                desvio = 1;
                                listaAlterQuant.Add(item.nomeresumido);
                                AdicionarAoCarrinhoQuant(ids2[0], quant.ToString(), 1);
                            }
                        }
                    }
                }

            }

            if (listaAlterQuant.Count > 0) return listaAlterQuant;

            return listaRemove;
        }

        /// <summary>
        /// Calcula o valor individual de cada produto que esta no carrinho
        /// </summary>
        /// <param name="quantidade"></param>
        /// <param name="valorUnitario"></param>
        /// <param name="codigoProduto"></param>
        /// <returns></returns>
        public ActionResult Calcular(int quantidade, decimal valorUnitario, String codigoProduto)
        {
            var valorFormatado = "";

            if (quantidade != 0)
            {
                var calc = (valorUnitario * quantidade);

                valorFormatado = String.Format("{0:#,0.00}", calc);

                AdicionarAoCarrinhoQuant(codigoProduto, quantidade.ToString(), 1);
            }

            return Json(valorFormatado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CalcularTotal(decimal valorUnitario)
        {
            var valorFormatado = String.Format("{0:#,0.00}", valorUnitario);

            return Json(valorFormatado, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Soma o valor dos produtos contidos no carrinho
        /// </summary>
        /// <param name="quantidade"></param>
        /// <param name="valorUnitario"></param>
        /// <returns></returns>
        public String somaProdutos(int quantidade, decimal valorUnitario)
        {
            var calc = (valorUnitario * quantidade);

            var valorFormatado = String.Format("{0:#,0.00}", calc);

            return valorFormatado;
        }

        /// <summary>
        /// limpa os itens do carrinho
        /// </summary>
        /// <param name="produtosCarrinho"></param>
        public void LimparCarrinho(string produtosCarrinho) {

            String[] idsRemo = produtosCarrinho.Split(',');

            foreach (var itemId in idsRemo)
            {
                Confirmacao(itemId);
            }
            desvio = 1;
        }
    }
}