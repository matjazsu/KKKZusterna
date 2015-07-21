using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using KKK_Zusterna.Helper;
using KKK_Zusterna.Models;
using PagedList;
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
            List<Trening> seznamTreningov = null;

            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                seznamTreningov = UpraviteljTrening.VrniTreningeAll();

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

            return View(seznamTreningov);
        }

        #endregion

        #region Tekmovanja

        #region Tekmovanja

        //Render Tekmovanja page --> just HTML (for now)
        public ActionResult Tekmovanja()
        {
            List<LetoTekmovanja> letoTekmovanja = null;

            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                letoTekmovanja = UpraviteljLetoTekmovanja.VrniLetoTekmovanja().OrderByDescending(t => t.ID_letoTekmovanja).ToList();

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

            return View(letoTekmovanja);
        }

        #endregion

        #region PrikaziTekmovanja

        public ActionResult PrikaziTekmovanja(int ID_letoTekmovanja, int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            LetoTekmovanja letoTekmovanja = null;
            List<Tekmovanja> tekmovanja = null;

            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                letoTekmovanja = UpraviteljLetoTekmovanja.VrniLetoTekmovanja(ID_letoTekmovanja);
                tekmovanja = UpraviteljTekmovanja.VrniTekmovanjaZaLeto(ID_letoTekmovanja).OrderByDescending(t => t.ID_tekmovanja).ToList();

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

            return View(Tuple.Create(letoTekmovanja, tekmovanja.ToPagedList(pageNumber, pageSize)));
        }

        #endregion

        #endregion

        #region Rezultati

        #region Rezultati

        //Render Rezultati page --> just HTML (for now)
        public ActionResult Rezultati()
        {
            List<LetoRezultati> letoRezultati = null;

            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                letoRezultati = UpraviteljLetoRezultati.VrniLetoRezultat().OrderByDescending(r => r.ID_letoRezultati).ToList();

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

            return View(letoRezultati);
        }

        #endregion

        #region PrikaziRezultate

        public ActionResult PrikaziRezultate(int ID_letoRezultati, int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            LetoRezultati letoRezultati = null;
            List<Rezultati> rezultati = null;

            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                letoRezultati = UpraviteljLetoRezultati.VrniLetoRezultat(ID_letoRezultati);
                rezultati = UpraviteljRezultati.VrniRezultateZaLeto(ID_letoRezultati).OrderByDescending(r => r.ID_rezultati).ToList();

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

            return View(Tuple.Create(letoRezultati, rezultati.ToPagedList(pageNumber, pageSize)));
        }

        #endregion

        #endregion

        #region VrniTopTekmovanja

        [ChildActionOnly]
        public ActionResult VrniTopTekmovanja()
        {
            List<Tekmovanja> tekmovanja = null;

            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                tekmovanja = UpraviteljTekmovanja.VrniTopTekmovanja();
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("Index", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return PartialView(tekmovanja);
        }

        #endregion

        #region VrniTopRezultate

        [ChildActionOnly]
        public ActionResult VrniTopRezultate()
        {
            List<Rezultati> rezultati = null;

            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                rezultati = UpraviteljRezultati.VrniTopRezultate();
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("Index", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return PartialView(rezultati);
        }

        #endregion

        #endregion
    }
}
