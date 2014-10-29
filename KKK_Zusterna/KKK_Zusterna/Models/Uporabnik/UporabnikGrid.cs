using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KKK_Zusterna.Models
{
    public class UporabnikGrid
    {
        #region Properties

        public int ID_uporabnik { get; set; }
        public string Ime { get; set; }
        public string Priimek { get; set; }
        public string Email { get; set; }
        public string Spremenil { get; set; }
        public DateTime SpremenilDatum { get; set; }
        public string Geslo { get; set; }
        public System.Int64 Administrator { get; set; }
        public System.Int64 FlagPrijava { get; set; }
        public System.Int64 FlagNapaka { get; set; }
        public System.Int64 FlagKontakt { get; set; }
        public System.Int64 FlagRegistracija { get; set; }
        public System.Int64 FlagNapakaAvtorizacija { get; set; }
        public System.Int64 FlagONas { get; set; }
        public System.Int64 FlagPovezave { get; set; }
        public System.Int64 FlagTreningi { get; set; }
        public System.Int64 FlagTekmovanja { get; set; }
        public System.Int64 FlagRezultati { get; set; }
        public System.Int64 FlagGalerijaSlik { get; set; }

        #endregion
    }
}