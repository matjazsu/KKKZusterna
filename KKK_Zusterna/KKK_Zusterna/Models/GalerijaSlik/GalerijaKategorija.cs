using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KKK_Zusterna.Models
{
    public class GalerijaKategorija
    {
        #region Properties

        [Required]
        [Display(Name = "Kategorija:")]
        public int ID_galerijaKategorija { get; set; }

        [Required]
        [Display(Name = "Naslov:")]
        public string Naslov { get; set; }

        [Required]
        [Display(Name = "URL Slika:")]
        public string URLSlika { get; set; }

        [Display(Name = "Spremenil uporabnik:")]
        public string SpremenilUporabnik { get; set; }

        [Display(Name = "URL Slika:")]
        public DateTime SpremenilDatum { get; set; }

        #endregion

        #region LifeCycle

        public GalerijaKategorija(){}

        #endregion
    }
}