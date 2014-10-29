using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using KKK_Zusterna.Helper;

namespace KKK_Zusterna.Models
{
    public class LetoTekmovanjaPPP
    {
        #region ConnectionString

        private string ConnString = System.Configuration.ConfigurationManager.ConnectionStrings["KKKZusternaDB"].ConnectionString;

        #endregion

        #region Functionality

        public List<LetoTekmovanja> VrniLetoTekmovanja()
        {
            List<LetoTekmovanja> letoTekmovanja = new List<LetoTekmovanja>();

            string query = "SELECT * FROM LetoTekmovanja ";

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
                DataTableToList<LetoTekmovanja> dtl = new DataTableToList<LetoTekmovanja>();
                letoTekmovanja = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return letoTekmovanja;
        }

        public LetoTekmovanja VrniLetoTekmovanja(int id)
        {
            LetoTekmovanja letoTekmovanja = null;

            string query = "SELECT * FROM LetoTekmovanja WHERE ID_letoTekmovanja = '" + id + "' ";

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

            //Pretvorimo dt v List<LetoTekmovanja>
            if (ds != null)
            {
                letoTekmovanja = new LetoTekmovanja();

                //Mapping
                letoTekmovanja.ID_letoTekmovanja = id;
                letoTekmovanja.Leto = dt.Rows[0]["Leto"] != null ? (string)dt.Rows[0]["Leto"] : "";
                letoTekmovanja.Spremenil = dt.Rows[0]["Spremenil"] != null ? (string)dt.Rows[0]["Spremenil"] : "";
                letoTekmovanja.SpremenilDatum = dt.Rows[0]["SpremenilDatum"] != null ? (DateTime)dt.Rows[0]["SpremenilDatum"] : System.DateTime.Now;
            }

            return letoTekmovanja;
        }

        public int GetNextIdLetoTekmovanja()
        {
            int nextIdTekmovanja = 0;

            string query = @"SELECT MAX(ID_letoTekmovanja) AS Stevilo FROM LetoTekmovanja";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            try
            {
                nextIdTekmovanja = (Convert.ToInt32(cmd.ExecuteScalar())) + 1;
            }
            catch (Exception)
            {
                nextIdTekmovanja = 1;
            }

            conn.Dispose();

            return nextIdTekmovanja;
        }

        public void ShraniLetoTekmovanja(LetoTekmovanja letoTekmovanja)
        {
            string query = @"INSERT INTO LetoTekmovanja 
                             VALUES ('" + letoTekmovanja.ID_letoTekmovanja + "', '" + letoTekmovanja.Leto + "', '" + letoTekmovanja.Spremenil + "', DATETIME('now') ) ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public List<LetoTekmovanja> VrniLetoTekmovanjaFilter(string id, string leto)
        {
            List<LetoTekmovanja> letoTekmovanja = new List<LetoTekmovanja>();

            string query = "";

            if (id != null && id != "")
            {
                query += "SELECT * FROM LetoTekmovanja WHERE ID_letoTekmovanja = '" + id + "' ";
            }
            else if (leto != null && leto != "" && leto != "--Izberi--")
            {
                query += "SELECT * FROM LetoTekmovanja WHERE LETO LIKE '%" + leto + "%' ";
            }
            else
            {
                query += "SELECT * FROM LetoTekmovanja ";
            }

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            SQLiteDataReader reader = cmd.ExecuteReader();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            ds.Tables.Add(dt);
            dt.Load(reader);

            conn.Dispose();

            //Pretvorimo dt v List<LetoRezultati>
            if (ds != null)
            {
                DataTableToList<LetoTekmovanja> dtl = new DataTableToList<LetoTekmovanja>();
                letoTekmovanja = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return letoTekmovanja;
        } 

        public void DeleteLetoTekmovanja(int id)
        {
            string query = @"DELETE FROM LetoTekmovanja WHERE ID_letoTekmovanja = '" + id + "' ";

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