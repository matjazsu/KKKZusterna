using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using KKK_Zusterna.Helper;

namespace KKK_Zusterna.Models
{
    public class TreningPPP
    {
        #region ConnectionString

        private string ConnString = System.Configuration.ConfigurationManager.ConnectionStrings["KKKZusternaDB"].ConnectionString;

        #endregion

        #region Functionality

        public List<TreningGrid> VrniTreninge()
        {
            List<TreningGrid> treningi = new List<TreningGrid>();

            string query = "SELECT ID_Trening, Naziv, Spremenil, SpremenilDatum FROM Trening";

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

            //Pretvorimo dt v List<Tekmovanja>
            if (ds != null)
            {
                DataTableToList<TreningGrid> dtl = new DataTableToList<TreningGrid>();
                treningi = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return treningi;
        }

        public List<Trening> VrniTreningeAll()
        {
            List<Trening> treningi = new List<Trening>();

            string query = "SELECT * FROM Trening";

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

            //Pretvorimo dt v List<Tekmovanja>
            if (ds != null)
            {
                DataTableToList<Trening> dtl = new DataTableToList<Trening>();
                treningi = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return treningi;
        }

        public Trening VrniTreningZaId(int ID_Trening)
        {
            Trening trening = null;

            string query = "SELECT * FROM Trening WHERE ID_Trening = '" + ID_Trening + "'";

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

            //Pretvorimo dt v List<Tekmovanja>
            if (ds != null)
            {
                trening = new Trening();

                trening.ID_Trening = (int)dt.Rows[0]["ID_Trening"];
                trening.Naziv = (string)dt.Rows[0]["Naziv"];
                trening.Spremenil = dt.Rows[0]["Spremenil"] != null ? (string)dt.Rows[0]["Spremenil"] : "";
                trening.SpremenilDatum = dt.Rows[0]["SpremenilDatum"] != null ? (DateTime)dt.Rows[0]["SpremenilDatum"] : System.DateTime.Now;

                trening.Pon_Dop_Od = dt.Rows[0]["Pon_Dop_Od"] != null ? (string)dt.Rows[0]["Pon_Dop_Od"] : "";
                trening.Pon_Dop_Do = dt.Rows[0]["Pon_Dop_Do"] != null ? (string)dt.Rows[0]["Pon_Dop_Do"] : "";
                trening.Pon_Dop_Tre = dt.Rows[0]["Pon_Dop_Tre"] != null ? (string)dt.Rows[0]["Pon_Dop_Tre"] : "";
                trening.Pon_Pop_Od = dt.Rows[0]["Pon_Pop_Od"] != null ? (string)dt.Rows[0]["Pon_Pop_Od"] : "";
                trening.Pon_Pop_Do = dt.Rows[0]["Pon_Pop_Do"] != null ? (string)dt.Rows[0]["Pon_Pop_Do"] : "";
                trening.Pon_Pop_Tre = dt.Rows[0]["Pon_Pop_Tre"] != null ? (string)dt.Rows[0]["Pon_Pop_Tre"] : "";

                trening.Tor_Dop_Od = dt.Rows[0]["Tor_Dop_Od"] != null ? (string)dt.Rows[0]["Tor_Dop_Od"] : "";
                trening.Tor_Dop_Do = dt.Rows[0]["Tor_Dop_Do"] != null ? (string)dt.Rows[0]["Tor_Dop_Do"] : "";
                trening.Tor_Dop_Tre = dt.Rows[0]["Tor_Dop_Tre"] != null ? (string)dt.Rows[0]["Tor_Dop_Tre"] : "";
                trening.Tor_Pop_Od = dt.Rows[0]["Tor_Pop_Od"] != null ? (string)dt.Rows[0]["Tor_Pop_Od"] : "";
                trening.Tor_Pop_Do = dt.Rows[0]["Tor_Pop_Do"] != null ? (string)dt.Rows[0]["Tor_Pop_Do"] : "";
                trening.Tor_Pop_Tre = dt.Rows[0]["Tor_Pop_Tre"] != null ? (string)dt.Rows[0]["Tor_Pop_Tre"] : "";

                trening.Sre_Dop_Od = dt.Rows[0]["Sre_Dop_Od"] != null ? (string)dt.Rows[0]["Sre_Dop_Od"] : "";
                trening.Sre_Dop_Do = dt.Rows[0]["Sre_Dop_Do"] != null ? (string)dt.Rows[0]["Sre_Dop_Do"] : "";
                trening.Sre_Dop_Tre = dt.Rows[0]["Sre_Dop_Tre"] != null ? (string)dt.Rows[0]["Sre_Dop_Tre"] : "";
                trening.Sre_Pop_Od = dt.Rows[0]["Sre_Pop_Od"] != null ? (string)dt.Rows[0]["Sre_Pop_Od"] : "";
                trening.Sre_Pop_Do = dt.Rows[0]["Sre_Pop_Do"] != null ? (string)dt.Rows[0]["Sre_Pop_Do"] : "";
                trening.Sre_Pop_Tre = dt.Rows[0]["Sre_Pop_Tre"] != null ? (string)dt.Rows[0]["Sre_Pop_Tre"] : "";

                trening.Cet_Dop_Od = dt.Rows[0]["Cet_Dop_Od"] != null ? (string)dt.Rows[0]["Cet_Dop_Od"] : "";
                trening.Cet_Dop_Do = dt.Rows[0]["Cet_Dop_Do"] != null ? (string)dt.Rows[0]["Cet_Dop_Do"] : "";
                trening.Cet_Dop_Tre = dt.Rows[0]["Cet_Dop_Tre"] != null ? (string)dt.Rows[0]["Cet_Dop_Tre"] : "";
                trening.Cet_Pop_Od = dt.Rows[0]["Cet_Pop_Od"] != null ? (string)dt.Rows[0]["Cet_Pop_Od"] : "";
                trening.Cet_Pop_Do = dt.Rows[0]["Cet_Pop_Do"] != null ? (string)dt.Rows[0]["Cet_Pop_Do"] : "";
                trening.Cet_Pop_Tre = dt.Rows[0]["Cet_Pop_Tre"] != null ? (string)dt.Rows[0]["Cet_Pop_Tre"] : "";

                trening.Pet_Dop_Od = dt.Rows[0]["Pet_Dop_Od"] != null ? (string)dt.Rows[0]["Pet_Dop_Od"] : "";
                trening.Pet_Dop_Do = dt.Rows[0]["Pet_Dop_Do"] != null ? (string)dt.Rows[0]["Pet_Dop_Do"] : "";
                trening.Pet_Dop_Tre = dt.Rows[0]["Pet_Dop_Tre"] != null ? (string)dt.Rows[0]["Pet_Dop_Tre"] : "";
                trening.Pet_Pop_Od = dt.Rows[0]["Pet_Pop_Od"] != null ? (string)dt.Rows[0]["Pet_Pop_Od"] : "";
                trening.Pet_Pop_Do = dt.Rows[0]["Pet_Pop_Do"] != null ? (string)dt.Rows[0]["Pet_Pop_Do"] : "";
                trening.Pet_Pop_Tre = dt.Rows[0]["Pet_Pop_Tre"] != null ? (string)dt.Rows[0]["Pet_Pop_Tre"] : "";

                trening.Sob_Dop_Od = dt.Rows[0]["Sob_Dop_Od"] != null ? (string)dt.Rows[0]["Sob_Dop_Od"] : "";
                trening.Sob_Dop_Do = dt.Rows[0]["Sob_Dop_Do"] != null ? (string)dt.Rows[0]["Sob_Dop_Do"] : "";
                trening.Sob_Dop_Tre = dt.Rows[0]["Sob_Dop_Tre"] != null ? (string)dt.Rows[0]["Sob_Dop_Tre"] : "";
                trening.Sob_Pop_Od = dt.Rows[0]["Sob_Pop_Od"] != null ? (string)dt.Rows[0]["Sob_Pop_Od"] : "";
                trening.Sob_Pop_Do = dt.Rows[0]["Sob_Pop_Do"] != null ? (string)dt.Rows[0]["Sob_Pop_Do"] : "";
                trening.Sob_Pop_Tre = dt.Rows[0]["Sob_Pop_Tre"] != null ? (string)dt.Rows[0]["Sob_Pop_Tre"] : "";

                trening.Ned_Dop_Od = dt.Rows[0]["Ned_Dop_Od"] != null ? (string)dt.Rows[0]["Ned_Dop_Od"] : "";
                trening.Ned_Dop_Do = dt.Rows[0]["Ned_Dop_Do"] != null ? (string)dt.Rows[0]["Ned_Dop_Do"] : "";
                trening.Ned_Dop_Tre = dt.Rows[0]["Ned_Dop_Tre"] != null ? (string)dt.Rows[0]["Ned_Dop_Tre"] : "";
                trening.Ned_Pop_Od = dt.Rows[0]["Ned_Pop_Od"] != null ? (string)dt.Rows[0]["Ned_Pop_Od"] : "";
                trening.Ned_Pop_Do = dt.Rows[0]["Ned_Pop_Do"] != null ? (string)dt.Rows[0]["Ned_Pop_Do"] : "";
                trening.Ned_Pop_Tre = dt.Rows[0]["Ned_Pop_Tre"] != null ? (string)dt.Rows[0]["Ned_Pop_Tre"] : "";
            }

            return trening;
        }

        public List<TreningGrid> VrniTreningeFilter(string ID_Trening, string Naziv)
        {
            List<TreningGrid> treningi = new List<TreningGrid>();

            #region Prepare query

            string query = "SELECT ID_Trening, Naziv, Spremenil, SpremenilDatum FROM Trening";

            if (!string.IsNullOrEmpty(ID_Trening))
            {
                query = "SELECT ID_Trening, Naziv, Spremenil, SpremenilDatum FROM Trening WHERE ID_Trening ='" + ID_Trening + "'";
            }
            else
            {
                if (!string.IsNullOrEmpty(Naziv))
                {
                    query = "SELECT ID_Trening, Naziv, Spremenil, SpremenilDatum FROM Trening WHERE Naziv LIKE'%" + Naziv + "%'";    
                }
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

            //Pretvorimo dt v List<Tekmovanja>
            if (ds != null)
            {
                DataTableToList<TreningGrid> dtl = new DataTableToList<TreningGrid>();
                treningi = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return treningi;
        }

        public int VrniNextId()
        {
            int nextIdTrening = 0;

            string query = @"SELECT MAX(ID_Trening) AS Stevilo FROM Trening";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            try
            {
                nextIdTrening = (Convert.ToInt32(cmd.ExecuteScalar())) + 1;
            }
            catch (Exception)
            {
                nextIdTrening = 1;
            }

            conn.Dispose();

            return nextIdTrening;
        }

        public void ShraniTrening(Trening trening)
        {
            string query = @"INSERT INTO Trening 
                             VALUES (
                             '" + trening.ID_Trening + "', '"
                                + trening.Naziv + "', '"
                                + trening.Spremenil + "', DATETIME('now'), '"
                                + trening.Pon_Dop_Od + "', '"
                                + trening.Pon_Dop_Do + "', '"
                                + trening.Pon_Dop_Tre + "', '"
                                + trening.Pon_Pop_Od + "', '"
                                + trening.Pon_Pop_Do + "', '"
                                + trening.Pon_Pop_Tre + "', '"
                                + trening.Tor_Dop_Od + "', '"
                                + trening.Tor_Dop_Do + "', '"
                                + trening.Tor_Dop_Tre + "', '"
                                + trening.Tor_Pop_Od + "', '"
                                + trening.Tor_Pop_Do + "', '"
                                + trening.Tor_Pop_Tre + "', '"
                                + trening.Sre_Dop_Od + "', '"
                                + trening.Sre_Dop_Do + "', '"
                                + trening.Sre_Dop_Tre + "', '"
                                + trening.Sre_Pop_Od + "', '"
                                + trening.Sre_Pop_Do + "', '"
                                + trening.Sre_Pop_Tre + "', '"
                                + trening.Cet_Dop_Od + "', '"
                                + trening.Cet_Dop_Do + "', '"
                                + trening.Cet_Dop_Tre + "', '"
                                + trening.Cet_Pop_Od + "', '"
                                + trening.Cet_Pop_Do + "', '"
                                + trening.Cet_Pop_Tre + "', '"
                                + trening.Pet_Dop_Od + "', '"
                                + trening.Pet_Dop_Do + "', '"
                                + trening.Pet_Dop_Tre + "', '"
                                + trening.Pet_Pop_Od + "', '"
                                + trening.Pet_Pop_Do + "', '"
                                + trening.Pet_Pop_Tre + "', '"
                                + trening.Sob_Dop_Od + "', '"
                                + trening.Sob_Dop_Do + "', '"
                                + trening.Sob_Dop_Tre + "', '"
                                + trening.Sob_Pop_Od + "', '"
                                + trening.Sob_Pop_Do + "', '"
                                + trening.Sob_Pop_Tre + "', '"
                                + trening.Ned_Dop_Od + "', '"
                                + trening.Ned_Dop_Do + "', '"
                                + trening.Ned_Dop_Tre + "', '"
                                + trening.Ned_Pop_Od + "', '"
                                + trening.Ned_Pop_Do + "', '"
                                + trening.Ned_Pop_Tre + "' )";
                                

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void UpdateTrening(Trening trening)
        {
            string query = @"UPDATE Trening
                             SET 
                                ID_Trening = '" + trening.ID_Trening + "', "
                                + "Naziv = '" + trening.Naziv + "', "
                                + "Spremenil = '" +trening.Spremenil + "', "
                                + "SpremenilDatum = DATETIME('now'), "
                                + "Pon_Dop_Od = '" + trening.Pon_Dop_Od + "', "
                                + "Pon_Dop_Do = '" + trening.Pon_Dop_Do + "', "
                                + "Pon_Dop_Tre = '" + trening.Pon_Dop_Tre + "', "
                                + "Pon_Pop_Od = '" + trening.Pon_Pop_Od + "', "
                                + "Pon_Pop_Do = '" + trening.Pon_Pop_Do + "', "
                                + "Pon_Pop_Tre = '" + trening.Pon_Pop_Tre + "', "
                                + "Tor_Dop_Od = '" + trening.Tor_Dop_Od + "', "
                                + "Tor_Dop_Do = '" + trening.Tor_Dop_Do + "', "
                                + "Tor_Dop_Tre = '" + trening.Tor_Dop_Tre + "', "
                                + "Tor_Pop_Od = '" + trening.Tor_Pop_Od + "', "
                                + "Tor_Pop_Do = '" + trening.Tor_Pop_Do + "', "
                                + "Tor_Pop_Tre = '" + trening.Tor_Pop_Tre + "', "
                                + "Sre_Dop_Od = '" + trening.Sre_Dop_Od + "', "
                                + "Sre_Dop_Do = '" + trening.Sre_Dop_Do + "', "
                                + "Sre_Dop_Tre = '" + trening.Sre_Dop_Tre + "', "
                                + "Sre_Pop_Od = '" + trening.Sre_Pop_Od + "', "
                                + "Sre_Pop_Do = '" + trening.Sre_Pop_Do + "', "
                                + "Sre_Pop_Tre = '" + trening.Sre_Pop_Tre + "', "
                                + "Cet_Dop_Od = '" + trening.Cet_Dop_Od + "', "
                                + "Cet_Dop_Do = '" + trening.Cet_Dop_Do + "', "
                                + "Cet_Dop_Tre = '" + trening.Cet_Dop_Tre + "', "
                                + "Cet_Pop_Od = '" + trening.Cet_Pop_Od + "', "
                                + "Cet_Pop_Do = '" + trening.Cet_Pop_Do + "', "
                                + "Cet_Pop_Tre = '" + trening.Cet_Pop_Tre + "', "
                                + "Pet_Dop_Od = '" + trening.Pet_Dop_Od + "', "
                                + "Pet_Dop_Do = '" + trening.Pet_Dop_Do + "', "
                                + "Pet_Dop_Tre = '" + trening.Pet_Dop_Tre + "', "
                                + "Pet_Pop_Od = '" + trening.Pet_Pop_Od + "', "
                                + "Pet_Pop_Do = '" + trening.Pet_Pop_Do + "', "
                                + "Pet_Pop_Tre = '" + trening.Pet_Pop_Tre + "', "
                                + "Sob_Dop_Od = '" + trening.Sob_Dop_Od + "', "
                                + "Sob_Dop_Do = '" + trening.Sob_Dop_Do + "', "
                                + "Sob_Dop_Tre = '" + trening.Sob_Dop_Tre + "', "
                                + "Sob_Pop_Od = '" + trening.Sob_Pop_Od + "', "
                                + "Sob_Pop_Do = '" + trening.Sob_Pop_Do + "', "
                                + "Sob_Pop_Tre = '" + trening.Sob_Pop_Tre + "', "
                                + "Ned_Dop_Od = '" + trening.Ned_Dop_Od + "', "
                                + "Ned_Dop_Do = '" + trening.Ned_Dop_Do + "', "
                                + "Ned_Dop_Tre = '" + trening.Ned_Dop_Tre + "', "
                                + "Ned_Pop_Od = '" + trening.Ned_Pop_Od + "', "
                                + "Ned_Pop_Do = '" + trening.Ned_Pop_Do + "', "
                                + "Ned_Pop_Tre = '" + trening.Ned_Pop_Tre + "' "
                                + "WHERE ID_Trening = '" + trening.ID_Trening + "' ";


            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void DeleteTrening(int ID_Trening)
        {
            string query = @"DELETE FROM Trening WHERE ID_Trening = '" + ID_Trening + "' ";

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