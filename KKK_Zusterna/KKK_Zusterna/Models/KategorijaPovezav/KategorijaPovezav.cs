using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KKK_Zusterna.Models
{
    public class KategorijaPovezav
    {
        #region Properties

        [Required]
        [Display(Name = "ID_KategorijaPovezav:")]
        public int ID_KategorijaPovezav { get; set; }

        [Required]
        [Display(Name = "Naslov:")]
        public string Naslov { get; set; }

        [Required]
        [Display(Name = "Spremenil")]
        public string Spremenil { get; set; }

        [Required]
        [Display(Name = "SpremenilDatum")]
        public DateTime SpremenilDatum { get; set; }
        
        #endregion

        #region LifeCycle

        public KategorijaPovezav() { }

        #endregion
    }
}