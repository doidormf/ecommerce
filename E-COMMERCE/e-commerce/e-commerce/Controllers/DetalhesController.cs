using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using e_commerce.Helpers;
using e_commerce.Models;
using e_commerce.Models.Classes;
using e_commerce.Models.Interfaces;
using e_commerce.Properties;
using VipWebUtils.Helpers;
using VipWebUtils.Helpers.Exceptions;
using VipWebUtils.Helpers.Extensoes;

namespace e_commerce.Controllers
{
    public class DetalhesController : Controller
    {
        private ProdutosDao produtos = new ProdutosDao();
        private DefineCaminho caminho = new DefineCaminho();
        private List<Cores> listaCores = new List<Cores>();
        private List<Cores> listaCoresPartial = new List<Cores>();
        private List<Tamanho> listaTamanho = new List<Tamanho>();
        private List<Produtos> listaTempProdutos = new List<Produtos>();
        private ICores contextoCores;
        private VipWebUtils.Helpers.Cor color = new VipWebUtils.Helpers.Cor();

        /// <summary>
        /// Mostra o detalhe dos produtos na pagina de detlhes
        /// onde é possivel realizar a compra do mesmo
        /// </summary>
        /// <Author>Claudinei Nascimento / Vip-Systems Informática & Consultoria Ltda.</Author>
        /// <returns>tela index</returns>
        public ActionResult Index(String codigo, String descricao, decimal preco, String img, String especi, String idCor,
            String nomCor, String nomTamanho, String codFamilia, String parcela, decimal VlrParcela)
        {
            if (!Diretorio.Existe(AppDomain.CurrentDomain.BaseDirectory + "/Logs"))
                Diretorio.Criar(AppDomain.CurrentDomain.BaseDirectory + "/Logs");

            Session["filial"] = Settings.Default.Filial;
            Session["rede"] = Settings.Default.Rede;

            ViewData["cod"] = Convert.ToInt64(codigo);

            ViewData["detalhe"] = especi.Trim();

            ViewData["cor"] = nomCor;

            ViewData["tamanho"] = nomTamanho.Trim();

            ViewData["preco"] = preco;

            ViewData["vlrParcela"] = VlrParcela;

            ViewData["qtdeParcelas"] = parcela;

            ViewData["imgGrande"] = img;

            ViewData["especificacao"] = descricao;

            ViewData["codFamilia"] = codFamilia;

            ViewData["codCor"] = idCor;

            var itemCoresProduto = getCores(codFamilia);

            ViewData["listaCoresProduto"] = itemCoresProduto;

            ViewBag.Tema = Settings.Default.Tema;

            return View("Index");
        }

        public ActionResult IndexBanner(String codigo, String descricao, decimal preco, String img, String especi, String idCor,
    String nomCor, String nomTamanho, String codFamilia, String parcela, decimal VlrParcela)
        {
            if (!Diretorio.Existe(AppDomain.CurrentDomain.BaseDirectory + "/Logs"))
                Diretorio.Criar(AppDomain.CurrentDomain.BaseDirectory + "/Logs");

            Session["filial"] = Settings.Default.Filial;
            Session["rede"] = Settings.Default.Rede;

            ViewData["cod"] = Convert.ToInt64(codigo);

            ViewData["detalhe"] = especi.Trim();

            ViewData["cor"] = nomCor;

            ViewData["tamanho"] = nomTamanho.Trim();

            ViewData["preco"] = preco;

            ViewData["qtdeParcelas"] = parcela;

            ViewData["vlrParcela"] = VlrParcela;

            ViewData["imgGrande"] = img;

            ViewData["especificacao"] = descricao;

            ViewData["codFamilia"] = codFamilia;

            ViewData["codCor"] = idCor;

            var itemCoresProduto = getCores(codFamilia);

            ViewData["listaCoresProduto"] = itemCoresProduto;

            ViewBag.Tema = Settings.Default.Tema;

            return View("IndexBanner");
        }

        /// <summary>
        /// Retorna as cores que o produto possui
        /// </summary>
        /// <Author>Claudinei Nascimento / Vip-Systems Informática & Consultoria Ltda.</Author>
        /// <returns>lista de cores</returns>
        [HttpPost]
        private List<Cores> getCores(String codFamilia)
        {
            try
            {
                ObjectResult<buscaprodutosCores_Result> result = null;

                result = produtos.getProdutosCores(codFamilia);

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        Cores cor = new Cores();
                        cor.ec5cod = item.ec5cod.Trim();
                        cor.CodFamilia = item.CodFamilia.Trim();
                        listaCores.Add(cor);
                    }
                }
            }
            catch (Exception ex)
            {
                StackTrace exe = new StackTrace(ex, true);
                CustomException ep = new CustomException(ex, exe, "");
                ep.Save(AppDomain.CurrentDomain.BaseDirectory + "Log.log");
            }

            return listaCores;
        }

        /// <summary>
        /// Retorna os tamanhos que o produto possui
        /// de acordo com a cor selecionada e pinta o quadrado do tamanho do produto
        ///
        /// </summary>
        /// <Author>Claudinei Nascimento / Vip-Systems Informática & Consultoria Ltda.</Author>
        /// <returns>Lista os Tamanhos</returns>
        /// <param name="codPai"></param>
        /// <param name="codCor"></param>
        /// <returns></returns>
        public ActionResult GetTamanhosRelacionados(String codPai, String codCor, String idProd)
        {
            try
            {
                List<buscaprodutos_result> result = null;

                result = produtos.getTamanhos(codPai, codCor);

               // ObjectResult<Parcelamentos_Result> resultParcelamento = null;

                foreach (var item in result)
                {
                    Tamanho tamanho = new Tamanho();

                    tamanho.codigo = item.codigo.Trim();
                    tamanho.CodFamilia = item.CodFamilia.Trim();
                    tamanho.descricao = item.descricao.Trim();
                    tamanho.nomeresumido = item.nomeresumido.Trim();
                    tamanho.ec5cod = item.ec5cod.Trim();
                    tamanho.ec5nom = item.ec5nom.Trim();
                    tamanho.fotoitem = item.fotoitem;
                    tamanho.ec6nom = item.ec6nom.Trim();
                    tamanho.preco = item.preco;
                    //foreach (var itemParcela in resultParcelamento = produtos.getParcelamento(item.preco))
                    //{
                    //    tamanho.parcela = itemParcela.parcela;
                    //    tamanho.VlrParcela = itemParcela.VlrParcela;
                    //}
                    if (item.codigo.Equals(idProd))
                    {
                        if (Settings.Default.Tema.Equals("Azul"))
                        {
                            tamanho.corDiv = "#5c87b2";
                        }
                        else if (Settings.Default.Tema.Equals("Cinza"))
                        {
                            tamanho.corDiv = "#888888";
                        }
                    }
                    listaTamanho.Add(tamanho);
                }
            }
            catch (Exception ex)
            {
                StackTrace exe = new StackTrace(ex, true);
                CustomException ep = new CustomException(ex, exe, "");
                ep.Save(AppDomain.CurrentDomain.BaseDirectory + "Log.log");
            }
            return PartialView("getTamanhos", listaTamanho);
        }

        /// <summary>
        /// Passa para a View as informações do produto
        /// </summary>
        /// <Author>Claudinei Nascimento / Vip-Systems Informática & Consultoria Ltda.</Author>
        /// <param name="codPai"></param>
        /// <param name="codCor"></param>
        /// <returns>Nova descrição dos produtos relacionados a cor selecionada</returns>
        public ActionResult GetProdutosByCorAndCodPai(String codPai, String codCor, String nomTam)
        {
            try
            {
                dynamic result = produtos.getTrocaImagens(codPai, codCor).First();
                ObjectResult<Parcelamentos_Result> resultParcelamento = null;
                if (result != null)
                {
                    ViewData["cod"] = Convert.ToInt64(result.codigo);

                    ViewData["detalhe"] = result.nomeresumido.Trim();

                    ViewData["cor"] = result.ec5nom;

                    ViewData["tamanho"] = result.ec6nom;

                    ViewData["preco"] = result.preco;
                    foreach (var itemParcela in resultParcelamento = produtos.getParcelamento(result.preco))
                    {
                        ViewData["qtdeParcelas"] = itemParcela.parcela;

                        ViewData["vlrParcela"] = itemParcela.VlrParcela;
                    }
                   
                    ViewData["imgGrande"] = caminho.getCaminho(result.fotoitem, result.codigo);

                    ViewData["especificacao"] = result.descricao;

                    ViewData["codCor"] = result.ec5cod;

                    ViewData["codFamilia"] = result.CodFamilia;

                    var itemCoresProduto = getCores(result.CodFamilia);

                    ViewData["listaCoresProduto"] = itemCoresProduto;
                }
            }
            catch (Exception ex)
            {
                StackTrace exe = new StackTrace(ex, true);
                CustomException ep = new CustomException(ex, exe, "");
                ep.Save(AppDomain.CurrentDomain.BaseDirectory + "Log.log");
            }

            ViewBag.Tema = Settings.Default.Tema;

            return PartialView("_Index");
        }

        /// <summary>
        /// Seta as informações do produtos de acordo com o tamanho selecionado.
        /// Os parametros veem da View(getTamanhos)
        /// </summary>
        /// <Author>Claudinei Nascimento / Vip-Systems Informática & Consultoria Ltda.</Author>
        /// <param name="codigo"></param>
        /// <param name="descricao"></param>
        /// <param name="preco"></param>
        /// <param name="img"></param>
        /// <param name="especi"></param>
        /// <param name="idCor"></param>
        /// <param name="nomCor"></param>
        /// <param name="nomTamanho"></param>
        /// <param name="codFamilia"></param>
        /// <returns></returns>
        public ActionResult setTamanhos(String codigo, String descricao, String preco, String img, String especi, String idCor,
            String nomCor, String nomTamanho, String codFamilia, String parcela, decimal VlrParcela)
        {
            try
            {
                ViewData["cod"] = Convert.ToInt64(codigo);

                ViewData["detalhe"] = especi.Trim();

                ViewData["cor"] = nomCor;

                ViewData["tamanho"] = nomTamanho;

                ViewData["preco"] = preco.Replace(".", ",");

                ViewData["qtdeParcelas"] = parcela;

                ViewData["vlrParcela"] = VlrParcela;

                ViewData["imgGrande"] = caminho.getCaminho(img, codigo);

                ViewData["especificacao"] = descricao;

                ViewData["codFamilia"] = codFamilia;

                ViewData["codCor"] = idCor;

                var itemCoresProduto = getCores(codFamilia);

                ViewData["listaCoresProduto"] = itemCoresProduto;
            }
            catch (Exception ex)
            {
                StackTrace exe = new StackTrace(ex, true);
                CustomException ep = new CustomException(ex, exe, "");
                ep.Save(AppDomain.CurrentDomain.BaseDirectory + "Log.log");
            }

            ViewBag.Tema = Settings.Default.Tema;

            return PartialView("_index");
        }

        /// <summary>
        /// Retorna a lista de cores em formatos Hexadecimal
        /// </summary>
        /// <Author>Claudinei Nascimento / Vip-Systems Informática & Consultoria Ltda.</Author>
        /// <param name="lista"></param>
        /// <returns></returns>
        public ActionResult CoresRelacionadas(IList<Cores> lista)
        {
            contextoCores = new CoresRepositorio();
            try
            {
                if (lista != null)
                {
                    foreach (var item in lista)
                    {
                        dynamic cor = contextoCores.getCoresSelecionadas(item.ec5cod);

                        foreach (var item2 in cor)
                        {
                            Cores coresRes = new Cores();

                            Color color = UintToColor(item2.bgcolor);
                            var html = string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
                            coresRes.hexadecimal = html;
                            coresRes.ec5cod = item.ec5cod;

                            coresRes.ec5nom = item2.descricao.Trim().Trim();
                            coresRes.CodFamilia = item.CodFamilia.Trim();
                            listaCoresPartial.Add(coresRes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StackTrace exe = new StackTrace(ex, true);
                CustomException ep = new CustomException(ex, exe, "");
                ep.Save(AppDomain.CurrentDomain.BaseDirectory + "Log.log");
            }
            return PartialView("CoresRelacionadas", listaCoresPartial);
        }

        public ActionResult GetProdutosRelacionados() {



            return PartialView();
        }

        /// <summary>
        /// Converte as cores em ARGB
        /// </summary>
        private const int RedShift = 0;

        private const int GreenShift = 8;
        private const int BlueShift = 16;

        protected Color UintToColor(int intColor)
        {
            return Color.FromArgb(
                (byte)((intColor >> RedShift) & 0xFF),
                (byte)((intColor >> GreenShift) & 0xFF),
                (byte)((intColor >> BlueShift) & 0xFF)
                );
        }
    }
}