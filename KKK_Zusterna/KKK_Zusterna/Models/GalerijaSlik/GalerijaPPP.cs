using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using KKK_Zusterna.Helper;

namespace KKK_Zusterna.Models
{
    public class GalerijaPPP
    {
        #region ConnectionString

        private string ConnString = System.Configuration.ConfigurationManager.ConnectionStrings["KKKZusternaDB"].ConnectionString;

        #endregion

        #region Functionality

        public int GetNextIDGalerija()
        {
            int nextIdGalerija = 0;

            string query = @"SELECT MAX(ID_galerija) AS Stevilo FROM Galerija";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            try
            {
                nextIdGalerija = (Convert.ToInt32(cmd.ExecuteScalar())) + 1;
            }
            catch (Exception)
            {
                nextIdGalerija = 1;
            }

            conn.Dispose();

            return nextIdGalerija;
        }

        public void InsertGalerija(Galerija galerija)
        {
            string query = @"INSERT INTO Galerija 
                             VALUES ('" + galerija.ID_galerija + "', '" + galerija.ID_galerijaKategorija + "', '" + galerija.URLSlika + "' )";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public List<Galerija> VrniGalerijoZaGalerijaKategorija(int ID_GalerijaKategorija)
        {
            List<Galerija> galerija = new List<Galerija>();

            string query = "SELECT * FROM Galerija WHERE ID_galerijaKategorija = '" + ID_GalerijaKategorija + "' ";

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
                DataTableToList<Galerija> dtl = new DataTableToList<Galerija>();
                galerija = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return galerija;
        }

        public void DeleteGalerija(int id_galerijaKategorija)
        {
            string query = "DELETE FROM Galerija WHERE ID_galerijaKategorija = '" + id_galerijaKategorija + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void DeleteRowGalerija(int ID_galerija)
        {
            string query = "DELETE FROM Galerija WHERE ID_galerija = '" + ID_galerija + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public Galerija VrniGalerijo(int ID_galerija)
        {
            Galerija galerija = null;

            string query = "SELECT * FROM Galerija WHERE ID_galerija = '" + ID_galerija + "' ";

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
                galerija = new Galerija();

                galerija.ID_galerija = ID_galerija;
                galerija.ID_galerijaKategorija = dt.Rows[0]["ID_galerijaKategorija"] != null ? (int)dt.Rows[0]["ID_galerijaKategorija"] : 0;
                galerija.URLSlika = dt.Rows[0]["URLSlika"] != null ? (string)dt.Rows[0]["URLSlika"] : "";
            }

            return galerija;
        }

        #endregion
    }
}