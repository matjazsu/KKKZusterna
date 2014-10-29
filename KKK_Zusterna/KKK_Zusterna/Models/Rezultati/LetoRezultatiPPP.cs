using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using KKK_Zusterna.Helper;

namespace KKK_Zusterna.Models
{
    public class LetoRezultatiPPP
    {
        #region ConnectionString

        private string ConnString = System.Configuration.ConfigurationManager.ConnectionStrings["KKKZusternaDB"].ConnectionString;

        #endregion

        #region Functionality

        public List<LetoRezultati> VrniLetoRezultat()
        {
            List<LetoRezultati> letoRezultati = new List<LetoRezultati>();

            string query = "SELECT * FROM LetoRezultati ";

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
                DataTableToList<LetoRezultati> dtl = new DataTableToList<LetoRezultati>();
                letoRezultati = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return letoRezultati;
        }

        public LetoRezultati VrniLetoRezultat(int id)
        {
            LetoRezultati letoRezultati = null;

            string query = "SELECT * FROM LetoRezultati WHERE ID_letoRezultati = '" + id + "' ";

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
                letoRezultati = new LetoRezultati();

                //Mapping
                letoRezultati.ID_letoRezultati = id;
                letoRezultati.Leto = dt.Rows[0]["Leto"] != null ? (string)dt.Rows[0]["Leto"] : "";
                letoRezultati.Spremenil = dt.Rows[0]["Spremenil"] != null ? (string)dt.Rows[0]["Spremenil"] : "";
                letoRezultati.SpremenilDatum = dt.Rows[0]["SpremenilDatum"] != null ? (DateTime)dt.Rows[0]["SpremenilDatum"] : System.DateTime.Now;
            }

            return letoRezultati;
        }

        public List<LetoRezultati> VrniLetoRezultatFilter(string id, string leto)
        {
            List<LetoRezultati> letoRezultati = new List<LetoRezultati>();

            string query = "";

            if (id != null && id != "")
            {
                query += "SELECT * FROM LetoRezultati WHERE ID_letoRezultati = '" + id + "' ";
            }
            else if (leto != null && leto != "" && leto != "--Izberi--")
            {
                query += "SELECT * FROM LetoRezultati WHERE LETO LIKE '%" + leto + "%' ";
            }
            else
            {
                query += "SELECT * FROM LetoRezultati ";
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
                DataTableToList<LetoRezultati> dtl = new DataTableToList<LetoRezultati>();
                letoRezultati = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return letoRezultati;
        }

        public int GetNextIdLetoRezultati()
        {
            int nextIdLetoRezultati = 0;

            string query = @"SELECT MAX(ID_letoRezultati) AS Stevilo FROM LetoRezultati";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            try
            {
                nextIdLetoRezultati = (Convert.ToInt32(cmd.ExecuteScalar())) + 1;
            }
            catch (Exception)
            {
                nextIdLetoRezultati = 1;
            }

            conn.Dispose();

            return nextIdLetoRezultati;
        }

        public void ShraniLetoRezultati(LetoRezultati letoRezultati)
        {
            string query = @"INSERT INTO LetoRezultati 
                             VALUES ('" + letoRezultati.ID_letoRezultati + "', '" + letoRezultati.Leto + "', '" + letoRezultati.Spremenil + "', DATETIME('now') ) ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void DeleteLetoRezultati(int id)
        {
            string query = @"DELETE FROM LetoRezultati WHERE ID_letoRezultati = '" + id + "' ";

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