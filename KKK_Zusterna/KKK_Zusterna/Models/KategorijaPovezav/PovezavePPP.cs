using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using KKK_Zusterna.Helper;

namespace KKK_Zusterna.Models
{
    public class PovezavePPP
    {
        #region ConnectionString

        private string ConnString = System.Configuration.ConfigurationManager.ConnectionStrings["KKKZusternaDB"].ConnectionString;

        #endregion

        #region Functionality

        public List<PovezaveGrid> VrniPovezaveZaKategorijo(int ID_KategorijaPovezav)
        {
            List<PovezaveGrid> povezave = new List<PovezaveGrid>();

            string query = "SELECT * FROM Povezave WHERE ID_KategorijaPovezav = '" + ID_KategorijaPovezav + "' ";

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
                DataTableToList<PovezaveGrid> dtl = new DataTableToList<PovezaveGrid>();
                povezave = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return povezave;
        }

        public List<PovezaveGrid> VrniPovezaveZaKategorijoFiltered(int ID_KategorijaPovezav, string ID_Povezave, string Naziv)
        {
            List<PovezaveGrid> povezave = new List<PovezaveGrid>();

            string query = "";

            if (!string.IsNullOrEmpty(ID_Povezave))
            {
                query += "SELECT * FROM ( SELECT * FROM Povezave WHERE ID_KategorijaPovezav = '" + ID_KategorijaPovezav + "') WHERE ID_Povezave = '" + ID_Povezave + "' ";
            }
            else if (!string.IsNullOrEmpty(Naziv))
            {
                query += "SELECT * FROM ( SELECT * FROM Povezave WHERE ID_KategorijaPovezav = '" + ID_KategorijaPovezav + "') WHERE Naziv LIKE '%" + Naziv + "%' ";
            }
            else
            {
                query += "SELECT * FROM ( SELECT * FROM Povezave WHERE ID_KategorijaPovezav = '" + ID_KategorijaPovezav + "')";
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
                DataTableToList<PovezaveGrid> dtl = new DataTableToList<PovezaveGrid>();
                povezave = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return povezave;
        }

        public int VrniNextIdForPovezave()
        {
            int nextIdPovezava = 0;

            string query = @"SELECT MAX(ID_Povezava) AS Stevilo FROM Povezave";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            try
            {
                nextIdPovezava = (Convert.ToInt32(cmd.ExecuteScalar())) + 1;
            }
            catch (Exception)
            {
                nextIdPovezava = 1;
            }

            conn.Dispose();

            return nextIdPovezava;
        }

        public void ShraniPovezavo(Povezave povezava)
        {
            string query = @"INSERT INTO Povezave 
                             VALUES ('" + povezava.ID_Povezava + "', '" + povezava.ID_KategorijaPovezav + "', '" + povezava.Naziv + "', '" + povezava.URL + "', '" + povezava.Spremenil + "', DATETIME('now') ) ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public Povezave VrniPovezavoZaId(int ID_povezava)
        {
            Povezave povezava = null;

            string query = "SELECT * FROM Povezave WHERE ID_Povezava = '" + ID_povezava + "' ";

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

            if (ds != null)
            {
                povezava = new Povezave();

                //Mapping
                povezava.ID_Povezava = (int)dt.Rows[0]["ID_Povezava"];
                povezava.ID_KategorijaPovezav = (int)dt.Rows[0]["ID_KategorijaPovezav"];
                povezava.Naziv = (string)dt.Rows[0]["Naziv"];
                povezava.URL = (string)dt.Rows[0]["URL"];
                povezava.Spremenil = dt.Rows[0]["Spremenil"] != null ? (string)dt.Rows[0]["Spremenil"] : "";
                povezava.SpremenilDatum = dt.Rows[0]["SpremenilDatum"] != null ? (DateTime)dt.Rows[0]["SpremenilDatum"] : System.DateTime.Now;
            }

            return povezava;
        }

        public void UpdatePovezava(Povezave povezava)
        {
            string query = @"UPDATE Povezave 
                             SET Naziv = '" + povezava.Naziv + "', URL = '" + povezava.URL + "', Spremenil = '" + povezava.Spremenil + "', SpremenilDatum = DATETIME('now') WHERE ID_Povezava = '" + povezava.ID_Povezava + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void DeletePovezava(int ID_Povezava)
        {
            string query = @"DELETE FROM Povezave WHERE ID_Povezava = '" + ID_Povezava + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void DeletePovezaveZaKategorijo(int ID_KategorijaPovezav)
        {
            string query = @"DELETE FROM Povezave WHERE ID_KategorijaPovezav = '" + ID_KategorijaPovezav + "' ";

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