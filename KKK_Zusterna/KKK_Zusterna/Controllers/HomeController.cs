using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using KKK_Zusterna.Models;
using KKK_Zusterna.Helper;
using PagedList;
using log4net;

namespace KKK_Zusterna.Controllers
{
    public class HomeController : Controller
    {
        #region Properties

        ILog logger = LogManager.GetLogger(typeof(HomeController));

        public NovicaPPP UpraviteljNovica = new NovicaPPP();

        public NovicaPrilogaPPP UpraviteljNovicaPriloga = new NovicaPrilogaPPP();

        #endregion

        #region Index

        //Render Novice/Index/Domov page
        public ActionResult Index(int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            List<NovicaGrid> novice = null;

            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                //Get novice
                novice = UpraviteljNovica.VrniNovice();

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
                MailHelper.SendMailForErrors("Index", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View(novice.ToPagedList(pageNumber, pageSize));
        }

        #endregion

        #region Prikazi Novico

        //Render Novica page
        public ActionResult PrikaziNovico(int ID_novica)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                //Get Novica
                Novica novica = UpraviteljNovica.VrniNovico(ID_novica);
                ViewBag.Data = novica;

                List<NovicaPriloga> priloge = UpraviteljNovicaPriloga.VrniPrilogeZaNovico(ID_novica);
                ViewBag.Priloge = priloge;

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
                MailHelper.SendMailForErrors("PrikaziNovico", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region NapakaAvtorizacija

        #region VrniNapakaAvtorizacija

        public ActionResult VrniNapakaAvtorizacija()
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                GlobalErrors.DodajNapako("Za izbrano akcijo nimate ustreznih pravic.");
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("VrniNapakaAvtorizacija", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();   
        }

        #endregion

        #region Send mail to admin

        [HttpPost, ValidateInput(false)]
        public ActionResult PosljiMailAdmin(string Ime, string Priimek, string Email, string Vsebina)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                //Send email
                MailHelper.SendMailForInvalidAuthentication(Ime, Priimek, Email, Vsebina);

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("PosljiMailAdmin", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("VrniNapakaAvtorizacija");
        }

        #endregion

        #endregion
    }
}
