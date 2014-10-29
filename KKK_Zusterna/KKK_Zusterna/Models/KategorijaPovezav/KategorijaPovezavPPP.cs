using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using KKK_Zusterna.Helper;

namespace KKK_Zusterna.Models
{
    public class KategorijaPovezavPPP
    {
        #region ConnectionString

        private string ConnString = System.Configuration.ConfigurationManager.ConnectionStrings["KKKZusternaDB"].ConnectionString;

        #endregion

        #region Functionality

        public List<KategorijaPovezavGrid> VrniKategorijePovezav()
        {
            List<KategorijaPovezavGrid> seznamKategorijePovezav = new List<KategorijaPovezavGrid>();

            string query = "SELECT * FROM KategorijaPovezav";

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
                DataTableToList<KategorijaPovezavGrid> dtl = new DataTableToList<KategorijaPovezavGrid>();
                seznamKategorijePovezav = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return seznamKategorijePovezav;
        }

        public List<KategorijaPovezavGrid> VrniKategorijePovezavFiltered(string id, string naslov)
        {
            List<KategorijaPovezavGrid> seznamKategorijePovezav = new List<KategorijaPovezavGrid>();

            #region Prepare query

            string query = "";

            if (!string.IsNullOrEmpty(id))
            {
                query = "SELECT * FROM KategorijaPovezav WHERE ID_KategorijaPovezav = '" + id + "'";
            }
            else
            {
                if (!string.IsNullOrEmpty(naslov))
                {
                    query = "SELECT * FROM KategorijaPovezav WHERE Naslov LIKE '%" + naslov + "%'";
                }
                else
                {
                    query = "SELECT * FROM KategorijaPovezav";
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

            //Pretvorimo dt v List<Novica>
            if (ds != null)
            {
                DataTableToList<KategorijaPovezavGrid> dtl = new DataTableToList<KategorijaPovezavGrid>();
                seznamKategorijePovezav = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return seznamKategorijePovezav;
        }

        public KategorijaPovezav VrniKategorijoPovezav(int ID_KategorijaPovezav)
        {
            KategorijaPovezav kategorijaPovezav = null;

            string query = @"SELECT * FROM KategorijaPovezav WHERE ID_KategorijaPovezav = '" + ID_KategorijaPovezav + "' ";

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
                kategorijaPovezav = new KategorijaPovezav();

                kategorijaPovezav.ID_KategorijaPovezav = (int)dt.Rows[0]["ID_KategorijaPovezav"];
                kategorijaPovezav.Naslov = (string)dt.Rows[0]["Naslov"];
                kategorijaPovezav.Spremenil = dt.Rows[0]["Spremenil"] != null ? (string)dt.Rows[0]["Spremenil"] : "";
                kategorijaPovezav.SpremenilDatum = dt.Rows[0]["SpremenilDatum"] != null ? (DateTime)dt.Rows[0]["SpremenilDatum"] : System.DateTime.Now;
            }

            return kategorijaPovezav;
        }

        public int GetNextIdKategorijaPovezav()
        {
            int nextIdKategorijaPovezav = 0;

            string query = @"SELECT MAX(ID_KategorijaPovezav) AS Stevilo FROM KategorijaPovezav";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            try
            {
                nextIdKategorijaPovezav = (Convert.ToInt32(cmd.ExecuteScalar())) + 1;
            }
            catch (Exception)
            {
                nextIdKategorijaPovezav = 1;
            }

            conn.Dispose();

            return nextIdKategorijaPovezav;
        }

        public void ShraniKategorijoPovezav(KategorijaPovezav kategorijaPovezav)
        {
            string query = @"INSERT INTO KategorijaPovezav 
                             VALUES ('" + kategorijaPovezav.ID_KategorijaPovezav + "', '" + kategorijaPovezav.Naslov + "', '" + kategorijaPovezav.Spremenil + "', DATETIME('now') ) ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void DeleteKategorijePovezav(int ID_KategorijaPovezav)
        {
            string query = @"DELETE FROM KategorijaPovezav WHERE ID_KategorijaPovezav = '" + ID_KategorijaPovezav + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void UpdateKategorijoPovezav(KategorijaPovezav kategorijaPovezav)
        {
            string query = @"UPDATE KategorijaPovezav 
                             SET Naslov = '" + kategorijaPovezav.Naslov + "', Spremenil = '" + kategorijaPovezav.Spremenil + "', SpremenilDatum = DATETIME('now') WHERE ID_KategorijaPovezav = '" + kategorijaPovezav.ID_KategorijaPovezav + "' ";

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