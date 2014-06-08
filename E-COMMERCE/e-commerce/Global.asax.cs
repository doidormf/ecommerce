using System.Web.Mvc;
using System.Web.Routing;
using e_commerce.Models;

namespace e_commerce
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            vsasliteEntities context = new vsasliteEntities();
            AreaRegistration.RegisterAllAreas();

            //Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~");
            //ConfigurationSection configurationSection = configuration.GetSection("connectionStrings");
            //if (!configurationSection.SectionInformation.IsProtected)
            //{
            //    configurationSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
            //    //configurationSection.SectionInformation.UnprotectSection();//descriptografa string
            //    configurationSection.SectionInformation.ForceSave = true;
            //    configuration.Save(ConfigurationSaveMode.Full);
            //}
            // Use LocalDB for Entity Framework by default
            //Database.DefaultConnectionFactory = new SqlConnectionFactory(context.Database.Connection.ConnectionString);

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}