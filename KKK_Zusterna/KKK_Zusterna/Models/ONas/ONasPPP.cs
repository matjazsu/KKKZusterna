using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace KKK_Zusterna.Models
{
    public class ONasPPP
    {
        #region ConnectionString

        private string ConnString = System.Configuration.ConfigurationManager.ConnectionStrings["KKKZusternaDB"].ConnectionString;

        #endregion

        #region Functionality

        public ONas VrniONas()
        {
            ONas oNas = null;

            string query = @"SELECT ID_ONas, Vsebina, Spremenil, SpremenilDatum FROM ONas WHERE ID_ONas = 1";

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
                oNas = new ONas();

                oNas.ID_ONas = (int)dt.Rows[0]["ID_ONas"];
                oNas.Vsebina = dt.Rows[0]["Vsebina"] != null ? (string)dt.Rows[0]["Vsebina"] : "";
                oNas.Spremenil = dt.Rows[0]["Spremenil"] != null ? (string)dt.Rows[0]["Spremenil"] : "";
                oNas.SpremenilDatum = dt.Rows[0]["SpremenilDatum"] != null ? (DateTime)dt.Rows[0]["SpremenilDatum"] : System.DateTime.Now;
            }

            return oNas;
        }

        public void ShraniONas(ONas tmpONas)
        {
            string query = @"INSERT INTO ONas 
                             VALUES ('" + tmpONas.ID_ONas + "', '" + tmpONas.Vsebina + "', '" + tmpONas.Spremenil + "', DATETIME('now'))";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void ZbrisiONas()
        {
            string query = @"DELETE FROM ONas";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void UrediONas(ONas oNas)
        {
            string query = @"UPDATE ONas 
                             SET Vsebina = '" + oNas.Vsebina + "', Spremenil = '" + oNas.Spremenil + "', SpremenilDatum = DATETIME('now')";

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