using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KKK_Zusterna.Models
{
    public class LetoRezultati
    {
        #region Properties

        [Required]
        [Display(Name = "Id:")]
        public int ID_letoRezultati { get; set; }

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

        public LetoRezultati(){}

        #endregion
    }
}