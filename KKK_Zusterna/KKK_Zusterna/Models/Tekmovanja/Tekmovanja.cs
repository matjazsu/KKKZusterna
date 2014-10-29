using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KKK_Zusterna.Models
{
    public class Tekmovanja
    {
        #region Properties

        [Required]
        [Display(Name = "Id:")]
        public int ID_tekmovanja { get; set; }

        [Required]
        [Display(Name = "Naslov:")]
        public string Naslov { get; set; }

        [Required]
        [Display(Name = "Vsebina:")]
        public string Vsebina { get; set; }

        [Required]
        [Display(Name = "URL Datoteka:")]
        public string URLFile { get; set; }

        [Required]
        [Display(Name = "Datoteka:")]
        public string FileName { get; set; }

        [Required]
        [Display(Name = "Leto tekmovanja:")]
        public int ID_letoTekmovanja { get; set; }

        [Required]
        [Display(Name = "Spremenil:")]
        public string Spremenil { get; set; }

        [Required]
        [Display(Name = "SpremenilDatum:")]
        public DateTime SpremenilDatum { get; set; }

        #endregion

        #region LifeCycle

        public Tekmovanja() { }

        #endregion
    }
}