<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<%
    if (Request.IsAuthenticated)
    {
%>
        Bem Vindo   <strong><%: Page.User.Identity.Name %></strong>!
        <%: Html.ActionLink(" Sair ", "LogOff", "Configuracao") %>
<%
    }
    else
    {
%>
<%: Html.ActionLink("Faça seu login ou cadastre-se ", "LogOn", "Account") %>
<%
    }
%>