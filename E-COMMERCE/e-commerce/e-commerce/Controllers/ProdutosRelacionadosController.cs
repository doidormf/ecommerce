using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Diagnostics;
using System.Web.Mvc;
using e_commerce.Helpers;
using e_commerce.Models;
using e_commerce.Models.Classes;
using e_commerce.Properties;
using VipWebUtils.Helpers.Exceptions;
using VipWebUtils.Helpers.Extensoes;

namespace e_commerce.Controllers
{
    public class ProdutosRelacionadosController : Controller
    {
        private DefineCaminho caminho = new DefineCaminho();
        private ProdutosDao produtos = new ProdutosDao();
        private List<Produtos> lista = new List<Produtos>();
        private List<Produtos> lista1 = new List<Produtos>();
        private List<Menu> listaMenu = new List<Menu>();

        //
        // GET: /ProdutosRelacionados/

        public ActionResult IndexProdutosRelacionados()
        {
            return View("IndexProdutosRelacionados");
        }

        /// <summary>
        /// Faz a busca do item selecionado no menu
        /// </summary>
        /// <Author>Claudinei Nascimento / Vip-Systems Informática & Consultoria Ltda.</Author>
        /// <returns>lista de produtos</returns>
        public ActionResult getProdutosRelacionados(String nomecategoria, String nomeSubCategoria, String categoria, String produto)
        {
            //variavel para ser executada a comparação na view IndexProdutosRelacionados
            ViewData["categorias"] = "true";
            ViewData["filtroTela"] = nomecategoria + " > " + nomeSubCategoria.ToUpper();
            try
            {
                ObjectResult<buscaelementoscontroleSelecionadoMenu_Result> result = null;
                result = produtos.getElementoItemSelecionadoMenu(filtroec1: categoria, filtroec2: produto, indagrup: 1);
               // ObjectResult<Parcelamentos_Result> resultParcelamento = null;

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        Produtos _produtos = new Produtos();

                        _produtos.CodFamilia = item.CodFamilia.Trim();
                        _produtos.codigo = item.codigo.ToString().Trim();
                        _produtos.descricao = item.descricao.Trim();
                        _produtos.fotoitem = caminho.getCaminho(item.fotoitem, item.codigo.ToString());
                        _produtos.nomeresumido = item.nomeresumido.Trim();
                        _produtos.ec5nom = item.ec5nom.Trim();
                        _produtos.ec6nom = item.ec6nom.Trim();
                        _produtos.ec5cod = item.ec5cod.Trim();
                        _produtos.preco = item.preco;
                        //foreach (var itemParcela in resultParcelamento = produtos.getParcelamento(Convert.ToDecimal(item.preco)))
                        //{
                        //    _produtos.parcela = itemParcela.parcela;
                        //    _produtos.VlrParcela = itemParcela.VlrParcela;
                        //}

                        lista.Add(_produtos);
                    }
                }
                ViewBag.Tema = Settings.Default.Tema;
            }
            catch (Exception ex)
            {
                StackTrace exe = new StackTrace(ex, true);
                CustomException ep = new CustomException(ex, exe, "");
                ep.Save(AppDomain.CurrentDomain.BaseDirectory + "Log.log");
            }
            return View("IndexProdutosRelacionados", lista);
        }

        /// <summary>
        /// Função para preencher o menu lateral de acordo com o produtos
        /// selecionado no menu superior
        /// </summary>
        /// <Author>Claudinei Nascimento / Vip-Systems Informática & Consultoria Ltda.</Author>
        /// <returns>partial view de categorias</returns>
        public ActionResult carregaMenuLateralGeral(String categoria, String nomeCategoria)
        {
            ViewData["nomeCategoria"] = nomeCategoria.Trim();
            try
            {
                ObjectResult<buscaelementoscontroleCategoria_Result> result = null;
                result = produtos.getElementoControleCategoria(idRetono: 2, filtroec1: categoria);

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        Menu _menu = new Menu();

                        _menu.codigo = item.Codigo.ToString().Trim();
                        _menu.descricao = item.Nome.Trim();
                        _menu.codigoCategoria = categoria.Trim();

                        listaMenu.Add(_menu);
                    }
                }
                ViewBag.Tema = Settings.Default.Tema;
            }
            catch (Exception ex)
            {
                StackTrace exe = new StackTrace(ex, true);
                CustomException ep = new CustomException(ex, exe, "");
                ep.Save(AppDomain.CurrentDomain.BaseDirectory + "Log.log");
            }

            return PartialView("carregaMenuLateralGeral", listaMenu);
        }

        /// <summary>
        /// Função para chamar a busca do elemento selecionado no menu latral
        /// e depois atribui o resultado a tela de produtos relacionados
        /// </summary>
        /// <Author>Claudinei Nascimento / Vip-Systems Informática & Consultoria Ltda.</Author>
        /// <returns>partial view de categorias</returns>
        public ActionResult categoriaGeral(String categoria, String nomeCategoria)
        {
            ViewData["filtroTela"] = "BUSCA > " + nomeCategoria.ToUpper().Trim();

            ViewData["codCategoria"] = nomeCategoria;

            ViewData["categorias"] = "false";

            List<Produtos> listaCate = getCategoriaMenuLateral(categoria);

            return View("IndexProdutosRelacionados", listaCate);
        }

        /// <summary>
        /// Faz a busca do item selecionado no menu lateral
        /// </summary>
        /// <Author>Claudinei Nascimento / Vip-Systems Informática & Consultoria Ltda.</Author>
        /// <returns>partial view de categorias</returns>
        public List<Produtos> getCategoriaMenuLateral(String categoria)
        {
            try
            {
                ObjectResult<buscaelementoscontroleSelecionadoMenuLateral_Result> result = null;
                result = produtos.getElementoItemSelecionadoMenuLateral(filtroec2: categoria, indagrup:1);
               //ObjectResult<Parcelamentos_Result> resultParcelamento = null;

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        Produtos _produtos = new Produtos();

                        _produtos.CodFamilia = item.CodFamilia.Trim();
                        _produtos.codigo = item.codigo.ToString().Trim();
                        _produtos.descricao = item.descricao.Trim();
                        _produtos.fotoitem = caminho.getCaminho(item.fotoitem, item.codigo.ToString());
                        _produtos.nomeresumido = item.nomeresumido.Trim();
                        _produtos.ec5nom = item.ec5nom.Trim();
                        _produtos.ec6nom = item.ec6nom.Trim();
                        _produtos.ec5cod = item.ec5cod.Trim();
                        _produtos.preco = item.preco;
                        //foreach (var itemParcela in resultParcelamento = produtos.getParcelamento(Convert.ToDecimal(item.preco)))
                        //{
                        //    _produtos.parcela = itemParcela.parcela;
                        //    _produtos.VlrParcela = itemParcela.VlrParcela;
                        //}

                        lista1.Add(_produtos);
                    }
                }
                ViewBag.Tema = Settings.Default.Tema;
            }
            catch (Exception ex)
            {
                StackTrace exe = new StackTrace(ex, true);
                CustomException ep = new CustomException(ex, exe, "");
                ep.Save(AppDomain.CurrentDomain.BaseDirectory + "Log.log");
            }

            return (lista1);
        }
    }
}