using System;
using System.Data.Objects;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using e_commerce.Helpers;
using e_commerce.Models;
using e_commerce.Models.Classes;
using e_commerce.Models.Repositorios;
using e_commerce.Properties;
using e_commerce.ServiceEndereco;
using VipWebUtils.Helpers.Security;

namespace e_commerce.Controllers
{
    public class AccountController : Controller
    {
        private ProdutosDao produtos = new ProdutosDao();
        private ClientesDao clientes = new ClientesDao();
        private HttpCookie cookie;

        public ActionResult LogOn(linkModel model)
        {
            ViewBag.Tema = Settings.Default.Tema;

            ViewBag.link = model.link;

            return View();
        }

        /// <summary>
        /// Função usada para efetuar o login do
        /// usuário
        /// </summary>
        /// <author>Claudinei Nascimento</author>
        /// <business>Vip-Systems Tecnologia & Inovação LTDA></business>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            ViewBag.Tema = Settings.Default.Tema;

            cookie = new HttpCookie("usuario");

            if (ModelState.IsValid)
            {
                String senha = Crypt.CriptografaM(model.Password);

                ecomm_clientes result = clientes.getLogin(model.UserName, senha);

                if (result != null)
                {
                    FormsAuthentication.SetAuthCookie(result.nome, model.RememberMe);


                    //String usuario = Crypt.Crypter(result.codigo);
                    String usuario = result.codigo;

                    if (Request.Cookies["usuario"] == null)
                    {
                        cookie.Expires = DateTime.Now.AddMinutes(Settings.Default.IntervaloLimpezaCookies);

                        Response.Cookies.Add(cookie);
                    }
                    cookie.Values.Add(usuario, null);

                    Response.Cookies.Add(cookie);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        ViewBag.Tema = Settings.Default.Tema;

                        return Redirect(returnUrl);
                    }
                    else
                    {
                        ViewBag.Tema = Settings.Default.Tema;

                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nome de usuário e/ou senha inválidos");
                }
            }
            return View(model);
        }

        public ActionResult BuscaCep(string cep)
        {
            //e_commerce.Helpers.Endereco endereco = new e_commerce.Helpers.Endereco();
            //RegisterModel endereco = new RegisterModel();
            e_commerce.Helpers.BuscaCepClass apiObj = new e_commerce.Helpers.BuscaCepClass();

            RegisterModel endereco = apiObj.ConsultaCep(cep);
            if (endereco.Endereco_Res == null)
                endereco.Endereco_Res = "Endereço não encontrado";

            return Json(endereco, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Função que verifica se o e-mail que o usuário
        /// está informando no cadastro, já está cadastrado
        /// no banco de dados.
        /// Se estiver cadastrado não ira permitir que seja efetuado o
        /// cadastro com esse e-mail
        /// </summary>
        /// <author>Claudinei Nascimento</author>
        /// <business>Vip-Systems Tecnologia & Inovação LTDA></business>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public ActionResult GetUser(String usuario)
        {
            int elemento = 0;

            ecomm_clientes result = clientes.getUsuarioCadastrado(usuario);

            if (result != null)
            {
                elemento = 1;
            }

            return Json(elemento, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCpf(String usuario)
        {
            int elemento = 0;

            ecomm_clientes result = clientes.getUsuarioCadastradobyCPF(usuario);

            if (result != null)
            {
                elemento = 1; 
            }
            
            return Json(elemento, JsonRequestBehavior.AllowGet);
        }

        /*
        public ActionResult Existe(String usuario)
        {
            ecomm_clientes result = clientes.getUsuarioCadastradobyCPF(usuario);

            if (result != null) return Json(true, JsonRequestBehavior.AllowGet);

            return Json(false, JsonRequestBehavior.AllowGet);
        }
         */

        /// <summary>
        /// Busca as informações do usuário e preenche o formla´rio com as mesmas
        /// </summary>
        /// <author>Claudinei Nascimento</author>
        /// <business>Vip-Systems Tecnologia & Inovação LTDA></business>
        /// <returns></returns>
        public ActionResult SearchCadastro()
        {
            ViewBag.Tema = Settings.Default.Tema;

            HttpCookie cookie = (HttpCookie)Request.Cookies["usuario"];

            if (cookie.Values.AllKeys[0] == null)
            {
                //linkModel model = new linkModel();
                //model.link = "/Carrinho/IndexCarrinho";
                //return RedirectToAction("LogOn", "Account", model);
                return RedirectToAction("LogOn", "Account");
            }

            ecomm_clientes usuario = clientes.getUsuarioById(cookie.Values.AllKeys[0]);
            // usuario.DATA_NASCIMENTO = usuario.DATA_NASCIMENTO.ToShortDateString();

            usuario.PASSAWORD = string.Empty;
            usuario.ConfirmPassword = string.Empty;
            usuario.EmailConf = string.Empty;

            return View(usuario);
        }

        /// <summary>
        /// Função responsável por alterar o cadastro do
        /// usuário
        /// </summary>
        /// <author>Claudinei Nascimento</author>
        /// <business>Vip-Systems Tecnologia & Inovação LTDA></business>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchCadastro(ecomm_clientes model, string edtConfPassord)
        {
            ViewBag.Tema = Settings.Default.Tema;

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.CPF_CNPJ))
                {
                    if (model.PESSOA == null)
                    {
                        model.PESSOA = "F";
                    }
                    if (model.OBSERVACOES == null)
                    {
                        model.OBSERVACOES = "";
                    }
                    if (model.CEP_ENTREGA == null)
                    {
                        model.CEP_ENTREGA = model.CEP_RESIDENCIAL;
                        model.ENDERECO_ENTREGA = model.ENDERECO_RESIDENCIAL;
                        model.NRO_ENTREGA = model.NRO_RESIDENCIAL;
                        model.BAIRRO_ENTREGA = model.BAIRRO_RESIDENCIAL;
                        model.CIDADE_ENTREGA = model.CIDADE_RESIDENCIAL;
                        model.UF_ENTREGA = model.UF_RESIDENCIAL;
                        model.COMPLEMENTO_ENTREGA = model.COMPLEMENTO_RESIDENCIAL;
                    }
                    if (model.CEP_COBRANCA == null)
                    {
                        model.CEP_COBRANCA = model.CEP_RESIDENCIAL;
                        model.ENDERECO_COBRANCA = model.ENDERECO_RESIDENCIAL;
                        model.NRO_COBRANCA = model.NRO_RESIDENCIAL;
                        model.BAIRRO_COBRANCA = model.BAIRRO_RESIDENCIAL;
                        model.CIDADE_COBRANCA = model.CIDADE_RESIDENCIAL;
                        model.UF_COBRANCA = model.UF_ENTREGA;
                        model.COMPLEMENTO_COBRANCA = model.COMPLEMENTO_RESIDENCIAL;
                    }
                    String senha = Crypt.CriptografaM(model.PASSAWORD);
                    //// Attempt to register the user
                    //MembershipCreateStatus createStatus;
                    ObjectResult<eComm_Atucli_Result> result = null;
                    result = produtos.setCliente(model.codigo, model.nome, model.RG_IE, model.CPF_CNPJ, model.FANTASIA_APELIDO, model.CODVENDEDOR, model.PESSOA, model.TELEFONE1, model.TELEFONE2, model.TELEFONE3,
                        model.FAX, model.EMAIL1, model.EMAIL2, model.HOMEPAGE, model.DATA_NASCIMENTO, model.PROFISSAO, Settings.Default.Rede, Settings.Default.Filial, model.SEXO, model.INSCRICAO_SUFRAMA,
                        senha, model.CLIENTE_BLOQUEADO, model.CEP_RESIDENCIAL, model.ENDERECO_RESIDENCIAL, model.NRO_RESIDENCIAL, model.BAIRRO_RESIDENCIAL, model.CIDADE_RESIDENCIAL, model.UF_RESIDENCIAL, model.COMPLEMENTO_RESIDENCIAL, model.CEP_COBRANCA, model.ENDERECO_COBRANCA,
                        model.NRO_COBRANCA, model.BAIRRO_COBRANCA, model.CIDADE_COBRANCA, model.UF_COBRANCA, model.COMPLEMENTO_COBRANCA, model.CEP_ENTREGA, model.ENDERECO_ENTREGA, model.NRO_ENTREGA, model.BAIRRO_ENTREGA, model.CIDADE_ENTREGA, model.UF_ENTREGA,
                        model.COMPLEMENTO_ENTREGA, model.CONTA_CONTABIL, model.ID_INTEGRA, model.AUTORIZA_ENVIO_SMS, model.OBSERVACOES, model.LIMITE_DE_CREDITO, model.EMAIL1);

                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            if (!item.pCodigo.Equals(0))
                            {
                                FormsAuthentication.SetAuthCookie(model.nome, false /* createPersistentCookie */);

                                ViewBag.Tema = Settings.Default.Tema;

                                ModelState.AddModelError("", "Dados Atualizados");
                            }
                            else
                            {
                                ModelState.AddModelError("", item.MsgErro);
                            }
                        }
                        return RedirectToAction("SearchCadastro", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ocorreram erros no formulário. Por favor corrija os erros e tente novamente");
                    }
                }
            }
            else
            {
                return View(model);
            }

            ViewBag.Tema = Settings.Default.Tema;

            return View();
        }

        /// <summary>
        /// Desconecta o suário do sistema
        /// </summary>
        /// <author>Claudinei Nascimento</author>
        /// <business>Vip-Systems Tecnologia & Inovação LTDA></business>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            ViewBag.Tema = Settings.Default.Tema;

            FormsAuthentication.SignOut();

            HttpCookie cookie = (HttpCookie)Request.Cookies["usuario"];

            if (cookie.Values.AllKeys[0] != null)
            {
                RemoverUsuario(cookie.Values.AllKeys[0]);
            }


            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Abre o formulário para cadastro do usuário
        /// </summary>
        /// <author>Claudinei Nascimento</author>
        /// <business>Vip-Systems Tecnologia & Inovação LTDA></business>
        /// <returns></returns>
        public ActionResult Register()
        {
            ViewBag.Tema = Settings.Default.Tema;

            return View();
        }

        /// <summary>
        /// Insere o registro do ususário no banco de dados
        /// </summary>
        /// <author>Claudinei Nascimento</author>
        /// <business>Vip-Systems Tecnologia & Inovação LTDA></business>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            ViewBag.Tema = Settings.Default.Tema;

            const string codCli = "";
            if (!string.IsNullOrEmpty(model.CPF_CNPJ))
            {
                if (model.Pessoa == null)
                {
                    model.Pessoa = "F";
                }
                if (model.Cep_Ent == null)
                {
                    model.Cep_Ent = model.Cep_Res;
                    model.Endereco_Ent = model.Endereco_Res;
                    model.Nro_Ent = model.Nro_Res;
                    model.Bairro_Ent = model.Bairro_Res;
                    model.Cidade_Ent = model.Cidade_Res;
                    model.Uf_Ent = model.Uf_Res;
                    model.Complemento_Ent = model.Complemento_Res;
                }
                if (model.Cep_Cob == null)
                {
                    model.Cep_Cob = model.Cep_Res;
                    model.Endereco_Cob = model.Endereco_Res;
                    model.Nro_Cob = model.Nro_Res;
                    model.Bairro_Cob = model.Bairro_Res;
                    model.Cidade_Cob = model.Cidade_Res;
                    model.Uf_Cob = model.Uf_Res;
                    model.Complemento_Cob = model.Complemento_Res;
                }

                //String senha = Crypt.Crypter(model.Password); 
                String senha = string.Empty;
                senha = Crypt.CriptografaM(model.Password);

                ObjectResult<eComm_Atucli_Result> result = null;
                result = produtos.setCliente(codCli, model.Nome, model.Rg_ie, model.CPF_CNPJ, model.Fantasia_Apelido, model.CodVendedor, model.Pessoa, model.Telefone1, model.Telefone2, model.Telefone3,
                    model.Fax, model.Email1, model.Email2, model.HomePage, model.Data_Nascimento, model.Profissao, Settings.Default.Rede, Settings.Default.Filial, model.Sexo, model.Inscricao_Suframa,
                    senha, model.Boqueado, model.Cep_Res, model.Endereco_Res, model.Nro_Res, model.Bairro_Res, model.Cidade_Res, model.Uf_Res, model.Complemento_Res, model.Cep_Cob, model.Endereco_Cob,
                    model.Nro_Cob, model.Bairro_Cob, model.Cidade_Cob, model.Uf_Cob, model.Complemento_Cob, model.Cep_Ent, model.Endereco_Ent, model.Nro_Ent, model.Bairro_Ent, model.Cidade_Ent, model.Uf_Ent,
                    model.Complemento_Ent, model.Conta_Contabil, model.IdIntegra, model.Autor_Sms, model.ObsCli, model.LimiteCredito, model.Email1);

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        if (!item.pCodigo.Equals(0))
                        {
                            cookie = new HttpCookie("usuario");

                            FormsAuthentication.SetAuthCookie(model.Nome, false /* createPersistentCookie */);

                            String usuario = Crypt.Crypter(item.pCodigo);

                            cookie.Values.Add(usuario, null);

                            Response.Cookies.Add(cookie);

                            ViewBag.Tema = Settings.Default.Tema;
                        }
                        else
                        {
                            ModelState.AddModelError("", item.MsgErro);
                        }
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Ocorreram erros no formulário. Verifique as informações digitadas e tente novamente");
                }
            }
            ViewBag.Tema = Settings.Default.Tema;

            return View(model);
        }

        public ActionResult RecuperarSenha()
        {
            ViewBag.Tema = Settings.Default.Tema;

            return View();
        }

        [HttpPost]
        public ActionResult RecuperarSenha(RecuperarSenha model)
        {
            if (!ModelState.IsValid) return PartialView("RecuperarSenha");

            ecomm_clientes user = clientes.getUsuarioCadastrado(model.email);

            if (user == null)
            {
                ViewBag.Menssagem = "O e-mail informado não consta em nossa base de dados";

                //ModelState.AddModelError("", "O e-mail informado não consta em nossa base de dados");

                return PartialView("RecuperarSenhaConf");
            }
            else
            {
                SetEmail email = new SetEmail();
                StringBuilder sb = new StringBuilder();
                sb.Append("<div style='font-size:14px; border: 1px solid #ccc; margin:5% 10% 0 10%;'>");
                sb.Append("<div style='background-color: #ccc;padding: 10px 10px 10px 10px; color:#fff; height: 60px; margin-bottom:15px;'><img src='http://" + Settings.Default.LinkSite + "/e-commerce/Imagens/Template/logo_small.png' /></div>");
                sb.Append("<div style='padding: 5px 5px 5px 5px;'>");
                sb.Append("<table>");
                sb.Append("<tr><td> ");
                sb.Append("<strong>Usuário:</strong> " + user.LOGIN);
                sb.Append("</td></tr>");
                sb.Append("<tr><td>");
                sb.Append("<strong>Senha:</strong> " + Crypt.DecriptografaM(user.PASSAWORD));
                sb.Append("</td></tr>");
                sb.Append("</table>");
                sb.Append("<div style='font-size:12px; color:#000; margin-bottom:15px;'>");
                sb.Append("<p>Estas são suas informações para login em nossa loja, caso não tenha feito a solicitação das mesmas, " +
                "pedimos que se dirija ao site e efetue a troca de sua senha.</p>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("<div style='background-color: #ccc;text-align:center; vertical-align:central;font-size:10px;padding: 10px; color:#000; height:30px;'>");
                sb.Append("<p>Esta é uma mensagem automática. Por favor, não a responda.</p>");
                sb.Append("</div>");
                sb.Append("</div>");

                ViewBag.Menssagem = email.SendEmail(user.LOGIN, "Recuperação de Senha", sb.ToString(), Settings.Default.EmailContato, Settings.Default.SenhaEmail);
            }

            return PartialView("RecuperarSenhaConf");
        }

        #region Funções não usadas até o momento

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewBag.Tema = Settings.Default.Tema;

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            ViewBag.Tema = Settings.Default.Tema;

            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        protected void RemoverUsuario(string idUsuario)
        {
            // Resgata o cookie
            HttpCookie cookie = (HttpCookie)Request.Cookies["usuario"];

            // Remove o id do produto do cookie
            cookie.Values.Remove(idUsuario);

            // Grava o cookie

            Response.Cookies.Add(cookie);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            ViewBag.Tema = Settings.Default.Tema;

            return View();
        }

        #endregion Funções não usadas até o momento
    }
}