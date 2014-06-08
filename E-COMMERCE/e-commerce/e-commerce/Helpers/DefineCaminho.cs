using System;
using System.IO;
using System.Text;
using e_commerce.Properties;
using VipWebUtils.Helpers;

namespace e_commerce.Helpers
{
    public class DefineCaminho
    {
        /// <summary>
        ///Função que trasforma o caminho da imagem vindo do banco, para que seja feito o acesso ]
        ///na pasta do servidor
        /// </summary>
        /// <param name="url"></param>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public String getCaminho(string url, string codigo)
        {
            String newUrl;

            if (!string.IsNullOrEmpty(url) || !string.IsNullOrWhiteSpace(url))
            {
                newUrl = url.Replace(Settings.Default.CaminhoImagens, "/Imagens/").Replace("+", "_").Trim();
                
                string[] img = newUrl.Split('\\');//pega o nome da imagem
                var last = img.Length-1;
                var filename = img[last];
                var path = @"\Imagens\Alto-verão 13\"+filename;

                //string[] caminho = img[0].Split('/');//pega o restante do caminho da imagem e divide por pastas

                StringBuilder diretorio = new StringBuilder();
                diretorio.Append(AppDomain.CurrentDomain.BaseDirectory);

                if (img[0].ToUpper() == "C:")
                    img = path.Split('\\');

                //\Imagens\Alto-verão 13\a10982.jpg
                
                diretorio.Append(img[0]);

                if (!Diretorio.Existe(diretorio.ToString()))
                    Diretorio.Criar(diretorio.ToString());

                string[] imagens = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + img[0] + img[1], img[2], SearchOption.TopDirectoryOnly);

                if (imagens.Length < 1)
                {
                    newUrl = @"~\Imagens\Template\semImagem.gif";
                    GravarLog.gravarLogError(String.Format("A imagem do produto [ {0} ] nomeada como [ {1} ] não foi encontrada na pasta [ {2} ]", codigo, img[2], img[1]), "Falta Imagem");
                }
                else
                {
                    newUrl = "~" + newUrl;
                }
            }
            else
            {
                newUrl = @"~\Imagens\Template\semImagem.gif";
                GravarLog.gravarLogError(String.Format("Não foi cadastrado imagem para o produto:[ {0} ] ", codigo), "Falta Imagem");
            }
            return newUrl;
        }
    }
}