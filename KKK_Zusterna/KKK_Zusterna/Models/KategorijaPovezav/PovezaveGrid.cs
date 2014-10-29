using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KKK_Zusterna.Models
{
    public class PovezaveGrid
    {
        #region Properties

        public int ID_Povezava { get; set; }
        public int ID_KategorijaPovezav { get; set; }
        public string Naziv { get; set; }
        public string URL { get; set; }
        public string Spremenil { get; set; }
        public DateTime SpremenilDatum { get; set; }

        #endregion
    }
}