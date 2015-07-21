using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using KKK_Zusterna.Helper;
using KKK_Zusterna.Models;
using log4net;

namespace KKK_Zusterna.Controllers
{
    public class KlubskiKoticekController : Controller
    {
        #region Properties

        ILog logger = LogManager.GetLogger(typeof(KlubskiKoticekController));

        ONasPPP UpraviteljONas = new ONasPPP();

        KontaktPPP UpraviteljKontakt = new KontaktPPP();

        KategorijaPovezavPPP UpraviteljKategorijPovezav = new KategorijaPovezavPPP();
        PovezavePPP UpraviteljPovezav = new PovezavePPP();

        #endregion

        #region ONas

        public ActionResult ONas()
        {
            ONas oNas = null;

            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                oNas = UpraviteljONas.VrniONas();

                //Obvestilo o uspehu akcije if TrenutniUporabnik != null
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                }
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("ONas", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View(oNas);
        }

        #endregion

        #region Kontakt

        #region Kontakt

        public ActionResult Kontakt()
        {
            Kontakt kontakt = null;

            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                kontakt = UpraviteljKontakt.VrniKontakt();

                //Obvestilo o uspehu akcije if TrenutniUporabnik != null
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                }
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("Kontakt", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View(kontakt);
        }

        #endregion

        #region PosljiMail

        [HttpPost, ValidateInput(false)]
        public ActionResult PosljiMail(string Ime, string Priimek, string Email, string Vsebina)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                //Send email
                MailHelper.SendMailFromKontaktForm(Ime, Priimek, Email, Vsebina);

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("PosljiMail", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("Kontakt");
        }

        #endregion

        #endregion

        #region Povezave

        public ActionResult Povezave()
        {
            List<PovezavePlus> seznamForView = null;

            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                #region Get data

                seznamForView = new List<PovezavePlus>();

                List<KategorijaPovezavGrid> seznamKategorijPovezav = UpraviteljKategorijPovezav.VrniKategorijePovezav();
                foreach (KategorijaPovezavGrid kategorijaPovezav in seznamKategorijPovezav)
                {
                    PovezavePlus povezavePlus = new PovezavePlus();
                    povezavePlus.ID_KategorijaPovezav = kategorijaPovezav.ID_KategorijaPovezav;
                    povezavePlus.Naslov = kategorijaPovezav.Naslov;
                    povezavePlus.Spremenil = kategorijaPovezav.Spremenil;
                    povezavePlus.SpremenilDatum = kategorijaPovezav.SpremenilDatum;

                    List<PovezaveGrid> seznamPovezav = UpraviteljPovezav.VrniPovezaveZaKategorijo(kategorijaPovezav.ID_KategorijaPovezav);
                    povezavePlus.SeznamPovezav = seznamPovezav;

                    seznamForView.Add(povezavePlus);
                }

                if (seznamForView.Count < 1)
                {
                    seznamForView = null;
                }

                #endregion

                //Obvestilo o uspehu akcije if TrenutniUporabnik != null
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                }

            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("Povezave", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View(seznamForView);
        }

        #endregion
    }
}
