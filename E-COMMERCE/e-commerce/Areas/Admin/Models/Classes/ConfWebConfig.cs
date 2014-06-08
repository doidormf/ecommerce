using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace e_commerce.Areas.Admin.Models.Classes
{
    public class ConfWebConfig
    {
        //[Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Caminho das Imagens")]
        public string caminhoImagens { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Rede")]
        public string rede { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Filial")]
        public string filial { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Mostrar produtos sem estoque")]
        public string filtraEstoque { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Total de Imagens na Tela Inicial")]
        public string qtdeElemntosPaginaInicial { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Menssagem de Carrinho Vazio")]
        public string msgCarrinho { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Código dos Produtos no Banner")]
        public string idsProdutosBanner { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "E-mail cadastrado no PagSeguro")]
        public string emailCredential { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Token fornecido pelo PagSeguro")]
        public string tokenCredential { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Tema do Site")]
        public string tema { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "E-mail de Recuperação de Senha")]
        public string emailContato { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string senhaEmail { get; set; }


        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "E-mail de Contato via Site")]
        public string emailContatoSite { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha do e-mail de Contato via Site")]
        public string senhaEmailContatoSite { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Servidor Smtp")]
        public string ServidorSmtp { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Responsável pelo frete ")]
        public string respFrete { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Peso minimo dos correios")]
        public string pesoMinCorreio { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "CEP de origem")]
        public string cepEnvio { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Link do Site")]
        public string linkSite { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Reservar Pedido no Site")]
        public string reservaBtComprar { get; set; }
    }
}