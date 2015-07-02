using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KKK_Zusterna.Enums
{
    public static class KKKZusternaEnum
    {
        public static readonly string SaveNovica = System.Configuration.ConfigurationManager.AppSettings["SaveNovica"];
        public static readonly string SaveGalerijaSlik = System.Configuration.ConfigurationManager.AppSettings["SaveGalerijaSlik"];
        public static readonly string SaveRezultati = System.Configuration.ConfigurationManager.AppSettings["SaveRezultati"];
        public static readonly string SaveTekmovanja = System.Configuration.ConfigurationManager.AppSettings["SaveTekmovanja"];
        public static readonly string SaveONas = System.Configuration.ConfigurationManager.AppSettings["SaveONas"];
        public static readonly string SaveNovicaPriloga = System.Configuration.ConfigurationManager.AppSettings["SaveNovicaPriloge"];
    }
}