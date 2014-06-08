using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using e_commerce.Models;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
using System.IO;

namespace e_commerce.Helpers
{
    public class BuscaCepClass
    {
        public string cep;

        public RegisterModel ConsultaCep(string cep)
        {
            RegisterModel entidade = new RegisterModel();

            
            //Verifica se o cep digitado é válido.
            Match regex = Regex.Match(cep, "^[0-9]{8}$");

            //Se o CEP digitado for valido...
            if (regex.Success)
            {
                try
                {
                    //CEP a ser pesquisado
                    this.cep = cep;

                    //Cria a requisição
                    HttpWebRequest Request =
                        (HttpWebRequest)WebRequest.Create("http://www.buscacep.correios.com.br/servicos/dnec/consultaEnderecoAction.do");

                    //Define o que será postado
                    string postData = "relaxation=" + this.cep + "&TipoCep=ALL&semelhante=N&Metodo=listaLogradouro&TipoConsulta=relaxation&StartRow=1&EndRow=10&cfm=1";

                    //Converte a string de post para um ByteStream
                    byte[] postBytes = Encoding.ASCII.GetBytes(postData);

                    //Parâmetros da requisição
                    Request.Method = "POST";
                    Request.ContentType = "application/x-www-form-urlencoded";
                    Request.ContentLength = postBytes.Length;

                    Stream requestStream = Request.GetRequestStream();

                    //Envia Requisição
                    requestStream.Write(postBytes, 0, postBytes.Length);
                    requestStream.Close();

                    //Resposta do servidor dos correios
                    HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
                    entidade.MsgErro = "Resposta do Servidor: " + response.StatusCode.ToString();

                    //String com a resposta do servidor
                    string responseText = new StreamReader(response.GetResponseStream(), Encoding.Default).ReadToEnd();

                    //Separa os dados com expressão regular
                    MatchCollection matches = Regex.Matches(responseText, ">(.*?)</td>");

                    //Exibe os dados recebidos
                    UTF8Encoding utf8 = new UTF8Encoding();

                    //Rua
                    entidade.Endereco_Res = matches[0].Groups[1].ToString();

                    //Bairro
                    entidade.Bairro_Res = matches[1].Groups[1].ToString();

                    //Cidade
                    entidade.Cidade_Res = matches[2].Groups[1].ToString();

                    //Estado
                    entidade.Uf_Res = matches[3].Groups[1].ToString();
                }
                catch (Exception ex)
                {
                    entidade.MsgErro = "! Erro na execução: " + ex.ToString();
                }
            }
            else
            {
                entidade.MsgErro = "CEP inválido";
            }

            return entidade;
        }
    }
}