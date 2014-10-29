using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.Web.Mvc;

namespace KKK_Zusterna.Models
{
    public class Novica
    {
        #region Properties

        [Required]
        [Display(Name = "ID:")]
        public int ID_novica { get; set; }

        [Required]
        [Display(Name = "Naslov:")]
        public string Naslov { get; set; }

        [Display(Name = "Povzetek:")]
        [AllowHtml]
        public string Povzetek { get; set; }

        [Display(Name = "Vsebina:")]
        [AllowHtml]
        public string Vsebina { get; set; }

        [Required]
        [Display(Name = "Spremenil:")]
        public int ID_uporabnik { get; set; }

        [Required]
        [Display(Name = "Datum zadnje spremembe:")]
        public DateTime DatumSpremenil { get; set; }

        [Display(Name = "Slika:")]
        public string URLSlika { get; set; }

        [Display(Name = "Uporabnik:")]
        public string Uporabnik { get; set; }

        #endregion

        #region LifeCycle

        public Novica(){}

        #endregion
    }
}