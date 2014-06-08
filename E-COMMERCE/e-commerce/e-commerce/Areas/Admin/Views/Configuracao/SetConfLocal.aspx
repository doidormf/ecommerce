<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/PageMasterAdmLog.Master" Inherits="System.Web.Mvc.ViewPage<e_commerce.Areas.Admin.Models.ConfLocal>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    SetConfLocal
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
   
    <div class="formredefilial">
         <div class="topredefilial">
    <strong>Rede e filial da loja local</strong><br />
    
    </div>
        <% using (Ajax.BeginForm("SetConfLocal", "Configuracao", new { IdOpcaoMenu = Session["IdOpcaoMenu"] }, new AjaxOptions()
                {
                    InsertionMode = InsertionMode.Replace,
                    UpdateTargetId = "retorno",
                    OnSuccess = "modal(2)"

                }, new { @class = ""})) { %>
        <%: Html.ValidationSummary(true) %>

        <table>
            <tr>
                <td style="text-align: right; padding-right: 5px;">
                    <%: Html.LabelFor(model => model.filial) %>
                </td>
                <td class="editor-field" style="margin-left: 5px;">
                    <%: Html.EditorFor(model => model.filial) %>
                    <%: Html.ValidationMessageFor(model => model.filial) %>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; padding-right: 5px;">
                    <%: Html.LabelFor(model => model.rede) %>
                </td>
                <td class="editor-field">
                    <%: Html.EditorFor(model => model.rede) %>
                    <%: Html.ValidationMessageFor(model => model.rede) %>
                </td>
                
            </tr>
        </table>
              <div id="botao">
            <input class="btn btn-primary" type="submit" value="Salvar" />
        </div>
        <div class="rodape">
        <span>ATENÇÃO: Estas informações só<br /> devem ser inseridas quando o acesso<br /> for feito em uma loja fisica.<br /><br />
       <a style="color:#780404" <%= Ajax.ActionLink("Restaurar Configurações Originais", "RemoverConfiguracao", "Configuracao", new AjaxOptions() { 
   
    InsertionMode = InsertionMode.Replace,
    UpdateTargetId = "retorno",
    OnSuccess="modal(2)"

}, new { @class = "" })%> 
        </span> 
            </div>

        <% } %>
    </div>
</asp:Content>
