using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KKK_Zusterna.Models
{
    public class GalerijaKategorijaGrid
    {
        #region Properties

        public int ID_galerijaKategorija { get; set; }
        public string Naslov { get; set; }
        public string URLSlika { get; set; }
        public string SpremenilUporabnik { get; set; }
        public DateTime SpremenilDatum { get; set; }

        #endregion
    }
}