using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace e_commerce.Helpers
{
    public class CriptografarConexao
    {

        public void CripterConexao() {

            Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~");
            ConfigurationSection configurationSection = configuration.GetSection("connectionStrings");
            configurationSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
            configurationSection.SectionInformation.ForceSave = true;
            configuration.Save(ConfigurationSaveMode.Full);
        
        }

        public void DecripterConexao()
        {

            Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~");
            ConfigurationSection configurationSection = configuration.GetSection("connectionStrings");
            configurationSection.SectionInformation.UnprotectSection();
            configurationSection.SectionInformation.ForceSave = true;
            configuration.Save(ConfigurationSaveMode.Full);

        }
    }
}