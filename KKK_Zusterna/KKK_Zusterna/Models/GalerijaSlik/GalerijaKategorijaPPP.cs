using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using KKK_Zusterna.Helper;

namespace KKK_Zusterna.Models
{
    public class GalerijaKategorijaPPP
    {
        #region ConnectionString

        private string ConnString = System.Configuration.ConfigurationManager.ConnectionStrings["KKKZusternaDB"].ConnectionString;

        #endregion

        #region Functionality

        public List<GalerijaKategorijaGrid> VrniKategorijeSlik()
        {
            List<GalerijaKategorijaGrid> seznamKategorij = new List<GalerijaKategorijaGrid>();

            string query = @"SELECT GalerijaKategorija.ID_galerijaKategorija, GalerijaKategorija.Naslov, GalerijaKategorija.URLSlika, GalerijaKategorija.SpremenilUporabnik, GalerijaKategorija.SpremenilDatum 
                             FROM GalerijaKategorija
                             ORDER BY ID_galerijaKategorija DESC";

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

            //Pretvorimo dt v List<GalerijaKategorijaGrid>
            if (ds != null)
            {
                DataTableToList<GalerijaKategorijaGrid> dtl = new DataTableToList<GalerijaKategorijaGrid>();
                seznamKategorij = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return seznamKategorij;
        }

        public List<GalerijaKategorijaGrid> VrniKategorijeSlikFiltered(string id, string naslov)
        {
            List<GalerijaKategorijaGrid> seznamKategorij = new List<GalerijaKategorijaGrid>();

            string query = "";

            if (id != "" && Uspeh(id))
            {
                query = @"SELECT GalerijaKategorija.ID_galerijaKategorija, GalerijaKategorija.Naslov, GalerijaKategorija.URLSlika, GalerijaKategorija.SpremenilUporabnik, GalerijaKategorija.SpremenilDatum 
                             FROM GalerijaKategorija
                             WHERE ID_galerijaKategorija = '" + id + "' ";
            }
            else
            {
                query = @"SELECT GalerijaKategorija.ID_galerijaKategorija, GalerijaKategorija.Naslov, GalerijaKategorija.URLSlika, GalerijaKategorija.SpremenilUporabnik, GalerijaKategorija.SpremenilDatum 
                             FROM GalerijaKategorija
                             WHERE Naslov LIKE '%" + naslov + "%' ";   
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

            //Pretvorimo dt v List<GalerijaKategorijaGrid>
            if (ds != null)
            {
                DataTableToList<GalerijaKategorijaGrid> dtl = new DataTableToList<GalerijaKategorijaGrid>();
                seznamKategorij = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return seznamKategorij;
        }

        public GalerijaKategorija VrniKategorijoSlik(int id)
        {
            GalerijaKategorija galerijaKategorija = null;

            string query = @"SELECT GalerijaKategorija.ID_galerijaKategorija, GalerijaKategorija.Naslov, GalerijaKategorija.URLSlika, GalerijaKategorija.SpremenilUporabnik, GalerijaKategorija.SpremenilDatum 
                             FROM GalerijaKategorija
                             WHERE ID_galerijaKategorija = '" + id + "' ";

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
                galerijaKategorija = new GalerijaKategorija();

                galerijaKategorija.ID_galerijaKategorija = (int)dt.Rows[0]["ID_galerijaKategorija"];
                galerijaKategorija.Naslov = (string)dt.Rows[0]["Naslov"];
                galerijaKategorija.URLSlika = dt.Rows[0]["URLSlika"] != null ? (string)dt.Rows[0]["URLSlika"] : "";
                galerijaKategorija.SpremenilUporabnik = dt.Rows[0]["SpremenilUporabnik"] != null ? (string)dt.Rows[0]["SpremenilUporabnik"] : "";
                galerijaKategorija.SpremenilDatum = dt.Rows[0]["SpremenilDatum"] != null ? (DateTime)dt.Rows[0]["SpremenilDatum"] : System.DateTime.Now;
            }

            return galerijaKategorija;
        }

        public int GetNextIDGalerijaKategorija()
        {
            int nextIdGalerijaKategorija = 0;

            string query = @"SELECT MAX(ID_galerijaKategorija) AS Stevilo FROM GalerijaKategorija";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            try
            {
                nextIdGalerijaKategorija = (Convert.ToInt32(cmd.ExecuteScalar())) + 1;
            }
            catch (Exception)
            {
                nextIdGalerijaKategorija = 1;
            }

            conn.Dispose();

            return nextIdGalerijaKategorija;
        }

        public void InsertGalerijaKategorija(GalerijaKategorija galerijaKategorija)
        {
            string query = @"INSERT INTO GalerijaKategorija 
                             VALUES ('" + galerijaKategorija.ID_galerijaKategorija + "', '" + galerijaKategorija.Naslov + "', '" + galerijaKategorija.URLSlika + "', '" + galerijaKategorija.SpremenilUporabnik + "', DATETIME('now') )";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void UpdateGalerijaKategorija(GalerijaKategorija galerijaKategorija)
        {
            string query = @"UPDATE GalerijaKategorija 
                             SET Naslov = '" + galerijaKategorija.Naslov + "', URLSlika ='" + galerijaKategorija.URLSlika + "', SpremenilUporabnik = '" + galerijaKategorija.SpremenilUporabnik + "', SpremenilDatum = DATETIME('now') " +
                           " WHERE ID_galerijaKategorija = '" + galerijaKategorija.ID_galerijaKategorija + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void DeleteGalerijaKategorija(int id_galerijaKategorija)
        {
            string query = "DELETE FROM GalerijaKategorija WHERE ID_galerijaKategorija = '" + id_galerijaKategorija + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public bool Uspeh(string naziv)
        {
            try
            {
                int.Parse(naziv);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}