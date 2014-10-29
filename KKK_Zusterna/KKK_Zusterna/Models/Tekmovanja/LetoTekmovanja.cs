using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KKK_Zusterna.Models
{
    public class LetoTekmovanja
    {
        #region Properties

        [Required]
        [Display(Name = "Id:")]
        public int ID_letoTekmovanja { get; set; }

        [Required]
        [Display(Name = "Leto:")]
        public string Leto { get; set; }

        [Required]
        [Display(Name = "Spremenil:")]
        public string Spremenil { get; set; }

        [Required]
        [Display(Name = "Datum spremembe:")]
        public DateTime SpremenilDatum { get; set; }

        #endregion

        #region LifeCycle

        public LetoTekmovanja() { }

        #endregion
    }
}