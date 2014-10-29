using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KKK_Zusterna.Models
{
    public class Rezultati
    {
        #region Properties

        [Required]
        [Display(Name = "Id:")]
        public int ID_rezultati { get; set; }

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
        [Display(Name = "Leto rezultati:")]
        public int ID_letoRezultati { get; set; }

        [Required]
        [Display(Name = "Spremenil:")]
        public string Spremenil { get; set; }

        [Required]
        [Display(Name = "SpremenilDatum:")]
        public DateTime SpremenilDatum { get; set; }

        #endregion

        #region LifeCycle

        public Rezultati(){}

        #endregion
    }
}