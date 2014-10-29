using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace KKK_Zusterna.Models
{
    public class KontaktPPP
    {
        #region ConnectionString

        private string ConnString = System.Configuration.ConfigurationManager.ConnectionStrings["KKKZusternaDB"].ConnectionString;

        #endregion

        #region Functionality

        public Kontakt VrniKontakt()
        {
            Kontakt kontakt = null;

            string query = "SELECT * FROM Kontakt";

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

            if (dt.Rows != null && dt.Rows.Count > 0)
            {
                kontakt = new Kontakt();

                //Mapping
                kontakt.ID_Kontakt = (int)dt.Rows[0]["ID_Kontakt"];
                kontakt.Spremenil = dt.Rows[0]["Spremenil"] != null ? (string)dt.Rows[0]["Spremenil"] : "";
                kontakt.SpremenilDatum = dt.Rows[0]["SpremenilDatum"] != null ? (DateTime)dt.Rows[0]["SpremenilDatum"] : System.DateTime.Now;
                kontakt.Email = dt.Rows[0]["Email"] != null ? (string)dt.Rows[0]["Email"] : "";
                kontakt.Telefon = dt.Rows[0]["Telefon"] != null ? (string)dt.Rows[0]["Telefon"] : "";
                kontakt.Fax = dt.Rows[0]["Fax"] != null ? (string)dt.Rows[0]["Fax"] : "";
                kontakt.GSM = dt.Rows[0]["GSM"] != null ? (string)dt.Rows[0]["GSM"] : "";
                kontakt.Naslov = dt.Rows[0]["Naslov"] != null ? (string)dt.Rows[0]["Naslov"] : "";
            }

            return kontakt;
        }

        public void InsertKontakt(Kontakt kontakt)
        {
            string query = @"INSERT INTO Kontakt 
                             VALUES ('" + kontakt.ID_Kontakt + "', '" 
                                        + kontakt.Spremenil + "', DATETIME('now'), '" 
                                        + kontakt.Email + "', '" 
                                        + kontakt.Telefon + "', '" 
                                        + kontakt.Fax + "', '" 
                                        + kontakt.GSM + "', '"
                                        + kontakt.Naslov + "' )";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void UpdateKontakt(Kontakt kontakt)
        {
            string query = @"UPDATE Kontakt
                             SET Spremenil = '" + kontakt.Spremenil + "', " 
                                                + "SpremenilDatum = DATETIME('now'), "
                                                + "Email = '" + kontakt.Email + "', "
                                                + "Telefon = '" + kontakt.Telefon + "', "
                                                + "Fax = '" + kontakt.Fax + "', "
                                                + "GSM = '" + kontakt.GSM + "', "
                                                + "Naslov = '" + kontakt.Naslov + "' " +
                           "WHERE ID_Kontakt = '" + kontakt.ID_Kontakt + "'";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void DeleteKontakt()
        {
            string query = "DELETE FROM Kontakt";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        #endregion
    }
}