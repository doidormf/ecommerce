using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Uol.PagSeguro;

/*
 * Data:  20/03/2013
 * Autor: Marcos Manoel da Silva
 *
 * Esta classe tem por objetivo gravar arquivos de LOG, esta classe é composta pelo seguinte método:
 *
 * gravar(Exception ex, StackTrace trace):
 *
 * Método do tipo void, que faz a gravação do log contendo:
 * Data:   data em que ocorreu o erro.
 * Hora:   hora em que ocorreu o erro.
 * Classe: classe que gerou o erro.
 * Método: método que gerou o erro.
 * Linha:  linha do erro.
 * Erro:   Erro causado.
 */

namespace e_commerce.Helpers
{
    public static class GravarLog
    {
        /// <summary>
        /// Grava o LOG de erro com a excessão que foi gerada e o rastro do erro.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="trace"></param>
        public static void gravarLogError(Exception ex, StackTrace trace)
        {
            try
            {
                criarDiretorio();

                using (StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "/Logs", true))
                {
                    writer.WriteLine("Data:   " + DateTime.Now.ToString("dd/MM/yyyy"));
                    writer.WriteLine("hora:   " + DateTime.Now.ToString("HH:mm:ss"));
                    writer.WriteLine("Classe: " + trace.GetFrame(0).GetMethod().ReflectedType);
                    writer.WriteLine("Metodo: " + trace.GetFrame(0).GetMethod());
                    writer.WriteLine("Linha:  " + trace.GetFrame(0).GetFileLineNumber());
                    writer.WriteLine("Erro:   " + ex.Message);
                    writer.WriteLine("********************************************************************************************************************************************************************");
                    writer.Close();
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("Acesso negado.\nUsuário não tem permissão para gravar em:\n" + AppDomain.CurrentDomain.BaseDirectory + @"Logs\LOG.txt" + "\nContate o adminstrador.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException("Diretório não encontrado .\nDiretório: " + AppDomain.CurrentDomain.BaseDirectory + @"Logs" + "não encontrado, verifique se o nome está correto e tente novamente." + "\nPara maiores informações contate o adminstrador.");
            }
            catch (Exception exe)
            {
                throw new Exception("Erro\n" + exe.Message);
            }
        }

        /// <summary>
        /// Grava uma mensagem de erro personalizada e o rastro do erro,
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="trace"></param>
        public static void gravarLogError(String erro, StackTrace trace)
        {
            try
            {
                criarDiretorio();

                using (StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "/Logs", true))
                {
                    writer.WriteLine("Data:   " + DateTime.Now.ToString("dd/MM/yyyy"));
                    writer.WriteLine("hora:   " + DateTime.Now.ToString("HH:mm:ss"));
                    writer.WriteLine("Classe: " + trace.GetFrame(0).GetMethod().ReflectedType);
                    writer.WriteLine("Metodo: " + trace.GetFrame(0).GetMethod());
                    writer.WriteLine("Linha:  " + trace.GetFrame(0).GetFileLineNumber());
                    writer.WriteLine("Erro:   " + erro);
                    writer.WriteLine("********************************************************************************************************************************************************************");
                    writer.Close();
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("Acesso negado.\nUsuário não tem permissão para gravar em:\n" + AppDomain.CurrentDomain.BaseDirectory + "/Logs" + "\nContate o adminstrador.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException("Diretório não encontrado .\nDiretório: " + AppDomain.CurrentDomain.BaseDirectory + @"Logs" + "não encontrado, verifique se o nome está correto e tente novamente." + "\nPara maiores informações contate o adminstrador.");
            }
            catch (Exception exe)
            {
                throw new Exception("Erro\n" + exe.Message);
            }
        }

        /// <summary>
        /// Grava uma mensagem de erro personalizada.
        /// </summary>
        /// <param name="ex"></param>
        public static void gravarLogError(String ex, String arquivo)
        {
            try
            {
                criarDiretorio();

                using (StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "/Logs/" + arquivo + ".txt", true))
                {
                    writer.WriteLine("Data:   " + DateTime.Now.ToString("dd/MM/yyyy"));
                    writer.WriteLine("hora:   " + DateTime.Now.ToString("HH:mm:ss"));
                    writer.WriteLine("Erro:   " + ex);
                    writer.WriteLine("********************************************************************************************************************************************************************");
                    writer.Close();
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("Acesso negado.\nUsuário não tem permissão para gravar em:\n" + AppDomain.CurrentDomain.BaseDirectory + "/Logs" + "\nContate o adminstrador.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException("Diretório não encontrado .\nDiretório: " + AppDomain.CurrentDomain.BaseDirectory + @"Logs" + "não encontrado, verifique se o nome está correto e tente novamente." + "\nPara maiores informações contate o adminstrador.");
            }
            catch (Exception exe)
            {
                throw new Exception("Erro\n" + exe.Message);
            }
        }

        public static void gravarMsgLog(int status, String referencia, String codigo, String data, String valor, IList<Item> lista, String arquivo)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "/Logs/" + arquivo + ".txt", true))
                {
                    writer.WriteLine("Data:   " + DateTime.Now.ToString("dd/MM/yyyy"));
                    writer.WriteLine("hora:   " + DateTime.Now.ToString("HH:mm:ss"));
                    writer.WriteLine("Status: " + status);
                    writer.WriteLine("Referencia: " + referencia);
                    writer.WriteLine("Codigo: " + codigo);
                    writer.WriteLine("Data:   " + data);
                    foreach (var item in lista)
                    {
                        writer.WriteLine("item ID:   " + item.Id);
                        writer.WriteLine("item Descrição:   " + item.Description);
                        writer.WriteLine("item Quantiadae:   " + item.Quantity);
                        writer.WriteLine("item Valor:   " + item.Amount);
                    }

                    writer.WriteLine("Valor da compra:  " + valor);
                    writer.WriteLine("********************************************************************************************************************************************************************");
                    writer.Close();
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("Acesso negado.\nUsuário não tem permissão para gravar em:\n" + AppDomain.CurrentDomain.BaseDirectory + "/Logs" + "\nContate o adminstrador.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException("Diretório não encontrado .\nDiretório: " + AppDomain.CurrentDomain.BaseDirectory + "/Logs" + "não encontrado, verifique se o nome está correto e tente novamente." + "\nPara maiores informações contate o adminstrador.");
            }
            catch (Exception exe)
            {
                throw new Exception("Erro\n" + exe.Message);
            }
        }

        /// <summary>
        /// Cria uma pasta chamada Logs no diretório de instalação da apalicação.
        /// </summary>
        /// <returns></returns>
        private static void criarDiretorio()
        {
            try
            {
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Logs"))
                {
                    try
                    {
                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Logs");
                    }
                    catch (UnauthorizedAccessException)
                    {
                        throw new UnauthorizedAccessException("Acesso negado.\nUsuário não tem permissão para gravar em:\n" + AppDomain.CurrentDomain.BaseDirectory + "/Logs" + "\nContate o adminstrador.");
                    }
                    catch (DirectoryNotFoundException)
                    {
                        throw new DirectoryNotFoundException("Diretório não encontrado .\nDiretório: " + AppDomain.CurrentDomain.BaseDirectory + " não encontrado, verifique se o nome está correto e tente novamente." + "\nPara maiores informações contate o adminstrador.");
                    }
                    catch (Exception exe)
                    {
                        throw new Exception("Erro\n" + exe.Message);
                    }
                }
            }
            catch (AppDomainUnloadedException)
            {
                throw new AppDomainUnloadedException("Aplicativo descarregado");
            }
        }
    }
}