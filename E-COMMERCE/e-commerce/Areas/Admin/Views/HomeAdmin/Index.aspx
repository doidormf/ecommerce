<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/PageMasterAdmin.Master" Inherits="System.Web.Mvc.ViewPage<e_commerce.Models.cadusu>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="login"  style="margin-top:70px;">
      <br />
        <p>
            Esta é uma área restrita! se você não é Administrador, por favor feche esta página.
        </p>
        <% using (Html.BeginForm())
           { %>

        <div>
            <fieldset>
                <legend></legend>
                <%: Html.ValidationSummary(true) %>
                <table>
                    <tr>
                        <td class="editor-label">
                            <%:Html.LabelFor(m => m.loguser) %>
                        </td>
                        <td class="editor-field">
                            <%: Html.TextBoxFor(m => m.loguser) %> <strong style="color: #EE0000; font-size: 14px; font-weight: bold">*</strong>
                            <%: Html.ValidationMessageFor(m => m.loguser) %>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label">
                            <%: Html.LabelFor(m => m.senuser) %>
                        </td>
                        <td class="editor-field">
                            <%: Html.PasswordFor(m => m.senuser) %> <strong style="color: #EE0000; font-size: 14px; font-weight: bold">*</strong>
                            <%: Html.ValidationMessageFor(m => m.senuser) %>
                        </td>
                    </tr>

                    <%--    <tr>
            <td>
                <span style="float:right;  padding: 0 0 0 15px;""><%: Html.CheckBoxFor(m => m.RememberMe) %></span>
                 </td>
                <td>
                 <span style="float:left; padding: 5px 0 0 5px;"><%: Html.LabelFor(m => m.RememberMe) %></span>
            </td>
        </tr>--%>
                </table>
                <p>
                    <input type="submit" value="Entrar" />
                </p>

              <%--  <p class="editor-field">
                    <%=  Ajax.ActionLink("Recuperar Senha","RecuperarSenha", "Account",new {}, new AjaxOptions()
                {

                    UpdateTargetId = "myModal",
                    InsertionMode = InsertionMode.Replace

                }, new { @class = "", onclick="modal(1)"}) %>
                </p>--%>
            </fieldset>
        </div>
        <% } %>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Contentrodape" runat="server">
</asp:Content>
