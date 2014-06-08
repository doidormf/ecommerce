using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace e_commerce.Models
{
    [MetadataType(typeof(ValidacaoUsu))]
    public partial class cadusu
    {
    }
    public class ValidacaoUsu
    {
        public string coduser { get; set; }

        
        public string nomuser { get; set; }
        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Usuário")]
        public string loguser { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Senha")]
        public string senuser { get; set; }

        public string codperfil { get; set; }

        public System.DateTime dtenvio { get; set; }

        public string flgallloja { get; set; }

        public string flgaltsenha { get; set; }
    }
}