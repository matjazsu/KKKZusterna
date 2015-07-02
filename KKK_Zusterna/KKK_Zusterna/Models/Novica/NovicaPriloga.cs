using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KKK_Zusterna.Models
{
    public class NovicaPriloga
    {
        #region Properties

        [Required]
        [Display(Name = "ID:")]
        public int ID_novicaPriloga { get; set; }

        [Required]
        [Display(Name = "ID novica:")]
        public int ID_novica { get; set; }

        [Display(Name = "Naslov")]
        public string Naslov { get; set; }

        [Display(Name = "Datoteka:")]
        public string URLFile { get; set; }

        #endregion
    }
}