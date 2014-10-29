using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using KKK_Zusterna.Models;

namespace KKK_Zusterna.Helper
{
    public static class MailHelper
    {
        #region Properties

        private static readonly string email = System.Configuration.ConfigurationManager.AppSettings["EmailProvider"];
        private static readonly string password = System.Configuration.ConfigurationManager.AppSettings["GesloProvider"];
        private static UporabnikPPP UpraviteljUporabnik = new UporabnikPPP();

        #endregion

        #region Functionality

        public static void SendMailForUserLogin(string Email)
        {
            bool poslji = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PosljiMailForUserLogin"]);

            if (poslji)
            {
                List<UporabnikGrid> uporabniki = UpraviteljUporabnik.VrniUporabnike();

                foreach (UporabnikGrid uporabnikGrid in uporabniki)
                {
                    if (uporabnikGrid.Administrator == 1 && uporabnikGrid.FlagPrijava == 1)
                    {
                        var loginInfo = new NetworkCredential(email, password);
                        var msg = new MailMessage();
                        var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                        msg.From = new MailAddress(email);
                        msg.To.Add(new MailAddress(uporabnikGrid.Email));
                        msg.Subject = "www.kkkzusterna.si - Prijava uporabnika";
                        msg.Body = "Na spletno stran www.kkkzusterna.si se je prijavil uporabnik: " + Email;
                        msg.IsBodyHtml = true;

                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = loginInfo;
                        smtpClient.Send(msg);
                    }
                }
            }
        }

        public static void SendMailToUserForSuccessfulRegistration(string Email, string geslo)
        {
            bool poslji = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PosljiMailToUserForSuccessfulRegistration"]);

            if (poslji)
            {
                var loginInfo = new NetworkCredential(email, password);
                var msg = new MailMessage();
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                msg.From = new MailAddress(email);
                msg.To.Add(new MailAddress(Email));
                msg.Subject = "www.kkkzusterna.si - Registracija uporabnika";
                msg.Body = "Obveščamo vas, da ste uspešno registriran uporabnik spletne strani www.kkkzusterna.si.<br /><br /><b>Vaše uporabniško ime:</b> " + 
                    Email + "<br/><b>Vaše geslo:</b> " + geslo + "<br /><br /><b>Obvestilo!</b><br />Svetujemo vam, da si ob prvi prijavi v spletno stran www.kkkzusterna.si spremenite geslo." +
                    "<br /><br />Lep pozdrav,<br />Ekipa www.kkkzusterna.si";
                msg.IsBodyHtml = true;

                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);
            }
        }

        public static void SendMailForErrors(string nazivMetode, string napaka)
        {
            bool poslji = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PosljiMailForSystemErrors"]);

            if (poslji)
            {
                List<UporabnikGrid> uporabniki = UpraviteljUporabnik.VrniUporabnike();

                foreach (UporabnikGrid uporabnikGrid in uporabniki)
                {
                    if (uporabnikGrid.Administrator == 1 && uporabnikGrid.FlagNapaka == 1)
                    {
                        var loginInfo = new NetworkCredential(email, password);
                        var msg = new MailMessage();
                        var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                        msg.From = new MailAddress(email);
                        msg.To.Add(new MailAddress(uporabnikGrid.Email));
                        msg.Subject = "www.kkkzusterna.si - Napaka";
                        msg.Body = "<b>Prišlo je do NAPAKE! <br /><br /></b> <b>Metoda naziv:</b> " + nazivMetode + "<br /><b>Napaka: </b>" + napaka;
                        msg.IsBodyHtml = true;

                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = loginInfo;
                        smtpClient.Send(msg);
                    }
                }
            }
        }

        public static void SendMailFromKontaktForm(string Ime, string Priimek, string Email, string Vsebina)
        {
            bool poslji = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PosljiMailFromKontakt"]);

            if (poslji)
            {
                List<UporabnikGrid> uporabniki = UpraviteljUporabnik.VrniUporabnike();

                foreach (UporabnikGrid uporabnikGrid in uporabniki)
                {
                    if (uporabnikGrid.Administrator == 1 && uporabnikGrid.FlagKontakt == 1)
                    {
                        var loginInfo = new NetworkCredential(email, password);
                        var msg = new MailMessage();
                        var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                        msg.From = new MailAddress(email);
                        msg.To.Add(new MailAddress(uporabnikGrid.Email));
                        msg.Subject = "www.kkkzusterna.si - Kontakt";
                        msg.Body = "<b>Uporabnik ime:</b> " + Ime + "<br /><br />" + "<b>Uporabnik priimek:</b> " +
                                   Priimek + "<br /><br />" + "<b>Uporabnik email:</b> " + Email + "<br /><br />" +
                                   "<b>Uporabnik vsebina:</b><br /> " + Vsebina;
                        msg.IsBodyHtml = true;

                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = loginInfo;
                        smtpClient.Send(msg);
                    }
                }
            }
        }

        public static void SendMailForUserRegistration(Uporabnik upr)
        {
            bool poslji = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PosljiMailFromRegistracija"]);

            if (poslji)
            {
                List<UporabnikGrid> uporabniki = UpraviteljUporabnik.VrniUporabnike();

                foreach (UporabnikGrid uporabnikGrid in uporabniki)
                {
                    if (uporabnikGrid.Administrator == 1 && uporabnikGrid.FlagRegistracija == 1)
                    {
                        var loginInfo = new NetworkCredential(email, password);
                        var msg = new MailMessage();
                        var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                        msg.From = new MailAddress(email);
                        msg.To.Add(new MailAddress(uporabnikGrid.Email));
                        msg.Subject = "www.kkkzusterna.si - Registracija uporabnika";
                        msg.Body = "Registracija uporabnika.<br /><br /><b>Uporabnik ime:</b> " + upr.Ime + "<br /><br />" + "<b>Uporabnik priimek:</b> " + upr.Priimek + "<br /><br />" + "<b>Uporabnik email:</b> " + upr.Email + "<br /><br />" + "<b>Uporabnik geslo:</b> " + upr.Geslo;
                        msg.IsBodyHtml = true;

                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = loginInfo;
                        smtpClient.Send(msg);
                    }
                }
            }
        }

        public static void SendMailForInvalidAuthentication(string Ime, string Priimek, string Email, string Vsebina)
        {
            bool poslji = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PosljiMailForInvalidAuthentication"]);

            if (poslji)
            {
                List<UporabnikGrid> uporabniki = UpraviteljUporabnik.VrniUporabnike();

                foreach (UporabnikGrid uporabnikGrid in uporabniki)
                {
                    if (uporabnikGrid.Administrator == 1 && uporabnikGrid.FlagNapakaAvtorizacija == 1)
                    {
                        var loginInfo = new NetworkCredential(email, password);
                        var msg = new MailMessage();
                        var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                        msg.From = new MailAddress(email);
                        msg.To.Add(new MailAddress(uporabnikGrid.Email));
                        msg.Subject = "www.kkkzusterna.si - Avtentikacija uporabnika";
                        msg.Body = "Napaka pri avtentikaciji uporabnika.<br /><br /><b>Uporabnik ime:</b> " + Ime + "<br /><br />" + "<b>Uporabnik priimek:</b> " + Priimek + "<br /><br />" + "<b>Uporabnik email:</b> " + Email + "<br /><br />" + "<b>Uporabnik vsebina:</b><br /> " + Vsebina;
                        msg.IsBodyHtml = true;

                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = loginInfo;
                        smtpClient.Send(msg);
                    }
                }
            }
        }

        public static void SendMailForNewONas(string ime, string priimek, string Email)
        {
            bool poslji = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PosljiMailForNewONas"]);

            if (poslji)
            {
                List<UporabnikGrid> uporabniki = UpraviteljUporabnik.VrniUporabnike();

                foreach (UporabnikGrid uporabnikGrid in uporabniki)
                {
                    if (uporabnikGrid.Administrator == 1 && uporabnikGrid.FlagONas == 1)
                    {
                        var loginInfo = new NetworkCredential(email, password);
                        var msg = new MailMessage();
                        var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                        msg.From = new MailAddress(email);
                        msg.To.Add(new MailAddress(uporabnikGrid.Email));
                        msg.Subject = "www.kkkzusterna.si - O nas";
                        msg.Body = "Segment O nas je bil spremenjen. <br /><br /><b>Spremenil:</b> " + ime + " " + priimek + "<br /><b>Email:</b> " + Email;
                        msg.IsBodyHtml = true;

                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = loginInfo;
                        smtpClient.Send(msg);
                    }
                }
            }
        }

        public static void SendMailForNewKontakt(string ime, string priimek, string Email)
        {
            bool poslji = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PosljiMailForNewKontakt"]);

            if (poslji)
            {
                List<UporabnikGrid> uporabniki = UpraviteljUporabnik.VrniUporabnike();

                foreach (UporabnikGrid uporabnikGrid in uporabniki)
                {
                    if (uporabnikGrid.Administrator == 1 && uporabnikGrid.FlagKontakt == 1)
                    {
                        var loginInfo = new NetworkCredential(email, password);
                        var msg = new MailMessage();
                        var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                        msg.From = new MailAddress(email);
                        msg.To.Add(new MailAddress(uporabnikGrid.Email));
                        msg.Subject = "www.kkkzusterna.si - Kontaktne informacije";
                        msg.Body = "Segment Kontaktne informacije je bil spremenjen. <br /><br /><b>Spremenil:</b> " + ime + " " + priimek + "<br /><b>Email:</b> " + Email;
                        msg.IsBodyHtml = true;

                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = loginInfo;
                        smtpClient.Send(msg);
                    }
                }
            }
        }

        public static void SendMailForNewPovezave(string ime, string priimek, string Email, string naslov)
        {
            bool poslji = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PosljiMailForNewPovezave"]);

            if (poslji)
            {
                List<UporabnikGrid> uporabniki = UpraviteljUporabnik.VrniUporabnike();

                foreach (UporabnikGrid uporabnikGrid in uporabniki)
                {
                    if (uporabnikGrid.Administrator == 1 && uporabnikGrid.FlagPovezave == 1)
                    {
                        var loginInfo = new NetworkCredential(email, password);
                        var msg = new MailMessage();
                        var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                        msg.From = new MailAddress(email);
                        msg.To.Add(new MailAddress(uporabnikGrid.Email));
                        msg.Subject = "www.kkkzusterna.si - Povezave";
                        msg.Body = "Dodana je bila nova povezava.<br /><br /><b>Naslov:</b> " + naslov + "<br /><b>Spremenil:</b> " + ime + " " + priimek + "<br /><b>Email:</b> " + Email;
                        msg.IsBodyHtml = true;

                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = loginInfo;
                        smtpClient.Send(msg);
                    }
                }
            }
        }

        public static void SendMailForNewTreningi(string ime, string priimek, string Email, string naziv)
        {
            bool poslji = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PosljiMailForNewTreningi"]);

            if (poslji)
            {
                List<UporabnikGrid> uporabniki = UpraviteljUporabnik.VrniUporabnike();

                foreach (UporabnikGrid uporabnikGrid in uporabniki)
                {
                    if (uporabnikGrid.Administrator == 1 && uporabnikGrid.FlagTreningi == 1)
                    {
                        var loginInfo = new NetworkCredential(email, password);
                        var msg = new MailMessage();
                        var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                        msg.From = new MailAddress(email);
                        msg.To.Add(new MailAddress(uporabnikGrid.Email));
                        msg.Subject = "www.kkkzusterna.si - Treningi";
                        msg.Body = "Dodan je bil nov trening.<br /><br /><b>Naziv:</b>" + naziv + "<br /><b>Spremenil:</b> " + ime + " " + priimek + "<br /<br /><b>Email:</b> " + Email;
                        msg.IsBodyHtml = true;

                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = loginInfo;
                        smtpClient.Send(msg);
                    }
                }
            }
        }

        public static void SendMailForNewTekmovanja(string ime, string priimek, string Email, string naslov)
        {
            bool poslji = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PosljiMailForNewTekmovanja"]);

            if (poslji)
            {
                List<UporabnikGrid> uporabniki = UpraviteljUporabnik.VrniUporabnike();

                foreach (UporabnikGrid uporabnikGrid in uporabniki)
                {
                    if (uporabnikGrid.Administrator == 1 && uporabnikGrid.FlagTekmovanja == 1)
                    {
                        var loginInfo = new NetworkCredential(email, password);
                        var msg = new MailMessage();
                        var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                        msg.From = new MailAddress(email);
                        msg.To.Add(new MailAddress(uporabnikGrid.Email));
                        msg.Subject = "www.kkkzusterna.si - Tekmovanja";
                        msg.Body = "Dodano je bilo novo tekmovanje.<br /><br /><b>Naslov:</b> " + naslov + "<br /><br /><b>Spremenil:</b> " + ime + " " + priimek + "<br /><b>Email:</b> " + Email;
                        msg.IsBodyHtml = true;

                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = loginInfo;
                        smtpClient.Send(msg);
                    }
                }
            }
        }

        public static void SendMailForNewRezultati(string ime, string priimek, string Email, string naslov)
        {
            bool poslji = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PosljiMailForNewRezultati"]);

            if (poslji)
            {
                List<UporabnikGrid> uporabniki = UpraviteljUporabnik.VrniUporabnike();

                foreach (UporabnikGrid uporabnikGrid in uporabniki)
                {
                    if (uporabnikGrid.Administrator == 1 && uporabnikGrid.FlagRezultati == 1)
                    {
                        var loginInfo = new NetworkCredential(email, password);
                        var msg = new MailMessage();
                        var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                        msg.From = new MailAddress(email);
                        msg.To.Add(new MailAddress(uporabnikGrid.Email));
                        msg.Subject = "www.kkkzusterna.si - Rezultati";
                        msg.Body = "Dodan je bil novi rezultat.<br /><br /><b>Naslov:</b> " + naslov + "<br /><br /><b>Spremenil:</b> " + ime + " " + priimek + "<br /><b>Email:</b> " + Email;
                        msg.IsBodyHtml = true;

                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = loginInfo;
                        smtpClient.Send(msg);
                    }
                }
            }
        }

        public static void SendMailForNewGalerijaSlik(string ime, string priimek, string Email, string naslov)
        {
            bool poslji = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PosljiMailForNewGalerijaSlik"]);

            if (poslji)
            {
                List<UporabnikGrid> uporabniki = UpraviteljUporabnik.VrniUporabnike();

                foreach (UporabnikGrid uporabnikGrid in uporabniki)
                {
                    if (uporabnikGrid.Administrator == 1 && uporabnikGrid.FlagGalerijaSlik == 1)
                    {
                        var loginInfo = new NetworkCredential(email, password);
                        var msg = new MailMessage();
                        var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                        msg.From = new MailAddress(email);
                        msg.To.Add(new MailAddress(uporabnikGrid.Email));
                        msg.Subject = "www.kkkzusterna.si - Galerija slik";
                        msg.Body = "Dodana je bila nova galerija slik.<br /><br /> <b>Naslov:</b> " + naslov + "<br /><br /><b>Spremenil:</b> " + ime + " " + priimek + "<br /><b>Email:</b> " + Email; ;
                        msg.IsBodyHtml = true;

                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = loginInfo;
                        smtpClient.Send(msg);
                    }
                }
            }
        }

        #endregion
    }
}