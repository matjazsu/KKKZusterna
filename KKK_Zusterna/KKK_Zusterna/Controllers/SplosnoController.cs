using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using KKK_Zusterna.Models;
using KKK_Zusterna.Helper;
using PagedList;
using log4net;

namespace KKK_Zusterna.Controllers
{
    public class SplosnoController : Controller
    {
        #region Properties

        ILog logger = LogManager.GetLogger(typeof(SplosnoController));

        private GalerijaKategorijaPPP UpraviteljGalerijaKategorija = new GalerijaKategorijaPPP();

        private GalerijaPPP UpraviteljGalerija = new GalerijaPPP();

        #endregion

        #region Splosno

        #region Galerija slik

        #region Galerija slik

        public ActionResult GalerijaSlik(int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            List<GalerijaKategorijaGrid> galerije = null;

            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                galerije = UpraviteljGalerijaKategorija.VrniKategorijeSlik();

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
                MailHelper.SendMailForErrors("GalerijaSlik", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View(galerije.ToPagedList(pageNumber, pageSize));
        }

        #endregion

        #region PrikaziGalerijo

        public ActionResult PrikaziGalerijo(int ID_galerijaKategorija)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                //Get galerija
                GalerijaKategorija galerijaKategorija = UpraviteljGalerijaKategorija.VrniKategorijoSlik(ID_galerijaKategorija);
                List<Galerija> galerija = UpraviteljGalerija.VrniGalerijoZaGalerijaKategorija(ID_galerijaKategorija);

                ViewBag.Kategorija = galerijaKategorija;
                ViewBag.Data = galerija;

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
                MailHelper.SendMailForErrors("PrikaziGalerijo", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("Galerija");
        }

        #endregion

        #endregion

        #endregion
    }
}
