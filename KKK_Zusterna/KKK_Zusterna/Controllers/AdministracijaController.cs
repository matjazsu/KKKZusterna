using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using KKK_Zusterna.Helper;
using KKK_Zusterna.Models;
using System.Drawing;
using KKK_Zusterna.Enums;
using log4net;
using System.Reflection;

namespace KKK_Zusterna.Controllers
{
    public class AdministracijaController : Controller
    {
        #region Properties

        ILog logger = LogManager.GetLogger(typeof(AdministracijaController));

        public UporabnikPPP UpraviteljUporabnik = new UporabnikPPP();

        public NovicaPPP UpraviteljNovica = new NovicaPPP();

        public ONasPPP UpraviteljONas = new ONasPPP();

        public KontaktPPP UpraviteljKontakt = new KontaktPPP();

        public KategorijaPovezavPPP UpraviteljKategorijePovezav = new KategorijaPovezavPPP();
        public PovezavePPP UpraviteljPovezav = new PovezavePPP();

        public TreningPPP UpraviteljTrening = new TreningPPP();

        public LetoTekmovanjaPPP UpraviteljLetoTekmovanja = new LetoTekmovanjaPPP();
        public TekmovanjaPPP UpraviteljTekmovanja = new TekmovanjaPPP();

        public LetoRezultatiPPP UpraviteljLetoRezultati = new LetoRezultatiPPP();
        public RezultatiPPP UpraviteljRezultati = new RezultatiPPP();

        public GalerijaKategorijaPPP UpraviteljGalerijaKategorija = new GalerijaKategorijaPPP();
        public GalerijaPPP UpraviteljGalerija = new GalerijaPPP();

        #endregion

        #region Uporabniki

        #region Prijava

        [HttpPost]
        public ActionResult Prijava(string email, string geslo)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                //Get novice
                List<NovicaGrid> tmp = UpraviteljNovica.VrniNovice();

                //Return List<Novice> to View
                ViewBag.Data = tmp;

                //Get uporabnik
                Uporabnik m_trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(email, geslo);

                if (m_trenutniUporabnik != null)
                {
                    FormsAuthentication.SetAuthCookie(m_trenutniUporabnik.Email, true);
                    
                    //Move to new thread for better performance
                    logger.Info("User " + m_trenutniUporabnik.Email + " has logged in.");
                    Thread thread = new Thread(() => MailHelper.SendMailForUserLogin(m_trenutniUporabnik.Email));
                    thread.Start();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    GlobalErrors.DodajNapako(GlobalErrors.napakaPrijava);
                    logger.Info("User " + email + " is not a valid user!");

                    return View("../Home/Index");
                }
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("Prijava", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);

                return View("../Home/Index");
            }
        }

        #endregion

        #region VrniRegistracijaForma

        public ActionResult VrniRegistracijaForma()
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();
                
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
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region RegistrirajUporabnika

        [HttpPost, ValidateInput(false)]
        public ActionResult RegistrirajUporabnika(string Email, string Ime, string Priimek, string Geslo, string PonoviGeslo)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                if (Geslo.Equals(PonoviGeslo))
                {
                    #region Mapping

                    Uporabnik novi = new Uporabnik();

                    novi.Email = Email;
                    novi.Ime = Ime;
                    novi.Priimek = Priimek;
                    novi.Geslo = Geslo;

                    #endregion

                    MailHelper.SendMailForUserRegistration(novi);

                    //Obvestilo o uspehu akcije
                    GlobalNotifications.DodajObvestilo("Podatki so bili uspešno poslani administratorju strani. V kolikor bo zahtevek odobren boste na vaš email prejeli uporabniško ime in geslo."); 
                }
                else
                {
                    GlobalErrors.DodajNapako("Gesli se ne ujemata.");
                }
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("VrniRegistracijaForma");
        }

        #endregion

        #region VrniUrediProfilForma

        [Authorize]
        public ActionResult VrniUrediProfilForma(string Email)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                Uporabnik tmp = UpraviteljUporabnik.VrniUporabnika(Email);

                ViewBag.Data = tmp;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("VrniUrediProfilForma", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region VrniSpremeniGesloForma

        [Authorize]
        public ActionResult VrniSpremeniGesloForma(string Email)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                Uporabnik tmpUporabnik = UpraviteljUporabnik.VrniUporabnika(Email);
                ViewBag.Data = tmpUporabnik;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("VrniSpremeniGesloForma", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region UpdateGesloUporabnik

        [HttpPost]
        [Authorize]
        public ActionResult UpdateGesloUporabnik(int ID_uporabnik, string TrenutnoGeslo, string NovoGeslo, string PonoviNovoGeslo)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                bool uspesnaValidacija = true;

                #region Validacija

                if (String.IsNullOrEmpty(TrenutnoGeslo))
                {
                    GlobalWarnings.DodajOpozorilo(GlobalWarnings.TrenutnoGesloPrazno);
                    uspesnaValidacija = false;
                }

                if (string.IsNullOrEmpty(NovoGeslo) || string.IsNullOrEmpty(PonoviNovoGeslo))
                {
                    GlobalWarnings.DodajOpozorilo(GlobalWarnings.NovoGesloPrazno);
                    uspesnaValidacija = false;
                }

                #endregion

                if (uspesnaValidacija)
                {
                    if (NovoGeslo.Equals(PonoviNovoGeslo))
                    {
                        Uporabnik tmpUporabnik = UpraviteljUporabnik.VrniUporabnika(ID_uporabnik);

                        #region MD5 hash

                        MD5 md5 = new MD5CryptoServiceProvider();
                        md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(TrenutnoGeslo));
                        byte[] enterPasswordBytes = md5.Hash;
                        string enterPassword = Convert.ToBase64String(enterPasswordBytes);

                        #endregion

                        if (enterPassword.Equals(tmpUporabnik.Geslo))
                        {
                            #region MD5 Get new hash for NovoGeslo
                            
                            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(NovoGeslo));
                            byte[] newPasswordBytes = md5.Hash;
                            string newPassword = Convert.ToBase64String(newPasswordBytes);

                            #endregion

                            #region Mapping

                            Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                            tmpUporabnik.Spremenil = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                            tmpUporabnik.Geslo = newPassword;

                            #endregion

                            #region Save Changes

                            UpraviteljUporabnik.PosodobiUporabnikGeslo(tmpUporabnik);
                            logger.Info("Password change successfully for user " + tmpUporabnik.Email);

                            #endregion

                            //Obvestilo o uspehu akcije
                            GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                        }
                        else
                        {
                            GlobalWarnings.DodajOpozorilo(GlobalWarnings.TrenutnoGesloNiPravo);
                        }
                    }
                    else
                    {
                        GlobalWarnings.DodajOpozorilo(GlobalWarnings.GesliSeNeUjemata);
                    }
                }

                Uporabnik updUporabnik = UpraviteljUporabnik.VrniUporabnika(ID_uporabnik);
                ViewBag.Data = updUporabnik;
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("UpdateGesloUporabnik", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("VrniSpremeniGesloForma");
        }

        #endregion

        #region UpdateProfilUporabnik

        [HttpPost]
        [Authorize]
        public ActionResult UpdateProfilUporabnik(int ID_uporabnik, 
                                                  string Email, 
                                                  string Ime, 
                                                  string Priimek, 
                                                  bool FlagPrijava = false,
                                                  bool FlagNapaka = false,
                                                  bool FlagKontakt = false,
                                                  bool FlagRegistracija = false,
                                                  bool FlagNapakaAvtorizacija = false,
                                                  bool FlagONas = false,
                                                  bool FlagPovezave = false,
                                                  bool FlagTreningi = false,
                                                  bool FlagTekmovanja = false,
                                                  bool FlagRezultati = false,
                                                  bool FlagGalerijaSlik = false)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                bool status = false;

                //Get current Uporabnik where ID_uporabnik = ID_uporabnik
                Uporabnik uporabnik = UpraviteljUporabnik.VrniUporabnika(ID_uporabnik);

                #region Mapping new values

                uporabnik.Ime = Ime;
                uporabnik.Priimek = Priimek;

                if (!uporabnik.Email.Equals(Email))
                {
                    uporabnik.Email = Email;
                    status = true;
                }

                Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                uporabnik.Spremenil = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                uporabnik.FlagPrijava = FlagPrijava ? 1 : 0;
                uporabnik.FlagNapaka = FlagNapaka ? 1 : 0;
                uporabnik.FlagKontakt = FlagKontakt ? 1 : 0;
                uporabnik.FlagRegistracija = FlagRegistracija ? 1 : 0;
                uporabnik.FlagNapakaAvtorizacija = FlagNapakaAvtorizacija ? 1 : 0;
                uporabnik.FlagONas = FlagONas ? 1 : 0;
                uporabnik.FlagPovezave = FlagPovezave ? 1 : 0;
                uporabnik.FlagTreningi = FlagTreningi ? 1 : 0;
                uporabnik.FlagTekmovanja = FlagTekmovanja ? 1 : 0;
                uporabnik.FlagRezultati = FlagRezultati ? 1 : 0;
                uporabnik.FlagGalerijaSlik = FlagGalerijaSlik ? 1 : 0;

                #endregion

                #region Update uporabnik

                UpraviteljUporabnik.PosodobiUporabnika(uporabnik);
                logger.Info("User " + uporabnik.Email + " updated successfully.");

                #endregion

                Uporabnik tmp = UpraviteljUporabnik.VrniUporabnika(uporabnik.ID_uporabnik);
                ViewBag.Data = tmp;

                //Obvestilo o uspehu akcije
                if (status)
                {
                    GlobalWarnings.DodajOpozorilo(GlobalWarnings.EMailUporabnikaSpremenjen);
                }
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);

            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("UpdateProfilUporabnik", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("VrniUrediProfilForma");
        }

        #endregion

        #region Uporabniki

        [Authorize(Roles = "Admin")]
        public ActionResult Uporabniki()
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                List<UporabnikGrid> seznamUporabnikov = UpraviteljUporabnik.VrniUporabnike();

                ViewBag.Data = seznamUporabnikov;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("Uporabniki", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region FindUporabnik

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult FindUporabnik(string ImeFilter, string PriimekFilter, string EmailFilter)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                List<UporabnikGrid> seznamUporabnikov = UpraviteljUporabnik.VrniFilteredUporabnike("", ImeFilter, PriimekFilter, EmailFilter, "", "");

                ViewBag.Data = seznamUporabnikov;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("FindUporabnik", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("Uporabniki");
        }

        #endregion

        #region UporabnikPodrobnosti

        [Authorize(Roles = "Admin")]
        public ActionResult UporabnikPodrobnosti(int ID_uporabnik)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                if (ID_uporabnik > 0)
                {
                    Uporabnik upr = UpraviteljUporabnik.VrniUporabnika(ID_uporabnik);
                    ViewBag.Data = upr;

                    //Obvestilo o uspehu akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);   
                }
                else
                {
                    ViewBag.Data = null;

                    //Obvestilo o dodajanju uporabnika
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.DodajanjeUporabnika);
                }
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("UporabnikPodrobnosti", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region UpdateUporabnik

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult UpdateUporabnik(int ID_uporabnik, string Email, string Ime, string Priimek, bool Administrator)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                Uporabnik tmpUporabnik = UpraviteljUporabnik.VrniUporabnika(ID_uporabnik);

                if (tmpUporabnik != null)
                {
                    #region Mapping

                    tmpUporabnik.Ime = Ime;
                    tmpUporabnik.Priimek = Priimek;
                    tmpUporabnik.Email = Email;

                    Uporabnik m_trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                    tmpUporabnik.Spremenil = m_trenutniUporabnik.Ime + " " + m_trenutniUporabnik.Priimek;

                    if (Administrator)
                    {
                        tmpUporabnik.Administrator = 1;
                    }
                    else
                    {
                        tmpUporabnik.Administrator = 0;
                    }

                    #endregion

                    UpraviteljUporabnik.PosodobiUporabnika(tmpUporabnik);
                    logger.Info("User " + tmpUporabnik.Email + " updated successfully.");

                    Uporabnik upr = UpraviteljUporabnik.VrniUporabnika(ID_uporabnik);
                    ViewBag.Data = upr;

                    #region SetRoles

                    if (upr.Administrator == 1)
                    {
                        if (!Roles.IsUserInRole(upr.Email, "Admin"))
                        {
                            Roles.AddUserToRole(upr.Email, "Admin");
                        }
                    }
                    else
                    {
                        if (Roles.IsUserInRole(upr.Email, "Admin"))
                        {
                            Roles.RemoveUserFromRole(upr.Email, "Admin");
                        }
                    }

                    logger.Info("Role for user " + tmpUporabnik.Email + " updated successfully.");

                    #endregion

                    //Obvestilo o uspehu akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                }
                else
                {
                    Uporabnik upr = UpraviteljUporabnik.VrniUporabnika(ID_uporabnik);

                    ViewBag.Data = upr;

                    GlobalErrors.DodajNapako("Uporabnik ne obstaja.");
                }
            }
            catch (Exception ex)
            {
                Uporabnik upr = UpraviteljUporabnik.VrniUporabnika(ID_uporabnik);

                ViewBag.Data = upr;

                if (upr.Administrator == 1)
                {
                    ViewBag.Administrator = true;
                }
                else
                {
                    ViewBag.Administrator = false;
                }

                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("UpdateUporabnik", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("UporabnikPodrobnosti");
        }

        #endregion

        #region DeleteUporabnik

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteUporabnik(int ID_uporabnik)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                UpraviteljUporabnik.DeleteUporabnik(ID_uporabnik);
                logger.Info("User " + ID_uporabnik + " deleted successfully.");

                List<UporabnikGrid> seznamUporabnikov = UpraviteljUporabnik.VrniUporabnike();

                ViewBag.Data = seznamUporabnikov;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);    
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("DeleteUporabnik", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("Uporabniki");
        }

        #endregion

        #region InsertUporabnik

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult InsertUporabnik(string Email, string Ime, string Priimek, bool Administrator, string Geslo, string PonoviGeslo)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                if (Geslo.Equals(PonoviGeslo))
                {
                    Uporabnik tmpUporabnik = new Uporabnik();

                    #region Mapping

                    tmpUporabnik.ID_uporabnik = UpraviteljUporabnik.GetNextIdUporabnik();
                    tmpUporabnik.Ime = Ime;
                    tmpUporabnik.Priimek = Priimek;
                    tmpUporabnik.Email = Email;

                    MD5 md5 = new MD5CryptoServiceProvider();
                    md5.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(Geslo));
                    byte[] result = md5.Hash;
                    tmpUporabnik.Geslo = Convert.ToBase64String(result);

                    Uporabnik m_trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                    tmpUporabnik.Spremenil = m_trenutniUporabnik.Ime + " " + m_trenutniUporabnik.Priimek;

                    if (Administrator)
                    {
                        tmpUporabnik.Administrator = 1;
                    }
                    else
                    {
                        tmpUporabnik.Administrator = 0;
                    }

                    tmpUporabnik.FlagPrijava = 0;
                    tmpUporabnik.FlagNapaka = 0;
                    tmpUporabnik.FlagKontakt = 0;
                    tmpUporabnik.FlagRegistracija = 0;
                    tmpUporabnik.FlagNapakaAvtorizacija = 0;
                    tmpUporabnik.FlagONas = 0;
                    tmpUporabnik.FlagPovezave = 0;
                    tmpUporabnik.FlagTreningi = 0;
                    tmpUporabnik.FlagTekmovanja = 0;
                    tmpUporabnik.FlagRezultati = 0;
                    tmpUporabnik.FlagGalerijaSlik = 0;

                    #endregion

                    UpraviteljUporabnik.ShraniUporabnika(tmpUporabnik);
                    logger.Info("User " + tmpUporabnik.Email + " inserted successfully.");

                    Uporabnik upr = UpraviteljUporabnik.VrniUporabnika(tmpUporabnik.ID_uporabnik);

                    ViewBag.Data = upr;

                    #region SetRoles

                    if (upr.Administrator == 1)
                    {
                        if (!Roles.IsUserInRole(upr.Email, "Admin"))
                        {
                            Roles.AddUserToRole(upr.Email, "Admin");
                        }
                    }
                    else
                    {
                        if (Roles.IsUserInRole(upr.Email, "Admin"))
                        {
                            Roles.RemoveUserFromRole(upr.Email, "Admin");
                        }
                    }

                    logger.Info("Role for user " + upr.Email + " updated successfully.");

                    #endregion

                    //Obvestilo o uspehu akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                    MailHelper.SendMailToUserForSuccessfulRegistration(upr.Email, Geslo);
                }
                else
                {
                    GlobalErrors.DodajNapako("Gesli se ne ujemata.");
                }
                
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("InsertUporabnik", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("UporabnikPodrobnosti");
        }

        #endregion

        #region Odjava

        [Authorize]
        public ActionResult Odjava()
        {
            FormsAuthentication.SignOut();

            GlobalNotifications.ZbrisiObvestilo();
            GlobalErrors.ZbrisiNapake();
            GlobalWarnings.ZbrisiOpozorilo();

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #endregion

        #region Kontakt

        #region VrniUrediKontakt

        [Authorize(Roles = "Admin")]
        public ActionResult VrniUrediKontakt()
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                Kontakt kontakt = UpraviteljKontakt.VrniKontakt();
                ViewBag.Data = kontakt;

                if (ViewBag.Data != null)
                {
                    //Obvestilo o uspehu akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                }
                else
                {
                    //Obvestilo o uspehu akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.DodajanjeKontakta);
                }
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("VrniUrediKontakt", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region InsertKontakt

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult InsertKontakt(string Email, string Telefon, string Fax, string GSM, string Naslov)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                #region Mapping

                Kontakt kontakt = new Kontakt();

                kontakt.ID_Kontakt = 1;

                Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                kontakt.Spremenil = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                kontakt.Email = Email;
                kontakt.Telefon = Telefon;
                kontakt.Fax = Fax;
                kontakt.GSM = GSM;
                kontakt.Naslov = Naslov;

                #endregion

                #region Insert

                UpraviteljKontakt.InsertKontakt(kontakt);

                #endregion

                Kontakt insKontakt = UpraviteljKontakt.VrniKontakt();
                ViewBag.Data = insKontakt;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                MailHelper.SendMailForNewKontakt(trenutniUporabnik.Ime, trenutniUporabnik.Priimek, trenutniUporabnik.Email);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("InsertKontakt", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("VrniUrediKontakt");
        }

        #endregion

        #region UpdateKontakt

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult UpdateKontakt(string Email, string Telefon, string Fax, string GSM, string Naslov)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                #region Mapping

                Kontakt kontakt = UpraviteljKontakt.VrniKontakt();

                Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                kontakt.Spremenil = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                kontakt.Email = Email;
                kontakt.Telefon = Telefon;
                kontakt.Fax = Fax;
                kontakt.GSM = GSM;
                kontakt.Naslov = Naslov;

                #endregion

                #region Update

                UpraviteljKontakt.UpdateKontakt(kontakt);

                #endregion

                Kontakt insKontakt = UpraviteljKontakt.VrniKontakt();
                ViewBag.Data = insKontakt;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                MailHelper.SendMailForNewKontakt(trenutniUporabnik.Ime, trenutniUporabnik.Priimek, trenutniUporabnik.Email);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("InsertKontakt", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("VrniUrediKontakt");
        }

        #endregion

        #region DeleteKontakt

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteKontakt()
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                UpraviteljKontakt.DeleteKontakt();

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("VrniUrediKontakt", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("../Home/Index");
        }

        #endregion

        #endregion

        #region Novice

        #region Prikazi vse novice

        [Authorize]
        public ActionResult UrediNovice()
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                List<NovicaGrid> seznamNovic = UpraviteljNovica.VrniNoviceFiltered("", "", "", "", "");

                ViewBag.Data = seznamNovic;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("UrediNovice", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("Novice");
        }

        #endregion

        #region Find Novica

        [HttpPost]
        [Authorize]
        public ActionResult FindNovica(string IDNovicaFilter, string NaslovFilter, string AvtorFilter)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                List<NovicaGrid> seznamNovic = UpraviteljNovica.VrniNoviceFiltered(IDNovicaFilter, NaslovFilter, AvtorFilter, "", "");

                ViewBag.Data = seznamNovic;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("FindNovica", ex.ToString());
                logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("Novice");
        }

        #endregion

        #region NovicaPodrobnosti

        [Authorize]
        public ActionResult NovicaPodrobnosti(int ID_novica)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                if (ID_novica > 0)
                {
                    Novica novica = UpraviteljNovica.VrniNovico(ID_novica);
                    ViewBag.Data = novica;

                    //Obvestilo o uspehu akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                }
                else
                {
                    ViewBag.Data = null;

                    //Obvestilo o dodajanju novice
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.DodajanjeNovice);
                }
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("NovicaPodrobnosti", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region UpdateNovica

        [HttpPost, ValidateInput(false)]
        [Authorize]
        public ActionResult UpdateNovica(int ID_novica, string Naslov, string Povzetek, string Vsebina, string Uporabnik, HttpPostedFileBase slika)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                Novica novica = UpraviteljNovica.VrniNovico(ID_novica);

                #region Save and delete Image

                string PathURLSlika = "";

                if (slika != null && slika.ContentLength > 0)
                {
                    if (novica.URLSlika != "")
                    {
                        //First delete current image fot this news 
                        var pot = Server.MapPath(novica.URLSlika);
                        System.IO.File.Delete(pot);
                    }

                    //Resize image
                    Image novaSlika = ImageHelper.ResizeImage(Image.FromStream(slika.InputStream, true, true), new Size(680, 510));

                    // code for saving the image file to a physical location.
                    var fileName = Path.GetFileName(slika.FileName);
                    var path = Path.Combine(Server.MapPath(KKKZusternaEnum.SaveNovica), fileName);
                    novaSlika.Save(path);
                    //slika.SaveAs(path);

                    PathURLSlika = Url.Content(Path.Combine(KKKZusternaEnum.SaveNovica, fileName));
                }

                #endregion

                #region Mapping

                novica.Naslov = Naslov;
                novica.Povzetek = Povzetek;
                novica.Vsebina = Vsebina;

                Uporabnik m_trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                novica.ID_uporabnik = m_trenutniUporabnik.ID_uporabnik;

                if (PathURLSlika != "")
                {
                    novica.URLSlika = PathURLSlika;   
                }

                #endregion

                #region Update Novica

                UpraviteljNovica.PosodobiNovico(novica);

                #endregion

                Novica posNovica = UpraviteljNovica.VrniNovico(novica.ID_novica);
                ViewBag.Data = posNovica;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);

            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("UpdateNovica", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("NovicaPodrobnosti");
        }

        #endregion

        #region DeleteNovica

        [Authorize]
        public ActionResult DeleteNovica(int ID_novica)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                Novica delNovica = UpraviteljNovica.VrniNovico(ID_novica);

                #region Delete image if delNovica.URLSlika != ""

                if (delNovica.URLSlika != "")
                {
                    // code for deleting the image file 
                    var path = Server.MapPath(delNovica.URLSlika);
                    System.IO.File.Delete(path);
                }

                #endregion

                UpraviteljNovica.DeleteNovica(delNovica.ID_novica);

                List<NovicaGrid> seznamNovic = UpraviteljNovica.VrniNoviceFiltered("", "", "", "", "");

                ViewBag.Data = seznamNovic;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("DeleteNovica", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("Novice");
        }

        #endregion

        #region DeleteNovicaImage

        [Authorize]
        public ActionResult DeleteNovicaImage(int ID_novica)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                Novica delNovica = UpraviteljNovica.VrniNovico(ID_novica);

                #region Delete image if delNovica.URLSlika != ""

                if (delNovica.URLSlika != "")
                {
                    // code for deleting the image file 
                    var path = Server.MapPath(delNovica.URLSlika);
                    System.IO.File.Delete(path);
                }

                #endregion

                UpraviteljNovica.DeleteNovicaImage(delNovica.ID_novica);

                Novica posNovica = UpraviteljNovica.VrniNovico(delNovica.ID_novica);
                ViewBag.Data = posNovica;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("DeleteNovicaImage", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("NovicaPodrobnosti");
        }

        #endregion

        #region InsertNovica

        [HttpPost, ValidateInput(false)]
        [Authorize]
        public ActionResult InsertNovica(string Naslov, string Povzetek, string Vsebina, HttpPostedFileBase slika)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                #region Save Image

                string PathURLSlika = "";

                if (slika != null && slika.ContentLength > 0)
                {
                    //Resize image
                    Image novaSlika = ImageHelper.ResizeImage(Image.FromStream(slika.InputStream, true, true), new Size(680, 510));

                    // code for saving the image file to a physical location.
                    var fileName = Path.GetFileName(slika.FileName);
                    var path = Path.Combine(Server.MapPath(KKKZusternaEnum.SaveNovica), fileName);
                    novaSlika.Save(path);
                    //slika.SaveAs(path);

                    PathURLSlika = Url.Content(Path.Combine(KKKZusternaEnum.SaveNovica, fileName));
                }

                #endregion

                #region Mapping

                Novica novica = new Novica();

                novica.ID_novica = UpraviteljNovica.GetNextIDNovica();
                novica.Naslov = Naslov;
                novica.Povzetek = Povzetek;
                novica.Vsebina = Vsebina;

                Uporabnik m_trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                novica.ID_uporabnik = m_trenutniUporabnik.ID_uporabnik;
                
                novica.URLSlika = PathURLSlika;

                #endregion

                #region Save novica

                UpraviteljNovica.ShraniNovico(novica);

                #endregion

                Novica shrNovica = UpraviteljNovica.VrniNovico(novica.ID_novica);
                ViewBag.Data = shrNovica;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("InsertNovica", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("NovicaPodrobnosti");
        }

        #endregion

        #endregion

        #region O Nas

        #region GetFormONas

        [Authorize]
        public ActionResult UrediONas()
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                ONas oNas = UpraviteljONas.VrniONas();
                ViewBag.Data = oNas;

                //Obvestilo o uspehu akcije
                if (oNas != null)
                {
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                }
                else
                {
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.DodajanjeONas);
                }
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("UrediONas", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region InsertONas

        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult InsertONas(string Vsebina)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                #region Mapping

                ONas tmpONas = new ONas();

                tmpONas.ID_ONas = 1;
                tmpONas.Vsebina = Vsebina;

                Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                tmpONas.Spremenil = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                tmpONas.SpremenilDatum = System.DateTime.Now;

                #endregion

                #region Save

                UpraviteljONas.ShraniONas(tmpONas);

                #endregion

                ONas shrONas = UpraviteljONas.VrniONas();
                ViewBag.Data = shrONas;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                MailHelper.SendMailForNewONas(trenutniUporabnik.Ime, trenutniUporabnik.Priimek, trenutniUporabnik.Email);

            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("InsertONas", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("UrediONas");
        }

        #endregion

        #region DeleteONas

        [Authorize]
        public ActionResult DeleteONas()
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                #region Delete ONas

                UpraviteljONas.ZbrisiONas();

                #endregion

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("DeleteONas", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("../KlubskiKoticek/ONas");
        }

        #endregion

        #region UpdateONas

        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateONas(string Vsebina)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                #region Mapping values

                ONas oNas = UpraviteljONas.VrniONas();

                oNas.Vsebina = Vsebina;

                Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                oNas.Spremenil = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                #endregion

                #region Update

                UpraviteljONas.UrediONas(oNas);

                #endregion

                ONas newONas = UpraviteljONas.VrniONas();
                ViewBag.Data = newONas;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                MailHelper.SendMailForNewONas(trenutniUporabnik.Ime, trenutniUporabnik.Priimek, trenutniUporabnik.Email);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("UpdateONas", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("UrediONas");
        }

        #endregion

        #endregion

        #region Povezave

        #region KategorijaPovezav

        #region UrediKategorijoPovezav

        [Authorize]
        public ActionResult UrediKategorijoPovezav()
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                //Select all KategorijaPovezav
                List<KategorijaPovezavGrid> seznam = UpraviteljKategorijePovezav.VrniKategorijePovezav();
                ViewBag.Data = seznam;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("UrediKategorijoPovezav", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("KategorijaPovezav");
        }

        #endregion

        #region FindKategorijaPovezav

        [HttpPost]
        [Authorize]
        public ActionResult FindKategorijaPovezav(string ID_KategorijaPovezav, string Naslov)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                List<KategorijaPovezavGrid> seznamFiltered = UpraviteljKategorijePovezav.VrniKategorijePovezavFiltered(ID_KategorijaPovezav, Naslov);
                ViewBag.Data = seznamFiltered;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("FindKategorijaPovezav", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("KategorijaPovezav");
        }

        #endregion

        #region KategorijaPovezavPodrobnosti

        [Authorize]
        public ActionResult KategorijaPovezavPodrobnosti(int ID_KategorijaPovezav)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                if (ID_KategorijaPovezav > 0)
                {
                    //Update
                    KategorijaPovezav kategorijaPovezav = UpraviteljKategorijePovezav.VrniKategorijoPovezav(ID_KategorijaPovezav);
                    ViewBag.Data = kategorijaPovezav;

                    List<PovezaveGrid> seznamPovezav = UpraviteljPovezav.VrniPovezaveZaKategorijo(ID_KategorijaPovezav);
                    ViewBag.SeznamPovezav = seznamPovezav;

                    //Obvestilo o uspehu akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                }
                else
                {
                    //Insert
                    ViewBag.Data = null;
                    ViewBag.SeznamPovezav = null;

                    //Obvestilo o uspehu priprave akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.DodajanjeKategorijePovezav);
                }
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("KategorijaPovezavPodrobnosti", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region InsertKategorijoPovezav

        [HttpPost]
        [Authorize]
        public ActionResult InsertKategorijoPovezav(string Naslov)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                #region Mapping

                KategorijaPovezav kategorijaPovezav = new KategorijaPovezav();

                kategorijaPovezav.ID_KategorijaPovezav = UpraviteljKategorijePovezav.GetNextIdKategorijaPovezav();
                kategorijaPovezav.Naslov = Naslov;

                Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                kategorijaPovezav.Spremenil = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                #endregion

                #region Save

                UpraviteljKategorijePovezav.ShraniKategorijoPovezav(kategorijaPovezav);

                #endregion

                KategorijaPovezav saveKategorijaPovezav = UpraviteljKategorijePovezav.VrniKategorijoPovezav(kategorijaPovezav.ID_KategorijaPovezav);
                ViewBag.Data = saveKategorijaPovezav;

                List<PovezaveGrid> seznamPovezav = UpraviteljPovezav.VrniPovezaveZaKategorijo(saveKategorijaPovezav.ID_KategorijaPovezav);
                ViewBag.SeznamPovezav = seznamPovezav;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("InsertKategorijoPovezav", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("KategorijaPovezavPodrobnosti");
        }

        #endregion

        #region DeleteKategorijaPovezav

        public ActionResult DeleteKategorijaPovezav(int ID_KategorijaPovezav)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                #region Delete all Povezave where ID_KategorijaPovezav = ID_KategorijaPovezav

                UpraviteljPovezav.DeletePovezaveZaKategorijo(ID_KategorijaPovezav);

                #endregion

                #region Delete KategorijaPovezav

                UpraviteljKategorijePovezav.DeleteKategorijePovezav(ID_KategorijaPovezav);

                #endregion

                List<KategorijaPovezavGrid> seznamKategorijPovezav = UpraviteljKategorijePovezav.VrniKategorijePovezav();
                ViewBag.Data = seznamKategorijPovezav;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("DeleteKategorijaPovezav", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("KategorijaPovezav");
        }

        #endregion

        #region UpdateKategorijaPovezave

        [HttpPost]
        [Authorize]
        public ActionResult UpdateKategorijaPovezave(int ID_KategorijaPovezav, string Naslov)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                #region Mapping

                KategorijaPovezav kategorijaPovezav = UpraviteljKategorijePovezav.VrniKategorijoPovezav(ID_KategorijaPovezav);
                kategorijaPovezav.Naslov = Naslov;

                Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                kategorijaPovezav.Spremenil = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                #endregion

                #region Update

                UpraviteljKategorijePovezav.UpdateKategorijoPovezav(kategorijaPovezav);

                #endregion

                KategorijaPovezav saveKategorijaPovezav = UpraviteljKategorijePovezav.VrniKategorijoPovezav(kategorijaPovezav.ID_KategorijaPovezav);
                ViewBag.Data = saveKategorijaPovezav;

                List<PovezaveGrid> seznamPovezav = UpraviteljPovezav.VrniPovezaveZaKategorijo(saveKategorijaPovezav.ID_KategorijaPovezav);
                ViewBag.SeznamPovezav = seznamPovezav;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("InsertKategorijoPovezav", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("KategorijaPovezavPodrobnosti");
        }

        #endregion

        #endregion

        #region Povezave

        #region FindPovezave

        [HttpPost]
        [Authorize]
        public ActionResult FindPovezave(int ID_KategorijaPovezav, string ID_Povezave, string Naziv)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                KategorijaPovezav kategorijaPovezav = UpraviteljKategorijePovezav.VrniKategorijoPovezav(ID_KategorijaPovezav);
                ViewBag.Data = kategorijaPovezav;

                List<PovezaveGrid> seznamFiltered = UpraviteljPovezav.VrniPovezaveZaKategorijoFiltered(ID_KategorijaPovezav, ID_Povezave, Naziv);
                ViewBag.SeznamPovezav = seznamFiltered;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("FindPovezave", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("KategorijaPovezavPodrobnosti");
        }

        #endregion

        #region PovezavePodrobnosti

        [Authorize]
        public ActionResult PovezavePodrobnosti(int ID_Povezave, int ID_KategorijaPovezav)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                if (ID_Povezave > 0)
                {
                    //Update
                    Povezave povezava = UpraviteljPovezav.VrniPovezavoZaId(ID_Povezave);
                    ViewBag.Data = povezava;

                    //Obvestilo o uspehu akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                }
                else
                {
                    //Insert
                    ViewBag.Data = null;
                    ViewBag.ID_KategorijaPovezav = ID_KategorijaPovezav;

                    //Obvestilo o uspehu priprave akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.DodajanjePovezave);
                }
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("PovezavePodrobnosti", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region InsertPovezava

        [HttpPost]
        [Authorize]
        public ActionResult InsertPovezava(int ID_KategorijaPovezav, string Naziv, string URL)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                #region Mapping

                Povezave povezava = new Povezave();

                povezava.ID_Povezava = UpraviteljPovezav.VrniNextIdForPovezave();
                povezava.ID_KategorijaPovezav = ID_KategorijaPovezav;
                povezava.Naziv = Naziv;
                povezava.URL = URL;

                Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                povezava.Spremenil = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                #endregion

                #region Save

                UpraviteljPovezav.ShraniPovezavo(povezava);

                #endregion

                Povezave shrPovezava = UpraviteljPovezav.VrniPovezavoZaId(povezava.ID_Povezava);
                ViewBag.Data = shrPovezava;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                MailHelper.SendMailForNewPovezave(trenutniUporabnik.Ime, trenutniUporabnik.Priimek, trenutniUporabnik.Email, shrPovezava.Naziv);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("InsertPovezava", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("PovezavePodrobnosti");
        }

        #endregion

        #region UpdatePovezava

        [HttpPost]
        [Authorize]
        public ActionResult UpdatePovezava(int ID_Povezava, string Naziv, string URL)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                #region Mapping

                Povezave povezava = UpraviteljPovezav.VrniPovezavoZaId(ID_Povezava);

                povezava.Naziv = Naziv;
                povezava.URL = URL;

                Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                povezava.Spremenil = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                #endregion

                #region Update

                UpraviteljPovezav.UpdatePovezava(povezava);

                #endregion

                Povezave updPovezava = UpraviteljPovezav.VrniPovezavoZaId(povezava.ID_Povezava);
                ViewBag.Data = updPovezava;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("UpdatePovezava", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("PovezavePodrobnosti");
        }

        #endregion

        #region DeletePovezava

        [Authorize]
        public ActionResult DeletePovezava(int ID_Povezava)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                Povezave povezava = UpraviteljPovezav.VrniPovezavoZaId(ID_Povezava);
                int ID_KategorijaPovezav = povezava.ID_KategorijaPovezav;

                #region Delete

                UpraviteljPovezav.DeletePovezava(povezava.ID_Povezava);

                #endregion

                KategorijaPovezav kategorijaPovezav = UpraviteljKategorijePovezav.VrniKategorijoPovezav(ID_KategorijaPovezav);
                ViewBag.Data = kategorijaPovezav;

                List<PovezaveGrid> seznamPovezav = UpraviteljPovezav.VrniPovezaveZaKategorijo(kategorijaPovezav.ID_KategorijaPovezav);
                ViewBag.SeznamPovezav = seznamPovezav;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("DeletePovezava", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("KategorijaPovezavPodrobnosti");
        }

        #endregion

        #endregion

        #endregion

        #region Treningi

        #region UrediKategorijoTreningov

        [Authorize]
        public ActionResult UrediKategorijoTreningov()
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                //Get all object Trening
                List<TreningGrid> seznamTreningov = UpraviteljTrening.VrniTreninge();
                ViewBag.Data = seznamTreningov;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("UrediKategorijoTreningov", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("KategorijaTreningov");
        }

        #endregion

        #region TreningPodrobnosti

        [Authorize]
        public ActionResult TreningPodrobnosti(int ID_Trening)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                if (ID_Trening > 0)
                {
                    //Update
                    Trening trening = UpraviteljTrening.VrniTreningZaId(ID_Trening);
                    ViewBag.Data = trening;

                    //Obvestilo o uspehu akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                }
                else
                {
                    //Insert
                    ViewBag.Data = null;

                    //Obvestilo o uspehu akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.DodajanjeTreninga);
                }
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("TreningPodrobnosti", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region FindTreningi

        [HttpPost]
        [Authorize]
        public ActionResult FindTreningi(string ID_Trening, string Naziv)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                List<TreningGrid> seznamTreningov = UpraviteljTrening.VrniTreningeFilter(ID_Trening, Naziv);
                ViewBag.Data = seznamTreningov;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("FindTreningi", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("KategorijaTreningov");
        }

        #endregion

        #region InsertTrening

        [HttpPost]
        [Authorize]
        public ActionResult InsertTrening(string Naziv,
                                         string Pon_Dop_Od,
                                         string Pon_Dop_Do,
                                         string Pon_Dop_Tre,
                                         string Pon_Pop_Od,
                                         string Pon_Pop_Do,
                                         string Pon_Pop_Tre,
                                         string Tor_Dop_Od,
                                         string Tor_Dop_Do,
                                         string Tor_Dop_Tre,
                                         string Tor_Pop_Od,
                                         string Tor_Pop_Do,
                                         string Tor_Pop_Tre,
                                         string Sre_Dop_Od,
                                         string Sre_Dop_Do,
                                         string Sre_Dop_Tre,
                                         string Sre_Pop_Od,
                                         string Sre_Pop_Do,
                                         string Sre_Pop_Tre,
                                         string Cet_Dop_Od,
                                         string Cet_Dop_Do,
                                         string Cet_Dop_Tre,
                                         string Cet_Pop_Od,
                                         string Cet_Pop_Do,
                                         string Cet_Pop_Tre,
                                         string Pet_Dop_Od,
                                         string Pet_Dop_Do,
                                         string Pet_Dop_Tre,
                                         string Pet_Pop_Od,
                                         string Pet_Pop_Do,
                                         string Pet_Pop_Tre,
                                         string Sob_Dop_Od,
                                         string Sob_Dop_Do,
                                         string Sob_Dop_Tre,
                                         string Sob_Pop_Od,
                                         string Sob_Pop_Do,
                                         string Sob_Pop_Tre,
                                         string Ned_Dop_Od,
                                         string Ned_Dop_Do,
                                         string Ned_Dop_Tre,
                                         string Ned_Pop_Od,
                                         string Ned_Pop_Do,
                                         string Ned_Pop_Tre)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                #region Mapping

                Trening trening = new Trening();

                trening.ID_Trening = UpraviteljTrening.VrniNextId();
                trening.Naziv = Naziv;

                Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                trening.Spremenil = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                trening.Pon_Dop_Od = Pon_Dop_Od;
                trening.Pon_Dop_Do = Pon_Dop_Do;
                trening.Pon_Dop_Tre = Pon_Dop_Tre;
                trening.Pon_Pop_Od = Pon_Pop_Od;
                trening.Pon_Pop_Do = Pon_Pop_Do;
                trening.Pon_Pop_Tre = Pon_Pop_Tre;

                trening.Tor_Dop_Od = Tor_Dop_Od;
                trening.Tor_Dop_Do = Tor_Dop_Do;
                trening.Tor_Dop_Tre = Tor_Dop_Tre;
                trening.Tor_Pop_Od = Tor_Pop_Od;
                trening.Tor_Pop_Do = Tor_Pop_Do;
                trening.Tor_Pop_Tre = Tor_Pop_Tre;

                trening.Sre_Dop_Od = Sre_Dop_Od;
                trening.Sre_Dop_Do = Sre_Dop_Do;
                trening.Sre_Dop_Tre = Sre_Dop_Tre;
                trening.Sre_Pop_Od = Sre_Pop_Od;
                trening.Sre_Pop_Do = Sre_Pop_Do;
                trening.Sre_Pop_Tre = Sre_Pop_Tre;

                trening.Cet_Dop_Od = Cet_Dop_Od;
                trening.Cet_Dop_Do = Cet_Dop_Do;
                trening.Cet_Dop_Tre = Cet_Dop_Tre;
                trening.Cet_Pop_Od = Cet_Pop_Od;
                trening.Cet_Pop_Do = Cet_Pop_Do;
                trening.Cet_Pop_Tre = Cet_Pop_Tre;

                trening.Pet_Dop_Od = Pet_Dop_Od;
                trening.Pet_Dop_Do = Pet_Dop_Do;
                trening.Pet_Dop_Tre = Pet_Dop_Tre;
                trening.Pet_Pop_Od = Pet_Pop_Od;
                trening.Pet_Pop_Do = Pet_Pop_Do;
                trening.Pet_Pop_Tre = Pet_Pop_Tre;

                trening.Sob_Dop_Od = Sob_Dop_Od;
                trening.Sob_Dop_Do = Sob_Dop_Do;
                trening.Sob_Dop_Tre = Sob_Dop_Tre;
                trening.Sob_Pop_Od = Sob_Pop_Od;
                trening.Sob_Pop_Do = Sob_Pop_Do;
                trening.Sob_Pop_Tre = Sob_Pop_Tre;

                trening.Ned_Dop_Od = Ned_Dop_Od;
                trening.Ned_Dop_Do = Ned_Dop_Do;
                trening.Ned_Dop_Tre = Ned_Dop_Tre;
                trening.Ned_Pop_Od = Ned_Pop_Od;
                trening.Ned_Pop_Do = Ned_Pop_Do;
                trening.Ned_Pop_Tre = Ned_Pop_Tre;

                #endregion

                #region Insert

                UpraviteljTrening.ShraniTrening(trening);

                #endregion

                Trening insTrening = UpraviteljTrening.VrniTreningZaId(trening.ID_Trening);
                ViewBag.Data = insTrening;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                MailHelper.SendMailForNewTreningi(trenutniUporabnik.Ime, trenutniUporabnik.Priimek, trenutniUporabnik.Email, insTrening.Naziv);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("InsertTrening", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("TreningPodrobnosti");
        }

        #endregion

        #region UpdateTrening

        [HttpPost]
        [Authorize]
        public ActionResult UpdateTrening(int ID_Trening,
                                         string Naziv,
                                         string Pon_Dop_Od,
                                         string Pon_Dop_Do,
                                         string Pon_Dop_Tre,
                                         string Pon_Pop_Od,
                                         string Pon_Pop_Do,
                                         string Pon_Pop_Tre,
                                         string Tor_Dop_Od,
                                         string Tor_Dop_Do,
                                         string Tor_Dop_Tre,
                                         string Tor_Pop_Od,
                                         string Tor_Pop_Do,
                                         string Tor_Pop_Tre,
                                         string Sre_Dop_Od,
                                         string Sre_Dop_Do,
                                         string Sre_Dop_Tre,
                                         string Sre_Pop_Od,
                                         string Sre_Pop_Do,
                                         string Sre_Pop_Tre,
                                         string Cet_Dop_Od,
                                         string Cet_Dop_Do,
                                         string Cet_Dop_Tre,
                                         string Cet_Pop_Od,
                                         string Cet_Pop_Do,
                                         string Cet_Pop_Tre,
                                         string Pet_Dop_Od,
                                         string Pet_Dop_Do,
                                         string Pet_Dop_Tre,
                                         string Pet_Pop_Od,
                                         string Pet_Pop_Do,
                                         string Pet_Pop_Tre,
                                         string Sob_Dop_Od,
                                         string Sob_Dop_Do,
                                         string Sob_Dop_Tre,
                                         string Sob_Pop_Od,
                                         string Sob_Pop_Do,
                                         string Sob_Pop_Tre,
                                         string Ned_Dop_Od,
                                         string Ned_Dop_Do,
                                         string Ned_Dop_Tre,
                                         string Ned_Pop_Od,
                                         string Ned_Pop_Do,
                                         string Ned_Pop_Tre)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                #region Mapping

                Trening trening = UpraviteljTrening.VrniTreningZaId(ID_Trening);
                trening.Naziv = Naziv;

                Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                trening.Spremenil = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                trening.Pon_Dop_Od = Pon_Dop_Od;
                trening.Pon_Dop_Do = Pon_Dop_Do;
                trening.Pon_Dop_Tre = Pon_Dop_Tre;
                trening.Pon_Pop_Od = Pon_Pop_Od;
                trening.Pon_Pop_Do = Pon_Pop_Do;
                trening.Pon_Pop_Tre = Pon_Pop_Tre;

                trening.Tor_Dop_Od = Tor_Dop_Od;
                trening.Tor_Dop_Do = Tor_Dop_Do;
                trening.Tor_Dop_Tre = Tor_Dop_Tre;
                trening.Tor_Pop_Od = Tor_Pop_Od;
                trening.Tor_Pop_Do = Tor_Pop_Do;
                trening.Tor_Pop_Tre = Tor_Pop_Tre;

                trening.Sre_Dop_Od = Sre_Dop_Od;
                trening.Sre_Dop_Do = Sre_Dop_Do;
                trening.Sre_Dop_Tre = Sre_Dop_Tre;
                trening.Sre_Pop_Od = Sre_Pop_Od;
                trening.Sre_Pop_Do = Sre_Pop_Do;
                trening.Sre_Pop_Tre = Sre_Pop_Tre;

                trening.Cet_Dop_Od = Cet_Dop_Od;
                trening.Cet_Dop_Do = Cet_Dop_Do;
                trening.Cet_Dop_Tre = Cet_Dop_Tre;
                trening.Cet_Pop_Od = Cet_Pop_Od;
                trening.Cet_Pop_Do = Cet_Pop_Do;
                trening.Cet_Pop_Tre = Cet_Pop_Tre;

                trening.Pet_Dop_Od = Pet_Dop_Od;
                trening.Pet_Dop_Do = Pet_Dop_Do;
                trening.Pet_Dop_Tre = Pet_Dop_Tre;
                trening.Pet_Pop_Od = Pet_Pop_Od;
                trening.Pet_Pop_Do = Pet_Pop_Do;
                trening.Pet_Pop_Tre = Pet_Pop_Tre;

                trening.Sob_Dop_Od = Sob_Dop_Od;
                trening.Sob_Dop_Do = Sob_Dop_Do;
                trening.Sob_Dop_Tre = Sob_Dop_Tre;
                trening.Sob_Pop_Od = Sob_Pop_Od;
                trening.Sob_Pop_Do = Sob_Pop_Do;
                trening.Sob_Pop_Tre = Sob_Pop_Tre;

                trening.Ned_Dop_Od = Ned_Dop_Od;
                trening.Ned_Dop_Do = Ned_Dop_Do;
                trening.Ned_Dop_Tre = Ned_Dop_Tre;
                trening.Ned_Pop_Od = Ned_Pop_Od;
                trening.Ned_Pop_Do = Ned_Pop_Do;
                trening.Ned_Pop_Tre = Ned_Pop_Tre;

                #endregion

                #region Insert

                UpraviteljTrening.UpdateTrening(trening);

                #endregion

                Trening updTrening = UpraviteljTrening.VrniTreningZaId(trening.ID_Trening);
                ViewBag.Data = updTrening;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("UpdateTrening", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("TreningPodrobnosti");
        }

        #endregion

        #region DeleteTrening

        [Authorize]
        public ActionResult DeleteTrening(int ID_Trening)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                UpraviteljTrening.DeleteTrening(ID_Trening);

                List<TreningGrid> seznam = UpraviteljTrening.VrniTreninge();
                ViewBag.Data = seznam;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("DeleteTrening", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("KategorijaTreningov");
        }

        #endregion

        #endregion

        #region Tekmovanja

        #region LetoTekmovanja

        #region Prikazi kategorijo "LetoTekmovanja"

        [Authorize]
        public ActionResult UrediLetoTekmovanja()
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                //Get list of LetoTekmovanja
                List<LetoTekmovanja> seznam = UpraviteljLetoTekmovanja.VrniLetoTekmovanja();
                ViewBag.Data = seznam;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("UrediLetoTekmovanja", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("LetoTekmovanja");
        }

        #endregion

        #region LetoTekmovanjaPodrobnosti

        [Authorize]
        public ActionResult LetoTekmovanjaPodrobnosti(int ID_letoTekmovanja)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                if (ID_letoTekmovanja > 0)
                {
                    //Get letoTekmovanja
                    LetoTekmovanja letoTekmovanja = UpraviteljLetoTekmovanja.VrniLetoTekmovanja(ID_letoTekmovanja);
                    ViewBag.Data = letoTekmovanja;

                    //Get seznam tekmovanj za izbrano leto
                    List<Tekmovanja> seznamTekmovanj = UpraviteljTekmovanja.VrniTekmovanjaZaLeto(ID_letoTekmovanja);
                    ViewBag.SeznamTekmovanj = seznamTekmovanj;

                    //Obvestilo o uspehu akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                }
                else
                {
                    ViewBag.Data = null;

                    //Obvestilo o dodajanju novice
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.DodajanjeLetaTekmovanj);
                }
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("LetoTekmovanjaPodrobnosti", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region InsertLetoTekmovanja

        [HttpPost]
        [Authorize]
        public ActionResult InsertLetoTekmovanja(string Leto)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                if (Leto != null && Leto != "" && Leto != "--Izberi--")
                {
                    #region Mapping

                    LetoTekmovanja novoLetoTekmovanja = new LetoTekmovanja();

                    novoLetoTekmovanja.ID_letoTekmovanja = UpraviteljLetoTekmovanja.GetNextIdLetoTekmovanja();
                    novoLetoTekmovanja.Leto = Leto;

                    Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                    novoLetoTekmovanja.Spremenil = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                    #endregion

                    //Save
                    UpraviteljLetoTekmovanja.ShraniLetoTekmovanja(novoLetoTekmovanja);

                    //Get inserted letoTekmovanja
                    LetoTekmovanja letoTekmovanja = UpraviteljLetoTekmovanja.VrniLetoTekmovanja(novoLetoTekmovanja.ID_letoTekmovanja);
                    ViewBag.Data = letoTekmovanja;

                    //Target SeznamRezultatov to null
                    ViewBag.SeznamRezultatov = null;

                    //Obvestilo o uspehu akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                }
                else
                {
                    GlobalErrors.DodajNapako("Izberite ustrezno leto.");
                }
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("InsertLetoTekmovanja", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("LetoTekmovanjaPodrobnosti");
        }

        #endregion

        #region FindLetoTekmovanja

        [HttpPost]
        [Authorize]
        public ActionResult FindLetoTekmovanja(string IDLetoTekmovanjaFilter, string LetoFilter)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                //Get list of LetoRezultat
                List<LetoTekmovanja> seznam = UpraviteljLetoTekmovanja.VrniLetoTekmovanjaFilter(IDLetoTekmovanjaFilter, LetoFilter);
                ViewBag.Data = seznam;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("FindLetoTekmovanja", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("LetoTekmovanja");
        }

        #endregion

        #region DeleteLetoTekmovanja

        [Authorize]
        public ActionResult DeleteLetoTekmovanja(int ID_letoTekmovanja)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                #region Delete tekmovanja where ID_letoTekmovanja == ID_letoTekmovanja

                List<Tekmovanja> seznamTekmovanj = UpraviteljTekmovanja.VrniTekmovanjaZaLeto(ID_letoTekmovanja);

                foreach (Tekmovanja tekmovanja in seznamTekmovanj)
                {
                    if (tekmovanja.URLFile != "")
                    {
                        // code for deleting the image file 
                        var path = Server.MapPath(tekmovanja.URLFile);
                        System.IO.File.Delete(path);
                    }
                }

                UpraviteljTekmovanja.DeleteTekmovanjaZaLeto(ID_letoTekmovanja);

                #endregion

                #region Delete LetoRezultati

                UpraviteljLetoTekmovanja.DeleteLetoTekmovanja(ID_letoTekmovanja);

                #endregion

                //Get seznam LetoRezultati
                List<LetoTekmovanja> seznam = UpraviteljLetoTekmovanja.VrniLetoTekmovanja();
                ViewBag.Data = seznam;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);

            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("DeleteLetoTekmovanja", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("LetoTekmovanja");
        }

        #endregion

        #endregion

        #region Tekmovanja

        #region FindTekmovanja

        [Authorize]
        public ActionResult FindTekmovanja(string IDTekmovanjaFilter, string NaslovFilter, int IDLetoTekmovanja)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                //Get object LetoTekmovanja
                LetoTekmovanja letoTekmovanja = UpraviteljLetoTekmovanja.VrniLetoTekmovanja(IDLetoTekmovanja);
                ViewBag.Data = letoTekmovanja;

                //Get seznam tekmovanj where Naslov LIKE NaslovFilter
                List<Tekmovanja> seznamTekmovanj = UpraviteljTekmovanja.VrniTekmovanjaFilter(IDLetoTekmovanja, IDTekmovanjaFilter, NaslovFilter);
                ViewBag.SeznamTekmovanj = seznamTekmovanj;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("FindTekmovanja", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("LetoTekmovanjaPodrobnosti");
        }

        #endregion

        #region TekmovanjaPodrobnosti

        [Authorize]
        public ActionResult TekmovanjaPodrobnosti(int ID_tekmovanja, int ID_letoTekmovanja)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                if (ID_tekmovanja > 0)
                {
                    //Get rezultat
                    Tekmovanja tekmovanja = UpraviteljTekmovanja.VrniTekmovanja(ID_tekmovanja);
                    ViewBag.Data = tekmovanja;

                    //Obvestilo o uspehu akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                }
                else
                {
                    ViewBag.Data = null;
                    ViewBag.ID_LetoTekmovanja = ID_letoTekmovanja;

                    //Obvestilo o dodajanju novice
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.DodajanjeTekmovanj);
                }
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("TekmovanjaPodrobnosti", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region InsertTekmovanje

        [HttpPost, ValidateInput(false)]
        [Authorize]
        public ActionResult InsertTekmovanje(string Naslov, string Vsebina, HttpPostedFileBase datoteka, string FileNameTekmovanje, int ID_LetoTekmovanja)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                #region Save File

                string PathURLDatoteka = "";
                string fileName = "";

                if (datoteka != null && datoteka.ContentLength > 0)
                {
                    // code for saving the image file to a physical location.
                    fileName = Path.GetFileName(datoteka.FileName);
                    var path = Path.Combine(Server.MapPath(KKKZusternaEnum.SaveTekmovanja), fileName);
                    datoteka.SaveAs(path);

                    PathURLDatoteka = Url.Content(Path.Combine(KKKZusternaEnum.SaveTekmovanja, fileName));
                }

                #endregion

                #region Mapping

                Tekmovanja tekmovanja = new Tekmovanja();

                tekmovanja.ID_tekmovanja = UpraviteljTekmovanja.GetNextIdTekmovanja();
                tekmovanja.Naslov = Naslov;
                tekmovanja.Vsebina = Vsebina;
                tekmovanja.URLFile = PathURLDatoteka;
                tekmovanja.FileName = string.IsNullOrEmpty(FileNameTekmovanje) ? fileName : FileNameTekmovanje;
                tekmovanja.ID_letoTekmovanja = ID_LetoTekmovanja;

                Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                tekmovanja.Spremenil = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                #endregion

                #region Save

                UpraviteljTekmovanja.ShraniTekmovanja(tekmovanja);

                #endregion

                Tekmovanja shrTekmovanja = UpraviteljTekmovanja.VrniTekmovanja(tekmovanja.ID_tekmovanja);
                ViewBag.Data = shrTekmovanja;
                
                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                MailHelper.SendMailForNewTekmovanja(trenutniUporabnik.Ime, trenutniUporabnik.Priimek, trenutniUporabnik.Email, shrTekmovanja.Naslov);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("InsertTekmovanje", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("TekmovanjaPodrobnosti");
        }

        #endregion

        #region DeleteTekmovanje

        [Authorize]
        public ActionResult DeleteTekmovanje(int ID_tekmovanja)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                int id_letoTekmovanja = 0;

                //Get object tekmovanja
                Tekmovanja tekmovanja = UpraviteljTekmovanja.VrniTekmovanja(ID_tekmovanja);
                id_letoTekmovanja = tekmovanja.ID_letoTekmovanja;

                //Delete File in rezultati
                if (tekmovanja.URLFile != "")
                {
                    // code for deleting the image file 
                    var path = Server.MapPath(tekmovanja.URLFile);
                    System.IO.File.Delete(path);
                }

                //Delete rezultati from DB
                UpraviteljTekmovanja.DeleteTekmovanja(ID_tekmovanja);

                //Get object LetoRezultat
                LetoTekmovanja letoTekmovanja = UpraviteljLetoTekmovanja.VrniLetoTekmovanja(id_letoTekmovanja);
                ViewBag.Data = letoTekmovanja;

                //Get seznam rezultatov where Naslov LIKE NaslovFilter
                List<Tekmovanja> seznamTekmovanj = UpraviteljTekmovanja.VrniTekmovanjaZaLeto(letoTekmovanja.ID_letoTekmovanja);
                ViewBag.SeznamTekmovanj = seznamTekmovanj;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("DeleteTekmovanje", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("LetoTekmovanjaPodrobnosti");
        }

        #endregion

        #region UpdateTekmovanje

        [HttpPost, ValidateInput(false)]
        [Authorize]
        public ActionResult UpdateTekmovanje(string ID_tekmovanja, string NaslovTekmovanja, string VsebinaTekmovanja, HttpPostedFileBase datoteka, string FileNameTekmovanje)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                Tekmovanja tekmovanja = UpraviteljTekmovanja.VrniTekmovanja(Convert.ToInt32(ID_tekmovanja));

                #region Save File

                string PathURLDatoteka = tekmovanja.URLFile;
                string fileName = "";

                if (datoteka != null && datoteka.ContentLength > 0)
                {
                    //Delete existing File
                    if (tekmovanja.URLFile != "")
                    {
                        // code for deleting the image file 
                        var pathDel = Server.MapPath(tekmovanja.URLFile);
                        System.IO.File.Delete(pathDel);
                    }

                    // code for saving the image file to a physical location.
                    fileName = Path.GetFileName(datoteka.FileName);
                    var path = Path.Combine(Server.MapPath(KKKZusternaEnum.SaveTekmovanja), fileName);
                    datoteka.SaveAs(path);

                    PathURLDatoteka = Url.Content(Path.Combine(KKKZusternaEnum.SaveTekmovanja, fileName));
                }

                #endregion

                #region Mapping

                tekmovanja.Naslov = NaslovTekmovanja;
                tekmovanja.Vsebina = VsebinaTekmovanja;
                tekmovanja.URLFile = PathURLDatoteka;
                tekmovanja.FileName = string.IsNullOrEmpty(FileNameTekmovanje) ? fileName : FileNameTekmovanje;

                Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                tekmovanja.Spremenil = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                #endregion

                #region Update

                UpraviteljTekmovanja.UpdateTekmovanja(tekmovanja);

                #endregion

                Tekmovanja updTekmovanja = UpraviteljTekmovanja.VrniTekmovanja(tekmovanja.ID_tekmovanja);
                ViewBag.Data = updTekmovanja;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("UpdateTekmovanje", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("TekmovanjaPodrobnosti");
        }

        #endregion

        #endregion

        #endregion

        #region Rezultati

        #region LetoRezultati

        #region Prikazi kategorijo "Leto rezultatov"

        [Authorize]
        public ActionResult UrediLetoRezultati()
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                //Get list of LetoRezultat
                List<LetoRezultati> seznam = UpraviteljLetoRezultati.VrniLetoRezultat();
                ViewBag.Data = seznam;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("UrediLetoRezultati", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("LetoRezultati");
        }

        #endregion

        #region FindLetoRezultati

        [HttpPost]
        [Authorize]
        public ActionResult FindLetoRezultati(string IDLetoRezultatiFilter, string LetoFilter)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                //Get list of LetoRezultat
                List<LetoRezultati> seznam = UpraviteljLetoRezultati.VrniLetoRezultatFilter(IDLetoRezultatiFilter, LetoFilter);
                ViewBag.Data = seznam;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("FindLetoRezultati", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("LetoRezultati");
        }

        #endregion

        #region LetoRezultatiPodrobnosti

        [Authorize]
        public ActionResult LetoRezultatiPodrobnosti(int ID_letoRezultati)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                if (ID_letoRezultati > 0)
                {
                    //Get letoRezultati
                    LetoRezultati letoRezultati = UpraviteljLetoRezultati.VrniLetoRezultat(ID_letoRezultati);
                    ViewBag.Data = letoRezultati;

                    //Get seznam rezultatov za izbrano leto
                    List<Rezultati> seznamRezultatov = UpraviteljRezultati.VrniRezultateZaLeto(ID_letoRezultati);
                    ViewBag.SeznamRezultatov = seznamRezultatov;

                    //Obvestilo o uspehu akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                }
                else
                {
                    ViewBag.Data = null;

                    //Obvestilo o dodajanju novice
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.DodajanjeLetaRezultatov);
                }
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("LetoRezultatiPodrobnosti", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region InsertLetoRezultatov

        [HttpPost]
        [Authorize]
        public ActionResult InsertLetoRezultatov(string Leto)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                if (Leto != null && Leto != "" && Leto != "--Izberi--")
                {
                    #region Mapping

                    LetoRezultati novoLetoRezultati = new LetoRezultati();

                    novoLetoRezultati.ID_letoRezultati = UpraviteljLetoRezultati.GetNextIdLetoRezultati();
                    novoLetoRezultati.Leto = Leto;

                    Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                    novoLetoRezultati.Spremenil = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                    #endregion

                    //Save
                    UpraviteljLetoRezultati.ShraniLetoRezultati(novoLetoRezultati);

                    //Get inserted letoRezultati
                    LetoRezultati letoRezultati = UpraviteljLetoRezultati.VrniLetoRezultat(novoLetoRezultati.ID_letoRezultati);
                    ViewBag.Data = letoRezultati;

                    //Target SeznamRezultatov to null
                    ViewBag.SeznamRezultatov = null;

                    //Obvestilo o uspehu akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                }
                else
                {
                    GlobalErrors.DodajNapako("Izberite ustrezno leto.");
                }
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("InsertLetoRezultatov", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("LetoRezultatiPodrobnosti");
        }

        #endregion

        #region DeleteLetoRezultati

        [Authorize]
        public ActionResult DeleteLetoRezultati(int ID_letoRezultati)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                #region Delete rezultati where ID_letoRezultati == ID_letoRezultati

                List<Rezultati> seznamRezultati = UpraviteljRezultati.VrniRezultateZaLeto(ID_letoRezultati);

                foreach (Rezultati rezultati in seznamRezultati)
                {
                    if (rezultati.URLFile != "")
                    {
                        // code for deleting the image file 
                        var path = Server.MapPath(rezultati.URLFile);
                        System.IO.File.Delete(path);
                    }
                }

                UpraviteljRezultati.DeleteRezultatiZaLeto(ID_letoRezultati);

                #endregion

                #region Delete LetoRezultati

                UpraviteljLetoRezultati.DeleteLetoRezultati(ID_letoRezultati);

                #endregion

                //Get seznam LetoRezultati
                List<LetoRezultati> seznam = UpraviteljLetoRezultati.VrniLetoRezultat();
                ViewBag.Data = seznam;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);

            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("DeleteLetoRezultati", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("LetoRezultati");
        }

        #endregion

        #endregion

        #region Rezultati

        #region RezultatiPodrobnosti

        [Authorize]
        public ActionResult RezultatiPodrobnosti(int ID_rezultati, int ID_letoRezultati)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                if (ID_rezultati > 0)
                {
                    //Get rezultat
                    Rezultati rezultat = UpraviteljRezultati.VrniRezultat(ID_rezultati);
                    ViewBag.Data = rezultat;

                    //Obvestilo o uspehu akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                }
                else
                {
                    ViewBag.Data = null;
                    ViewBag.VsebinaPredlog = "Rezultate lahko dobite:";
                    ViewBag.ID_LetoRezultati = ID_letoRezultati;

                    //Obvestilo o dodajanju novice
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.DodajanjeRezultatov);
                }
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("RezultatiPodrobnosti", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region InsertRezultat

        [HttpPost, ValidateInput(false)]
        [Authorize]
        public ActionResult InsertRezultat(string Naslov, string Vsebina, HttpPostedFileBase datoteka, string FileNameRezultat, int ID_LetoRezultati)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                #region Save File

                string PathURLDatoteka = "";
                string fileName = "";

                if (datoteka != null && datoteka.ContentLength > 0)
                {
                    // code for saving the image file to a physical location.
                    fileName = Path.GetFileName(datoteka.FileName);
                    var path = Path.Combine(Server.MapPath(KKKZusternaEnum.SaveRezultati), fileName);
                    datoteka.SaveAs(path);

                    PathURLDatoteka = Url.Content(Path.Combine(KKKZusternaEnum.SaveRezultati, fileName));
                }

                #endregion

                #region Mapping

                Rezultati rezultati = new Rezultati();

                rezultati.ID_rezultati = UpraviteljRezultati.GetNextIdRezultati();
                rezultati.Naslov = Naslov;
                rezultati.Vsebina = Vsebina;
                rezultati.URLFile = PathURLDatoteka;
                rezultati.FileName = string.IsNullOrEmpty(FileNameRezultat) ? fileName : FileNameRezultat;
                rezultati.ID_letoRezultati = ID_LetoRezultati;

                Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                rezultati.Spremenil = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                #endregion

                #region Save

                UpraviteljRezultati.ShraniRezultati(rezultati);

                #endregion

                Rezultati shrRezultati = UpraviteljRezultati.VrniRezultat(rezultati.ID_rezultati);
                ViewBag.Data = shrRezultati;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                MailHelper.SendMailForNewRezultati(trenutniUporabnik.Ime, trenutniUporabnik.Priimek, trenutniUporabnik.Email, shrRezultati.Naslov);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("InsertRezultat", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("RezultatiPodrobnosti");
        }

        #endregion

        #region FindRezultati

        [HttpPost]
        [Authorize]
        public ActionResult FindRezultati(string IDRezultatiFilter, string NaslovFilter, int IDLetoRezultati)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                //Get object LetoRezultat
                LetoRezultati letoRezultati = UpraviteljLetoRezultati.VrniLetoRezultat(IDLetoRezultati);
                ViewBag.Data = letoRezultati;

                //Get seznam rezultatov where Naslov LIKE NaslovFilter
                List<Rezultati> seznamRezultatov = UpraviteljRezultati.VrniRezultatFilter(IDRezultatiFilter, IDLetoRezultati, NaslovFilter);
                ViewBag.SeznamRezultatov = seznamRezultatov;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("FindRezultati", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("LetoRezultatiPodrobnosti");
        }

        #endregion

        #region DeleteRezultat

        [Authorize]
        public ActionResult DeleteRezultat(int ID_rezultat)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                int id_letoRezultati = 0;

                //Get object rezultat
                Rezultati rezultati = UpraviteljRezultati.VrniRezultat(ID_rezultat);
                id_letoRezultati = rezultati.ID_letoRezultati;

                //Delete File in rezultati
                if (rezultati.URLFile != "")
                {
                    // code for deleting the image file 
                    var path = Server.MapPath(rezultati.URLFile);
                    System.IO.File.Delete(path);
                }

                //Delete rezultati from DB
                UpraviteljRezultati.DeleteRezultati(ID_rezultat);

                //Get object LetoRezultat
                LetoRezultati letoRezultati = UpraviteljLetoRezultati.VrniLetoRezultat(id_letoRezultati);
                ViewBag.Data = letoRezultati;

                //Get seznam rezultatov where Naslov LIKE NaslovFilter
                List<Rezultati> seznamRezultatov = UpraviteljRezultati.VrniRezultateZaLeto(letoRezultati.ID_letoRezultati);
                ViewBag.SeznamRezultatov = seznamRezultatov;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("DeleteRezultat", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("LetoRezultatiPodrobnosti");
        }

        #endregion

        #region UpdateRezultati

        [HttpPost, ValidateInput(false)]
        [Authorize]
        public ActionResult UpdateRezultati(string ID_rezultati, string NaslovRezultat, string VsebinaRezultat, HttpPostedFileBase datoteka, string FileNameRezultat)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                Rezultati rezultati = UpraviteljRezultati.VrniRezultat(Convert.ToInt32(ID_rezultati));

                #region Save File

                string PathURLDatoteka = rezultati.URLFile;
                string fileName = "";

                if (datoteka != null && datoteka.ContentLength > 0)
                {
                    //Delete existing File
                    if (rezultati.URLFile != "")
                    {
                        // code for deleting the image file 
                        var pathDel = Server.MapPath(rezultati.URLFile);
                        System.IO.File.Delete(pathDel);
                    }

                    // code for saving the image file to a physical location.
                    fileName = Path.GetFileName(datoteka.FileName);
                    var path = Path.Combine(Server.MapPath(KKKZusternaEnum.SaveRezultati), fileName);
                    datoteka.SaveAs(path);

                    PathURLDatoteka = Url.Content(Path.Combine(KKKZusternaEnum.SaveRezultati, fileName));
                }

                #endregion

                #region Mapping

                rezultati.Naslov = NaslovRezultat;
                rezultati.Vsebina = VsebinaRezultat;
                rezultati.URLFile = PathURLDatoteka;
                rezultati.FileName = string.IsNullOrEmpty(FileNameRezultat) ? fileName : FileNameRezultat;

                Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                rezultati.Spremenil = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                #endregion

                #region Update

                UpraviteljRezultati.UpdateRezultati(rezultati);

                #endregion

                Rezultati updRezultati = UpraviteljRezultati.VrniRezultat(rezultati.ID_rezultati);
                ViewBag.Data = updRezultati;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("UpdateRezultati", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("RezultatiPodrobnosti");
        }

        #endregion

        #endregion

        #endregion

        #region Galerija slik

        #region Galerija Kategorija && Galerija

        #region Prikazi vse galerije slik

        [Authorize]
        public ActionResult UrediGalerijoSlik()
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                List<GalerijaKategorijaGrid> seznam = UpraviteljGalerijaKategorija.VrniKategorijeSlikFiltered("", "");

                ViewBag.Data = seznam;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("UrediGalerijoSlik", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("GalerijaKategorija");
        }

        #endregion

        #region FindGalerijaKategorija

        [Authorize]
        [HttpPost]
        public ActionResult FindGalerijaKategorija(string IDGalerijaKategorijaFilter, string NaslovGalerijaKategorija)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                List<GalerijaKategorijaGrid> seznam = UpraviteljGalerijaKategorija.VrniKategorijeSlikFiltered(IDGalerijaKategorijaFilter, NaslovGalerijaKategorija);

                ViewBag.Data = seznam;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("FindGalerijaKategorija", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("GalerijaKategorija");
        }

        #endregion

        #region GalerijaKategorijaPodrobnosti

        [Authorize]
        public ActionResult GalerijaKategorijaPodrobnosti(int ID_galerijaKategorija)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                if (ID_galerijaKategorija > 0)
                {
                    GalerijaKategorija galerijaKategorija = UpraviteljGalerijaKategorija.VrniKategorijoSlik(ID_galerijaKategorija);
                    ViewBag.Data = galerijaKategorija;

                    List<Galerija> galerija = UpraviteljGalerija.VrniGalerijoZaGalerijaKategorija(ID_galerijaKategorija);
                    ViewBag.Galerija = galerija;

                    //Obvestilo o uspehu akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                }
                else
                {
                    ViewBag.Data = null;

                    //Obvestilo o dodajanju novice
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.DodajanjeGalerijeSlik);
                }
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("GalerijaKategorijaPodrobnosti", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View();
        }

        #endregion

        #region InsertGalerijoSlik

        [HttpPost]
        [Authorize]
        public ActionResult InsertGalerijoSlik(string Naslov, HttpPostedFileBase NaslovnaSlika, HttpPostedFileBase[] Slike)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                string newPath = Server.MapPath(KKKZusternaEnum.SaveGalerijaSlik + Naslov);

                if (!System.IO.Directory.Exists(newPath))
                {
                    #region Create directory

                    System.IO.Directory.CreateDirectory(newPath);

                    #endregion

                    #region Save NaslovnaSlika

                    string PathURLSlika = "";

                    if (NaslovnaSlika != null && NaslovnaSlika.ContentLength > 0)
                    {
                        //resize image
                        Image novaNaslovnaSlika = ImageHelper.ResizeImage(Image.FromStream(NaslovnaSlika.InputStream, true, true), new Size(680, 510));

                        // code for saving the image file to a physical location.
                        var fileName = Path.GetFileName(NaslovnaSlika.FileName);
                        var path = Path.Combine(newPath, fileName);
                        novaNaslovnaSlika.Save(path);
                        //NaslovnaSlika.SaveAs(path);

                        PathURLSlika = Url.Content(Path.Combine(KKKZusternaEnum.SaveGalerijaSlik + Naslov, fileName));
                    }

                    #endregion

                    #region Mapping

                    GalerijaKategorija nova = new GalerijaKategorija();

                    nova.ID_galerijaKategorija = UpraviteljGalerijaKategorija.GetNextIDGalerijaKategorija();
                    nova.Naslov = Naslov;
                    nova.URLSlika = PathURLSlika;

                    Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                    nova.SpremenilUporabnik = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                    #endregion

                    #region Save GalerijaKategorija

                    UpraviteljGalerijaKategorija.InsertGalerijaKategorija(nova);

                    #endregion

                    GalerijaKategorija updateGalerijaKategorija = UpraviteljGalerijaKategorija.VrniKategorijoSlik(nova.ID_galerijaKategorija);
                    ViewBag.Data = updateGalerijaKategorija;

                    #region Insert galerija

                    if (Slike != null && Slike.Count() > 0)
                    {
                        foreach (HttpPostedFileBase slika in Slike)
                        {
                            if (slika != null && !System.IO.File.Exists(newPath + slika.FileName))
                            {
                                #region Save image

                                string pathForImage = "";

                                //resize image
                                Image novaSlika = ImageHelper.ResizeImage(Image.FromStream(slika.InputStream, true, true), new Size(680, 510));

                                // code for saving the image file to a physical location.
                                var fileName = Path.GetFileName(slika.FileName);
                                var path = Path.Combine(newPath, fileName);
                                novaSlika.Save(path);
                                //slika.SaveAs(path);

                                pathForImage = Url.Content(Path.Combine(KKKZusternaEnum.SaveGalerijaSlik + Naslov, fileName));

                                #endregion

                                #region Mapping

                                Galerija galerija = new Galerija();

                                galerija.ID_galerija = UpraviteljGalerija.GetNextIDGalerija();
                                galerija.ID_galerijaKategorija = updateGalerijaKategorija.ID_galerijaKategorija;
                                galerija.URLSlika = pathForImage;

                                #endregion

                                #region Insert

                                UpraviteljGalerija.InsertGalerija(galerija);

                                #endregion   
                            }
                        }
                    }

                    #endregion

                    List<Galerija> galerijaSlik = UpraviteljGalerija.VrniGalerijoZaGalerijaKategorija(updateGalerijaKategorija.ID_galerijaKategorija);
                    ViewBag.Galerija = galerijaSlik;

                    //Obvestilo o uspehu akcije
                    GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
                    MailHelper.SendMailForNewGalerijaSlik(trenutniUporabnik.Ime, trenutniUporabnik.Priimek, trenutniUporabnik.Email, Naslov);
                }
                else
                {
                    GlobalErrors.DodajNapako("Naslov že obstaja.");
                }
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("InsertGalerijoSlik", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("GalerijaKategorijaPodrobnosti");
        }

        #endregion

        #region UpdateGalerijaSlik

        [Authorize]
        [HttpPost]
        public ActionResult UpdateGalerijaSlik(int ID_galerijaKategorija, string Naslov, HttpPostedFileBase NaslovnaSlika, HttpPostedFileBase[] Slike)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                GalerijaKategorija galerijaKategorija = UpraviteljGalerijaKategorija.VrniKategorijoSlik(ID_galerijaKategorija);

                #region Delete/Update NaslovnaSlika

                string PathURLSlika = "";

                if (NaslovnaSlika != null && NaslovnaSlika.ContentLength > 0)
                {
                    if (galerijaKategorija.URLSlika != "")
                    {
                        //First delete current image fot this galerija 
                        var pot = Server.MapPath(galerijaKategorija.URLSlika);
                        System.IO.File.Delete(pot);
                    }

                    //Resize image
                    Image novaNaslovnaSlika = ImageHelper.ResizeImage(Image.FromStream(NaslovnaSlika.InputStream, true, true), new Size(680, 510));

                    // code for saving the image file to a physical location.
                    var fileName = Path.GetFileName(NaslovnaSlika.FileName);
                    var path = Path.Combine(Server.MapPath(KKKZusternaEnum.SaveGalerijaSlik + Naslov), fileName);
                    novaNaslovnaSlika.Save(path);
                    //NaslovnaSlika.SaveAs(path);

                    PathURLSlika = Url.Content(Path.Combine(KKKZusternaEnum.SaveGalerijaSlik + Naslov, fileName));
                }

                #endregion

                #region Mapping

                if (PathURLSlika != "")
                {
                    galerijaKategorija.URLSlika = PathURLSlika;
                }

                Uporabnik trenutniUporabnik = UpraviteljUporabnik.VrniUporabnika(System.Web.HttpContext.Current.User.Identity.Name);
                galerijaKategorija.SpremenilUporabnik = trenutniUporabnik.Ime + " " + trenutniUporabnik.Priimek;

                #endregion

                #region Update Galerija Kategorija

                UpraviteljGalerijaKategorija.UpdateGalerijaKategorija(galerijaKategorija);

                #endregion

                GalerijaKategorija updateGalerijaKategorija = UpraviteljGalerijaKategorija.VrniKategorijoSlik(galerijaKategorija.ID_galerijaKategorija);
                ViewBag.Data = updateGalerijaKategorija;

                #region Update galerija

                if (Slike != null && Slike.Count() > 0)
                {
                    foreach (HttpPostedFileBase slika in Slike)
                    {

                        if(slika != null && !System.IO.File.Exists(Server.MapPath(KKKZusternaEnum.SaveGalerijaSlik + Naslov + slika.FileName)))
                        {
                            #region Save image

                            string pathForImage = "";

                            //Resize image
                            Image novaSlika = ImageHelper.ResizeImage(Image.FromStream(slika.InputStream, true, true), new Size(680, 510));

                            // code for saving the image file to a physical location.
                            var fileName = Path.GetFileName(slika.FileName);
                            var path = Path.Combine(Server.MapPath(KKKZusternaEnum.SaveGalerijaSlik + Naslov), fileName);
                            novaSlika.Save(path);
                            //slika.SaveAs(path);

                            pathForImage = Url.Content(Path.Combine(KKKZusternaEnum.SaveGalerijaSlik + Naslov, fileName));

                            #endregion

                            #region Mapping

                            Galerija galerija = new Galerija();

                            galerija.ID_galerija = UpraviteljGalerija.GetNextIDGalerija();
                            galerija.ID_galerijaKategorija = updateGalerijaKategorija.ID_galerijaKategorija;
                            galerija.URLSlika = pathForImage;

                            #endregion

                            #region Insert

                            UpraviteljGalerija.InsertGalerija(galerija);

                            #endregion
                        }
                    }
                }

                #endregion

                List<Galerija> galerijaSlik = UpraviteljGalerija.VrniGalerijoZaGalerijaKategorija(updateGalerijaKategorija.ID_galerijaKategorija);
                ViewBag.Galerija = galerijaSlik;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("UpdateGalerijaSlik", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("GalerijaKategorijaPodrobnosti");
        }

        #endregion

        #region DeleteGalerijaKategorijaAndGalerija

        [Authorize]
        public ActionResult DeleteGalerijaSlik(int ID_galerijaKategorija)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                GalerijaKategorija galerijaKategorija = UpraviteljGalerijaKategorija.VrniKategorijoSlik(ID_galerijaKategorija);

                #region Delete Directory

                string newPath = Server.MapPath(KKKZusternaEnum.SaveGalerijaSlik + galerijaKategorija.Naslov);

                if (System.IO.Directory.Exists(newPath))
                {
                    System.IO.Directory.Delete(newPath, true);   
                }

                #endregion

                #region Delete GalerijaKategorija

                UpraviteljGalerijaKategorija.DeleteGalerijaKategorija(ID_galerijaKategorija);

                #endregion

                #region Delete Galerija

                UpraviteljGalerija.DeleteGalerija(galerijaKategorija.ID_galerijaKategorija);

                #endregion

                List<GalerijaKategorijaGrid> seznam = UpraviteljGalerijaKategorija.VrniKategorijeSlikFiltered("", "");

                ViewBag.Data = seznam;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("DeleteGalerijaSlik", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("GalerijaKategorija");
        }

        #endregion

        #region DeleteGalerija

        [Authorize]
        public ActionResult DeleteGalerija(int ID_galerijaKategorija)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                #region Delete images from file system

                GalerijaKategorija kategorija = UpraviteljGalerijaKategorija.VrniKategorijoSlik(ID_galerijaKategorija);
                List<Galerija> galerija = UpraviteljGalerija.VrniGalerijoZaGalerijaKategorija(ID_galerijaKategorija);

                foreach (Galerija slika in galerija)
                {
                    if (System.IO.File.Exists(Server.MapPath(slika.URLSlika)))
                    {
                        if (!slika.URLSlika.Equals(kategorija.URLSlika))
                        {
                            System.IO.File.Delete(Server.MapPath(slika.URLSlika));   
                        }
                    }
                }

                #endregion

                #region Delete Galerija

                UpraviteljGalerija.DeleteGalerija(ID_galerijaKategorija);

                #endregion

                GalerijaKategorija galerijaKategorija = UpraviteljGalerijaKategorija.VrniKategorijoSlik(ID_galerijaKategorija);
                ViewBag.Data = galerijaKategorija;

                List<Galerija> galerijaSlik = UpraviteljGalerija.VrniGalerijoZaGalerijaKategorija(ID_galerijaKategorija);
                ViewBag.Galerija = galerijaSlik;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("DeleteGalerija", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("GalerijaKategorijaPodrobnosti");
        }

        #endregion

        #region DeleteSelectedImageFromGalerija

        [Authorize]
        public ActionResult DeleteSelectedImageFromGalerija(int ID_galerija)
        {
            try
            {
                //Zbrisemo obvestila && napake
                GlobalNotifications.ZbrisiObvestilo();
                GlobalErrors.ZbrisiNapake();
                GlobalWarnings.ZbrisiOpozorilo();

                Galerija galerija = UpraviteljGalerija.VrniGalerijo(ID_galerija);

                //Tmp value
                int ID_galerijaKategorija = galerija.ID_galerijaKategorija;

                #region Delete image from FileSystem

                if (System.IO.File.Exists(Server.MapPath(galerija.URLSlika)))
                {
                    System.IO.File.Delete(Server.MapPath(galerija.URLSlika));
                }

                #endregion

                #region Delete galerija

                UpraviteljGalerija.DeleteRowGalerija(ID_galerija);

                #endregion

                GalerijaKategorija galerijaKategorija = UpraviteljGalerijaKategorija.VrniKategorijoSlik(ID_galerijaKategorija);
                ViewBag.Data = galerijaKategorija;

                List<Galerija> galerijaSlik = UpraviteljGalerija.VrniGalerijoZaGalerijaKategorija(ID_galerijaKategorija);
                ViewBag.Galerija = galerijaSlik;

                //Obvestilo o uspehu akcije
                GlobalNotifications.DodajObvestilo(GlobalNotifications.UspehOperacije);
            }
            catch (Exception ex)
            {
                GlobalErrors.ZbrisiNapake();
                GlobalErrors.DodajNapako(ex.ToString());
                MailHelper.SendMailForErrors("DeleteSelectedImageFromGalerija", ex.ToString());
				logger.Error("ERROR in method " + MethodInfo.GetCurrentMethod() + ": " + ex);
            }

            return View("GalerijaKategorijaPodrobnosti");
        }

        #endregion

        #endregion

        #endregion

    }
}
