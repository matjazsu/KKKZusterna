using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using KKK_Zusterna.Helper;

namespace KKK_Zusterna.Models
{
    public class UporabnikPPP
    {
        #region ConnectionString

        private string ConnString = System.Configuration.ConfigurationManager.ConnectionStrings["KKKZusternaDB"].ConnectionString;

        #endregion

        #region Functionality

        public void PosodobiUporabnika(Uporabnik upr)
        {
            string query = @"UPDATE Uporabnik 
                             SET Ime = '" + upr.Ime + "', Priimek = '" + upr.Priimek + "', Email = '" +
                                upr.Email + "', Spremenil = '" + upr.Spremenil + "', SpremenilDatum = DATETIME('now'), Administrator = '" + upr.Administrator + "'" 
                                + SetFlagsForUpdate(upr) + " WHERE ID_uporabnik = '" + upr.ID_uporabnik + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void PosodobiUporabnikGeslo(Uporabnik upr)
        {
            string query = @"UPDATE Uporabnik 
                             SET Geslo = '" + upr.Geslo + "', Spremenil = '" + 
                                            upr.Spremenil + "', SpremenilDatum = DATETIME('now') WHERE ID_uporabnik = '" + upr.ID_uporabnik + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void ShraniUporabnika(Uporabnik upr)
        {
            string query = @"INSERT INTO Uporabnik 
                             VALUES ('" + upr.ID_uporabnik + "', '" + upr.Ime + "', '" + upr.Priimek + "', '" +
                                upr.Email + "', '" + upr.Spremenil + "', DATETIME('now'), '" + upr.Geslo + 
                                "', '" + upr.Administrator + "'" + SetFlagsForInsert(upr) + ") ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public int GetNextIdUporabnik()
        {
            int nextIdUporabnik = 0;

            string query = @"SELECT MAX(ID_uporabnik) FROM Uporabnik";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            try
            {
                nextIdUporabnik = (Convert.ToInt32(cmd.ExecuteScalar())) + 1;
            }
            catch (Exception)
            {
                nextIdUporabnik = 1;
            }

            conn.Dispose();

            return nextIdUporabnik;
        }

        public Uporabnik VrniUporabnika(string email, string geslo)
        {
            Uporabnik upr = null;

            string query = "SELECT * FROM Uporabnik WHERE Email = '" + email + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            SQLiteDataReader reader = cmd.ExecuteReader();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            ds.Tables.Add(dt);
            ds.EnforceConstraints = false;
            dt.Load(reader);

            conn.Dispose();

            if (dt != null && dt.Rows.Count > 0)
            {
                #region Compute MD5 hash

                MD5 md5 = new MD5CryptoServiceProvider();

                md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(geslo));

                byte[] enterPasswordBytes = md5.Hash;

                string enterPassword = Convert.ToBase64String(enterPasswordBytes);

                #endregion

                if (enterPassword.Equals(Convert.ToString(dt.Rows[0]["Geslo"])))
                {
                    upr = new Uporabnik();

                    upr.ID_uporabnik = (int)dt.Rows[0]["ID_uporabnik"];
                    upr.Ime = dt.Rows[0]["Ime"] != null ? (string)dt.Rows[0]["Ime"] : "";
                    upr.Priimek = dt.Rows[0]["Priimek"] != null ? (string)dt.Rows[0]["Priimek"] : "";
                    upr.Email = dt.Rows[0]["Email"] != null ? (string)dt.Rows[0]["Email"] : "";
                    upr.Spremenil = dt.Rows[0]["Spremenil"] != null ? (string)dt.Rows[0]["Spremenil"] : "";
                    upr.SpremenilDatum = dt.Rows[0]["SpremenilDatum"] != null ? (DateTime)dt.Rows[0]["SpremenilDatum"] : System.DateTime.Now;
                    upr.Geslo = dt.Rows[0]["Geslo"] != null ? (string)dt.Rows[0]["Geslo"] : "";
                    upr.Administrator = dt.Rows[0]["Administrator"] != null ? Convert.ToInt32(dt.Rows[0]["Administrator"]) : 0;   
                }
            }

            return upr;
        }

        public Uporabnik VrniUporabnika(int id_uporabnik)
        {
            Uporabnik upr = null;

            string query = "SELECT * FROM Uporabnik WHERE ID_uporabnik = '" + id_uporabnik + "'";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            SQLiteDataReader reader = cmd.ExecuteReader();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            ds.Tables.Add(dt);
            ds.EnforceConstraints = false;
            dt.Load(reader);

            conn.Dispose();

            if (dt != null && dt.Rows.Count > 0)
            {
                upr = new Uporabnik();

                upr.ID_uporabnik = (int)dt.Rows[0]["ID_uporabnik"];
                upr.Ime = dt.Rows[0]["Ime"] != null ? (string)dt.Rows[0]["Ime"] : "";
                upr.Priimek = dt.Rows[0]["Priimek"] != null ? (string)dt.Rows[0]["Priimek"] : "";
                upr.Email = dt.Rows[0]["Email"] != null ? (string)dt.Rows[0]["Email"] : "";
                upr.Spremenil = dt.Rows[0]["Spremenil"] != null ? (string)dt.Rows[0]["Spremenil"] : "";
                upr.SpremenilDatum = dt.Rows[0]["SpremenilDatum"] != null ? (DateTime)dt.Rows[0]["SpremenilDatum"] : System.DateTime.Now;
                upr.Geslo = dt.Rows[0]["Geslo"] != null ? (string)dt.Rows[0]["Geslo"] : "";
                upr.Administrator = dt.Rows[0]["Administrator"] != null ? Convert.ToInt32(dt.Rows[0]["Administrator"]) : 0;
                upr.FlagPrijava = dt.Rows[0]["FlagPrijava"] != null ? Convert.ToInt32(dt.Rows[0]["FlagPrijava"]) : 0; ;
                upr.FlagNapaka = dt.Rows[0]["FlagNapaka"] != null ? Convert.ToInt32(dt.Rows[0]["FlagNapaka"]) : 0; ;
                upr.FlagKontakt = dt.Rows[0]["FlagKontakt"] != null ? Convert.ToInt32(dt.Rows[0]["FlagKontakt"]) : 0; ;
                upr.FlagRegistracija = dt.Rows[0]["FlagRegistracija"] != null ? Convert.ToInt32(dt.Rows[0]["FlagRegistracija"]) : 0; ;
                upr.FlagNapakaAvtorizacija = dt.Rows[0]["FlagNapakaAvtorizacija"] != null ? Convert.ToInt32(dt.Rows[0]["FlagNapakaAvtorizacija"]) : 0; ;
                upr.FlagONas = dt.Rows[0]["FlagONas"] != null ? Convert.ToInt32(dt.Rows[0]["FlagONas"]) : 0; ;
                upr.FlagPovezave = dt.Rows[0]["FlagPovezave"] != null ? Convert.ToInt32(dt.Rows[0]["FlagPovezave"]) : 0; ;
                upr.FlagTreningi = dt.Rows[0]["FlagTreningi"] != null ? Convert.ToInt32(dt.Rows[0]["FlagTreningi"]) : 0; ;
                upr.FlagTekmovanja = dt.Rows[0]["FlagTekmovanja"] != null ? Convert.ToInt32(dt.Rows[0]["FlagTekmovanja"]) : 0; ;
                upr.FlagRezultati = dt.Rows[0]["FlagRezultati"] != null ? Convert.ToInt32(dt.Rows[0]["FlagRezultati"]) : 0; ;
                upr.FlagGalerijaSlik = dt.Rows[0]["FlagGalerijaSlik"] != null ? Convert.ToInt32(dt.Rows[0]["FlagGalerijaSlik"]) : 0; ;
            }

            return upr;
        }

        public Uporabnik VrniUporabnika(string email)
        {
            Uporabnik upr = null;

            string query = "SELECT * FROM Uporabnik WHERE Email = '" + email + "'";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            SQLiteDataReader reader = cmd.ExecuteReader();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            ds.Tables.Add(dt);
            ds.EnforceConstraints = false;
            dt.Load(reader);

            conn.Dispose();

            if (dt != null && dt.Rows.Count > 0)
            {
                upr = new Uporabnik();

                upr.ID_uporabnik = (int)dt.Rows[0]["ID_uporabnik"];
                upr.Ime = dt.Rows[0]["Ime"] != null ? (string)dt.Rows[0]["Ime"] : "";
                upr.Priimek = dt.Rows[0]["Priimek"] != null ? (string)dt.Rows[0]["Priimek"] : "";
                upr.Email = dt.Rows[0]["Email"] != null ? (string)dt.Rows[0]["Email"] : "";
                upr.Spremenil = dt.Rows[0]["Spremenil"] != null ? (string)dt.Rows[0]["Spremenil"] : "";
                upr.SpremenilDatum = dt.Rows[0]["SpremenilDatum"] != null ? (DateTime)dt.Rows[0]["SpremenilDatum"] : System.DateTime.Now;
                upr.Geslo = dt.Rows[0]["Geslo"] != null ? (string)dt.Rows[0]["Geslo"] : "";
                upr.Administrator = dt.Rows[0]["Administrator"] != null ? Convert.ToInt32(dt.Rows[0]["Administrator"]) : 0;
                upr.FlagPrijava = dt.Rows[0]["FlagPrijava"] != null ? Convert.ToInt32(dt.Rows[0]["FlagPrijava"]) : 0; ;
                upr.FlagNapaka = dt.Rows[0]["FlagNapaka"] != null ? Convert.ToInt32(dt.Rows[0]["FlagNapaka"]) : 0; ;
                upr.FlagKontakt = dt.Rows[0]["FlagKontakt"] != null ? Convert.ToInt32(dt.Rows[0]["FlagKontakt"]) : 0; ;
                upr.FlagRegistracija = dt.Rows[0]["FlagRegistracija"] != null ? Convert.ToInt32(dt.Rows[0]["FlagRegistracija"]) : 0; ;
                upr.FlagNapakaAvtorizacija = dt.Rows[0]["FlagNapakaAvtorizacija"] != null ? Convert.ToInt32(dt.Rows[0]["FlagNapakaAvtorizacija"]) : 0; ;
                upr.FlagONas = dt.Rows[0]["FlagONas"] != null ? Convert.ToInt32(dt.Rows[0]["FlagONas"]) : 0; ;
                upr.FlagPovezave = dt.Rows[0]["FlagPovezave"] != null ? Convert.ToInt32(dt.Rows[0]["FlagPovezave"]) : 0; ;
                upr.FlagTreningi = dt.Rows[0]["FlagTreningi"] != null ? Convert.ToInt32(dt.Rows[0]["FlagTreningi"]) : 0; ;
                upr.FlagTekmovanja = dt.Rows[0]["FlagTekmovanja"] != null ? Convert.ToInt32(dt.Rows[0]["FlagTekmovanja"]) : 0; ;
                upr.FlagRezultati = dt.Rows[0]["FlagRezultati"] != null ? Convert.ToInt32(dt.Rows[0]["FlagRezultati"]) : 0; ;
                upr.FlagGalerijaSlik = dt.Rows[0]["FlagGalerijaSlik"] != null ? Convert.ToInt32(dt.Rows[0]["FlagGalerijaSlik"]) : 0; ;
            }

            return upr;
        }

        public List<UporabnikGrid> VrniUporabnike()
        {
            List<UporabnikGrid> seznamUporabnikov = new List<UporabnikGrid>();

            string query = "SELECT * FROM Uporabnik";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            SQLiteDataReader reader = cmd.ExecuteReader();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            ds.Tables.Add(dt);
            ds.EnforceConstraints = false;
            dt.Load(reader);

            conn.Dispose();

            //Pretvorimo dt v List<Novica>
            if (ds != null)
            {
                DataTableToList<UporabnikGrid> dtl = new DataTableToList<UporabnikGrid>();
                seznamUporabnikov = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return seznamUporabnikov;
        }

        public void DeleteUporabnik(int id_uporabnik)
        {
            string query = @"DELETE FROM Uporabnik WHERE ID_uporabnik = '" + id_uporabnik + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public List<UporabnikGrid> VrniFilteredUporabnike(string id_uporabnik, string ime, string priimek, string email, string DatumSpremenilOd, string DatumSpremenilDo)
        {
            List<UporabnikGrid> seznamUporabnikov = new List<UporabnikGrid>();

            string query = "";

            #region Create query

            if (id_uporabnik != "")
            {
                query = "SELECT * FROM Uporabnik WHERE ID_uporabnik = '" + id_uporabnik + "'";
            }
            else if (DatumSpremenilOd != "" || DatumSpremenilDo != "")
            {
                if (DatumSpremenilOd != "" && DatumSpremenilDo == "")
                {
                    query = "SELECT * FROM Uporabnik WHERE Ime LIKE '%" + ime + "%' AND Priimek LIKE '%" + priimek + "%' AND Email LIKE '%" + email + "%' AND SpremenilDatum > '" + DatumSpremenilOd + "' ";   
                }
                else if (DatumSpremenilOd == "" && DatumSpremenilDo != "")
                {
                    query = "SELECT * FROM Uporabnik WHERE Ime LIKE '%" + ime + "%' AND Priimek LIKE '%" + priimek + "%' AND Email LIKE '%" + email + "%' AND SpremenilDatum < '" + DatumSpremenilDo + "' ";
                }
                else
                {
                    query = "SELECT * FROM Uporabnik WHERE Ime LIKE '%" + ime + "%' AND Priimek LIKE '%" + priimek + "%' AND Email LIKE '%" + email + "%' AND SpremenilDatum > '" + DatumSpremenilOd + "' AND SpremenilDatum < '" + DatumSpremenilDo + "' ";
                }
            }    
            else
            {
                query = "SELECT * FROM Uporabnik WHERE Ime LIKE '%" + ime + "%' AND Priimek LIKE '%" + priimek + "%' AND Email LIKE '%" + email + "%' ";
            }

            #endregion

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            SQLiteDataReader reader = cmd.ExecuteReader();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            ds.Tables.Add(dt);
            ds.EnforceConstraints = false;
            dt.Load(reader);

            conn.Dispose();

            //Pretvorimo dt v List<Novica>
            if (ds != null)
            {
                DataTableToList<UporabnikGrid> dtl = new DataTableToList<UporabnikGrid>();
                seznamUporabnikov = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return seznamUporabnikov;
        }

        #region private methods

        private string SetFlagsForUpdate(Uporabnik upr)
        {
            string res = ", FlagPrijava = '" + upr.FlagPrijava + "'";
            res += ", FlagNapaka = '" + upr.FlagNapaka + "'";
            res += ", FlagKontakt = '" + upr.FlagKontakt + "'";
            res += ", FlagRegistracija = '" + upr.FlagRegistracija + "'";
            res += ", FlagNapakaAvtorizacija = '" + upr.FlagNapakaAvtorizacija + "'";
            res += ", FlagNapakaAvtorizacija = '" + upr.FlagNapakaAvtorizacija + "'";
            res += ", FlagONas = '" + upr.FlagONas + "'";
            res += ", FlagPovezave = '" + upr.FlagPovezave + "'";
            res += ", FlagTreningi = '" + upr.FlagTreningi + "'";
            res += ", FlagTekmovanja = '" + upr.FlagTekmovanja + "'";
            res += ", FlagRezultati = '" + upr.FlagRezultati + "'";
            res += ", FlagGalerijaSlik = '" + upr.FlagGalerijaSlik + "'";

            return res;
        }

        private string SetFlagsForInsert(Uporabnik upr)
        {
            string res = ", '" + upr.FlagPrijava + "'";
            res += ", '" + upr.FlagNapaka + "'";
            res += ", '" + upr.FlagKontakt + "'";
            res += ", '" + upr.FlagRegistracija + "'";
            res += ", '" + upr.FlagNapakaAvtorizacija + "'";
            res += ", '" + upr.FlagONas + "'";
            res += ", '" + upr.FlagPovezave + "'";
            res += ", '" + upr.FlagTreningi + "'";
            res += ", '" + upr.FlagTekmovanja + "'";
            res += ", '" + upr.FlagRezultati + "'";
            res += ", '" + upr.FlagGalerijaSlik + "'";

            return res;
        }

        #endregion

        #endregion
    }
}