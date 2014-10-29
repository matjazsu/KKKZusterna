using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using KKK_Zusterna.Helper;
using KKK_Zusterna.Models;
using log4net;

namespace KKK_Zusterna.Controllers
{
    public class TekmovanjaController : Controller
    {
        #region Properties

        ILog logger = LogManager.GetLogger(typeof(TekmovanjaController));

        private TreningPPP UpraviteljTrening = new TreningPPP();

        private LetoRezultatiPPP UpraviteljLetoRezultati = new LetoRezultatiPPP();
        private RezultatiPPP UpraviteljRezultati = new RezultatiPPP();

        private LetoTekmovanjaPPP UpraviteljLetoTekmovanja = new LetoTekmovanjaPPP();
        private TekmovanjaPPP UpraviteljTekmovanja = new TekmovanjaPPP();

        #endregion

        #region Tekmovanja

        #region Treningi

        public ActionResult Treningi()
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                List<Trening> seznamTreningov = UpraviteljTrening.VrniTreningeAll();
                if (seznamTreningov.Count > 0)
                {
                    ViewBag.Data = seznamTreningov;   
                }
                else
                {
                    ViewBag.Data = null;
                }

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
                MailHelper.SendMailForErrors("Treningi", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region Tekmovanja

        #region Tekmovanja

        //Render Tekmovanja page --> just HTML (for now)
        public ActionResult Tekmovanja()
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                List<LetoTekmovanja> letoTekmovanja = UpraviteljLetoTekmovanja.VrniLetoTekmovanja();
                ViewBag.Data = letoTekmovanja;

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
                MailHelper.SendMailForErrors("Tekmovanja", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region PrikaziTekmovanja

        public ActionResult PrikaziTekmovanja(int ID_letoTekmovanja)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                LetoTekmovanja letoTekmovanja = UpraviteljLetoTekmovanja.VrniLetoTekmovanja(ID_letoTekmovanja);
                ViewBag.LetoTekmovanja = letoTekmovanja;

                List<Tekmovanja> tekmovanja = UpraviteljTekmovanja.VrniTekmovanjaZaLeto(ID_letoTekmovanja);
                ViewBag.Data = tekmovanja;

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
                MailHelper.SendMailForErrors("PrikaziTekmovanja", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #endregion

        #region Rezultati

        #region Rezultati

        //Render Rezultati page --> just HTML (for now)
        public ActionResult Rezultati()
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                List<LetoRezultati> letoRezultati = UpraviteljLetoRezultati.VrniLetoRezultat();
                ViewBag.Data = letoRezultati;

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
                MailHelper.SendMailForErrors("Rezultati", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region PrikaziRezultate

        public ActionResult PrikaziRezultate(int ID_letoRezultati)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                LetoRezultati letoRezultati = UpraviteljLetoRezultati.VrniLetoRezultat(ID_letoRezultati);
                ViewBag.LetoRezultati = letoRezultati;

                List<Rezultati> rezultati = UpraviteljRezultati.VrniRezultateZaLeto(ID_letoRezultati);
                ViewBag.Data = rezultati;

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
                MailHelper.SendMailForErrors("PrikaziRezultate", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #endregion

        #region VrniTopTekmovanja

        [ChildActionOnly]
        public ActionResult VrniTopTekmovanja()
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                List<Tekmovanja> tekmovanja = UpraviteljTekmovanja.VrniTopTekmovanja();
                ViewBag.Data = tekmovanja;
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("Index", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return PartialView();
        }

        #endregion

        #region VrniTopRezultate

        [ChildActionOnly]
        public ActionResult VrniTopRezultate()
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                List<Rezultati> tekmovanja = UpraviteljRezultati.VrniTopRezultate();
                ViewBag.Data = tekmovanja;
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("Index", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return PartialView();
        }

        #endregion

        #endregion
    }
}
