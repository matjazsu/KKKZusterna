﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KKK_Zusterna.Models
{
    public class KategorijaPovezavGrid
    {
        #region Properties

        public int ID_KategorijaPovezav { get; set; }
        public string Naslov { get; set; }
        public string Spremenil { get; set; }
        public DateTime SpremenilDatum { get; set; }

        #endregion
    }
}