using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SQLite;

namespace KKK_Zusterna.Models
{
    public class Uporabnik
    {
        #region Properties

        [Required]
        [Display(Name = "ID:")]
        public int ID_uporabnik { get; set; }

        [Display(Name = "Ime:")]
        public string Ime { get; set; }

        [Display(Name = "Priimek:")]
        public string Priimek { get; set; }

        [Required]
        [Display(Name = "Email:")]
        public string Email { get; set; }

        [Display(Name = "Spremenil")]
        public string Spremenil { get; set; }

        [Required]
        [Display(Name = "SpremenilDatum")]
        public DateTime SpremenilDatum { get; set; }

        [Required]
        [Display(Name = "Geslo:")]
        public string Geslo { get; set; }

        [Display(Name = "Administrator")]
        public int Administrator { get; set; }

        [Display(Name = "FlagPrijava")]
        public int FlagPrijava { get; set; }

        [Display(Name = "FlagNapaka")]
        public int FlagNapaka { get; set; }

        [Display(Name = "FlagKontakt")]
        public int FlagKontakt { get; set; }

        [Display(Name = "FlagRegistracija")]
        public int FlagRegistracija { get; set; }

        [Display(Name = "FlagNapakaAvtorizacija")]
        public int FlagNapakaAvtorizacija { get; set; }

        [Display(Name = "FlagONas")]
        public int FlagONas { get; set; }

        [Display(Name = "FlagPovezave")]
        public int FlagPovezave { get; set; }

        [Display(Name = "FlagTreningi")]
        public int FlagTreningi { get; set; }

        [Display(Name = "FlagTekmovanja")]
        public int FlagTekmovanja { get; set; }

        [Display(Name = "FlagRezultati")]
        public int FlagRezultati { get; set; }

        [Display(Name = "FlagGalerijaSlik")]
        public int FlagGalerijaSlik { get; set; }

        #endregion

        #region LifeCycle

        public Uporabnik(){}

        #endregion
    }
}