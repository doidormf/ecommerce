using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Web;
using System.Web.Mvc;
using e_commerce.Models;
using e_commerce.Models.Classes;
using e_commerce.Models.Repositorios;
using e_commerce.Properties;
using VipWebUtils.Helpers.Security;

namespace e_commerce.Controllers
{
    public class PedidosController : Controller
    {
        private PedidosDAO _pedidosDao = new PedidosDAO();
        private ClientesDao clientes = new ClientesDao();
        private List<SP_GetPedido> _pedido = null;

        /// <summary>
        /// Lista todos os pedidos feitos pelo cliente
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
           

            HttpCookie cookie = (HttpCookie)Request.Cookies["usuario"];

            if (cookie.Values.AllKeys[0] == null) return RedirectToAction("LogOn", "Account");

            String usuarioId = Crypt.Decrypter(cookie.Values.AllKeys[0]);

            ObjectResult<SP_GetPedido_Result> result = null;

            result = _pedidosDao.getAllPedidos(usuarioId, "");
            _pedido = new List<SP_GetPedido>();
            if (result != null)
            {
                foreach (var item in result)
                {
                    SP_GetPedido ped = new SP_GetPedido();
                  
                    ped.pednum = item.pednum;
                    ped.dtcad = String.Format("{0:dd/MM/yyyy}", item.dtcad);
                    ped.vlrPedido =  String.Format("{0:#,0.00}", item.vlrpedido);
                    ped.idstaPedido = item.idsta;
                    ped.idsta = GetManutencaoPedido.statusConsultaPedido(item.idsta);
                    ped.totitem = (int)item.totitem;
                    ped.frete = item.frete;
                   // ped.dtprventrega = String.Format("{0:dd/MM/yyyy}", item.dtprventrega);
                    ped.dtprventrega = getNumerodeDias(item.dtcad, item.dtprventrega).ToString();
                    _pedido.Add(ped);
                }
            }

            ViewBag.Tema = Settings.Default.Tema;

            return View(_pedido);
        }

        private int getNumerodeDias(DateTime firstDate, DateTime secondDate)
        {
            return secondDate.Subtract(firstDate).Days;
        }

        /// <summary>
        /// Mosta todos os detalhes da de um determindao produto
        /// </summary>
        /// <param name="numPedido"></param>
        /// <param name="frete"></param>
        /// <param name="idStatus"></param>
        /// <param name="dataCompra"></param>
        /// <param name="dataEntr"></param>
        /// <returns></returns>
        public ActionResult Details(string numPedido, decimal frete, string idStatus, string dataCompra, string dataEntr)
        {
            decimal soma = 0;
            decimal multiplicao = 0;

            HttpCookie cookie = (HttpCookie)Request.Cookies["usuario"];

            if (cookie.Values.AllKeys[0] == null) return RedirectToAction("LogOn", "Account");

            String usuarioId = Crypt.Decrypter(cookie.Values.AllKeys[0]);

            ObjectResult<SP_GetPedido_Result> result = null;

            result = _pedidosDao.getAllPedidos(usuarioId, numPedido);
            _pedido = new List<SP_GetPedido>();

            if (result != null)
            {
              ViewBag.status = GetManutencaoPedido.statusConsultaPedido(idStatus);
              ViewBag.numPedido = numPedido;
           
              ViewBag.dataCompra = dataCompra;
              ViewBag.dataPrevistaEntrega = dataEntr +" dias úteis*";

             // if(idStatus.Equals("10")) ViewBag.dataPrevistaEntrega = dataEntr;

                foreach (var item in result)
                {
                    
                    SP_GetPedido ped = new SP_GetPedido();
   
                    ped.idprod = item.idprod;
                    ped.dsc = item.dsc;
                    ped.qtde = (int)item.qtde;
                    ped.prcvenda = String.Format("{0:#,0.00}",item.prcvenda);
                    multiplicao = (decimal)item.prcvenda * ped.qtde;
                    ped.prctotalprod = String.Format("{0:#,0.00}", multiplicao);
                    soma += multiplicao; 
                    
                    _pedido.Add(ped);
                }
            }
              ViewBag.ValorTotaCompra = String.Format("{0:#,0.00}", soma);
              ViewBag.frete = String.Format("{0:#,0.00}", frete).Trim();
              ViewBag.SomaTotal = String.Format("{0:#,0.00}", soma + frete);
            
              ViewBag.Tema = Settings.Default.Tema;

            return View(_pedido);
        }
    }
}