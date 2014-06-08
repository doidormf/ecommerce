using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace e_commerce.Helpers
{
    public class CalculoFrete
    {
        public string CalcularFreteValorData()
        {
            //40010 SEDEX Varejo
            //40045 SEDEX a Cobrar Varejo
            //40215 SEDEX 10 Varejo
            //40290 SEDEX Hoje Varejo
            //41106 PAC Varejo

            // Dados da empresa, se tiver contrato com os Correios
            string nCdEmpresa = string.Empty;
            string sDsSenha = string.Empty;
            string retorno = string.Empty;
            string retornoErro = string.Empty;
            // Código do tipo de frete - por padrão deixei o SEDEX
            string nCdServico = "40010,41106";
            // Cep de origem e destino - apenas números
            string sCepOrigem = "40280000";
            string sCepDestino = "40280000";
            // Peso total da encomenda - por padrão deixei 1kg
            string nVlPeso = "1";
            // Formato da encomenda - por padrão deixei caixa
            int nCdFormato = 1;
            // Para encomenda do tipo PAC, deve-se preencher a dimensão da embalagem
            decimal nVlComprimento = 20;
            decimal nVlAltura = 20;
            decimal nVlLargura = 20;
            decimal nVlDiametro = 20;
            // Informa se é por mão própria - por padrão deixei Não
            string sCdMaoPropria = "N";
            // Valor declarado - por padrão não informo
            decimal nVlValorDeclarado = 10;
            // Se desejo recebr aviso de recebimento - por padrão não quero
            string sCdAvisoRecebimento = "N";


            // Instancio o web-service
            Correios.CalcPrecoPrazoWSSoapClient webServiceCorreios = new Correios.CalcPrecoPrazoWSSoapClient();


            // Efetuo a requisição
            Correios.cResultado retornoCorreios = webServiceCorreios.CalcPrecoPrazo(nCdEmpresa, sDsSenha, nCdServico, sCepOrigem, sCepDestino, nVlPeso, nCdFormato, nVlComprimento, nVlAltura, nVlLargura, nVlDiametro, sCdMaoPropria, nVlValorDeclarado, sCdAvisoRecebimento);

            // correios.cResultado retornoCorreios = webServiceCorreios.CalcPrazoData("40010", "06445130", "05311900", "03/05/2013");

            //correios.cResultado retornoCorreios2 =  webServiceCorreios.
            // Verifico se há retorno
            if (retornoCorreios.Servicos.Length > 0)
            {
                foreach (var item in retornoCorreios.Servicos.ToList())
                {
                    if (item.Erro == "0")
                    {
                        // Se deu tudo certo, então retorna o valor
                        retorno = "R$ " + item.Valor;
                    }
                    else
                    {
                        retornoErro = item.MsgErro;
                        retorno = "R$ " + item.Valor;
                    }

                }
            }
            else
            {
                retorno = "01"; //"NÃO FOI POSSÍVEL CONSULTAR O SERVIÇO DESEJADO!";
            }


            return retorno;
        }

        public int CalcularDataEntrega(string tipoServico, string cepOrigem, string cepDestino, string dataPostagem)
        { 
            string retorno = string.Empty;
            string tipoEntrega = tipoServicoCorreios(tipoServico);
            // Instancio o web-service
            Correios.CalcPrecoPrazoWSSoapClient webServiceCorreios = new Correios.CalcPrecoPrazoWSSoapClient();

            //calcula o prazo de entrega
            Correios.cResultado retornoCorreios = webServiceCorreios.CalcPrazoData(tipoEntrega, cepOrigem.Replace("-", "").Trim(), cepDestino.Replace("-", "").Trim(), dataPostagem);

            if (retornoCorreios.Servicos.Length > 0)
            {
                // Se deu tudo certo, então retorna o valor
                if (retornoCorreios.Servicos[0].Erro == "0")
                {
                   retorno = retornoCorreios.Servicos[0].PrazoEntrega;
                }
                else
                {
                    String ret = retornoCorreios.Servicos[0].MsgErro;
                    retorno = retornoCorreios.Servicos[0].PrazoEntrega;
                }
            }
            else
            {
                retorno = "-1";//"NÃO FOI POSSÍVEL CONSULTAR O SERVIÇO DESEJADO!";
            }

            return Convert.ToInt32(retorno);
        }

        public string tipoServicoCorreios(string tipoServico) {

            string retorno = string.Empty;

            switch (tipoServico)
            {
                case "1":
                    retorno = "40010";// SEDEX Varejo
                    break;
                case "2":
                    retorno = "41106";// PAC Varejo
                    break;
                default:
                    retorno = "41106";
                    break;
            }
            return retorno;
        }

    }
}