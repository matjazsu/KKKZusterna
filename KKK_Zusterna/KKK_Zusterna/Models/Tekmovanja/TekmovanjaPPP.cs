using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using KKK_Zusterna.Helper;

namespace KKK_Zusterna.Models
{
    public class TekmovanjaPPP
    {
        #region ConnectionString

        private string ConnString = System.Configuration.ConfigurationManager.ConnectionStrings["KKKZusternaDB"].ConnectionString;

        #endregion

        #region Functionality

        public List<Tekmovanja> VrniTekmovanjaZaLeto(int id_letoTekmovanja)
        {
            List<Tekmovanja> tekmovanja = new List<Tekmovanja>();

            string query = "SELECT * FROM Tekmovanja WHERE ID_letoTekmovanja = '" + id_letoTekmovanja + "' ";

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
                DataTableToList<Tekmovanja> dtl = new DataTableToList<Tekmovanja>();
                tekmovanja = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return tekmovanja;
        }

        public void DeleteTekmovanjaZaLeto(int id_letoTekmovanja)
        {
            string query = @"DELETE FROM Tekmovanja WHERE ID_letoTekmovanja = '" + id_letoTekmovanja + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public List<Tekmovanja> VrniTekmovanjaFilter(int id_letoTekovanja, string id, string naslov)
        {
            List<Tekmovanja> tekmovanja = new List<Tekmovanja>();

            string query = "";

            if (id != null && id != "")
            {
                query += "SELECT * FROM (SELECT * FROM Tekmovanja WHERE ID_letoTekmovanja = '" +id_letoTekovanja + "' ) WHERE ID_tekmovanja = '" + id + "' ";
            }
            else if (naslov != "")
            {
                query += "SELECT * FROM (SELECT * FROM Tekmovanja WHERE ID_letoTekmovanja = '" + id_letoTekovanja + "' ) WHERE Naslov LIKE '%" + naslov + "%' ";
            }
            else
            {
                query += "SELECT * FROM (SELECT * FROM Tekmovanja WHERE ID_letoTekmovanja = '" + id_letoTekovanja + "' )";
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
                DataTableToList<Tekmovanja> dtl = new DataTableToList<Tekmovanja>();
                tekmovanja = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return tekmovanja;
        } 

        public Tekmovanja VrniTekmovanja(int id)
        {
            Tekmovanja tekmovanja = null;

            string query = "SELECT * FROM Tekmovanja WHERE ID_tekmovanja = '" + id + "' ";

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
                tekmovanja = new Tekmovanja();

                //Mapping
                tekmovanja.ID_tekmovanja = id;
                tekmovanja.Naslov = dt.Rows[0]["Naslov"] != null ? (string)dt.Rows[0]["Naslov"] : "";
                tekmovanja.Vsebina = dt.Rows[0]["Vsebina"] != null ? (string)dt.Rows[0]["Vsebina"] : "";
                tekmovanja.URLFile = dt.Rows[0]["URLFile"] != null ? (string)dt.Rows[0]["URLFile"] : "";
                tekmovanja.ID_letoTekmovanja = dt.Rows[0]["ID_letoTekmovanja"] != null ? (int)dt.Rows[0]["ID_letoTekmovanja"] : 0;
                tekmovanja.Spremenil = dt.Rows[0]["Spremenil"] != null ? (string)dt.Rows[0]["Spremenil"] : "";
                tekmovanja.SpremenilDatum = dt.Rows[0]["SpremenilDatum"] != null ? (DateTime)dt.Rows[0]["SpremenilDatum"] : System.DateTime.Now;
                tekmovanja.FileName = dt.Rows[0]["FileName"] != null ? (string)dt.Rows[0]["FileName"] : "";
            }

            return tekmovanja;
        }

        public int GetNextIdTekmovanja()
        {
            int nextIdTekmovanja = 0;

            string query = @"SELECT MAX(ID_tekmovanja) AS Stevilo FROM Tekmovanja";

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

        public void ShraniTekmovanja(Tekmovanja tekmovanja)
        {
            string query = @"INSERT INTO Tekmovanja 
                             VALUES ('" + tekmovanja.ID_tekmovanja + "', '" + tekmovanja.Naslov + "', '" + tekmovanja.Vsebina + "', '" + tekmovanja.URLFile + "', '" + tekmovanja.ID_letoTekmovanja + "', '" + tekmovanja.Spremenil + "', DATETIME('now'), '" + tekmovanja.FileName + "' ) ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void DeleteTekmovanja(int id)
        {
            string query = @"DELETE FROM Tekmovanja WHERE ID_tekmovanja = '" + id + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void UpdateTekmovanja(Tekmovanja tekmovanja)
        {
            string query = @"UPDATE Tekmovanja 
                             SET Naslov = '" + tekmovanja.Naslov + "', Vsebina = '" + tekmovanja.Vsebina
                                             + "', URLFile = '" + tekmovanja.URLFile + "', Spremenil = '"
                                             + tekmovanja.Spremenil + "', SpremenilDatum = DATETIME('now'), FileName = '" + tekmovanja.FileName + "' WHERE ID_tekmovanja = '" + tekmovanja.ID_tekmovanja + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public List<Tekmovanja> VrniTopTekmovanja()
        {
            List<Tekmovanja> tekmovanja = new List<Tekmovanja>();

            string query = "SELECT * FROM Tekmovanja ORDER BY ID_tekmovanja DESC LIMIT 5";

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
                DataTableToList<Tekmovanja> dtl = new DataTableToList<Tekmovanja>();
                tekmovanja = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return tekmovanja;
        } 

        #endregion
    }
}