using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KKK_Zusterna.Models
{
    public static class GlobalErrors
    {
        #region Properties

        public static List<string> seznamNapak = new List<string>();

        public static bool Prijavil = false;

        #endregion

        #region Static Errors

        public static string napakaPrijava = "Uporabnik z danim e-naslovom in geslom ne obstaja!";

        #endregion

        #region Functionality

        public static void DodajNapako(string napaka)
        {
            if (!seznamNapak.Contains(napaka))
            {
                seznamNapak.Add(napaka);
            }
        }

        public static void OdstraniNapako(string napaka)
        {
            if (seznamNapak.Contains(napaka))
            {
                seznamNapak.Remove(napaka);
            }
        }

        public static void ZbrisiNapake()
        {
            seznamNapak.Clear();
        }

        #endregion
    }
}