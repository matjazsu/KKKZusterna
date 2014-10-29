using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KKK_Zusterna.Models
{
    public class NovicaGrid
    {
        #region Properties

        public int ID_novica { get; set; }
        public string Naslov { get; set; }
        public string Povzetek { get; set; }
        public string Vsebina { get; set; }
        public int ID_uporabnik { get; set; }
        public DateTime DatumSpremenil { get; set; }
        public string URLSlika { get; set; }
        public string Uporabnik { get; set; }

        #endregion
    }
}