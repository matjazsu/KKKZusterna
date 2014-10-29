using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KKK_Zusterna.Models
{
    public class Povezave
    {
        #region Properties

        [Required]
        [Display(Name = "ID_Povezava:")]
        public int ID_Povezava { get; set; }

        [Required]
        [Display(Name = "ID_KategorijaPovezav:")]
        public int ID_KategorijaPovezav { get; set; }

        [Required]
        [Display(Name = "Naziv:")]
        public string Naziv { get; set; }

        [Required]
        [Display(Name = "URL:")]
        public string URL { get; set; }

        [Required]
        [Display(Name = "Spremenil")]
        public string Spremenil { get; set; }

        [Required]
        [Display(Name = "SpremenilDatum")]
        public DateTime SpremenilDatum { get; set; }
        
        #endregion

        #region LifeCycle

        public Povezave() { }

        #endregion
    }
}