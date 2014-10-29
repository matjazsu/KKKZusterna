using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KKK_Zusterna.Models
{
    public class Trening
    {
        #region Properties

        public int ID_Trening { get; set; }
        public string Naziv { get; set; }
        public string Spremenil { get; set; }
        public DateTime SpremenilDatum { get; set; }

        //PON

        public string Pon_Dop_Od { get; set; }
        public string Pon_Dop_Do { get; set; }
        public string Pon_Dop_Tre { get; set; }

        public string Pon_Pop_Od { get; set; }
        public string Pon_Pop_Do { get; set; }
        public string Pon_Pop_Tre { get; set; }

        //TOR

        public string Tor_Dop_Od { get; set; }
        public string Tor_Dop_Do { get; set; }
        public string Tor_Dop_Tre { get; set; }

        public string Tor_Pop_Od { get; set; }
        public string Tor_Pop_Do { get; set; }
        public string Tor_Pop_Tre { get; set; }

        //SRE

        public string Sre_Dop_Od { get; set; }
        public string Sre_Dop_Do { get; set; }
        public string Sre_Dop_Tre { get; set; }

        public string Sre_Pop_Od { get; set; }
        public string Sre_Pop_Do { get; set; }
        public string Sre_Pop_Tre { get; set; }

        //CET

        public string Cet_Dop_Od { get; set; }
        public string Cet_Dop_Do { get; set; }
        public string Cet_Dop_Tre { get; set; }

        public string Cet_Pop_Od { get; set; }
        public string Cet_Pop_Do { get; set; }
        public string Cet_Pop_Tre { get; set; }

        //PET

        public string Pet_Dop_Od { get; set; }
        public string Pet_Dop_Do { get; set; }
        public string Pet_Dop_Tre { get; set; }

        public string Pet_Pop_Od { get; set; }
        public string Pet_Pop_Do { get; set; }
        public string Pet_Pop_Tre { get; set; }

        //SOB

        public string Sob_Dop_Od { get; set; }
        public string Sob_Dop_Do { get; set; }
        public string Sob_Dop_Tre { get; set; }

        public string Sob_Pop_Od { get; set; }
        public string Sob_Pop_Do { get; set; }
        public string Sob_Pop_Tre { get; set; }

        //NED

        public string Ned_Dop_Od { get; set; }
        public string Ned_Dop_Do { get; set; }
        public string Ned_Dop_Tre { get; set; }

        public string Ned_Pop_Od { get; set; }
        public string Ned_Pop_Do { get; set; }
        public string Ned_Pop_Tre { get; set; }

        #endregion

        #region Constructor

        public Trening(){}

        #endregion
    }
}