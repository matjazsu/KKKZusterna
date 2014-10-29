using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KKK_Zusterna.Models
{
    public class TreningGrid
    {
        #region Properties

        public int ID_Trening { get; set; }
        public string Naziv { get; set; }
        public string Spremenil { get; set; }
        public DateTime SpremenilDatum { get; set; }

        #endregion

        #region Functionality

        public TreningGrid(){}

        #endregion
    }
}