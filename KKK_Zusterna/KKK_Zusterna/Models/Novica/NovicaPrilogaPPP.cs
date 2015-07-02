using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using KKK_Zusterna.Helper;

namespace KKK_Zusterna.Models
{
    public class NovicaPrilogaPPP
    {
        #region ConnectionString

        private string ConnString = System.Configuration.ConfigurationManager.ConnectionStrings["KKKZusternaDB"].ConnectionString;

        #endregion

        #region Functionality

        public int GetNextIDNovicaPriloga()
        {
            int nextIdNovicaPriloga = 0;

            string query = @"SELECT MAX(ID_novicaPriloga) AS Stevilo FROM NovicaPriloga";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            try
            {
                nextIdNovicaPriloga = (Convert.ToInt32(cmd.ExecuteScalar())) + 1;
            }
            catch (Exception)
            {
                nextIdNovicaPriloga = 1;
            }

            conn.Dispose();

            return nextIdNovicaPriloga;
        }

        public void InsertNovicaPriloga(NovicaPriloga novicaPriloga)
        {
            string query = @"INSERT INTO NovicaPriloga 
                             VALUES ('" + novicaPriloga.ID_novicaPriloga + "', '" + novicaPriloga.ID_novica + "', '" + novicaPriloga.URLFile + "', '" + novicaPriloga.Naslov + "' )";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public List<NovicaPriloga> VrniPrilogeZaNovico(int ID_novica)
        {
            List<NovicaPriloga> priloge = new List<NovicaPriloga>();

            string query = "SELECT * FROM NovicaPriloga WHERE ID_novica = '" + ID_novica + "' ";

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

            //Pretvorimo dt v List<NovicaPriloga>
            if (ds != null)
            {
                DataTableToList<NovicaPriloga> dtl = new DataTableToList<NovicaPriloga>();
                priloge = dtl.FromDataTableToList(ds.Tables[0]);
            }

            return priloge;
        }

        public void DeleteNovicaPriloga(int ID_novicaPriloga)
        {
            string query = "DELETE FROM NovicaPriloga WHERE ID_novicaPriloga = '" + ID_novicaPriloga + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public void DeletePrilogeZaNovico(int ID_novica)
        {
            string query = "DELETE FROM NovicaPriloga WHERE ID_novica = '" + ID_novica + "' ";

            SQLiteConnection conn = new SQLiteConnection(ConnString);

            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public NovicaPriloga VrniNovicaPriloga(int ID_novicaPriloga)
        {
            NovicaPriloga priloga = null;

            string query = "SELECT * FROM NovicaPriloga WHERE ID_novicaPriloga = '" + ID_novicaPriloga + "' ";

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
                priloga = new NovicaPriloga();

                priloga.ID_novicaPriloga = ID_novicaPriloga;
                priloga.ID_novica = dt.Rows[0]["ID_novica"] != null ? (int)dt.Rows[0]["ID_novica"] : 0;
                priloga.URLFile = dt.Rows[0]["URLFile"] != null ? (string)dt.Rows[0]["URLFile"] : "";
                priloga.Naslov = dt.Rows[0]["Naslov"] != null ? (string)dt.Rows[0]["Naslov"] : "";
            }

            return priloga;
        }

        #endregion
    }
}