using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.IO;
using KKK_Zusterna.Helper;

namespace KKK_Zusterna.Models
{
    public class NovicaPPP
    {
        #region ConnectionString

        private string ConnString = System.Configuration.ConfigurationManager.ConnectionStrings["KKKZusternaDB"].ConnectionString;

        #endregion

        #region Functionality

        public List<NovicaGrid> VrniNovice()
        {
            List<NovicaGrid> seznamNovic = new List<NovicaGrid>();

            string query = @"SELECT Novica.ID_novica, Novica.Naslov, Novica.Povzetek, Novica.Vsebina, Novica.ID_uporabnik, Novica.DatumSpremenil, Novica.URLSlika, (Uporabnik.Ime || ' ' || Uporabnik.Priimek) AS Uporabnik 
                             FROM Novica, Uporabnik 
                             WHERE Novica.ID_uporabnik = Uporabnik.ID_uporabnik ORDER BY ID_novica DESC";

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
                DataTableToList<NovicaGrid> dtl = new DataTableToList<NovicaGrid>();
                seznamNovic = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return seznamNovic;
        }

        public List<NovicaGrid> VrniNoviceFiltered(string id_novica, string naslov, string avtor, string DatumOdFilter, string DatumDoFilter)
        {
            List<NovicaGrid> seznamNovic = new List<NovicaGrid>();

            string query = "";

            #region Create Query

            if (id_novica != "")
            {

                query = @"SELECT Novica.ID_novica, Novica.Naslov, Novica.Povzetek, Novica.Vsebina, Novica.ID_uporabnik, Novica.DatumSpremenil, Novica.URLSlika, (Uporabnik.Ime || ' ' || Uporabnik.Priimek) AS Uporabnik 
                            FROM Novica, Uporabnik 
                            WHERE Novica.ID_uporabnik = Uporabnik.ID_uporabnik AND       
                            Novica.ID_novica = '" + id_novica + "'";
            }
            else if (DatumDoFilter == "" && DatumOdFilter == "")
            {
                query = @"SELECT Novica.ID_novica, Novica.Naslov, Novica.Povzetek, Novica.Vsebina, Novica.ID_uporabnik, Novica.DatumSpremenil, Novica.URLSlika, (Uporabnik.Ime || ' ' || Uporabnik.Priimek) AS Uporabnik 
                            FROM Novica, Uporabnik 
                            WHERE Novica.ID_uporabnik = Uporabnik.ID_uporabnik AND       
                            Novica.Naslov LIKE '%" + naslov + "%' AND (Uporabnik.Ime LIKE '%" + avtor + "%' OR Uporabnik.Priimek LIKE '%" + avtor + "%')";
            }
            else
            {
                if (DatumOdFilter != "" && DatumDoFilter == "")
                {
                    query = @"SELECT Novica.ID_novica, Novica.Naslov, Novica.Povzetek, Novica.Vsebina, Novica.ID_uporabnik, Novica.DatumSpremenil, Novica.URLSlika, (Uporabnik.Ime || ' ' || Uporabnik.Priimek) AS Uporabnik 
                            FROM Novica, Uporabnik 
                            WHERE Novica.ID_uporabnik = Uporabnik.ID_uporabnik AND       
                            Novica.Naslov LIKE '%" + naslov + "%' AND Novica.DatumSpremenil > '" + DatumOdFilter + "' AND (Uporabnik.Ime LIKE '%" + avtor + "%' OR Uporabnik.Priimek LIKE '%" + avtor + "%') ";
                }
                else if (DatumOdFilter == "" && DatumDoFilter != "")
                {
                    query = @"SELECT Novica.ID_novica, Novica.Naslov, Novica.Povzetek, Novica.Vsebina, Novica.ID_uporabnik, Novica.DatumSpremenil, Novica.URLSlika, (Uporabnik.Ime || ' ' || Uporabnik.Priimek) AS Uporabnik 
                            FROM Novica, Uporabnik 
                            WHERE Novica.ID_uporabnik = Uporabnik.ID_uporabnik AND       
                            Novica.Naslov LIKE '%" + naslov + "%' AND Novica.DatumSpremenil < '" + DatumDoFilter + "' AND (Uporabnik.Ime LIKE '%" + avtor + "%' OR Uporabnik.Priimek LIKE '%" + avtor + "%') ";
                }
                else
                {
                    query = @"SELECT Novica.ID_novica, Novica.Naslov, Novica.Povzetek, Novica.Vsebina, Novica.ID_uporabnik, Novica.DatumSpremenil, Novica.URLSlika, (Uporabnik.Ime || ' ' || Uporabnik.Priimek) AS Uporabnik
                            FROM Novica, Uporabnik 
                            WHERE Novica.ID_uporabnik = Uporabnik.ID_uporabnik AND       
                            Novica.Naslov LIKE '%" + naslov + "%' AND Novica.DatumSpremenil > '" + DatumOdFilter + "' AND Novica.DatumSpremenil < '" + DatumDoFilter + "' AND (Uporabnik.Ime LIKE '%" + avtor + "%' OR Uporabnik.Priimek LIKE '%" + avtor + "%') ";
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
                DataTableToList<NovicaGrid> dtl = new DataTableToList<NovicaGrid>();
                seznamNovic = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return seznamNovic;
        } 

        public void ShraniNovico(Novica novica)
        {
            string query = @"INSERT INTO Novica 
                             VALUES ('" + novica.ID_novica + "', '" + novica.Naslov + "', '" + novica.Povzetek + "', '" +
                                novica.Vsebina + "', '" + novica.ID_uporabnik + "', DATETIME('now'), '" +
                                novica.URLSlika + "') ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void PosodobiNovico(Novica novica)
        {
            string query = @"UPDATE Novica 
                             SET Naslov = '" + novica.Naslov + "', Povzetek = '" + novica.Povzetek + "', Vsebina = '" +
                                novica.Vsebina + "', ID_uporabnik = '" + novica.ID_uporabnik + "', DatumSpremenil = DATETIME('now'), URLSlika = '" +
                                novica.URLSlika + "' WHERE ID_novica = '" + novica.ID_novica + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public int GetNextIDNovica()
        {
            int nextIdNovica = 0;

            string query = @"SELECT MAX(ID_novica) AS Stevilo FROM Novica";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            try
            {
                nextIdNovica = (Convert.ToInt32(cmd.ExecuteScalar())) + 1;
            }
            catch (Exception)
            {
                nextIdNovica = 1;
            }

            conn.Dispose();

            return nextIdNovica;
        }

        public Novica VrniNovico(int id_novica)
        {
            Novica novica = null;

            string query = @"SELECT Novica.ID_novica, Novica.Naslov, Novica.Povzetek, Novica.Vsebina, Novica.ID_uporabnik, Novica.DatumSpremenil, Novica.URLSlika, (Uporabnik.Ime || ' ' || Uporabnik.Priimek) AS Uporabnik 
                             FROM Novica, Uporabnik 
                             WHERE Novica.ID_uporabnik = Uporabnik.ID_uporabnik AND Novica.ID_novica = '" + id_novica + "' ";

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
                novica = new Novica();

                novica.ID_novica = (int)dt.Rows[0]["ID_novica"];
                novica.Naslov = (string)dt.Rows[0]["Naslov"];
                novica.Povzetek = dt.Rows[0]["Povzetek"] != null ? (string)dt.Rows[0]["Povzetek"] : "";
                novica.Vsebina = dt.Rows[0]["Vsebina"] != null ? (string)dt.Rows[0]["Vsebina"] : "";
                novica.ID_uporabnik = dt.Rows[0]["ID_uporabnik"] != null ? (int)dt.Rows[0]["ID_uporabnik"] : 0;
                novica.DatumSpremenil = dt.Rows[0]["DatumSpremenil"] != null ? (DateTime)dt.Rows[0]["DatumSpremenil"] : System.DateTime.Now;
                novica.URLSlika = dt.Rows[0]["URLSlika"] != null ? (string)dt.Rows[0]["URLSlika"] : "";
                novica.Uporabnik = dt.Rows[0]["Uporabnik"] != null ? (string)dt.Rows[0]["Uporabnik"] : "";
            }

            return novica;
        }

        public void DeleteNovica(int id_novica)
        {
            string query = @"DELETE FROM Novica WHERE ID_novica = '" + id_novica + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void DeleteNovicaImage(int id_novica)
        {
            string query = @"UPDATE Novica SET URLSlika = '' WHERE ID_novica = '" + id_novica + "' ";

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