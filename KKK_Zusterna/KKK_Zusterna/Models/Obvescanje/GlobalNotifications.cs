using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KKK_Zusterna.Models
{
    public static class GlobalNotifications
    {
        #region Properties

        public static List<string> seznamObvestil = new List<string>();

        #endregion

        #region Static Notifications

        public static string UspesnoPrijavilUporabnik = "Uspešno ste se prijavilo kot ";

        public static string UspehOperacije = "Akcija uspešno izvedena.";

        public static string DodajanjeNovice = "Proces priprave akcije za dodajanje novice uspešno izveden.";

        public static string DodajanjeUporabnika = "Proces priprave akcije za dodajanje uporabnika uspešno izveden.";

        public static string DodajanjeKontakta = "Proces priprave akcije za dodajanje kontaktnih informacij uspešno izveden.";

        public static string DodajanjeONas = "Proces priprave akcije za dodajanje vsebine 'O nas' uspešno izveden.";

        public static string DodajanjeTreninga = "Proces priprave akcije za dodajanje treninga uspešno izveden.";

        public static string DodajanjeKategorijePovezav = "Proces priprave akcije za dodajanje kategorije povezav uspešno izveden.";
        public static string DodajanjePovezave = "Proces priprave akcije za dodajanje povezave uspešno izveden.";

        public static string DodajanjeLetaRezultatov = "Proces priprave akcije za dodajanje leta rezultatov uspešno izveden.";
        public static string DodajanjeRezultatov = "Proces priprave akcije za dodajanje rezultatov uspešno izveden.";

        public static string DodajanjeLetaTekmovanj = "Proces priprave akcije za dodajanje leta tekmovanj uspešno izveden.";
        public static string DodajanjeTekmovanj = "Proces priprave akcije za dodajanje tekmovanj uspešno izveden.";

        public static string DodajanjeGalerijeSlik = "Proces priprave akcije za dodajanje galerije slik uspešno izveden.";

        #endregion

        #region Functionality

        public static void DodajObvestilo(string napaka)
        {
            if (!seznamObvestil.Contains(napaka))
            {
                seznamObvestil.Add(napaka);
            }
        }

        public static void OdstraniObvestilo(string napaka)
        {
            if (seznamObvestil.Contains(napaka))
            {
                seznamObvestil.Remove(napaka);
            }
        }

        public static void ZbrisiObvestilo()
        {
            seznamObvestil.Clear();
        }

        public static string SetObvetiloUspehUporabnik(string ime, string priimek)
        {
            return UspesnoPrijavilUporabnik + ime + " " + priimek + "."; 
        }

        #endregion
    }
}