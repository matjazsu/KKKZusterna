using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KKK_Zusterna.Models
{
    public class ONas
    {
        #region Properties

        [Required]
        [Display(Name = "ID_ONas:")]
        public int ID_ONas { get; set; }

        [AllowHtml]
        [Display(Name = "Vsebina:")]
        public string Vsebina { get; set; }

        [Required]
        [Display(Name = "Spremenil")]
        public string Spremenil { get; set; }

        [Required]
        [Display(Name = "SpremenilDatum")]
        public DateTime SpremenilDatum { get; set; }
        
        #endregion

        #region LifeCycle

        public ONas(){}

        #endregion
    }
}