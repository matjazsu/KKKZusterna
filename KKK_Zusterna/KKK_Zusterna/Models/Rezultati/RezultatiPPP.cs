using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using KKK_Zusterna.Helper;

namespace KKK_Zusterna.Models
{
    public class RezultatiPPP
    {
        #region ConnectionString

        private string ConnString = System.Configuration.ConfigurationManager.ConnectionStrings["KKKZusternaDB"].ConnectionString;

        #endregion

        #region Functionality

        public List<Rezultati> VrniRezultateZaLeto(int id_letoRezultati)
        {
            List<Rezultati> rezultati = new List<Rezultati>();

            string query = "SELECT * FROM Rezultati WHERE ID_letoRezultati = '" + id_letoRezultati + "' ";

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

            //Pretvorimo dt v List<Rezultati>
            if (ds != null)
            {
                DataTableToList<Rezultati> dtl = new DataTableToList<Rezultati>();
                rezultati = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return rezultati;
        }

        public Rezultati VrniRezultat(int id)
        {
            Rezultati rezultati = null;

            string query = "SELECT * FROM Rezultati WHERE ID_rezultati = '" + id + "' ";

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

            //Pretvorimo dt v List<Rezultati>
            if (ds != null)
            {
                rezultati = new Rezultati();

                //Mapping
                rezultati.ID_rezultati = id;
                rezultati.Naslov = dt.Rows[0]["Naslov"] != null ? (string)dt.Rows[0]["Naslov"] : "";
                rezultati.Vsebina = dt.Rows[0]["Vsebina"] != null ? (string)dt.Rows[0]["Vsebina"] : "";
                rezultati.URLFile = dt.Rows[0]["URLFile"] != null ? (string)dt.Rows[0]["URLFile"] : "";
                rezultati.ID_letoRezultati = dt.Rows[0]["ID_letoRezultati"] != null ? (int)dt.Rows[0]["ID_letoRezultati"] : 0;
                rezultati.Spremenil = dt.Rows[0]["Spremenil"] != null ? (string)dt.Rows[0]["Spremenil"] : "";
                rezultati.SpremenilDatum = dt.Rows[0]["SpremenilDatum"] != null ? (DateTime)dt.Rows[0]["SpremenilDatum"] : System.DateTime.Now;
                rezultati.FileName = dt.Rows[0]["FileName"] != null ? (string)dt.Rows[0]["FileName"] : "";
            }

            return rezultati;
        }

        public List<Rezultati> VrniRezultatFilter(string id, int id_letoRezultati, string naslov)
        {
            List<Rezultati> rezultati = new List<Rezultati>();

            string query = "";

            if (id != null && id != "")
            {
                query += "SELECT * FROM (SELECT * FROM Rezultati WHERE ID_letoRezultati = '" + id_letoRezultati + "') WHERE ID_rezultati = '" + id + "' ";
            }
            else if (naslov != "")
            {
                query += "SELECT * FROM (SELECT * FROM Rezultati WHERE ID_letoRezultati = '" + id_letoRezultati + "') WHERE Naslov LIKE '%" + naslov + "%' ";
            }
            else
            {
                query += "SELECT * FROM (SELECT * FROM Rezultati WHERE ID_letoRezultati = '" + id_letoRezultati + "') ";
            }

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

            //Pretvorimo dt v List<LetoRezultati>
            if (ds != null)
            {
                DataTableToList<Rezultati> dtl = new DataTableToList<Rezultati>();
                rezultati = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return rezultati;
        }

        public int GetNextIdRezultati()
        {
            int nextIdRezultati = 0;

            string query = @"SELECT MAX(ID_rezultati) AS Stevilo FROM Rezultati";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            try
            {
                nextIdRezultati = (Convert.ToInt32(cmd.ExecuteScalar())) + 1;
            }
            catch (Exception)
            {
                nextIdRezultati = 1;
            }

            conn.Dispose();

            return nextIdRezultati;
        }

        public void ShraniRezultati(Rezultati rezultati)
        {
            string query = @"INSERT INTO Rezultati 
                             VALUES ('" + rezultati.ID_rezultati + "', '" + rezultati.Naslov + "', '" + rezultati.Vsebina + "', '" + rezultati.URLFile + "', '" + rezultati.ID_letoRezultati + "', '" + rezultati.Spremenil + "', DATETIME('now'), '" + rezultati.FileName + "' ) ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void UpdateRezultati(Rezultati rezultati)
        {
            string query = @"UPDATE Rezultati 
                             SET Naslov = '" + rezultati.Naslov + "', Vsebina = '" + rezultati.Vsebina 
                                             + "', URLFile = '" + rezultati.URLFile + "', Spremenil = '" 
                                             + rezultati.Spremenil + "', SpremenilDatum = DATETIME('now'), FileName = '" + rezultati.FileName + "' WHERE ID_rezultati = '" + rezultati.ID_rezultati + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void DeleteRezultati(int id)
        {
            string query = @"DELETE FROM Rezultati WHERE ID_rezultati = '" + id + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void DeleteRezultatiZaLeto(int id_letoRezultati)
        {
            string query = @"DELETE FROM Rezultati WHERE ID_letoRezultati = '" + id_letoRezultati + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public List<Rezultati> VrniTopRezultate()
        {
            List<Rezultati> rezultati = new List<Rezultati>();

            string query = "SELECT * FROM Rezultati ORDER BY ID_rezultati DESC LIMIT 5";

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

            //Pretvorimo dt v List<Rezultati>
            if (ds != null)
            {
                DataTableToList<Rezultati> dtl = new DataTableToList<Rezultati>();
                rezultati = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return rezultati;
        } 

        #endregion
    }
}