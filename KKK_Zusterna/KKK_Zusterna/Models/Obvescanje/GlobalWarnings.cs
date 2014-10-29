using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KKK_Zusterna.Models
{
    public static class GlobalWarnings
    {
        #region Properties

        public static List<string> seznamOpozoril = new List<string>();

        #endregion

        #region Static Notifications

        public static string EMailUporabnikaSpremenjen = "Zaradi spremembe e-mail naslova, vam svetujemo da se ponovno prijavite.";

        public static string TrenutnoGesloPrazno = "Validacija ni uspela: trenutno geslo.";
        public static string NovoGesloPrazno = "Validacija ni uspela: novo geslo.";
        public static string GesliSeNeUjemata = "Validacija ni uspela: gesli se ne ujemata.";
        public static string TrenutnoGesloNiPravo = "Vpisano geslo ni pravilno. Poskusite znova.";

        #endregion

        #region Functionality

        public static void DodajOpozorilo(string napaka)
        {
            if (!seznamOpozoril.Contains(napaka))
            {
                seznamOpozoril.Add(napaka);
            }
        }

        public static void OdstraniOpozorilo(string napaka)
        {
            if (seznamOpozoril.Contains(napaka))
            {
                seznamOpozoril.Remove(napaka);
            }
        }

        public static void ZbrisiOpozorilo()
        {
            seznamOpozoril.Clear();
        }

        #endregion
    }
}