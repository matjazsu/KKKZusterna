using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KKK_Zusterna.Models
{
    public class Kontakt
    {
        #region Properties

        public int ID_Kontakt { get; set; }
        public string Spremenil { get; set; }
        public DateTime SpremenilDatum { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Fax { get; set; }
        public string GSM { get; set; }
        public string Naslov { get; set; }

        #endregion

        #region Constructor

        public Kontakt(){}

        #endregion
    }
}