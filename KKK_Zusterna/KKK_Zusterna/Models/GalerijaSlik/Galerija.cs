using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KKK_Zusterna.Models
{
    public class Galerija
    {
        #region Properties

        [Required]
        [Display(Name = "Galerija")]
        public int ID_galerija { get; set; }

        [Required]
        [Display(Name = "Galerija kategorija:")]
        public int ID_galerijaKategorija { get; set; }

        [Required]
        [Display(Name = "URL Slika:")]
        public string URLSlika { get; set; }

        #endregion

        #region LifeCycle

        public Galerija(){}

        #endregion
    }
}