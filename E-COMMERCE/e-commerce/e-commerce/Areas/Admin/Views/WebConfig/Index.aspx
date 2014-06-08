<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/PageMasterAdmLog.Master" Inherits="System.Web.Mvc.ViewPage<e_commerce.Areas.Admin.Models.Classes.ConfWebConfig>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    .:: Configurações ::.
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <script>
        $(document).ready(function () {
            document.getElementById('caminhoImagens').disabled = true;
        });
    </script>
   
    <div id="tudo">
        <table style="width: 920px; border: 1px solid; border-radius: 5px; border-color: #ccc; padding: 5px 5px 5px 15px; margin: 5px 0 0 15px;">

            <tr>
                <td><strong style="font-size: 16px;">Configurações do Sistema</strong></td>
                <td style="text-align: right"><strong style="font-size: 11px; color: #808080">Todos os campos são de preenchimento obrigatório!</strong><br /><span style="font-size: 11px; color: #808080">Coloque o cusor do mouse sobre o campos para ver uma nota do mesmo</span> </td>
            </tr>
        </table>
    </div>
    <% using (Html.BeginForm())
       { %>
    <%: Html.ValidationSummary(true) %>
    <div id="divFisica">

        <table style="width: 920px; border: 1px solid; border-radius: 5px; border-color: #ccc; padding: 10px 0 5px 0; margin: 5px 0 0 15px;">
            <tr>
                <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.caminhoImagens) %>
                </td>
                <td class="editor-field">
                    <%: Html.TextBoxFor(model => model.caminhoImagens, new { @style="width: 350px;",EditableAttribute=false, Title="Pasta raiz do PC onde estão as imagens do site.              Ex: C:\\SYS_VIP\\ECOMMERCE\\IMAGENS"}) %>
                    <%: Html.ValidationMessageFor(model => model.caminhoImagens) %>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.rede) %>
                </td>
                <td class="editor-field">
                    <%: Html.TextBoxFor(model => model.rede, new { @style="width: 150px;",Title="Rede da loja"}) %>
                    <%: Html.ValidationMessageFor(model => model.rede) %>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.filial) %>
                </td>
                <td class="editor-field">
                    <%: Html.TextBoxFor(model => model.filial, new { @style="width: 150px;",Title="Filial da loja"}) %>
                    <%: Html.ValidationMessageFor(model => model.filial) %>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.filtraEstoque) %>
                </td>
                <td class="editor-field">

                    <%: Html.DropDownListFor(model => model.filtraEstoque, new[]{ new SelectListItem(){ Text = "Não", Value = "1"},
                                                                           new SelectListItem(){ Text = "Sim", Value = "0"}}, new {@style="width: 70px;",Title="Define se irá mostrar os produtos que têm estoque"})%>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.idsProdutosBanner) %>
                </td>
                <td class="editor-field">
                    <%: Html.TextAreaFor(model => model.idsProdutosBanner, new { @style="width: 350px; min-height: 150px; max-height:250px;  max-width: 350px;",Title="Códigos dos produtos que estão no banner da tela inicial. Obs. Estes códigos devem ser inseridos na mesma ordem das imagens e separados por virgula"}) %>
                    <%: Html.ValidationMessageFor(model => model.idsProdutosBanner) %>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.qtdeElemntosPaginaInicial) %>
                </td>
                <td class="editor-field">
                    <%: Html.TextBoxFor(model => model.qtdeElemntosPaginaInicial, new { @style="width: 100px;",Title="Total de imagens que iram aparecer na tela inicial do site"}) %>
                    <%: Html.ValidationMessageFor(model => model.qtdeElemntosPaginaInicial) %>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.msgCarrinho) %>
                </td>
                <td class="editor-field">
                    <%: Html.TextAreaFor(model => model.msgCarrinho, new { @style="width: 350px; min-height: 50px; max-height:250px;  max-width: 350px;",Title="Menssagem que aparecerá na tela do carrinho quando o mesmo estiver vazio"}) %>
                    <%: Html.ValidationMessageFor(model => model.msgCarrinho) %>
                </td>
            </tr>
            <tr>
                 <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.emailCredential) %>
                </td>
                <td class="editor-field">
                    <%: Html.TextBoxFor(model => model.emailCredential, new { @style="width: 350px;",Title="E-mail que cadastrou no PagSeguro para receber pagamentos"}) %>
                    <%: Html.ValidationMessageFor(model => model.emailCredential) %>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.tokenCredential) %>
                </td>
                <td class="editor-field">
                    <%: Html.TextBoxFor(model => model.tokenCredential, new { @style="width: 350px;",Title="Token fornecido pelo PagSeguro"}) %>
                    <%: Html.ValidationMessageFor(model => model.tokenCredential) %>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.tema) %>
                </td>
                <td class="editor-field">

                    <%: Html.DropDownListFor(model => model.tema, new[]{ new SelectListItem(){ Text = "Azul", Value = "Azul"},
                                                                        new SelectListItem(){ Text = "Cinza", Value = "Cinza"}}, new {@style="width: 100px;",Title="Define a cor do template do site"})%>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.linkSite) %>
                </td>
                 <td class="editor-field">
                    <%: Html.TextBoxFor(model => model.linkSite, new { @style="width: 350px;",Title="Link do seu e-Commerce. Atenção: Não colocar http:// ou https:// coloque somente www.meusite.com.br"}) %>
                    <%: Html.ValidationMessageFor(model => model.linkSite) %>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.emailContato) %>
                </td>
                <td class="editor-field">
                    <%: Html.TextBoxFor(model => model.emailContato, new { @style="width: 350px;", Title="Este e-mail será utilizado para reenviar a senha do usuário"}) %>
                    <%: Html.ValidationMessageFor(model => model.emailContato) %>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.senhaEmail) %>
                </td>
                <td class="editor-field">
                    <%: Html.EditorFor(model => model.senhaEmail,new { @style="width: 350px;", Title="Senha do e-mail utilizado para reenviar a senha do usuário"}) %>
                    <%: Html.ValidationMessageFor(model => model.senhaEmail) %>
                </td>
            </tr>

             <tr>
                <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.emailContatoSite) %>
                </td>
                <td class="editor-field">
                    <%: Html.TextBoxFor(model => model.emailContatoSite, new { @style="width: 350px;", Title="Este e-mail será utilizado para reenviar a senha do usuário"}) %>
                    <%: Html.ValidationMessageFor(model => model.emailContatoSite) %>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.senhaEmailContatoSite) %>
                </td>
                <td class="editor-field">
                    <%: Html.EditorFor(model => model.senhaEmailContatoSite,new { @style="width: 350px;", Title="Senha do e-mail utilizado para reenviar a senha do usuário"}) %>
                    <%: Html.ValidationMessageFor(model => model.senhaEmailContatoSite) %>
                </td>
            </tr>

            <tr>
                <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.ServidorSmtp) %>
                </td>
                <td class="editor-field">
                    <%: Html.TextBoxFor(model => model.ServidorSmtp,new { @style="width: 250px;", Title="Servidor SMTP de envio de email, esta informação deve ser verificada junto ao seu servidor de e-mails"}) %>
                    <%: Html.ValidationMessageFor(model => model.ServidorSmtp) %>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.respFrete) %>
                </td>
                <td class="editor-field">

                    <%: Html.DropDownListFor(model => model.respFrete, new[]{ new SelectListItem(){ Text = "Emitente", Value = "1"},
                                                                       new SelectListItem(){ Text = "Destinatário", Value = "2"}}, new {@style="width: 150px;",Title="Define quem arcará com as despesas do frete"})%>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.pesoMinCorreio) %>
                </td>
                <td class="editor-field">
                    <%: Html.TextBoxFor(model => model.pesoMinCorreio, new { @style="width: 100px;", Title="Define o peso minimo que os correios utilizam para calcular o frete. Obs. Neste campo deve ser inserido somente valores numéricos, se peso do item for de 1KG deve-se inserir 1000."}) %>
                    <%: Html.ValidationMessageFor(model => model.pesoMinCorreio) %>
                </td>
            </tr>
            <tr>

                <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.cepEnvio) %>
                </td>
                <td class="editor-field">
                    <%: Html.TextBoxFor(model => model.cepEnvio,new { @style="width: 180px;", Title="CEP de onde se encontra sua loja ou local de onde seram enviados os produtos"}) %>
                    <%: Html.ValidationMessageFor(model => model.cepEnvio) %>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:10px">
                    <%: Html.LabelFor(model => model.reservaBtComprar) %>
                </td>
                <td class="editor-field">
                    <%: Html.DropDownListFor(model => model.reservaBtComprar, new[]{ new SelectListItem(){ Text = "Não", Value = "False"},
                                                                              new SelectListItem(){ Text = "Sim", Value = "True"}}, new {@style="width: 80px;",Title="Define se a reserva dos itens seram feitas antes de ser redirecionado para o site do PagSeguro"})%>
                </td>
            </tr>
        </table>
    </div>
    <div style="width: 920px; float: left; /*border: 1px solid; border-radius: 5px; border-color: #ccc; */  /*padding: 5px 0 5px 10px; */ margin: 5px 0 90px 15px;">
            <div style="float: right;">
                <input class="btn btn-primary" type="submit" value="Salvar Alterações" />
            </div>
        </div>

    <% } %>
</asp:Content>
