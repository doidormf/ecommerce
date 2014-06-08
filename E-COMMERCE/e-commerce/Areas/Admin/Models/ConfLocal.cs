using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace e_commerce.Areas.Admin.Models
{
    public class ConfLocal
    {
        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Filial")]
        public string filial { get; set; }

        [Required(ErrorMessage = "Preenchimento Obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Rede")]
        public string rede { get; set; }
    }
}